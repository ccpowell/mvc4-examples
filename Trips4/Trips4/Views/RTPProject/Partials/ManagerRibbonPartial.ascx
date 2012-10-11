<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpVersionModel>" %>

<script type="text/javascript">
    var DropAmendmentUrl = '<%=Url.Action("DropAmendment","RtpProject") %>';

    $(document).ready(function () {

    });


    $('.btn-drop').live("click", function() {
        var message = "Are you sure you want to drop this project?";

        if (confirm(message) == true) {
            $.ajax({
                type: "POST",
                url: DropAmendmentUrl,
                data: "ProjectVersionId=<%= Model.ProjectVersionId %>",
                dataType: "json",
                success: function(response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        location.reload();
                    }
                    else {
                        alert(response.message + " Details: " + response.exceptionMessage);
                        addResultMessage(response.message + " Details: " + response.exceptionMessage, 'error');
                    }
                },
                error: function(response, textStatus, AjaxException) {
                    addResultMessage(response.statusText, 'error');
                }
            });
        }
        return false;
    });

    $('.btn-undodrop').live("click", function () {
        var message = "Are you sure you want to undo the cancellation of this project?";
        
        if (confirm(message) == true) {
            $.ajax({
                type: "POST",
                url: DropAmendmentUrl,
                data: "ProjectVersionId=<%= Model.ProjectVersionId %>"
                    + "&VersionStatusId=<%= (int)DRCOG.Domain.Enums.RTPVersionStatus.Inactive %>",
                dataType: "json",
                success: function (response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        location.reload();
                    }
                    else {
                        alert(response.message + " Details: " + response.exceptionMessage);
                        addResultMessage(response.message + " Details: " + response.exceptionMessage, 'error');
                    }
                },
                error: function (response, textStatus, AjaxException) {
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
<%if (Model.IsCycleEditable())
  { %>
    <%= Html.HiddenFor(x => x.AmendmentStatusId) %>
    <% Html.RenderPartial("~/Views/RtpProject/Partials/ManagerRibbonButtonsPartial.ascx", Model); %>
<% } %>
</div>
