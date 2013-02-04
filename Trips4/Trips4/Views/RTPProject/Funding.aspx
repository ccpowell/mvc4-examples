<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.RTP.Project.FundingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Funding
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="BannerContent" runat="server">Regional Transportation Plan <%= Model.ProjectSummary.RtpYear%></asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <link href="<%= Page.ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript" ></script>
    <link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script src="<%= Page.ResolveClientUrl("~/scripts/Funding-rtp.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maxlength.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.maskMoney.js")%>" type="text/javascript" ></script>
    <script type="text/javascript">
        var EditFinancialRecordUrl = '<%=Url.Action("UpdateFinancialRecord")%>';
        var GetCategoryDetails = '<%= Url.Action("GetPlanReportGroupingCategoryDetails")%>'
        var deleteFundingSource = '<%= Url.Action("DeleteFundingSource")%>'
        var addFundingSource = '<%= Url.Action("AddFundingSource")%>'
        var updateFundingSource = '<%= Url.Action("UpdateFundingSource")%>'
    </script>

    <script type="text/javascript">
        $(document).ready(App.tabs.initializeRtpProjectTabs);
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container funding-tab">
    <% Html.RenderPartial("~/Views/RtpProject/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
   
    <div class="ui-widget">
 
        <!-- FINANCIAL RECORD -->
        <div id="funding-container">
		        <div id="resultRecord"></div>
		        <h2 style="float: left; text-align: left;">Project Financial Records</h2><span style="position: relative; left: 100px; text-align: right;" ><span style="font-size: 1.2em; font-weight: bold;">{ <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span> }</span></span>
		        <br />
		        <div id="fundingRecords">
        		    
		            <div id="fundingRecords-section1" style="position: relative;">
                        <p>
                            <label>Constant Cost:</label>
                            <%= Html.DrcogTextBox("Funding.ConstantCost", Model.ProjectSummary.IsEditable(), Model.ProjectFunding.ConstantCost, new { @class = "money" })%>
                        </p>
                        <p>
                            <label>Vision Cost:</label>
                            <%= Html.DrcogTextBox("Funding.VisionCost", Model.ProjectSummary.IsEditable(), Model.ProjectFunding.VisionCost, new { @class = "money" })%>
                        </p>
                        <p>
                            <label>YOE Cost:</label>
                            <%= Html.DrcogTextBox("Funding.YOECost", Model.ProjectSummary.IsEditable(), Model.ProjectFunding.YOECost, new { @class = "money" })%>
                        </p>
                        <br />
                        <div>
                            <label>Notes:</label>
                            <span class="faketextarea"><%--Notes are soon to come--%></span>
                        </div>
                        
                        <%if (Model.ProjectSummary.IsEditable()) { %>
                        <div id="rtp-funding-planinfo" class="box">
                            <h3>Plan Info</h3>
                            <div>
                                <label>Plan:</label>
                                <span class="fakeinput"><%= Model.ProjectSummary.RtpYear %></span>
                            </div>
                            <div>
                                <label>Cycle:</label>
                                <span class="fakeinput"><%= Model.ProjectSummary.Cycle.Name %></span>
                            </div>
                            <div>
                                <label>Type:</label>
                                <%= Html.DropDownList("Funding.PlanTypeId",
                                    Model.ProjectSummary.IsEditable(),
                                    new SelectList(Model.PlanTypes, "key", "value", Model.ProjectFunding.PlanTypeId),
                                    "-- Select --",
                                    new { @class = "not-required", title = "Please select a project sponsor" })%>
                                <%--<span class="fakeinput"><%= Model.ProjectSummary.PlanType %></span>--%>
                            </div>
                            <div>
                                <label>Date of Adoption:</label>
                                <span class="fakeinput"><% if(Model.ProjectSummary.AdoptionDate != DateTime.MinValue) { %><%= Model.ProjectSummary.AdoptionDate.ToShortDateString() %><% } %></span>
                            </div>
                            <div>
                                <label>Last Amended:</label>
                                <span class="fakeinput"><% if(Model.ProjectSummary.LastAmendmentDate != DateTime.MinValue) { %><%= Model.ProjectSummary.LastAmendmentDate.ToShortDateString()%><% } %></span>
                            </div>
                        </div>
                        <% } %>
                    </div>
                    <hr style="position: relative; left: 10px; width: 620px;" />
                    <div id="fundingRecords-section2" style="position: relative;">
                        <p>
                            <label>Category:</label><span id="btn-newcategory" class="fakelink<% if(!Model.ProjectSummary.IsEditable()) {%> hidden<% } %>">(create category)</span>
                            <%= Html.DropDownList("Funding.ReportGroupingCategoryId",
                                    Model.ProjectSummary.IsEditable(),
                                    new SelectList(Model.ProjectFunding.ReportGroupingCategories, "key", "value", Model.ProjectFunding.ReportGroupingCategoryId),
                                    "-- Select --",
                                    new { @class = "mediumInputElement not-required", title = "Please select a project sponsor" })%>
                        </p>
                        <p>
                            <label>Short Name</label>
                            <span id="catShort" class="fakeinput"></span>
                        </p>
                        <p>
                            <label>Description</label>
                            <span id="catLong" class="fakeinput"></span>
                        </p> 
                         
                        <div id="rtp-funding-amendment" class="box">
                            <div>
                                <label>Amendment Status:</label>
                                <span class="fakeinput"><%= Model.ProjectSummary.Cycle.Status %></span>
                            </div>
                            <div>
                                <label>Reason:</label>
                                <span class="faketextarea"><%= Model.ProjectSummary.Cycle.Reason %></span>
                            </div>
                            <div>
                                <label>Start Date:</label>
                                <span class="fakeinput"><% if(Model.ProjectSummary.Cycle.Date != DateTime.MinValue) { %><%= Model.ProjectSummary.Cycle.Date.ToShortDateString() %><% } %></span>
                            </div>
                        </div>
                    </div>
                    <hr style="position: relative; left: 10px; width: 620px;" />
                    
                    <h2>Funding Source(s)</h2>
                    <table id="fundingsources">
                    <%foreach (DRCOG.Domain.Models.RTP.FundingSource source in Model.FundingSources)
                     { %>
                        <tr id="fundingsource_row_<%=source.Id %>">
                            <td>
                                <span class="fakeinput w250" id="fundingsource_<%= source.Id %>_name"><%= source.Name %></span>
                            </td>
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                            <td><button class="fundingsource-delete fg-button ui-state-default ui-priority-primary ui-corner-all" id='fundingsource_delete_<%=source.Id.ToString() %>'>Delete</button></td>                                
                        <%} %> 
                        </tr>
                    <%} %>
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                        <tr id="fundingsource-editor">
                        <td><%= Html.DropDownList("fundingsource_new_name",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AvailableFundingResources, "key", "value"),
                            "-- Select --",
                            new { @class = "longInputElement not-required", title = "Please select a project sponsor" })%>
                        </td>
                        <td><button id="btn-fundingsource-new" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all add">Add</button></td>                                
                        </tr>                      
                    <%} %>
                    </table>
                </div>
                <div id="update-inview">
                <button class="update-financialrecord fg-button ui-state-default ui-priority-primary ui-corner-all <%if(!Model.ProjectSummary.IsEditable()){ %>hidden<% } %>" id='update_<%= Model.ProjectFunding.ProjectVersionId %>'>Update</button>
                </div>
            <br />
                
        </div>
	</div>
</div>

<% Html.RenderPartial("~/Views/RtpProject/Partials/AddCategoryPartial.ascx", Model.ProjectSummary); %>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
