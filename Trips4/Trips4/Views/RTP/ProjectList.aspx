<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.ProjectListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid"%>
<%@ Import Namespace="MvcContrib.UI.Grid.ActionSyntax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">RTP Project List</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<link href="<%= ResolveUrl("~/Content/jquery.dataTables.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript" ></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.quicksearch.js")%>" type="text/javascript" ></script>
<script type="text/javascript" charset="utf-8">
    var oProjectListGrid;
    $(document).ready(function() {

        oProjectListGrid = $('#projectListGrid').dataTable({
            "iDisplayLength": 100,
            "aaSorting": [[1, "asc"]],
            "aoColumns": [null, { sWidth: '60px' }, { sWidth: '110px' }, { sWidth: '60px' }, { sWidth: '190px' }, null, null, { "bVisible": false }, { "bVisible": false}]
        });


//        $(".grid_ltgreen.odd").css("background-color", "#97e393");
//        $(".grid_ltgreen.even").css("background-color", "#6ee768");
        


        if ($('#message').html != "") {
            $('div#message').addClass('warning');
            //autoHide(10000);
        }

        function autoHide(timeout) {
            if (isNaN(timeout)) timeout = 5000;
            setTimeout(function() {
                $("div#message").fadeOut("slow", function() {
                    $("div#message").empty().removeClass().removeAttr('style');
                });
            }, timeout);
        }


    });

    function confirmDelete() {
        if (confirm('Are you sure you want to delete this amendment?'))
            return true;
        else
            return false;
    }
</script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
<div class="clear"></div>
    
    <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>

    <div class="tab-content-container">
    <div id="button-container">
    <% if (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))
       { %>
        <% if (Model.RtpSummary.IsAmendable() && Model.RtpSummary.Cycle.StatusId == (int)DRCOG.Domain.Enums.RTPCycleStatus.Active)
           { %>
            <span id="btn-amendprojects" class="fg-button w200 ui-state-default ui-corner-all">Amend Projects</span>
        <% } %>
        <% if (Model.RtpSummary.IsEditable())
           { %>
            <span id="btn-restoreproject" class="fg-button w200 ui-state-default ui-corner-all">Restore Projects</span>
            <span id="btn-newproject" class="fg-button w200 ui-state-default ui-corner-all">New Project</span>
        <% } %>
        <% if (Model.RtpSummary.IsAmendable() && Model.RtpSummary.Cycle.StatusId == (int)DRCOG.Domain.Enums.RTPCycleStatus.Pending)
           { %>
            <span id="btn-includemore" class="fg-button w200 ui-state-default ui-corner-all includemore">Include More</span>
            <span id="btn-amendpendingprojects" class="fg-button w200 ui-state-default ui-corner-all">Adopt Cycle</span>
        <% } %>
    <% } %>
    </div>
    <div style='display:none'>
        <div id="dialog-restore-project" class="dialog" title="Restore projects ...">
            <%Html.RenderPartial("~/Views/RTP/Partials/RestoreProjects.ascx", Model); %>
        </div>
    </div>
    <%Html.RenderPartial("~/Views/RTP/Partials/CreatePartial.ascx", Model.RtpSummary); %>
    <%Html.RenderPartial("~/Views/RTP/Partials/AmendPartial.ascx", Model); %>
    <% if (Model.RtpSummary.IsAmendable() && Model.RtpSummary.Cycle.StatusId == (int)DRCOG.Domain.Enums.RTPCycleStatus.Pending)
        { %>
        <%Html.RenderPartial("~/Views/RTP/Partials/AmendPendingPartial.ascx", Model); %>
    <% } %>
    
    <div class="big-bold">
        <div>
        Project List for: 
        <%if ( !String.IsNullOrEmpty(Model.ListCriteria)) { %> <%= Model.ListCriteria %> <% } else { %>Cycle <%= Model.RtpSummary.Cycle.Name %> <%= Model.RtpSummary.Cycle.Status %> <% } %> 
        </div>
        <% if (!Model.RtpSummary.Cycle.NextCycleId.Equals(default(int)) && (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator")))
           { %>
        <div>
            Next Cycle: <%= Html.ActionLink(Model.RtpSummary.Cycle.NextCycleName + " " + Model.RtpSummary.Cycle.NextCycleStatus, "ProjectList", new { controller = "RTP", year = Model.RtpSummary.RtpYear, cycleId = Model.RtpSummary.Cycle.NextCycleId }) %>
        </div>
        <% } %>
    </div>
    
    
    <% if (ViewData.ContainsKey("ShowMessage"))
    {%>
    <div id="message" style="position: absolute; top: -15px; left: 160px; width: 400px;">
        <%= ViewData["ShowMessage"].ToString() %>
    </div>
    <% } %>

    <% if (Model.ProjectList.Count > 0) { %>
    <span id="currentCycleId" style="display: none;"><%= Model.RtpSummary.Cycle.Id %></span>
    <span id="nextCycleId" style="display: none;"><%= Model.RtpSummary.Cycle.NextCycleId %></span>
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
        }).Attributes(id => "projectListGrid").RowAttributes(data => new MvcContrib.Hash(@class => (data.Item.Cycle.Id.Equals(Model.RtpSummary.Cycle.Id)) ? "grid_ltgreen" : "")).Render(); %>
    <% } else { %>
    No projects matching these search criteria were found.
    <% }  %>
    <div class="belowTable">
    
    <%--<a id="createNewTip" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="<%=Url.Action("Create","Project", new {year=Model.RtpSummary.RtpYear}) %>">
        <span class="ui-icon ui-icon-circle-plus"></span>Create New Project</a>--%>
    
    </div>
    </div>
</div>

</asp:Content>


