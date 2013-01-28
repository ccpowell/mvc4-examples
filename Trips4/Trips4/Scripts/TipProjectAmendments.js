// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// This is the script for the TIP Project Amendments page
// N.B. this is the list of amendments for a single project, not for the whole TIP year.

var App = App || {};



App.ui = (function ($) {
    'use strict';


    function initialize() {
        alert("initialize");
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);