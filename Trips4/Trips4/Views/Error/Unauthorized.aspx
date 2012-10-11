<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<UnauthorizedViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Unauthorized Access
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
    <h2>Unauthorized Resource Requested</h2>
    <p>You have requested an unauthorized resource. If you feel this is in error please contact the system administrator.</p>
    <p>Message: <%=ViewData.Model.Message %></p>
</div>
</asp:Content>

