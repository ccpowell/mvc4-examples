<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.Survey.ProjectListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid"%>
<%@ Import Namespace="MvcContrib.UI.Grid.ActionSyntax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Survey <%= Model.Current.Name %> Project List</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server"><%= Model.Current.Name%> Survey Projects</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<link href="<%= ResolveUrl("~/Content/jquery.dataTables.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript" ></script>

<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.quicksearch.js")%>" type="text/javascript" ></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maskedinput-1.2.2.min.js")%>" type="text/javascript" ></script>

<script type="text/javascript" charset="utf-8">
    var oProjectListGrid;
    $(document).ready(function() {
    
        $(".change-status").live("click", function() {
            $(".status-container").hide();
            $(this).children(":first-child").show();
        });

        /* Trying to get rows highlighting
        $(".gridrow").live("click", function() {
            $(this).addClass('selected');
        });

        $(".gridrow_alternate").live("click", function() {
            $(this).addClass('selected');
        });
        */

        oProjectListGrid = $('#projectListGrid').dataTable({
            "iDisplayLength": 25,
            "aaSorting": [[1, "asc"]],
            "aoColumns": [{ sWidth: '120px' }, { sWidth: '110px' <% if ( Model.Person.HasProjects ) { %>, "bVisible": false  <% } %> }, { sWidth: '60px' }, { sWidth: '60px' }, { sWidth: '60px' },/*{ sWidth: '30px' },*/ { "bVisible": false}]
        });

        if ($('#message').html != "") {
            $('div#message').addClass('warning');
            //autoHide(10000);
        }


        if ('<%= Model.Current.IsAdmin() %>' === 'True') {
            $("#projectListGrid_filter").show();
        } else {
            $("#projectListGrid_filter").hide();
        }

        function autoHide(timeout) {
            if (isNaN(timeout)) timeout = 5000;
            setTimeout(function() {
                $("div#message").fadeOut("slow", function() {
                    $("div#message").empty().removeClass().removeAttr('style');
                });
            }, timeout);
        }
        
        var SetSurveyStatusUrl = '<%=Url.Action("SetSurveyStatus","Survey") %>';

        
        $("#opentopublic").mask("9999");
        
        $(".completed").colorbox(
        {
            width: "375px",
            height: "200px",
            inline: true,
            href: "#completed-form",
            onLoad: function() {
                
            },
            onComplete: function() {
                var $buttonCompletedSave = $('<span id="button-completed-save" class="cboxBtn">Save</span>').appendTo('#cboxContent');
                var projectversionid = $(this).parent().children(":first-child").attr("id").replace("pv-","");
                
                $('#button-completed-save').live("click", function() {
                    var opentopublic = $("#opentopublic").val();
                    if (opentopublic >= 2000) { /* it is assumed the date is greater than 2000 */
                        addResultMessage('An Open to Public date is required to complete this project', 'error');
                    } else {
                        if (confirm("Are you sure you want to mark this project as open to the public?") == true) {
                            $.ajax({
                                type: "POST",
                                url: SetSurveyStatusUrl,
                                data: "ProjectVersionId=" + projectversionid
                                    + "&UpdateStatusId=<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Completed %>"
                                    + "&EndConstructionYear=" + opentopublic,
                                dataType: "json",
                                success: function(response, textStatus, XMLHttpRequest) {
                                    if (response.error == "false") {
                                        location.reload();
                                    }
                                    else {
                                        addResultMessage(response.message + " Details: " + response.exceptionMessage, 'error');
                                    }
                                },
                                error: function(response, textStatus, AjaxException) {
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
            onCleanup: function() { $("#button-completed-save").remove(); $(".dialog-result").removeClass("success error").html('').hide(); }
        });

        $('.btn-reviewed, .btn-withdraw, .btn-accept, .btn-editcontinue, .btn-current').live("click", function() {
            var message = "Are you sure you want to mark this project as ";
            var projectversionid = $(this).parent().children(":first-child").attr("id").replace("pv-","");
            var statusobject = $(this).closest("tr").children(".status");
            var updatestatusid;
            
             if ($(this).hasClass("btn-reviewed")) {
                message += "reviewed?";
                updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Reviewed %>";
            } else if ($(this).hasClass("btn-withdraw")) {
                message += "withdrawn?";
                updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Withdrawn %>";
            } else if ($(this).hasClass("btn-accept")) {
                message += "accepted?";
                updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Accepted %>";
            } else if ($(this).hasClass("btn-current")) {
                message += "accepted?";
                updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Current %>";
            } else if ($(this).hasClass("btn-editcontinue")) {
                message = "Are you sure you want to continue editing?";
                updatestatusid = "<%= (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Revised %>";
            }
            
            if (confirm(message) == true) {
                $.ajax({
                    type: "POST",
                    url: SetSurveyStatusUrl,
                    data: "ProjectVersionId=" + projectversionid
                        + "&UpdateStatusId=" + updatestatusid,
                    dataType: "json",
                    success: function(response, textStatus, XMLHttpRequest) {
                        if (response.error == "false") {
                            location.reload();
                        }
                        else {
                            addResultMessage(response.message + " Details: " + response.exceptionMessage, 'error');
                        }
                    },
                    error: function(response, textStatus, AjaxException) {
                        //$.fn.colorbox.close();
                        addResultMessage(response.statusText, 'error');
                    }
                });
            }
            
            return false;
        });
        
        

    });
</script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
<div class="clear"></div>
    
    <%Html.RenderPartial("~/Views/Survey/Partials/TabPartial.ascx", Model.Current); %>

    <div class="tab-content-container">
    <div id="button-container">
    <%--<% if (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))
       { %>
        <% if (Model.RtpSummary.IsAmendable())
           { %>
            <span id="btn-amendprojects" class="fg-button w200 ui-state-default ui-corner-all">Amend Projects</span>
        <% } %>
        <% if (Model.RtpSummary.IsEditable())
           { %>
            <span id="btn-restoreproject" class="fg-button w200 ui-state-default ui-corner-all">Restore Projects</span>
            <span id="btn-newproject" class="fg-button w200 ui-state-default ui-corner-all">New Project</span>
        <% } %>
    <% } %>--%>
    </div>
    <%--<div style='display:none'>
        <div id="dialog-restore-project" class="dialog" title="Restore projects ...">
            <%Html.RenderPartial("~/Views/Survey/Partials/RestoreProjects.ascx", Model); %>
        </div>
    </div>
    <%Html.RenderPartial("~/Views/Survey/Partials/CreatePartial.ascx", Model.RtpSummary); %>
    <%Html.RenderPartial("~/Views/Survey/Partials/AmendPartial.ascx", Model); %>--%>
    
    
    
    <%if ( !String.IsNullOrEmpty(Model.ListCriteria)) { %> <h2>Project List for: <%= Model.ListCriteria %> </h2> <% } else { %><%--Cycle <%= Model.RtpSummary.Cycle.Name %> <%= Model.RtpSummary.Cycle.Status %>--%> <% } %>
    <% if (ViewData.ContainsKey("ShowMessage"))
    {%>
    <div id="message" style="position: absolute; top: -15px; left: 160px; width: 400px;">
        <%= ViewData["ShowMessage"].ToString() %>
    </div>
    <% } %>

    <% if (Model.ProjectList.Count > 0) { %>
    <% Html.Grid(Model.ProjectList).Columns(column => {
     		column.For(x => Html.ActionLink(x.ProjectName, "Info", new { controller = "Survey", year = x.TimePeriod, id = x.ProjectVersionId })).Encode(false).Named("Project Name");
            column.For(x => x.COGID).Named("COGID");
            column.For(x => x.ImprovementType).Named("Improvement Type");
            column.For(x => x.SponsorName).Named("Sponsor");
            column.For(x => x.UpdateStatus).Named("Status").Attributes(@class => "status");
            //column.For("Change Status").Named("").Action(x =>	{ %>
                <%--<td>
                    <% if (Model.Person.SponsorsProject(x.ProjectVersionId) && Model.Current.IsOpen()) { %>
                    <div style="position: relative;" class="link change-status">change status
                        <div style="display:none" class="status-container">
                            <% Html.RenderPartial("~/Views/Survey/Partials/ManagerRibbonButtonsPartial.ascx", new DRCOG.Domain.Models.Survey.Project() { UpdateStatusId = x.UpdateStatusId, ProjectVersionId = x.ProjectVersionId }); %>
                        </div>
                    </div>
                    <% } %>
	            </td>--%>
                <% //});
           column.For(x => x.ProjectVersionId);
       }).Attributes(id => "projectListGrid").Render(); %>
    <% } else { %>
    No Minor or Collector Roadway Projects were found. Please add projects if available. 
    <% }  %>
    <div class="belowTable">
    
    <%--<a id="createNewTip" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="<%=Url.Action("Create","Project", new {year=Model.RtpSummary.RtpYear}) %>">
        <span class="ui-icon ui-icon-circle-plus"></span>Create New Project</a>--%>
    
    
    
    </div>
    </div>
</div>

<!-- This contains the hidden content for inline calls --> 
<div style='display:none'> 
	<%Html.RenderPartial("~/Views/Survey/Partials/CompletedFormPartial.ascx", new DRCOG.Domain.Models.Survey.Project()); %>
</div>

</asp:Content>


