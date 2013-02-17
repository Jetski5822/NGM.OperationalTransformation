using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR.Hubs;
using NGM.ContentPad.Models;
using NGM.SignalR;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Environment;
using Orchard.Security;

namespace NGM.ContentPad.Hubs {
    //https://github.com/SignalR/SignalR/wiki/Hubs
    [HubName("contentHub")]
    public class ContentHub : OrchardHub {
        private readonly Work<IContentManager> _workContentManager;
        private readonly Work<IAuthenticationService> _workAuthenticationService;

        private const string ContentGroupNameFormat = "ContentItem-{0}";

        public ContentHub(Work<IContentManager> workContentManager,
            Work<IAuthenticationService> workAuthenticationService) {
            _workContentManager = workContentManager;
            _workAuthenticationService = workAuthenticationService;
        }

        private IContentManager ContentManager {
            get { return _workContentManager.Value; }
        }

        private IAuthenticationService AuthenticationService {
            get { return _workAuthenticationService.Value; }
        }

        public void Join(int contentItemId) {
            var user = AuthenticationService.GetAuthenticatedUser();
            var content = ContentManager.Get(contentItemId);

            Clients.Caller.id = user.Id;
            Clients.Caller.username = user.UserName;
            Clients.Caller.activeContent = contentItemId;

            var isUserOwner = content.As<ICommonPart>().Owner == user;
            var userViewModel = new UserViewModel(user);
            var contentGroupName = ContentGroupName(contentItemId);

            // Tell the people who are editing this content that you are viewing it and cmaybe editing it.
            Clients.Group(contentGroupName).addUser(userViewModel, contentGroupName, isUserOwner).Wait();

            // Add the caller to the group so they receive content updates
            Groups.Add(Context.ConnectionId, contentGroupName);
        }

        public void SendPatches(int contentItemId, string elementId, int contentItemVersion, List<PatchModel> patches) {
            var patchModelToPatchMapper = new PatchModelToPatchMapper();
            var patchesTranslated = patches.Select(o => patchModelToPatchMapper.Map(o)).ToList();

            var draftContentItem = ContentManager.Get(contentItemId, VersionOptions.DraftRequired);

            ContentManager.UpdateEditor(draftContentItem, new HubUpdateModel(elementId, patchesTranslated));

            Clients.Caller.changeAcknowledged(draftContentItem.Version);

            Clients.OthersInGroup(ContentGroupName(contentItemId)).applyPatches(elementId, contentItemVersion, patches);
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
}