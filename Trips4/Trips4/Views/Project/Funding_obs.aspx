<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.FundingViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	DRCOG :: Funding
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.meio.mask.min.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/jquery.eede.js")%>" type="text/javascript" ></script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript" ></script>
    <link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        var EditFinancialRecordUrl = '<%=Url.Action("UpdateFinancialRecord")%>';
        var EditFinancialRecordDetailUrl = '<%=Url.Action("UpdateFinancialRecordDetail")%>';
        var AddFinancialRecordDetailUrl = '<%=Url.Action("AddFinancialRecordDetail")%>';
    </script>
    <script src="<%= Page.ResolveClientUrl("~/scripts/Funding.js")%>" type="text/javascript" ></script>
 
    <link href="<%= Page.ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container funding-tab">
<% Html.RenderPartial("~/Views/Project/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>

<div class="ui-widget"> 
<!-- FINANCIAL RECORD DETAILS -->    
    <div id="financial-record-details">
        <div style="float: left; width: 100%;" >
            <h2 style="float: left; text-align: left;">Project Financial Record Details</h2>
            <span style="float: right; text-align: right;" ><span style="font-size: 1.2em; font-weight: bold;">{ <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span> }</span></span>
        </div>
        <div id="resultRecordDetail"></div>
        <table>
            <tr>
                <th style="width:250px;">Funding Group</th>
                <th style="width:100px;">Funding Level</th>
                <% foreach (var increment in Model.FundingDetailPivotModel.FundingIncrements) { %>
                    <th style="width:75px;"><%= increment.FundingIncrementName.ToString() %></th>
                <% } %>
                <%if (Model.ProjectSummary.IsEditable()) { %>
                    <th>&nbsp;</th>
                <% } %>
            </tr>
            <% foreach (System.Data.DataRow item in Model.FundingDetailPivotModel.FundingDetailTable.Rows) { %>
                <tr id="fundingdetail_row_<%=item[1].ToString() + '_' + item[3].ToString() %>">
                    <td id="fundingdetail_<%=item[1].ToString() + '_' + item[3].ToString() %>_FundingType"><%=item[2].ToString() %></td>
                    <td id="fundingdetail_<%=item[1].ToString() + '_' + item[3].ToString() %>_FundingLevel"><%=item[4].ToString() %></td>
                <% for (int j = 8; j < item.Table.Columns.Count; j++) { %>
                    <td><%= Html.DrcogTextBox("fundingdetail_" + item[1].ToString() + "_" + item[3].ToString() + "_i" + (j - 7), Model.ProjectSummary.IsEditable(), item[j].ToString(), new { style = "width:75px;", @maxlength = "75", @class = "money", @alt = "money" })%></td>            
                <% } %>
                
                <%  int pt = 0;
                    int lastColumnIndex = (item.Table.Columns.Count - 1);
                    for (int j = 8; j < (item.Table.Columns.Count - 1); j++) 
                        pt = pt + int.Parse(item[j].ToString()); %>
                    <%= Html.Hidden("ProgrammedTotal_" + item[1].ToString() + '_' + item[3].ToString(), pt.ToString()) %>
                    <%= Html.Hidden("OutyearTotal_" + item[1].ToString() + '_' + item[3].ToString(), int.Parse(item[lastColumnIndex].ToString()))%>
                    
                <%if (Model.ProjectSummary.IsEditable()) { %>
                    <td><a id="updateFRD_<%=item[0].ToString() + '_' + item[1].ToString() + '_' + item[3].ToString() %>" class="update-financialrecorddetail fg-button w70 medium ui-state-default ui-priority-primary ui-corner-all" href="#">Update</a></td>
                <% } %>
            </tr>
            <% } %>
        </table>
    </div>

    <%if (Model.ProjectSummary.IsEditable()) { %>
    <div>
        <p>
            <a id="AddFundingGroup" class="add-financialrecorddetail fg-button w380 medium ui-state-default ui-priority-primary ui-corner-all" href="#">Add new Funding Group</a>
        </p>
    </div>
    <% } %>
    <div style="float: left; width: 100%;">&nbsp;</div>
    
<!-- FINANCIAL DETAIL SUMMARY -->
    <div id="financial-record-detail-summary"> 
	    <div style="float: left; width: 100%;" >
		    <h2 style="float: left; text-align: left;">Project Financial Record Details Summary</h2>
		    <span style="float: right; text-align: right;" ><span style="font-size: 1.2em; font-weight: bold;">{ <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span> }</span></span>
		</div>    
        <table style="float: left;">
        <tr>
            <th>Previous</th>
            <th>Programmed</th>
            <th>OutYear</th>
            <th>Future</th>
            <th>Total</th>
        </tr>
        <% foreach (var item in Model.TipProjectFunding) { %>
        <tr>
            <td><div id="previousTotalLabel"><%= item.Previous.ToString()%></div></td>
            <td><div id="programmedTotalLabel">0</div></td>
            <td><div id="outyearTotalLabel">0</div></td>
            <td><div id="futureTotalLabel"><%= item.Future.ToString()%></div></td>
            <td><div id="projectCostTotalLabel">0</div></td>
        </tr>
        <% } %>
        </table>
    </div>
    <div style="float: left; width: 100%;">&nbsp;</div>
  
<!-- FINANCIAL RECORD -->   
    <div id="financial-record"> 
		<div style="float: left; width: 100%;" >
		    <h2 style="float: left; text-align: left;">Project Financial Records</h2>
		    <span style="float: right; text-align: right;" ><span style="font-size: 1.2em; font-weight: bold;">{ <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span> }</span></span>
		</div>
		<div id="resultRecord" style="float: left; text-align: left; width: 100%;"></div>
		<table id="financial-record">
        <tr>
            <th>Previous</th>
            <th>Future</th>
            <th>TipFunding</th>
            <th>FederalTotal</th>
            <th>StateTotal</th>
            <th>LocalTotal</th>
            <th>TotalCost</th>
            <%if(Model.ProjectSummary.IsEditable()){ %><th></th><% } %>
        </tr>
    <% foreach (var item in Model.TipProjectFunding) { %>
        <tr id="financialrecord_row_<%=item.ProjectFinancialRecordID.ToString() %>">
            <td><%= Html.DrcogTextBox("financialrecord_" + item.ProjectFinancialRecordID.ToString() + "_Previous", Model.ProjectSummary.IsEditable(), item.Previous.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecord_" + item.ProjectFinancialRecordID.ToString() + "_Future", Model.ProjectSummary.IsEditable(), item.Future.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecord_" + item.ProjectFinancialRecordID.ToString() + "_Funding", Model.ProjectSummary.IsEditable(), item.Funding.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecord_" + item.ProjectFinancialRecordID.ToString() + "_FederalTotal", Model.ProjectSummary.IsEditable(), item.FederalTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecord_" + item.ProjectFinancialRecordID.ToString() + "_StateTotal", Model.ProjectSummary.IsEditable(), item.StateTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecord_" + item.ProjectFinancialRecordID.ToString() + "_LocalTotal", Model.ProjectSummary.IsEditable(), item.LocalTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecord_" + item.ProjectFinancialRecordID.ToString() + "_TotalCost", Model.ProjectSummary.IsEditable(), item.TotalCost.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
        <%if(Model.ProjectSummary.IsEditable()){ %>
            <td><button class="update-financialrecord fg-button ui-state-default ui-priority-primary ui-corner-all" id='updateFR_<%=item.ProjectFinancialRecordID.ToString() %>'>Update</button></td>
        <% } %>
        </tr>
    <% } %>
    </table>
    </div>
    <div style="float: left; width: 100%;">&nbsp;</div>   
 
<!-- FINANCIAL RECORD HISTORY --> 
    <div id="financial-details-history">
    <div style="float: left; width: 100%;" >
        <h2 style="float: left; text-align: left;">Project Financial Records History</h2>
        <span style="float: right; text-align: right;" ><span style="font-size: 1.2em; font-weight: bold;">{ <span style="font-size: .7em; font-weight: normal;">all costs and revenues in $1,000s</span> }</span></span>
    </div>
		<table id="financial-record-history">
        <tr>
            <th></th>
            <th>Amend<br />Date</th>
            <th>Amend<br />Status</th>
            <th>Previous</th>
            <th>Future</th>
            <th>TipFunding</th>
            <th>Federal<br />Total</th>
            <th>State<br />Total</th>
            <th>Local<br />Total</th>
            <th>Total<br />Cost</th>
        </tr>
    <% foreach (var item in Model.ProjectFundingHistory) { %>
        <tr id="financialrecordhistory_row_<%=item.ProjectFinancialRecordID.ToString() %>">
            <td>
                <% if (item.ProjectFinancialRecordID.ToString() == Model.ProjectSummary.ProjectVersionId.ToString()) { %>
                    Current
                <% } else  { %>
                <a href="<%= Url.Action("Details", "Project", new { tipYear = Model.ProjectSummary.TipYear, id = item.ProjectFinancialRecordID.ToString() }) %>">Details</a>
                <% } %>
            </td>
            <td><%= item.AmendmentDate.Value.Date.ToShortDateString() %></td>
            <td><%= item.AmendmentStatus.ToString() %></td>
            <td><%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_Previous", Model.ProjectSummary.IsEditable(), item.Previous.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_Future", Model.ProjectSummary.IsEditable(), item.Future.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_Funding", Model.ProjectSummary.IsEditable(), item.Funding.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_FederalTotal", Model.ProjectSummary.IsEditable(), item.FederalTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_StateTotal", Model.ProjectSummary.IsEditable(), item.StateTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_LocalTotal", Model.ProjectSummary.IsEditable(), item.LocalTotal.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
            <td><%= Html.DrcogTextBox("financialrecordhistory_" + item.ProjectFinancialRecordID.ToString() + "_TotalCost", Model.ProjectSummary.IsEditable(), item.TotalCost.ToString(), new { style = "width:75px;", @maxlength = "75", @alt = "money" })%></td>
        </tr>
    <% } %>
    </table>
    </div>
    <div style="float: left; width: 100%;">&nbsp;</div>
 
 	</div> 
</div>

<!-- This contains the hidden content for inline calls. Now becomes the Add Funding Group dialog. -DBD -->
<div style='display:none'>
	<div id='add-newGroup-panel' style='padding:10px; background:#fff;'>
        <table>
        <%if(Model.ProjectSummary.IsEditable()){ %>
            <tr>
                <td colspan="6">
                    <%= Html.DropDownList("FinancialRecordDetail.FundingTypes",
                                true,
                                new SelectList(Model.FundingTypes, "key", "value"),
                                "-- Select a Funding Type --", 
                                new { @class = "mediumInputElement not-required", title="Please select a funding type" })%>
                </td>
                <td>
                <a id="AddFRD_<%= Model.ProjectSummary.ProjectVersionId.ToString() %>" class="add-newFundingGroup-final fg-button w70 medium ui-state-default ui-priority-primary ui-corner-all" href="#">Add</a>
                </td>
            </tr>
        <% } %>
        </table>
	</div>
</div>

<!-- Panel -->
<div id="toppanel">
	<div id="panel">
		<div class="content clearfix">
			<% Html.RenderPartial("~/Views/Project/Partials/ProjectSummaryBoxPartial.ascx", Model.ProjectSummary); %>
		</div>
	</div>	

    <!-- The tab on top -->	
	<div class="tab">
		<ul class="login">
	    	<li class="left">&nbsp;</li>
	        <li>General Info</li>
			<li class="sep">|</li>
			<li id="toggle">
				<a id="open" class="open" href="#">Open Panel</a>
				<a id="close" style="display: none;" class="close" href="#">Close Panel</a>			
			</li>
	    	<li class="right">&nbsp;</li>
		</ul> 
	</div> <!-- / top -->
	
</div> 
<!--panel -->
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
