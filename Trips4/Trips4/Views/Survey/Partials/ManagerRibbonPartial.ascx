﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.Survey.ProjectBaseViewModel>" %>

<script type="text/javascript">
    var SetSurveyStatusUrl = '<%=Url.Action("SetSurveyStatus","Survey") %>';

    $(document).ready(function () {
        $("#opentopublic").mask("9999");
        $(".completed").colorbox(
        {
            width: "375px",
            height: "200px",
            inline: true,
            href: "#completed-form",
            onLoad: function () {

            },
            onComplete: function () {
                var $buttonCompletedSave = $('<span id="button-completed-save" class="cboxBtn">Save</span>').appendTo('#cboxContent');

                $('#button-completed-save').live("click", function () {
                    var currentYear = parseInt((new Date).getFullYear());
                    var opentopublic = parseInt($("#opentopublic").val());
                    
                    if ((opentopublic <= 2000) || (opentopublic > currentYear)) { /* it is assumed the date is greater than 2000 */
                        addResultMessage('An Open to Public date is required to complete this project', 'error');
                    } else {
                        if (confirm("Are you sure you want to mark this project as completed and open to the public?") == true) {
                            $.ajax({
                                type: "POST",
                                url: SetSurveyStatusUrl,
                                data: "ProjectVersionId=<%= Model.Project.ProjectVersionId %>"
                                    + "&UpdateStatusId=<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Completed %>"
                                    + "&EndConstructionYear=" + opentopublic,
                                dataType: "json",
                                success: function (response, textStatus, XMLHttpRequest) {
                                    if (response.error == "false") {
                                        location = '<%= Url.Action("ProjectList","Survey", new { year=Model.Current.Name }) %>';
                                    }
                                    else {
                                        addResultMessage(response.message + " Details: " + response.exceptionMessage, 'error');
                                    }
                                },
                                error: function (response, textStatus, AjaxException) {
                                    //$.fn.colorbox.close();
                                    addResultMessage(response.statusText, 'error');
                                }
                            });
                        } else {
                            $.fn.colorbox.close();
                            window.onbeforeunload = null;
                        }
                    }

                    return false;
                });
            },
            onCleanup: function () { $("#button-completed-save").remove(); $(".dialog-result").removeClass("success error").html('').hide(); }
        });

    });


    $('#btn-edit').live("click", function() {
        if (confirm("Are you sure you want to edit this project?")) {
            location = '<%= Url.Action("Edit","Survey", new { projectVersionId = Model.Project.ProjectVersionId, previousVersionId = Model.Project.PreviousVersionId }) %>';
        }
    });

    $('.btn-reviewed, .btn-withdraw, .btn-accept, .btn-editcontinue, .btn-current').live("click", function () {
        var message = "Are you sure you ";
        var updatestatusid;
        var returnToList = false;

        if ($(this).hasClass("btn-reviewed")) {
            message += "do not want to make changes to existing/current project information?";
            updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Reviewed %>";
            returnToList = true;
        } else if ($(this).hasClass("btn-withdraw")) {
            message += "want to mark this project as withdrawn?";
            updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Withdrawn %>";
            returnToList = true;
        } else if ($(this).hasClass("btn-accept")) {
            message += "want to accept these changes?";
            updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Accepted %>";
            returnToList = true;
        } else if ($(this).hasClass("btn-current")) {
            message += "want to accept these changes?";
            updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Current %>";
            returnToList = false;
        } else if ($(this).hasClass("btn-editcontinue")) {
            message = "Are you sure you want to continue editing?";
            updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Revised %>";
            returnToList = false;
        }

        if (confirm(message) == true) {
            $.ajax({
                type: "POST",
                url: SetSurveyStatusUrl,
                data: "ProjectVersionId=<%= Model.Project.ProjectVersionId %>"
                    + "&UpdateStatusId=" + updatestatusid,
                dataType: "json",
                success: function (response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        if (!returnToList) location.reload();
                        else location = '<%= Url.Action("ProjectList","Survey", new { year=Model.Current.Name }) %>';
                    }
                    else {
                        alert(response.message + " Details: " + response.exceptionMessage);
                        addResultMessage(response.message + " Details: " + response.exceptionMessage, 'error');
                    }
                },
                error: function (response, textStatus, AjaxException) {
                    //$.fn.colorbox.close();
                    addResultMessage(response.statusText, 'error');
                }
            });
        }
        return false;
    });
    
    
    function addResultMessage(message, state) {
        $('.dialog-result').html(message);
        $('.dialog-result').addClass(state).show();
        $.fn.colorbox.resize();
    }

</script>

<div id="manager-ribbon">
<%if(Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin) || (Model.Project.IsSponsorContact && (Model.Project.IsEditable() || Model.Current.IsOpen()))){ %>
   <%-- <span id="update-status">Update Status:
    <%= Html.DropDownList("Project.UpdateStatusId",
        Model.Project.IsEditable(),
        new SelectList(Model.AvailableUpdateStatus, "key", "value", Model.Project.UpdateStatusId),
        new { @class = "required", title = "Please select a Primary Sponsor" })%>
    </span>
   --%>
    <%= Html.HiddenFor(x => x.Project.UpdateStatusId) %>
    <% if (!Model.Project.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited))
       {
           if (Model.Project.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Accepted) || Model.Project.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Current))
        { %>
            <div class="btn-editcontinue fg-button w125 ui-state-default ui-priority-primary ui-corner-all bg-green" >Continue editing</div>
        <% } else { %>
            <div id="btn-edit" class="fg-button w75 ui-state-default ui-priority-primary ui-corner-all bg-green" >Edit</div>
            <% } %>
    <% } %>
       
    <% Html.RenderPartial("~/Views/Survey/Partials/ManagerRibbonButtonsPartial.ascx", Model.Project); %>
<% } %>
</div>


<!-- This contains the hidden content for inline calls --> 
<div style='display:none'> 
	<%Html.RenderPartial("~/Views/Survey/Partials/CompletedFormPartial.ascx", Model.Project); %>
</div>