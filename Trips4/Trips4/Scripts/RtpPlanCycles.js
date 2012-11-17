// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the RTP view PlanCycles.aspx 

var App = App || {};

App.ui = (function ($) {
    "use strict";

    var $dlgAddCycle;

    // Accept the Add New Cycle dialog
    function acceptCycle() {

        if (!$("form", $dlgAddCycle).valid()) {
            alert("Please enter all required fields");
            return false;
        }

        $.ajax("/Trips4/RTP/someyear/CreateCycle", {
            type: "POST",
            data: { cycle: $("#cycle-Name").val() },
            success: function (data, textStatus, jqXHR) {
                alert(data.message);
                if (data.error === "false") {
                    window.location.reload();
                }
            }
        });

        return false;
    }

    // Cancel the Add New Cycle dialog
    function cancelCycle() {
        $dlgAddCycle.dialog("close");
        return false;
    }

    // Show the Add New Cycle dialog
    function addCycle() {
        $dlgAddCycle.dialog("open").find("form").validate().resetForm();
    }

    // Initialize the Add New Cycle dialog
    function initializeAddCycle() {
        $dlgAddCycle = $("#cycle-dialog");
        $dlgAddCycle.dialog({
            autoOpen: false,
            resizable: true,
            modal: true,
            buttons: {
                Accept: acceptCycle,
                Cancel: cancelCycle
            },
            width: 'auto'
        });

        // for validation, the input fields require names
        $("form input", $dlgAddCycle).each(function (index, item) {
            $(item).attr("name", item.id);
        });
    }

    function initialize() {
        $('#cyclesGrid').dataTable({
            "bPaginate": true,
            "bLengthChange": false,
            "bFilter": true,
            "bSort": false,
            "bInfo": false,
            "bAutoWidth": true,
            "iDisplayLength": 10,
            "aoColumns": [{ "bVisible": false }, null, null, null]
        }).show();

        initializeAddCycle();

        $("#addCycle")
            .button()
            .click(addCycle);
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);