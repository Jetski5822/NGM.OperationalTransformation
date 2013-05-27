using Orchard.UI.Resources;

namespace NGM.OperationalTransformation {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var resourceManifest = builder.Add();
            resourceManifest.DefineScript("diff-match-patch").SetUrl("diff_match_patch.js", "diff_match_patch_uncompressed.js").SetDependencies("jQuery");
            resourceManifest.DefineScript("jQuery_Text_Change_Event").SetUrl("jquery.textchange.min.js").SetDependencies("jQuery");
            
            resourceManifest.DefineScript("ContentPadPatch").SetUrl("contentpad.patch.js").SetDependencies("diff-match-patch");
            resourceManifest.DefineScript("ContentPadUI").SetUrl("contentpad.ui.js").SetDependencies("jQuery_Text_Change_Event");

            resourceManifest.DefineScript("ContentPadInlineEditUI").SetUrl("contentpad.inlineedit.ui.js").SetDependencies("ContentPadUI");

            resourceManifest.DefineScript("ContentPad").SetUrl("contentpad.js").SetDependencies("ContentPadPatch", "ContentPadUI", "ContentPadInlineEditUI", "jQuery_SignalR_Hubs");
        }
    }
}