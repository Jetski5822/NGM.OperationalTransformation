//using System.Web.Routing;
//using NGM.OperationalTransformation.Models;
//using Orchard;
//using Orchard.ContentManagement;
//using Orchard.Core.Contents;
//using Orchard.DisplayManagement.Implementation;
//using Orchard.DisplayManagement.Shapes;
//using Orchard.Security;
//using Orchard.UI.Admin;

//namespace NGM.OperationalTransformation {
//    public class OTShapeFactory : IShapeDisplayEvents, IShapeFactoryEvents {
//        private readonly IAuthorizer _authorizer;
//        private readonly WorkContext _workContext;

//        public OTShapeFactory(IAuthorizer authorizer, IWorkContextAccessor workContextAccessor) {
//            _authorizer = authorizer;
//            _workContext = workContextAccessor.GetContext();
//        }

//        private bool IsActivable(IContent content) {
//            // activate on front-end only
//            if (AdminFilter.IsApplied(new RequestContext(_workContext.HttpContext, new RouteData())))
//                return false;

//            if (!_authorizer.Authorize(Permissions.EditContent, content))
//                return false;

//            var part = content.As<ContentPadPart>();

//            if (part == null || part.Id == 0)
//                return false;

//            return true;
//        }

//        public void Displaying(ShapeDisplayingContext context) {
//            var shape = context.Shape;
//            var content = shape.ContentItem as IContent;
//            if (content == null)
//                return;

//            if (!IsActivable(content)) {
//                return;
//            }

//            var shapeMetadata = (ShapeMetadata) context.Shape.Metadata;

//            if (shapeMetadata.Wrappers.Contains("ContentPadWrapper"))
//                return;

//            if (shapeMetadata.Type != "Widget") {

//            }
//        }

//        public void Displayed(ShapeDisplayedContext context) {
//        }

//        public void Creating(ShapeCreatingContext context) {
            
//        }

//        public void Created(ShapeCreatedContext context) {

//        }
//    }
//}