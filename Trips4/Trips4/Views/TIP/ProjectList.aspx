<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.ProjectListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">TIP Project List</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<link href="<%= ResolveUrl("~/Content/jquery.dataTables.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
<script type="text/javascript" charset="utf-8">
    $(document).ready(function() {
    $('#projectListGrid').dataTable({
        "iDisplayLength": 50,
        "aaSorting": [[1, "asc"], [2, "asc"]],
        "aoColumns": [{ "bSortable": false }, { "sWidth": "100px" }, null, null, null, null, null, null]
    });

    function autoHide() {
        setTimeout(function() {
            $("div#result").fadeOut("slow", function() {
                $("div#result").empty().removeClass().removeAttr('style');
            });
        }, 5000);
    }
});
</script>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
<%--<h2 ><%=Html.ActionLink("TIP List", "Index",new {controller="TIP"}) %> / TIP <%=Model.TipSummary.TipYear%></h2>--%>
<div class="clear"></div>
    
    <%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>

    <div class="tab-content-container">
    <h2><%--<%= Model.TipSummary.TipYear %> --%>TIP Project request results</h2>

    <% if (Model.ProjectList.Count > 0) { %>
   <%= Html.Grid(Model.ProjectList).Columns(column =>
        {
            column.For(x => Html.ActionLink("Details", "Funding", new { controller = "Project", year = x.TipYear, id = x.ProjectVersionId })).Encode(false);
            //column.For(x => x.TipYear).Named("TIP Year");
            column.For(x => x.TipId).Named("TIPID");
            column.For(x => x.Title).Named("Title");
            column.For(x => x.SponsorAgency).Named("Primary<br>Sponsor");
            column.For(x => x.ProjectType).Named("Project<br>Type");
            column.For(x => x.VersionStatus).Named("Version<br>Status");
            column.For(x => x.AmendmentStatus).Named("Amendment<br>Status");
            column.For(x => x.AmendmentDate.ToShortDateString()).Named("Amendment<br>Date");
        }).Attributes(id => "projectListGrid")%>
    <% } else { %>
    No projects matching these search criteria were found.
    <% }  %>
    <div class="belowTable">
    
    <% Html.RenderPartial("~/Views/Project/Partials/CreatePartial.ascx", Model); %>
    
    <%--<a id="button-create-project" class="fg-button w380 ui-state-default ui-corner-all" href="#">Create Completely New Project</a>--%>
    <%--<a id="createNewTip" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="<%=Url.Action("Create","Project", new {tipYear=Model.TipSummary.TipYear}) %>">--%>
    <%--<span class="ui-icon ui-icon-circle-plus"></span>Create New Project</a>--%>
    
    </div>
    </div>
</div>

</asp:Content>


