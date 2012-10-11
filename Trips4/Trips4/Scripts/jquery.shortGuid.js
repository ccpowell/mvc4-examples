/**
 * jQuery.fn.sort
 * --------------
 * @author James Padolsey (http://james.padolsey.com)
 * @version 0.1
 * @updated 18-MAR-2010
 * --------------
 * @param Function comparator:
 *   Exactly the same behaviour as [1,2,3].sort(comparator)
 *   
 * @param Function getSortable
 *   A function that should return the element that is
 *   to be sorted. The comparator will run on the
 *   current collection, but you may want the actual
 *   resulting sort to occur on a parent or another
 *   associated element.
 *   
 *   E.g. $('td').sort(comparator, function(){
 *      return this.parentNode; 
 *   })
 *   
 *   The <td>'s parent (<tr>) will be sorted instead
 *   of the <td> itself.
 */
(function ($) {



    var methods = {

        init: function (options) {
        },
        decode: function () {
            alert(this.val());
            var input = this.val().replace("_", "/").replace("-", "+") + "==";

            var pads = 0,
                i,
                b10,
                imax = input.length,
                x = [];

            s = String(input);

            if (imax === 0) {
                return s;
            }

            if (imax % 4 !== 0) {
                throw "Cannot decode base64";
            }

            if (s.charAt(imax - 1) === methods._PADCHAR) {
                pads = 1;

                if (s.charAt(imax - 2) === methods._PADCHAR) {
                    pads = 2;
                }

                // either way, we want to ignore this last block
                imax -= 4;
            }

            for (i = 0; i < imax; i += 4) {
                b10 = (methods._getbyte64(s, i) << 18) | (methods._getbyte64(s, i + 1) << 12) | (methods._getbyte64(s, i + 2) << 6) | methods._getbyte64(s, i + 3);
                x.push(String.fromCharCode(b10 >> 16, (b10 >> 8) & 0xff, b10 & 0xff));
            }

            switch (pads) {
                case 1:
                    b10 = (methods._getbyte64(s, i) << 18) | (methods._getbyte64(s, i + 1) << 12) | (methods._getbyte64(s, i + 2) << 6);
                    x.push(String.fromCharCode(b10 >> 16, (b10 >> 8) & 0xff));
                    break;

                case 2:
                    b10 = (methods._getbyte64(s, i) << 18) | (methods._getbyte64(s, i + 1) << 12);
                    x.push(String.fromCharCode(b10 >> 16));
                    break;
            }

            return x.join("");

        },
        encode: function () {
            // GOOD
        },

        _PADCHAR: '=',
        _ALPHA: 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/',
        _VERSION: '1.0',

        // private method for UTF-8 encoding
        _getbyte64: function (s, i) {
            var idx = methods._ALPHA.indexOf(s.charAt(i));

            if (idx === -1) {
                throw "Cannot decode base64";
            }

            return idx;
        }
    };

    $.fn.shortGuid = function (method) {

        // Method calling logic
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery.shortGuid');
        }

    };

})(jQuery);