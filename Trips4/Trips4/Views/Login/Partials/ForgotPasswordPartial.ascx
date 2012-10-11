<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Security.PasswordRecoveryModel>" %>

    <h2>Did you forget your password?</h2>
    
    <div class="dialog-result" style="display:none;">
        <span></span><br clear="all"/>
    </div>
    <% using (Html.BeginForm("ForgotPassword", "Account", FormMethod.Post, new { @id = "forgotPasswordForm" })) %>
    <% { %>
        <%= Html.ValidationSummary(true, "Login was unsuccessful. Please correct the errors and try again.", "@class='error'")%>
            
            <% if (ViewData["SuccessMessage"] != null)
               { %>
            <div>
            <p class="success"><%= ViewData["SuccessMessage"].ToString() %></p>
            <p><%= Html.ActionLink("Return to log on", "LogOn") %></p>
            </div>
            <% }
               else
               { %>
            <div>
                <h3>Please enter your email address below and a recovery email will be sent to your account</h3>
                <br />
                <span class="editor-field"><%= Html.TextBoxFor(model => model.Email, new { size = "40", @class = "required ghost_text", @placeholder = "your email", @maxlength = "256" })%></span>
            </div>
            <% } %>
<% } %>

<script type="text/javascript" charset="utf-8">
    var ForgotPasswordUrl = '<%=Url.Action("ForgotPassword","Account") %>';

    $(document).ready(function() {


        $("#btn_forgotPassword").colorbox({
            width: "500px",
            height: "200px",
            inline: true,
            href: "#forgotPasswordContainer",
            escKey: true,
            overlayClose: true,
            onLoad: function() {

            },
            onOpen: function() {
                //$(document).unbind("keydown.cbox_close");
            },
            onComplete: function() {
                //$.fn.colorbox.resize();

                var $buttonForgotPassword = $('<span id="button-forgotpassword" class="cboxBtn">Request Reset</span>').appendTo('#cboxContent');
                $("#Email").val("").show();
                $.validator.messages.required = "";

                $("#forgotPasswordForm").validate({
                    rules: {
                        Email: {
                            required: true,
                            email: true
                        }
                    },
                    invalidHandler: function(e, validator) {
                        var errors = validator.numberOfInvalids();
                        if (errors) {
                            var message = errors == 1
                                ? 'You missed 1 field. It has been highlighted below'
                                : 'You missed ' + errors + ' fields. They have been highlighted below';
                            $("div.dialog-result span").html(message);
                            $("div.dialog-result").addClass("error").autoHide({ wait: 10000, removeClass: "error" });
                            $('#button-forgotpassword').html("Request Reset").removeClass("disabled");
                        } else {
                            $("div.dialog-result").hide();

                        }

                    },
                    onkeyup: false,
                    errorPlacement: function(error, element) {
                        //$.fn.colorbox.resize();
                    }
                });



                $('#button-forgotpassword').live("click", function() {
                    var emailBox = $("#Email");
                    if ($("#forgotPasswordForm").validate().form() == true) {
                        $('#button-forgotpassword').html("Sending").addClass("disabled");
                        $.ajax({
                            type: "POST",
                            url: ForgotPasswordUrl,
                            data: "Email=" + $("input#Email").val(),
                            dataType: "json",
                            success: function(response, textStatus, XMLHttpRequest) {
                                if (response.error == "false") {

                                    $('#button-forgotpassword').hide();
                                    var $cboxResponse = $('<span class="cboxLabel">Email Sent</span>').appendTo('#cboxContent');
                                    $('div.dialog-result span').html(response.message);
                                    $('div.dialog-result').addClass('success').show();
                                    $("#forgotPasswordContainer").append("<span id='responseInner'>Recovery email successfully sent to " + emailBox.val() + "</span>");
                                    emailBox.hide();
                                    //autoHide(2500);
                                } else {
                                    $("#Email").val("");
                                    $('#button-forgotpassword').html("Request Reset").removeClass("disabled");
                                    $('div.dialog-result span').html(response.message + "<br/>" + response.exceptionMessage);
                                    $('div.dialog-result').addClass('error').autoHide({ wait: 10000, removeClass: "error" });
                                }
                                window.onbeforeunload = null;
                            },
                            error: function(response, textStatus, AjaxException) {
                                //alert("error");
                                //$('').html(response.statusText);
                                //$('').addClass('error');
                                //autoHide(10000);
                                $('#button-forgotpassword').html("Request Reset").removeClass("disabled");
                                $("#cboxClose").show();
                            }
                        });
                    }
                    return false;
                });

            },
            onClosed: function() {
                $("#button-forgotpassword").remove();
                $(".cboxLabel").remove();
                $(".dialog-result").removeClass("success error").hide();
                $("#responseInner").remove();
            }
        });
    });

    function stripNonNumeric(str) {
        str += '';
        var rgx = /^\d|\.|-$/;
        var out = '';
        for (var i = 0; i < str.length; i++) {
            if (rgx.test(str.charAt(i))) {
                if (!((str.charAt(i) == '.' && out.indexOf('.') != -1) ||
             (str.charAt(i) == '-' && out.length != 0))) {
                    out += str.charAt(i);
                }
            }
        }
        return out;
    }
    
    function submitForm() {
        alert(stripNonNumeric($("input.phone").val()));
        if (!$(this).hasClass("disabled")) {
        $(this).html("Processing... Please DO NOT Close this window!!!").addClass("disabled");
        $("#cboxClose").hide();

        }
    }
</script>