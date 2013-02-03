<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>

<div>
    <a href='<%= Url.Action("Index",new {controller="RTP"}) %>'>
          <%= Html.Image(Url.Content("~/content/marker/16/previous.png"), "Plans", null) %>Plans</a>
</div>

<%
    bool isAdmin = HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator");
 %>

<div id="page-tabs">
    <ul id="page-tabs-list">
        <li data-action="dashboard"><a href="#tab-contents">
            <%= Model.RtpYear %>
            Breakdown</a></li>
        <li data-action="projectsearch"><a href="#tab-contents">Project Search</a></li>
        <li data-action="projectlist"><a href="#tab-contents">Projects</a></li>
        <% if (isAdmin) { %>
        <li data-action="amendments"><a href="#tab-contents">Amendments</a></li>
        <% } %>
        <li data-action="reports"><a href="#tab-contents">Reports</a></li>
        <% if (isAdmin) { %>
        <li data-action="agencies"><a href="#tab-contents">Sponsors</a></li>
        <li data-action="fundinglist"><a href="#tab-contents">Funding Sources</a></li>
        <li data-action="plancycles"><a href="#tab-contents">Plan Cycles</a></li>
        <li data-action="status"><a href="#tab-contents">RTP Status</a></li>
        <% } %>
    </ul>
    <div id="tab-contents">
    </div>
</div>
