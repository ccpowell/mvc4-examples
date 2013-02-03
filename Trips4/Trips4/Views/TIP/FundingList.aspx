<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.FundingSourceListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">TIP Funding List</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeaderContent" runat="server">
<script type="text/javascript" charset="utf-8">
    $(document).ready(function() {
        $('#fundingListGrid').dataTable({
            "iDisplayLength": 10,
            "aaSorting": [[2, "asc"]],
            "aoColumns": [{ "bSortable": false, "bVisible" : false }, null, null, null, null, null,null]
        });
    });
</script>
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
<div class="clear"></div>
<%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>
     <div class="tab-content-container">
     
     <div id="button-container">
    <% if (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))
       { %>
        <a id="createFundingSource" class="fg-button ui-state-default fg-button-icon-left ui-corner-all">
        <span class="ui-icon ui-icon-circle-plus"></span>Create New Funding Source</a>
    <% } %>
    </div>
    <h2>Funding Sources</h2>
    <br />
    <%= Html.Grid(Model.FundingSources).Columns(column => {
        column.For(x => Html.ActionLink("Details", "Detail", new {controller="Funding", tipYear=x.TimePeriod, id = x.FundingTypeId})).Encode(false);
        column.For(x => "<a href='' class='updateFundingSource' id='source-" + x.FundingTypeId + "'>" + x.FundingType + "</a>").Encode(false);
        //column.For(x => x.FundingType);
        column.For(x => x.Code);
        column.For(x => x.Selector);
        column.For(x => x.SourceOrganization.OrganizationName).Named("Source");        
        column.For(x => x.RecipentOrganization.OrganizationName).Named("Recipient");
        column.For(x => x.IsDiscretionary).Named("Discretion");
        }).Attributes(id => "fundingListGrid")%>
     	        
    <div class="clear"></div>
    <div class="belowTable"></div> 
    </div> 
</div>

        <% Html.RenderPartial("~/Views/TIP/Partials/ManageFundingSourcePartial.ascx"); %>

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
