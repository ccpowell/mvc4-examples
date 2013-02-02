/// <reference path="jquery-1.9.0-vsdoc.js" />
/// <reference path="jquery-ui-1.10.0.js" />

// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, App: false */

App.ui = (function ($) {
    "use strict";

    function initialize() {
        var activeIndex = 0;

        switch (App.action) {
            case "testpage1": activeIndex = 0; break;
            case "testpage2": activeIndex = 1; break;
            case "testpage3": activeIndex = 2; break;
            case "testpage4": activeIndex = 3; break;
            case "testpage5": activeIndex = 4; break;
            case "testpage6": activeIndex = 5; break;
        }
        $("#tabs").tabs({
            active: activeIndex,
            beforeActivate: function (event, ui) {
                var action = ui.newTab.data("action");
                if (action) {
                    window.location.assign("/Contacts/Home/" + action);
                }
            }
        });
    }


    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);
