// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the RTP view Projects.aspx

var App = App || {};

App.pp = {
    CurrentCycleId: 19,
    PreviousCycleId: 18,
    NextCycleId: 0,
    RtpPlanYear: "2035-S",
    RtpPlanYearId: 78,
    CurrentCycleName: "2011-1",
    NextCycleName: ""
};


App.ui = (function ($) {
    "use strict";

    function setActiveCycle() {
        var stuff = {
            Name: "hoover",
            Ints: [1, 2, 3]
        },
            sstuff = JSON.stringify(stuff, null, 2);

        $.ajax(App.env.applicationPath + '/operation/misc/PostStuff', {
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

    // ajax callback 
    // fill in #amend-availableProjects
    function showAmendableProjects(data) {
        var options = "";
        $.each(data, function (index, el) {
            options += '<option value="' + el.Value + '">' + el.Text + '</option>';
        });
        $("#amend-availableProjects").html(options);
    }

    function getAmendableProjects() {
        var stuff = {
            cycleId: App.pp.CurrentCycleId,
            rtpPlanYearId: App.pp.RtpPlanYearId
        },
            sstuff = JSON.stringify(stuff, null, 2);

        $.ajax(App.env.applicationPath + '/operation/misc/RtpGetAmendableProjects', {
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: sstuff,
            success: showAmendableProjects,
            error: function () {
                alert("bummer");
            }
        });
    }

    function amendProjects() {
        getAmendableProjects();
        $("#dialog-amend-project").dialog("open");
    }

    function initialize() {
        $("#dialog-amend-project").dialog({
            autoOpen: false,
            width: 900,
            height: 600,
            modal: true,
            buttons: {
                "Amend": function () {
                    alert("amending things");
                },
                "Close": function () {
                    $(this).dialog("close");
                }
            }
        });

        $("#amend-projects")
            .button()
            .click(amendProjects);
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);