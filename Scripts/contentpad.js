/// <reference path="contentpad.ui.js" />
/// <reference path="contentpad.patch.js" />

(function ($, connection, window, ui, patch) {
    "use strict";

    var pad = connection.contentHub,
        $ui = $(ui),
        contentitemid = null,
        contentitemversion = null,
        isInApply = false;

    $ui.bind(ui.events.textchange, function (event, self) {
        if (isInApply)
            return;
        
        // UI Concern....
        var previousText = $(self).data('pre');
        var newText = $(self).val();
        $(self).data('pre', newText);

        var patches = patch.calculatepatch(previousText, newText);

        if (patches == null) {
            console.debug('No patch to send: New text: "' + newText + '", Previous Text: "' + previousText + '"');
            return;
        }

        console.debug('Patch to send: ' + patches.toString());
        
        pad.server.sendPatches(contentitemid, $(self).attr("id"), contentitemversion, patches);
    });
    
    pad.client.changeAcknowledged = function (updatedContentItemVersion) {
        //var value = patchversionstobeapplied.shift();
        this.contentitemversion = updatedContentItemVersion;
    };

    pad.client.applyPatches = function (elementId, patchedContentItemVersion, patches) {
        isInApply = true;
        
        var self = $('#' + elementId);
        var currentOnScreenValue = self.val();

        var patchedText = patch.applypatch(patches, currentOnScreenValue);

        $(self).val(patchedText);
        $(self).data('pre', patchedText);
        
        isInApply = false;
    };

    $(function () {
        console.debug("UI Initializing...");
        ui.initialize();
        console.debug("UI Initialized!");

        $.contenthub = {};

        $.contenthub.initConnection = function (contentItemId, contentItemVersion) {
            console.debug("Connection Initializing...");
            contentitemid = contentItemId;
            contentitemversion = contentItemVersion;

            connection.hub.start(function() {
                pad.server.join(contentItemId)
                    .fail(function (e) {
                        console.debug("Connection failed to Initialize!");
                    })
                    .done(function() {
                        console.debug("Connection Initialized!");
                    });
            });

            pad.client.addUser = function(user, groupName, isOwner) {
            };
        };
    });
})(jQuery, $.connection, window, window.contentpad.ui, window.contentpad.patch);
