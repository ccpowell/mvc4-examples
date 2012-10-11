<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.AmendmentsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">TIP Amendments</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="view-content-container">
<%--<h2 ><%=Html.ActionLink("TIP List", "Index",new {controller="TIP"}) %> / TIP <%=Model.TipSummary.TipYear%></h2>--%>
<div class="clear"></div>
<%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>

    <div class="leftColumn">
    <h2>Amendment Management</h2>
        <%=Html.ActionLink("Manage Amendments of an Existing Project", "ManageAmendmentList", new { controller = "Tip", year = Model.TipSummary.TipYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>
        <%-- <%=Html.ActionLink("Create New Amendment for Existing Project", "CreateAmendmentList", new { controller = "Tip", year = Model.TipSummary.TipYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>
        <%=Html.ActionLink("Edit or Delete Amendment for Existing Project *", "EditAmendmentList", new { controller = "Tip", year = Model.TipSummary.TipYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%> --%>
        <a id="button-create-project" class="fg-button w380 ui-state-default ui-corner-all" href="#">Create Completely New Project</a>
        <a id="button-restore-project" class="fg-button w380 ui-state-default ui-corner-all" href="#">Restore a Project</a>
        <%-- <%=Html.ActionLink("Restore a Project *", "RestoreProjectList", new { controller = "Tip", year = Model.TipSummary.TipYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>--%>        
        <%--<%=Html.ActionLink("Move a Waiting List Project into the TIP *", "WaitingList", new { controller = "Tip", year = Model.TipSummary.TipYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>--%>     
        <%--<%=Html.ActionLink("Move a Non-TIP Project into the TIP *", "NonTipList", new { controller = "Tip", year = Model.TipSummary.TipYear }, new { @class = "fg-button w380 ui-state-default ui-corner-all" })%>--%>
        <div class="clear"></div>
        <%--<div class="ui-widget">
			<div style="padding: 0pt 0.7em;" class="ui-state-error ui-corner-all"> 
				<p>
				    <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span> 
				    <strong>*:</strong>These functions are under construction.
				</p>
			</div>
		</div>--%>
    </div>
    <div class="rightColumn">
        <h2>Process Amendments</h2>
        <a id="btn-amendprojects" class="fg-button w380 ui-state-default ui-corner-all">Amend Projects</a>
        <%-- this one needs attention --%>
        <%--<%=Html.ActionLink("Show List of Proposed Amendments *", "ProposedAmendmentList", new { controller = "Tip", year = Model.TipSummary.TipYear }, new { @id = "proposed-list", @class = "fg-button w380 ui-state-default ui-corner-all" })%>--%>
        
        <%--<a class="fg-button w380 ui-state-default ui-corner-all">Set Projects from Proposed to Approved</a>--%>
        <div class="clear"></div>
        <%--<div class="ui-widget">
			<div style="padding: 0pt 0.7em;" class="ui-state-error ui-corner-all"> 
				<p>
				    <span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span> 
				    <strong>*:</strong>These functions are under construction.
				</p>
			</div>
		</div>--%>
    </div>
    <div class="clear"></div>
            
</div>

<div style='display:none'>
    <div id="dialog-restore-project" class="dialog" title="Restore project from TIPYear ...">
        <h2>Restore project from TIPYear</h2>
        <div class="error" style="display:none;">
          <span></span>.
        </div>
        <form>
        <fieldset>
            <p>
                <label for="tipYearDestination">TIP Year to:</label>
                <input type="text" id="tipYearDestination" name="tipYearDestination" class="big" readonly="true" value="<%=Model.TipSummary.TipYear.ToString()%>" />
            </p>
            <p>For the search, you may enter either/or of the values below.</p>
            <p>
                <label for="tipYearSourceID">TIP Year from:</label>
                <%= Html.DropDownListFor(x => x.RestoreYears, new SelectList(Model.RestoreYears, "Key", "Value"), new { @id = "tipYearSourceID", @class = "mediumInputElement big" })%>
                <%--<select id="tipYearSourceID" name="tipYearSourceID" class="mediumInputElement big" size="1"></select>--%>
            </p>
            <p>
                <label for="tipSourceID">TIP ID:</label>
                <input type="text" id="tipSourceID" name="tipSourceID" class="mediumInputElement big"></select>
            </p>
            <button type="submit" id="restore-project-list" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all" >View Available</button>
        </fieldset>
        </form>
    </div>
</div>

<% Html.RenderPartial("~/Views/Project/Partials/CreatePartial.ascx", Model); %>
<% Html.RenderPartial("~/Views/TIP/Partials/AmendPartial.ascx", Model); %>

<script type="text/javascript">
    
    $().ready(function() {
        //hook up all the click events for the buttons
        $('#proposed-list').colorbox({ width: "900px", height: "500px", iframe: true });
        
    });

    var showRestoreCandidatesUrl = "<%=Url.Action("RestoreProjectList","TIP")%>";

    $(function() {
        $("#button-restore-project").colorbox({
            width: "500px",
            height: "400px",
            inline: true,
            href: "#dialog-restore-project",
            onLoad: function() {
//            $.getJSON('<%= Url.Action("GetRestoreYears")%>/', { tipYearDestination: "<%= Model.TipSummary.TipYear.ToString() %>" }, function(data) {
//             alert("in");
//                    $('#tipYearSourceID').fillSelect(data);
//                });
            }
        });

    });
    
    $('#restore-project-list').click(function() {
        var yearDestination = $("#tipYearDestination").val(),
            yearSourceID = $("#tipYearSourceID").val(),
            tipSourceID = $("#tipSourceID").val();

        //reset form values
        //$('#tipYearDestination').val("");
        //$('#tipYearSourceID').val("");

        //Controller action: RestoreProjectList(string yearDestination, int yearSourceID, string tipID)
        //var redirectActionUrl = "/TIP/" + yearDestination + "/RestoreProjectList?yearDestination=" + yearDestination + "&yearSourceID=" + yearSourceID + "&tipID=" + tipSourceID;
        
        var redirectActionUrl = "<%= Url.Action("RestoreProjectList","TIP", new { @id = "r_timeperiod", @yearDestination = "r_dtimeperiod", @yearSourceID = "r_yearSourceID", @tipID = "r_tipSourceID" }) %>"
        
        redirectActionUrl = redirectActionUrl.replace("r_timeperiod", yearDestination);
        redirectActionUrl = redirectActionUrl.replace("r_dtimeperiod", yearDestination);
        redirectActionUrl = redirectActionUrl.replace("r_yearSourceID", yearSourceID);
        redirectActionUrl = redirectActionUrl.replace("r_tipSourceID", tipSourceID);
        

        location = redirectActionUrl;
        
        return false;
    });
</script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
