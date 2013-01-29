// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the TIP Project Amendments page
// N.B. this is the list of amendments for a single project, not for the whole TIP year.

var App = App || {};

function confirmDelete() {
    'use strict';
    return (confirm('Are you sure you want to delete this amendment?'));
}

App.ui = (function ($) {
    'use strict';
    var $createAmendmentDialog, $confirmDeleteDialog;

    function createAmendment() {
        var stuff = {
            ProjectVersionId: App.pp.ProjectVersionId,
            PreviousProjectVersionId: App.pp.PreviousVersionId,
            AmendmentReason: $("#NewAmendment_AmendmentReason").val(),
            AmendmentCharacter: $("#NewAmendment_AmendmentCharacter").val(),
            AmendmentTypeId: $("#NewAmendment_AmendmentTypeId").val(),
            AmendmentStatusId: App.pp.AmendmentStatusId
        };
        // TODO: validate form

        // send data. reload if successful.
        App.postit("/Api/TipProjectAmendment", {
            data: JSON.stringify(stuff),
            success: function (id) {
                var url;
                if (!id) {
                    id = App.pp.PreviousVersionId;
                }
                url = '/Trips/Project/' + App.pp.TipYear + '/Amendments/' + id + '?message=Amendment%20created%20successfully.';
                window.onbeforeunload = null;
                window.location.assign(url);
            }
        });
    }

    function promoteAmendment() {
        var stuff = {
            ProjectVersionId: App.pp.ProjectVersionId,
            PreviousProjectVersionId: App.pp.PreviousVersionId,
            AmendmentStatusId: App.pp.AmendmentStatusId
        };

        // send data. reload if successful.
        App.postit("/Api/TipProjectAmendment", {
            data: JSON.stringify(stuff),
            success: function (id) {
                var url;
                if (!id) {
                    id = App.pp.PreviousVersionId;
                }
                url = '/Trips/Project/' + App.pp.TipYear + '/Amendments/' + id + '?message=Amendment%20updated%20successfully.';
                window.onbeforeunload = null;
                window.location.assign(url);
            }
        });
    }


    function deleteAmendment() {
        // send data. redirect if successful.
        App.postit("/Api/TipProjectAmendment/" + App.pp.ProjectVersionId, {
            type: "DELETE",
            success: function (id) {
                var url;
                if (!id) {
                    id = App.pp.PreviousVersionId;
                }
                url = '/Trips/Project/' + App.pp.TipYear + '/Amendments/' + id + '?message=Amendment%20deleted%20successfully.';
                window.onbeforeunload = null;
                window.location.assign(url);
            }
        });
    }

    function updateAmendment() {
        var amendmentReason = $("#ProjectAmendments_AmendmentReason").val(),
            amendmentCharacter = $("#ProjectAmendments_AmendmentCharacter").val(),
            stuff = {
                ProjectVersionId: App.pp.ProjectVersionId,
                AmendmentReason: amendmentReason,
                AmendmentCharacter: amendmentCharacter
            };
        App.postit("/api/TipProjectAmendment", {
            type: "PUT",
            data: JSON.stringify(stuff),
            success: function (response) {
                window.onbeforeunload = null;
                window.location.reload();
            }
        });
    }



    // fill dialog with data from the page
    function autofillAmendment() {
        $("#NewAmendment_AmendmentReason", $createAmendmentDialog).val($("#ProjectAmendments_AmendmentReason").val());
        $("#NewAmendment_AmendmentCharacter", $createAmendmentDialog).val($("#ProjectAmendments_AmendmentCharacter").val());
        $("#NewAmendment_AmendmentTypeId", $createAmendmentDialog).val($("#ProjectAmendments_AmendmentTypeId").val());
    }

    // enable/disable Move to Proposed button
    function charReasonCheck() {
        var reason = $("#ProjectAmendments_AmendmentReason").val(),
            character = $("#ProjectAmendments_AmendmentCharacter").val();
        if ((!App.pp.AmendmentIsPending && reason === "") || character === "") {
            $("#move-to-proposed").button("disable");
        } else {
            $("#move-to-proposed").button("enable");
        }
    }

    function initialize() {
        $('#projectListGrid').dataTable({
            "iDisplayLength": 10,
            "bSort": false,
            //"aaSorting": [[1, "desc"]],
            "aoColumns": [
                { "bSortable": false, "sWidth": "5%" },
                { "sWidth": "15%" },
                { "sWidth": "15%" },
                { "sWidth": "15%" },
                { "sWidth": "50%"}]
        });
        $('#projectListGrid_length').attr("style", "display:none");

        $('#update-amendment').button().click(function () {
            updateAmendment();
            return false;
        });


        // run check after leaving input
        //$("#ProjectAmendments_AmendmentReason, #ProjectAmendments_AmendmentCharacter").blur(function() {
        //    charReasonCheck();
        //});
        $createAmendmentDialog = $("#create-amendment-dialog");
        $createAmendmentDialog.dialog({
            autoOpen: false,
            width: 700,
            height: 500,
            modal: true,
            buttons: {
                "Process": createAmendment,
                "AutoFill": autofillAmendment,
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });

        $("#create-amendment").button().click(function () {
            $createAmendmentDialog.dialog("open");
            return false;
        });

        // move-to-proposed and amend-project are identical operations
        $("#move-to-proposed, #amend-project").button().click(function () {
            promoteAmendment();
            return false;
        });

        // run check right away
        charReasonCheck();

        $confirmDeleteDialog = $("#delete-amendment-dialog");
        $confirmDeleteDialog.dialog({
            autoOpen: false,
            modal: true,
            buttons: {
                "Delete": function () {
                    deleteAmendment();
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });

        $("#delete-amendment").button().click(function () {
            $confirmDeleteDialog.dialog("open");
            return false;
        });
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);