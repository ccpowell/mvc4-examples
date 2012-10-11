<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>

<h2 id="breadCrumbs">
  <%--<%=Html.ActionLink("RTP List", "Index", new {controller="RTP"}) %> 
/ <%= Html.ActionLink("RTP " + Model.RtpYear, "Dashboard", new { controller = "RTP", year = Model.RtpYear })%> 
/ <%=Html.ActionLink("Search", "ProjectSearch", new { controller = "RTP", year = Model.RtpYear })%> 
/ Project: <%= Model.RtpId%>--%>
<%= Model.ProjectName %><% if( Model.IsActive ) { %> (Adopted)<% } %>
</h2>
