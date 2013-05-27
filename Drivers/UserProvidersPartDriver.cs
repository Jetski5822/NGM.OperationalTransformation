using JetBrains.Annotations;
using NGM.OperationalTransformation.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement;

namespace NGM.OperationalTransformation.Drivers {
    [UsedImplicitly]
    public class UserProvidersPartDriver : ContentPartDriver<ContentPadPart> {
        private const string TemplateName = "Parts/Content.Pad";

        protected override string Prefix {
            get {
                return "ContentPadPart";
            }
        }

        protected override DriverResult Editor(ContentPadPart contentPadPart, dynamic shapeHelper) {
            if (contentPadPart.Id == 0) return null;

            return ContentShape("Parts_ContentPad_Edit", () => {
                        return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: contentPadPart, Prefix: Prefix);
                    });
        }

        protected override DriverResult Editor(ContentPadPart userProvidersPart, IUpdateModel updater, dynamic shapeHelper) {
            return Editor(userProvidersPart, shapeHelper);
        }
    }
}