// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false, App: false, $: false */

// This is the script for the TIP Project Amendments page
// N.B. this is the list of amendments for a single project, not for the whole TIP year.

$(document).ready(function () {
    'use strict';

    function add1Agency(id) {
        App.postit("/Operation/TipProjectOperation/AddCurrent1Agency", {
            data: JSON.stringify({ AgencyId: id, ProjectVersionId: App.pp.ProjectVersionId }),
            success: function () {
                var selector = "#AvailableAgencies option[value='" + id + "']";
                var sponsorId = $('#PrimarySponsorId');
                var oldPrimarySponsor = $('#PrimarySponsor').html();

                $('#AvailableAgencies').prepend('<option value="' + sponsorId.val() + '">' + oldPrimarySponsor + '</option>');
                $('#PrimarySponsor').html($(selector).text());
                $(selector).remove();
                sponsorId.val(id);
                $('#AvailableAgencies option').sort(function (a, b) {
                    return $(a).text() > $(b).text() ? 1 : -1;
                });
            }
        });
    }

    function add2Agency(id) {
        App.postit("/Operation/TipProjectOperation/AddCurrent2Agency", {
            data: JSON.stringify({ AgencyId: id, ProjectVersionId: App.pp.ProjectVersionId }),
            success: function () {
                var selector = "#AvailableAgencies option[value='" + id + "']";
                $(selector).remove().appendTo('#Current2Agencies');
            }
        });
    }

    function remove2Agency(id) {
        App.postit("/Operation/TipProjectOperation/DropCurrent2Agency", {
            data: JSON.stringify({ AgencyId: id, ProjectVersionId: App.pp.ProjectVersionId }),
            success: function () {
                //success
                var selector = "#Current2Agencies option[value='" + id + "']";
                var selector2 = "#AvailableAgencies";
                $(selector).remove().prependTo('#AvailableAgencies');
                $('#AvailableAgencies option').sort(function (a, b) {
                    return $(a).text() > $(b).text() ? 1 : -1;
                });
            }
        });
    }

    function ShowMessageDialog(title, message) {
        $('#dialog').dialog('option', 'title', title);
        $('#dialogMessage').html(message);
        $('#dialog').dialog('open');
    }

    //$('#AvailableAgencies').removeAttr('multiple');
    //$('#Current2Agencies').removeAttr('multiple');

    $(".growable").growing({ buffer: 5 });
    //        var ppy1options = {
    //            caption: true,
    //            navigation: 'hover',
    //            direction: 'left'
    //        }
    //        $('#ppy1').popeye(ppy1options);

    // Prevent accidental navigation away
    if (App.pp.isEditable) {
        App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
        $('#submitForm').button({ disabled: true });
    }

    $('#add1').click(function () {
        $('#AvailableAgencies option:selected').each(function (i) {
            //make callback add to Eligible Agency list
            add1Agency($(this).val());
        });
        return false;
    });

    $('#add2').click(function () {
        $('#AvailableAgencies option:selected').each(function (i) {
            //make callback add to Eligible Agency list
            add2Agency($(this).val());
        });
        return false;
    });

    $('#remove2').click(function () {
        $('#Current2Agencies option:selected').each(function (i) {
            remove2Agency($(this).val());
        });
        return false;
    });


    //Initialize the dialog
    $("#dialog").dialog({
        autoOpen: false,
        draggable: false,
        bgiframe: true,
        modal: true,
        buttons: {
            Ok: function () {
                $(this).dialog('close');
            }
        }
    });

    $('#SponsorId').change(function () {
        var sponsorId = $('#SponsorId').val();
        $.getJSON(App.pp.UpdateAvailableSponsorContacts + sponsorId, null, function (data) {
            $('#SponsorContactId').fillSelect(data);
        });
    });

    var tabinforightheight = $("#tab-info-right").height();
    var uploadwrapperheight = $("#uploadwrapper").height();

    $("#uploadwrapper").css('position', 'absolute');
    $("#uploadwrapper").css('top', tabinforightheight + 40);
    $("#uploadwrapper").css('right', '0px');

    var pageheight = $(".page").height();
    var pagesizeleft = (pageheight - (tabinforightheight + uploadwrapperheight + 40 + 193));

    if (pagesizeleft < 0) {
        // need to grow the size of the page
        $(".page").height(pageheight + 40);
    }

    $('#InfoModel_ImprovementTypeId').bind('change', function () {
        var improvementtypeid = $('#InfoModel_ImprovementTypeId :selected').val();
        improvementtypeid = parseInt(improvementtypeid);

        $.ajax({
            type: "POST",
            url: App.pp.GetImprovementTypeMatch,
            data: "id=" + improvementtypeid,
            dataType: "json",
            success: function (response) {
                $('#InfoModel_ProjectTypeId').val(response.id);
            }
        });
    });

    $('#InfoModel_ProjectTypeId').bind('change', function () {
        var projecttypeid = $('#InfoModel_ProjectTypeId :selected').val();
        var parsed = parseInt(projecttypeid);
        projecttypeid = !isNaN(parsed) ? parsed : 0;
        $.ajax({
            type: "POST",
            url: App.pp.GetProjectTypeMatch,
            data: "id=" + projecttypeid,
            dataType: "json",
            success: function (response) {
                $('#InfoModel_ImprovementTypeId').empty();
                $('#InfoModel_ImprovementTypeId')
                        .fillSelect(response.data, { 'defaultOptionText': '-- Select Improvement Type --' })
                        .sortOptions();
            }
        });
    });
});