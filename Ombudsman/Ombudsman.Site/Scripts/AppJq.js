/// <reference path="jquery-1.8.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.23.js" />

// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

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
    var self = this,
        editFacility = new App.Facility(),
        editOmbudsman = new App.Ombudsman();

    self.getOmbudsmanFilter = function () {
        return {
            onlyOmbudsmanIsActive: $("#filter-ombudsman-isactive option:selected").val()
        };
    };

    self.getFacilityFilter = function () {
        return {
            onlyFacilityName: $("#filter-facility-name").val(),
            onlyFacilityTypeId: $("#filter-facility-typeid option:selected").val(),
            onlyFacilityIsActive: $("#filter-facility-isactive option:selected").val(),
            onlyOmbudsmanName: $("#filter-ombudsman-name").val()
        };
    };

    self.editFacilityIsUpdate = function () {
        return (editFacility !== null) && (editFacility.FacilityId > 0);
    };

    self.setEditFacility = function (facility) {
        var $dlg = $("#facility-dialog");
        editFacility = facility;

        $("#facdlg-FacilityId", $dlg).text(facility.FacilityId);
        $("#facdlg-FacilityTypeId option", $dlg).val(facility.FacilityTypeId);
        $("#facdlg-OmbudsmanName", $dlg).val(facility.OmbudsmanName);
        $("#facdlg-Name", $dlg).val(facility.Name);
        $("#facdlg-Address1", $dlg).val(facility.Address1);
        $("#facdlg-Address2", $dlg).val(facility.Address2);
        $("#facdlg-City", $dlg).val(facility.City);
        $("#facdlg-State", $dlg).val(facility.State);
        $("#facdlg-ZipCode", $dlg).val(facility.ZipCode);
        $("#facdlg-Phone", $dlg).val(facility.Phone);
        $("#facdlg-Fax", $dlg).val(facility.Fax);
        $("#facdlg-Phone", $dlg).val(facility.Phone);
        $("#facdlg-IsActive", $dlg).prop("checked", facility.IsActive);
        $("#facdlg-NumberOfBeds", $dlg).val(facility.NumberOfBeds);
        $("#facdlg-IsMedicaid", $dlg).prop("checked", facility.IsMedicaid);
        $("#facdlg-IsContinuum", $dlg).prop("checked", facility.IsContinuum);
    };

    // get a Facility from the user-entered data 
    self.getEditFacility = function () {
        var $dlg = $("#facility-dialog"),
            facility = new App.Facility();
        facility.FacilityId = editFacility.FacilityId;
        facility.FacilityTypeId = $("#facdlg-FacilityTypeId option:selected", $dlg).val();
        facility.OmbudsmanName = $("#facdlg-OmbudsmanName", $dlg).val();
        facility.Name = $("#facdlg-Name", $dlg).val();
        facility.Address1 = $("#facdlg-Address1", $dlg).val();
        facility.Address2 = $("#facdlg-Address2", $dlg).val();
        facility.City = $("#facdlg-City", $dlg).val();
        facility.State = $("#facdlg-State", $dlg).val();
        facility.ZipCode = $("#facdlg-ZipCode", $dlg).val();
        facility.Phone = $("#facdlg-Phone", $dlg).val();
        facility.Fax = $("#facdlg-Fax", $dlg).val();
        facility.IsActive = $("#facdlg-IsActive", $dlg).is(":checked");
        facility.NumberOfBeds = $("#facdlg-NumberOfBeds", $dlg).val();
        facility.IsMedicaid = $("#facdlg-IsMedicaid", $dlg).is(":checked");
        facility.IsContinuum = $("#facdlg-IsContinuum", $dlg).is(":checked");

        return facility;
    };


    self.editOmbudsmanIsUpdate = function () {
        return (editOmbudsman !== null) && (editOmbudsman.OmbudsmanId > 0);
    };

    self.getEditOmbudsman = function () {
        var $dlg = $("#ombudsman-dialog"),
            ombudsman = new App.Ombudsman();
        ombudsman.OmbudsmanId = editOmbudsman.OmbudsmanId;
        ombudsman.Name = $("#ombdlg-Name", $dlg).val();
        ombudsman.Email = $("#ombdlg-Email", $dlg).val();
        ombudsman.Address1 = $("#ombdlg-Address1", $dlg).val();
        ombudsman.Address2 = $("#ombdlg-Address2", $dlg).val();
        ombudsman.City = $("#ombdlg-City", $dlg).val();
        ombudsman.State = $("#ombdlg-State", $dlg).val();
        ombudsman.ZipCode = $("#ombdlg-ZipCode", $dlg).val();
        ombudsman.Fax = $("#ombdlg-Fax", $dlg).val();
        ombudsman.Phone = $("#ombdlg-Phone", $dlg).val();
        return ombudsman;
    };

    self.setEditOmbudsman = function (ombudsman) {
        var $dlg = $("#ombudsman-dialog");
        editOmbudsman = ombudsman;
        $("#ombdlg-OmbudsmanId", $dlg).text(ombudsman.OmbudsmanId);
        $("#ombdlg-Name", $dlg).val(ombudsman.Name);
        $("#ombdlg-Email", $dlg).val(ombudsman.Email);
        $("#ombdlg-Address1", $dlg).val(ombudsman.Address1);
        $("#ombdlg-Address2", $dlg).val(ombudsman.Address2);
        $("#ombdlg-City", $dlg).val(ombudsman.City);
        $("#ombdlg-State", $dlg).val(ombudsman.State);
        $("#ombdlg-ZipCode", $dlg).val(ombudsman.ZipCode);
        $("#ombdlg-Fax", $dlg).val(ombudsman.Fax);
        $("#ombdlg-Phone", $dlg).val(ombudsman.Phone);
    };

    return self;
};


