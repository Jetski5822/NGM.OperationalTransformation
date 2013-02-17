//using System.Web.Routing;
//using Orchard;
//using Orchard.ContentManagement;
//using Orchard.Core.Contents;
//using Orchard.DisplayManagement.Implementation;
//using Orchard.Security;
//using Orchard.UI.Admin;

//namespace NGM.ContentPad {
//    public class Shapes : ShapeDisplayEvents {
//        private readonly IAuthorizer _authorizer;
//        private readonly IWorkContextAccessor _workContext;

//        public Shapes(IAuthorizer authorizer, IWorkContextAccessor workContext) {
//            _authorizer = authorizer;
//            _workContext = workContext;
//        }

//        public override void Displaying(ShapeDisplayingContext context) {
//            context.ShapeMetadata.OnDisplaying(displaying => {

//                if (AdminFilter.IsApplied(new RequestContext(_workContext.GetContext().HttpContext, new RouteData())))
//                    return;

//                if (!_authorizer.Authorize(Permissions.EditContent))
//                    return;

//                if (displaying.ShapeMetadata.Type == "Widget")
//                    return;

//                if (displaying.ShapeMetadata.Type == "EditorTemplate")
//                    return;

//                ContentPart contentPart = displaying.Shape.ContentPart;
//                ContentField contentField = displaying.Shape.ContentField;

//                if (contentField != null) {
//                    displaying.ShapeMetadata.Wrappers.Add("ContentField_Wrapper");
//                }
//                else if (contentPart != null) {
//                    displaying.ShapeMetadata.Wrappers.Add("ContentPart_Wrapper");
//                }
//            });
//        }
//    }
//}