using Orchard.UI.Resources;

namespace NGM.OperationalTransformation {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var resourceManifest = builder.Add();
            resourceManifest.DefineScript("jQuery_SignalR").SetUrl("jquery.signalR.min.js", "jquery.signalR.js").SetVersion("1.0.0-rc2").SetDependencies("jQuery");
            resourceManifest.DefineScript("jQuery_SignalR_Hubs").SetUrl("~/signalr/hubs").SetDependencies("jQuery_SignalR");

            resourceManifest.DefineScript("diff-match-patch").SetUrl("diff_match_patch.js", "diff_match_patch_uncompressed.js").SetDependencies("jQuery");
            resourceManifest.DefineScript("jQuery_Text_Change_Event").SetUrl("jquery.textchange.min.js").SetDependencies("jQuery");
            
            resourceManifest.DefineScript("ContentPadPatch").SetUrl("contentpad.patch.js").SetDependencies("diff-match-patch");
            resourceManifest.DefineScript("ContentPadUI").SetUrl("contentpad.ui.js").SetDependencies("jQuery_Text_Change_Event");

            resourceManifest.DefineScript("ContentPad").SetUrl("contentpad.js").SetDependencies("ContentPadPatch", "ContentPadUI", "jQuery_SignalR_Hubs");
        }
    }
}