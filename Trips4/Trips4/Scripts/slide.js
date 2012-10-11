$(document).ready(function() {

    // Expand Panel
    $("#open").click(function() {
        $("div#panel").slideDown("slow");
        // set breadCrumbs to lower z-index so it is hidden
        $("#breadCrumbs").css('z-index', 998);
    });

    // Collapse Panel
    $("#close").click(function() {
        // remember to bring the breadCrumbs back to life
        $("div#panel").slideUp("slow", function() { $("#breadCrumbs").css('z-index', 1000); });
    });

    // Switch buttons from "Log In | Register" to "Close Panel" on click
    $("#toggle a").click(function() {
        $("#toggle a").toggle();
    });

});