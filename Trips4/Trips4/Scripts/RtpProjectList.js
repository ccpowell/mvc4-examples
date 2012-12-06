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


App.ui = (function ($) {
    'use strict';

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

        $.ajax(App.env.applicationPath + '/operation/misc/RtpAmendProjects', {
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: sstuff,
            success: function (data) {
                alert('Projects amended');
                $('#amend-selectedProjects').empty();
            },
            error: function () {
                alert('bummer');
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
        $('#amend-availableProjects').html(options);
        if (data.length) {
            $('#dialog-amend-project').dialog('open');
        } else {
            alert('There are no amendable projects in this RTP Plan Year.');
        }
    }

    function getAmendableProjects() {
        var stuff = {
            cycleId: App.pp.CurrentCycleId, // shouldn't be needed
            rtpPlanYearId: App.pp.RtpPlanYearId
        },
            sstuff = JSON.stringify(stuff, null, 2);

        $.ajax(App.env.applicationPath + '/operation/misc/RtpGetAmendableProjects', {
            type: 'POST',
            contentType: 'application/json',
            dataType: 'json',
            data: sstuff,
            success: showAmendableProjects,
            error: function () {
                alert('bummer');
            }
        });
    }

    function amendProjects() {
        getAmendableProjects();
    }

    function initialize() {
        $('#dialog-amend-project').dialog({
            autoOpen: false,
            width: 900,
            height: 600,
            modal: true,
            buttons: {
                'Amend': function () {
                    amendSelectedProjects();
                },
                'Close': function () {
                    $(this).dialog('close');
                }
            }
        });

        $('#amend-projects')
            .button()
            .click(amendProjects);

        $('#amend-addProject').click(function () {
            $('#amend-availableProjects option:selected').each(function (index, el) {
                $(this).remove().prependTo('#amend-selectedProjects').attr('selected', false);
            });
        });


        $('#amend-removeProject').click(function () {
            $('amend-selectedProjects option:selected').each(function (index, el) {
                $(this).remove().prependTo('#amend-availableProjects').attr('selected', false);
            });
        });

    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);