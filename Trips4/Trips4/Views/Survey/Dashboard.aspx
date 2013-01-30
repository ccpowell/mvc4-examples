<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.Survey.DashboardViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Survey
    <%=Model.Current.Name%>
    Dashboard</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">
    <%=Model.Current.Name%>
    Survey Projects</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.dataTables.css") %>" rel="stylesheet"
        type="text/css" />
    <script src="<%= Url.Content("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/SurveyDashboard.js")%>" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        var App = App || {};
        App.pp = {
            SurveyYear: '<%= Model.Year %>',
            SurveyId: <%= Model.Current.Id %>,
            SurveyName: '<%= Model.Current.Name %>'
        };

        $(document).ready(function () {
            var baseUrl = '<%=Url.Action("Dashboard","Survey",new {year=Model.Current.Name}) %>';
            function refresh() {
                var url = baseUrl + '?listType=' + $('#projectFilter').val() + '&showAll=' + $('#projectFilterShowEmpty').prop("checked");
                window.location = url;
                return false;
            }

            $('#projectFilter').change(refresh);
            $('#projectFilterShowEmpty').change(refresh);
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="view-content-container">
        <div class="clear">
        </div>
        <%Html.RenderPartial("~/Views/Survey/Partials/TabPartial.ascx", Model.Current); %>
        <div class="tab-form-container">
            <h2>
                Summary of Projects in
                <% if (false /* Model.RtpStatus.IsCurrent */ )
                   { %>Adopted
                <% } %>
                Survey
                <%=Model.Current.Name%></h2>
            <label for="projectFilter">
                View Projects by:</label>
            <%=Html.DropDownList("projectFilter", Model.GetProjectListTypes())%>
            <br />
            <label for="projectFilterShowEmpty">
                Show empty items:</label>
            <%=Html.CheckBox("projectFilterShowEmpty", Model.ShowAll)%>
            <% Html.Grid(Model.DashboardItems)
            .Columns(column =>
            {
                column.For(x => Html.ActionLink("View Projects", "ProjectList", new { controller = "Survey", year = Model.Current.Name, df = x.ItemName, dft = Model.ListType })).Encode(false);
                column.For(x => x.ItemName).Named(Model.ListType.ToString());
                column.For(x => x.count).Named("Count");
                if (Model.ListType == DRCOG.Domain.Enums.SurveyDashboardListType.Sponsor)
                {
                    column.For(x => x.ItemDate.HasValue ? x.ItemDate.Value.ToShortDateString() : "not yet").Named("Printed Certification");
                }
            }).Attributes(@class => "grid w600").Empty("No Minor or Collector Roadway Projects were found. Please add projects if available.").Render();
            %>
        </div>
        <%--<div class="helpContainer">
        <h2>RTP Dashboard</h2>        
        <p>Use this form to get the basic over view of the RTP</p>    
            <div class="ui-widget">
			</div>          
    </div>--%>
        <div class="clear">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
