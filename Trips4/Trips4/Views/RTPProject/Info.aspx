<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.Project.InfoViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project General Information</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.ProjectSummary.RtpYear%></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= Page.ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript" ></script>
<script src="<%= Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript" ></script>
<link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript" ></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.sort.js")%>" type="text/javascript" ></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>" type="text/javascript"></script>

<script type="text/javascript">
    var isDirty = false, formSubmittion = false;
    var add1url = '<%=Url.Action("AddCurrent1Agency","RtpProject", new {year=Model.InfoModel.RtpYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
    var remove1url = '<%=Url.Action("DropCurrent1Agency","RtpProject", new {year=Model.InfoModel.RtpYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
    var add2url = '<%=Url.Action("AddCurrent2Agency","RtpProject", new {year=Model.InfoModel.RtpYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
    var remove2url = '<%=Url.Action("DropCurrent2Agency","RtpProject", new {year=Model.InfoModel.RtpYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';

    $(document).ready(function() {
        //$('#AvailableAgencies').removeAttr('multiple');
        //$('#Current2Agencies').removeAttr('multiple');
        //alert("<% =Model.ProjectSummary.IsActive  %>, <%=Model.ProjectSummary.IsEditable() %>");

        $(".growable").growing({ buffer: 5 });

        // Prevent accidental navigation away
        App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
        $('#submitForm').button({ disabled: true });

        // pre-submit callback 
        function showRequest(formData, jqForm, options) {
            // formData is an array; here we use $.param to convert it to a string to display it 
            // but the form plugin does this for you automatically when it submits the data 
            var queryString = $.param(formData);

            // jqForm is a jQuery object encapsulating the form element.  To access the 
            // DOM element for the form do this: 
            // var formElement = jqForm[0]; 

            alert('About to submit: \n\n' + queryString);

            // here we could return false to prevent the form from being submitted; 
            // returning anything other than false will allow the form submit to continue 
            return true;
        }

        function ShowMessageDialog(title, message) {
            $('#dialog').dialog('option', 'title', title);
            $('#dialogMessage').html(message);
            $('#dialog').dialog('open');
        }

        //Initialize the dialog
        $("#dialog").dialog({
            autoOpen: false,
            draggable: false,
            bgiframe: true,
            modal: true,
            buttons: {
                Ok: function() {
                    $(this).dialog('close');
                }
            }
        });
        
        //        $('#SponsorId').change(function() {
        //            var sponsorId = $('#SponsorId').val();
        //            $.getJSON('<%= Url.Action("UpdateAvailableSponsorContacts")%>/' + sponsorId, null, function(data) {
        //                $('#SponsorContactId').fillSelect(data);
        //            });
        //        });

        $('#ProjectSponsorsModel_PrimarySponsor_OrganizationId').change(function() {
            var sponsorId = $('#ProjectSponsorsModel_PrimarySponsor_OrganizationId').val();
            $.ajax({
                type: "POST",
                url: '<%= Url.Action("UpdateAvailableSponsorContacts")%>',
                data: "id=" + sponsorId,
                dataType: "json",
                success: function(response) {
                    //alert(response.id + ' ' + response.value);
                    $('#InfoModel_SponsorContactId').fillSelect(response);
                },
                error: function(response) {
                    //$('#result').html(response.error);
                }
            });
        });

        $('#InfoModel_ImprovementTypeId').bind('change', function() {
            var improvementtypeid = $('#InfoModel_ImprovementTypeId :selected').val();
            improvementtypeid = parseInt(improvementtypeid);

            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetImprovementTypeMatch")%>',
                data: "id=" + improvementtypeid,
                dataType: "json",
                success: function(response) {
                    //alert(response.id + ' ' + response.value);
                    $('#InfoModel_ProjectTypeId').val(response.id);
                    $('#InfoModel_ProjectType').text(response.value);
                },
                error: function(response) {
                    $('#result').html(response.error);
                }
            });
        });
    });
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% string value = String.Empty; %>
<div class="tab-content-container">
    <% Html.RenderPartial("~/Views/RTPProject/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
    
    <div class="tab-form-container">   
    <form method="put" action="/api/RtpProjectInfo" id="dataForm">
        <%Html.RenderPartial("~/Views/RtpProject/Partials/ManagerRibbonPartial.ascx", Model.ProjectSummary); %>
        <fieldset>
        <legend></legend>
        <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
        <%= Html.Hidden("InfoModel.RtpYear", Model.InfoModel.RtpYear)%>         
        <%= Html.Hidden("InfoModel.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
        <%= Html.Hidden("InfoModel.ProjectId", Model.ProjectSummary.ProjectId)%>
        <div id="tab-info-left">
        <p>
            <label>Project Name:</label>
            <%= Html.DrcogTextBox("InfoModel.ProjectName", 
                    Model.ProjectSummary.IsEditable(), 
                    Model.InfoModel.ProjectName, 
                    new { @class = "longInputElement required", title="Please enter a project title.", @MAXLENGTH = 100 })%>
        </p>
        <p>
            <label>Primary Sponsor:</label>
            <%= Html.DropDownList("ProjectSponsorsModel.PrimarySponsor.OrganizationId", 
                Model.ProjectSummary.IsEditable(),
                new SelectList(Model.ProjectSponsorsModel.GetAvailableAgenciesList(), "key", "value", Model.ProjectSponsorsModel.PrimarySponsor.OrganizationId),
                "-- Select a Primary Sponsor --", 
                new { @class = "mediumInputElement required", title="Please select a Primary Sponsor" })%>
        </p>
        <%if (Model.ProjectSummary.IsEditable()) { %>
        <p>
            <label>Sponsor Contact:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.SponsorContactId",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.AvailableSponsorContacts, "key", "value", Model.InfoModel.SponsorContactId),
                    "-- Select a Sponsor Contact --",
                    new { @class = "mediumInputElement not-required", title = "Please select a project sponsor" })%>
            <%}
              else
              {%> 
            <% value = "None Selected"; if (Model.InfoModel.SponsorContactId != null) { Model.AvailableSponsorContacts.TryGetValue((int)Model.InfoModel.SponsorContactId, out value); } %>
            <span class="fakeinput medium"><%= Html.Encode(value)%></span>
            <%= Html.Hidden("InfoModel.SponsorContactId", Model.InfoModel.SponsorContactId)%>
            <br />
            <% } %>
        </p>
        <% } %>
        <p>
            <label>Admin Level:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.AdministrativeLevelId",
                        Model.ProjectSummary.IsEditable(),
                        new SelectList(Model.AvailableAdminLevels, "key", "value", Model.InfoModel.AdministrativeLevelId),
                        "-- Select Admin Level --",
                        new { @class = "mediumInputElement required", title = "Please select an Admin Level" })%>
                <%--<span>Facility Responsibility Level</span>--%>
            <%}
              else
              {%> 
                <% value = "None Selected"; if (Model.InfoModel.AdministrativeLevelId != null) { Model.AvailableAdminLevels.TryGetValue((int)Model.InfoModel.AdministrativeLevelId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.AdministrativeLevelId", Model.InfoModel.AdministrativeLevelId)%>
                <br />
            <% } %>
        </p>
       
        <p>
            <label>Project Type:</label>
            <% value = "None Selected"; if (Model.InfoModel.ProjectTypeId != null) 
               { 
                   Model.AvailableProjectTypes.TryGetValue((int)Model.InfoModel.ProjectTypeId, out value); 
               } %>
            <span class="fakeinput medium" id="InfoModel_ProjectType"><%= Html.Encode(value)%></span>
            <%= Html.Hidden("InfoModel.ProjectTypeId", Model.InfoModel.ProjectTypeId)%>
            <br />
        </p>
         <p>
            <label>Improvement Type:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.ImprovementTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableImprovementTypes, "key", "value", Model.InfoModel.ImprovementTypeId), 
                    "-- Select Improvment Type--", 
                    new { @class = "mediumInputElement required", title="Please select an improvment type." })%>
            <%}
              else
              {%> 
                <% value = "None Selected"; if (Model.InfoModel.ImprovementTypeId != null) { Model.AvailableImprovementTypes.TryGetValue((int)Model.InfoModel.ImprovementTypeId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.ImprovementTypeId", Model.InfoModel.ImprovementTypeId)%>
                <br />
            <% } %>
        </p>
        <%if (Model.ProjectSummary.IsEditable()) { %>
        <p>
            <label>Highway or Transit:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.TransportationTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableRoadOrTransitTypes, "key", "value", Model.InfoModel.TransportationTypeId), 
                    "-- Select --", 
                    new { @class = "mediumInputElement required", title="Please specify if this is a Road or transit project" })%>
             <%}
              else
              {%> 
                <% value = "None Selected"; if (Model.InfoModel.TransportationTypeId != null) { Model.AvailableRoadOrTransitTypes.TryGetValue((int)Model.InfoModel.TransportationTypeId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.TransportationTypeId", Model.InfoModel.TransportationTypeId)%>
                <br />
            <% } %>   
        </p>
        <% } %>
        <p>
            <label>Selection Agency:</label>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
                <%= Html.DropDownList("InfoModel.SelectionAgencyId",
                        Model.ProjectSummary.IsEditable(),
                        new SelectList(Model.AvailableSelectionAgencies, "key", "value", Model.InfoModel.SelectionAgencyId),
                        "-- Select a Selection Agency --", 
                        new { @class = "mediumInputElement not-required", title="Please select an Agency" })%>
            <%}
              else
              {%> 
                <% value = "None Selected"; if (Model.InfoModel.SelectionAgencyId != null) { Model.AvailableSelectionAgencies.TryGetValue((int)Model.InfoModel.SelectionAgencyId, out value); } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.SelectionAgencyId", Model.InfoModel.SelectionAgencyId)%>
                <br />
            <% } %> 
        </p>
        <%if (Model.ProjectSummary.IsEditable()) { %>
        <p>
           <label>Regionally Significant?:</label>
           <%if (Model.ProjectSummary.IsEditable())
              { %>
               <%=Html.CheckBox("InfoModel.IsRegionallySignificant",
                    Model.ProjectSummary.IsEditable(), 
                    Model.InfoModel.IsRegionallySignificant.Value, 
                    new { @class = "smallInputElement not-required", title = "Specify Regional Signifigance!" })%>
            <%}
              else
              {%> 
                <% value = "No"; if (Model.InfoModel.IsRegionallySignificant != null) { value =  (bool)Model.InfoModel.IsRegionallySignificant ? "Yes" : "No"; } %>
                <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                <%= Html.Hidden("InfoModel.IsRegionallySignificant", Model.InfoModel.IsRegionallySignificant)%>
                <br />
            <% } %> 
        </p>
        <% } %>
        </div>
        <div id="tab-info-right">
        <p><label>Sponsor Notes:</label></p>
        <p>
        <%= Html.TextArea2("InfoModel_SponsorNotes", 
                            Model.ProjectSummary.IsEditable(),
                            Model.InfoModel.SponsorNotes,
                            0,
                            0,
                            new { @name = "InfoModel.SponsorNotes", @class = "mediumInputElement not-required growable", title = "Please add the sponsor comments." })%>
        </p>
        
        <p><label>DRCOG Notes:</label></p>
        <p>
        <%= Html.TextArea2("InfoModel_DRCOGNotes",
                            Model.ProjectSummary.IsEditable(),
                            Model.InfoModel.DRCOGNotes,
                            0,
                            0,
                            new { @name = "InfoModel.DRCOGNotes", @class = "mediumInputElement growable" })%>
        </p>
        </div>
        <div class="clear"></div> 
        
        <%if(Model.ProjectSummary.IsEditable()){ %>
            <div class="relative">
                <button type="submit" id="submitForm">
                    Save Changes</button>
                <div id="submit-result">
                </div>
            </div>
        <%} %>
        </fieldset>
    </form>
</div>
   
<div class="clear"></div>
</div>

</asp:Content>



