// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the RTP view PlanCycles.aspx 

var App = App || {};

App.ui = (function ($) {
    "use strict";

    var $dlgAddCycle, editPlanCycleId;

    function setEditPlanCycle(data) {
        editPlanCycleId = data.Id || 0;
        $("#cycle-Name").val(data.Name || "");
        $("#cycle-Description").val(data.Description || "");
    }

    function getEditPlanCycle() {
        return {
            Name: $("#cycle-Name").val(),
            Description: $("#cycle-Description").val(),
            RtpYearId: $("#RtpYearId").val(),
            Id: editPlanCycleId
        };
    }

    // Accept the New Cycle dialog
    function acceptCycle() {
        var pc = getEditPlanCycle(),
            type = "PUT";

        if (!$("form", $dlgAddCycle).valid()) {
            alert("Please enter all required fields");
            return false;
        }

        // POST to create, PUT to update
        if (!pc.Id) {
            type = "POST";
        }

        $.ajax(App.env.applicationPath + "/api/RtpPlanCycle", {
            type: type,
            data: pc,
            success: function (data, textStatus, jqXHR) {
                window.location.reload();
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
        setEditPlanCycle({});
        $dlgAddCycle.dialog("open")
            .dialog("option", { title: "Add New Plan Cycle" })
            .find("form").validate().resetForm();
        return false;
    }

    // Edit the cycle just fetched from the server
    function editCycle(cycle) {
        $dlgAddCycle.dialog("open")
            .dialog("option", { title: "Edit Plan Cycle" })
            .find("form").validate().resetForm();
        setEditPlanCycle(cycle);
        return false;
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

    // initialize the table displaying the Plan cycles
    // stash the IDs from the hidden first column
    // The order is predetermined by Status and cannot be changed.
    function initializeCycleTable() {
        // remove empty row from toolkit
        if (($('#cyclesGrid tbody tr').length == 1) && ($('#cyclesGrid tbody tr td').length < 2)) {
            alert("no Plan Cycles");
            return;
        }

        $('#cyclesGrid').dataTable({
            "bPaginate": true,
            "bLengthChange": false,
            "bFilter": true,
            "bSort": false,
            "bInfo": false,
            "bAutoWidth": true,
            "iDisplayLength": 10,
            "aoColumns": [{ "bVisible": false }, { sClass: "pointer" }, null, null],
            bJQueryUI: true,
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                var $row = $(nRow);
                // attach ID to row for callbacks to use
                $row.data("id", aData[0]);

                // add class to row depending on status?
                if (aData[2] === "New") {
                    $row.addClass("plancycle-row-new");
                } else {
                    $row.removeClass("plancycle-row-new");
                }
            }
        }).show();

        // bind click on the first column of the row
        $('#cyclesGrid tbody').on("click", "tr td:first-child", function (e) {
            var id = $(this).closest("tr").data("id");
            $.getJSON(App.env.applicationPath + "/api/RtpPlanCycle", { id: id }, editCycle);
            return false; // stop propagation and default behavior
        });
    }

    function initialize() {
        // initialize table 
        initializeCycleTable();

        // initialize Add/Edit Plan Cycle dialog
        initializeAddCycle();

        // initialize button for Add Plan Cycle (if present)
        $("#addCycle")
            .button()
            .click(addCycle);
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);