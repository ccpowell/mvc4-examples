<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.Survey.Survey>" %>
<%@ Import Namespace="MvcContrib.UI.Grid"%>
<%@ Import Namespace="MvcContrib.UI.Grid.ActionSyntax" %>

<ul id="tabnav">
    <%if (!Html.IsActionCurrent("Index") && Model.IsAdmin()) { %><li class="notab tab-w-image"><a href="<%=Url.Action("Index",new {controller="Survey"}) %>"><% var url = Html.ResolveUrl("~/content/marker/16/previous.png"); %><%=Html.Image(url.ToString(), "Survey List", null) %>Surveys</a></li> <% } %>
    <%if (Html.IsActionCurrent("Index") && Model.IsAdmin()) { %><li><a href="<%= Url.Action("Status", "RTP", new { year = 0 }) %>">Create a new Survey through the current RTP</a></li> <% } %>
    <%if ((Model.IsAdmin() || !Request.IsAuthenticated) && Model.Id != default(int)) { %><li ><a <%=Html.IsActionCurrent("Dashboard") ? "class='activetab'" : "" %> href="<%=Url.Action("Dashboard","Survey",new {year=Model.Name}) %>"><%= Model.Name %> Breakdown</a></li><% } %>
	<% if (Model.IsAdmin()&& Model.Id != default(int)) { %>
	    <li><a <%=Html.IsActionCurrent("Agencies") ? "class='activetab'" : "" %> href="<%=Url.Action("Agencies","Survey", new {year=Model.Name}) %>">Sponsors</a></li>
	<% } %>
    <%--<li ><a <%=Html.IsActionCurrent("ProjectSearch") ? "class='activetab'" : "" %> href="<%=Url.Action("ProjectSearch","Survey", new {year=Model.Name}) %>">Project Search</a></li>--%>
	<%if (Model.IsEditable() && ((Model.SponsorsOrganization != null && !Model.SponsorsOrganization.OrganizationId.Equals(default(int))) || Model.IsAdmin()) && !Model.Id.Equals(default(int))) { %>
        <li>
            <a id="button-create-project" href="#" class="">
                Add Project<% if (Model.SponsorsOrganization != null && !Model.SponsorsOrganization.OrganizationId.Equals(default(int))) { %> for <%= Model.SponsorsOrganization.OrganizationName.ToString()%><% } %>
            </a>
        </li>
    <% } %>
    <%if (Model.IsAdmin() && !Model.Id.Equals(default(int))) { %><li><a id="btn-includemore" href="#" class="" title="Include more Projects">Include More</a></li><% } %>
    
    <% if (Model.AgencyProjectList != null) { %>
        <% if (Model.AgencyProjectList.Count > 0) { %>
            <%if (Model.ShowCertification){ %><li><a href="#" class="pricing" title="Print Certificaton Form">Print <%= Model.AgencyProjectList.FirstOrDefault().SponsorName.ToString()%> Certification</a></li><% } %>
        <% } %>
    <% } %>
    <%if (Model.IsAdmin() && Model.Id != default(int)) { %><li ><a href="#" id="survey-overview" title="Survey Overview">Survey Overview</a></li><% } %>
    <%if (Model.IsAdmin() && Model.Id != default(int)) { %><li ><a href="#" id="report-modelerextract-xls">Modeler Extract</a></li><% } %>
</ul>

<%--<% if (Request.IsAuthenticated) { %>
    <span style="position: absolute; top: 0px; right: 30px;"><a id="isSponsorlink" href="#">Do you have a sponsor code to enter?</a></span>
<% } %>--%>

<script type="text/javascript">
var GetAmendableProjects = App.env.applicationPath + '/Operation/Misc/SurveyGetAmendableProjects'; 
var AmendProjects = '<%=Url.Action("AmendForNewSurvey","Survey") %>';
var SurveyOverview = '<%=Url.Action("GetDetailsOverview","Survey") %>';
var timePeriodId = "<%= Model.Id %>";


