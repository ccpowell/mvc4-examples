// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the RTP Project Scope page
// see Copy of RtpProjectScope for the original

var App = App || {};

App.ui = (function ($) {
    'use strict';
    var targetSegmentDetails = null;

    function addSegment() {
        window.location.reload();
    }

    function openSegmentDetails() {
        if (targetSegmentDetails) {
            $('#segment-details-dialog input[id^="segment-details-"]').each(function (index, el) {
                var property = el.id.replace('segment-details-', '');
                $(el).val(targetSegmentDetails[property]);
            });
        } else {
            $('#segment-details-dialog input').val("");
        }
        $('#segment-details-dialog')
            .dialog("open");
        return false;
    }

    function initialize() {
        $('#segment-details-dialog').dialog({
            autoOpen: false,
            width: 920,
            height: 500,
            modal: true,
            buttons: {
                "Add": addSegment,
                "Cancel": function () {
                    $(this).dialog("close");
                }
            }
        });

        $('#new-segmentdetails').button().click(function () {
            targetSegmentDetails = null;
            openSegmentDetails();
            return false;
        });

        $('button[data-segment-details]').button().click(function () {
            var id = $(this).data('segment-details');
            // get Segment Details and open dialog
            alert("edit " + id);
            return false;
        });
        $('button[data-segment-delete]').button().click(function () {
            var id = $(this).data('segment-delete');
            // get Segment Details and open dialog
            alert("delete " + id);
            return false;
        });
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);