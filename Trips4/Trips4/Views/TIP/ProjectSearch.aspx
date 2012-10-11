<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.ProjectSearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project Search</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript" language="jscript">

function reset_form_elements(formElements) {
    $(formElements).find(':input').each(function() {  
            switch(this.type) {  
            case 'password':  
            case 'select-multiple':  
            case 'select-one':  
            case 'text':  
            case 'textarea':  
                $(this).val('');  
            break;  
            case 'checkbox':  
            case 'radio':  
                this.checked = false;  
        }  
    });      
} 

</script>


<div class="view-content-container">
<%--<h2><%=Html.ActionLink("TIP List", "Index",new {controller="TIP"}) %> / TIP <%=Model.TipSummary.TipYear%></h2>--%>
<div class="clear"></div>
    
    <%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>


    <div class="tab-content-container">

        <h2>Search for Project in the TIP</h2>
        <p>This will be the search form instructions. </p>
        <!---<p>When you submit the search, the criteria will be stored in session, and the page will re-direct to
        the Project List tab. The Project List tab will always respect that last set of search criteria applied.</p>
        <p>We are awaiting further confirmation about how the users interact with the search UI. The hope is
        that the UI can be compartmentalized, which will make the back-end queries much easier to manage.</p>--->
       
    <% using (Html.BeginForm("ProjectSearch", "Tip", FormMethod.Post, new { @id = "dataForm" })) %>
    <%{ %>
    
       <fieldset>
       
       <table>
       <tr>
       <td valign="top">
       
          <p><b>Select by Project Identifiers</b></p>
            <p>
                <label>TIPID:</label>
                <%= Html.DrcogTextBox("ProjectSearchModel.TipID", 
                    true,
                    Model.ProjectSearchModel.TipID, 
                    new { @class = "mediumInputElement not-required", title = "Please enter a TipID" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_TipID", 
                    true,
                    Model.ProjectSearchModel.Exclude_TipID) %>
                    <span class="tt-exclude" title="Checking this option will exclude the entered TIPID from the search results">
                        Exclude
                    </span>
            </p>
            <p>
                <label>STIP-ID:</label>
                <%= Html.DrcogTextBox("ProjectSearchModel.StipId", 
                    true,
                    Model.ProjectSearchModel.StipId, 
                    new { @class = "mediumInputElement not-required", title = "Please enter a STIP-ID" })%>
            </p>
            <% if(Request.IsAuthenticated && (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))) { %>
            <p>
                <label>COGID:</label>
                <%= Html.DrcogTextBox("ProjectSearchModel.COGID", 
                    true, 
                    Model.ProjectSearchModel.COGID, 
                    new { @class = "mediumInputElement not-required", title = "Please enter a COGID" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_COGID", 
                    true, 
                    Model.ProjectSearchModel.Exclude_COGID) %>
                    <span class="tt-exclude" title="Checking this option will exclude the entered COGID from the search results">
                        Exclude
                    </span>
            </p>
            <% } %>
              <p>
                <label>Project Name:</label>
                <%= Html.DrcogTextBox("ProjectSearchModel.ProjectName", 
                    true, 
                    Model.ProjectSearchModel.ProjectName, 
                    new { @class = "mediumInputElement not-required", title = "Please enter a string to search on" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_ProjectName", 
                    true,
                    Model.ProjectSearchModel.Exclude_ProjectName)%>
                    <span class="tt-exclude" title="Checking this option will exclude the entered Project Name from the search results">
                        Exclude
                    </span>
            </p>            
            
            <p>
                <label>Project Type: </label><%= Html.CheckBox("ProjectSearchModel.Exclude_ProjectType", 
                    true,
                    Model.ProjectSearchModel.Exclude_ProjectType)%>
                    <span class="tt-exclude" title="Checking this option will exclude the selected Project Type from the search results">
                        Exclude
                    </span><br />  
                <%= Html.DropDownList("ProjectSearchModel.ProjectTypeID", 
                    true,
                    new SelectList(Model.AvailableProjectTypes, "key", "value", Model.ProjectSearchModel.ProjectTypeID),
                    "---(Include all or select from list)---", 
                    new { @class = "mediumInputElement not-required", title="Please select a project type" })%>
                         
            </p>
            <p>
                <label>Improvement Type: </label><%= Html.CheckBox("ProjectSearchModel.Exclude_ImprovementType", 
                    true,
                    Model.ProjectSearchModel.Exclude_ImprovementType)%>
                    <span class="tt-exclude" title="Checking this option will exclude the selected Improvement Type from the search results">
                        Exclude
                    </span><br />
                <%= Html.DropDownList("ProjectSearchModel.ImprovementTypeID", 
                    true,
                    new SelectList(Model.AvailableImprovementTypes, "key", "value", Model.ProjectSearchModel.ImprovementTypeID),
                    "---(Include all or select from list)---", 
                    new { @class = "mediumInputElement not-required", title="Please select an improvement type" })%>
                                   
            </p>
            <p>
                <label>Funding Type:</label><%= Html.CheckBox("ProjectSearchModel.Exclude_FundingType", 
                    true,
                    Model.ProjectSearchModel.Exclude_FundingType)%>
                    <span class="tt-exclude" title="Checking this option will exclude the selected Funding Type from the search results">
                        Exclude
                    </span><br />  
                <%= Html.DropDownList("ProjectSearchModel.FundingTypeID", 
                    true,
                    new SelectList(Model.AvailableFundingTypes, "key", "value", Model.ProjectSearchModel.FundingTypeId),
                    "---(Include all or select from list)---", 
                    new { @class = "mediumInputElement not-required", title="Please select a funding type" })%>
                         
            </p>
            
            <p><b>Select by Sponsor / Geography / CDOT Region</b></p>
             <p>
                <label>Sponsor:</label>
                
                <%--<%= Html.CheckBox("ProjectSearchModel.Exclude_SponsorAgency", 
                    true,
                    Model.ProjectSearchModel.Exclude_SponsorAgency) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected Sponsor from the search results">
                        Exclude
                    </span> <br />--%>

                    <%= Html.DropDownList("ProjectSearchModel.SponsorAgencyID", 
                    true,
                   new SelectList(Model.AvailableSponsors, "key", "value", Model.ProjectSearchModel.SponsorAgencyID),
                   "---(Include all or select from list)---", 
                   new { @class = "mediumInputElement not-required", title="Please select a project sponsor" })%>
            </p>

            <p>
                <label>Geography:
                <%--<%= Html.CheckBox("ProjectSearchModel.Exclude_SponsorAgency", 
                    true,
                    Model.ProjectSearchModel.Exclude_SponsorAgency) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected Sponsor from the search results">
                        Exclude
                    </span> <br />--%>

                    <%= Html.DropDownList("ProjectSearchModel.GeographyName", 
                    true,
                    new SelectList(Model.AvailableGeographies),
                   //new SelectList(Model.AvailableGeographies.ToDictionary(k => k. , "Label", "Text", Model.ProjectSearchModel.GeographyId),
                   "---(Include all or select from list)---", 
                   new { @class = "mediumInputElement not-required", title="Please select a project sponsor" })%>
                </label>
            </p>

            <p>
                <label>CDOT Region:
                <%--<%= Html.CheckBox("ProjectSearchModel.Exclude_SponsorAgency", 
                    true,
                    Model.ProjectSearchModel.Exclude_SponsorAgency) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected Sponsor from the search results">
                        Exclude
                    </span> <br />--%>

                    <%= Html.DropDownListFor(x => x.ProjectSearchModel.CdotRegionId
                        , Model.CdotRegions
                        , new { @class = "mediumInputElement not-required" }) %>
                </label>
            </p>
       
       </td>
       <td valign="top">
       
       <p><b>Select by TIP Characteristics</b></p>
                
            <p>
              <label>Version Status:</label>
              <%= Html.DropDownList("ProjectSearchModel.VersionStatusId", 
                    true,
                   new SelectList(Model.AvailableVersionStatuses, "key", "value", Model.ProjectSearchModel.VersionStatusId),
                   "---(Include all or select from list)---", 
                   new { @class = "mediumInputElement not-required", title="Please select an Active Version" })%>
               <%= Html.CheckBox("ProjectSearchModel.Exclude_ActiveVersion", 
                    true,
                    Model.ProjectSearchModel.Exclude_ActiveVersion) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected version status from the search results">
                        Exclude
                    </span> 
              </p>
              <p>
                <label>TIP Year:</label>            
                <%= Html.DropDownList("ProjectSearchModel.TipYearID", 
                    true,
                    new SelectList(Model.AvailableTipYears, "key", "value", Model.TipSummary.TipYearTimePeriodID),
                    "---(Include all or select from list)---", 
                    new { @class = "mediumInputElement not-required", title="Please select a valid TIP year" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_TipYear", 
                    true,
                    Model.ProjectSearchModel.Exclude_TipYear) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected TIP year from the search results">
                        Exclude
                    </span> 
            </p>
            <p>
                <label for="ProjectSearchModel.ScopeTerm">Search Project Scopes:<br />
                    <%= Html.TextBoxFor(x => x.ProjectSearchModel.ScopeTerm, new { @class = "w300", @maxlength = "256" })%>
                </label>
            </p>
            <p>
                <label for="ProjectSearchModel.PoolTerm">Search Active Pool Projects:<br />
                    <%= Html.TextBoxFor(x => x.ProjectSearchModel.PoolTerm, new { @class = "w300", @maxlength = "256" })%>
                </label>
            </p>      
              <p>
                <label>Amendment Status:</label>           
                
               <%= Html.CheckBox("ProjectSearchModel.Exclude_AmendmentStatus", 
                    true,
                    Model.ProjectSearchModel.Exclude_AmendmentStatus) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected amendment status from the search results">
                        Exclude
                    </span><br />
                    <%= Html.DropDownList("ProjectSearchModel.AmendmentStatusID", 
                    true,
                    new SelectList(Model.AvailableAmendmentStatuses, "key", "value", Model.ProjectSearchModel.AmendmentStatusID),
                    "---(Include all or select from list)---",
                    new { @class = "mediumInputElement not-required", title = "Please select an amendment status" })%>  
            </p> 
            <p><b>Admin Quick Search</b></p>
            <p>
                <%=Html.ActionLink("Proposed Amendments", "ProjectList", new { controller = "Tip", TipYear = Model.TipSummary.TipYear, AmendmentTypeId = (int)DRCOG.Domain.Enums.AmendmentType.Administrative, @AmendmentStatusId = (int)DRCOG.Domain.Enums.TIPAmendmentStatus.Proposed }, new { @class = "fg-button w300 ui-state-default ui-corner-all" })%>
                <% if(Request.IsAuthenticated && (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))) { %>
                    <%=Html.ActionLink("Submitted Amendments", "ProjectList", new { controller = "Tip", TipYear = Model.TipSummary.TipYear, AmendmentTypeId = (int)DRCOG.Domain.Enums.AmendmentType.Administrative, @AmendmentStatusId = (int)DRCOG.Domain.Enums.TIPAmendmentStatus.Submitted }, new { @class = "fg-button w300 ui-state-default ui-corner-all" })%>
                <% } %>
                <%--<button type="button" id="test" class="fg-button-important ui-state-default ui-priority-primary ui-corner-all">Test</button>--%>
            </p>
            <p style="padding-top:8px;"><b>Policy Quick Search</b></p>
            <p>
                <%=Html.ActionLink("Proposed Amendments", "ProjectList", new { controller = "Tip", TipYear = Model.TipSummary.TipYear, AmendmentTypeId = (int)DRCOG.Domain.Enums.AmendmentType.Policy, @AmendmentStatusId = (int)DRCOG.Domain.Enums.TIPAmendmentStatus.Proposed }, new { @class = "fg-button w300 ui-state-default ui-corner-all" })%>
                <% if(Request.IsAuthenticated && (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))) { %>
                    <%=Html.ActionLink("Submitted Amendments", "ProjectList", new { controller = "Tip", TipYear = Model.TipSummary.TipYear, AmendmentTypeId = (int)DRCOG.Domain.Enums.AmendmentType.Policy, @AmendmentStatusId = (int)DRCOG.Domain.Enums.TIPAmendmentStatus.Submitted }, new { @class = "fg-button w300 ui-state-default ui-corner-all" })%>
                <% } %>
                <%--<button type="button" id="test" class="fg-button-important ui-state-default ui-priority-primary ui-corner-all">Test</button>--%>
            </p>
       </td>
       </tr>

       </table>
       
        <p>
            <button type="submit" id="submitForm" class="fg-button-important ui-state-default ui-priority-primary ui-corner-all" >Search Projects</button>&nbsp;
            <button type="button" id="resetForm" class="fg-button-important ui-state-default ui-priority-primary ui-corner-all" onclick="reset_form_elements(this.form)" >Reset Criteria</button>
        </p>

    </fieldset>
    <%} %>
    
    </div>
    
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.tools.min.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $(".tt-exclude[title]").tooltip();
        var ProjectSearchModel_ImprovementTypeID = $('#ProjectSearchModel_ImprovementTypeID');

        ProjectSearchModel_ImprovementTypeID.prop("disabled", true);


        ProjectSearchModel_ImprovementTypeID.bind('change', function () {
            var improvementtypeid = $('#ProjectSearchModel_ImprovementTypeID :selected').val();
            improvementtypeid = parseInt(improvementtypeid);

            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetImprovementTypeMatch")%>',
                data: "id=" + improvementtypeid,
                dataType: "json",
                success: function (response) {
                    //alert(response.id + ' ' + response.value);
                    $('#ProjectSearchModel_ProjectTypeID').val(response.id);
                },
                error: function (response) {
                    $('#result').html(response.error);
                }
            });
        });

        $('#ProjectSearchModel_ProjectTypeID').bind('change', function () {
            var projecttypeid = $('#ProjectSearchModel_ProjectTypeID :selected').val();
            var parsed = parseInt(projecttypeid);
            projecttypeid = !isNaN(parsed) ? parsed : 0;
            $.ajax({
                type: "POST",
                url: '<%= Url.Action("GetProjectTypeMatch")%>',
                data: "id=" + projecttypeid,
                dataType: "json",
                success: function (response) {
                    ProjectSearchModel_ImprovementTypeID.removeOption(/./);
                    ProjectSearchModel_ImprovementTypeID
                        .fillSelect(response.data, { 'defaultOptionText': '---(Include all or select from list)---' })
                        .sortOptions();
                    ProjectSearchModel_ImprovementTypeID.prop("disabled", false);
                },
                error: function (response) {
                    $('#result').html(response.data);
                }
            });
        });
    });
</script>

</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
