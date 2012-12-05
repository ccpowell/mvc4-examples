// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the RTP view Projects.aspx 

var App = App || {};

App.ui = (function ($) {
    "use strict";

    var activeCycleId;

    function setActiveCycle() {
    }

    function showAmendableProjects() {
    }

    function getAmendableProjects() {
    }

    function amendProjects() {
        var stuff = {
            Name: "hoover",
            Ints: [1, 2, 3]
        },
            sstuff = JSON.stringify(stuff, null, 2);

        $.ajax('/Trips4/operation/misc/PostStuff', {
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: sstuff,
            success: function (data) {
                alert("Okay ");
            },
            error: function () {
                alert("bummer");
            }
        });
    }

    function initialize() {
        $("#amend-projects")
            .button()
            .click(amendProjects);
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);