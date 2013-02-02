<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<PoolListViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">TIP Pool List</asp:Content>
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
<h2><%=Html.ActionLink("TIP List", "Index",new {controller="TIP"}) %> / TIP <%=Model.TipSummary.TipYear%></h2>
<div class="clear"></div>
<%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>
<div class="tab-content-container">
<p>Pools List UI</p>
<p>As ot 8/13/09 the database provided to DTS does not contain pool information.</p>
</div>


    <div class="clear"></div>
    <div class="belowTable">
        
        <a id="createPool" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="<%=Url.Action("Create","Pool", new {tipYear=Model.TipSummary.TipYear}) %>">
        <span class="ui-icon ui-icon-circle-plus"></span>Create New Pool</a>
            
           
    </div>  

</div>

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
