// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the Login default page Index

var App = App || {};

App.ui = (function ($) {
    'use strict';

    // jui callback
    // open dialog
    function openForgotPassword() {
        $('#forgot-password-dialog').dialog("open");
        return false;
    }

    function sendEmail() {
        var email = $('#forgot-password-email').val(),
            sstuff;
        // TODO: validate
        sstuff = JSON.stringify({ email: email });
        App.postit("/Operation/Misc/LoginResetPassword", {
            data: sstuff,
            success: function (message) {
                alert(message);
                $('#forgot-password-dialog').dialog("close");
            }
        });
    }

    function initialize() {
        // create forgotten password dialog
        $('#forgot-password-dialog').dialog({
            autoOpen: false,
            width: 500,
            height: 250,
            modal: true,
            buttons: {
                "Send Email": sendEmail,
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });

        // buttons
        $('#forgot-password')
            .button()
            .click(openForgotPassword);

        // IE doesn't handle button value properly so we specifically set it
        $("#login-guest")
            .button()
            .click(function () {
                $("#login-type").val("guest");
                return true;
            });

        $("#login-administrator")
            .button({ icons: { primary: "ui-icon-locked"} })
            .click(function () {
                $("#login-type").val("administrator");
                return true;
            });
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);