<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Account Search</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Page.ResolveClientUrl("~/Content/js/DTS/AccountSearch.js")%>" type="text/javascript"></script>
    <script src="<%= Page.ResolveClientUrl("~/Content/js/DTS/AccountDetail.js")%>" type="text/javascript"></script>
    
    <link href="<%=Page.ResolveClientUrl("~/Content/js/dojo-release-1.3.1/dojox/grid/resources/Grid.css")%>" rel="stylesheet" type="text/css" />
    <link href="<%=Page.ResolveClientUrl("~/Content/js/dojo-release-1.3.1/dojox/grid/resources/tundraGrid.css")%>" rel="stylesheet" type="text/css" />
   
    <link href="<%=Page.ResolveClientUrl("~/Content/PanelView.css")%>" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="<%=Page.ResolveClientUrl("~/Content/js/DTS/css/SiteTabsPartial.css")%>" />   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="leftPane" >
        <%-- Html.RenderPartial("~/Views/Account/Partials/AccountSearchPartial.ascx", ViewData.Model.SearchModel); --%>
    </div>
    <div id="mainPane" >
      <%-- Html.RenderPartial("~/Views/Account/Partials/AccountDetailPartial.ascx", ViewData.Model.DetailModel); --%>
    </div>
    <div class="clear"></div>
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
