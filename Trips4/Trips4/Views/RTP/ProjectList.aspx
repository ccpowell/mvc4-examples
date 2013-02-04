<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.ProjectListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<%@ Import Namespace="MvcContrib.UI.Grid.ActionSyntax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    RTP Project List</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server">
    Regional Transportation Plan
    <%= Model.RtpSummary.RtpYear %></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Url.Content("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.quicksearch.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/RtpProjectList.js")%>" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        var App = App || {};
        App.pp = {
            CurrentCycleId: <%= Model.RtpSummary.Cycle.Id %>,
            PreviousCycleId: <%= Model.RtpSummary.Cycle.PriorCycleId %>,
            NextCycleId: <%= Model.RtpSummary.Cycle.NextCycleId %>,
            RtpPlanYear: '<%= Model.RtpSummary.RtpYear %>',
            RtpPlanYearId: <%= Model.RtpSummary.RTPYearTimePeriodID %>
        };
        function confirmDelete() {
            return !!(confirm('Are you sure you want to delete this amendment?'));
        }
    </script>
    <script type="text/javascript">
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="view-content-container">
        <div class="clear">
        </div>
        <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>
        <div class="tab-content-container">

            <div id="button-container">
                <% if (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))
                   { %>
                <% if (Model.RtpSummary.IsCycleAmendable() && Model.RtpSummary.Cycle.StatusId == (int)DRCOG.Domain.Enums.RTPCycleStatus.Active)
                   { %>
                <button id="amend-projects">
                    Amend Projects</button>
                <% } %>
                <% if (Model.RtpSummary.IsCycleEditable())
                   { %>
                <button id="restore-projects">
                    Restore Projects</button>
                <button id="create-project">
                    New Project</button>
                <% } %>
                <% if (Model.RtpSummary.IsCycleIncludable())
                   { %>
                <button id="include-projects">
                    Include More</button>
                <button id="adopt-cycle">
                    Adopt Cycle</button>
                <% } %>
                <% if (Model.RtpSummary.IsMissingNextCycle())
                   { %>
                   There is no Pending or New cycle. Please use the Plan Cycles tab to add a New cycle.
                   <% } %>
                <% } %>
            </div>
            <!-- clear the buttons -->
            <div style="height: 45px; width: 1px; position: relative;">
                &nbsp;</div>
            <div class="big-bold">
                <div>
                    <%if (!String.IsNullOrEmpty(Model.ListCriteria))
                      { %>
                    Project List for:
                    <%= Model.ListCriteria %>
                    <% }
                      else
                      { %>
                    Cycle
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
            <br />
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
        <div id="dialog-amendpending-project" class="dialog" title="Amend Plan Projects">
            <h2>
                Projects ready for amending:<span id="countReady"></span></h2>
            <ul id="amend-list">
            </ul>
        </div>
        <div id="dialog-create-project" title="Create New RTP Project">
            <h2>
                Create a new Project</h2>
            <form action="" id="create-project-form">
            <p>
                <label for="facilityName">
                    Facility Name:</label>
                <input type="text" name="facilityName" class="big required" id="facilityName" />
            </p>
            <p>
                <label for="projectName">
                    Project Name:</label>
                <input type="text" name="projectName" class="big required" id="projectName" />
            </p>
            <p>
                <label for="availableSponsors">
                    Sponsor:</label>
                <select class="mediumInputElement big" id="availableSponsors" name="availableSponsors"
                    size="1">
                </select>
            </p>
            </form>
        </div>
        <div id="dialog-amend-projects">
            <div class="info" id="amend-info">
                Select the projects you wish to amend in the next cycle</div>
            <table id="amendProjects">
                <tr>
                    <td>
                        Available Projects:
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        Selected Projects: <span id="amend-countReady"></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <select id="amend-availableProjects" class="w400 nobind" size="25" multiple="multiple">
                        </select>
                    </td>
                    <td>
                        <a href="#" id="amend-addProject" title="Add Project">
                            <img src="<%=Url.Content("~/content/images/24-arrow-next.png")%>" alt="add" /></a><br />
                        <a href="#" id="amend-removeProject" title="Remove Project">
                            <img src="<%=Url.Content("~/content/images/24-arrow-previous.png")%>" alt="remove" /></a><br />
                    </td>
                    <td>
                        <select id="amend-selectedProjects" class="w400 nobind" size="25" multiple="multiple">
                        </select>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
