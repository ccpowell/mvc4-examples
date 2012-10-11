<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.ProjectListViewModel>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Edit Amendments</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/jquery.dataTables.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.dataTables.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">
        $(document).ready(function() {
        $('#projectListGrid').dataTable({
            "iDisplayLength": 10,
            "aaSorting": [[1, "desc"]],
            "aoColumns": [{ "bSortable": false }, null, null, null, null,null]   
               
            });
    });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
<h2 style="float:left;">Edit an existing Amendment to a Project in the <%=Model.TipSummary.TipYear%> TIP </h2>
<div class="clear"></div>
      

    <div class="tab-content-container">
    <h2>Projects with Active Amendments</h2>

   <%= Html.Grid(Model.ProjectList).Columns(column => {
        column.For(x => Html.ActionLink("Edit", "Info", new { controller="Project", year = Model.TipSummary.TipYear, id = x.ProjectVersionId })).Encode(false);
	    column.For(x => x.TipId).Named("TIPID");            
        column.For(x => x.Title).Named("Title");
        column.For(x => x.SponsorAgency).Named("Sponsor");
        column.For(x => x.ProjectType).Named("Project Type");
        column.For(x => x.AmendmentStatus).Named("Amendment Status");
    }).Attributes(id=> "projectListGrid") %>

    
    <div class="belowTable">
    
    <a id="createNewTip" class="fg-button ui-state-default fg-button-icon-left ui-corner-all" href="<%=Url.Action("Amendments","Tip", new {tipYear=Model.TipSummary.TipYear}) %>">
        <span class="ui-icon ui-icon-circle-triangle-w"></span>Return to TIP Amendment Page</a>
    </div>
    </div>

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
