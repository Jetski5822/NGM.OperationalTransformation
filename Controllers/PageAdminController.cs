using System.Web.Mvc;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Settings;
using Orchard.UI.Admin;

namespace NGM.OperationalTransformation.Controllers {
    [Admin]
    public class PageAdminController : Controller
    {
        private readonly IOrchardServices _orchardServices;
        private readonly ISiteService _siteService;

        public PageAdminController(IOrchardServices orchardServices, 
            ISiteService siteService,
            IShapeFactory shapeFactory) {
            _orchardServices = orchardServices;
            _siteService = siteService;

            Shape = shapeFactory;
            Logger = NullLogger.Instance;
            T = NullLocalizer.Instance;
        }

        dynamic Shape { get; set; }
        protected ILogger Logger { get; set; }
        public Localizer T { get; set; }

        public ActionResult Item() {
            var page = _orchardServices.ContentManager.New("Page");
            if (page == null)
                return HttpNotFound();

            dynamic model = _orchardServices.ContentManager.BuildEditor(page);
            return View((object)model);
        }
    }
}