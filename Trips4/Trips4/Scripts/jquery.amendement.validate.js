$(document).ready(function() {
    jQuery.validator.messages.required = "";
    $("#dataForm").validate({
        invalidHandler: function(e, validator) {
            var errors = validator.numberOfInvalids();
            if (errors) {
                var message = errors == 1
					? 'You missed 1 field. It has been highlighted below'
					: 'You missed ' + errors + ' fields.  They have been highlighted below';
                $("div.error span").html(message);
                $("div.error").show();
            } else {
                $("div.error").hide();
            }
        },
        onkeyup: false,
        submitHandler: function() {
            $("div.error").hide();
            //alert("submit! use link below to go to the other step");
        },
        debug: true
    });
});