$(document).ready(function() {
    $("#survey-overview").colorbox({
        width: "960px",
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
        href: "#survey-overview-inline",
        onLoad: function() {
            var oTable = $('#survey-overview-table').dataTable( {
                "bStateSave": true,
                "bServerSide": true,
                "iTotalRecords": 50,
                "iTotalDisplayRecords": 50,
                "iDisplayLength": 50,
                "sAjaxSource": SurveyOverview,
                "fnServerParams": function (aoData) {
                    aoData.push({ "name": "sTimePeriodId", "value": timePeriodId });
                },
                "bProcessing": true,
                "aoColumns": [
                                { "sWidth": "200px" }, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
                            ]
	        });
        },
        onComplete: function() {
        },
        onCleanup: function() { 
            //$("#btn-print").remove(); $("#btn-print").unbind(); 
        }
    });
    $('#report-modelerextract-xls').click(function (e) {
        e.preventDefault();
        
        var reportAction = '<%= Url.Action("DownloadModelerExtract", "Survey", new { @timePeriodId = Model.Id }) %>';

        if (timePeriodId.length > 0) {
            var redirectUrl = reportAction;
            location.assign(redirectUrl);
        }
        return false;
    });
});

<%if (Model.ShowCertification) { %>
    <% if (Model.AgencyProjectList != null) { %>
        <% if (Model.AgencyProjectList.Count > 0) { %>
    var SendPrintNotificationUrl = '<%=Url.Action("SendPrintVerification","Survey") %>';

    $(document).ready(function() {
        $(".pricing").colorbox({
            width: "833px",
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
            href: "#price_report",
            onComplete: function() {
                var buttonCompletedSave = $('<span id="btn-print" class="cboxBtn">Print</span>').appendTo('#cboxContent');
                $("#btn-print").bind("click", function() {
                    ClickHereToPrint();
                    //send notifications
                    $.ajax({
                        type: "POST",
                        url: SendPrintNotificationUrl,
                        data: "sponsorName=<%= Model.AgencyProjectList.FirstOrDefault().SponsorName.ToString()%>",
                        dataType: "json",
                        success: function(response, textStatus, XMLHttpRequest) {
                            if (response.error == "false") {
                            }
                            else {
                            }
                        },
                        error: function(response, textStatus, AjaxException) {
                        }
                    });
                });
            },
            onCleanup: function() { $("#btn-print").remove(); $("#btn-print").unbind(); }
        });


        
    });
        <% } %>
    <% } %>
<% } %>
</script>

