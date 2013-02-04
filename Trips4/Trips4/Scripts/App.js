// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This script defines some common functions.
// Since it is included in Site.Master, these functions are universal.

var App = App || {};

// POST an operation using JSON.
// The data should be serialized using JSON.stringify.
// NOTE: ajax(url, options) doesn't work with jquery 1.4
App.postit = function (url, options) {
    'use strict';

    // extend options
    var poptions = jQuery.extend({}, {
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json'
    }, options);

    // add application path if needed
    if (url.toLowerCase().indexOf(App.env.applicationPath.toLowerCase()) !== 0) {
        url = App.env.applicationPath + url;
    }
    poptions.url = url;

    // send it along
    jQuery.ajax(poptions);
};

App.utility = (function ($) {
    "use strict";

    // Prevent accidental navigation away
    function bindInputToConfirmUnload(form, button, result) {
        var $form = $(form),
            $button = $(button),
            $result = $(result),
            url = $form.attr("action");

        function setConfirmUnload() {
            $button.button("enable");
            $result.empty().hide();
            window.onbeforeunload = function unloadMessage() {
                return 'You have entered new data on this page. If you navigate away from this page without first saving your data, the changes will be lost.';
            };
        }

        // Bind inputs to enable the submit button
        $(':input', $form)
            .bind("change", setConfirmUnload)
            .bind("keyup", setConfirmUnload);
        $(':input.nobind', $form)
            .unbind("change")
            .unbind("keyup");

        // fix the URL
        if (url.indexOf(App.env.applicationPath) !== 0) {
            url = App.env.applicationPath + url;
        }
        //alert("submitting to " + url);

        //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
        $form.validate({
            submitHandler: function (form) {
                $button.button("disable").blur().removeClass('ui-state-hover');
                $form.ajaxSubmit({
                    url: url,
                    success: function (response) {
                        $result.html(response || "Your changes have been saved.").addClass("success").removeClass("error").show();
                        window.onbeforeunload = null;
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $result.text(errorThrown.toString()).addClass('error').removeClass("success").show();
                        $button.button("enable");
                    },
                    dataType: 'json'
                });
            }
        });
    }

    function parseBoolean(string) {
        if (string) {
            switch (string.toLowerCase()) {
                case "true": case "yes": case "1": return true;
                default: return false;
            }
        }
        return false;
    }


    return {
        bindInputToConfirmUnload: bindInputToConfirmUnload,
        parseBoolean: parseBoolean
    };
} (jQuery));

App.tabs = (function ($) {
    "use strict";

    function initializeTabs(makeUrl) {
        var activeIndex = 0;

        // find the index of the list item with the action
        $("#page-tabs-list > li").each(function (index, el) {
            if ($(el).data("action") === App.env.action) {
                activeIndex = index;
                return false;
            }
        });

        $("#page-tabs").tabs({
            active: activeIndex,
            beforeActivate: function (event, ui) {
                var action = ui.newTab.data("action"), url;
                if (action) {
                    url = makeUrl(action);
                    if (url) {
                        window.location.assign(url);
                    }
                }
                return true;
            }
        });
    }


    function initializeTipProjectTabs() {
        initializeTabs(function (action) {
            var segments = [App.env.applicationPath, "Project", App.routeData.year, action, App.routeData.id];
            return segments.join('/');
        });
    }

    function initializeTipTabs() {
        initializeTabs(function (action) {
            var segments = [App.env.applicationPath, "TIP", App.routeData.year, action];
            return segments.join('/');
        });
    }

    function initializeRtpTabs() {
        initializeTabs(function (action) {
            var segments = [App.env.applicationPath, "RTP", App.routeData.year, action];
            return segments.join('/');
        });
    }

    function initializeRtpProjectTabs() {
        initializeTabs(function (action) {
            var segments = [App.env.applicationPath, "RtpProject", App.routeData.year, action, App.routeData.id];
            return segments.join('/');
        });
    }


    function initializeSurveyTabs() {
        initializeTabs(function (action) {
            var segments = [App.env.applicationPath, "Survey", App.routeData.year, action];
            return segments.join('/');
        });
    }

    function initializeSurveyProjectTabs() {
        initializeTabs(function (action) {
            var segments = [App.env.applicationPath, "Survey", App.routeData.year, action, App.routeData.id];
            return segments.join('/');
        });
    }


    return {
        initializeTipProjectTabs: initializeTipProjectTabs,
        initializeTipTabs: initializeTipTabs,
        initializeRtpProjectTabs: initializeRtpProjectTabs,
        initializeRtpTabs: initializeRtpTabs,
        initializeSurveyProjectTabs: initializeSurveyProjectTabs,
        initializeSurveyTabs: initializeSurveyTabs
    };
} (jQuery));