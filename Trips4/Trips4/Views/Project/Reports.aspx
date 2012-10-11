<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIPProject.DetailViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project Reports</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container">
    <div class="leftColumn">
        
        <div class="clear"></div>
    </div>
    <div class="ui-widget">
		<div style="padding: 0pt 0.7em;" class="ui-state-error ui-corner-all"> 
			<p>
			<span style="float: left; margin-right: 0.3em;" class="ui-icon ui-icon-alert"></span> 
			<strong>Note:</strong>Not built</p>
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
