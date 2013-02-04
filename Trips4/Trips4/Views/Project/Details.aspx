<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.DetailViewModel>" %>

<%@ Import Namespace="MvcContrib.UI.Grid" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Project Detailed Home</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Url.Content("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <link href="<%= Url.Content("~/Content/tip.description.print.css") %>" rel="stylesheet"
        type="text/css" media="print" />
    <script src="<%= Url.Content("~/scripts/Slide.js")%>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(App.tabs.initializeTipProjectTabs);
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="tab-content-container">
        <% Html.RenderPartial("~/Views/Project/Partials/TipProjectTabPartial.ascx", Model.ProjectSummary); %>
        <div id="details">
            <fieldset>
                <legend>
                    <%=Model.ProjectSummary.ProjectName%><%-- - <a href="#" class="printtest" title="Print Test">PRINT</a>--%></legend>
                <% if (ViewData["message"] != null)
                   { %>
                <div id="message" class="info">
                    <%= ViewData["message"].ToString() %></div>
                <% } %>
                <ul id="status-summary">
                    <li><span class="darkGrey boldFont">Begin Construction:</span>
                        <%= !Model.GeneralInfo["BeginConstructionYear"].Equals(String.Empty) ? Model.GeneralInfo["BeginConstructionYear"] : "none" %>
                    </li>
                    <li><span class="darkGrey boldFont">Open to Public:</span> <span id="openToPublic">
                        <%= !Model.GeneralInfo["EndConstructionYear"].Equals(String.Empty) ? Model.GeneralInfo["EndConstructionYear"] : "none" %></span>
                    </li>
                    <li><span class="darkGrey boldFont">Project Status:</span> <span id="projectStatus">
                        <%= Model.GeneralInfo["ProjectStatus"]%></span></li>
                    <li><span class="darkGrey boldFont">Amendment Status:</span> <span id="amendmentStatus">
                        <%= Model.GeneralInfo["AmendmentStatus"] %></span></li>
                </ul>
                <div class="printarea">
                    <div>
                        <span class="floatLeft">Title: <span class="boldFont">
                            <%=Model.ProjectSummary.ProjectName%></span></span> <span class="floatRight">Project
                                Type: <span class="boldFont">
                                    <%= Model.GeneralInfo["ProjectType"]%></span></span>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="summary-snapshot">
                        <div>
                            TIP-ID: <span class="boldFont">
                                <%= Model.InfoModel.TipId %></span></div>
                        <div>
                            STIP-ID: <span class="boldFont">
                                <%= Model.InfoModel.STIPID %></span></div>
                        <div>
                            Open to Public: <span class="boldFont">
                                <%= !Model.GeneralInfo["EndConstructionYear"].Equals(String.Empty) ? Model.GeneralInfo["EndConstructionYear"] : "none" %></span></div>
                        <div>
                            Sponsor: <span class="boldFont">
                                <%= Model.ProjectSponsorsModel.GetCurrent1Agency()[0].OrganizationName %></span></div>
                    </div>
                    <div id="scope" class="box">
                        <div id="scope-info">
                            <h3>
                                Project Scope</h3>
                            <p>
                                <%= Model.GeneralInfo["Scope"] %></p>
                        </div>
                        <div id="location-image">
                            <img src='<%= Url.Action("ShowPhoto", "Project", new { id = Model.InfoModel.LocationMapId, @maxWidth = "300" }) %>'
                                id="image_<%= Model.InfoModel.LocationMapId %>" class="resize" alt="Image map for <%= Model.InfoModel.ProjectName %>" />
                        </div>
                        <div class="clear">
                        </div>
                        <div id="scope-affected">
                            <div id="scope-muni">
                                <span class="boldFont">Affected Municipality(ies)</span>
                                <ul>
                                    <%foreach (DRCOG.Domain.Models.MunicipalityShareModel muni in Model.MuniShares)
                                      { %>
                                    <li>
                                        <%=muni.MunicipalityName%>
                                        <%= Convert.ToInt32(muni.Share.Value).ToString() %>%</li>
                                    <% } %>
                                </ul>
                            </div>
                            <div id="scope-county">
                                <span class="boldFont">Affected County(ies)</span>
                                <ul>
                                    <%foreach (DRCOG.Domain.Models.CountyShareModel cty in Model.CountyShares)
                                      { %>
                                    <li>
                                        <%=cty.CountyName %>
                                        <%= Convert.ToInt32(cty.Share.Value).ToString() %>%</li>
                                    <% } %>
                                </ul>
                            </div>
                            <div class="clear">
                            </div>
                        </div>
                    </div>
                    <table id="funding-summary">
                        <tr>
                            <th style="width: 150px;">
                                Amounts in $1,000s
                            </th>
                            <th style="width: 80px;">
                                Prior Funding
                            </th>
                            <%--<th style="width:100px;">Funding Level</th>--%>
                            <% foreach (var increment in Model.FundingDetailPivotModel.FundingIncrements)
                               { %>
                            <th style="width: 75px;">
                                <%= increment.FundingIncrementName.ToString() %>
                            </th>
                            <% } %>
                            <th style="width: 80px;">
                                Future Funding
                            </th>
                            <th style="width: 80px;">
                                Total Funding
                            </th>
                        </tr>
                        <% foreach (System.Data.DataRow item in Model.FundingDetailPivotModel.FundingDetailTable.Rows)
                           {
                               if ((bool)item[8])
                               { %>
                        <tr id="fundingdetail_row_<%= item[1].ToString() + '_' + item[3].ToString() %>">
                            <td id="fundingdetail_<%= item[1].ToString() + '_' + item[3].ToString() %>_FundingType">
                                <%= item[4].ToString() %>
                                (<%= item[7].ToString() %>)
                            </td>
                            <%--<td id="fundingdetail_<%= item[1].ToString() + '_' + item[3].ToString() %>_FundingLevel"><%= item[4].ToString() %></td>--%>
                            <td>
                            </td>
                            <% for (int j = 9; j < item.Table.Columns.Count; j++)
                               { %>
                            <td>
                                $<%=item[j].ToString() %>
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
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <% } %>
                        <tr>
                            <td>
                                Total
                            </td>
                            <td>
                                $<%= Model.TipProjectFunding.Previous %>
                            </td>
                            <%  int c = 3;
                                foreach (var increment in Model.FundingDetailPivotModel.FundingIncrements)
                                {
                            %>
                            <td>
                                <div id="funding-summary-<%= c.ToString() %>">
                                </div>
                            </td>
                            <% c++;
                    } %>
                            <td>
                                $<%= Model.TipProjectFunding.Future %>
                            </td>
                            <td>
                                <div id="projectCostTotalLabel">
                                    0</div>
                            </td>
                        </tr>
                    </table>
                    <div id="poolwrapper">
                        <h2>
                            Pool Projects</h2>
                        <% if (Model.PoolProjects.Count == 0)
                           { %>
                        There are not any pool projects for
                        <%= Model.ProjectSummary.ProjectName %>
                        <% }
                           else
                           { %>
                        All pool project funding depicts federal and/or state funding only
                        <% } %>
                        <br />
                        <% if (Model.PoolProjects.Count > 0)
                           { %>
                        <%= Html.Grid(Model.PoolProjects).Columns(column =>
        {
            column.For(x => x.ProjectName).Named("Project Name");
            column.For(x => x.Description).Named("Description");
            column.For(x => x.BeginAt).Named("Begin").Attributes(style => "width: 100px;");
            column.For(x => x.EndAt).Named("End").Attributes(style => "width: 100px;");
            column.For(x => x.Cost).Named("Cost").Format("{0:c0}").Attributes(style => "width: 100px;");
        }).Attributes(id => "pool-project", style => "width: 100%;")%>
                        <% } %>
                    </div>
                    <div id="amendmentwrapper">
                        <h2>
                            Amendment History</h2>
                        <%= Html.Grid(Model.AmendmentList).Columns(column =>
        {
            column.For(x => x.AmendmentDate.Equals(DateTime.MinValue) ? String.Empty : x.AmendmentDate.ToShortDateString()).Named("Date");
            column.For(x => x.AmendmentStatus).Named("Status");
            column.For(x => x.AmendmentCharacter).Named("Description");
        }).Attributes(id => "projectListGrid")%>
                    </div>
                </div>
            </fieldset>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            var printarea = $("#details div.printarea").html();
            $("#printarea").html(printarea);
            $(".printtest").bind("click", function () {
                ClickHereToPrintTest();
            });




            /*
            $(".printtest").colorbox({
            width: "900px",
            height: "699px",
            iframe: false,
            open: false,
            overlayClose: true,
            opacity: .5,
            initialWidth: "300px",
            initialHeight: "100px",
            transition: "elastic",
            speed: 350,
            close: "Close",
            photo: false,
            inline: true,
            href: "#test_report",
            onLoad: function () {
            var printarea = $("#details div.printarea").html();
            $("#printarea").html(printarea);
            },
            onComplete: function () {
            var buttonCompletedSave = $('<span id="btn-print" class="cboxBtn">Print</span>').appendTo('#cboxContent');
            $("#btn-print").bind("click", function () {
            ClickHereToPrintTest();
            });
            },
            onCleanup: function () { $("#btn-print").remove(); $("#btn-print").unbind(); }
            });
            */
        });
    </script>
    <div style='display: none'>
        <div id='test_report'>
            <script type="text/javascript">
                //<!--

                function ClickHereToPrintTest() {
                    try {
                        var oIframe = document.getElementById('ifrmPrintTest');
                        var oContent = document.getElementById('printarea').innerHTML;
                        var oDoc = (oIframe.contentWindow || oIframe.contentDocument);
                        if (oDoc.document) oDoc = oDoc.document;
                        oDoc.write("<html><head><title></title>");
                        oDoc.write("<link href='" + '<%= ResolveUrl("~/Content/tip.description.print.css") %>' + "' rel='stylesheet' type='text/css' />");
                        oDoc.write("</head></body><body onload='this.focus(); this.print();'>");
                        oDoc.write(oContent + "</body></html>");
                        oDoc.close();
                    }
                    catch (e) {
                        self.print();
                    }
                }

                //-->
            </script>
            <iframe id='ifrmPrintTest' src='#' style="width: 0pt; height: 0pt; border: none;">
            </iframe>
            <div id="printarea">
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
    <script type="text/javascript">

    var $projectStatus = $("#projectStatus");
    var $amendmentStatus = $("#amendmentStatus");
    if ($projectStatus.text() == "Active") { $projectStatus.css("color", "Green"); } else { $projectStatus.css("color", "Red") };
    
    $('#projectCostTotalLabel').html('$' + UpdateProjectTotals());

    $('div[id^=funding-summary-]').each(function () {
        //alert($(this).attr('id'));
        var count = $(this).attr('id').replace("funding-summary-", "");
        //alert(sumOfColumns("funding-summary", count, true));
        $(this).html("$" + sumOfColumns("funding-summary", count, true));
    });
    //$('#funding-summary-3').html(sumOfColumns("funding-summary", 3, true));
    
    function UpdateProjectTotals() {
        var sum = 0;
        var previous = parseInt('<%= Model.TipProjectFunding.Previous %>');
        var programmed = parseInt(UpdateProgrammedTotals());
        var outyear = parseInt(UpdateOutyearTotals());
        var future = parseInt('<%= Model.TipProjectFunding.Future %>');
        //alert(previous + ', ' + programmed + ', ' + outyear + ', ' + future);
        sum = previous + programmed + outyear + future;
        return sum;
    }

    function UpdateOutyearTotals() {
        //Total up all values that start with "OutyearTotal"
        var sum = 0;
        $("input[id*='hdnOutyearTotal']").each(function() {
            //add only if the value is number
            if (!isNaN(this.value) && this.value.length != 0) {
                sum += parseFloat(this.value);
            }
        });
        return sum;
    }

    function UpdateProgrammedTotals() {
        //Total up all values that start with "ProgrammedTotal"
        var sum = 0;
        $("input[id*='hdnProgrammedTotal']").each(function() {
            //add only if the value is number
            if(!isNaN(this.value) && this.value.length!=0) {
                sum += parseFloat(this.value);
            }
        });
        return sum;
    }

    function sumOfColumns(tableID, columnIndex, hasHeader) {
      var tot = 0;
      $("#" + tableID + " tr" + (hasHeader ? ":gt(0)" : ""))
      .children("td:nth-child(" + columnIndex + ")")
      .each(function() {
        var val = $(this).html().replace("$","");
        if (!isNaN(val)) {
            tot += parseInt($(this).html().replace("$",""));
        }
      });
      return tot;
    }

    </script>
</asp:Content>
