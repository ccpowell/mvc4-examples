// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the RTP view ProjectList.aspx

var App = App || {};

App.postit = function (url, options) {
    'use strict';
    jQuery.extend(options, {
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json'
    });

    jQuery.ajax(App.env.applicationPath + url, options);
};


App.ui = (function ($) {
    'use strict';

    var adoptCycleData = null,
        mode = null,
        modeAmend = {
            title: 'Amend Projects',
            buttonLabel: 'Amend',
            info: 'Select the projects you wish to amend in the next cycle',
            getUrl: '/operation/misc/RtpGetAmendableProjects',
            setUrl: '/operation/misc/RtpAmendProjects',
            noneMessage: 'There are no amendable projects in this RTP Plan Year.'
        },
        modeRestore = {
            title: 'Restore Projects',
            buttonLabel: 'Restore',
            info: 'Select the projects you wish to restore',
            getUrl: '/operation/misc/RtpGetAvailableRestoreProjects',
            setUrl: '/operation/misc/RtpRestoreProjects',
            noneMessage: 'There are no restorable projects in this RTP Plan Year.'
        };

    // Amend
    function amendSelectedProjects() {
        var stuff = {
            rtpPlanYearId: App.pp.RtpPlanYearId,
            projectIds: []
        },
            sstuff;

        // get list of project ids
        $('#amend-selectedProjects option').each(function (index, el) {
            stuff.projectIds.push(el.value);
        });

        sstuff = JSON.stringify(stuff, null, 2);

        App.postit(mode.setUrl, {
            data: sstuff,
            success: function (data) {
                alert('Cycle Updated');
                $('#amend-selectedProjects').empty();
            }
        });
    }

    // ajax callback 
    // fill in #amend-availableProjects
    function showAmendableProjects(data) {
        var options = '';
        $.each(data, function (index, el) {
            options += '<option value="' + el.Value + '">' + el.Text + '</option>';
        });
        if (data.length) {
            $('#amend-availableProjects').html(options);
            $('#amend-selectedProjects').empty();
            $('#amend-info').text(mode.info);
            $('#dialog-amend-projects')
                .dialog('option', 'buttons', [{
                    text: mode.buttonLabel,
                    click: amendSelectedProjects
                }, {
                    text: 'Close',
                    click: function () {
                        //$(this).dialog('close');
                        window.location.reload();
                    }
                }])
                .dialog('option', 'title', mode.title)
                .dialog('open');
        } else {
            alert(mode.noneMessage);
        }
    }

    // dialog callback. Create a project then browse to the new project location.
    function createProject(e) {
        var stuff = {
            projectName: $("#projectName").val(),
            facilityName: $("#facilityName").val(),
            plan: App.pp.RtpPlanYear,
            sponsorOrganizationId: $("#availableSponsors").val(),
            cycleid: App.pp.CurrentCycleId
        },
        sstuff;

        sstuff = JSON.stringify(stuff, null, 2);

        App.postit('/api/RtpProjectItem', {
            data: sstuff,
            success: function (data) {
                var redirectActionUrl = App.env.applicationPath
                    + '/RtpProject/' + App.pp.RtpPlanYear
                    + '/Info/' + data
                    + '?message=' + encodeURIComponent('Project created successfully.');
                alert('A new RTP Project has been created with ID ' + data);
                location.assign(redirectActionUrl);
            }
        });
    }

    // ajax callback. Show the Create Project dialog.
    function showCreateProject(data) {
        var options = '';
        $.each(data, function (index, el) {
            options += '<option value="' + el.Value + '">' + el.Text + '</option>';
        });
        $('#availableSponsors').html(options);
        $('#dialog-create-project').dialog('open');
    }

    function getProjects() {
        var stuff = {
            cycleId: App.pp.CurrentCycleId, // shouldn't be needed
            rtpPlanYearId: App.pp.RtpPlanYearId
        },
            sstuff = JSON.stringify(stuff, null, 2);

        App.postit(mode.getUrl, {
            data: sstuff,
            success: showAmendableProjects
        });
    }

    // use adoptCycleData to adopt the cycle
    function adopt() {
        var stuff = {
            rtpPlanYearId: App.pp.RtpPlanYearId,
            projectIds: []
        },
            sstuff;
        $.each(adoptCycleData, function (index, itemData) {
            stuff.projectIds.push(itemData.ProjectVersionId);
        });
        adoptCycleData = null;
        sstuff = JSON.stringify(stuff, null, 2);
        App.postit('/operation/misc/RtpAdoptProjects', {
            data: sstuff,
            success: function () {
                alert("Cycle Adopted");
                window.location.reload();
            }
        });
    }


    // ajax callback. Show the Adopt Cycle dialog.
    function showAdoptCycle(data) {
        var obj = "";
        $.each(data, function (index, itemData) {
            obj += "<li class='relative' id='row-amendable-" + itemData.ProjectVersionId + "'>"
                + itemData.SponsorAgency + " " + itemData.ProjectVersionId
                + ": " + itemData.RtpYear
                + ", " + itemData.ProjectName
                + "</li>";
        });
        $('#amend-list').html(obj);
        $('#dialog-amendpending-project').dialog('open');

        adoptCycleData = data;
    }

    // get data required for Adopt Cycle
    function getAdoptCycleData() {
        var stuff = {
            cycleId: App.pp.CurrentCycleId,
            rtpPlanYearId: App.pp.RtpPlanYearId
        },
            sstuff = JSON.stringify(stuff, null, 2);
        adoptCycleData = null;
        App.postit('/operation/misc/RtpGetAmendablePendingProjects', {
            data: sstuff,
            success: showAdoptCycle
        });
    }


    // get data required for Create Project
    function getCreateProjectData() {
        var stuff = {
            rtpPlanYearId: App.pp.RtpPlanYearId
        },
            sstuff = JSON.stringify(stuff, null, 2);

        App.postit('/operation/misc/RtpGetSponsorOrganizations', {
            data: sstuff,
            success: showCreateProject
        });
    }

    function getAmendableProjects() {
        mode = modeAmend;
        getProjects();
    }

    function getRestorableProjects() {
        mode = modeRestore;
        getProjects();
    }


    function initialize() {
        var oProjectListGrid;

        // amend dialog and actions
        $('#dialog-amend-projects').dialog({
            autoOpen: false,
            width: 900,
            height: 600,
            modal: true
        });

        $('#amend-addProject').click(function () {
            $('#amend-availableProjects option:selected').each(function (index, el) {
                $(this).remove().prependTo('#amend-selectedProjects').attr('selected', false);
            });
        });

        $('#amend-removeProject').click(function () {
            $('#amend-selectedProjects option:selected').each(function (index, el) {
                $(this).remove().prependTo('#amend-availableProjects').attr('selected', false);
            });
        });

        // create project dialog
        $('#dialog-create-project').dialog({
            autoOpen: false,
            width: 600,
            height: 400,
            modal: true,
            buttons: {
                "Create": createProject,
                "Close": function () {
                    $(this).dialog("close");
                }
            }
        });

        $('#dialog-amendpending-project').dialog({
            autoOpen: false,
            width: 900,
            height: 600,
            modal: true,
            buttons: {
                "Adopt": adopt,
                "Close": function () {
                    $(this).dialog("close");
                }
            }
        });

        // buttons
        $('#amend-projects, #include-projects')
            .button()
            .click(getAmendableProjects);

        $('#restore-projects')
            .button()
            .click(getRestorableProjects);

        $('#create-project')
            .button()
            .click(getCreateProjectData);

        $('#adopt-cycle')
            .button()
            .click(getAdoptCycleData);

        // table
        oProjectListGrid = $('#projectListGrid').dataTable({
            "iDisplayLength": 100,
            "bJQueryUI": true,
            "aaSorting": [[1, "asc"]],
            "aoColumns": [
                null,
                { sWidth: '60px' },
                { sWidth: '110px' },
                { sWidth: '60px' },
                { sWidth: '190px' },
                null,
                null,
                { "bVisible": false },
                { "bVisible": false }
            ]
        });

        // initial message - make it go away
        window.setTimeout(function () {
            $("div#message").fadeOut("slow", function () {
                $("div#message").empty().removeClass().removeAttr('style');
            });
        }, 5000);
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);