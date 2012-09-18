/// <reference path="jquery-1.8.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.23.js" />
/// <reference path="knockout-2.1.0.debug.js" />

// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false, ko: false */

var App = App || {};

App.Facility = function () {
    "use strict";
    return {
        FacilityId: 0,
        FacilityTypeId: 1,
        OmbudsmanName: '',
        Name: '',
        Address1: '',
        Address2: '',
        City: '',
        State: 'CO',
        ZipCode: '',
        Phone: '',
        Fax: '',
        IsActive: false,
        NumberOfBeds: 0,
        IsMedicaid: false,
        IsContinuum: false
    };
};


App.Ombudsman = function () {
    "use strict";
    return {
        OmbudsmanId: 0,
        Name: '',
        UserName: '',
        Email: '',
        Address1: '',
        Address2: '',
        City: '',
        State: 'CO',
        ZipCode: '',
        Phone: '',
        Fax: ''
    };
};

App.ViewModel = function ($) {
    "use strict";
    var self = this;
    self.ombudsmen = ko.observableArray([]);
    self.ombudsmanNames = ko.observableArray([]);
    self.filterOmbudsmanName = ko.observable('');
    self.filterFacilityTypeId = ko.observable(0);

    self.isUpFacility = ko.observable(false);
    self.editFacility = ko.observable(App.Facility());
    self.editFacilityIsUpdate = ko.computed(function () {
        var fac = self.editFacility();
        return (fac !== null) && (fac.FacilityId > 0);
    });

    self.createFacility = function () {
        self.editFacility(App.Facility());
        self.isUpFacility(true);
    };

    self.cancelEditFacility = function () {
        self.isUpFacility(false);
        self.editFacility(App.Facility());
    };

    // HACK: selectOptions binding only works with an observable.
    self.xFacilityTypeId = ko.observable(1);

    self.getEditFacility = function () {
        var facility = self.editFacility();
        facility.FacilityTypeId = self.xFacilityTypeId();
        return facility;
    };

    self.acceptEditFacility = function () {
        var facility = self.getEditFacility(),
            isUpdate = self.editFacilityIsUpdate(),
            type = "POST",
            url = "/ombudsman/api/facility";
        if (isUpdate) {
            type = "PUT";
            url += "/" + facility.FacilityId;
        }
        $.ajax(url, {
            type: type,
            data: facility,
            complete: function (jqXHR, status) {
                if (status === "success") {
                    if (isUpdate) {
                        alert("facility updated");
                    } else {
                        alert("facility created");
                    }
                    self.cancelEditFacility();
                }
            }
        });
    };

    self.isUpOmbudsman = ko.observable(false);
    self.editOmbudsman = ko.observable(App.Ombudsman());
    self.editOmbudsmanIsUpdate = ko.computed(function () {
        var omb = self.editOmbudsman();
        return (omb !== null) && (omb.OmbudsmanId > 0);
    });
    
    self.createOmbudsman = function () {
        self.editOmbudsman(App.Ombudsman());
        self.isUpOmbudsman(true);
    };

    self.cancelEditOmbudsman = function () {
        self.isUpOmbudsman(false);
        self.editOmbudsman(App.Ombudsman());
    };

    self.acceptEditOmbudsman = function () {
        var omb = self.editOmbudsman(),
            isUpdate = self.editOmbudsmanIsUpdate(),
            type = "POST",
            url = "/ombudsman/api/ombudsman";
        if (isUpdate) {
            type = "PUT";
            url += "/" + omb.OmbudsmanId;
        }
        $.ajax(url, {
            type: type,
            data: omb,
            complete: function (jqXHR, status) {
                if (status === "success") {
                    if (isUpdate) {
                        alert("ombudsman updated");
                    } else {
                        alert("ombudsman created");
                    }
                    self.cancelEditOmbudsman();
                }
            }
        });
    };
};


