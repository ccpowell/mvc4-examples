<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.InfoViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Create New Project</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="tab-content-container">
<h2>Create a new Project in the <%= Model.ProjectSummary.TipYear%> TIP</h2>
<div class="clear"></div>
<div class="tab-form-container">
    <% using (Html.BeginForm("Create", "Project", FormMethod.Post, new { @id = "infoForm" })) %>
    <%{ %>
        <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
        <%= Html.Hidden("InfoModel.TipYear", Model.ProjectSummary.TipYear)%>             
        
        <table style="margin:0 auto;">
            <tr>
                <td><label>Project Name</label></td>
                <td><%= Html.DrcogTextBox("InfoModel.ProjectName", true, null, new { @class = "" })%></td>
            </tr>
            <tr>
                <td><label>Sponsor</label></td>
                <td><%= Html.DropDownList("InfoModel.SponsorId", new SelectList(Model.AvailableSponsors, "key", "value", Model.InfoModel.SponsorId), "-- Select --", new { @class = "" })%></td>
            </tr>
            <tr>
                <td><label>Sponsor Contact</label></td>
                <td>
                <%if (Model.AvailableSponsorContacts != null)
                  {%>                
                    <%= Html.DropDownList("InfoModel.SponsorContactId", new SelectList(Model.AvailableSponsorContacts, "key", "value", Model.InfoModel.SponsorContactId), "-- Please select a sponsor --",new { @class = "" })%>
                <%}
                  else
                  { %>
                    <select id="InfoModel_SponsorContactId" name="InfoModel.SponsorContactId" class="">
                    <option selected>-- Please select a sponsor --</option>
                    </select>
                <%} %>
                </td>
            </tr>
            <tr>
                <td><label>Admin Level</label></td>
                <td><%= Html.DropDownList("InfoModel.AdministrativeLevelId", new SelectList(Model.AvailableAdminLevels, "key", "value", Model.InfoModel.AdministrativeLevelId), "-- Select --",new { @class = "" })%></td>
            </tr>
            <tr>
                <td><label>Improvement Type</label></td>
                <td><%= Html.DropDownList("InfoModel.ImprovementTypeId", new SelectList(Model.AvailableImprovementTypes, "key", "value", Model.InfoModel.ImprovementTypeId), "-- Select --", new { @class = "" })%></td>
            </tr>
            <tr>
                <td><label>Project Type</label></td>
                <td><%= Html.DropDownList("InfoModel.ProjectTypeId", new SelectList(Model.AvailableProjectTypes, "key", "value", Model.InfoModel.ProjectTypeId), "-- Select --", new { @class = "" })%></td>
            </tr>
            <tr>
                <td><label>Road or Transit</label></td>
                <td><%= Html.DropDownList("InfoModel.TransportationTypeId", new SelectList(Model.AvailableRoadOrTransitTypes, "key", "value", Model.InfoModel.TransportationTypeId), "-- Select --", new { @class = "" })%></td>
            </tr>
            <tr>
                <td><label>Pool Name</label></td>
                <td>No Pool data in database</td>
            </tr>
            <tr>
                <td><label>Pool Master</label></td>
                <td><%=Html.CheckBox("InfoModel.IsPoolMaster",true,Model.InfoModel.IsPoolMaster) %></td>
            </tr>
            <tr>
                <td><label>Selection Agency</label></td>
                <td>Not clear now to get this from the database<%--<%= Html.DropDownList("SelectionAgencyId", new SelectList(Model.AvailableSelectionAgencies, "key", "value", Model.InfoModel.SelectionAgencyId), "-- Select --")%>--%></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>   
            <tr>
                <td colspan="2">
                    <label>Sponsor Notes</label><br />
                    <%= Html.TextArea("InfoModel.SponsorNotes", "", 5, 50, Model.InfoModel.SponsorNotes)%>
                </td>
            </tr>
            <tr>    
                <td colspan="2">
                    <label>DRCOG Notes</label><br />
                    <%= Html.TextArea("InfoModel.DRCOGNotes", "", 5, 50, Model.InfoModel.DRCOGNotes)%>
                    <p class="question">Should the DRCOG Notes be available?</p>
                </td>
            </tr>
            <tr>
                <td><button id="submitForm" type="submit" class="fg-button ui-state-disabled ui-corner-all" >Create Project</button></td>
                <td><a href="#">Cancel</a></td>
            </tr>
        </table>
    <%} %>
</div>
 <div class="helpContainer">        
        <h2>Create new Project</h2>
        <p>Fill out the fields in this form and click Save to create a new project.</p>
        <p>After clicking Save, you will be taken the the standard Project Editing interface.</p>                
    </div>
<div class="clear"></div>
</div>
<script type="text/javascript">

    $().ready(function() {
        // Prevent accidental navigation away
        $(':input', document.infoForm).bind("change", function() { setConfirmUnload(true); }); 
        $(':input', document.infoForm).bind("keyup", function() { setConfirmUnload(true); });
        $('#submitForm').click(function() { window.onbeforeunload = null; return true; });
    });
    
    function setConfirmUnload(on) {
        $('#submitForm').removeClass('ui-state-disabled');        
        window.onbeforeunload = (on) ? unloadMessage : null;
    }

    function unloadMessage() {
        return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
    }

    //Ajax functions
    $('#InfoModel_SponsorId').change(function() {
        var sponsorId = $('#InfoModel_SponsorId').val();
        
        $.getJSON('<%= Url.Action("UpdateAvailableSponsorContacts")%>/' + sponsorId, null, function(data) {
            $('#InfoModel_SponsorContactId').fillSelect(data);
        });
    });


</script>
 

</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
