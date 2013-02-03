
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.Survey.ProjectBaseViewModel>" %>

<div id="tabnav-container">
<%if (Model.Current.IsAdmin() && !Model.Current.IsOpen()) { %>
<div style="position: absolute; top: 15px; left: 0px; z-index: 1000" class="info">
    <% if (Model.Project.IsInEditMode) { %>
        The current survey is closed. The project is in an editable mode so the project can still be updated by the sponsor contact.
    <% } else { %>
        The current survey is closed. If you want the user to be able to make a change the survey will need to be opened.
    <% } %>
</div>
<% } %>

<div>
<a href="<%=Url.Action("ProjectList","Survey",new {year=Model.Current.Name}) %>"><%=Html.Image(Url.Content("~/content/marker/16/previous.png"), "Survey Project List", null)%><%= Model.Current.Name%> Project List</a>
</div>

<div id="page-tabs">
    <ul id="page-tabs-list">
        <li data-action="info"><a href="#tab-contents">General Info</a></li>
        <li data-action="scope"><a href="#tab-contents">Scope Details</a></li>
    </ul>
    <div id="tab-contents">
    </div>
</div>

</div>
