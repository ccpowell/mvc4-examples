<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.ReportsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    TIP Reports</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="clear">
    </div>
    <%Html.RenderPartial("~/Views/RTP/Partials/TabPartial.ascx", Model.RtpSummary); %>
    <div class="tab-form-container">
        <div class="ui-widget leftColumn">
            <h2>
                General Reports</h2>
            <a id="button-nd-conformity" class="fg-button w380 ui-state-default ui-corner-all"
                href="#">NDConformity</a><a id="button-appendix-4" class="fg-button w380 ui-state-default ui-corner-all"
                    href="#">Appendix 4</a> <a id="button-project-list" class="fg-button w380 ui-state-default ui-corner-all"
                        href="#">Project List</a><a id="button-modeler" class="fg-button w380 ui-state-default ui-corner-all" href="#">
                    Modeler Extract</a>
        </div>
        <div class="ui-widget rightColumn">
        </div>
    </div>
    <div class="clear">
    </div>
    <div style='display: none'>
        <div id="dialog-nd-conformity" class="dialog">
            <h2>
                Report: NDConformity
            </h2>
            <div>
                <label for="report-nd-conformity-planCycle" class="big">
                    For Cycle:
                </label>
                <%= Html.DropDownList("report-nd-conformity-planCycle", new SelectList(Model.CurrentPlanCycles, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </div>
            <div>
                <label for="report-nd-conformity-excludeBefore" class="big">
                    Exclude Before Year:
                </label>
                <input type="text" id="report-nd-conformity-excludeBefore" class="big" />
            </div>
            <div>
                <label for="report-nd-conformity-surveyYear" class="big">
                    Survey Year:
                </label>
                <%= Html.DropDownList("report-nd-conformity-surveyYear", new SelectList(Model.SurveyYears, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </div>
            <div>
                <label for="report-nd-conformity-excludeSurveyBefore" class="big">
                    Exclude Before Year:
                </label>
                <input type="text" id="report-nd-conformity-excludeSurveyBefore" class="big" />
            </div>
            <% Html.RenderPartial("DocumentFormatSelection", "report-nd-conformity-format"); %>
            <button id="report-nd-conformity" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report
            </button>
        </div>
        <div id="dialog-appendix-4" class="dialog">
            <h2>
                Report: Appendix 4
            </h2>
            <div>
                <label for="report-appendix-4-planCycle" class="big">
                    For Cycle:
                </label>
                <%= Html.DropDownList("report-appendix-4-planCycle", new SelectList(Model.CurrentPlanCycles, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </div>
            <div>
                <label for="report-appendix-4-excludeBefore" class="big">
                    Exclude Before Year:
                </label>
                <input type="text" id="report-appendix-4-excludeBefore" class="big" />
            </div>
            <% Html.RenderPartial("DocumentFormatSelection", "report-appendix-4-format"); %>
            <button id="report-appendix-4" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report
            </button>
        </div>
        <div id="dialog-project-list" class="dialog">
            <h2>
                Report: Project List
            </h2>
            <div>
                <label for="report-project-list-planCycle" class="big">
                    For Cycle:
                </label>
                <%= Html.DropDownList("report-project-list-planCycle", new SelectList(Model.CurrentPlanCycles, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </div>
            <div>
                <label for="report-project-list-excludeBefore" class="big">
                    Exclude Before Year:
                </label>
                <input type="text" id="report-project-list-excludeBefore" class="big" />
            </div>
            <% Html.RenderPartial("DocumentFormatSelection", "report-project-list-format"); %>
            <button id="report-project-list" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report
            </button>
        </div>
        <div id="dialog-modeler" class="dialog">
            <h2>
                Report: Modeler Extract
            </h2>
            <div>
                <label for="report-modeler-excludeBefore" class="big">
                    Exclude Before Year:
                </label>
                <input type="text" id="report-modeler-excludeBefore" class="big" />
            </div>
            <div>This report is only available in Excel format.</div>
            <button id="report-modeler" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report
            </button>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {
            "use strict";
            function getReportOptions(prefix) {
                var format = $("input[name=" + prefix + "-format]:checked").val(),
                    planCycle = $("#" + prefix + "-planCycle").val(),
                    excludeBefore = $("#" + prefix + "-excludeBefore").val(),
                    surveyYear = $("#" + prefix + "-surveyYear").val(),
                    excludeSurveyBefore = $("#" + prefix + "-excludeSurveyBefore").val();

                return {
                    format: format,
                    planCycle: planCycle,
                    surveyYear: surveyYear,
                    excludeSurveyBefore: excludeSurveyBefore,
                    excludeBefore: excludeBefore
                };
            }

            function getReportUrl(prefix, allCycleUrl, singleCycleUrl) {
                var options = getReportOptions(prefix),
                    reportUrl = allCycleUrl;
                if (options.planCycle > 0) {
                    reportUrl = singleCycleUrl;
                    reportUrl += "&rs:CycldID=" + options.planCycle;
                }
                if (options.excludeBefore > 0) {
                    reportUrl += "&rs:ExcludeOpenBefore=" + options.excludeBefore;
                }
                if (options.surveyYear > 0) {
                    reportUrl += "&rs:IncludeSurveyYearID=" + options.surveyYear;
                }
                if (options.excludeSurveyBefore > 0) {
                    reportUrl += "&rs:ExcludeSurveyOpenBefore=" + options.excludeSurveyBefore;
                }
                reportUrl += "&rs:Format=" + options.format;
                return reportUrl;
            }

            $("#report-nd-conformity").click(function () {
                var allCycleUrl = "http://sqlprod/reportserver?/TransportationReports/RTP.NetworkChangesByLocation.ConformityModeling&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>",
                    singleCycleUrl = "http://sqlprod/reportserver?/TransportationReports/RTP.NetworkChangesByLocation.ConformityModeling.AllCycles&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>",
                    reportUrl = getReportUrl("report-nd-conformity", allCycleUrl, singleCycleUrl);

                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-nd-conformity").colorbox({
                width: "700px",
                inline: true,
                href: "#dialog-nd-conformity"
            });

            $("#report-appendix-4").click(function () {
                var allCycleUrl = "http://sqlprod/reportserver?/TransportationReports/RTP.ProjectsList.Appendix4.AllCycles&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>",
                    singleCycleUrl = "http://sqlprod/reportserver?/TransportationReports/RTP.ProjectsList.Appendix4&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>",
                    reportUrl = getReportUrl("report-appendix-4", allCycleUrl, singleCycleUrl);

                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-appendix-4").colorbox({
                width: "700px",
                inline: true,
                href: "#dialog-appendix-4"
            });

            $("#report-project-list").click(function () {
                var allCycleUrl = "http://sqlprod/reportserver?/TransportationReports/RTP.ProjectsList.Appendix4.AllCycles&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>",
                    singleCycleUrl = "http://sqlprod/reportserver?/TransportationReports/RTP.ProjectsList.Appendix4&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>",
                    reportUrl = getReportUrl("report-project-list", allCycleUrl, singleCycleUrl);

                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-project-list").colorbox({
                width: "700px",
                inline: true,
                href: "#dialog-project-list"
            });

            $("#report-modeler").click(function () {
                var excludeBefore = $("#report-modeler-excludeBefore").val(),
                    reportUrl = '<%= Url.Action("DownloadModelerExtract", new {timePeriodId = Model.RtpSummary.RTPYearTimePeriodID}) %>';

                if (excludeBefore > 0) {
                    reportUrl += "&excludeOpenBefore=" + excludeBefore;
                }
                    
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-modeler").colorbox({
                width: "700px",
                inline: true,
                href: "#dialog-modeler"
            });
        });
    
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script type="text/javascript">
        $(document).ready(App.tabs.initializeRtpTabs);
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
