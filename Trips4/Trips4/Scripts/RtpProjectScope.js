﻿// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the RTP Project Scope page
// see Copy of RtpProjectScope for the original

var App = App || {};

App.ui = (function ($) {
    'use strict';
    var targetSegmentDetails = null,
        targetLrs = null;


    function getTargetLrs() {
        var $dlg = $('#segment-lrs-dialog'),
            fields = 'input[id^="segment-lrs-"], select[id^="segment-lrs-"]',
            t = {
                SegmentId: targetSegmentDetails.SegmentId
            },
            stuff = { target: t, type: "POST" };

        // copy original LRS
        if (targetLrs) {
            $.extend(t, targetLrs);
            stuff.type = "PUT";
        }

        // update with fields from the form
        $(fields, $dlg).each(function (index, el) {
            var property = el.id.replace('segment-lrs-', '');
            t[property] = $(el).val();
        });

        //stuff.valid = $('form', $dlg).valid();

        return stuff;
    }

    // get Segment data from the form.
    // also validates the form and determines create/update type.
    function getTargetSegment() {
        var $dlg = $('#segment-details-dialog'),
            fields = 'input[id^="segment-details-"], select[id^="segment-details-"]',
            t = {
                ProjectVersionId: App.pp.ProjectVersionId
            },
            stuff = { target: t, type: "POST" };

        if (targetSegmentDetails) {
            $.extend(t, targetSegmentDetails);
            stuff.type = "PUT";
        }

        $(fields, $dlg).each(function (index, el) {
            var property = el.id.replace('segment-details-', '');
            t[property] = $(el).val();
        });

        stuff.valid = $('form', $dlg).valid();
        return stuff;
    }

    // create or update Segment
    function saveSegment() {
        var stuff = getTargetSegment();
        if (stuff.valid) {
            App.postit("/api/RtpProjectSegment", {
                data: JSON.stringify(stuff.target),
                type: stuff.type,
                success: function () { window.location.reload(); }
            });
        }
    }

    // refresh the list of LRS in the Segment Details
    function reloadLrsTable() {
        var $dlg = $('#segment-details-dialog'),
            $lrs = $('#segment-details-lrs-table', $dlg),
            $lrsBody = $('tbody', $lrs);

        // remove rows in LRS table
        $lrsBody.empty();

        if (targetSegmentDetails) {
            var stuff = { segmentId: targetSegmentDetails.SegmentId, x: 0 };
            $.getJSON("/Trips/api/RtpProjectLrs", stuff, function (data) {
                $.each(data, function (index, item) {
                    var row = '<tr>' +
                        '<td>' + item.Routename + '</td>' +
                        '<td>' + item.BEGINMEASU + ' - ' + item.ENDMEASURE + '</td>' +
                        '<td>' +
                        '<span class="table-button" data-command="details">Details</span>' +
                        '<span class="table-button" data-command="delete">Delete</span>' +
                        '</td>' +
                        '</tr>',
                        $row = $(row);
                    $row.data("lrs-id", item.Id);
                    $lrsBody.append($row);
                });
            });
        }
    }

    // create or update LRS
    function saveLrs() {
        var stuff = getTargetLrs();
        App.postit("/api/RtpProjectLrs/", {
            data: JSON.stringify(stuff.target),
            type: stuff.type,
            success: reloadLrsTable
        });
    }

    function openSegmentDetails() {
        var $dlg = $('#segment-details-dialog'),
            $lrs = $('#segment-details-lrs-section', $dlg),
            fields = 'input[id^="segment-details-"], select[id^="segment-details-"]';

        reloadLrsTable();

        // clear the form fields
        $('form', $dlg).validate().resetForm();

        // set data if this is an edit
        // HACK! remove displayed errors. a bug in validate keeps resetForm
        // from clearing them properly.
        if (targetSegmentDetails) {
            $(fields, $dlg).removeClass('error').each(function (index, el) {
                var property = el.id.replace('segment-details-', '');
                $(el).val(targetSegmentDetails[property]);
            });
            $lrs.show();
        } else {
            $(fields, $dlg).removeClass('error');
            $lrs.hide();
        }


        $dlg.dialog("open");
        return false;
    }


    function openLrs() {
        var $dlg = $('#segment-lrs-dialog'),
            fields = 'input[id^="segment-lrs-"], select[id^="segment-lrs-"]';

        if (targetLrs) {
            $(fields, $dlg).each(function (index, el) {
                var property = el.id.replace('segment-lrs-', '');
                $(el).val(targetLrs[property]);
            });
        } else {
            $(fields, $dlg).val("");
        }
        $dlg.dialog("open");
        return false;
    }


    function initializeDetailsDialog() {
        var $dlg = $('#segment-details-dialog'),
            $table = $('table#segments');

        $dlg.dialog({
            autoOpen: false,
            width: 920,
            height: 600,
            modal: true,
            buttons: {
                "Save": saveSegment,
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });

        // inputs need names for validation - we just copy the ID
        $('form input', $dlg).each(function (index, el) {
            $(el).attr("name", el.id);
        });

        // LRS table buttons
        $table.on('click', 'span[data-segment-details]', function (e) {
            var id = $(this).data('segment-details');
            // get Segment Details and open dialog
            App.postit("/api/RtpProjectSegment/" + id, {
                type: "GET",
                success: function (data) {
                    targetSegmentDetails = data;
                    openSegmentDetails();
                }
            });

            return false;
        });

        $table.on('click', 'span[data-segment-delete]', function (e) {
            var id = $(this).data('segment-delete');
            App.postit("/api/RtpProjectSegment/" + id, {
                type: "DELETE",
                success: function () {
                    window.location.reload();
                }
            });

            return false;
        });

        // Add LRS button
        $('#segment-details-add-lrs').button().click(function () {
            targetLrs = null;
            openLrs();
            return false;
        });

        // buttons in LRS table
        $('table#segment-details-lrs-table tbody').on('click', 'span.table-button', function (e) {
            var id = $(this).closest('tr').data('lrs-id'),
                command = $(this).data('command');
            switch (command) {
                case 'delete':
                    App.postit("/api/RtpProjectLrs/" + id, {
                        type: "DELETE",
                        success: reloadLrsTable
                    });
                    break;

                case 'details':
                    $.getJSON('/Trips/api/RtpProjectLrs/' + id, function (data) {
                        targetLrs = data;
                        openLrs();
                    });
                    break;
            }
        });

    }

    function initializeLrsDialog() {
        $('#segment-lrs-dialog').dialog({
            autoOpen: false,
            width: 600,
            height: 600,
            modal: true,
            buttons: {
                "Save": function () {
                    saveLrs();
                    $(this).dialog("close");
                },
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });
        //
    }

    function initialize() {
        initializeDetailsDialog();
        initializeLrsDialog();

        $('#new-segmentdetails').button().click(function () {
            targetSegmentDetails = null;
            openSegmentDetails();
            return false;
        });


        App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
        $('#submitForm').button({ disabled: true });
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);