<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.ViewModels.TIP.FundingSourceViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Funding Source Details</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
    <h2>Funding Source Details </h2>
     <% using (Html.BeginForm("UpdateInfo", "Project", FormMethod.Post, new { @id = "infoForm" })) %>
    <%{ %>
        <%= Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
        <%= Html.Hidden("TipYear", Model.TipYear)%>
        
        This form has yet to be drawn. -DBD
        
        
     <%} %>
     <a id="Save" class="fg-button ui-priority-primary ui-state-disabled fg-button-icon-left ui-corner-all" href="#">
     <span class="ui-icon ui-icon-disk"></span>Save</a>
     <a  class="fg-button ui-state-default ui-priority-secondary fg-button-icon-left ui-corner-all" href="<%=Url.Action("FundingList","TIP", new {tipYear=Model.TipYear}) %>">
     <span class="ui-icon ui-icon-close"></span>Cancel</a>
     
     
     <div class="clear"></div>   
</div>
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
