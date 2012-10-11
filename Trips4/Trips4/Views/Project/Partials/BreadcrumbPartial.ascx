<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.TIPProject.TipSummary>" %>

<h2 id="breadCrumbs">
<%--  <%=Html.ActionLink("TIP List", "Index", new {controller="TIP"}) %> 
/ <%= Html.ActionLink("TIP " + Model.TipYear, "Dashboard", new { controller = "Tip", year = Model.TipYear })%> 
/ <%=Html.ActionLink("Search", "ProjectSearch", new { controller = "TIP", year = Model.TipYear })%> 
/ Project: <%= Model.TipId%>--%>
<%= Model.ProjectName %> (<%= Model.TipId %>)
</h2>
