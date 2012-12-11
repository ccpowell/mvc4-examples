<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.ProjectListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<%@ Import Namespace="MvcContrib.UI.Grid.ActionSyntax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    RTP Project List</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server">
    Regional Transportation Plan
    <%= Model.RtpSummary.RtpYear %></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/jquery.dataTables.css") %>" rel="stylesheet"
        type="text/css" />
    <script src="<%= Url.Content("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.quicksearch.js")%>" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        var oProjectListGrid;
        $(document).ready(function () {

            oProjectListGrid = $('#projectListGrid').dataTable({
                "iDisplayLength": 100,
                "aaSorting": [[1, "asc"]],
                "aoColumns": [null, { sWidth: '60px' }, { sWidth: '110px' }, { sWidth: '60px' }, { sWidth: '190px' }, null, null, { "bVisible": false }, { "bVisible": false}]
            });

            if ($('#message').html != "") {
                $('div#message').addClass('warning');
                //autoHide(10000);
            }

            function autoHide(timeout) {
                if (isNaN(timeout)) timeout = 5000;
                setTimeout(function () {
                    $("div#message").fadeOut("slow", function () {
                        $("div#message").empty().removeClass().removeAttr('style');
                    });
                }, timeout);
            }
        });

        function confirmDelete() {
            return !!(confirm('Are you sure you want to delete this amendment?'));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="view-content-container">
        <div class="clear">
        </div>
        <div>
            EXPERIMENTAL</div>
        <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>
        <div class="tab-content-container">
            <div id="button-container">
                <% if (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))
                   { %>
                <% if (Model.RtpSummary.IsAmendable() && Model.RtpSummary.Cycle.StatusId == (int)DRCOG.Domain.Enums.RTPCycleStatus.Active)
                   { %>
                <button id="btn-amendprojects">
                    Amend Projects</button>
                <% } %>
                <% if (Model.RtpSummary.IsEditable())
                   { %>
                <button id="btn-restoreproject">
                    Restore Projects</button>
                <button id="btn-newproject">
                    New Project</button>
                <% } %>
                <% if (Model.RtpSummary.IsAmendable() && Model.RtpSummary.Cycle.StatusId == (int)DRCOG.Domain.Enums.RTPCycleStatus.Pending)
                   { %>
                <button id="btn-includemore">
                    Include More</button>
                <button id="btn-amendpendingprojects">
                    Adopt Cycle</button>
                <% } %>
                <% } %>
            </div>
            <div class="big-bold">
                <div>
                    Project List for:
                    <%if (!String.IsNullOrEmpty(Model.ListCriteria))
                      { %>
                    <%= Model.ListCriteria %>
                    <% }
                      else
                      { %>Cycle
                    <%= Model.RtpSummary.Cycle.Name %>
                    <%= Model.RtpSummary.Cycle.Status %>
                    <% } %>
                </div>
                <% if (!Model.RtpSummary.Cycle.NextCycleId.Equals(default(int)) && (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator")))
                   { %>
                <div>
                    Next Cycle:
                    <%= Html.ActionLink(Model.RtpSummary.Cycle.NextCycleName + " " + Model.RtpSummary.Cycle.NextCycleStatus, "ProjectList", new { controller = "RTP", year = Model.RtpSummary.RtpYear, cycleId = Model.RtpSummary.Cycle.NextCycleId }) %>
                </div>
                <% } %>
            </div>
            <% if (ViewData.ContainsKey("ShowMessage"))
               { %>
            <div id="message" style="position: absolute; top: -15px; left: 160px; width: 400px;">
                <%= ViewData["ShowMessage"].ToString() %>
            </div>
            <% } %>
            <% if (Model.ProjectList.Count > 0)
               { %>
            <span id="currentCycleId" style="display: none;">
                <%= Model.RtpSummary.Cycle.Id %></span> <span id="nextCycleId" style="display: none;">
                    <%= Model.RtpSummary.Cycle.NextCycleId %></span>
            <% Html.Grid(Model.ProjectList).Columns(column =>
        {
            //column.For(x => Html.ActionLink("Details", "Details", new { controller = "Project", year = Model.TipSummary.TipYear, id = x.ProjectVersionId })).Encode(false);
            column.For(x => Html.ActionLink(x.Title, "Details", new { controller = "RtpProject", year = x.RtpYear, id = x.ProjectVersionId })).Encode(false).Named("Project Name");
            column.For(x => x.PlanType).Named("Plan Type");
            column.For(x => x.COGID).Named("COGID");
            column.For(x => x.TIPId).Named("TIPID").Action(x =>
            { %>
            <td>
                <% if (!String.IsNullOrEmpty(x.TIPId))
                   { %>
                <%= Html.ActionLink("Details", "Funding", new { controller = "Project", year = x.TipTimePeriod, id = x.TIPId })%>
                <% }
                   else
                   { %>&nbsp;<% } %>
            </td>
            <% });
            //column.For(x => x.TIPId).Named("TIPID");
            column.For(x => x.ImprovementType).Named("Improvement Type");
            column.For(x => x.SponsorAgency).Named("Sponsor");
            //column.For(x => x.RtpId).Named("Plan");
            //column.For(x => x.VersionStatus).Named("Version Status");
            column.For(x => x.AmendmentStatus).Named("Stage").Action(x =>
            { %>
            <td>
                <%= x.AmendmentStatus %>
                <% if (x.AmendmentStatus.Equals("Pending"))
                   { %>
                <%= Html.ActionLink("Delete", "DeleteAmendment", new { controller = "RtpProject", year = Model.RtpSummary.RtpYear, projectVersionId = x.ProjectVersionId, previousProjectVersionId = default(int)}, new { onclick = "return confirmDelete();" })%>
                <% }
                   else
                   { %>&nbsp;<% } %>
            </td>
            <% });
            column.For(x => x.ProjectVersionId);
            column.For(x => x.Cycle.Id);
        }).Attributes(id => "projectListGrid").Render(); %>
            <% }
               else
               { %>
            No projects matching these search criteria were found.
            <% }  %>
            <div class="belowTable">
            </div>
        </div>
        <div style='display: none'>
            <div id="dialog-restore-project" class="dialog" title="Restore projects ...">
            </div>
        </div>
    </div>
</asp:Content>
