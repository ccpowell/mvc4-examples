<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.InfoViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project General Information</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.popeye.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.popeye.style.css") %>" rel="stylesheet"
        type="text/css" />
    <link href="<%= Url.Content("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Url.Content("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/slide.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.sort.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.growing-textarea.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.popeye-2.0.4.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.isEditable = App.utility.parseBoolean('<%= Model.ProjectSummary.IsEditable().ToString() %>');
        App.pp.add1url = '<%= Url.Action("AddCurrent1Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
        App.pp.remove1url = '<%= Url.Action("DropCurrent1Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
        App.pp.add2url = '<%= Url.Action("AddCurrent2Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
        App.pp.remove2url = '<%= Url.Action("DropCurrent2Agency","Project", new {tipYear=Model.InfoModel.TipYear, projectVersionId=Model.InfoModel.ProjectVersionId}) %>';
        App.pp.UpdateAvailableSponsorContacts = '<%= Url.Action("UpdateAvailableSponsorContacts") %>/';
        App.pp.GetImprovementTypeMatch = '<%= Url.Action("GetImprovementTypeMatch") %>';
        App.pp.GetProjectTypeMatch = '<%= Url.Action("GetProjectTypeMatch") %>';
        App.pp.ProjectVersionId = parseInt('<%=  Model.ProjectSummary.ProjectVersionId %>');
        App.pp.TipYear = '<%=  Model.ProjectSummary.TipYear %>';

        $(document).ready(App.tabs.initializeTipProjectTabs);
    </script>
    <script src="<%= Url.Content("~/scripts/TipProjectInfo.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tab-content-container">
        <% Html.RenderPartial("~/Views/Project/Partials/TipProjectTabPartial.ascx", Model.ProjectSummary); %>
        <div class="tab-form-container">
            <% if (ViewData["message"] != null)
               { %>
            <div id="message" class="info">
                <%= ViewData["message"].ToString() %></div>
            <% } %>
            <form method="put" action="/api/TipProjectInfo" id="dataForm">
            <fieldset>
                <legend></legend>
                <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
                <%= Html.Hidden("InfoModel.TipYear", Model.InfoModel.TipYear)%>
                <%= Html.Hidden("InfoModel.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
                <%= Html.Hidden("InfoModel.ProjectId", Model.ProjectSummary.ProjectId)%>
                <div id="tab-info-left">
                    <p>
                        <label>
                            Project Name:</label>
                        <%= Html.DrcogTextBox("InfoModel.ProjectName", 
                    (Model.ProjectSummary.IsEditable()), 
                    Model.InfoModel.ProjectName, 
                    new { @class = "longInputElement required", title="Please enter a project title.", @MAXLENGTH = 100 })%>
                    </p>
                    <p>
                        <div id="currentSponsorsForm" class="tab-info-sponsor">
                            <label>
                                Sponsor(s):</label>
                            <br />
                            <table border="1" rules="none">
                                <tr>
                                    <td>
                                        Primary Sponsor:
                                    </td>
                                    <% if (Model.ProjectSummary.IsEditable())
                                       { %>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        Available Agencies:
                                    </td>
                                    <% } %>
                                </tr>
                                <tr>
                                    <td>
                                        <span id="PrimarySponsor" class="fakeinput medium" style="margin: 0 0 0 3px;">
                                            <%= Html.Encode(Model.ProjectSponsorsModel.PrimarySponsor.OrganizationName) %></span>
                                        <%= Html.Hidden("PrimarySponsorId", Model.ProjectSponsorsModel.PrimarySponsor.OrganizationId) %>
                                    </td>
                                    <% if (Model.ProjectSummary.IsEditable())
                                       { %>
                                    <td>
                                        <%--<a href="#" id="remove1" ><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />--%>
                                        <a href="#" id="add1">
                                            <img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                                    </td>
                                    <td rowspan="3">
                                        <%= Html.ListBox("AvailableAgencies", 
                            new SelectList(Model.ProjectSponsorsModel.GetAvailableAgencySelectList().Items, "OrganizationId", "OrganizationName"),
                                                    new { @class = "mediumInputElement nobind", size = 10 })%><br />
                                    </td>
                                    <% } %>
                                </tr>
                                <tr>
                                    <td>
                                        Secondary Sponsor(s):
                                    </td>
                                    <% if (Model.ProjectSummary.IsEditable())
                                       { %>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <% } %>
                                </tr>
                                <tr>
                                    <td>
                                        <%= Html.ListBox("Current2Agencies", 
                        new SelectList(Model.ProjectSponsorsModel.GetCurrent2AgencySelectList().Items, "OrganizationId", "OrganizationName"),
                                                new { @class = "mediumInputElement nobind", size = 5 })%><br />
                                    </td>
                                    <%if (Model.ProjectSummary.IsActive && Model.ProjectSummary.IsEditable())
                                      { %>
                                    <td>
                                        <a href="#" id="remove2">
                                            <img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                                        <a href="#" id="add2">
                                            <img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <% } %>
                                </tr>
                            </table>
                            <% if (Model.ProjectSummary.IsEditable())
                               { %>
                            <span>Note: Changes are stored to the database as they are made in the interface.</span>
                            <% } %>
                        </div>
                    </p>
                    <% if (Model.ProjectSummary.IsEditable())
                       { %>
                    <p>
                        <label>
                            Sponsor Contact:</label>
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
                        <% string value = "None Selected"; if (Model.InfoModel.SponsorContactId != null) { Model.AvailableSponsorContacts.TryGetValue((int)Model.InfoModel.SponsorContactId, out value); } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("InfoModel.SponsorContactId", Model.InfoModel.SponsorContactId)%>
                        <br />
                        <% } %>
                    </p>
                    <% } %>
                    <% if (Model.ProjectSummary.IsEditable())
                       { %>
                    <p>
                        <label>
                            Admin Level:</label>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <%= Html.DropDownList("InfoModel.AdministrativeLevelId",
                        Model.ProjectSummary.IsEditable(),
                        new SelectList(Model.AvailableAdminLevels, "key", "value", Model.InfoModel.AdministrativeLevelId),
                        "-- Select Admin Level --",
                        new { @class = "mediumInputElement required", title = "Please select an Admin Level" })%>
                        <%}
                          else
                          {%>
                        <% string value = "None Selected"; if (Model.InfoModel.AdministrativeLevelId != null) { Model.AvailableAdminLevels.TryGetValue((int)Model.InfoModel.AdministrativeLevelId, out value); } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("InfoModel.AdministrativeLevelId", Model.InfoModel.AdministrativeLevelId)%>
                        <br />
                        <% } %>
                    </p>
                    <% } %>
                    <p>
                        <label>
                            TIP ID:</label>
                        <span class="fakeinput medium">
                            <%= Html.Encode(Model.ProjectSummary.TipId)%></span>
                    </p>
                    <p>
                        <label>
                            STIP ID:</label>
                        <%= Html.DrcogTextBox("InfoModel.STIPID", Model.ProjectSummary.IsEditable(), Model.InfoModel.STIPID, null)%>
                    </p>
                    <p>
                        <label>
                            Project Type:</label>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <%= Html.DropDownList("InfoModel.ProjectTypeId",
                    Model.ProjectSummary.IsEditable(), 
                    new SelectList(Model.AvailableProjectTypes, "key", "value", Model.InfoModel.ProjectTypeId), 
                    "-- Select Project Type--", 
                    new { @class = "mediumInputElement required", title="Please select a project type" })%>
                        <%}
                          else
                          {%>
                        <% string value = "None Selected"; if (Model.InfoModel.ProjectTypeId != null) { Model.AvailableProjectTypes.TryGetValue((int)Model.InfoModel.ProjectTypeId, out value); } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("InfoModel.ProjectTypeId", Model.InfoModel.ProjectTypeId)%>
                        <br />
                        <% } %>
                    </p>
                    <p>
                        <label>
                            Improvement Type:</label>
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
                        <% string value = "None Selected"; if (Model.InfoModel.ImprovementTypeId != null) { Model.AvailableImprovementTypes.TryGetValue((int)Model.InfoModel.ImprovementTypeId, out value); } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("InfoModel.ImprovementTypeId", Model.InfoModel.ImprovementTypeId)%>
                        <br />
                        <% } %>
                    </p>
                    <p>
                        <label>
                            Road or Transit:</label>
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
                        <% string value = "None Selected"; if (Model.InfoModel.TransportationTypeId != null) { Model.AvailableRoadOrTransitTypes.TryGetValue((int)Model.InfoModel.TransportationTypeId, out value); } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("InfoModel.TransportationTypeId", Model.InfoModel.TransportationTypeId)%>
                        <br />
                        <% } %>
                    </p>
                    <p>
                        <label>
                            Pool Name:</label>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <%= Html.DropDownList("InfoModel.ProjectPoolId",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.AvailablePools, "key", "value", Model.InfoModel.ProjectPoolId), 
                    "-- Select Pool --",
                    new { @class = "mediumInputElement not-required", title = "Please specify a pool for this project (if applicable)" })%>
                        <%}
                          else
                          {%>
                        <% string value = "None Selected"; if (Model.InfoModel.ProjectPoolId != null) { Model.AvailablePools.TryGetValue((int)Model.InfoModel.ProjectPoolId, out value); } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("InfoModel.ProjectPoolId", Model.InfoModel.ProjectPoolId)%>
                        <br />
                        <% } %>
                    </p>
                    <%--<br />--%>
                    <%--<p>
            <label>Pool Master:</label>                               
            <br />
        </p>--%>
                    <p>
                        <label>
                            Selection Agency:</label>
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
                        <% string value = "None Selected"; if (Model.InfoModel.SelectionAgencyId != null) { Model.AvailableSelectionAgencies.TryGetValue((int)Model.InfoModel.SelectionAgencyId, out value); } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("InfoModel.SelectionAgencyId", Model.InfoModel.SelectionAgencyId)%>
                        <br />
                        <% } %>
                    </p>
                    <p>
                        <label>
                            Regionally Significant?:</label>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <%=Html.CheckBox("InfoModel.IsRegionallySignificant",
                    Model.ProjectSummary.IsEditable(), 
                    Model.InfoModel.IsRegionallySignificant.Value, 
                    new { @class = "smallInputElement not-required", title = "Specify Regional Signifigance!" })%>
                        <%}
                          else
                          {%>
                        <% string value = "No"; if (Model.InfoModel.IsRegionallySignificant != null) { value = (bool)Model.InfoModel.IsRegionallySignificant ? "Yes" : "No"; } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("InfoModel.IsRegionallySignificant", Model.InfoModel.IsRegionallySignificant)%>
                        <br />
                        <% } %>
                    </p>
                </div>
                <div id="tab-info-right">
                    <% if (Request.IsAuthenticated)
                       { %>
                    <p>
                        <label>
                            Sponsor Notes:</label></p>
                    <p>
                        <%= Html.TextArea2("InfoModel_SponsorNotes", 
                    Model.ProjectSummary.IsEditable(),
                    Model.InfoModel.SponsorNotes,
                    0,
                    0,
                    new { @name = "InfoModel.SponsorNotes", @class = "not-required mediumInputElement growable", @rows = "0", title = "Please add the sponsor comments." })%>
                    </p>
                    <p>
                        <label>
                            DRCOG Notes:</label></p>
                    <p>
                        <%= Html.TextArea2("InfoModel_DRCOGNotes",
                    Model.ProjectSummary.IsEditable(),
                    Model.InfoModel.DRCOGNotes,
                    0,
                    0,
                    new { @name = "InfoModel.DRCOGNotes", @class = "mediumInputElement growable" })%>
                    </p>
                    <% } %>
                    <span id="uploadplaceholder"></span>
                </div>
                <div class="clear">
                </div>
                <%if (Model.ProjectSummary.IsEditable())
                  { %>
                <div>
                    <button type="submit" id="submitForm">
                        Save Changes</button>
                    <div id="submit-result">
                    </div>
                </div>
                <% } %>
            </fieldset>
            </form>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
