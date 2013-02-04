<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<LocationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project Location</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.popeye.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/jquery.popeye.style.css") %>" rel="stylesheet"
        type="text/css" />
    <script src="<%= Url.Content("~/scripts/jquery.growing-textarea.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.popeye-2.0.4.min.js")%>" type="text/javascript"></script>
    <!-- 
    Slide was originally for a login panel, and maybe was to be used for a details panel
    which is currently not implemented. At this point it just keeps the details panel hidden.
    Remove ProjectSummaryBoxPartial and it will not be needed.
     -->
    <link href="<%= ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(App.tabs.initializeTipProjectTabs);
    </script>
    <script src="<%= Url.Content("~/scripts/TipProjectLocation.js")%>" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tab-content-container">
        <% Html.RenderPartial("~/Views/Project/Partials/TipProjectTabPartial.ascx", Model.ProjectSummary); %>
        <div class="tab-form-container">
            <form method="put" action="/api/TipProjectLocation" id="dataForm">
            <%=Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
            <%=Html.Hidden("TipYear", Model.ProjectSummary.TipYear)%>
            <%=Html.Hidden("ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
            <%=Html.Hidden("ProjectId", Model.ProjectSummary.ProjectId)%>
            <div id="top-fullwidth" class="box">
                <p>
                    <label>
                        Facility Name:</label>
                    <%= Html.DrcogTextBox("FacilityName", 
                        Model.ProjectSummary.IsEditable(), 
                        Model.TipProjectLocation.FacilityName, 
                        new { @class = "longInputElement required", title="Please enter a primary road or facility name." })%>
                </p>
                <p>
                    <label>
                        Limits (cross streets) or Area of Project Description</label></p>
                <p>
                    <%= Html.TextArea2("Limits", Model.ProjectSummary.IsEditable(), Model.TipProjectLocation.Limits, 5, 10, new { @class = "longInputElement growable" }) %>
                    <%--<textarea class="longInputElement" name="Limits"><%=Model.TipProjectLocation.Limits %></textarea>--%>
                </p>
                <p>
                    <label>
                        Route # (if applicable)</label>
                </p>
                <div>
                    <br />
                    <%--<%= Html.DropDownListFor(x => x.TipProjectLocation.CdotRegionId, Model.CDOTRegions, new { @class = "mediumInputElement" })%>--%>
                    <p>
                        <h2>
                            Affected CDOT Region</h2>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <%= Html.DropDownList("CdotRegionId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.CDOTRegions, "value", "text", Model.TipProjectLocation.CdotRegionId),
                            "-- Select a CDOT Region --",
                            new { @class = "mediumInputElement", title="Please select a CDOT Region." })%>
                        <%}
                          else
                          {%>
                        <% string value = "None Selected"; if (Model.TipProjectLocation.CdotRegionId != default(int)) { value = Model.CDOTRegions.FirstOrDefault(x => x.Value == Model.TipProjectLocation.CdotRegionId.ToString()).Text; } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("CdotRegionId", Model.TipProjectLocation.CdotRegionId)%>
                        <br />
                        <% } %>
                    </p>
                    <p>
                        <h2>
                            Affected Project Delays Location</h2>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <%= Html.DropDownList("AffectedProjectDelaysLocationId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AffectedProjectDelaysLocation, "value", "text", Model.TipProjectLocation.AffectedProjectDelaysLocationId),
                            "-- Select a Location --",
                            new { @class = "mediumInputElement", title = "Please select an Affected Project Delays Location." })%>
                        <%}
                          else
                          {%>
                        <% string value = "None Selected"; if (Model.TipProjectLocation.AffectedProjectDelaysLocationId != default(int)) { value = Model.AffectedProjectDelaysLocation.FirstOrDefault(x => x.Value == Model.TipProjectLocation.AffectedProjectDelaysLocationId.ToString()).Text; } %>
                        <span class="fakeinput medium">
                            <%= Html.Encode(value)%></span>
                        <%= Html.Hidden("AffectedProjectDelaysLocationId", Model.TipProjectLocation.AffectedProjectDelaysLocationId)%>
                        <br />
                        <% } %>
                    </p>
                </div>
                <%if (Model.ProjectSummary.IsEditable())
                  { %>
                <div id="submit-container" class="long">
                    <button type="submit" id="submitForm">
                        Save Changes</button>
                    <div id="submit-result">
                    </div>
                </div>
                <%} %>
                <div class="clear">
                </div>
            </div>
            <div>
                <br />
                <h2>
                    Affected Counties</h2>
                <% if (Model.ProjectSummary.IsEditable())
                   { %>
                <p>
                    Note: This is not versioned. Editing these values will effect all versions of this
                    project</p>
                <% } %>
                <table id="county-shares">
                    <tr>
                        <td>
                            Primary
                        </td>
                        <td>
                            Share
                        </td>
                        <td>
                            County
                        </td>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td>
                        </td>
                        <% } %>
                    </tr>
                    <%foreach (DRCOG.Domain.Models.CountyShareModel cty in Model.CountyShares)
                      { %>
                    <tr id="county_row_<%=cty.CountyId.ToString() %>">
                        <td>
                            <%= Html.SimpleCheckBox("cty_" + cty.CountyId.ToString() + "_IsPrimary", cty.Primary.Value, cty.ProjectId.ToString() + "_isPrimary", Model.ProjectSummary.IsEditable()  )%>
                        </td>
                        <td>
                            <span id="cshare_<%= cty.ProjectId.ToString()%>">
                                <%= (int)cty.Share.Value %></span>%
                        </td>
                        <%--<%= Html.DrcogTextBox("cty_" + cty.CountyId.ToString() + "_share", Model.ProjectSummary.IsEditable(), (int)cty.Share.Value, new { style = "width:30px;", @id = "cshare_" + cty.ProjectId.ToString() })%>%
                    </td>--%>
                        <td>
                            <%=cty.CountyName %>
                        </td>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td>
                            <button class="delete-county fg-button ui-state-default ui-priority-primary ui-corner-all"
                                data-name="<%= cty.CountyName %>" data-id="<%= cty.CountyId %>">
                                Delete</button>
                        </td>
                        <%} %>
                    </tr>
                    <%} %>
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                    <tr id="county-editor">
                        <td>
                            <%= Html.CheckBox("IsPrimary", Model.ProjectSummary.IsEditable(), false, new { @id ="new_primary" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("Share", Model.ProjectSummary.IsEditable(), 0, new { style = "width:30px;", @id = "cshare_new" })%>%
                        </td>
                        <td>
                            <%= Html.DropDownList("County",
                                    Model.ProjectSummary.IsEditable(),
                                    new SelectList(Model.AvailableCounties, "key", "value", ""), 
                                    "-- Select a County--",
                                    new { @class = "mediumInputElement", title = "Please enter a project sponsor agency.", @id="new_county" })%>
                        </td>
                        <td>
                            <button id="add-county" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">
                                Add</button>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td>
                            Share Total:
                        </td>
                        <td>
                            <div id="county-share-sum">
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td>
                            &nbsp;
                        </td>
                        <%} %>
                    </tr>
                </table>
            </div>
            <div>
                <br />
                <h2>
                    Affected Municipalities</h2>
                <% if (Model.ProjectSummary.IsEditable())
                   { %>
                <p>
                    Note: This is not versioned. Editing these values will effect all versions of this
                    project</p>
                <%} %>
                <table id="muni-shares">
                    <tr>
                        <td>
                            Primary
                        </td>
                        <td>
                            Share
                        </td>
                        <td>
                            Municipality
                        </td>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td>
                        </td>
                        <%} %>
                    </tr>
                    <%foreach (DRCOG.Domain.Models.MunicipalityShareModel muni in Model.MuniShares)
                      { %>
                    <tr id="muni_row_<%=muni.MunicipalityId.ToString() %>">
                        <td>
                            <%= Html.SimpleCheckBox("muni_" + muni.MunicipalityId.ToString() + "_IsPrimary", muni.Primary.Value,   muni.ProjectId.ToString() + "_isPrimary" ,Model.ProjectSummary.IsEditable())%>
                        </td>
                        <td>
                            <span id="mshare_<%= muni.ProjectId.ToString()%>">
                                <%= (int)muni.Share.Value %></span>%
                        </td>
                        <%--<td><%= Html.DrcogTextBox("muni_" + muni.MunicipalityId.ToString() + "_share", Model.ProjectSummary.IsEditable(), (int)muni.Share.Value, new { style = "width:30px;", @id = "mshare_" + muni.ProjectId.ToString() })%>%</td>--%>
                        <td>
                            <%=muni.MunicipalityName%>
                        </td>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td>
                            <button class="delete-muni fg-button ui-state-default ui-priority-primary ui-corner-all"
                                data-name="<%= muni.MunicipalityName %>" data-id='<%= muni.MunicipalityId %>'>
                                Delete</button>
                        </td>
                        <%} %>
                    </tr>
                    <%} %>
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                    <tr id="muni-editor">
                        <td>
                            <%= Html.CheckBox("IsPrimary", Model.ProjectSummary.IsEditable(), false, new { @id = "new_muni_primary" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("Share", Model.ProjectSummary.IsEditable(), 0, new { style = "width:30px;", @id = "mshare_new" })%>%
                        </td>
                        <td>
                            <%= Html.DropDownList("Municipality",
                                        Model.ProjectSummary.IsEditable(),
                                        new SelectList(Model.AvailableMunicipalities, "key", "value", ""), 
                                        "-- Select a Municipality--",
                                        new { @class = "mediumInputElement", title = "Please enter a project sponsor agency.", @id="new_muni" })%>
                        </td>
                        <td>
                            <button disabled="disabled" id="add-muni" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">
                                Add</button>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td>
                            Share Total:
                        </td>
                        <td>
                            <div id="muni-share-sum">
                            </div>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td>
                            &nbsp;
                        </td>
                        <%} %>
                    </tr>
                </table>
            </div>
            </form>
            <div id="locationmap">
                <span id="uploadplaceholder"></span>
                <div id="uploadwrapper">
                    <% using (Html.BeginForm("UpdateImage", "Project", FormMethod.Post, new { id = "imageForm", enctype = "multipart/form-data" }))
                       {%>
                    <%= Html.Hidden("LocationMapId", Model.TipProjectLocation.LocationMapId) %>
                    <%= Html.Hidden("Year", Model.TipProjectLocation.Year)%>
                    <%= Html.Hidden("ProjectVersionId", Model.TipProjectLocation.ProjectVersionId)%>
                    <% if (!Model.TipProjectLocation.LocationMapId.Equals(default(int)) && Model.TipProjectLocation.Image != null)
                       { %>
                    <div class="ppy" id="ppy1">
                        <ul class="ppy-imglist">
                            <li><a href="<%= Url.Action("ShowPhoto", "Project", new { id = Model.TipProjectLocation.LocationMapId }) %>">
                                <img src='<%= Url.Action("ShowPhoto", "Project", new { id = Model.TipProjectLocation.LocationMapId }) %>'
                                    id="image_<%= Model.TipProjectLocation.LocationMapId %>" class="resize" alt="<%= Model.TipProjectLocation.Image.Name %>" />
                            </a></li>
                        </ul>
                        <div class="ppy-outer">
                            <div class="ppy-stage-wrap">
                                <div class="ppy-stage">
                                    <div class="ppy-counter">
                                        <strong class="ppy-current"></strong>/ <strong class="ppy-total"></strong>
                                    </div>
                                </div>
                            </div>
                            <div class="ppy-nav">
                                <div class="ppy-nav-wrap">
                                    <a class="ppy-switch-enlarge" title="Enlarge">Enlarge</a> <a class="ppy-switch-compact"
                                        title="Close">Close</a>
                                </div>
                            </div>
                        </div>
                    </div>
                    <% if (Model.ProjectSummary.IsEditable())
                       { %>
                    <br />
                    <button id="delete-image" data-imageid='<%= Model.TipProjectLocation.LocationMapId %>'>
                        Remove</button>
                    <% } %>
                    <% } %>
                    <% if (Model.ProjectSummary.IsEditable())
                       { %>
                    <% if (Model.TipProjectLocation.LocationMapId.Equals(default(int)) && Model.TipProjectLocation.Image == null)
                       { %>
                    <div id="upload-image">
                        <p>
                            Maximum Upload Size: 4Mb<br />
                        </p>
                        <p>
                            <input type="file" id="fileUpload" name="fileUpload" /></p>
                        <p>
                            <button type="submit" id="submitImageForm" class="fg-button ui-state-default ui-priority-primary ui-corner-all">
                                Upload</button>
                        </p>
                    </div>
                    <% } %>
                    <% } %>
                    <% } %>
                </div>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
</asp:Content>
