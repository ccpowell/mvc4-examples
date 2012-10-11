<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.ProjectSearchViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project Search</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.RtpSummary.RtpYear %></asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
<%--<h2><%=Html.ActionLink("RTP List", "Index",new {controller="RTP"}) %> / RTP <%=Model.RtpSummary.RtpYear%></h2>--%>
<div class="clear"></div>
    
    <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>


    <div class="tab-content-container">

        <h2>Search for Project in the RTP</h2>
        <p>
            In the fields below you have the option of dynamic searching using the following options:
            <ul>
                <li>1. *: wildcard searching ex. Arapaho* or *Arapaho*</li>
            </ul>
        </p>
        <!---<p>When you submit the search, the criteria will be stored in session, and the page will re-direct to
        the Project List tab. The Project List tab will always respect that last set of search criteria applied.</p>
        <p>We are awaiting further confirmation about how the users interact with the search UI. The hope is
        that the UI can be compartmentalized, which will make the back-end queries much easier to manage.</p>--->
       
    <% using (Html.BeginForm("ProjectSearch", "RTP", FormMethod.Post, new { @id = "dataForm-rtpsearch" })) %>
    <%{ %>
    
       <fieldset>
       
       <table>
       <tr>
       <td valign="top">
       
          <p><b>Select by Project Identifiers</b></p>
            <p>
                <label>RTPID:</label>
                <%= Html.DrcogTextBox("ProjectSearchModel.RtpID", 
                    true,
                    Model.ProjectSearchModel.RtpID,
                    new { @class = "mediumInputElement not-required highlight-red", title = "Please enter a RtpID" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_ID", 
                    true,
                    Model.ProjectSearchModel.Exclude_ID) %>
                    <span class="tt-exclude" title="Checking this option will exclude the entered RTPID from the search results">
                        Exclude
                    </span>
            </p>
            <p>
                <label>COGID:</label>
                <%= Html.DrcogTextBox("ProjectSearchModel.COGID", 
                    true, 
                    Model.ProjectSearchModel.COGID,
                                        new { @class = "mediumInputElement not-required highlight-red", title = "Please enter a COGID" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_COGID", 
                    true, 
                    Model.ProjectSearchModel.Exclude_COGID) %>
                    <span class="tt-exclude" title="Checking this option will exclude the entered COGID from the search results">
                        Exclude
                    </span>
            </p>
            <p>
                <label>TIPID:</label>
                <%= Html.DrcogTextBox("ProjectSearchModel.TipId", 
                    true, 
                    Model.ProjectSearchModel.TipId,
                                        new { @class = "mediumInputElement not-required highlight-red", title = "Please enter a TIPID" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_TipId", 
                    true,
                    Model.ProjectSearchModel.Exclude_TipId)%>
                    <span class="tt-exclude" title="Checking this option will exclude the entered TIPID from the search results">
                        Exclude
                    </span>
            </p>
              <p>
                <label>Project Name:</label>
                <%= Html.DrcogTextBox("ProjectSearchModel.ProjectName", 
                    true, 
                    Model.ProjectSearchModel.ProjectName,
                                        new { @class = "mediumInputElement not-required highlight-red", title = "Please enter a string to search on" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_ProjectName", 
                    true,
                    Model.ProjectSearchModel.Exclude_ProjectName)%>
                    <span class="tt-exclude" title="Checking this option will exclude the entered project name from the search results">
                        Exclude
                    </span>
            </p>            
            <p>
                <label>Improvement Type:</label>            
                <%= Html.DropDownList("ProjectSearchModel.ImprovementTypeID", 
                    true,
                    new SelectList(Model.AvailableImprovementTypes, "key", "value", Model.ProjectSearchModel.ImprovementTypeID),
                    "---(Include all or select from list)---",
                                        new { @class = "mediumInputElement not-required highlight-red", title = "Please select an improvement type" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_ImprovementType", 
                    true,
                    Model.ProjectSearchModel.Exclude_ImprovementType)%>
                    <span class="tt-exclude" title="Checking this option will exclude the selected improvement type from the search results">
                        Exclude
                    </span>                    
            </p>
            <p>
                <label>Project Type:</label>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_ProjectType", 
                    true,
                    Model.ProjectSearchModel.Exclude_ProjectType)%>    
                <span class="tt-exclude" title="Checking this option will exclude the selected project type from the search results">
                    Exclude
                </span><br />         
                <%= Html.DropDownList("ProjectSearchModel.ProjectTypeID", 
                    true,
                    new SelectList(Model.AvailableProjectTypes, "key", "value", Model.ProjectSearchModel.ProjectTypeID),
                    "---(Include all or select from list)---",
                                        new { @class = "mediumInputElement not-required highlight-red", title = "Please select a project type" })%>
                     
            </p>
            
            <p><b>Select by Sponsor / Geography</b></p>
             <p>
                <label>Sponsor:</label>
                <%= Html.DropDownList("ProjectSearchModel.SponsorAgencyID", 
                    true,
                   new SelectList(Model.EligibleAgencies, "key", "value", Model.ProjectSearchModel.SponsorAgencyID),
                   "---(Include all or select from list)---",
                                       new { @class = "mediumInputElement not-required highlight-red", title = "Please select a project sponsor" })%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_SponsorAgency", 
                    true,
                    Model.ProjectSearchModel.Exclude_SponsorAgency) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected sponsor from the search results">
                        Exclude
                    </span>
            </p>
       
       </td>
       <td valign="top">
       
       <p><b>Select by RTP Characteristics</b></p>
                
            <%--<p>
              <label>Version Status:</label>
              <%= Html.DropDownList("ProjectSearchModel.ActiveVersion",
                   new[] {
                           new SelectListItem { Text = "Active", Value = "true" }
                        ,   new SelectListItem { Text = "Inactive", Value = "false" }
                        },
                       "---(Include all or select from list)---",
                       new { @class = "mediumInputElement not-required highlight-red", title="Please select wether to include inactive items" })%>
               <%= Html.CheckBox("ProjectSearchModel.Exclude_ActiveVersion", 
                    true,
                    Model.ProjectSearchModel.Exclude_ActiveVersion) %>Exclude
              </p>--%>
              <p>
                <label>Plan Type:</label>
                <%= Html.DropDownList("ProjectSearchModel.PlanTypeId", 
                    true,
                    new SelectList(Model.AvailablePlanTypes, "key", "value"),
                    "---(Include all or select from list)---",
                    new { @class = "mediumInputElement not-requiredhighlight-red highlight-red", title = "Please select a plan type" })%> 
                <%= Html.CheckBox("ProjectSearchModel.Exclude_PlanType", 
                    true,
                    Model.ProjectSearchModel.Exclude_PlanType) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected plan type from the search results">
                        Exclude
                    </span>
              </p>
              <p>
                <label>Plan:</label>            
                <%= Html.DropDownList("ProjectSearchModel.RtpYearID", 
                    true,
                    new SelectList(Model.AvailablePlanYears, "key", "value", Model.ProjectSearchModel.RtpYearID),
                    "---(Include all or select from list)---",
                    new { @class = "mediumInputElement not-required highlight-red", title = "Please select a valid RTP year" })%>
                <%--<%= Html.Hidden("ProjectSearchModel.RtpYear") %>--%>
                <%= Html.CheckBox("ProjectSearchModel.Exclude_Year", 
                    true,
                    Model.ProjectSearchModel.Exclude_Year) %>
                    <span class="tt-exclude" title="Checking this option will exclude the selected plan from the search results">
                        Exclude
                    </span>
                    <br /> <%--needed to push scenario-container below when actived by jquery--%>
                <span id="scenario-container" style="display: none; ">
                    <label for="ProjectSearchModel.NetworkID">Scenario:</label>
                    <select class="mediumInputElement not-required highlight-red" id="ProjectSearchModel_NetworkID" name="ProjectSearchModel.NetworkID" size="1"></select>
                    <%--<label>Cycle:</label>
                    <%= Html.DropDownList("ProjectSearchModel.AmendmentStatusID", 
                        true,
                        new SelectList(Model.AvailableAmendmentStatuses, "key", "value", Model.ProjectSearchModel.AmendmentStatusID),
                        "---(Include all or select from list)---",
                        new { @class = "mediumInputElement not-required", title = "Please select an amendment status" })%>--%>
                </span>
            </p>
            <%if (Model.RtpSummary.IsEditable())
              { %>     
              <p>
              <br />
                <label>Plan Stage:</label>           
                <%= Html.DropDownList("ProjectSearchModel.AmendmentStatusID",
                    true,
                    new SelectList(Model.AvailableAmendmentStatuses, "key", "value", Model.ProjectSearchModel.AmendmentStatusID),
                    "---(Include all or select from list)---",
                    new { @class = "mediumInputElement not-requiredhighlight-red highlight-red", title = "Please select an amendment status" })%> 
               <%= Html.CheckBox("ProjectSearchModel.Exclude_AmendmentStatus",
                    true,
                    Model.ProjectSearchModel.Exclude_AmendmentStatus)%>
                    <span class="tt-exclude" title="Checking this option will exclude the selected plan stage from the search results">
                        Exclude
                    </span>  
            </p>
            <% } %>
       </td>
       </tr>
       </table>
       
        <p>
            <button type="submit" id="submitForm" class="fg-button-important ui-state-default ui-priority-primary ui-corner-all" >Search Projects</button>
            <button type="button" id="resetForm" class="fg-button-important ui-state-default ui-priority-primary ui-corner-all" onclick="reset_form_elements(this.form)">Reset</button>
        </p>

    </fieldset>
    <%} %>
    
    </div>
    
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">
        var GetPlanScenarios = '<%=Url.Action("GetPlanScenariosForCurrentCycle","RTP")%>';
        
    </script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/rtp-search.js")%>" type="text/javascript" ></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.tools.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".tt-exclude[title]").tooltip();
        });
    </script>
</asp:Content>



<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
