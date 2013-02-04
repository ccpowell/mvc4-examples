<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.Survey.FundingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Survey Funding
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Transportation Improvement Survey <%= Model.Current.Name %></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Page.ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript" ></script>
    <link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Page.ResolveClientUrl("~/scripts/funding.survey.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maxlength.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maskMoney.js")%>" type="text/javascript" ></script>
    <script type="text/javascript">
        var EditFinancialRecordUrl = '<%=Url.Action("UpdateFinancialRecord")%>';
        var GetCategoryDetails = '<%= Url.Action("GetPlanReportGroupingCategoryDetails")%>'
        var deleteFundingSource = '<%= Url.Action("DeleteFundingSource")%>'
        var addFundingSource = '<%= Url.Action("AddFundingSource")%>'
        var updateFundingSource = '<%= Url.Action("UpdateFundingSource")%>'
        var UpdateProjectUpdateStatusUrl = '<%=Url.Action("UpdateProjectUpdateStatus")%>';
    </script>
    <script type="text/javascript">
        $(document).ready(App.tabs.initializeSurveyProjectTabs);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container funding-tab">
    <%Html.RenderPartial("~/Views/Survey/Partials/ProjectTabPartial.ascx", Model); %>
   
    <div class="ui-widget">
 
<!-- FINANCIAL RECORD -->
<div id="funding-container">
		<div id="resultRecord"></div>
		<h2 style="float: left; text-align: left;">Project Financial Records</h2><span style="position: relative; left: 100px; text-align: right;" ><span style="font-size: 1.2em; font-weight: bold;">{ <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span> }</span></span>
		<br />
		<div id="fundingRecords">
		    
		    <div style="position: relative;">
		        <% 
		            System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
                    nfi.CurrencyGroupSeparator = "";
                    nfi.CurrencySymbol = "";          
		        %>
		        <p>
		            <label>Total Cost:</label>
                    
                    <%= Html.DrcogTextBox("Funding.TotalCost", Model.Project.IsEditable(), Model.Project.Funding.TotalCost.ToString("C", nfi), new { @class = "money" })%>
		        </p>
                <br />
                
            </div>
            <hr style="position: relative; left: 10px; width: 620px;" />
            
            <h2>Funding Source(s)</h2>
            <table id="fundingsources">
            <%foreach (DRCOG.Domain.Models.Survey.FundingSource source in Model.FundingSources)
             { %>
                <tr id="fundingsource_row_<%=source.Id %>">
                    <td>
                        <span class="fakeinput w250" id="fundingsource_<%= source.Id %>_name"><%= source.Name %></span>
                    </td>
                <%if (Model.Project.IsEditable())
                  { %>
                    <td><button class="fundingsource-delete fg-button ui-state-default ui-priority-primary ui-corner-all" id='fundingsource_delete_<%=source.Id.ToString() %>'>Delete</button></td>                                
                <%} %> 
                </tr>
            <%} %>
            <%if (Model.Project.IsEditable())
              { %>
                <tr id="fundingsource-editor">
                <td><%= Html.DropDownList("fundingsource_new_name",
                    Model.Project.IsEditable(),
                    new SelectList(Model.AvailableFundingResources, "key", "value"),
                    "-- Select --",
                    new { @class = "longInputElement not-required", title = "Please select a project sponsor" })%>
                </td>
                <td><button id="btn-fundingsource-new" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all add">Add</button></td>                                
                </tr>                      
            <%} %>
            </table>
        </div>
        
        <br />
        
            <%--<button class="update-financialrecord fg-button ui-state-default ui-priority-primary ui-corner-all <%if(!Model.Project.IsEditable()){ %>hidden<% } %>" id='update_<%= Model.ProjectFunding.ProjectVersionId %>'>Update</button>--%>
        
        
    <br />
        
</div>
	</div>
</div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
