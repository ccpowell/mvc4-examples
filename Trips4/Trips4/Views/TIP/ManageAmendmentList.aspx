<%@ Page Title="" Language="C#" 
    MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.ProjectListViewModel>" %>
    
<%@ Import Namespace="MvcContrib.UI.Grid"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Manage Amendments</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">

<script type="text/javascript" charset="utf-8">
    $(document).ready(function() {
    $('#projectListGrid').dataTable({
        "iDisplayLength": 25,
        "aaSorting": [[1, "desc"]],
        "aoColumns": [{ "bSortable": false }, null, null, null, null, null, null, null, null]              
        });
});
</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
    <h2 style="float:left;">Manage Amendments to a Project in the <%=Model.TipSummary.TipYear%> TIP </h2>
    <div class="clear"></div>
      
    <div class="tab-content-container">
        <h2>Current Managable Amendments</h2>

       <%= Html.Grid(Model.ProjectList).Columns(column => {
            column.For(x => Html.ActionLink("Manage", "Amendments", new { controller="Project", year = Model.TipSummary.TipYear, id = x.ProjectVersionId })).Encode(false);
            column.For(x => x.TipYear).Named("TIP Year");  
            column.For(x => x.TipId).Named("TIPID");            
            column.For(x => x.Title).Named("Title");
            column.For(x => x.SponsorAgency).Named("Primary<br>Sponsor");
            column.For(x => x.ProjectType).Named("Project<br>Type");
            column.For(x => x.VersionStatus).Named("Version<br>Status");
            column.For(x => x.AmendmentStatus).Named("Amendment<br>Status");
            column.For(x => x.AmendmentDate).Named("Amendment<br>Date");
        }).Attributes(id=> "projectListGrid") %>
    
        <div class="belowTable">
            <span class="ui-icon ui-icon-circle-triangle-w"></span>Return to TIP Amendment Page</a>
        </div>
    </div>
</div>
    

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
