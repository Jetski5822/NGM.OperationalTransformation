(function ($, window) {
    "use strict";

    var dmp = new diff_match_patch();

    var patch = {
        calculatepatch: function (previousText, newText) {
            var diff = dmp.diff_main(previousText, newText, true);
            if (diff.length > 2) {
                dmp.diff_cleanupSemantic(diff);
            }
            return dmp.patch_make(previousText, newText, diff);
        },

        applypatch: function (patches, currentvalue) {
            console.log('Patch to apply: ' + dmp.patch_deepCopy(patches).toString());
            
            return dmp.patch_apply(patches, currentvalue)[0];
        },
    };

    if (!window.contentpad) {
        window.contentpad = {};
    }

    window.contentpad.patch = patch;
})(jQuery, window);