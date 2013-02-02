<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.TIPProject.TipSummary>" %>
<h2 id="breadCrumbs">
    <%= Model.ProjectName %>
    (<%= Model.TipId %>)
</h2>
<div>
    <a href="<%=Url.Action("ProjectList","TIP", new { year = Model.TipYear }) %>">
        <%=Html.Image(Url.Content("~/content/marker/16/previous.png"), "TIP Project List", null) %><%= Model.TipYear%>
        Project List</a>
</div>
<div id="page-tabs" style="border-style: none">
    <ul id="page-tabs-list">
        <li data-action="details"><a href="#tab-contents">Project Description</a></li>
        <li data-action="info"><a href="#tab-contents">General Info</a></li>
        <li data-action="location"><a href="#tab-contents">Location</a></li>
        <li data-action="scope"><a href="#tab-contents">Scope</a></li>
        <li data-action="funding"><a href="#tab-contents">Funding</a></li>
        <li data-action="amendments"><a href="#tab-contents">Amendments</a></li>
    </ul>
    <div id="tab-contents">
    </div>
</div>
