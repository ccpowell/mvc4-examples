<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	PasswordChanged
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <div class="view-content-container">
    <h2>My Profile</h2>
    <p>Your password was successfully updated. You can continue to work with the application, but the next time you login, you will need to use your new password.</p>
    <%=Html.ActionLink("Return Home","Index",new{controller="Home"}) %>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
