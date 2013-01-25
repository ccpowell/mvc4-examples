/// <reference path="jquery-1.9.0-vsdoc.js" />
/// <reference path="jquery-ui-1.10.0.js" />

// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

var App = App || {};

App.Contact = function () {
    "use strict";
    return {
        Id: null,
        UserName: '',
        Title: '',
        Email: '',
        Phone: '',
        Organization: ''
    };
};

App.ContactList = function () {
    "use strict";
    return {
        Name: '',
        Id: '',
        OwnerId: '',
        Contacts: [],
        ContactIds: []
    };
};

App.utility = (function ($) {
    "use strict";
    var checkbuttonChangeIcon;

    // this jQuery UI callback will update the icon when the button is clicked
    checkbuttonChangeIcon = function () {
        $(this).button("option", {
            icons: { primary: this.checked ? 'ui-icon-check' : 'ui-icon-closethick' }
        });
    };
    function setCheckButton(jel, value) {
        jel.prop("checked", value)
            .button("refresh")
            .button("option", {
                icons: { primary: value ? 'ui-icon-check' : 'ui-icon-closethick' }
            });
    }

    return {
        setCheckButton: setCheckButton,
        checkbuttonChangeIcon: checkbuttonChangeIcon
    };

} (jQuery));


App.ViewModel = function ($) {
    "use strict";
    var self = this,
        editContactList = new App.ContactList(),
        editContact = new App.Contact();

    self.editContactListIsUpdate = function () {
        return (editContactList !== null) && (editContactList.Id !== null);
    };

    self.editContactIsUpdate = function () {
        return (editContact !== null) && (editContact.Id !== null);
    };

    self.setEditContactList = function (clist) {
        var $dlg = $("#contactlist-dialog");
        editContactList = clist;

        $("#cldlg-Id", $dlg).text(clist.Id);
        $("#cldlg-Name", $dlg).val(clist.Name);
    };

    // get a ContactList from the user-entered data 
    self.getEditContactList = function () {
        var $dlg = $("#contactlist-dialog"),
            clist = new App.ContactList();
        clist.Id = editContactList.Id;
        clist.Name = $("#cldlg-Name", $dlg).val();
        return clist;
    };

    self.getEditContact = function () {
        var $dlg = $("#contact-dialog"),
            contact = new App.Contact();
        contact.Id = editContact.Id;
        contact.UserName = $("#contactdlg-UserName", $dlg).val();
        contact.Email = $("#contactdlg-Email", $dlg).val();
        contact.Phone = $("#contactdlg-Phone", $dlg).val();
        contact.Title = $("#contactdlg-Title", $dlg).val();
        contact.Organization = $("#contactdlg-Organization", $dlg).val();
        return contact;
    };

    self.setEditContact = function (contact) {
        var $dlg = $("#contact-dialog");
        editContact = contact;
        $("#contactdlg-Id", $dlg).text(contact.Id);
        $("#contactdlg-UserName", $dlg).val(contact.UserName);
        $("#contactdlg-Email", $dlg).val(contact.Email);
        $("#contactdlg-Phone", $dlg).val(contact.Phone);
        $("#contactdlg-Title", $dlg).val(contact.Title);
        $("#contactdlg-Organization", $dlg).val(contact.Organization);
    };

    return self;
};


