using System.Collections.Generic;
using NGM.ContentPad.Differential.DiffMatchPatch;
using Orchard.ContentManagement;
using Orchard.Localization;

namespace NGM.ContentPad.Hubs {
    public class HubUpdateModel : IUpdateModel {
        private readonly string _elementId;

        private readonly string _prefix;
        private readonly string _elementWithoutPrefix;

        private readonly List<Patch> _patches;

        public HubUpdateModel(string elementId, List<Patch> patches) {
            _elementId = elementId;

            _prefix = elementId.Substring(0, elementId.IndexOf('_'));
            _elementWithoutPrefix = elementId.Substring(elementId.IndexOf('_') + 1, elementId.Length - (elementId.IndexOf('_') + 1));

            _patches = patches;
        }

        public bool TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties) where TModel : class {
            if (!prefix.Equals(_prefix))
                return false;

            var currentValue = model.GetType().GetProperty(_elementWithoutPrefix).GetValue(model, null) as string;

            var diffMatchPatch = new diff_match_patch();
            var obj = diffMatchPatch.patch_apply(_patches, currentValue);
            
            model.GetType().GetProperty(_elementWithoutPrefix).SetValue(model, obj[0], null);

            return true;
        }

        public void AddModelError(string key, LocalizedString errorMessage) {
            
        }
    }
}