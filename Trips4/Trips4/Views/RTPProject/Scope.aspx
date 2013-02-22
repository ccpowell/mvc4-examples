<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.Project.ScopeViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project General Information</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">
    Regional Transportation Plan
    <%= Model.ProjectSummary.RtpYear%></asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/scripts/jquery.meio.mask.min.js")%>" type="text/javascript"></script>
    <link href="<%= Url.Content("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/scripts/slide.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/RtpProjectScope.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(App.tabs.initializeRtpProjectTabs);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tab-content-container">
        <% Html.RenderPartial("~/Views/RtpProject/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
        <div class="tab-form-container tab-scope">
            <% using (Html.BeginForm("UpdateScope", "RtpProject", FormMethod.Post, new { @id = "dataForm" })) %>
            <% { %>
            <fieldset>
                <legend></legend>
                <%=Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
                <%=Html.Hidden("RtpProjectScope.RtpYear", Model.ProjectSummary.RtpYear)%>
                <%=Html.Hidden("RtpProjectScope.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
                <%=Html.Hidden("ProjectId", Model.ProjectSummary.ProjectId)%>
                <p>
                    <label>
                        Short Description:</label>
                    <input id="RtpProjectScope_ShortDescription" class="longInputElement" title="Please provide a short description for the project."
                        name="RtpProjectScope.ShortDescription" maxlength="256" value="<%= Model.RtpProjectScope.ShortDescription%>" />
                </p>
                <p>
                    <label>
                        Project Scope:</label></p>
                <p>
                    <textarea id="RtpProjectScope_ProjectDescription" class="longInputElement required"
                        title="Please provide a description for the project." name="RtpProjectScope.ProjectDescription"
                        rows="10" cols="50"><%= Model.RtpProjectScope.ProjectDescription%></textarea>
                </p>
                <% if (Model.ProjectSummary.IsEditable())
                   { %>
                <div class="relative">
                    <button type="submit" id="submitForm">
                        Save Changes</button>
                    <div id="result">
                    </div>
                </div>
                <br />
                <% } %>
            </fieldset>
            <% } %>
        </div>
        <div class="clear">
        </div>
        <%--<% Html.RenderPartial("~/Views/Project/Partials/PoolProjectPartial.ascx", Model.PoolProjects); %>--%>
        <div style="position: relative;">
            <h2>
                Project Segments</h2>
            <%if (Model.ProjectSummary.IsEditable())
              { %>
            <button id="new-segmentdetails" style="position: absolute; top: -5px; right: 50px;">
                Add New
            </button>
            <% } %>
            <table id="segments">
                <thead>
                    <tr>
                        <th>
                            Facility Name
                        </th>
                        <th>
                            Start At
                        </th>
                        <th>
                            End At
                        </th>
                        <th>
                            Open Year
                        </th>
                        <th>
                            Network
                        </th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <% 
                        int rowCount = 0;
                        foreach (var item in Model.Segments.ToList<DRCOG.Domain.Models.RTP.SegmentModel>())
                        {
                            rowCount++;
                    %>
                    <tr id="segment_row_<%=item.SegmentId.ToString() %>" <%= rowCount % 2 == 0 ? "class=\"even\"" : "class=\"odd\""%>>
                        <td>
                            <%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_FacilityName", false, item.FacilityName.ToString(), new { style = "width:200px;", @maxlength = "75" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_StartAt", false, item.StartAt.ToString(), new { style = "width:110px;", @maxlength = "50" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_EndAt", false, item.EndAt.ToString(), new { style = "width:110px;", @maxlength = "50" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("segment_" + item.SegmentId.ToString() + "_OpenYear", false, item.OpenYear.ToString(), new { style = "width:75px;", @maxlength = "4" })%>
                        </td>
                        <td>
                            <%= Html.DropDownList("segment_" + item.SegmentId.ToString() + "_NetworkId",
                                                        false,
                            new SelectList(Model.AvailableNetworks, "key", "value", item.NetworkId),
                            "-- Select --",
                            new { @class = "not-required", title = "Please select a network" })%>
                        </td>
                        <td>
                            <span class="table-button" data-segment-details='<%=item.SegmentId.ToString() %>'>Details</span>
                            <%if (Model.ProjectSummary.IsEditable())
                              { %>
                            <span class="table-button" data-segment-delete='<%=item.SegmentId.ToString() %>'>Delete</span>
                            <% } %>
                        </td>
                    </tr>
                    <% } %>
                    <tr style="display: none;" id="segment_last_record">
                        <td colspan="5">
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="clear">
        </div>
    </div>
    <!-- This contains the hidden content for inline calls -->
    <div style='display: none'>
        <div id="segment-details-dialog" title="Segment">
            <form id="segment-details-form" action="">
            <table>
                <tr>
                    <td>
                        <p>
                            <label for="segment-details-FacilityName">
                                Facility Name</label>
                            <input type="text" id="segment-details-FacilityName" class="w200 required" />
                        </p>
                        <p>
                            <label for="segment-details-StartAt">
                                Start At</label>
                            <input type="text" id="segment-details-StartAt" class="w200 required" />
                        </p>
                        <p>
                            <label for="segment-details-EndAt">
                                End At</label>
                            <input type="text" id="segment-details-EndAt" class="w200" />
                        </p>
                        <p>
                            <label for="segment-details-OpenYear">
                                Open Year</label>
                            <input type="text" id="segment-details-OpenYear" class="w200" />
                        </p>
                        <p>
                            <label for="segment-details-NetworkId">
                                Network</label>
                            <%= Html.DropDownList("segment-details-NetworkId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableNetworks, "key", "value"),
                            "-- Select a Network --",
                            new { @class = "not-required", title = "Please select a network" })%>
                        </p>
                        <p>
                            <label for="segment-details-ImprovementTypeId">
                                Improvement Type</label>
                            <%= Html.DropDownList("segment-details-ImprovementTypeId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableImprovementTypes, "key", "value"),
                            "-- Select an Improvement Type --",
                            new { @class = "not-required", title = "Please select an Improvement Type" })%>
                        </p>
                    </td>
                    <td>
                        <p>
                            <label for="segment-details-LanesBase">
                                Lanes Base</label>
                            <input type="text" id="segment-details-LanesBase" class="w200" />
                        </p>
                        <p>
                            <label for="segment-details-LanesFuture">
                                Lanes Future</label>
                            <input type="text" id="segment-details-LanesFuture" class="w200" />
                        </p>
                        <p>
                            <label for="segment-details-SpacesFuture">
                                Spaces Future</label>
                            <input type="text" id="segment-details-SpacesFuture" class="w200" />
                        </p>
                        <p>
                            <label for="segment-details-AssignmentStatus">
                                Assignment Status</label>
                            <input type="text" id="segment-details-AssignmentStatusId" class="w200" />
                        </p>
                        <p>
                            <label for="segment-details-ModelFacilityTypeId">
                                Model Facility Type</label>
                            <%= Html.DropDownList("segment-details-ModelingFacilityTypeId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableFacilityTypes, "key", "value"),
                            "-- Select a Facility Type --",
                            new { @class = "not-required", title = "Please select a Facility Type" })%>
                        </p>
                        <p>
                            <label for="segment-details-PlanFacilityTypeId">
                                Plan Facility Type</label>
                            <%= Html.DropDownList("segment-details-PlanFacilityTypeId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableFacilityTypes, "key", "value"),
                            "-- Select a Facility Type --",
                            new { @class = "not-required", title = "Please select a Facility Type" })%>
                        </p>
                    </td>
                </tr>
            </table>
            </form>
            <div id="segment-details-lrs-section">
                <table id="segment-details-lrs-table">
                    <caption>
                        LRS Records
                        <button id="segment-details-add-lrs">
                            Add LRS Record</button></caption>
                    <thead>
                        <tr>
                            <th>
                                Route Name
                            </th>
                            <th>
                                Measures
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
        <div id="segment-lrs-dialog" title="LRS">
            <p>
                <label for="segment-lrs-Routename">
                    Route Name</label>
                <input type="text" id="segment-lrs-Routename" class="w200" />
            </p>
            <p>
                <label for="segment-lrs-BEGINMEASU">
                    Begin Measure</label>
                <input type="text" id="segment-lrs-BEGINMEASU" class="w100" />
            </p>
            <p>
                <label for="segment-lrs-ENDMEASURE">
                    End Measure</label>
                <input type="text" id="segment-lrs-ENDMEASURE" class="w100" />
            </p>
            <p>
                <label for="segment-lrs-Comments">
                    Comments</label>
                <input type="text" id="segment-lrs-Comments" class="w200" />
            </p>
            <p>
                <label for="segment-lrs-Offset">
                    Offset</label>
                <input type="text" id="segment-lrs-Offset" class="w100" />
            </p>
        </div>
    </div>
</asp:Content>
