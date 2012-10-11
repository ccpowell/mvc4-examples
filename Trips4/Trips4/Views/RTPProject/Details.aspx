<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
    Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.Project.DetailViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project Detailed Home</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.ProjectSummary.RtpYear%></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
<script src="<%= ResolveClientUrl("~/scripts/Slide.js")%>" type="text/javascript" ></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container">
    <% Html.RenderPartial("~/Views/RTPProject/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
    <div id="details">
        <div id="scope" style="padding-top: 20px;">
            <h2 style="padding: 0 10px 10px 0px;">Description: <span style="font-style: italic;"><%= Model.GeneralInfo["ShortDescription"] %></span></h2>
            <ul>
                <li><span class="darkGrey boldFont">Project Status:</span> <span id="projectStatus"><%= Model.GeneralInfo["ProjectStatus"]%></span></li>
                <li><span class="darkGrey boldFont">Amendment Status:</span> <span id="amendmentStatus"></span><%= Model.ProjectSummary.AmendmentStatus %></span></li>
            </ul>
        </div>
        
        <div id="regionMLeft">
            <h2>Details</h2>
            <hr style="margin-right: 10px;"/>
            <table class="totalWidth">
                <tr>
                    <td class="first">Project Type:</td>
                    <td>&nbsp;</td>
                    <td><div class="totalWidth"><%= Model.GeneralInfo["ProjectType"]%></div></td>
                </tr>
                <%--<tr>
                    <td>Improvement Type:</td>
                    <td>&nbsp;</td>
                    <td><div class="totalWidth"><%= Model.GeneralInfo["ImprovementType"]%></div></td>
                </tr>--%>
                <tr>
                    <td class="first">Primary Sponsor:</td>
                    <td>&nbsp;</td>
                    <td><div class="totalWidth"><%= Model.GeneralInfo["PrimarySponsor"]%></div></td>
                </tr>
                <%--<tr>
                    <td>Road or Transit:</td>
                    <td>&nbsp;</td>
                    <td><div class="totalWidth"><%= Model.GeneralInfo["RoadOrTransit"]%></div></td>
                </tr>--%>
                <%--<tr>
                    <td>Selection Agency:</td>
                    <td>&nbsp;</td>
                    <td><div class="totalWidth"><%= Model.GeneralInfo["SelectionAgency"]%></div></td>
                </tr>--%>
                <%--<%if (Model.ProjectSummary.IsEditable()) { %>
                <tr>
                    <td>Regionally Significant?:</td>
                    <td>&nbsp;</td>
                    <td><%= Model.GeneralInfo["RegionalSignificance"]%></td>
                </tr>
                <% } %>--%>
            </table>
            
        </div>
        <div id="regionMRight">
            <h2>Funding</h2>
            <hr style="margin-right: 10px;"/>
            <table class="totalWidth">
                <tr>
                    <td>Estimated Cost:</td>
                    <td>&nbsp;</td>
                    <td><div>$ <%= string.Format("{0:0,0.0}", Convert.ToDouble(Model.GeneralInfo["ConstantCost"])) %> (in 1000s, FY'08 $s)</div></td>
                </tr>
                <tr>
                    <td style="vertical-align: top;">Funding Source:</td>
                    <td>&nbsp;</td>
                    <td>
                        <%= Model.GroupingCategory.Description %>
                        <%--<% foreach (var source in Model.FundingSources) { %>
                                <div><%= source.Name %></div>
                        <% } %>--%>
                    </td>
                </tr>
                <%--<tr>
                    <td>Sponsor Contact:</td>
                    <td>&nbsp;</td>
                    <td><div><%= Model.ProjectSponsorsModel.SponsorContact %></div></td>
                </tr>
                <tr>
                    <td>Admin Level:</td>
                    <td>&nbsp;</td>
                    <td><div><%= Model.GeneralInfo["AdminLevel"] %></div></td>
                </tr>
                <tr>
                    <td>Facility Name:</td>
                    <td>&nbsp;</td>
                    <td><div><%= Model.GeneralInfo["FacilityName"]%></div></td>
                </tr>--%>
            </table>
        </div>
        <%if (Model.ProjectSummary.IsEditable()) { %>
        <div id="notes">
            <table class="totalWidth">
                <tr>
                    <td>
                        Sponsor Notes: 
                        <div class="blockoff totalWidth"><%= Model.GeneralInfo["SponsorNotes"]%></div>
                    </td>
                    <td>
                        DRCOG Notes:
                        <div class="blockoff totalWidth"><%= Model.GeneralInfo["DRCOGNotes"]%></div>        
                    </td>
                </tr>
            </table>
        </div>
        <% } %>
        <div id="location">
            
        </div>
        <div id="segments-wrapper">
           <h2>Project Staging</h2>
           <table id="segments">
            <tr>
                <th style="width:250px;">Facility Name</th>
                <th style="width:250px;">Improvement Type</th>
                <th style="width:150px;">Begin</th>
                <th style="width:150px;">End</th>
                <th style="width:75px;">Staging</th>
                <th style="width:75px;">Open Year</th>
            </tr>

        <% foreach (var item in (Model.Segments as List<DRCOG.Domain.Models.RTP.SegmentModel>).Where(x => (x.ModelingFacilityTypeId != 13763 && x.ModelingFacilityTypeId != 38935 && x.ModelingFacilityTypeId != 13408 && x.ModelingFacilityTypeId != 13761)))
           { %>
            <tr id="segment_row_<%=item.SegmentId.ToString() %>">
                <td><%= item.FacilityName.ToString() %></td>
                <td><%= item.ImprovementType.ToString() %></td>
                <td><%= item.StartAt.ToString()%></td>
                <td><%= item.EndAt.ToString() %></td>
                <td><%= item.Staging.ToString() %></td>
                <td><%= item.OpenYear.ToString() %></td>
            </tr>
        <% } %>
           </table>
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
</script>
</asp:Content>
