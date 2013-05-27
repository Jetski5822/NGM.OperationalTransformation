(function ($, window, ui, inlineeditui) {
    "use strict";

    var $ui = $(ui),
        $inlineeditui = $(inlineeditui);

    $inlineeditui.bind(inlineeditui.events.editorPrepared, function (event, self) {
        ui.initialize();
    });

    $ui.bind(ui.events.patchesapplied, function () {
        $inlineeditui.trigger(inlineeditui.events.notifyEditorChanged);
    });

})(jQuery, window, window.contentpad.ui, window.orchard.inlineedit.ui);