using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NGM.OperationalTransformation.Models;
using NGM.SignalR;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment;
using Orchard.Security;
using Orchard.Services;

namespace NGM.OperationalTransformation.Hubs {
    //https://github.com/SignalR/SignalR/wiki/Hubs
    [HubName("contentHub")]
    public class ContentHub : Hub {
        private readonly Work<IContentManager> _workContentManager;
        private readonly Work<IAuthenticationService> _workAuthenticationService;
        private readonly IClock _clock;

        private const string ContentGroupNameFormat = "ContentItem-{0}";

        public ContentHub(Work<IContentManager> workContentManager,
            Work<IAuthenticationService> workAuthenticationService,
            IClock clock) {
            _workContentManager = workContentManager;
            _workAuthenticationService = workAuthenticationService;
            _clock = clock;
        }

        private IContentManager ContentManager {
            get { return _workContentManager.Value; }
        }

        private IAuthenticationService AuthenticationService {
            get { return _workAuthenticationService.Value; }
        }

        public override Task OnConnected() {
            return Clients.All.joined(Context.ConnectionId, _clock.UtcNow.ToString(CultureInfo.InvariantCulture));
        }

        public override Task OnDisconnected() {
            return Clients.All.leave(Context.ConnectionId, _clock.UtcNow.ToString(CultureInfo.InvariantCulture));
        }

        public override Task OnReconnected() {
            return Clients.All.rejoined(Context.ConnectionId, _clock.UtcNow.ToString(CultureInfo.InvariantCulture));
        }

        public void Join(int contentItemId) {
            Join(contentItemId, false);
        }

        public void Join(int contentItemId, bool reconnecting) {
            IUser user = AuthenticationService.GetAuthenticatedUser() ?? new UnauthenticatedUser(Context.ConnectionId);

            OnUserInitialize(contentItemId, user, reconnecting);
        }

        public void SendPatches(int contentItemId, string elementId, string elementName, int contentItemVersion, List<PatchModel> patches) {
            var patchModelToPatchMapper = new PatchModelToPatchMapper();
            var patchesTranslated = patches.Select(o => patchModelToPatchMapper.Map(o)).ToList();

            var draftContentItem = ContentManager.Get(contentItemId, VersionOptions.DraftRequired);

            ContentManager.UpdateEditor(draftContentItem, new HubUpdateModel(elementName, patchesTranslated));

            // TODO: This needs to be an options ('Auto Publish Item', but also allow people to osee if a draft is currently availible)
            ContentManager.Publish(draftContentItem);

            Clients.Caller.changeAcknowledged(draftContentItem.Version);

            Clients.OthersInGroup(ContentGroupName(contentItemId)).applyPatches(elementId, contentItemVersion, patches);
        }

        private void OnUserInitialize(int contentItemId, IUser user, bool reconnecting) {
            if (!reconnecting) {
                Clients.Caller.id = user.Id;
                Clients.Caller.username = user.UserName;
                Clients.Caller.activeContent = contentItemId;
            }

            var content = ContentManager.Get(contentItemId);

            var isOwner = content.As<ICommonPart>().Owner == user;
            var userViewModel = new UserViewModel(user);
            var contentGroupName = ContentGroupName(contentItemId);

            // Tell the people who are editing this content that you are viewing it and cmaybe editing it.
            Clients.Group(contentGroupName).addUser(userViewModel, contentGroupName, isOwner).Wait();
            // Add the caller to the group so they receive content updates
            Groups.Add(Context.ConnectionId, contentGroupName);
        }

        private static string ContentGroupName(int contentItemId) {
            return string.Format(ContentGroupNameFormat, contentItemId);
        }

        //public void Update(int contentItemId, string contentPartName, string newValue) {
        //    if (_orchardServices.WorkContext.CurrentUser == null) {
        //        return;
        //    }

        //    var currentuser = _orchardServices.WorkContext.CurrentUser;

        //    // Call the broadcastMessage method to update clients.
        //    var contentItem = _contentManager.Get(contentItemId, VersionOptions.DraftRequired);

        //    if (contentItem == null)
        //        contentItem = _contentManager.Create("Page", VersionOptions.DraftRequired);

        //    Clients.All.addMessage(contentItem.Id);

        //    //dynamic model = _contentManager.UpdateEditor(contentItem, this);

        //    //_contentManager.Publish(contentItem);
        //}
    }

    public class UserViewModel {
        public UserViewModel(IUser user) {
            UserName = user.UserName;
        }

        public string UserName { get; private set; }
    }

    public class UnauthenticatedUser : IUser {
        public UnauthenticatedUser(string conntentionId) {
            ConntentionId = conntentionId;
        }

        public string ConntentionId { get; private set; }

        public ContentItem ContentItem { get; private set; }
        public int Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
    }

}