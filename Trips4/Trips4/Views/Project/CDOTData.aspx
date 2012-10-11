<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProjectCdotDataViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Project General Information
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="view-content-container" >
        <% Html.RenderPartial("~/Views/Project/Partials/BreadcrumbPartial.ascx", Model.ProjectSummary); %>
        <div class="clear"></div>        
        <%Html.RenderPartial("~/Views/Project/Partials/ProjectTabPartial.ascx", Model.ProjectSummary); %>
        
        <div id="resultRecord"></div>
        <h2 style="float: left; text-align: left;">Project Financial Records</h2><span style="float: right; text-align: right;" ><span style="font-size: 1.2em; font-weight: bold;">{ <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span> }</span></span>
        <br />
        <div id="projectCDOTDataForm" class="formContainer" style="margin-left:20px;">
             <% Model.ProjectSummary.IsCurrent = false; %>
             <table>
                <thead>
                    <tr>
                        <th>Construction Date</th>
                        <th>Scheduled AD Date</th>
                        <th>Project Stage Date</th>
                        <th>Project Closed</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                                <%= Html.DrcogTextBox("ConstructionDate", Model.ProjectSummary.IsEditable(), Model.TipCdotData.ShowShortDate(Model.TipCdotData.ConstructionDate), null)%>
                        </td>
                        <td>
                                <%= Html.DrcogTextBox("ScheduledADDate", Model.ProjectSummary.IsEditable(), Model.TipCdotData.ShowShortDate(Model.TipCdotData.ScheduledADDate), null)%>
                        </td>
                        <td>
                                <%= Html.DrcogTextBox("ProjectStageDate", Model.ProjectSummary.IsEditable(), Model.TipCdotData.ShowShortDate(Model.TipCdotData.ProjectStageDate), null)%>
                        </td>
                        <td>
                                <%= Html.DrcogTextBox("ProjectClosed", Model.ProjectSummary.IsEditable(), Model.TipCdotData.ShowShortDate(Model.TipCdotData.ProjectClosed), null)%>
                        </td>
                    </tr>
                </tbody>
             </table>
             <br />
             <p>
                <label>CDOT Project Number:</label>
                    <%= Html.DrcogTextBox("CDOTProjectNumber", Model.ProjectSummary.IsEditable(), Model.TipCdotData.CDOTProjectNumber, null)%>
             </p>
            <p>   
                <label>CMS Number:</label>
                    <%= Html.DrcogTextBox("CMSNumber", Model.ProjectSummary.IsEditable(), Model.TipCdotData.CMSNumber, null)%>
            </p>
            <p>
                <label>LRP Number:</label>
                    <%= Html.DrcogTextBox("LRPNumber", Model.ProjectSummary.IsEditable(), Model.TipCdotData.LRPNumber, null)%>
            </p>
            <p>    
                <label>Project Stage:</label>
                    <%= Html.DrcogTextBox("ProjectStage", Model.ProjectSummary.IsEditable(), Model.TipCdotData.ProjectStage, null)%>
            </p>
            <p>    
                <label>Sub Account:</label>
                    <%= Html.DrcogTextBox("SubAccount", Model.ProjectSummary.IsEditable(), Model.TipCdotData.SubAccount, null)%>
            </p>
            <p>
                <label>CDOT Project Manager:</label>
                    <%= Html.DrcogTextBox("CDOTProjectManager", Model.ProjectSummary.IsEditable(), Model.TipCdotData.CDOTProjectManager, null)%>
            </p>
            <br />
            <table id="CdotGeneralGroup">
                <tr>
                    <td>
                        <label>Region:</label>
                            <%= Html.DrcogTextBox("Region", Model.ProjectSummary.IsEditable(), Model.TipCdotData.Region, null)%>
                    </td>
                    <td>
                        <label>Comm District:</label>
                            <%= Html.DrcogTextBox("CommDistrict", Model.ProjectSummary.IsEditable(), Model.TipCdotData.CommDistrict, null)%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>STIP ID:</label>
                            <%= Html.DrcogTextBox("STIPID", Model.ProjectSummary.IsEditable(), Model.TipCdotData.STIPID, null)%>
                    </td>
                    <td>
                        <label>Investment Category:</label>
                            <%= Html.DrcogTextBox("InvestmentCategory", Model.ProjectSummary.IsEditable(), Model.TipCdotData.InvestmentCategory, null)%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Begin Miles:</label>
                            <%= Html.DrcogTextBox("BeginMilePost", Model.ProjectSummary.IsEditable(), Model.TipCdotData.BeginMilePost, null)%>
                    </td>
                    <td>
                        <label>Corridor ID:</label>
                            <%= Html.DrcogTextBox("CorridorID", Model.ProjectSummary.IsEditable(), Model.TipCdotData.CorridorID, null)%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>End Miles:</label>
                            <%= Html.DrcogTextBox("EndMilePost", Model.ProjectSummary.IsEditable(), Model.TipCdotData.EndMilePost, null)%>
                    </td>
                    <td>
                        <label>Construction RE:</label>
                            <%= Html.DrcogTextBox("ConstructionRE", Model.ProjectSummary.IsEditable(), Model.TipCdotData.ConstructionRE, null)%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Route Segment:</label>
                            <%= Html.DrcogTextBox("RouteSegment", Model.ProjectSummary.IsEditable(), Model.TipCdotData.RouteSegment, null)%>
                    </td>
                    <td>
                        <label>STIP Project Division:</label>
                            <%= Html.DrcogTextBox("STIPProjectDivision", Model.ProjectSummary.IsEditable(), Model.TipCdotData.STIPProjectDivision, null)%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>TPR ID:</label>
                            <%= Html.DrcogTextBox("TPRID", Model.ProjectSummary.IsEditable(), Model.TipCdotData.TPRID, null)%>
                    </td>
                    <td>
                        <label>TPR Abbr:</label>
                            <%= Html.DrcogTextBox("TPRAbbr", Model.ProjectSummary.IsEditable(), Model.TipCdotData.TPRAbbr, null)%>
                    </td>
                </tr>
            </table>
            <p>
                <label>Notes:</label>
                <br />
                <%if (Model.ProjectSummary.IsEditable())
                  { %>
                    <%= Html.TextArea2("Notes", true, Model.TipCdotData.Notes, 10, 10, new { cols = "100", rows = "5" })%>
                <% }
                  else
                  { %>
                    <div id="Notes" class="faketextarea" style="display: block; position: relative; top: 25px; width: 500px; min-height: 120px;"><%= Html.Encode(Model.TipCdotData.Notes.ToString())%></div>
                <% } %>
            </p>
            </div>
            <br />
            <%if(Model.ProjectSummary.IsEditable() && false){ // hiding until I am ready for it to be edited%>
                <button class="update-financialrecord fg-button ui-state-default ui-priority-primary ui-corner-all" id='update_<%= Model.ProjectSummary.ProjectVersionId %>'>Update</button>
            <% } %>
            <br />
    </div>
</asp:Content>
