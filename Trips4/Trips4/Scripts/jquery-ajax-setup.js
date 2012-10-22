/// <reference path="jquery-1.8.1-vsdoc.js" />
/// <reference path="jquery-ui-1.8.23.js" />

// The following comments are for JSLint.
// Do NOT remove them!
// see http://www.jslint.com/
/*jslint browser: true, debug: true, devel: true, white: true, plusplus: true, maxerr: 100, unparam: true, indent: 4 */
/*global jQuery: false, Microsoft: false */

(function ($) {
    "use strict";
    $(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {
        alert("There was an error communicating with the server.");
    });
    $(document).ajaxComplete(function (event, jqXHR, ajaxSettings, thrownError) {
        // The custom header is added in LoginController.
        // Its presence indicates that we got the login page instead of a json response.
        // The only reason for that is session timeout.
        var isLoginPage = jqXHR.getResponseHeader("LoginPage"), login;
        if (isLoginPage) {
            login = "/Trips4/login/index?message=Your+session+has+timed+out.&ReturnUrl=" + encodeURIComponent(window.location.pathname + window.location.search);
            window.location.assign(login);
        }
    });
}(jQuery));