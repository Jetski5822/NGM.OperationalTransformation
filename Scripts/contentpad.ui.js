(function($, window) {
    "use strict";



    var ui = {
        events: {
            textchange: 'contentpad.ui.textchange',
            patchesapplying: 'contentpad.ui.patchesapplying',
            patchesapplied: 'contentpad.ui.patchesapplied'
        },

        initialize: function () {
            var $ui = $(this);
            
            $(":input").each(function (index) {
                $(this).data('pre', $(this).val());
            });

            var timeout;
            $(":input").on('textchange', function (event) {                
                clearTimeout(timeout);

                var self = this;
                
                timeout = setTimeout(function() {
                    $ui.trigger(ui.events.textchange, self);
                }, 250);
            });
        },
        
        applypatch: function (element, patchedText) {
            var $ui = $(this);
            
            $ui.trigger(ui.events.patchesapplying);
            
            $(element).val(patchedText);
            $(element).data('pre', patchedText);
            
            $ui.trigger(ui.events.patchesapplied);
        }
    };

    if (!window.contentpad) {
        window.contentpad = {};
    }

    window.contentpad.ui = ui;
})(jQuery, window);