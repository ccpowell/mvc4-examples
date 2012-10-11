<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.AmendmentsViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Project General Information
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />

<link href="<%= ResolveUrl("~/Content/jquery.dataTables.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript"></script>
<%--<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.amendement.validate.js")%>" type="text/javascript"></script>--%>
<script type="text/javascript" charset="utf-8">
    $(document).ready(function() {
        $('#projectListGrid').dataTable({
            "iDisplayLength": 10,
            "aaSorting": [[2, "desc"]],
            "aoColumns": [{ "bSortable": false }, null, null]
        });
        $('#projectListGrid_length').attr("style", "display:none");
    });
        
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="view-content-container" style="height:520px;">
       <% Html.RenderPartial("~/Views/RtpProject/Partials/ProjectGenericPartial.ascx", Model.RtpSummary); %>
        
        <div class="tab-content-container">
            <%= Html.Grid(Model.AmendmentList).Columns(column =>
                {
                    //column.For(x => Html.ActionLink("View", "Details", new { controller = "Project", year = Model.ProjectSummary.TipYear, id = x.ProjectVersionId })).Encode(false);
                    column.For(x => Html.ActionLink(x.ProjectName, "Details", new { controller = "Project", year = Model.RtpSummary.RtpYear, id = x.ProjectVersionId })).Named("Title").Encode(false);
                    //column.For(x => x.ProjectName).Named("Title");
                    column.For(x => x.AmendmentStatus).Named("Amendment Status");
                    column.For(x => x.AmendmentDate).Named("Amendment Date");
                }).Attributes(id => "projectListGrid")%>
            <div id="projectAmendmentsForm" class="formContainer leftColumn" style="margin-left:20px;">
            <%if (Model.RtpSummary.IsTopStatus)
              { %>
                <a id="confirmAmend" class="fg-button w380 ui-state-default ui-corner-all" href="#">
                    <% if (new List<String> { "Approved", "Amended" }.Contains(Model.RtpSummary.AmendmentStatus))
                       { %>Create Amendment
                    <% }
                       else if (Model.RtpSummary.AmendmentStatus.Equals("Submitted"))
                       { %>Move to Proposed
                    <% }
                       else if (Model.RtpSummary.AmendmentStatus.Equals("Proposed"))
                       { %>Amend Project <% } %>
                    </a>
                <% // Can only delete if IsEditable and the VersionModel says it is eligible for delete
                    if (!Model.RtpSummary.PreviousVersionId.Equals(default(int)) && Model.RtpSummary.IsEditable() && Model.RtpSummary.CanDelete(Model.RtpSummary.AmendmentStatus)) 
                   { %>
                      <%=Html.ActionLink("Delete Amendment", "DeleteAmendment", new { controller = "Project", projectVersionId = Model.RtpSummary.ProjectVersionId, previousProjectVersionId = Model.RtpSummary.PreviousVersionId }, new { @id = "confirm-delete", @class = "fg-button w380 ui-state-default ui-corner-all deleteAmendment" })%>
                <% } %>
                <% } %>
                
                <div class="clear"></div>
            </div>
        </div>
    </div>
    <!-- This contains the hidden content for inline calls -->
	<div style='display:none'>
		<div id='confirmDeleteContent' style='padding:10px; background:#fff;'>
		    <% Html.RenderPartial("~/Views/RtpProject/Partials/AmendProjectPartial.ascx", Model.RtpSummary); %>
		</div>
	</div>
	

<script type="text/javascript">
    //$('#confirm-delete').colorbox({ width: "80%", height: "80%", iframe: true });
    $("#confirmAmend").colorbox({ width: "800px", height: "290px", inline: true, href: "#confirmDeleteContent" });

</script>    

</asp:Content>
