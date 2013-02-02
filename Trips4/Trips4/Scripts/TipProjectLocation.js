// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the Login default page Index

var App = App || {};

App.ui = (function ($) {
    'use strict';
    var AddCountyUrl = '/Trips/Operation/TipProjectOperation/AddCountyShare',
        DropCountyUrl = '/Trips/Operation/TipProjectOperation/RemoveCountyShare',
        AddMuniUrl = '/Trips/Operation/TipProjectOperation/AddMuniShare',
        DropMuniUrl = '/Trips/Operation/TipProjectOperation/RemoveMuniShare';

    function updateCountyShareTotal() {
        var county_share_total = 0;
        var allocated_share = 0;
        var countyEditor = $("tr#county-editor");
        //Sum the shares
        $('input[id^=cshare], td span[id^=cshare]').each(function () {
            var val = $(this).val();
            if (val === '') {
                // try get the text version
                val = $(this).text();
                allocated_share += parseInt(val);
            }
            var parsed = parseInt(val);

            county_share_total += !isNaN(parsed) ? parsed : 0;
        });
        $('#county-share-sum').html(county_share_total);
        //Enable Add button...
        var addButton = $('#add-county');
        //alert(parseInt($('#cshare_new').val()) + " : " + $('#new_county option:selected').val());
        if (county_share_total <= 100 && parseInt($('#cshare_new').val()) > 0 && $('#new_county option:selected').val() != '') {
            addButton.removeClass('ui-state-disabled').removeAttr('disabled');
        } else {

            //add disabled class if it does not exist
            if (!addButton.hasClass('ui-state-disabled')) {
                addButton.addClass('ui-state-disabled').attr('disabled', 'disabled');
            }
        }

        if (allocated_share == 100) {
            countyEditor.hide();
        } else {
            countyEditor.show();
        }
    }

    function updateMuniShareTotal() {
        var muni_share_total = 0;
        var allocated_share = 0
        var muniEditor = $("tr#muni-editor");

        //Sum the shares
        $('input[id^=mshare], td span[id^=mshare]').each(function () {
            var val = $(this).val();
            if (val === '') {
                // try get the text version
                val = $(this).text();
                allocated_share += parseInt(val);
            }
            var parsed = parseInt(val);

            muni_share_total += !isNaN(parsed) ? parsed : 0;
        });
        $('#muni-share-sum').html(muni_share_total);

        //Enable Add button...
        var addButton = $('#add-muni');
        if (muni_share_total <= 100 && $('#mshare_new').val() != 0 && $('#new_muni option:selected').val() != '') {
            addButton.removeClass('ui-state-disabled').removeAttr('disabled');
        } else {
            //add disabled class if it does not exist
            if (!addButton.hasClass('ui-state-disabled')) {
                addButton.addClass('ui-state-disabled').attr('disabled', 'disabled');
            }
        }

        if (allocated_share == 100) {
            muniEditor.hide();
        } else {
            muniEditor.show();
        }
    }


    function initialize() {
        App.tabs.initializeTipProjectTabs();

        $(".growable").growing({ buffer: 5 });

        var ppy1options = {
            caption: true,
            navigation: 'hover',
            direction: 'left'
        };
        $('#ppy1').popeye(ppy1options);

        // Prevent accidental navigation away
        App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
        $('#submitForm').button({ disabled: true });

        // Delete location map image
        $('#delete-image').button().click(function () {
            var recordid = $(this).data('imageid'),
                url = '/Operation/TipProjectOperation/DeleteLocationMap',
                stuff = { imageId: recordid, projectVersionId: App.pp.ProjectVersionId };
            App.postit(url, {
                data: JSON.stringify(stuff),
                success: function () {
                    window.location.reload();
                }
            });
            return false;
        });

        var top_fullwidth_height = $("#top-fullwidth").height();
        //        var uploadwrapperheight = $("#uploadwrapper").height();

        $("#uploadwrapper").css('position', 'absolute');
        $("#uploadwrapper").css('right', '0px');
        $("#uploadwrapper").css('top', top_fullwidth_height + 40);

        $('.delete-county').live("click", function () {
            //get the countyId and project id
            var pid = $('#ProjectId').val(),
                ctyId = $(this).data("id"),
                name = $(this).data("name"),
                stuff = {
                    ProjectId: pid,
                    CountyId: ctyId
                };
            $('#county_row_' + ctyId).remove();
            App.postit(DropCountyUrl, {
                data: JSON.stringify(stuff),
                success: function () {
                    $('#result').html("County removed");
                    $('#new_county').addOption(ctyId, name).sortOptions();
                    window.onbeforeunload = null;
                    updateCountyShareTotal();
                }
            });
            return false;
        });

        $('.delete-muni').live("click", function () {
            //get the countyId and project id
            var pid = $('#ProjectId').val(),
                muniId = $(this).data("id"),
                name = $(this).data("name"),
                stuff = {
                    MunicipalityId: muniId,
                    ProjectId: pid
                };
            $('#muni_row_' + muniId).remove();
            App.postit(DropMuniUrl, {
                data: JSON.stringify(stuff),
                success: function () {
                    $('#result').html("Municipality dropped");
                    $('#new_muni').addOption(muniId, name).sortOptions();
                    updateMuniShareTotal();
                }
            });
            return false;
        });

        updateCountyShareTotal();
        updateMuniShareTotal();

        //Hook in the keyup event so we can keep track of changes to the shares
        $('input[id^=cshare]').live('keyup', function () { updateCountyShareTotal(); });
        $('input[id^=mshare]').live('keyup', function () { updateMuniShareTotal(); });
        $('#new_muni').bind('change', function () { updateMuniShareTotal(); });
        $('#new_county').bind('change', function () { updateCountyShareTotal(); });

        //Add a county to the list
        $('#add-county').click(function () {
            //grab the values from the active form
            var share = $('#cshare_new').val()
                , primary = $('#new_primary').prop('checked')
                , countyId = $('#new_county option:selected').val()
                , countyName = $('#new_county option:selected').text()
                , pid = $('#ProjectId').val()
                , stuff = {
                    ProjectId: pid,
                    CountyId: countyId,
                    Share: share,
                    Primary: primary
                };

            //Remove selected option to ensure that we can't re-add it.
            //this leaves a hole, in that you could add, then remove, 
            //then want to re-add, and it would not be in the list.
            $('#new_county option:selected').remove();

            //reset the new share value to 0
            $('#cshare_new').val(0);
            $('#new_primary').prop('checked', false);

            //if primary is checked, see if primary is checked elsewhere, and warn user

            //Do we try to see if the county is already listed?

            App.postit(AddCountyUrl, {
                data: JSON.stringify(stuff),
                success: function (response) {
                    $('#result').html("County added.");

                    //Disable the add button
                    $('#add-county').addClass('ui-state-disabled').attr('disabled', 'disabled');

                    //Add into the DOM
                    var content = "<tr id='county_row_" + countyId + "'>";
                    if (primary == true) {
                        content += "<td><input id='" + countyId + "_isPrimary' type='checkbox'  name='cty_" + countyId + "_IsPrimary' checked='checked'/></td>";
                    } else {
                        content += "<td><input id='" + countyId + "_isPrimary' type='checkbox'  name='cty_" + countyId + "_IsPrimary' /></td>";
                    }
                    content += "<td><span id='cshare_" + countyId + "'>" + share + "</span>%</td>";
                    content += "<td>" + countyName + "</td>";
                    content += "<td><button class='delete-county fg-button ui-state-default ui-priority-primary ui-corner-all' name='"
                        + countyName + "' id='delete_" + countyId + "'>Delete</button></td></tr>";
                    $('#county-editor').before(content);

                    updateCountyShareTotal();
                    window.onbeforeunload = null;
                }
            });
            return false;
        });

        //Add a county to the list
        $('#add-muni').click(function () {
            //grab the values from the active form
            var share = $('#mshare_new').val()
                , primary = $('#new_muni_primary').prop('checked')
                , muniId = $('#new_muni option:selected').val()
                , muniName = $('#new_muni option:selected').text()
                , pid = $('#ProjectId').val()
                , stuff = {
                    MunicipalityId: muniId,
                    ProjectId: pid,
                    Share: share,
                    Primary: primary
                };

            //Remove selected option to ensure that we can't re-add it.
            //this leaves a hole, in that you could add, then remove, 
            //then want to re-add, and it would not be in the list.
            $('#new_muni option:selected').remove();

            //reset the new share value to 0
            $('#mshare_new').val(0);
            $('#new_muni_primary').prop('checked', false);

            //if primary is checked, see if primary is checked elsewhere, and warn user

            //Do we try to see if the county is already listed?

            App.postit(AddMuniUrl, {
                data: JSON.stringify(stuff),
                success: function () {
                    $('#result').html("Municipality added");
                    $('#add-muni').addClass('ui-state-disabled').attr('disabled', 'disabled');

                    //Add into the DOM
                    var content = "<tr id='muni_row_" + muniId + "'>";
                    if (primary == true) {
                        content += "<td><input id='" + muniId + "_isPrimary' type='checkbox'  name='muni_" + muniId + "_IsPrimary' checked='checked'/></td>";
                    } else {
                        content += "<td><input id='" + muniId + "_isPrimary' type='checkbox'  name='muni_" + muniId + "_IsPrimary' /></td>";
                    }
                    content += "<td><span id='mshare_" + muniId + "'>" + share + "</span>%</td>";
                    content += "<td>" + muniName + "</td>";
                    content += "<td><button class='delete-muni fg-button ui-state-default ui-priority-primary ui-corner-all' name='" + muniName + "' id='delete_" + muniId + "'>Delete</button></td></tr>";
                    $('#muni-editor').before(content);

                    updateMuniShareTotal();
                    window.onbeforeunload = null;
                }
            });
            return false;
        });
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);