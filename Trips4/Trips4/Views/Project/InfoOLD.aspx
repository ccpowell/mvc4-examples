<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<InfoViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">DRCOG :: Project General Information</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>

<script type="text/javascript">
    var isDirty = false, formSubmittion = false;

    $(document).ready(function() {
        // Prevent accidental navigation away
        $(':input', document.dataForm).bind("change", function() { setConfirmUnload(true); });
        $(':input', document.dataForm).bind("keyup", function() { setConfirmUnload(true); });
        //disable the onbeforeunload message if we are using the submitform button
        if ($('#submitForm')) {
            $('#submitForm').click(function() { window.onbeforeunload = null; return true; });
        }

        //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
        $("#dataForm").validate({
            submitHandler: function(form) {
                $(form).ajaxSubmit({   
                    dataType: 'json',                 
                    success: function(response) {
                        $('#result').html(response.message);                        
                        $('#submitForm').addClass('ui-state-disabled');
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        $('#result').text(data.message);
                        $('#result').addClass('error');
                    }
                });
            }
        });
    });

    function setConfirmUnload(on) {
        $('#submitForm').removeClass('ui-state-disabled');
        $('#result').html("");           
        window.onbeforeunload = (on) ? unloadMessage : null;
    }

    function unloadMessage() {
        return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
    }

    $('#SponsorId').change(function() {
        var sponsorId = $('#SponsorId').val();
        $.getJSON('<%= Url.Action("UpdateAvailableSponsorContacts")%>/' + sponsorId, null, function(data) {
            $('#SponsorContactId').fillSelect(data);
        });
    });