App.ui = (function ($) {
    "use strict";
    var contactListDataTable, contactDataTable;

    function reloadContactListTable() {
    }

    function reloadContactTable() {
        var url = App.env.applicationPath + "/api/contact";
        contactDataTable.fnClearTable();
        $.getJSON(url, function (data) {
            contactDataTable.fnAddData(data, true);
        });
    }

    function cancelEditContact() {
        $("#contact-dialog").dialog("close");
        return false;
    }

    function acceptEditContact() {
        var contact = App.viewmodel.getEditContact(),
            isUpdate = App.viewmodel.editContactIsUpdate(),
            type = "POST",
            url = App.env.applicationPath + "/api/contact";

        if (!$("#contact-dialog form").valid()) {
            alert("Please enter all required fields");
            return false;
        }

        if (isUpdate) {
            type = "PUT";
            url += "/" + contact.Id;
        }

        $.ajax(url, {
            type: type,
            data: contact,
            complete: function (jqXHR, status) {
                if (status === "success") {
                    if (isUpdate) {
                        alert("Contact updated");
                    } else {
                        alert("Contact created");
                    }
                    cancelEditContact();
                    reloadContactTable();
                }
            }
        });
        return false;
    }

    function cancelEditContactList() {
        $("#contactlist-dialog").dialog("close");
        return false;
    }

    function acceptEditContactList() {
        var clist = App.viewmodel.getEditContactList(),
            isUpdate = App.viewmodel.editContactListIsUpdate(),
            type = "POST",
            url = App.env.applicationPath + "/api/contactlist";

        if (!$("#contactlist-dialog form").valid()) {
            alert("Please enter all required fields");
            return false;
        }

        if (isUpdate) {
            type = "PUT";
            url += "/" + clist.Id;
        }

        $.ajax(url, {
            type: type,
            data: clist,
            complete: function (jqXHR, status) {
                if (status === "success") {
                    if (isUpdate) {
                        alert("ContactList updated");
                    } else {
                        alert("ContactList created");
                    }
                    cancelEditContactList();
                    reloadContactListTable();
                }
            }
        });
        return false;
    }

    // edit or create facility
    function editContactList(facility) {
        App.viewmodel.setEditContactList(facility);
        $("#contactlist-dialog").dialog("open").find("form").validate().resetForm();
        return false;
    }

    // edit or create contact
    function editContact(contact) {
        App.viewmodel.setEditContact(contact);
        $("#contact-dialog").dialog("open").find("form").validate().resetForm();
        return false;
    }

    function initialize() {
        var tid;

        function initializeContactListTable() {
            var dtoptions = {
                bStateSave: false,
                bServerSide: false,
                bProcessing: true,
                bFilter: false,
                bSort: true,
                bAutoWidth: false,
                bPaginate: true,
                sPaginationType: "full_numbers",
                bJQueryUI: true,
                fnRowCallback: function (nRow, aData, iDisplayIndex) {
                    var $row = $(nRow);
                    // attach ID to row for callbacks to use
                    $row.data("id", aData.Id);
                },

                aoColumns: [
                    { sTitle: "Name", mDataProp: "Name", sClass: "pointer" }
                ]
            };
            contactListDataTable = $("#contactlist-table").dataTable(dtoptions);

            // bind click on the first column of the row
            $('#contactlist-table tbody').on("click", "tr td:first-child", function (e) {
                var id = $(this).closest("tr").data("id");
                $.getJSON("/ombudsman/api/facility", { id: id }, editContactList);
                return false; // stop propagation and default behavior
            });
        }

        function initializeContactTable() {
            var dtoptions = {
                bStateSave: false,
                bServerSide: false,
                bProcessing: true,
                bFilter: false,
                bSort: true,
                bAutoWidth: false,
                bPaginate: true,
                sPaginationType: "full_numbers",
                bJQueryUI: true,
                fnRowCallback: function (nRow, aData, iDisplayIndex) {
                    var $row = $(nRow);
                    // attach ID to row for callbacks to use
                    $row.data("id", aData.Id);
                },
                aoColumns: [
                    { sTitle: "UserName", mDataProp: "UserName", sClass: "pointer" },
                    { sTitle: "Organization", mDataProp: "Organization" },
                    { sTitle: "Title", mDataProp: "Title" },
                    { sTitle: "Phone", mDataProp: "Phone" },
                    { sTitle: "Email", mDataProp: "Email" }
                ]
            };
            contactDataTable = $("#contact-table").dataTable(dtoptions);

            // bind click on the first column of the row
            $('#contact-table tbody').on("click", "tr td:first-child", function (e) {
                var id = $(this).closest("tr").data("id");
                $.getJSON(App.env.applicationPath + "/api/contact", { id: id }, editContact);
                return false; // stop propagation and default behavior
            });
        }

        function initializeContactDialog() {
            var $dlg = $("#contact-dialog");
            $dlg.dialog({ autoOpen: false, resizable: true, modal: true, width: 'auto' });
            $("#contactdlg-acceptEditContact", $dlg)
                .button()
                .click(acceptEditContact);
            $("#contactdlg-cancelEditContact", $dlg)
                .button()
                .click(cancelEditContact);

            // for validation, the input fields require names
            $("form input", $dlg).each(function (index, item) {
                $(item).attr("name", item.id);
            });
        }

        function initializeContactListDialog() {
            var $dlg = $("#contactlist-dialog");
            $dlg.dialog({ autoOpen: false, resizable: true, modal: true, width: 'auto' });
            $("#cldlg-acceptEditContactList", $dlg)
                .button()
                .click(acceptEditContactList);
            $("#cldlg-cancelEditContactList", $dlg)
                .button()
                .click(cancelEditContactList);

            // for validation, the input fields require names
            $("form input", $dlg).each(function (index, item) {
                $(item).attr("name", item.id);
            });
        }

        function initializeMain() {
            $("#tabs").tabs();

            initializeContactListTable();
            initializeContactTable();

            $("#create-contactlist")
                .button()
                .click(function () {
                    editContactList(new App.ContactList());
                });

            $("#create-contact")
                .button()
                .click(function () {
                    editContact(new App.Contact());
                });
        }

        function initView() {
            // we don't need anything at this point
            //if (true) {
            initializeMain();
            initializeContactListDialog();
            initializeContactDialog();
            window.clearInterval(tid);

            reloadContactTable();
            reloadContactListTable();
            //}
        }

        //$("#debug").click(function () { debugger; });

        $.ajaxSetup({
            // Disable caching of AJAX responses
            cache: false,
            error: function (jqXHR, textStatus, errorThrown) {
                alert("ajax error " + errorThrown.toString());
            }
        });

        // get data required for app
        // save it in the viewmodel
        // when all data arrived, initialize bindings
        App.viewmodel = new App.ViewModel(jQuery);

        tid = window.setInterval(initView, 10);
    }


    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);