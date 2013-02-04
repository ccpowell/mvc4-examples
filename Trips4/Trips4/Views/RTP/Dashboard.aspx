<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<RtpDashboardViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">RTP Dashboard</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
<div class="clear"></div>

<%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>

    <div class="tab-form-container">
       <h2>Summary of Projects in <% if( false /* Model.RtpStatus.IsCurrent */ ) { %>Adopted <% } %> Regional Transportation Plan <%=Model.RtpSummary.RtpYear%></h2>
        <label for="projectFilter">View Projects by:</label>
        <%=Html.DropDownList("projectFilter", Model.GetProjectListTypes())%>
        <br />
        <% Html.Grid(Model.DashboardItems)
            .Columns(column =>
            {
                column.For(x => Html.ActionLink("View Projects", "ProjectList", new { controller = "RTP", year = Model.RtpSummary.RtpYear, df = x.ItemName, dft=Model.ListType })).Encode(false);
                column.For(x => x.ItemName).Named(Model.ListType.ToString());
                column.For(x => x.count).Named("Count");
            }).Attributes(@class =>"grid w400").Render();
            %>
    </div>         
    
    <%--<div class="helpContainer">
        <h2>RTP Dashboard</h2>        
        <p>Use this form to get the basic over view of the RTP</p>    
            <div class="ui-widget">
			</div>          
    </div>--%>

<div class="clear"></div>
</div>
<script type="text/javascript">
    var baseUrl = '<%=Url.Action("Dashboard","RTP",new {year=Model.RtpSummary.RtpYear}) %>';
    $(document).ready(function() {
        $('#projectFilter').change(function() {
            var url = baseUrl + '?listType=' + $('#projectFilter').val();
            window.location = url;
        });
    });
</script>
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