</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container">
    <% Html.RenderPartial("~/Views/Project/Partials/BreadcrumbPartial.ascx", Model.ProjectSummary); %>
    <div class="clear"></div>
    <% Html.RenderPartial("~/Views/Project/Partials/ProjectTabPartial.ascx", Model.ProjectSummary); %>
    
    <div class="tab-form-container">    
    <% using (Html.BeginForm("UpdateInfo", "Project", FormMethod.Post, new { @id = "dataForm" })) %>
    <%{ %>
        <fieldset>
        <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
        <%= Html.Hidden("InfoModel.TipYear", Model.InfoModel.TipYear)%>         
        <%= Html.Hidden("InfoModel.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
        <%= Html.Hidden("InfoModel.ProjectId", Model.ProjectSummary.ProjectId)%>
        
        <p>
            <label>Project Name:</label>
            <%= Html.DrcogTextBox("InfoModel.ProjectName", 
                    Model.ProjectSummary.IsEditable(), 
                    Model.InfoModel.ProjectName, 
                    new { @class = "longInputElement required", title="Please enter a project title." })%>
        </p>
        <p>
            <label>Sponsor:</label>
            <%= Html.DropDownList("InfoModel.SponsorId",
                                Model.ProjectSummary.IsEditable(),
                                new SelectList(Model.AvailableSponsors, "key", "value", Model.InfoModel.SponsorId), 
                "-- Select a Sponsor Agency--",
                new { @class = "mediumInputElement required", title = "Please enter a project sponsor agency." })%>
        </p>
        <p>
            <label>Sponsor Contact:</label>
            <%-- We have two blocks b/c during dev't there were no sponsors available --%>
            <%if (Model.AvailableSponsorContacts != null)
                  {%>                
                    <%= Html.DropDownList("InfoModel.SponsorContactId",
                                                Model.ProjectSummary.IsEditable(),
                                                new SelectList(Model.AvailableSponsorContacts, "key", "value", Model.InfoModel.SponsorContactId),
                        "-- Select a Sponsor Contact --", 
                        new { @class = "mediumInputElement not-required", title="Please select a project sponsor" })%>
                <%}
                  else
                  { %>
                    <select id="InfoModel_SponsorContactId" 
                        name="InfoModel.SponsorContactId" 
                        class="mediumInputElement not-required" title="Please select a project sponsor">
                    <option selected>-- Select a Sponsor Contact --</option>
                    </select>
                <%} %>    
        </p>
        <p>
            <label>Admin Level:</label>
            <%= Html.DropDownList("InfoModel.AdministrativeLevelId",
                                Model.ProjectSummary.IsEditable(), 
                new SelectList(Model.AvailableAdminLevels, "key", "value", Model.InfoModel.AdministrativeLevelId),
                "-- Select Admin Level --", 
                new { @class = "mediumInputElement required", title="Please select an Admin Level" })%>
                <span>Facility Responsibility Level</span>
        </p>
       
        <p>
            <label>Project Type:</label> 
            <%= Html.DropDownList("InfoModel.ProjectTypeId",
                                Model.ProjectSummary.IsEditable(), 
                new SelectList(Model.AvailableProjectTypes, "key", "value", Model.InfoModel.ProjectTypeId), 
                "-- Select Project Type--", 
                new { @class = "mediumInputElement required", title="Please select a project type" })%>
        </p>
         <p>
            <label>Improvement Type:</label>
            <%= Html.DropDownList("InfoModel.ImprovementTypeId",
                        Model.ProjectSummary.IsEditable(), 
                        new SelectList(Model.AvailableImprovementTypes, "key", "value", Model.InfoModel.ImprovementTypeId), 
                        "-- Select Improvment Type--", 
                        new { @class = "mediumInputElement required", title="Please select an improvment type." })%>
        </p>
        <p>
            <label>Road or Transit:</label>
            <%= Html.DropDownList("InfoModel.TransportationTypeId",
                                Model.ProjectSummary.IsEditable(), 
                new SelectList(Model.AvailableRoadOrTransitTypes, "key", "value", Model.InfoModel.TransportationTypeId), 
                "-- Select --", 
                new { @class = "mediumInputElement required", title="Please specify if this is a Road or transit project" })%>
        </p>
        <p>
            <label>Pool Name:</label>
              <%= Html.DropDownList("InfoModel.ProjectPoolId",
                                Model.ProjectSummary.IsEditable(),
                                  new SelectList(Model.AvailablePools, "key", "value", Model.InfoModel.ProjectPoolId), 
                "-- Select Pool --",
                                  new { @class = "mediumInputElement not-required", title = "Please specify a pool for this project (if applicable)" })%>
        </p>
        <br />
        <p>
            <label>Pool Master:</label>                               
            <br />
        </p>
        <p>
            <label>Selection Agency:</label>
                <%= Html.DropDownList("InfoModel.SelectionAgencyId",
                        Model.ProjectSummary.IsEditable(),
                        new SelectList(Model.AvailableSelectionAgencies, "key", "value", Model.InfoModel.SelectionAgencyId),
                        "-- Select a Selection Agency --", 
                        new { @class = "mediumInputElement not-required", title="Please select an Agency" })%>
        </p>
        <p><label>Sponsor Notes:</label></p>
        <p><textarea class="longInputElement required" title="Please add the sponsor comments" name="InfoModel.SponsorNotes" rows="10" cols="50"><%=  Model.InfoModel.SponsorNotes%></textarea></p>
        
        <p><label>DRCOG Notes:</label></p>
        <p><textarea class="longInputElement" name="InfoModel.DRCOGNotes" rows="10" cols="50"><%=  Model.InfoModel.DRCOGNotes%></textarea></p>
                                   
        <%if(Model.ProjectSummary.IsEditable()){ %>
            <p>
            <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all" >Save Changes</button>
            <div id="result"></div>
            </p>
        <%} %>
        
        </fieldset>
    <%} %>
</div>
   
   <% Html.RenderPartial("~/Views/Project/Partials/ProjectSummaryBoxPartial.ascx", Model.ProjectSummary); %>
   
<div class="clear"></div>
</div>
<script type="text/javascript">

    


</script>

</asp:Content>



