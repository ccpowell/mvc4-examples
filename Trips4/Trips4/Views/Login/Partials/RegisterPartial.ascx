<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.Person>" %>
<div id="register">
    <%= Html.ValidationSummary(true, "Registration was unsuccessful. Please correct the errors and try again.", new { @class="error"} )%>

    <h2>Create Account</h2>
    <% using (Html.BeginForm("Register", "Account", FormMethod.Post, new { @id = "registrationForm" })) %>
    <% { %>
    <fieldset>
        <legend>Please fill out the following information</legend>
        <table>
            
            <tr>
                <td>
                    <%= Html.LabelFor(model => model.profile.FirstName)%>
                </td>
                <td>
                    <span class="editor-field"><%= Html.TextBoxFor(model => model.profile.FirstName, new { size = "40", @class = "required", @maxlength = "256" })%></span>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Html.LabelFor(model => model.profile.LastName)%>
                </td>
                <td>
                    <span class="editor-field"><%= Html.TextBoxFor(model => model.profile.LastName, new { size = "40", @class = "required", @maxlength = "256" })%></span>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Html.LabelFor(model => model.profile.UserName)%>
                </td>
                <td>
                    <span class="editor-field"><%= Html.TextBoxFor(model => model.profile.UserName, new { size = "40", @class = "required", @maxlength = "256" })%></span>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Html.LabelFor(model => model.profile.Phone)%>
                </td>
                <td>
                    <span class="editor-field"><%= Html.TextBoxFor(model => model.profile.Phone, new { size = "40", @class = "required phone", @maxlength = "256" })%></span>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Html.LabelFor(model => model.profile.RecoveryEmail)%>
                </td>
                <td>
                    <span class="editor-field"><%= Html.TextBoxFor(model => model.profile.RecoveryEmail, new { size = "40", @class = "required email", @maxlength = "256" })%></span>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="ConfirmEmail">Confirm Email</label>
                </td>
                <td>
                    <span class="editor-field"><%=Html.TextBox("ConfirmEmail", "", new { size = "40", @class = "required", @equalTo = "#profile_Email", @maxlength = "256" })%></span>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <span class="red">* Password must contain a minumum of 7 characters</span>
                </td>
            </tr>
            <tr>
                <td>
                    <%= Html.LabelFor(model => model.profile.Password)%>
                </td>
                <td>
                    <span class="editor-field"><%= Html.PasswordFor(model => model.profile.Password, new { size = "40", @class = "required" })%></span>
                </td>
            </tr>
            <tr>
                <td>
                    <label for="ConfirmPassword">Confirm Password</label>
                </td>
                <td>
                    <span class="editor-field"><%= Html.Password("ConfirmPassword", "", new { size = "40", @class = "required", @equalTo = "#profile_Password" })%></span>
                </td>
            </tr>
            
            <%--<tr>
                <td>
                    <%= Html.LabelFor(model => model.profile.SponsorCode)%>
                </td>
                <td>
                    <span class="editor-field">
                        <input name="SponsorCode" class="code" id="profile_SponsorCode" type="text" size="40" maxLength="12"/>
                    </span>
                </td>
            </tr>--%>
        </table>
        <div class="dialog-result" style="display:none;">
          <span></span>.
        </div>
        <input type="submit" value="Submit" />
    </fieldset>
    <% } %>
</div>
<div style="display: none;" id="register-success">
    <h2>An account was successfully create for:<br /><span class="success-userName"></span></h2>
    <span style="font-variant:small-caps; font-size: 1.4em;">Important:</span>
    <p>
        To complete the registration process
        please check for an email sent to <span class="success-email"></span> where you will confirm the account.
        Once completed you will be logged into T.R.I.P.S.
    </p>
</div>

