<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TipDashboardViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    TIP Dashboard</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.TipYear = '<%=  Model.TipSummary.TipYear %>';
        $(document).ready(App.tabs.initializeTipTabs);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="view-content-container">
        <%--<h2 ><%=Html.ActionLink("TIP List", "Index",new {controller="TIP"}) %> / TIP <%=Model.TipSummary.TipYear%></h2>--%>
        <div class="clear">
        </div>
        <% Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>
        <div class="tab-form-container">
            <h2>
                Summary of
                <%= Model.TipSummary.TipYear %>
                TIP Projects</h2>
            <label for="projectFilter">
                View Projects by:</label>
            <%= Html.DropDownList("projectFilter", Model.GetProjectListTypes())%>
            <br />
            <% Html.Grid(Model.DashboardItems)
            .Columns(column =>
            {
                column.For(x => Html.ActionLink("View Projects", "ProjectList", new { controller = "Tip", year = Model.TipSummary.TipYear, df = x.ItemName, dft = Model.ListType })).Encode(false).Attributes(@style => "width: 100px;");
                column.For(x => x.ItemName).Named(Model.ListType.ToString());
                column.For(x => x.count).Named("Count").Attributes(@style => "width: 50px;");
            }).Attributes(@class => "grid w850").Render(); 
            %>
            <div class="quickTasks" style="display: <%= (Model.DashboardItems.Count <= 0 ? "block" : "none") %>;">
                <h2>
                    Quick Tasks</h2>
                <span id="btn-restoreproject" class="fg-button w200 ui-state-default ui-corner-all">
                    Import Projects</span>
            </div>
        </div>
        <div style='display: none'>
            <div id="dialog-restore-project" class="dialog" title="Restore projects ...">
                <% Html.RenderPartial("~/Views/TIP/Partials/RestoreProjects.ascx", Model); %>
            </div>
        </div>
        <div class="helpContainer" style="display: none;">
            <h2>
                TIP Dashboard</h2>
            <p>
                Use this form to get the basic over view of the TIP</p>
            <div class="ui-widget">
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    <script type="text/javascript">
        var baseUrl = '<%=Url.Action("Dashboard","Tip",new {tipYear=Model.TipSummary.TipYear}) %>';
        $(document).ready(function () {
            $('#projectFilter').change(function () {
                var paramMarker = (baseUrl.indexOf('?') > -1) ? '&' : '?';
                var url = baseUrl + paramMarker + 'listType=' + $('#projectFilter').val();
                window.location = url;
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
