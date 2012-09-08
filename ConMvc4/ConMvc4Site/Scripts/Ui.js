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

    function initializePublicLists() {
    }
    function initializeOwnedLists() {
    }
    function initializeAllContacts() {
        var ajaxsourceurl = "/page/getusers",
            oTable;

        oTable = $('#all-users-table').dataTable({
            "bStateSave": true,
            "bServerSide": true,
            "iTotalRecords": 50,
            "iTotalDisplayRecords": 50,
            "iDisplayLength": 50,
            "sAjaxSource": ajaxsourceurl,
            "bProcessing": true,
            "aoColumns": [
                { sTitle: "ID", mDataProp: "Id" },
                { sTitle: "Name", mDataProp: "UserName" },
                { sTitle: "Organization", mDataProp: "Organization", sWidth: "200px" },
                { sTitle: "Title", mDataProp: "Title", "sWidth": "200px", bSortable: false },
                { sTitle: "Primary Contact", mDataProp: "Phone", "sWidth": "100px", "sClass": "phone-US", bSortable: false },
                { sTitle: "Email", bSortable: false, mDataProp: "RecoveryEmail",
                    fnRender: function (oObj) {
                        return '<a href="mailto:' + oObj.aData.RecoveryEmail + '">' + oObj.aData.RecoveryEmail + '</a>';
                    }
                }
            ]
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

        App.viewmodel = new App.ViewModel($);
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
