<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.StatusViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    TIP Status</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
    <script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>"
        type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            "use strict";

            // Prevent accidental navigation away
            App.utility.bindInputToConfirmUnload('#dataForm', '#submitForm', '#submit-result');
            $('#submitForm').button({ disabled: true });

            //setup the date pickers
            $(".datepicker").datepicker();
            $("#Notes").growing();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="view-content-container">
        <%--<h2 ><%=Html.ActionLink("TIP List", "Index",new {controller="TIP"}) %> / TIP <%=Model.TipSummary.TipYear%></h2>--%>
        <div class="clear">
        </div>
        <%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>
        <div id="tipStatusForm" class="tab-form-container">
            <form id="dataForm" action="/api/TipStatus" method="put">
            <div style="margin-right: 50px; margin-top: 10px; float: right;">
                <h2>
                    Funding Increments</h2>
                <fieldset>
                    <legend></legend>
                    <p>
                        <label>
                            Year 1:
                            <%= Html.CheckBox("FundingIncrement_Year_1", Model.IsEditable(), Model.TipStatus.FundingIncrement_Year_1)%>
                        </label>
                    </p>
                    <p>
                        <label>
                            Year 2:
                            <%= Html.CheckBox("FundingIncrement_Year_2", Model.IsEditable(), Model.TipStatus.FundingIncrement_Year_2)%>
                        </label>
                    </p>
                    <p>
                        <label>
                            Year 3:
                            <%= Html.CheckBox("FundingIncrement_Year_3", Model.IsEditable(), Model.TipStatus.FundingIncrement_Year_3)%>
                        </label>
                    </p>
                    <p>
                        <label>
                            Year 4:
                            <%= Html.CheckBox("FundingIncrement_Year_4", Model.IsEditable(), Model.TipStatus.FundingIncrement_Year_4)%>
                        </label>
                    </p>
                    <p>
                        <label>
                            Year 5:
                            <%= Html.CheckBox("FundingIncrement_Year_5", Model.IsEditable(), Model.TipStatus.FundingIncrement_Year_5)%>
                        </label>
                    </p>
                    <p>
                        <label>
                            Year 6:
                            <%= Html.CheckBox("FundingIncrement_Year_6", Model.IsEditable(), Model.TipStatus.FundingIncrement_Year_6)%>
                        </label>
                    </p>
                    <p>
                        <label>
                            Years 4-6:
                            <%= Html.CheckBox("FundingIncrement_Years_4_6", Model.IsEditable(), Model.TipStatus.FundingIncrement_Years_4_6)%>
                        </label>
                    </p>
                    <p>
                        <label>
                            Years 5-6:
                            <%= Html.CheckBox("FundingIncrement_Years_5_6", Model.IsEditable(), Model.TipStatus.FundingIncrement_Years_5_6)%>
                        </label>
                    </p>
                </fieldset>
            </div>
            <fieldset>
                <legend></legend>
                <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
                <h2>
                    Program Start and End Dates</h2>
                <%=Html.Hidden("TimePeriodId", Model.TipStatus.TimePeriodId)%>
                <%=Html.Hidden("ProgramId", Model.TipStatus.ProgramId)%>
                <p>
                    <label>
                        TIP Year:
                    </label>
                    <%=Html.DrcogTextBox("TipYear",Model.TipSummary.IsCurrent,Model.TipSummary.TipYear, new{@class="required", @style="width: 100px;", title="Please specify a tip year (i.e. 2008-2013)"}) %>
                </p>
                <p>
                    <label for="CurrentStatus">
                        Current:</label>
                    <%= Html.CheckBox("IsCurrent", Model.IsEditable(), Model.TipStatus.IsCurrent, new { @id = "CurrentStatus" })%>
                </p>
                <p>
                    <label for="PendingStatus">
                        Pending:</label>
                    <%= Html.CheckBox("IsPending", Model.IsEditable(), Model.TipStatus.IsPending, new { @id = "PendingStatus" })%>
                </p>
                <p>
                    <label for="PreviousStatus">
                        Previous:</label>
                    <%= Html.CheckBox("IsPrevious", Model.IsEditable(), Model.TipStatus.IsPrevious, new { @id = "PreviousStatus" })%>
                </p>
                <h2>
                    TIP Meeting and Approval Dates</h2>
                <p>
                    <label for="summary_PublicHearing" class="beside">
                        Public Hearing:</label>
                    <%= Html.DrcogTextBox("PublicHearing", Model.IsEditable(), Model.TipStatus.PublicHearing.HasValue ? Model.TipStatus.PublicHearing.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                </p>
                <p>
                    <label for="summary_BoardApproval">
                        Board Adoption:</label>
                    <%= Html.DrcogTextBox("Adoption", Model.IsEditable(), Model.TipStatus.Adoption.HasValue ? Model.TipStatus.Adoption.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                </p>
                <p>
                    <label for="summary_GovernorApproval">
                        Governor's Approval:</label>
                    <%= Html.DrcogTextBox("GovernorApproval", Model.IsEditable(), Model.TipStatus.GovernorApproval.HasValue ? Model.TipStatus.GovernorApproval.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                </p>
                <p>
                    <label for="summary_USDOTApproval">
                        U.S. DOT Approval:</label>
                    <%= Html.DrcogTextBox("USDOTApproval", Model.IsEditable(), Model.TipStatus.USDOTApproval.HasValue ? Model.TipStatus.USDOTApproval.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                </p>
                <p>
                    <label for="summary_EPAApproval">
                        U.S. EPA Approval:</label>
                    <%= Html.DrcogTextBox("EPAApproval", Model.IsEditable(), Model.TipStatus.EPAApproval.HasValue ? Model.TipStatus.EPAApproval.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                </p>
                <p>
                    <label for="summary_ShowDelayDate">
                        Delay visibility Date:</label>
                    <%= Html.DrcogTextBox("ShowDelayDate", Model.IsEditable(), Model.TipStatus.ShowDelayDate.HasValue ? Model.TipStatus.ShowDelayDate.Value.ToShortDateString() : "", new { @class = "shortInputElement datepicker" })%>
                </p>
                <p>
                    <label for="summary_Notes">
                        Notes:</label>
                    <%= Html.TextArea2("Notes", Model.IsEditable(), Model.TipStatus.Notes, 10, 1)%>
                </p>
            </fieldset>
            <br />
            <%if (Model.IsEditable())
              { %>
            <div class="relative">
                <button type="submit" id="submitForm">
                    Save Changes</button>
                <div id="submit-result">
                </div>
            </div>
            <%} %>
            </form>
        </div>
    </div>
    <div class="clear">
    </div>
</asp:Content>
