// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false, ko: false */

var App = App || {};

App.ViewModel = function ($) {
    "use strict";
    var self = this;
    self.editUser = ko.observable(null);
    self.editUserIsUpdate = ko.observable(false);

    self.createUser = function () {
        self.editUserIsUpdate(false);
        App.viewmodel.editUser({ Id: '', UserName: '', Organization: '', Title: '', RecoveryEmail: '', Phone: '' });
    };

    self.cancelEditUser = function () {
        self.editUser(null);
    };

    self.acceptEditUser = function () {
        var user = self.editUser(),
            type = "POST",
            url = "/api/user",
            isUpdate = self.editUserIsUpdate();
        if (isUpdate) {
            type = "PUT";
            url += "/" + user.Id;
        }
        $.ajax(url, {
            type: type,
            data: user,
            complete: function (jqXHR, status) {
                if (status === "success") {
                    if (isUpdate) {
                        alert("user updated");
                    } else {
                        alert("user created");
                    }
                    self.editUser(null);
                }
            }
        });
    };
};
App.viewmodel = new App.ViewModel(jQuery);

App.ui = (function ($) {
    "use strict";

    function initializePublicLists() {
        var ajaxsourceurl = "/Page/GetPublicLists",
            oTable;

        oTable = $('#public-lists-table').dataTable({
            "bStateSave": true,
            "bServerSide": true,
            "iTotalRecords": 50,
            "iTotalDisplayRecords": 50,
            "iDisplayLength": 50,
            "sAjaxSource": ajaxsourceurl,
            "bProcessing": true,
            bFilter: false,
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                $(nRow).data("id", aData.Id);
            },
            "aoColumns": [
                { sTitle: "ID", mDataProp: "Id" },
                { sTitle: "Name", mDataProp: "Name" }
            ]
        });
    }

    function initializeOwnedLists() {
        var ajaxsourceurl = "/Page/GetOwnedLists?id=" + App.userId,
            oTable;

        oTable = $('#owned-lists-table').dataTable({
            "bStateSave": true,
            "bServerSide": true,
            "iTotalRecords": 50,
            "iTotalDisplayRecords": 50,
            "iDisplayLength": 50,
            "sAjaxSource": ajaxsourceurl,
            "bProcessing": true,
            bFilter: false,
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                $(nRow).data("id", aData.Id);
            },
            "aoColumns": [
                { sTitle: "ID", mDataProp: "Id" },
                { sTitle: "Name", mDataProp: "Name" }
            ]
        });
    }

    function initializeAllContacts() {
        var ajaxsourceurl = "/page/getusers",
            oTable;

        oTable = $('#all-users-table').dataTable({
            bFilter: false,
            "bStateSave": true,
            "bServerSide": true,
            "iTotalRecords": 50,
            "iTotalDisplayRecords": 50,
            "iDisplayLength": 50,
            "sAjaxSource": ajaxsourceurl,
            "bProcessing": true,
            fnRowCallback: function (nRow, aData, iDisplayIndex) {
                // attach ID to row for callbacks to use
                $(nRow).data("id", aData.Id);
            },
            "aoColumns": [
                { sTitle: "Name", mDataProp: "UserName" },
                { sTitle: "Organization", mDataProp: "Organization", sWidth: "200px" },
                { sTitle: "Title", mDataProp: "Title", "sWidth": "200px", bSortable: false },
                { sTitle: "Primary Contact", mDataProp: "Phone", "sWidth": "100px", "sClass": "phone-US", bSortable: false },
                { sTitle: "Email", bSortable: false, mDataProp: function (source, type) {
                    return '<a href="mailto:' + source.RecoveryEmail + '">' + source.RecoveryEmail + '</a>';
                }
                }
            ]
        });

        // bind click on the first column of the row
        $('#all-users-table tbody').on("click", "tr td:eq(0)", function (e) {
            var uid = $(this).closest("tr").data("id");
            $.getJSON("/api/user", { id: uid }, function (data) {
                App.viewmodel.editUserIsUpdate(true);
                App.viewmodel.editUser(data);
            });
            return false; // stop propagation and default behavior
        });

        // refresh table after an edit or create
        App.viewmodel.editUser.subscribe(function (obj) {
            if (!obj) {
                oTable.fnDraw();
            }
        });
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

        ko.applyBindings(App.viewmodel, document.body);

        initializeAllContacts();
        initializePublicLists();
        initializeOwnedLists();
    }


    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);