App.ui = (function ($) {
    "use strict";
    var facilityDataTable, ombudsmanDataTable;

    function getOmbId(name) {
        var id = 0;
        if (name && name.length > 0) {
            $.each(App.viewmodel.ombudsmen(), function (index, item) {
                if (item.Name === name) {
                    id = item.OmbudsmanId;
                    return false;
                }
            });
        }
        return id;
    }

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
            sAjaxSource: "/Ombudsman/page/getfacilities",
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                // attach ID to row for callbacks to use
                $(nRow).data("id", aData.FacilityId);
            },
            fnServerParams: function (aoData) {
                aoData.push({ name: 'onlyOmbudsmanId', value: getOmbId(App.viewmodel.filterOmbudsmanName()) });
                aoData.push({ name: 'onlyFacilityTypeId', value: App.viewmodel.filterFacilityTypeId() });
            },

            aoColumns: [
                { sTitle: "Name", mDataProp: "Name", sClass: "pointer" },
                { sTitle: "Type", mDataProp: "FacilityTypeName" },
                { sTitle: "Ombudsman Name", mDataProp: "OmbudsmanName" }
            ]
        };
        facilityDataTable = $("#facility-table").dataTable(dtoptions);

        // bind click on the first column of the row
        $('#facility-table tbody').on("click", "tr td:first-child", function (e) {
            var id = $(this).closest("tr").data("id");
            $.getJSON("/ombudsman/api/facility", { id: id }, function (data) {
                App.viewmodel.editFacility(data);
                App.viewmodel.isUpFacility(true);
            });
            return false; // stop propagation and default behavior
        });
    }


    function initializeOmbudsmanTable() {
        var dtoptions;

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
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                // attach ID to row for callbacks to use
                $(nRow).data("id", aData.OmbudsmanId);
            },
            aoColumns: [
                { sTitle: "Name", mDataProp: "Name", sClass: "pointer" },
                { sTitle: "User Name", mDataProp: "UserName" }
            ]
        };
        ombudsmanDataTable = $("#ombudsman-table").dataTable(dtoptions);

        // bind click on the first column of the row
        $('#ombudsman-table tbody').on("click", "tr td:first-child", function (e) {
            var id = $(this).closest("tr").data("id");
            $.getJSON("/ombudsman/api/ombudsman", { id: id }, function (data) {
                App.viewmodel.editOmbudsman(data);
                App.viewmodel.isUpOmbudsman(true);
            });
            return false; // stop propagation and default behavior
        });
    }


    function reloadFacilityTable() {
        facilityDataTable.fnDraw();
    }

    function reloadOmbudsmanTable() {
        ombudsmanDataTable.fnDraw();
    }

    function reloadOmbudsmen() {
        $.ajax("/ombudsman/api/ombudsman", {
            success: function (data) {
                var items = [];
                jQuery.each(data, function (index, item) {
                    items.push(item.Name);
                });
                App.viewmodel.ombudsmanNames(items);
                App.viewmodel.ombudsmen(data);
            }
        });
    }

    function initialize() {

        function initView() {
            initializeFacilityTable();
            initializeOmbudsmanTable();

            ko.applyBindings(App.viewmodel, document.body);

            App.viewmodel.filterFacilityTypeId.subscribe(reloadFacilityTable);
            App.viewmodel.filterOmbudsmanName.subscribe(reloadFacilityTable);
            App.viewmodel.isUpFacility.subscribe(function (isUp) {
                if (!isUp) {
                    reloadFacilityTable();
                }
            });
            App.viewmodel.isUpOmbudsman.subscribe(function (isUp) {
                if (!isUp) {
                    reloadOmbudsmanTable();
                }
            });
        }


        //$("#debug").click(function () { debugger; });

        $.ajaxSetup({
            // Disable caching of AJAX responses
            // TODO: handle caching at the server?
            cache: false,
            error: function (jqXHR, textStatus, errorThrown) {
                alert("ajax error " + errorThrown.toString());
            }
        });

        // get data required for app
        // save it in the viewmodel
        // when all data arrived, initialize bindings
        App.viewmodel = new App.ViewModel($);
        reloadOmbudsmen();

        window.setTimeout(initView, 3000);
    }

    function acOmbudsman(request, response) {
        $.ajax("/ombudsman/page/getacombudsman", {
            data: { term: request.term },
            success: function (data) {
                var names = [];
                $.each(data, function (index, item) {
                    names.push(item.Name);
                });
                response(names);
            }
        });
    }

    return {
        initialize: initialize,
        acOmbudsman: acOmbudsman
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);