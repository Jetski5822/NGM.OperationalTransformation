(function($, window) {
    "use strict";



    var ui = {
        events: {
            textchange: 'contentpad.ui.textchange'
        },

        initialize: function () {
            var $ui = $(this);
            
            $(":input").each(function (index) {
                $(this).data('pre', $(this).val());
            });

            var timeout;
            $(":input").on('textchange', function(event) {
                clearTimeout(timeout);

                var self = this;
                
                timeout = setTimeout(function() {
                    $ui.trigger(ui.events.textchange, self);
                }, 250);
            });
        }
    };

    if (!window.contentpad) {
        window.contentpad = {};
    }

    window.contentpad.ui = ui;
})(jQuery, window);