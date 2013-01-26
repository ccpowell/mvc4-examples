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

    // This object will save the items and match on the named field.
    function Autocompleter(field) {
        var self = this;
        this.items = [];

        // jui autocomplete callback
        this.autocomplete = function (request, response) {
            $.ajax(App.env.applicationPath + "/operation/misc/GetAutoCompleteContact", {
                data: { prefix: request.term, field: field },
                success: function (data) {
                    self.items = data;
                    var names = [];
                    $.each(data, function (index, item) {
                        names.push(item[field]);
                    });
                    response(names);
                }
            });
        };

        // return the item that matches
        this.isOnList = function (term) {
            var isOn = null;
            $.each(self.items, function (index, item) {
                if (item[field] === term) {
                    isOn = item;
                    return false;
                }
            });
            return isOn;
        };
    }

    return {
        Autocompleter: Autocompleter,
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
    var contactListDataTable, contactDataTable, sclDataTable, sclId;

    function reloadContactListTable() {
        var url = App.env.applicationPath + "/api/contactlist";
        contactListDataTable.fnClearTable();
        $.getJSON(url, function (data) {
            contactListDataTable.fnAddData(data, true);
        });
    }

    function reloadSclTable() {
        var url = App.env.applicationPath + "/api/contactlist";
        sclDataTable.fnClearTable();
        $.getJSON(url, { id: sclId }, function (data) {
            sclDataTable.fnAddData(data.Contacts, true);
        });
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

    // edit or create ContactList
    function editContactList(clist) {
        App.viewmodel.setEditContactList(clist);
        $("#contactlist-dialog").dialog("open").find("form").validate().resetForm();
        return false;
    }

    // edit or create Contact
    function editContact(contact) {
        App.viewmodel.setEditContact(contact);
        $("#contact-dialog").dialog("open").find("form").validate().resetForm();
        return false;
    }

    // add a tab to view this ContactList
    function viewContactList(clist) {
        sclId = clist.Id;
        $("#scl-title").text(clist.Name);
        $("#tabs").tabs({ disabled: [] }).tabs({ active: 2 });
        reloadSclTable();
    }

    function addContacts() {
        $("#addcontacts-dialog").dialog("open");
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
                    // attach ContactList to row for callbacks to use
                    $row.data("clist", aData);
                },

                aoColumns: [
                    { sTitle: "Name", mData: "Name" },
                    { sTitle: "Operations", mData: "Name", bSortable: false, mRender: function (field, op, oData) {
                        return '<span class="table-button">View Members</span';
                    }
                    }
                ]
            };
            contactListDataTable = $("#contactlist-table").dataTable(dtoptions);

            // operation buttons 
            $('#contactlist-table tbody').on("click", "tr span.table-button", function (e) {
                var clist = $(this).closest("tr").data("clist");
                viewContactList(clist);
                return false; // stop propagation and default behavior
            });
        }

        function initializeContactTable() {
            var dtoptions = {
                bStateSave: false,
                bServerSide: false,
                bProcessing: true,
                bFilter: true,
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


        function initializeSclTable() {
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
            sclDataTable = $("#scl-table").dataTable(dtoptions);

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
            $("#contactdlg-accept", $dlg)
                .button()
                .click(acceptEditContact);
            $("#contactdlg-cancel", $dlg)
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
            $("#cldlg-accept", $dlg)
                .button()
                .click(acceptEditContactList);
            $("#cldlg-cancel", $dlg)
                .button()
                .click(cancelEditContactList);

            // for validation, the input fields require names
            $("form input", $dlg).each(function (index, item) {
                $(item).attr("name", item.id);
            });
        }

        function initializeAddContactsDialog() {
            var $dlg = $("#addcontacts-dialog"),
                $uname = $("#addcontacts-UserName", $dlg),
                $email = $("#addcontacts-Email", $dlg),
                $org = $("#addcontacts-Organization", $dlg),
                $addUserName = $("#addcontacts-add-UserName", $dlg),
                $addEmail = $("#addcontacts-add-Email", $dlg),
                acEmail = new App.utility.Autocompleter("Email"),
                acUserName = new App.utility.Autocompleter("UserName");

            function fillInFields(contact, which) {
                var hasContact = !!contact;
                if (!contact) {
                    contact = new App.Contact();
                }
                $email.val(contact.Email);
                $uname.val(contact.UserName);
                $org.text(contact.Organization);
                $addUserName.button((hasContact && which === "UserName") ? "enable" : "disable");
                $addEmail.button((hasContact && which === "Email") ? "enable" : "disable");
            }

            $dlg.dialog({
                autoOpen: false,
                resizable: true,
                modal: true,
                width: 'auto',
                open: function () { fillInFields(null, null); },
                close: function () { fillInFields(null, null); }
            });


            // autocomplete fields fill in values for all fields
            $uname.autocomplete({
                source: acUserName.autocomplete,
                minLength: 3,
                select: function (event, ui) {
                    var contact = acUserName.isOnList(ui.item.value);
                    fillInFields(contact, "UserName");
                    return false;
                }
            });

            $email.autocomplete({
                source: acEmail.autocomplete,
                minLength: 3,
                select: function (event, ui) {
                    var contact = acEmail.isOnList(ui.item.value);
                    fillInFields(contact, "Email");
                    return false;
                }
            });

            function contactAdded() {
                fillInFields(null);
                alert("Contact added");
            }

            $addUserName
                .button()
                .click(function () {
                    var contact = acUserName.isOnList($uname.val());
                    if (contact !== null) {
                        $.post(App.env.applicationPath + "/operation/misc/AddContact",
                            { contactlist: sclId, contact: contact.Id },
                            contactAdded);
                    }
                    return false;
                });

            $addEmail
                .button()
                .click(function () {
                    var contact = acEmail.isOnList($email.val());
                    if (contact !== null) {
                        $.post(App.env.applicationPath + "/operation/misc/AddContact",
                            { contactlist: sclId, contact: contact.Id },
                            contactAdded);
                    }
                    return false;
                });

            $("#addcontacts-cancel", $dlg)
                .button()
                .click(function () {
                    fillInFields(null);
                    $dlg.dialog("close");
                    reloadSclTable();
                    return false;
                });
        }

        function initializeMain() {
            $("#tabs").tabs({ disabled: [2] });

            initializeContactListTable();
            initializeContactTable();
            initializeSclTable();

            $("#create-contactlist")
                .button()
                .click(function () {
                    editContactList(new App.ContactList());
                    return false;
                });

            $("#create-contact")
                .button()
                .click(function () {
                    editContact(new App.Contact());
                    return false;
                });

            $("#scl-add-contacts")
                .button()
                .click(addContacts);
        }

        function initView() {
            // we don't need anything at this point
            //if (true) {
            initializeMain();
            initializeContactListDialog();
            initializeContactDialog();
            initializeAddContactsDialog();
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