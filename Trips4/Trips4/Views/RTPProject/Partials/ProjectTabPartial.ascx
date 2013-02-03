<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>
<% 
    string back = Url.Action("ProjectList", "RTP", new { year = Model.RtpYear, cycleid = Model.Cycle.Id });
    if (Model.Cycle.StatusId.Equals((int)DRCOG.Domain.Enums.RTPCycleStatus.Active))
    {
        back = Url.Action("ProjectList", "RTP", new { year = Model.RtpYear });
    }
    bool isAdmin = HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator");
%>
<div>
    <a href='<%= back %>'>
        <%= Html.Image(Url.Content("~/content/marker/16/previous.png"), "Plans", null) %><%= Model.RtpYear%>
        Project List</a>
</div>
<div id="page-tabs">
    <ul id="page-tabs-list">
        <li data-action="details"><a href="#tab-contents">Project Description</a></li>
        <% if (isAdmin)
           { %>
        <li data-action="info"><a href="#tab-contents">General Info</a></li>
        <li data-action="scope"><a href="#tab-contents">Scope</a></li>
        <li data-action="funding"><a href="#tab-contents">Plan Info</a></li>
        <li data-action="location"><a href="#tab-contents">Location</a></li>
        <% } %>
    </ul>
    <div id="tab-contents">
    </div>
</div>
