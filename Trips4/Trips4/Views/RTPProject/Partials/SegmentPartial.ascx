<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.RTP.Project.ScopeViewModel>" %>

<div id="segment-details-error" style="display:none; position: absolute; bottom: 10px; left: 190px; width: 400px;"></div>
<div id="AmendmentForm" title="Amend Project">
            <fieldset>
            <%=Html.ValidationSummary("Unable to amend. Please correct the errors and try again.")%>
            <%= Html.Hidden("segment_details_SegmentId")%>
               <table>
               <tr>
               <td>
               <p>
                    <label for="FacilityName">Facility Name</label>
                    <%= Html.DrcogTextBox("segment_details_FacilityName", Model.ProjectSummary.IsEditable(), "", new { style = "width:200px;", @maxlength = "225" })%>
               </p>
               <p>
                    <label for="StartAt">Start At</label>
                    <%= Html.DrcogTextBox("segment_details_StartAt", Model.ProjectSummary.IsEditable(), "", new { style = "width:200px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="EndAt">End At</label>
                    <%= Html.DrcogTextBox("segment_details_EndAt", Model.ProjectSummary.IsEditable(), "", new { style = "width:200px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="OpenYear">Open Year</label>
                    <%= Html.DrcogTextBox("segment_details_OpenYear", Model.ProjectSummary.IsEditable(), "", new { style = "width:100px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="NetworkId">Network</label>
                    <%= Html.DropDownList("segment_details_NetworkId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableNetworks, "key", "value"),
                            "-- Select a Network --",
                            new { @class = "not-required", title = "Please select a network" })%>
               </p>
               <p>
                    <label for="ImprovementTypeId">Improvement Type</label>
                    <%= Html.DropDownList("segment_details_ImprovementTypeId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableImprovementTypes, "key", "value"),
                            "-- Select an Improvement Type --",
                            new { @class = "not-required", title = "Please select an Improvement Type" })%>
               </p>
               
               
               </td>
               <td>
               <p>
                    <label for="LanesBase">Lanes Base</label>
                    <%= Html.DrcogTextBox("segment_details_LanesBase", Model.ProjectSummary.IsEditable(), "", new { style = "width:150px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="LanesFuture">Lanes Future</label>
                    <%= Html.DrcogTextBox("segment_details_LanesFuture", Model.ProjectSummary.IsEditable(), "", new { style = "width:150px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="SpacesFuture">Spaces Future</label>
                    <%= Html.DrcogTextBox("segment_details_SpacesFuture", Model.ProjectSummary.IsEditable(), "", new { style = "width:150px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="AssignmentStatus">Assignment Status</label>
                    <%= Html.DrcogTextBox("segment_details_AssignmentStatusId", Model.ProjectSummary.IsEditable(), "", new { style = "width:150px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="ModelFacilityTypeId">Model Facility Type</label>
                    <%= Html.DropDownList("segment_details_ModelingFacilityTypeId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableFacilityTypes, "key", "value"),
                            "-- Select a Facility Type --",
                            new { @class = "not-required", title = "Please select a Facility Type" })%>
               </p>
               <p>
                    <label for="PlanFacilityTypeId">Plan Facility Type</label>
                    <%= Html.DropDownList("segment_details_PlanFacilityTypeId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableFacilityTypes, "key", "value"),
                            "-- Select a Facility Type --",
                            new { @class = "not-required", title = "Please select a Facility Type" })%>
               </p>
               </td>
               </tr>
               </table>
               
               
               <div id="lrslinks"></div>
               
               
                <%if(Model.ProjectSummary.IsEditable()){ %>
                    <div class="relative">
                        <%--<button id="process-segment-old" disabled='disabled' class="update-segment-details fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">Add</button>--%>
                        <div id="process-segment-result"></div>
                    </div>
                <% } %>
                
               
            </fieldset>
</div>
<div style="display: none;" id="lrsdetails-container">
   <h2>LRS Details</h2>
   <div id="lrsdetails"></div>
</div>

<script type="text/javascript">

</script>