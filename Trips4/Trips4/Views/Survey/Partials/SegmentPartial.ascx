<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.Survey.ScopeViewModel>" %>

<h2>Project Staging Details</h2>
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
                    <%= Html.DrcogTextBox("segment_details_FacilityName", Model.Project.IsEditable(), "", new { style = "width:200px;", @maxlength = "225" })%>
               </p>
               <p>
                    <label for="StartAt">Start At</label>
                    <%= Html.DrcogTextBox("segment_details_StartAt", Model.Project.IsEditable(), "", new { style = "width:200px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="EndAt">End At</label>
                    <%= Html.DrcogTextBox("segment_details_EndAt", Model.Project.IsEditable(), "", new { style = "width:200px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="OpenYear">Open Year</label>
                    <%= Html.DrcogTextBox("segment_details_OpenYear", Model.Project.IsEditable(), "", new { style = "width:100px;", @maxlength = "75" })%>
               </p>
               <%if (Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)) { %>
               <p>
                    <label for="NetworkId">Network</label>
                    <%= Html.DropDownList("segment_details_NetworkId",
                                                    Model.Project.IsEditable(),
                            new SelectList(Model.AvailableNetworks, "key", "value"),
                            "-- Select a Network --",
                            new { @class = "not-required", title = "Please select a network" })%>
               </p>
               <p>
                    <label for="ImprovementTypeId">Improvement Type</label>
                    <%= Html.DropDownList("segment_details_ImprovementTypeId",
                            Model.Project.IsEditable(),
                            new SelectList(Model.AvailableImprovementTypes, "key", "value"),
                            "-- Select an Improvement Type --",
                            new { @class = "not-required", title = "Please select an Improvement Type" })%>
               </p>
               <% } %>
               
               
               </td>
               <td>
               <p>
                    <label for="LanesBase">Lanes Base</label>
                    <%= Html.DrcogTextBox("segment_details_LanesBase", Model.Project.IsEditable() && !Model.Project.ImprovementTypeId.Equals(16), "0", new { style = "width:150px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="LanesFuture">Lanes Future</label>
                    <%= Html.DrcogTextBox("segment_details_LanesFuture", Model.Project.IsEditable(), "", new { style = "width:150px;", @maxlength = "75" })%>
               </p>
               <%if (Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)) { %>
               <p>
                    <label for="Length">Length</label>
                    <%= Html.DrcogTextBox("segment_details_Length", Model.Project.IsEditable(), "", new { style = "width:150px;" })%>
               </p>
               <p>
                    <label for="SpacesBase">Spaces Base</label>
                    <%= Html.DrcogTextBox("segment_details_SpacesBase", Model.Project.IsEditable(), "", new { style = "width:150px;", @maxlength = "75" })%>
               </p>
               <p>
                    <label for="SpacesFuture">Spaces Future</label>
                    <%= Html.DrcogTextBox("segment_details_SpacesFuture", Model.Project.IsEditable(), "", new { style = "width:150px;", @maxlength = "75" })%>
               </p>
               <%--<p>
                    <label for="AssignmentStatus">Assignment Status</label>
                    <%= Html.DrcogTextBox("segment_details_AssignmentStatusId", Model.Project.IsEditable(), "", new { style = "width:150px;", @maxlength = "75" })%>
               </p>--%>
               <%--<p>
                    <label for="ModelFacilityTypeId">Model Facility Type</label>
                    <%= Html.DropDownList("segment_details_ModelingFacilityTypeId",
                            Model.Project.IsEditable(),
                            new SelectList(Model.AvailableFacilityTypes, "key", "value"),
                            "-- Select a Facility Type --",
                            new { @class = "not-required", title = "Please select a Facility Type" })%>
               </p>--%>
               <p>
                    <label for="PlanFacilityTypeId">Facility Type</label>
                    <%= Html.DropDownList("segment_details_PlanFacilityTypeId",
                            Model.Project.IsEditable(),
                            new SelectList(Model.AvailableFacilityTypes, "key", "value"),
                            "-- Select a Facility Type --",
                            new { @class = "not-required", title = "Please select a Facility Type" })%>
               </p>
               <p>
                    <label for="ModelingCheck">Modeling Status</label>
                    <%= Html.CheckBox("segment_details_modelingcheck", Model.Project.IsEditable(), false) %>
               </p>
               <% } %>
               </td>
               </tr>
               </table>
               
               <%if (Model.Project.IsEditable(DRCOG.Domain.Models.Survey.InstanceSecurity.EditLevel.Admin)) { %>
               <div style="display: block;" id="lrsdetails-container">
                   <h2>LRS Details</h2>
                   <div id="lrsdetails"></div>
               </div>
               <% } %>
               
               
               
                <%if(Model.Project.IsEditable()){ %>
                    <div class="relative">
                        <%--<button id="process-segment-old" disabled='disabled' class="update-segment-details fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">Add</button>--%>
                        <div id="process-segment-result"></div>
                    </div>
                <% } %>
                
               
            </fieldset>
</div>
