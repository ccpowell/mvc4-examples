/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1-vsdoc.js" />

// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

var App = App || {};

App.ui = (function ($) {
    "use strict";
    function initializeOmbudsmanTable() {
        var dtoptions, $dt;

        dtoptions = {
            bStateSave: false,
            bServerSide: true,
            bProcessing: true,
            bFilter: false,
            bSort: false,
            bAutoWidth: true,
            bPaginate: true,
            sAjaxSource: "/page/getombudsmen",
            aoColumns: [
                { sTitle: "ID", mDataProp: "OmbudsmanId" },
                { sTitle: "Name", mDataProp: "Name" }
            ]
        };
        $dt = $("#ombudsman-table").dataTable(dtoptions);

    }

    function initializeFacilityTable() {
        var dtoptions, $dt;

        dtoptions = {
            bStateSave: false,
            bServerSide: true,
            bProcessing: true,
            bFilter: false,
            bSort: false,
            bAutoWidth: true,
            bPaginate: true,
            sAjaxSource: "/page/getfacilities",
            /*
            fnServerData: function (sSource, aoData, fnCallback) {
                $.getJSON(sSource, function (page) {
                    $("#message").append("<div>facilities table fetched</div>");
                    fnCallback(page);
                });
            },
            */
            aoColumns: [
                { sTitle: "ID", mDataProp: "FacilityId" },
                { sTitle: "Name", mDataProp: "Name" },
                { sTitle: "Ombudsman Name", mDataProp: "OmbudsmanName" }
            ]
        };
        $dt = $("#facility-table").dataTable(dtoptions);
    }


    function initialize() {

        $.ajaxSetup({
            // Disable caching of AJAX responses
            // TODO: handle caching at the server?
            cache: false,
            error: function (jqXHR, textStatus, errorThrown) {
                alert("ajax error " + errorThrown.toString());
            }
        });
        initializeOmbudsmanTable();
        initializeFacilityTable();
        $("#message").append("<div>initialized</div>");
    }


    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);
