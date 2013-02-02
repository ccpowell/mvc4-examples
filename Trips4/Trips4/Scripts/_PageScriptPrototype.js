// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

// Template for a page script

var App = App || {};

App.ui = (function ($) {
    'use strict';
    
    function initialize() {
    }

    return {
        initialize: initialize
    };
} (jQuery));

jQuery(document).ready(App.ui.initialize);