App.ui = (function ($) {
    "use strict";
    var facilityDataTable, ombudsmanDataTable;

    function Autocompleter() {
        var self = this;
        this.ombudsmen = [];
        this.acOmbudsman = function (request, response) {
            $.ajax("/ombudsman/page/getacombudsman", {
                data: { term: request.term },
                success: function (data) {
                    self.ombudsmen = data;
                    var names = [];
                    $.each(data, function (index, item) {
                        names.push(item.Name);
                    });
                    response(names);
                }
            });
        };

        this.isOnList = function (term) {
            var isOn = false;
            $.each(self.ombudsmen, function (index, item) {
                if (item === term) {
                    isOn = true;
                    return false;
                }
            });
            return isOn;
        };
    }

    function reloadFacilityTable() {
        facilityDataTable.fnDraw();
    }

    function reloadOmbudsmanTable() {
        ombudsmanDataTable.fnDraw();
    }

    function cancelEditOmbudsman() {
        $("#ombudsman-dialog").dialog("close");
        return false;
    }

    function acceptEditOmbudsman() {
        var omb = App.viewmodel.getEditOmbudsman(),
            isUpdate = App.viewmodel.editOmbudsmanIsUpdate(),
            type = "POST",
            url = "/ombudsman/api/ombudsman";

        if (!$("#ombudsman-dialog form").valid()) {
            alert("Please enter all required fields");
            return false;
        }
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
                        alert("Ombudsman updated");
                    } else {
                        alert("Ombudsman created");
                    }
                    cancelEditOmbudsman();
                    reloadOmbudsmanTable();
                }
            }
        });
        return false;
    }

    function cancelEditFacility() {
        $("#facility-dialog").dialog("close");
        return false;
    }

    function acceptEditFacility() {
        var facility = App.viewmodel.getEditFacility(),
            isUpdate = App.viewmodel.editFacilityIsUpdate(),
            type = "POST",
            url = "/ombudsman/api/facility";

        if (!$("#facility-dialog form").valid()) {
            alert("Please enter all required fields");
            return false;
        }

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
                        alert("Facility updated");
                    } else {
                        alert("Facility created");
                    }
                    cancelEditFacility();
                    reloadFacilityTable();
                }
            }
        });
        return false;
    }



    // edit or create facility
    function editFacility(facility) {
        App.viewmodel.setEditFacility(facility);
        $("#facility-dialog").dialog("open");
        return false;
    }

    // edit or create ombudsman
    function editOmbudsman(ombudsman) {
        App.viewmodel.setEditOmbudsman(ombudsman);
        $("#ombudsman-dialog").dialog("open");
        return false;
    }

    function initialize() {
        var tid;

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
                    var filter = App.viewmodel.getFacilityFilter();
                    aoData.push({ name: 'onlyFacilityName', value: filter.onlyFacilityName });
                    aoData.push({ name: 'onlyOmbudsmanName', value: filter.onlyOmbudsmanName });
                    aoData.push({ name: 'onlyFacilityTypeId', value: filter.onlyFacilityTypeId });
                    aoData.push({ name: 'onlyFacilityIsActive', value: filter.onlyFacilityIsActive });
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
                $.getJSON("/ombudsman/api/facility", { id: id }, editFacility);
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
                fnServerParams: function (aoData) {
                    var filter = App.viewmodel.getOmbudsmanFilter();
                    aoData.push({ name: 'onlyOmbudsmanIsActive', value: filter.onlyOmbudsmanIsActive });
                },
                aoColumns: [
                { sTitle: "Name", mDataProp: "Name", sClass: "pointer" }
            ]
            };
            ombudsmanDataTable = $("#ombudsman-table").dataTable(dtoptions);

            // bind click on the first column of the row
            $('#ombudsman-table tbody').on("click", "tr td:first-child", function (e) {
                var id = $(this).closest("tr").data("id");
                $.getJSON("/ombudsman/api/ombudsman", { id: id }, editOmbudsman);
                return false; // stop propagation and default behavior
            });
        }

        function initializeOmbudsmanDialog() {
            var $dlg = $("#ombudsman-dialog");
            $dlg.dialog({ autoOpen: false, resizable: true, modal: true, width: 'auto' });
            $("#ombdlg-acceptEditOmbudsman", $dlg)
                .button()
                .click(acceptEditOmbudsman);
            $("#ombdlg-cancelEditOmbudsman", $dlg)
                .button()
                .click(cancelEditOmbudsman);

            // for validation, the input fields require names
            $("form input", $dlg).each(function (index, item) {
                $(item).attr("name", item.id);
            });
        }

        function initializeFacilityDialog() {
            var $dlg = $("#facility-dialog"),
                autocompleter = new Autocompleter();
            $dlg.dialog({ autoOpen: false, resizable: true, modal: true, width: 'auto' });
            $("#facdlg-OmbudsmanName", $dlg)
                .autocomplete({
                    source: autocompleter.acOmbudsman,
                    close: reloadFacilityTable
                })
                .change(function () {
                    var current = $(this).val();
                    if (!autocompleter.isOnList(current)) {
                        $(this).val('');
                    }
                    reloadFacilityTable();
                });
            $("#facdlg-acceptEditFacility", $dlg)
                .button()
                .click(acceptEditFacility);
            $("#facdlg-cancelEditFacility", $dlg)
                .button()
                .click(cancelEditFacility);

            // for validation, the input fields require names
            $("form input", $dlg).each(function (index, item) {
                $(item).attr("name", item.id);
            });
        }

        function initializeMain() {
            var autocompleter = new Autocompleter();

            $("#tabs").tabs();

            initializeFacilityTable();
            initializeOmbudsmanTable();

            // table filter by type
            $("#filter-facility-typeid")
                .change(reloadFacilityTable);

            // table filter by Active
            $("#filter-facility-isactive")
                .change(reloadFacilityTable);

            // table filter by name
            $("#filter-facility-name")
                .change(reloadFacilityTable);

            // autocomplete for table filter ombudsman
            $("#filter-ombudsman-name")
                .autocomplete({
                    source: autocompleter.acOmbudsman,
                    close: reloadFacilityTable
                })
                .change(function () {
                    var current = $(this).val();
                    if (!autocompleter.isOnList(current)) {
                        $(this).val('');
                    }
                    reloadFacilityTable();
                });

            // table filter by Active
            $("#filter-ombudsman-isactive")
                .change(reloadOmbudsmanTable);

            $("#create-facility").button().click(function () {
                editFacility(new App.Facility());
            });

            $("#create-ombudsman").button().click(function () {
                editOmbudsman(new App.Ombudsman());
            });

            $("#download-facility-list").button().click(function () {
                var filter = App.viewmodel.getFacilityFilter(),
                    query;
                query =
                    'onlyFacilityName=' + encodeURIComponent(filter.onlyFacilityName) + '&' +
                    'onlyFacilityIsActive=' + encodeURIComponent(filter.onlyFacilityIsActive) + '&' +
                    'onlyFacilityTypeId=' + encodeURIComponent(filter.onlyFacilityTypeId) + '&' +
                    'onlyOmbudsmanName=' + encodeURIComponent(filter.onlyOmbudsmanName);
                window.open("/Ombudsman/Page/GetFacilityList?" + query, "_blank");
            });

        }

        function initView() {
            // we don't need anything at this point
            //if (true) {
            initializeMain();
            initializeFacilityDialog();
            initializeOmbudsmanDialog();
            window.clearInterval(tid);
            //}
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

        tid = window.setInterval(initView, 100);
    }


    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);