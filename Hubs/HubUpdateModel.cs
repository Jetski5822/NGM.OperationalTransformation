using System.Collections.Generic;
using NGM.OperationalTransformation.Differential.DiffMatchPatch;
using Orchard.ContentManagement;
using Orchard.Localization;

namespace NGM.OperationalTransformation.Hubs {
    public class HubUpdateModel : IUpdateModel {
        private readonly string _elementName;

        private readonly string _prefix;
        private readonly string _elementWithoutPrefix;

        private readonly List<Patch> _patches;

        public HubUpdateModel(string elementName, List<Patch> patches) {
            _elementName = elementName.Replace(".", "_");

            _prefix = _elementName.Substring(0, _elementName.IndexOf('_'));
            _elementWithoutPrefix = _elementName.Substring(_elementName.IndexOf('_') + 1, _elementName.Length - (_elementName.IndexOf('_') + 1));

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