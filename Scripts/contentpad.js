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

        if (patches == null || patches.length == 0) {
            console.log('No patch to send: New text: "' + newText + '", Previous Text: "' + previousText + '"');
            return;
        }

        console.log('Patch to send: ' + patches.toString());
        
        pad.server.sendPatches(contentitemid, $(self).attr("id"), $(self).attr("name"), contentitemversion, patches);
    });
    
    pad.client.changeAcknowledged = function (updatedContentItemVersion) {
        var $contentPad = $(contentPad);
        $contentPad.trigger(contentPad.events.changeAcknowledged, this.contentitemid);
        
        this.contentitemversion = updatedContentItemVersion;
    };

    pad.client.applyPatches = function (elementId, patchedContentItemVersion, patches) {
        isInApply = true;

        var self = $('#' + elementId);
        var currentOnScreenValue = self.val();

        var patchedText = patch.applypatch(patches, currentOnScreenValue);

        ui.applypatch(self, patchedText);
        
        isInApply = false;
    };

    $(function () {
        var initial = true;

        console.log("UI Initializing...");
        ui.initialize();
        console.log("UI Initialized!");

        $.contenthub = {};

        $.contenthub.initConnection = function (contentItemId, contentItemVersion) {
            console.log("Connection Initializing...");
            contentitemid = contentItemId;
            contentitemversion = contentItemVersion;

            connection.hub.start(function() {
                pad.server.join(contentItemId)
                    .fail(function (e) {
                        console.log(e);
                        console.log("Connection failed to Initialize!");
                    })
                    .done(function() {
                        console.log("Connection Initialized!");
                    });
            });

            connection.hub.stateChanged(function (change) {
                if (change.newState === $.connection.connectionState.reconnecting) {
                    
                }
                else if (change.newState === $.connection.connectionState.connected) {
                    if (!initial) {
                        
                        
                    } else {
                        
                    }

                    initial = false;
                }
            });

            connection.hub.disconnected(function () {
                connection.hub.log('Dropped the connection from the server. Restarting in 5 seconds.');

                // Restart the connection
                setTimeout(function () {
                    connection.hub.start(options)
                                  .done(function () {
                                      // Turn the firehose back on
                                      pad.server.join(contentItemId, true).fail(function (e) {
                                      });
                                  });
                }, 5000);
            });

            connection.hub.error(function (err) {
                // Make all pending messages failed if there's an error
            });

            pad.client.addUser = function(user, groupName, isOwner) {
            };
        };
    });
    
    var contentPad = {
        events: {
            changeAcknowledged: 'contentpad.changeAcknowledged'
        }
    };

    window.contentpad = contentPad;
    
})(jQuery, $.connection, window, window.contentpad.ui, window.contentpad.patch);
