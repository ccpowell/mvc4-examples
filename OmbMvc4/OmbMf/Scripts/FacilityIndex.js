/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1-vsdoc.js" />

// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false, ko: false */

var App = App || {};

App.ViewModel = function ($) {
    "use strict";
    var self = this;
    self.filterOmbudsmanId = ko.observable(0);
    self.filterFacilityTypeId = ko.observable(0);
};


App.ui = (function ($) {
    "use strict";
    var facilityDataTable;

    function initializeFacilityTable() {
        var dtoptions = {
            bStateSave: false,
            bServerSide: true,
            bProcessing: true,
            bFilter: false,
            bSort: false,
            bAutoWidth: true,
            bPaginate: true,
            sPaginationType: "full_numbers",
            bJQueryUI: true,
            sAjaxSource: "/page/getfacilities",
            fnServerParams: function (aoData) {
                aoData.push({ name: 'onlyOmbudsmanId', value: App.viewmodel.filterOmbudsmanId() });
                aoData.push({ name: 'onlyFacilityTypeId', value: App.viewmodel.filterFacilityTypeId() });
            },

            aoColumns: [
                { sTitle: "ID", mDataProp: "FacilityId" },
                { sTitle: "Name", mDataProp: "Name" },
                { sTitle: "Type", mDataProp: "FacilityType.Name" },
                { sTitle: "Ombudsman Name", mDataProp: null, fnRender: function (oObj) {
                    // Facility is oObj.aData
                    var omb = oObj.aData.Ombudsman;
                    if (omb) {
                        return omb.Name;
                    }
                    return "";
                }
                },
                { mDataProp: null, sWidth: "120px", fnRender: function (oObj) {
                    // Facility is oObj.aData
                    var id = oObj.aData.FacilityId,
                        editButton, deleteButton;
                    editButton = '<a href="/Facility/Edit?id=' + id + '">Edit</a>';
                    deleteButton = '<a href="/Facility/Delete?id=' + id + '">Delete</a>';
                    return editButton + ' | ' + deleteButton;
                }
                }
            ]
        };
        facilityDataTable = $("#facility-table").dataTable(dtoptions);
    }

    function reloadTable() {
        facilityDataTable.fnDraw();
    }

    function initialize() {
        $("#debug").click(function () { debugger; });

        $.ajaxSetup({
            // Disable caching of AJAX responses
            // TODO: handle caching at the server?
            cache: false,
            error: function (jqXHR, textStatus, errorThrown) {
                alert("ajax error " + errorThrown.toString());
            }
        });


        App.viewmodel = new App.ViewModel($);
        ko.applyBindings(App.viewmodel, document.body);

        App.viewmodel.filterFacilityTypeId.subscribe(reloadTable);
        App.viewmodel.filterOmbudsmanId.subscribe(reloadTable);

        initializeFacilityTable();
    }


    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);
