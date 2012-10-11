/* Copyright (c) 2010 Daniel Tucker
 * Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
 * and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
 *
 * Version: 1.0.0
 * Written with jQuery 1.4.2
 */
(function($) {
    $.fn.autoHide = function(options) {

        var defaults = {
            wait: 5000,
            removeClass: null,
            resizeColorbox: false
        };
        var options = $.extend(defaults, options);

        return this.each(function() {
            var obj = $(this);
            obj.show();
            setTimeout(function() {
                obj.fadeOut("slow", function() {
                    if (options.removeClass != null) {
                        obj.removeClass(options.removeClass);
                    } else obj.removeClass();
                    if (options.resizeColorbox) {
                        $.fn.colorbox.resize();
                    }
                    //obj.removeClass();//.removeAttr('style');
                });
            }, options.wait);

        });
    };
})(jQuery);
