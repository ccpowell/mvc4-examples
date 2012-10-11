<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StrikesViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Project General Information
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="view-content-container" style="height:520px;">
        <% Html.RenderPartial("~/Views/Project/Partials/BreadcrumbPartial.ascx", Model.ProjectSummary); %>
        <div class="clear"></div>        
        <%Html.RenderPartial("~/Views/Project/Partials/ProjectTabPartial.ascx", Model.ProjectSummary); %>
        
        <div class="ui-widget">
				<div style="padding: 0pt 0.7em;" class="ui-state-error ui-corner-all"> 
					<p>
					<span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span> 
					<strong>Note:</strong>This form will likely be removed after specification review. -DBD 02/10/2010</p>
				</div>
			</div>
        
        <div id="projectStrikesForm" class="formContainer" style="margin-left:20px;">
        This form will likely be removed after specification review. -DBD 02/10/2010
        </div>
        
        <div class="helpContainer">
            <fieldset>
            <legend>Project Strikes Information</legend>
            <p>Area for Project's Strikes Information Help Text.</p>
            </fieldset>         
        </div>
    </div>
    

    <script type="text/javascript"></script>
</asp:Content>
