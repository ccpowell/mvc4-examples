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
            sPaginationType: "full_numbers",
            bJQueryUI: true,
            sAjaxSource: "/Ombudsman/page/getombudsmen",
            aoColumns: [
                { sTitle: "ID", mDataProp: "OmbudsmanId" },
                { sTitle: "Name", mDataProp: "Name" },
                { sTitle: "User Name", mDataProp: "UserName" },
                { mDataProp: null, sWidth: "120px", fnRender: function (oObj) {
                    // Ombudsman is oObj.aData
                    var id = oObj.aData.OmbudsmanId,
                        editButton, deleteButton;
                    editButton = '<a href="/Ombudsman/Ombudsman/Edit?id=' + id + '">Edit</a>';
                    deleteButton = '<a href="/Ombudsman/Ombudsman/Delete?id=' + id + '">Delete</a>';
                    return editButton + ' | ' + deleteButton;
                }
                }
            ]
        };
        $dt = $("#ombudsman-table").dataTable(dtoptions);
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
    }


    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);
