// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the RTP view Projects.aspx

var App = App || {};

App.pp = {
    CurrentCycleId: 22,
    PreviousCycleId: 19,
    NextCycleId: 0,
    RtpPlanYear: '2035-S',
    RtpPlanYearId: 78,
    CurrentCycleName: '2011-1',
    NextCycleName: ''
};

App.postit = function (url, options) {
    'use strict';
    $.extend(options, {
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json'
    });

    $.ajax(App.env.applicationPath + url, options);
};


App.ui = (function ($) {
    'use strict';

    var mode = null,
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
            $('#amend-removeProjects').html('');
            $('#amend-info').text(mode.info);
            $('#dialog-amend-projects')
                .dialog('option', 'buttons', [{
                    text: mode.buttonLabel,
                    click: amendSelectedProjects
                }, {
                    text: 'Close', 
                    click: function () {
                        $(this).dialog('close');
                    }
                }])
                .dialog('option', 'title', mode.title)
                .dialog('open');
        } else {
            alert(mode.noneMessage);
        }
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

    function getAmendableProjects() {
        mode = modeAmend;
        getProjects();
    }

    function getRestorableProjects() {
        mode = modeRestore;
        getProjects();
    }


    function initialize() {
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

        $('#amend-projects, #include-projects')
            .button()
            .click(getAmendableProjects);

        $('#restore-projects')
            .button()
            .click(getRestorableProjects);
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);