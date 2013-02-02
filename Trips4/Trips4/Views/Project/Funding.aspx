<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.FundingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Funding
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Url.Content("~/scripts/jquery.meio.mask.min.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.eede.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/slide.js")%>" type="text/javascript"></script>
    <link href="<%= Url.Content("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var App = App || {};
        App.pp = App.pp || {};
        App.pp.ProjectVersionId = parseInt('<%=  Model.ProjectSummary.ProjectVersionId %>');
        App.pp.TipYear = '<%=  Model.ProjectSummary.TipYear %>';
        var EditFinancialRecordUrl = '<%=Url.Action("UpdateFinancialRecord")%>';
        var EditFinancialRecordDetailUrl = '<%=Url.Action("UpdateFinancialRecordDetail")%>';
        var AddFinancialRecordDetailUrl = '<%=Url.Action("AddFinancialRecordDetail")%>';
        var DeleteFinancialRecordDetail = '<%=Url.Action("DeleteFinancialRecordDetail")%>';
        var DeletePhaseUrl = '<%=Url.Action("DeletePhase")%>';
        var AddPhaseUrl = '<%=Url.Action("AddPhase")%>';

        $(document).ready(App.tabs.initializeTipProjectTabs);
    </script>
    <script src="<%= Url.Content("~/scripts/Funding.js")%>" type="text/javascript"></script>
    <script src="<%= Url.Content("~/scripts/jquery.formatCurrency-1.4.0.min.js")%>"
        type="text/javascript"></script>
    <link href="<%= Url.Content("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tab-content-container funding-tab">
        <% Html.RenderPartial("~/Views/Project/Partials/TipProjectTabPartial.ascx", Model.ProjectSummary); %>
        <div class="ui-widget">
            <%= Html.Hidden("CurrentFundingPeriodID", Model.ProjectSummary.TipYearTimePeriodID) %>
            <%= Html.Hidden("ProjectVersionId", Model.ProjectSummary.ProjectVersionId) %>
            <%= Html.Hidden("ProjectFinancialRecordId", Model.ProjectSummary.ProjectFinancialRecordId) %>
            <% if (ViewData["message"] != null)
               { %>
            <div id="message" class="info">
                <%= ViewData["message"].ToString() %></div>
            <% } %>
            <!-- FINANCIAL RECORD DETAILS -->
            <div id="financial-record-details">
                <div style="width: 100%;">
                    <h2 style="float: left; text-align: left;">
                        Project Financial Record Details</h2>
                    <span style="float: right; text-align: right;"><span style="font-size: 1.2em; font-weight: bold;">
                        { <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span>
                        }</span></span>
                    <div style="clear: both;">
                    </div>
                </div>
                <div id="resultRecordDetail">
                </div>
                <table>
                    <tr>
                        <th style="width: 250px;">
                            Funding Group
                        </th>
                        <th style="width: 100px;">
                            Funding Level
                        </th>
                        <% foreach (var increment in Model.FundingDetailPivotModel.FundingIncrements)
                           { %>
                        <th style="width: 75px;">
                            <%= increment.FundingIncrementName.ToString() %>
                        </th>
                        <% } %>
                        <%if (Model.ProjectSummary.IsInAmendment)
                          { %>
                        <th>
                            &nbsp;
                        </th>
                        <% } %>
                    </tr>
                    <% foreach (System.Data.DataRow item in Model.FundingDetailPivotModel.FundingDetailTable.Rows)
                       {
                           if ((bool)item[8])
                           { %>
                    <tr id="fundingdetail_row_<%= item[1].ToString() + '_' + item[3].ToString() %>">
                        <td id="fundingdetail_<%= item[1].ToString() + '_' + item[3].ToString() %>_FundingType">
                            <%= item[2].ToString() %>
                        </td>
                        <td id="fundingdetail_<%= item[1].ToString() + '_' + item[3].ToString() %>_FundingLevel">
                            <%= item[4].ToString() %>
                        </td>
                        <% for (int j = 9; j < item.Table.Columns.Count; j++)
                           { %>
                        <td>
                            <%= Html.DrcogTextBox("fundingdetail_" + item[1].ToString() + "_" + item[3].ToString() + "_i" + (j - 8), Model.ProjectSummary.IsInAmendment, item[j].ToString(), new { style = "width:75px;", @maxlength = "75", @class = "money", @alt = "money" })%>
                        </td>
                        <% }
                } %>
                        <%  int pt = 0;
                            int ft = 0, st = 0, lt = 0;
                            int lastColumnIndex = (item.Table.Columns.Count - 1);
                            for (int j = 9; j < (item.Table.Columns.Count); j++)
                            {
                                int parseOut = default(int);
                                if (int.TryParse(item[j].ToString(), out parseOut))
                                {
                                    if (j < (item.Table.Columns.Count - 1)) pt = pt + parseOut;
                                    if (item[4].ToString().Equals("Federal")) { ft = ft + parseOut; }
                                    if (item[4].ToString().Equals("State")) { st = st + parseOut; }
                                    if (item[4].ToString().Equals("Local")) { lt = lt + parseOut; }
                                }
                            } %>
                        <%= Html.Hidden("hdnProgrammedTotal_" + item[1].ToString() + '_' + item[3].ToString(), pt.ToString())%>
                        <%= Html.Hidden("hdnOutyearTotal_" + item[1].ToString() + '_' + item[3].ToString(), int.Parse(item[lastColumnIndex].ToString()))%>
                        <%= Html.Hidden("hdnFederalTotal_" + item[1].ToString() + '_' + item[3].ToString(), ft.ToString())%>
                        <%= Html.Hidden("hdnStateTotal_" + item[1].ToString() + '_' + item[3].ToString(), st.ToString())%>
                        <%= Html.Hidden("hdnLocalTotal_" + item[1].ToString() + '_' + item[3].ToString(), lt.ToString())%>
                        <%if (Model.ProjectSummary.IsInAmendment && (bool)item[8])
                          { %>
                        <td>
                            <a id="updateFRD_<%=item[0].ToString() + '_' + item[1].ToString() + '_' + item[3].ToString() %>"
                                class="update-financialrecorddetail fg-button w70 medium ui-state-default ui-priority-primary ui-corner-all"
                                href="#">Update</a>
                        </td>
                        <% } %>
                    </tr>
                    <% } %>
                </table>
            </div>
            <%if (Model.ProjectSummary.IsInAmendment)
              { %>
            <div>
                <a id="AddFundingGroup" class="add-financialrecorddetail fg-button w380 medium ui-state-default ui-priority-primary ui-corner-all"
                    href="#">Add new Funding Group</a>
                <%= Html.DropDownList("FinancialRecordDetail.ProjectFundingResources",
                true,
                new SelectList(Model.ProjectFundingResources, "key", "value"),
                "-- Delete a funding resource --", 
                new { @class = "mediumInputElement not-required", title="Please select a funding resource to delete" })%>
                <a id="DeleteFundingGroup" class="delete-financialrecorddetail fg-button medium ui-state-default ui-priority-primary ui-corner-all"
                    href="#">Delete Funding Group</a>
            </div>
            <% } %>
            <div style="float: left; width: 100%;">
                &nbsp;</div>
            <div id="funding-phases">
                <h2>
                    Project Phases</h2>
                <table>
                    <tr>
                        <th>
                            Year
                        </th>
                        <th>
                            Phase
                        </th>
                        <th>
                            Resource
                        </th>
                        <th>
                            Delayed
                        </th>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %><th>
                        </th>
                        <% } %>
                    </tr>
                    <% foreach (var item in Model.FundingPhases)
                       { %>
                    <tr id="phase_row_<%= item.ProjectFinancialRecordId.ToString() %>_<%= item.FundingIncrementId.ToString() %>_<%= item.FundingResourceId.ToString() %>_<%= item.PhaseId.ToString() %>">
                        <td>
                            <%= item.Year %>
                        </td>
                        <td>
                            <%= item.Phase %>
                        </td>
                        <td>
                            <%= item.FundingResource %>
                        </td>
                        <td style="color: red; font-weight: bold;">
                            <%= item.IsDelayed ? "YES" : "" %>
                        </td>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td>
                            <button class="delete-phase fg-button ui-state-default ui-priority-primary ui-corner-all"
                                id='delete_<%= item.ProjectFinancialRecordId.ToString() %>_<%= item.FundingIncrementId.ToString() %>_<%= item.FundingResourceId.ToString() %>_<%= item.PhaseId.ToString() %>'>
                                Delete
                            </button>
                        </td>
                        <% } %>
                    </tr>
                    <% } %>
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                    <tr id="phase-editor">
                        <td>
                            <%= Html.DropDownList("new_year",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.FundingYearsAvailable, "key", "value", ""), 
                    "-- Year --",
                    new { @class = "mediumInputElement", title = "Select a Year.", @id="new_year" })%>
                        </td>
                        <td>
                            <%= Html.DropDownList("new_phase",
                    Model.ProjectSummary.IsEditable(),
                    new SelectList(Model.FundingPhasesAvailable, "key", "value", ""), 
                    "-- Phase --",
                    new { @class = "mediumInputElement", title = "Select a Phase.", @id="new_phase" })%>
                        </td>
                        <td>
                            <%= Html.DropDownList("new_fundingresource",
                true,
                new SelectList(Model.ProjectFundingResources, "key", "value"),
                "-- Funding Resource --", 
                new { @class = "mediumInputElement", title="Please select a Funding Resource." })%>
                        </td>
                        <td>
                        </td>
                        <td>
                            <button id="add-phase" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">
                                Add</button>
                        </td>
                    </tr>
                    <% } %>
                </table>
            </div>
            <br />
            <!-- FINANCIAL DETAIL SUMMARY -->
            <div id="financial-record-detail-summary">
                <div style="float: left; width: 100%;">
                    <h2 style="float: left; text-align: left;">
                        Project Financial Record Details Summary</h2>
                    <span style="float: right; text-align: right;"><span style="font-size: 1.2em; font-weight: bold;">
                        { <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span>
                        }</span></span>
                </div>
                <br />
                <table style="float: left;">
                    <tr>
                        <th>
                            Previous
                        </th>
                        <th>
                            Programmed
                        </th>
                        <th>
                            OutYear
                        </th>
                        <th>
                            Future
                        </th>
                        <th>
                            Total
                        </th>
                        <%if (Model.ProjectSummary.IsInAmendment)
                          { %><th>
                        </th>
                        <% } %>
                    </tr>
                    <% foreach (var item in Model.TipProjectFunding)
                       { %>
                    <tr id="financialrecord_row_<%=item.ProjectFinancialRecordID.ToString() %>">
                        <td>
                            <div id="previousTotalLabel">
                                <%= Html.DrcogTextBox("previousTotalTextBox", Model.ProjectSummary.IsInAmendment, item.Previous.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></div>
                        </td>
                        <td>
                            <div id="programmedTotalLabel" class="currency">
                                0</div>
                        </td>
                        <td>
                            <div id="outyearTotalLabel" class="currency">
                                0</div>
                        </td>
                        <td>
                            <div id="futureTotalLabel">
                                <%= Html.DrcogTextBox("futureTotalTextBox", Model.ProjectSummary.IsInAmendment, item.Future.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></div>
                        </td>
                        <td>
                            <div id="projectCostTotalLabel" class="currency">
                                0</div>
                        </td>
                        <%if (Model.ProjectSummary.IsInAmendment)
                          { %>
                        <td>
                            <button class="update-financialrecord fg-button ui-state-default ui-priority-primary ui-corner-all"
                                id='updateFR_<%=item.ProjectFinancialRecordID.ToString() %>'>
                                Update</button>
                        </td>
                        <% } %>
                    </tr>
                    <% } %>
                </table>
                <table style="float: right;">
                    <tr>
                        <th>
                            Federal<br />
                            Total
                        </th>
                        <th>
                            State<br />
                            Total
                        </th>
                        <th>
                            Local<br />
                            Total
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <div id="federalTotalLabel" class="currency">
                                0</div>
                        </td>
                        <td>
                            <div id="stateTotalLabel" class="currency">
                                0</div>
                        </td>
                        <td>
                            <div id="localTotalLabel" class="currency">
                                0</div>
                        </td>
                    </tr>
                </table>
                <div style="clear: both;">
                </div>
                <br />
            </div>
            <%--<div style="float: left; width: 100%;">&nbsp;</div>--%>
            <!-- FINANCIAL RECORD -->
            <!-- Removed. -DBD -->
            <!-- FINANCIAL RECORD HISTORY -->
            <div id="financial-details-history">
                <div style="width: 100%;">
                    <h2 style="float: left; text-align: left;">
                        Project Financial Records History</h2>
                    <span style="float: right; text-align: right;"><span style="font-size: 1.2em; font-weight: bold;">
                        { <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span>
                        }</span></span>
                    <div style="clear: both;">
                    </div>
                </div>
                <table id="financial-record-history">
                    <tr>
                        <th>
                        </th>
                        <th>
                            Amend<br />
                            Date
                        </th>
                        <th>
                            Amend<br />
                            Status
                        </th>
                        <th>
                            Previous
                        </th>
                        <th>
                            Future
                        </th>
                        <%--<th>TipFunding</th>--%>
                        <th>
                            Federal<br />
                            Total
                        </th>
                        <th>
                            State<br />
                            Total
                        </th>
                        <th>
                            Local<br />
                            Total
                        </th>
                        <th>
                            Total<br />
                            Cost
                        </th>
                    </tr>
                    <% foreach (var item in Model.ProjectFundingHistory)
                       { %>
                    <tr id="financialrecordhistory_row_<%=item.ProjectFinancialRecordID.ToString() %>">
                        <td>
                            <% if (item.ProjectVersionId.ToString() == Model.ProjectSummary.ProjectVersionId.ToString())
                               { %>
                            Current
                            <% }
                               else
                               { %>
                            <a href="<%= Url.Action("Details", "Project", new { tipYear = Model.ProjectSummary.TipYear, id = item.ProjectVersionId.ToString() }) %>">
                                Details</a>
                            <% } %>
                        </td>
                        <td>
                            <%= item.AmendmentDate.Value.Date.ToShortDateString() %>
                        </td>
                        <td>
                            <%= item.AmendmentStatus.ToString() %>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_Previous", Model.ProjectSummary.IsInAmendment, item.Previous.ToString(), new { @style = "width:75px;", @maxlength = "75", @alt = "money" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_Future", Model.ProjectSummary.IsInAmendment, item.Future.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%>
                        </td>
                        <%--<td><%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_Funding", Model.ProjectSummary.IsInAmendment, item.Funding.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>--%>
                        <td>
                            <%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_FederalTotal", Model.ProjectSummary.IsInAmendment, item.FederalTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_StateTotal", Model.ProjectSummary.IsInAmendment, item.StateTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_LocalTotal", Model.ProjectSummary.IsInAmendment, item.LocalTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%>
                        </td>
                        <td>
                            <%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_TotalCost", Model.ProjectSummary.IsInAmendment, item.TotalCost.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%>
                        </td>
                    </tr>
                    <% } %>
                </table>
            </div>
            <div style="float: left; width: 100%;">
                &nbsp;</div>
        </div>
    </div>
    <!-- This contains the hidden content for inline calls. Now becomes the Add Funding Group dialog. -DBD -->
    <div style='display: none'>
        <div id='add-newGroup-panel' style='padding: 10px; background: #fff;'>
            <table>
                <%if (Model.ProjectSummary.IsInAmendment)
                  { %>
                <tr>
                    <td colspan="6">
                        <%= Html.DropDownList("FinancialRecordDetail.FundingTypes",
                                true,
                                new SelectList(Model.FundingTypes, "key", "value"),
                                "-- Select a Funding Type --", 
                                new { @class = "mediumInputElement not-required", title="Please select a funding type" })%>
                    </td>
                    <td>
                        <a id="AddFRD_<%= Model.ProjectSummary.ProjectVersionId.ToString() %>" class="add-newFundingGroup-final fg-button w70 medium ui-state-default ui-priority-primary ui-corner-all"
                            href="#">Add</a>
                    </td>
                </tr>
                <% } %>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
