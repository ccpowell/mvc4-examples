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
                        href="#">Project List</a>
        </div>
        <div class="ui-widget rightColumn">
        </div>
    </div>
    <div class="clear">
    </div>
    <div style='display: none'>
        <div id="dialog-nd-conformity" class="dialog">
            <h2>
                Report: NDConformity</h2>
            <% Html.RenderPartial("DocumentFormatSelection", "report-nd-conformity-format"); %>
            <button id="report-nd-conformity" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-appendix-4" class="dialog">
            <h2>
                Report: Appendix 4</h2>
            <% Html.RenderPartial("DocumentFormatSelection", "report-appendix-4-format"); %>
            <button id="report-appendix-4" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-project-list" class="dialog">
            <h2>
                Report: Project List</h2>
            <% Html.RenderPartial("DocumentFormatSelection", "report-project-list-format"); %>
            <button id="report-project-list" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {
            $("#report-nd-conformity").click(function () {
                var reportUrl,
                    format = $("input[name=report-nd-conformity-format]:checked").val();
                reportUrl = "http://sqlprod/reportserver/TransportationReports/RTP.NetworkChangesByLocation.ConformityModeling.AllCycles&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>" + "&rs:Format=" + format;
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-nd-conformity").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-nd-conformity"
            });
        });

        $(function () {
            $("#report-appendix-4").click(function () {
                var reportUrl,
                    format = $("input[name=report-appendix-4-format]:checked").val();
                reportUrl = "http://sqlprod/reportserver?/TransportationReports/RTP.ProjectsList.Appendix4.AllCycles&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>" + "&rs:Format=" + format;
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-appendix-4").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-appendix-4"
            });
        });

        $(function () {
            $("#report-project-list").click(function () {
                var reportUrl,
                    format = $("input[name=report-project-list-format]:checked").val();
                reportUrl = "http://sqlprod/reportserver?/TransportationReports/RTP.ProjectsList.Appendix4.AllCycles&rs:Command=Render&rc:Parameters=false&YearID=<%= Model.RtpSummary.RTPYearTimePeriodID %>" + "&rs:Format=" + format;
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-project-list").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-project-list"
            });
        });
    
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
