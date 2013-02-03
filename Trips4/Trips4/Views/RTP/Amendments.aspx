<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.RtpBaseViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">RTP Amendments</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.RtpYear = '<%=  Model.RtpSummary.RtpYear %>';
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="view-content-container">
<h2 ><%=Html.ActionLink("RTP List", "Index",new {controller="RTP"}) %> / RTP <%=Model.RtpSummary.RtpYear%></h2>
<div class="clear"></div>
<%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>

    <div class="leftColumn">
    <h2>Start New Amendment to the RTP</h2>
        <%=Html.ActionLink("Create New Amendment for Existing Project", "CreateAmendmentList", new { controller = "Rtp", year = Model.RtpSummary.RtpYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>
        <%=Html.ActionLink("Edit or Delete Amendment for Existing Project", "EditAmendmentList", new { controller = "Rtp", year = Model.RtpSummary.RtpYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>
        <a id="button-create-project" class="fg-button w380 ui-state-default ui-corner-all" href="#">Create Completely New Project</a>
        <%=Html.ActionLink("Restore a Project", "RestoreProjectList", new { controller = "Rtp", year = Model.RtpSummary.RtpYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>        
        <%=Html.ActionLink("Move a Waiting List Project into the TIP", "WaitingList", new { controller = "Rtp", year = Model.RtpSummary.RtpYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>        
        <%=Html.ActionLink("Move a Non-TIP Project into the TIP", "NonTipList", new { controller = "Rtp", year = Model.RtpSummary.RtpYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>                
        <div class="clear"></div>
        <div class="ui-widget">
				<div style="padding: 0pt 0.7em;" class="ui-state-error ui-corner-all"> 
					<p>
					<span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span> 
					<strong>Note:</strong>These are in progress.</p>
				</div>
			</div>
    </div>
    <div class="rightColumn">
    <h2>Process Amendments</h2>
        <%=Html.ActionLink("Show List of Proposed Amendments", "ProposedAmendmentList", new { controller = "Rtp", year = Model.RtpSummary.RtpYear }, new { @id = "proposed-list", @class = "fg-button w380 ui-state-default ui-corner-all" })%>
        <a class="fg-button w380 ui-state-default ui-corner-all">Set Projects from Proposed to Approved</a>
        <div class="clear"></div>
        <div class="ui-widget">
			<div style="padding: 0pt 0.7em;" class="ui-state-error ui-corner-all"> 
				<p>
				    <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span> 
				    <strong>Note:</strong>These are in progress.
				</p>
			</div>
		</div>
    </div>
    <div class="clear"></div>
            
</div>

<% Html.RenderPartial("~/Views/RTPProject/Partials/CreatePartial.ascx", Model.RtpSummary); %>

<script type="text/javascript">
    
    $().ready(function() {
        //hook up all the click events for the buttons
    $('#proposed-list').colorbox({ width: "900px", height: "500px", iframe: true });
        
    });

</script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
