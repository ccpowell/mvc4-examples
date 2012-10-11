<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.ReportsViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    TIP Reports</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--<h2 ><%=Html.ActionLink("TIP List", "Index",new {controller="TIP"}) %> / TIP <%=Model.TipSummary.TipYear%></h2>--%>
    <div class="clear">
    </div>
    <%Html.RenderPartial("~/Views/TIP/Partials/TipTabPartial.ascx", Model.TipSummary); %>
    <div class="tab-form-container">
        <div class="ui-widget leftColumn">
            <h2>
                General Reports</h2>
            <a id="button-projectsbysponsor" class="fg-button w380 ui-state-default ui-corner-all"
                href="#">Projects By Sponsor </a><a id="button-projectDescriptionsReport" class="fg-button w380 ui-state-default ui-corner-all"
                    href="#">Project Descriptions</a> <a id="button-projectAmendmentReport" class="fg-button w380 ui-state-default ui-corner-all"
                        href="#">Project Amendment</a>
            <h2>
                ALOP Reports</h2>
            <a id="button-alopReport" class="fg-button w380 ui-state-default ui-corner-all" href="#">
                Run Alop Report</a>
        </div>
        <div class="ui-widget rightColumn">
            <h2>
                Appendix A</h2>
            <a id="button-appendixA_ProjectsByCity" class="fg-button w380 ui-state-default ui-corner-all"
                href="#">Appendix A - Projects By City</a> <a id="button-appendixA_ProjectsByCounty"
                    class="fg-button w380 ui-state-default ui-corner-all" href="#">Appendix A - Projects
                    By County</a> <a id="button-appendixA_ProjectsByProjectType" class="fg-button w380 ui-state-default ui-corner-all"
                        href="#">Appendix A - Projects By Project Type</a>
            <h2>
                Table 2</h2>
            <a id="button-projectsFundingBySource" class="fg-button w380 ui-state-default ui-corner-all"
                href="#">Table 2 - Projects Funding By Source</a>
        </div>
    </div>
    <div class="clear">
    </div>
    <div id="amendmentHold-admin" class="box fixedHold" style="display: none;">
        <span>Exclude from report</span>
        <ul id="sortableAmendmentHold-admin" class="connectedSortable">
            <%foreach (DRCOG.Domain.Models.TIPProject.TipReportProject item in Model.ReportDetails.Where(x => x.Name == "CurrentAdmin").SelectMany(x => x.projects.Where(y => (y.AmendmentTypeId == (int)DRCOG.Domain.Enums.AmendmentType.Administrative && y.IsOnHold))))
              { %>
            <li id="pv_<%= item.ProjectVersionId.ToString() %>" class="ui-state-highlight">
                <%= item.TipId.ToString() %>
                :
                <%= item.COGID.ToString() %><br />
                <span class="pv_projectname" style="margin-right: 5px;">
                    <%= item.ProjectName.ToString() %></span></li>
            <% } %>
        </ul>
    </div>
    <div id="amendmentHold-policy" class="box fixedHold" style="display: none;">
        <span>Exclude from report</span>
        <ul id="sortableAmendmentHold-policy" class="connectedSortable">
            <%foreach (DRCOG.Domain.Models.TIPProject.TipReportProject item in Model.ReportDetails.Where(x => x.Name == "CurrentPolicy").SelectMany(x => x.projects.Where(y => (y.AmendmentTypeId == (int)DRCOG.Domain.Enums.AmendmentType.Policy && y.IsOnHold))))
              { %>
            <li id="pv_<%= item.ProjectVersionId.ToString() %>" class="ui-state-highlight">
                <%= item.TipId.ToString() %>
                :
                <%= item.COGID.ToString() %><br />
                <span class="pv_projectname" style="margin-right: 5px;">
                    <%= item.ProjectName.ToString() %></span></li>
            <% } %>
        </ul>
    </div>
    <div style='display: none'>
        <div id="dialog-report" class="dialog" title="Restore project from TIPYear ...">
            <h2>
                Report: Projects By Sponsor</h2>
            <div class="error" style="display: none;">
                <span></span>.
            </div>
            <div>
                <label for="tipYear">
                    For TIP Year:</label>
                <input type="text" id="tipYear" name="tipYear" class="big" readonly="true" value="<%=Model.TipSummary.TipYear.ToString()%>" />
                <input type="hidden" id="TipYearId" value="<%=Model.TipSummary.TipYearTimePeriodID.ToString() %>" />
            </div>
            <div>
                <%= Html.DropDownList("SponsorAgencyID", new SelectList(Model.CurrentSponsors, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </div>
            <% Html.RenderPartial("DocumentFormatSelection", "report-projectbysponsor-format"); %>
            <button id="report-projectbysponsor" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-projectDescriptionsReport" class="dialog">
            <h2>
                Report: Project Descriptions</h2>
            <% Html.RenderPartial("DocumentFormatSelection", "report-projectDescriptionsReport-format"); %>
            <button id="report-projectDescriptionsReport" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-appendixA_ProjectsByCity" class="dialog">
            <h2>
                Report: Projects By City</h2>
            <% Html.RenderPartial("DocumentFormatSelection", "report-appendixA_ProjectsByCity-format"); %>
            <button id="report-appendixA_ProjectsByCity" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-appendixA_ProjectsByCounty" class="dialog">
            <h2>
                Report: Projects By County</h2>
            <% Html.RenderPartial("DocumentFormatSelection", "report-appendixA_ProjectsByCounty-format"); %>
            <button id="report-appendixA_ProjectsByCounty" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-appendixA_ProjectsByProjectType" class="dialog">
            <h2>
                Report: Projects By Project Type</h2>
            <% Html.RenderPartial("DocumentFormatSelection", "report-appendixA_ProjectsByProjectType-format"); %>
            <button id="report-appendixA_ProjectsByProjectType" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-projectsFundingBySource" class="dialog">
            <h2>
                Report: Funding By Source</h2>
            <% Html.RenderPartial("DocumentFormatSelection", "report-projectsFundingBySource-format"); %>
            <button id="report-projectsFundingBySource" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-alopReport" class="dialog" title="Report for Alop">
            <h2>
                Report: Alop</h2>
            <div class="error" style="display: none;">
                <span></span>.
            </div>
            <div>
                <label for="reportYear" class="big">
                    Select the report to run:</label>
                <input type="hidden" id="referrerYear" value="<%=Model.TipSummary.TipYear %>" />
                <%= Html.DropDownList("reportId", new SelectList(Model.AvailableAlopReport, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </div>
            <% Html.RenderPartial("DocumentFormatSelection", "report-alopReport-format"); %>
            <button type="submit" id="report-alopReport" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                Run the Report</button>
        </div>
        <div id="dialog-report-Amendment" class="dialog" title="Print Amendments by Date">
            <h2>
                Report: Project Amendment</h2>
            <div class="error" style="display: none;">
                <span></span>.
            </div>
            <div id="amendmentReportContainer">
                    <div>
                        <button type="submit" id="btn-dialog-report-Amendment-admin" name="<%= Model.ReportDetails.Where(x => x.Name == "CurrentAdmin").Select(x => x.Id).FirstOrDefault() %>"
                            class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                            Current Admin</button>
                        <%if (Model.ReportDetails.HasCurrentPolicy())
                          { %>
                        <button type="submit" id="btn-dialog-report-Amendment-policy" name="<%= Model.ReportDetails.Where(x => x.Name == "CurrentPolicy").FirstOrDefault().Id %>"
                            class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all">
                            Current Policy</button>
                        <% } %>
                    </div>
                    <%--<p>
                    <label for="report-amendment-date" class="big">For Amendment Date:</label>
                    <%= Html.DropDownList("report-amendment-date", new SelectList(Model.AvailableAmendmentDates, "Key", "Value"), new { @class = "mediumInputElement big" })%>
                    <input type="hidden" id="report-amendment-yearid" value="<%=Model.TipSummary.TipYearTimePeriodID.ToString() %>" />
                </p>--%>
                    <%--<button type="submit" id="btn-report-amendment" class="fg-button ui-state-default big ui-priority-primarystate-enabled ui-corner-all" >Run the Report</button>--%>
            </div>
            <%if (Model.ReportDetails.HasCurrentPolicy())
              { %>
            <div id="amendmentSortContainer-policy" name="<%= Model.ReportDetails.Where(x => x.Name == "CurrentPolicy").First().Id %>"
                style="display: none;">
                <div id="amendmentSort-policy">
                    Projects:
                    <ul id="sortableAmendmentProjects-policy" class="connectedSortable">
                        <%foreach (DRCOG.Domain.Models.TIPProject.TipReportProject item in Model.ReportDetails.Where(x => x.Name == "CurrentPolicy").SelectMany(x => x.projects.Where(y => (y.AmendmentTypeId == (int)DRCOG.Domain.Enums.AmendmentType.Policy && !y.IsOnHold))))
                          { %>
                        <li id="pv_<%= item.ProjectVersionId.ToString() %>" class="ui-state-highlight">
                            <%= item.TipId.ToString() %>
                            :
                            <%= item.SponsorAgency.ToString() %><br />
                            <span class="pv_projectname" style="margin-right: 5px;">
                                <%= item.ProjectName.ToString() %></span></li>
                        <% } %>
                    </ul>
                </div>
            </div>
            <% } %>
            <div id="amendmentSortContainer-admin" name="<%= Model.ReportDetails.Where(x => x.Name == "CurrentAdmin").Select(x => x.Id).FirstOrDefault() %>"
                style="display: none;">
                <div id="amendmentSort-admin">
                    Projects:
                    <ul id="sortableAmendmentProjects-admin" class="connectedSortable">
                        <%
                            foreach (DRCOG.Domain.Models.TIPProject.TipReportProject item in Model.ReportDetails.Where(x => x.Name == "CurrentAdmin").SelectMany(x => x.projects.Where(y => (y.AmendmentTypeId == (int)DRCOG.Domain.Enums.AmendmentType.Administrative && !y.IsOnHold))))
                            { %>
                        <li id="pv_<%= item.ProjectVersionId.ToString() %>" class="ui-state-highlight">
                            <%= item.TipId.ToString() %>
                            :
                            <%= item.SponsorAgency.ToString() %><br />
                            <span class="pv_projectname" style="margin-right: 5px;">
                                <%= item.ProjectName.ToString() %></span></li>
                        <% } %>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var updateSortUrl = '<%=Url.Action("UpdateReportProjectVersionSort","TIP") %>';
        var setProjectVersionOnHoldUrl = '<%=Url.Action("SetReportProjectVersionOnHold","TIP") %>';

        $().ready(function () {
            //hook up all the click events for the buttons
            $('#proposed-list').colorbox({ width: "900px", height: "500px", iframe: true });
        });

        var floatHolding = function (ev) {
            var cbox = $("#cboxContent"); //$("#amendmentHold-admin");
            var pos = $(cbox).offset();
            var width = $(cbox).width();

            $("#amendmentHold-admin").css({ "left": (pos.left + width) + "px", "top": pos.top + "px" });
            $("#amendmentHold-policy").css({ "left": (pos.left + width) + "px", "top": pos.top + "px" });
        }

        $(function () {
            $("#sortableAmendmentProjects-policy, #sortableAmendmentProjects-admin").sortable({
                connectWith: '.connectedSortable',
                placeholder: 'ui-state-highlight',
                items: 'li:not(.ui-state-disabled)',
                receive: function (event, ui) {
                    //addCycle($(ui.item).attr("id").replace("cycle_", ""));
                },
                remove: function (event, ui) {
                    setProjectVersionOnHold($(ui.item).attr("id").replace("pv_", ""), $(this).parent().parent().attr("name"));
                    //removeCycle($(ui.item).attr("id").replace("cycle_", ""));
                },
                update: function () {
                    $("#sortableAmendmentProjects-policy, #sortableAmendmentProjects-admin").sortable("refresh");
                    //alert();
                    parent.$.fn.colorbox.resize();
                    updateSort($(this));
                }
            })

            $("#sortableAmendmentHold-policy, #sortableAmendmentHold-admin").sortable({
                connectWith: '.connectedSortable'
                ,
                placeholder: 'ui-state-highlight'
            })

            $("#sortableAmendmentProjects-policy li, #sortableAmendmentHold-policy li, #sortableAmendmentProjects-admin li, #sortableAmendmentHold-admin li").disableSelection();
        });

        function updateSort(element) {
            var values = $(element).sortable({ items: 'li' }).sortable('toArray');
            var reportId = $(element).parent().parent().attr("name");
            if (values.length > 0) {
                $.ajax({
                    type: "POST",
                    url: updateSortUrl,
                    data: "projects=" + values
                    + "&reportId=" + reportId,
                    dataType: "json",
                    success: function (response, textStatus, XMLHttpRequest) {
                        if (response.error == "false") {
                            //$('div#resultRecordDetail').html(response.message);
                            //$('div#resultRecordDetail').addClass('success');
                            //autoHide(2500);
                        }
                        else {
                            //$("#sortable2").sortable('cancel');
                            alert("Cycle error");
                        }
                    },
                    error: function (response, textStatus, AjaxException) {
                        alert("BIG error " + response.statusText);
                        //$('div.dialog-result').html(response.statusText);
                        //$('div.dialog-result').addClass('error').show();
                        //autoHide(10000);
                    }
                });
            }
            return false;
        }

        function setProjectVersionOnHold(id, reportId) {
            $.ajax({
                type: "POST",
                url: setProjectVersionOnHoldUrl,
                data: "projectVersionId=" + id
                    + "&reportId=" + reportId,
                dataType: "json",
                success: function (response, textStatus, XMLHttpRequest) {
                    if (response.error == "false") {
                        //$('div#resultRecordDetail').html(response.message);
                        //$('div#resultRecordDetail').addClass('success');
                        //autoHide(2500);
                    }
                    else {
                        //$("#sortable2").sortable('cancel');
                        alert("Sort error");
                    }
                },
                error: function (response, textStatus, AjaxException) {
                    alert("BIG error " + response.statusText);
                    //$('div.dialog-result').html(response.statusText);
                    //$('div.dialog-result').addClass('error').show();
                    //autoHide(10000);
                }
            });
            return false;
        }


        $("#btn-dialog-report-Amendment-admin").live("click", function (e) {
            e.preventDefault();
            printReport($(this).attr("name"));
        });


        var showRestoreCandidatesUrl = '<%=Url.Action("RestoreProjectList","TIP")%>"';

        $(function () {
            $("#report-projectbysponsor").click(function () {
                var reportUrl,
                    format = $("input[name=report-projectbysponsor-format]:checked").val(),
                    tipYearId = $("#TipYearId").val(),
                    sponsorId = $("#SponsorAgencyID").val();
                // reset values
                $("#SponsorAgencyID").val(0);
                if (sponsorId > 0) {
                    reportUrl = "http://sqlprod/reportserver?/TransportationReports/TIP.ProjectsBySponsorByTIPYear&rs:Command=Render&rc:Parameters=false&tipyear=" + tipYearId + "&sponsors=" + sponsorId + "&rs:Format=" + format;
                    location.assign(reportUrl);
                    $.fn.colorbox.close();
                }
            });

            $("#button-projectsbysponsor").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-report",
                onLoad: function () {
                    $.getJSON('<%= Url.Action("GetCurrentTimePeriodSponsorAgencies")%>/', { year: "<%= Model.TipSummary.TipYear.ToString() %>" }, function (data) {
                        $('#SponsorAgencyID').fillSelect(data);
                    });
                }
            });
        });

        $(function () {
            $("#report-projectDescriptionsReport").click(function () {
                var reportUrl,
                    format = $("input[name=report-projectDescriptionsReport-format]:checked").val();
                reportUrl = "http://sqlprod/reportserver?/TransportationReports/TIP.ProjectDescriptionsReport&rs:Command=Render&rc:Parameters=false&TipYearID=<%= Model.TipSummary.TipYearTimePeriodID %>" + "&rs:Format=" + format;
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-projectDescriptionsReport").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-projectDescriptionsReport"
            });
        });

        $(function () {
            $("#report-appendixA_ProjectsByCity").click(function () {
                var reportUrl,
                    format = $("input[name=report-appendixA_ProjectsByCity-format]:checked").val();
                reportUrl = "http://sqlprod/reportserver?/TransportationReports/TIP.ProjectsByCity.AppendixA&rs:Command=Render&rc:Parameters=false&TipYearID=<%= Model.TipSummary.TipYearTimePeriodID %>" + "&rs:Format=" + format;
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-appendixA_ProjectsByCity").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-appendixA_ProjectsByCity"
            });
        });

        $(function () {
            $("#report-appendixA_ProjectsByCounty").click(function () {
                var reportUrl,
                    format = $("input[name=report-appendixA_ProjectsByCounty-format]:checked").val();
                reportUrl = "http://sqlprod/reportserver?/TransportationReports/TIP.ProjectsByCounty.AppendixA&rs:Command=Render&rc:Parameters=false&TipYearID=<%= Model.TipSummary.TipYearTimePeriodID %>" + "&rs:Format=" + format;
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-appendixA_ProjectsByCounty").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-appendixA_ProjectsByCounty"
            });
        });

        $(function () {
            $("#report-appendixA_ProjectsByProjectType").click(function () {
                var reportUrl,
                    format = $("input[name=report-appendixA_ProjectsByProjectType-format]:checked").val();
                reportUrl = "http://sqlprod/reportserver?/TransportationReports/TIP.ProjectsByProjectType.AppendixA&rs:Command=Render&rc:Parameters=false&TipYearID=<%= Model.TipSummary.TipYearTimePeriodID %>" + "&rs:Format=" + format;
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-appendixA_ProjectsByProjectType").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-appendixA_ProjectsByProjectType"
            });
        });

        $(function () {
            $("#report-projectsFundingBySource").click(function () {
                var reportUrl,
                    format = $("input[name=report-projectsFundingBySource-format]:checked").val();
                reportUrl = "http://sqlprod/reportserver?/TransportationReports/TIP.ProjectsFundingBySource.Table2&rs:Command=Render&rc:Parameters=false&TipYearID=<%= Model.TipSummary.TipYearTimePeriodID %>" + "&rs:Format=" + format;
                location.assign(reportUrl);
                $.fn.colorbox.close();
            });

            $("#button-projectsFundingBySource").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-projectsFundingBySource"
            });
        });

        $(function () {
            $("#report-alopReport").click(function () {
                var reportUrl = null,
                    reportFolder = "TransportationReports/ALOP.ProjectList",
                    referrerYear = $("#referrerYear").val(),
                    reportId = $("#reportId").val(),
                    format = $("input[name=report-alopReport-format]:checked").val();

                if (format === "Word") {
                    reportUrl = '<%= Url.Action("RenderAlopReport", "Tip", new { @reportShortGuid = "r_reportId", @reportFolder = "r_reportFolder", @reportFormat = "r_reportFormat" }) %>';
                    reportUrl = reportUrl.replace("r_reportId", reportId).replace("r_reportFolder", reportFolder).replace("r_reportFormat", format);
                } else if (format === "Excel") {
                    reportUrl = '<%= Url.Action("DownloadReportList", "Tip", new { @reportId = "r_reportId", @referrerYear = "r_referrerYear" }) %>';
                    reportUrl = reportUrl.replace("r_reportId", reportId).replace("r_referrerYear", referrerYear);
                } else {
                    alert(format + " is not currently supported for this report.");
                }

                if (reportUrl) {
                    location.assign(reportUrl);
                }
                $.fn.colorbox.close();
            });

            // PDF not supported.
            // Hide PDF button and set excel as default.
            $("input[name=report-alopReport-format][value=PDF]").hide().closest("label").hide();
            $("input[name=report-alopReport-format][value=Excel]").prop("checked", true);
            $("#button-alopReport").colorbox({
                width: "500px",
                inline: true,
                href: "#dialog-alopReport"
            });

        });


        $(function () {
            $("#button-projectAmendmentReport").colorbox({
                width: "800px",
                height: "200px",
                inline: true,
                href: "#dialog-report-Amendment",
                onComplete: function () {
                    var hasPolicy = "<%= Model.ReportDetails.HasCurrentPolicy() %>".toLowerCase();
                    var btnPolicy = '<span id="btn-report-amendment-sort-policy" class="cboxBtn">Policy</span>';
                    var $buttonRestoreProjects = $('<span id="btn-report-amendment" name="" style="display:none;" class="cboxBtn">Print</span>').appendTo('#cboxContent');
                    var $buttonRestoreProjects3 = $('<span id="btn-report-amendment-sort-hide" style="display: none;" class="cboxBtn">Hide</span>').appendTo('#cboxContent');
                    var $labelDivider = $('<span id="dialog-report-Amendment-d1" style="display:none;" class="cboxLabel cboxDivider">|</span>').appendTo('#cboxContent');
                    var $labelSort = $('<span style="padding-right: 0px;" class="cboxLabel">Sort:</span>').appendTo('#cboxContent');

                    var $buttonRestoreProjects1 = $('<span id="btn-report-amendment-sort-admin" style="" class="cboxBtn">Admin</span>').appendTo('#cboxContent');

                    if (hasPolicy === 'true') {
                        $(btnPolicy).appendTo('#cboxContent');
                    }

                    $("#btn-report-amendment-sort-policy").css("display", (hasPolicy ? "inline" : "none"));
                    var btnPrint = $("#btn-report-amendment");

                    $("#btn-report-amendment").live("click", function (e) {
                        e.preventDefault();
                        var reportId = $(this).attr("name");
                        if (reportId != "") {
                            printReport(reportId);
                            $.fn.colorbox.close();
                        }
                        //alert("Danny's nifty report would be opening now!");
                        return false;
                    });

                    $("#btn-report-amendment-sort-admin").live("click", function (e) {
                        e.preventDefault();
                        $("#dialog-report-Amendment-d1").show();
                        $("#amendmentReportContainer").hide();
                        $("#amendmentSortContainer-admin").show();
                        $("#btn-report-amendment").show();
                        $("#amendmentHold-admin").show();
                        $("#amendmentHold-policy").hide();
                        $(btnPrint).attr("name", $("#amendmentSortContainer-admin").attr("name"));
                        //alert(btnPrint.attr("name"));
                        parent.$.fn.colorbox.resize();
                        floatHolding;

                        $("#btn-report-amendment-sort").hide();
                        $("#btn-report-amendment-sort-hide").show();
                        $("#amendmentSortContainer-policy").hide();
                    });

                    $("#btn-report-amendment-sort-policy").live("click", function (e) {
                        e.preventDefault();
                        $("#amendmentReportContainer").hide();
                        $("#amendmentSortContainer-policy").show();
                        $("#amendmentSortContainer-admin").hide();
                        $("#btn-report-amendment").show();
                        $("#amendmentHold-policy").show();
                        $("#amendmentHold-admin").hide();

                        $(btnPrint).attr("name", $("#amendmentSortContainer-policy").attr("name"));
                        //alert(btnPrint.attr("name"));
                        parent.$.fn.colorbox.resize();

                        $("#btn-report-amendment-sort").hide();
                        $("#btn-report-amendment-sort-hide").show();
                    });

                    $("#btn-report-amendment-sort-hide").live("click", function () {
                        $("#amendmentReportContainer").show();
                        $("#amendmentSortContainer-admin").hide();
                        $("#amendmentSortContainer-policy").hide();
                        $('#button-projectAmendmentReport').colorbox.resize();

                        $("#btn-report-amendment").hide();

                        $("#btn-report-amendment-sort").show();
                        $("#btn-report-amendment-sort-hide").hide();

                        $("#dialog-report-Amendment-d1").hide();
                        $("#amendmentHold-admin").hide();
                        $("#amendmentHold-policy").hide();

                    });

                },
                onClosed: function () {
                    $("#amendmentReportContainer").show();
                    $("#amendmentSortContainer-admin").hide();
                    $("#amendmentSortContainer-policy").hide();
                    $("#amendmentHold-admin").hide();

                    $("#btn-report-amendment").remove();
                    //$("#btn-report-amendment-sort-policy").remove();
                    //$("#btn-report-amendment-sort-admin").remove();
                    //$("#btn-report-amendment-sort-hide").remove();

                    $("#cboxContent .cboxBtn").remove();
                    $("#cboxContent .cboxLabel").remove();

                    $("#amendmentHold-policy").hide();
                    $("#amendmentHold-admin").hide();

                }

            });

        });

        function printReport(id) {
            var redirectUrl = "http://sqlprod/reportserver?/TransportationReports/TIP.ProjectAmendmentReport&rs:Command=Render&rs:Format=WORD&rc:Parameters=false&ReportID=" + id;
            location = redirectUrl;
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
