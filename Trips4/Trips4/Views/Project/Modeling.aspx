<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.ProjectBaseViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Modeling</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container">
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
</div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