<script type="text/javascript" charset="utf-8">
    var CreateAccount = '<%=Url.Action("Register","Account") %>';

    $(document).ready(function () {

        $("#btn_register").colorbox({
            width: "500px",
            //            height: "520px",
            inline: true,
            href: "#registrationContainer",
            escKey: false,
            overlayClose: false,
            onLoad: function () {

            },
            onOpen: function () {
                $(document).unbind("keydown.cbox_close");

            },
            onComplete: function () {
                $("input.phone").mask("(999) 999-9999");

                var $form = $("form#registrationForm");
                $("form#registrationForm :submit").hide();

                $.fn.colorbox.resize();
                setTimeout(function () {
                    var $buttonRegister = $('<span id="button-register" class="cboxBtn">Register</span>').appendTo('#cboxContent');
                }, 300);

                // propose username by combining first- and lastname 
                $("#profile_UserName").focus(function () {
                    var firstname = $("#profile_FirstName").val();
                    var lastname = $("#profile_LastName").val();
                    if (firstname && lastname && !this.value) {
                        this.value = (firstname.substr(0, 1) + lastname).toLowerCase();
                    }
                });

                var validator = $form.validate({
                    rules: {
                        profile_FirstName: "required",
                        profile_LastName: "required",
                        profile_UserName: {
                            required: true,
                            minlength: 5
                        },
                        profile_Password: {
                            required: true,
                            minlength: 7
                        },
                        password_confirm: {
                            required: true,
                            minlength: 7,
                            equalTo: "#profile_Password"
                        },
                        profile_RecoveryEmail: {
                            required: true,
                            email: true
                        },
                        ConfirmEmail: {
                            required: true,
                            email: true,
                            equalTo: "#profile_RecoveryEmail"
                        }
                    },
                    invalidHandler: function (e, validator) {
                        var errors = validator.numberOfInvalids();
                        if (errors > 0) {
                            var message = errors == 1
                                ? 'You missed 1 field. It has been highlighted below'
                                : 'You missed ' + errors + ' fields. They have been highlighted above';
                            $("div.dialog-result span").html(message);
                            $("div.dialog-result").addClass('error').show();
                            $('#button-register').html("Register").addClass("disabled");
                        } else {
                            $("div.dialog-result").hide();
                            $('#button-register').html("Register").removeClass("disabled");
                        }
                    },
                    onkeyup: false,
                    // specifying a submitHandler prevents the default submit, good for the demo 
                    submitHandler: function () {
                        $("div.dialog-result").hide();
                        var jqxhr = $.post($form.attr('action'), $form.serialize(), function (json) {
                            if (json.error === 'false') {
                                $("#register").hide();
                                $("#register-success .success-userName").text($("input#profile_UserName").val());
                                $("#register-success .success-email").text($("input#profile_RecoveryEmail").val());
                                $("#register-success p").addClass('info');
                                $("#register-success").show();

                                $.fn.colorbox.resize();
                                $("#button-register").remove();

                                window.onbeforeunload = null;
                            } else {
                                $('.dialog-result').html(json.message);
                                $('.dialog-result').addClass('error').show();
                                $.fn.colorbox.resize();
                            }
                        }, 'json')
                        .error(function () {
                            $('#button-register').html("Register").removeClass("disabled");
                            $("#cboxClose").show();
                        });
                        //                            $.ajax({
                        //                                type: "POST",
                        //                                url: CreateAccount,
                        //                                data: "UserName=" + $("input#profile_UserName").val()
                        //                                    + "&FirstName=" + $("input#profile_FirstName").val()
                        //                                    + "&LastName=" + $("input#profile_LastName").val()
                        //                                    + "&Phone=" + stripNonNumeric($("input#profile_Phone").val())
                        //                                    + "&Email=" + $("input#profile_RecoveryEmail").val()
                        //                                    + "&Password=" + $("input#profile_Password").val()
                        //                                    + "&SponsorCode=" + $("input#profile_SponsorCode").val(),
                        //                                dataType: "json",
                        //                                success: function (response, textStatus, XMLHttpRequest) {
                        //                                    if (response.error == "false") {
                        //                                        //$('').html(response.message);
                        //                                        //$('').addClass('success');
                        //                                        //autoHide(2500);
                        //                                        //window.location = "/";
                        //                                        $("#register").hide();
                        //                                        $("#register-success").show();
                        //                                        $("#register-success .userName").text($("input#profile_Email").val());

                        //                                        $.fn.colorbox.resize();
                        //                                        $("#button-register").remove();

                        //                                    } else {
                        //                                        //$('.dialog-result').html(response.message + " Details: " + response.exceptionMessage);
                        //                                        //$('.dialog-result').addClass('error');
                        //                                        //autoHide(10000);
                        //                                        $('.dialog-result').html(response.message);
                        //                                        $('.dialog-result').addClass('error').show();
                        //                                        $.fn.colorbox.resize();
                        //                                    }
                        //                                    window.onbeforeunload = null;
                        //                                },
                        //                                error: function (response, textStatus, AjaxException) {
                        //                                    //alert("error");
                        //                                    //$('').html(response.statusText);
                        //                                    //$('').addClass('error');
                        //                                    //autoHide(10000);
                        //                                    $('#button-register').html("Register").removeClass("disabled");
                        //                                    $("#cboxClose").show();
                        //                                }
                        //                            });
                        //                        }
                    },
                    errorPlacement: function (error, element) {
                        $.fn.colorbox.resize();
                    },
                    debug: true
                });

                $('#button-register').live("click", function () {
                    $form.submit();
                    return false;
                });

            },
            onClosed: function () {
                $("#button-register").remove();
                $("#registrationForm").validate().resetForm();
                document.forms['registrationForm'].reset();
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