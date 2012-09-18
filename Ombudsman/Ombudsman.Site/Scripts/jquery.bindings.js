/// <reference path="jquery-1.8.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.23.js" />
/// <reference path="knockout-2.1.0.debug.js" />

// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 50, unparam: true, indent: 4 */
/*global jQuery: false, ko: false */

// NOTE: jquery ui and knockout do not play well together unless custom bindings are used,
// so we provide them here.

(function ($) {
    "use strict";

    ko.bindingHandlers.jqpushbutton = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            var jel = $(element);
            jel.button();
        }
    };

    ko.bindingHandlers.jqtoggle = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            var jel = $(element);
            jel.button().click(function (event) {
                var yesno = jel.prop("checked");
                valueAccessor()(yesno);
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called once when the binding is first applied to an element,
            // and again whenever the associated observable changes value.
            // Update the DOM element based on the supplied values here.
            var jel = $(element),
                yesno = ko.utils.unwrapObservable(valueAccessor());
            jel.prop("checked", yesno).button("refresh");
        }
    };

    ko.bindingHandlers.jqcheckbutton = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            var jel = $(element),
                allBindings = allBindingsAccessor(),
                options = {};

            // Grab some more data from another binding property
            if (allBindings.jqcheckbuttonOptions) {
                $.extend(options, allBindings.jqcheckbuttonOptions);
            }

            jel.button(options).click(function (event) {
                var yesno = jel.prop("checked");
                if (yesno) {
                    jel.button("option", "icons", { primary: 'ui-icon-circle-check' });
                }
                else {
                    jel.button("option", "icons", { primary: 'ui-icon-circle-close' });
                }
                valueAccessor()(yesno);
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called once when the binding is first applied to an element,
            // and again whenever the associated observable changes value.
            // Update the DOM element based on the supplied values here.
            var jel = $(element),
                yesno = ko.utils.unwrapObservable(valueAccessor());
            if (yesno) {
                jel.button("option", "icons", { primary: 'ui-icon-circle-check' });
            }
            else {
                jel.button("option", "icons", { primary: 'ui-icon-circle-close' });
            }
            jel.prop("checked", yesno).button("refresh");
        }
    };

    ko.bindingHandlers.jqradiobutton = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            // When the button is clicked on, set the target value
            // to this button's value
            var jel = $(element);
            jel.button().click(function (event) {
                var yesno = jel.prop("checked"),
                    icon = 'ui-icon-radio-off';
                if (yesno) {
                    icon = 'ui-icon-circle-check';
                    valueAccessor()(jel.val());
                }
                jel.button("option", "icons", { primary: icon });
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called once when the binding is first applied to an element,
            // and again whenever the associated observable changes value.
            // Update the DOM element based on the supplied values here.
            // If the observable value === this button value, check it
            var jel = $(element),
                yesno = ko.utils.unwrapObservable(valueAccessor()) === jel.val(),
                icon = 'ui-icon-radio-off';
            if (yesno) {
                icon = 'ui-icon-circle-check';
            }

            jel.prop("checked", yesno)
                .button("refresh")
                .button("option", "icons", { primary: icon });
        }
    };

    ko.bindingHandlers.jqdatepicker = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            var options,
                jel = $(element),
                value = valueAccessor(),
                allBindings = allBindingsAccessor(),
                valueUnwrapped = ko.utils.unwrapObservable(value);

            options = {
                onClose: function () {
                    var date = jel.datepicker("getDate");
                    valueAccessor()(date);
                }
            };

            // Grab some more data from another binding property
            if (allBindings.jqdatepickerOptions) {
                $.extend(options, allBindings.jqdatepickerOptions);
            }

            // use data value
            if (valueUnwrapped) {
                options.defaultDate = valueUnwrapped;
            }

            jel.datepicker(options);
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called once when the binding is first applied to an element,
            // and again whenever the associated observable changes value.
            // Update the DOM element based on the supplied values here.
            var date = ko.utils.unwrapObservable(valueAccessor());
            $(element).datepicker("setDate", date);
        }
    };

    ko.bindingHandlers.jqautocomplete = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here

            var jel = $(element),
                allBindings = allBindingsAccessor(),
                options = {};

            // Grab some more data from another binding property
            if (allBindings.jqautocompleteOptions) {
                $.extend(options, allBindings.jqautocompleteOptions);
            }

            jel.autocomplete(options);
        }
    };


    ko.bindingHandlers.jqdialog = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            // This will be called when the binding is first applied to an element
            // Set up any initial state, event handlers, etc. here
            var options = {},
                jel = $(element),
                allBindings = allBindingsAccessor();

            options = {
                close: function () {
                    valueAccessor()(false);
                }
            };

            // Grab some more data from another binding property
            if (allBindings.jqdialogOptions) {
                $.extend(options, allBindings.jqdialogOptions);
                // TODO: handle close conflicts
            }

            jel.dialog(options);
        },

        update: function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
            // This will be called once when the binding is first applied to an element,
            // and again whenever the associated observable changes value.
            // Update the DOM element based on the supplied values here.
            var isOpen = ko.utils.unwrapObservable(valueAccessor());
            if (isOpen) {
                //ko.applyBindingsToDescendants(bindingContext, element);
                $(element).dialog("open");
            } else {
                $(element).dialog("close");
            }
        }
    };

} (jQuery));