<div style='display: none'>
    <div id='price_report'>

        <script type="text/javascript">
            //<!--

            function ClickHereToPrint() {
                try {
                    var oIframe = document.getElementById('ifrmPrint');
                    var oContent = document.getElementById('pricingPrintArea').innerHTML;
                    var oDoc = (oIframe.contentWindow || oIframe.contentDocument);
                    if (oDoc.document) oDoc = oDoc.document;
                    oDoc.write("<html><head><title></title>");
                    oDoc.write("<link href='" + '<%= ResolveUrl("~/Content/CertPrint.css") %>' + "' rel='stylesheet' type='text/css' />");
                    oDoc.write("</head></body><body onload='this.focus(); this.print();'>");
                    oDoc.write(oContent + "</body></html>");
                    oDoc.close();
                }
                catch (e) {
                    self.print();
                }
            }

            function ClickHereToPrintTest() {
                try {
                    var oIframe = document.getElementById('ifrmPrint');
                    var oContent = document.getElementById('PrintAreaTest').innerHTML;
                    var oDoc = (oIframe.contentWindow || oIframe.contentDocument);
                    if (oDoc.document) oDoc = oDoc.document;
                    oDoc.write("<html><head><title></title>");
                    oDoc.write("<link href='" + '<%= ResolveUrl("~/Content/CertPrint.css") %>' + "' rel='stylesheet' type='text/css' />");
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

        <iframe id='ifrmPrint' src='#' style="width:0pt; height:0pt; border: none;"></iframe>

    <% if (Model.AgencyProjectList != null && Model.AgencyProjectList.Count > 0) { %>
        <div id="pricingPrintArea">
           <div class="myreport">
                <div id="logo">
                    <% var url = Html.ResolveUrl("~/content/images/drcog_logo_print.jpg"); %>
                    <%=Html.Image(url.ToString(), "D.R.C.O.G. Logo", null) %>
                </div>
                <h2><%= Model.AgencyProjectList.FirstOrDefault().SponsorName.ToString()%> Certification and Project List</h2>
                
                <h4>Verify the project list below, then print and sign this form and submit to DRCOG to complete the Transportation Improvement Survey response.</h4>
                <h3>Agency Technical Contact Person(s):</h3>
                
                <% if (Model.AgencySponsorContacts != null && Model.AgencySponsorContacts.Count > 0)
                  { %>
                <% Html.Grid(Model.AgencySponsorContacts).Columns(column =>
                   {
                       column.For(x => x.FullName).Named("Name");
                       column.For(x => x.Title).Named("Title");
                       column.For(x => x.Phone).Named("Phone Number");
                       //column.For(x => x.Fax).Named("Fax Number");
                       column.For(x => x.Email).Named("Email Address");
                   }).Attributes(new Dictionary<string, object> { { "id", "ContactListGrid" }, { "border", "0" } }).Render(); %>
                <% }
                  else
                  { %>
                No projects matching these search criteria were found.
                <% }  %>
                
                <% 
                    System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                    nfi.CurrencyGroupSeparator = ",";
                    nfi.CurrencySymbol = "$";
	            %>
               <h3>Project List</h3>
               <% if (Model.AgencyProjectList.Count > 0)
                  { %>
                    <% if (Model.AgencyProjectList[0].Funding != null)
                       { %>
                <% Html.Grid(Model.AgencyProjectList).Columns(column =>
                   {
                       column.For(x => x.ProjectName).Named("Project Name").Attributes(@class => "projectname");
                       column.For(x => x.ImprovementType).Named("Improvement Type").Attributes(@class => "improvetype");
                       column.For(x => x.UpdateStatus).Named("Survey Status");
                       column.For(x => x.ReportOnlyOpenDate).Named("Opened to Public");
                       column.For(x => x.Funding.TotalCost.ToString("C0", nfi)).Named("Total Cost<br/><span style=\"font-size: 8pt\">(in $1,000's)</span>");
                   }).Attributes(id => "MyProjectListGrid").Render(); %>
                   <% } %>
                <% }
                  else
                  { %>
                No projects matching these search criteria were found.
                <% }  %>
                
                <h3>Certification:</h3>
                <p>
                    I,________________________________________________________, certify that I have 
                    reviewed the projects listed 
                    above and that (a) the information provided in the database is correct to 
                    the best of our knowledge and (b) there are adequate revenues for the projects 
                    lised as open to the public by 2025 or earlier.
                </p>
                <p>Signature:____________________________________________________________ Date:___________________</p>
                <p>Name (please print):_______________________________________________________________________</p>
                <p>
                    Title:___________________________________________________________________<br />
                    <span style="padding: 0 0 0 35px;">(Transportation Director or Manager)</span>
                </p>  
                
                <p>
                    Thank you for completing the 2013 Transportation Improvement Survey!
                    <br />
                    You may be contacted if further information is needed.
                </p>              
                
           </div>
        </div>


        <div id="PrintAreaTest">
           <div class="myreport">
                <h2>Test Print</h2>            
           </div>
        </div>
    <% } %>

    </div>

    <div id='survey-overview-inline'>
        <table id="survey-overview-table" class="display primary">
	        <caption></caption>
	        <colgroup />
	        <colgroup span="2" title="title" />
	        <thead>
		        <tr>
			        <th scope="col">Project Name</th>
                    <th scope="col">Sponsor</th>
                    <th scope="col">COGID</th>
			        <th scope="col">Improvement Type</th>
                    <th scope="col">Network</th>
                    <th scope="col">OpenYear</th>
                    <th scope="col">Facility Name</th>
                    <th scope="col">Start At</th>
                    <th scope="col">End At</th>
                    <th scope="col">Lanes Base</th>
                    <th scope="col">Lanes Future</th>
                    <th scope="col">Facility Type</th>
                    <th scope="col">Modeling Check</th>
                    <th scope="col">LRS RouteName</th>
                    <th scope="col">LRS Begin</th>
                    <th scope="col">LRS End</th>
		        </tr>
	        </thead>
	        <tfoot>
		        <tr>
			        <td></td>
                    <td></td>
                    <td></td>
			        <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
		        </tr>
	        </tfoot>
	        <tbody>
	        </tbody>
        </table>
    </div>
</div>

<%if (!Model.Id.Equals(default(int)) && Request.IsAuthenticated && Model.IsEditable()) { %>
    <%--<% Html.RenderPartial("~/Views/Survey/Partials/CreatePartial.ascx", Model); %>--%>
    <% Html.RenderAction("CreatePartial", "Survey", new { model = Model }); %>
<% } %>
<%if (Model.IsAdmin()) { %>
    <% Html.RenderPartial("~/Views/Survey/Partials/AmendPartial.ascx"); %>
<% } %>
<%--<%if (Request.IsAuthenticated) { Html.RenderAction("BecomeASpsonsor"); } %>--%>