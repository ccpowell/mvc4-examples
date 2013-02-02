<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.TIPProject.TipSummary>" %>

<div>
    <a href="<%= Url.Action("TipList", "TIP", new { tipYear = Model.TipYear }) %>">
        <%= Html.Image(Url.Content("~/content/marker/16/previous.png"), "TIP List", null) %>
        TIP List</a>
</div>

<%
    bool isAdmin = HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator");
 %>

<div id="page-tabs">
    <ul id="page-tabs-list">
        <li data-action="dashboard"><a href="#tab-contents">
            <%= Model.TipYear %>
            Dashboard</a></li>
        <li data-action="projectsearch"><a href="#tab-contents">Project Search</a></li>
        <li data-action="projectlist"><a href="#tab-contents">Projects</a></li>
        <% if (isAdmin) { %>
        <li data-action="amendments"><a href="#tab-contents">Amendments</a></li>
        <% } %>
        <li data-action="reports"><a href="#tab-contents">Reports</a></li>
        <% if (isAdmin) { %>
        <li data-action="agencies"><a href="#tab-contents">Sponsors</a></li>
        <li data-action="fundinglist"><a href="#tab-contents">Funding Sources</a></li>
        <li data-action="status"><a href="#tab-contents">TIP Status</a></li>
        <li data-action="delays"><a href="#tab-contents">Delays</a></li>
        <% } %>
    </ul>
    <div id="tab-contents">
    </div>
</div>
