// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This script defines some common functions

var App = App || {};

App.utility = (function ($) {
    "use strict";

    // Prevent accidental navigation away
    function bindInputToConfirmUnload(form, button, result) {
        var $form = $(form),
            $button = $(button),
            $result = $(result);

        function setConfirmUnload() {
            $button.button("enable");
            $result.empty().hide();
            window.onbeforeunload = function unloadMessage() {
                return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
            }
        }

        $(':input', $form)
            .bind("change", setConfirmUnload)
            .bind("keyup", setConfirmUnload);
        $(':input.nobind', $form)
            .unbind("change")
            .unbind("keyup");

        //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
        $(form).validate({
            submitHandler: function (form) {
                $button.button("disable");
                $form.ajaxSubmit({
                    success: function (response) {
                        $result.html(response.message).addClass("success").removeClass("error").show();
                        window.onbeforeunload = null;
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $result.text(errorThrown).addClass('error').removeClass("success").show();
                        $button.button("enable");
                    },
                    dataType: 'json'
                });
            }
        });
    }

    return {
        bindInputToConfirmUnload: bindInputToConfirmUnload
    };
} (jQuery));
