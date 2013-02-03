<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.FundingSourceListViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">RTP Funding List</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="HeaderContent" runat="server">
<script type="text/javascript" charset="utf-8">
    $(document).ready(function() {
        $('#fundingListGrid').dataTable({
            "iDisplayLength": 10,
            "aaSorting": [[1, "asc"]],
            "aoColumns": [{ "bSortable": false, "bVisible": false }, null, null, null, null]
        });
    });
</script>

    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.RtpYear = '<%=  Model.RtpSummary.RtpYear %>';
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
    <div class="clear"></div>
    <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>
    <div class="tab-content-container">
        <h2>Funding Sources</h2>
        <%= Html.Grid(Model.FundingSources).Columns(column => {
            column.For(x => Html.ActionLink("Details", "Detail", new {controller="Funding", tipYear=x.Plan, id = x.FundingTypeId})).Encode(false);
            column.For(x => x.FundingType);            
            column.For(x => x.Code);
            //column.For(x => x.Selector);
            column.For(x => x.SourceOrganization.OrganizationName).Named("Source");        
            column.For(x => x.RecipentOrganization.OrganizationName).Named("Recipient");
            //column.For(x => x.IsDiscretionary).Named("Discretion");
            }).Attributes(id => "fundingListGrid")%>
         	        
        <div class="clear"></div>
        <div class="belowTable" style="display: none;">
            <a id="createFundingSource" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="<%=Url.Action("Create","Funding", new {plan=Model.RtpSummary.RtpYear}) %>">
            <span class="ui-icon ui-icon-circle-plus"></span>Create New Funding Source</a>
        </div> 
    </div> 
</div>

</asp:Content>


<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
