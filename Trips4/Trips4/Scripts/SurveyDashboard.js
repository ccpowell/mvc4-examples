// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the Survey view Dashboard.aspx

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

    // Amend
    function amendSelectedProjects() {
        var stuff = {
            surveyId: App.pp.SurveyId,
            projectIds: []
        },
            sstuff;

        // get list of project ids
        $('#amend-selectedProjects option').each(function (index, el) {
            stuff.projectIds.push(el.value);
        });

        sstuff = JSON.stringify(stuff, null, 2);

        App.postit('/operation/misc/SurveyAmendProjects', {
            data: sstuff,
            success: function (data) {
                alert('Survey Updated');
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
            $('#amend-info').text('Select projects to include');
            $('#dialog-amend-projects')
                .dialog('open');
        } else {
            alert('No projects are available to include');
        }
    }

    function getAmendableProjects() {
        App.postit('/operation/misc/SurveyGetAmendableProjects', {
            success: showAmendableProjects
        });
        return false;
    }

    function initialize() {
        // amend dialog and actions
        $('#dialog-amend-projects').dialog({
            autoOpen: false,
            width: 900,
            height: 700,
            modal: true,
            title: 'Include More Projects',
            buttons: [{
                text: 'Include',
                click: amendSelectedProjects
            }, {
                text: 'Close',
                click: function () {
                    window.location.reload();
                }
            }]
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

        // input control that starts it
        $('#btn-includemore')
            .click(getAmendableProjects);
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);