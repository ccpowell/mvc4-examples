
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
<ul id="tabnav">
    <li class="notab tab-w-image"><a href="<%=Url.Action("ProjectList","Survey",new {year=Model.Current.Name}) %>"><% var url = Html.ResolveUrl("~/content/marker/16/previous.png"); %><%=Html.Image(url.ToString(), "Survey Project List", null) %><%= Model.Current.Name%> Project List</a></li>
	<li ><a <%=Html.IsActionCurrent("Info") ? "class='activetab'" : "" %> href="<%= Url.Action("Info", "Survey", new { year = Model.Current.Name, id = Model.Project.ProjectVersionId }) %>">General Info</a></li>
	<li ><a <%=Html.IsActionCurrent("Scope") ? "class='activetab'" : "" %> href="<%= Url.Action("Scope", "Survey", new { year = Model.Current.Name, id = Model.Project.ProjectVersionId }) %>">Scope Details</a></li>
	<%--<li ><a <%=Html.IsActionCurrent("Funding") ? "class='activetab'" : "" %> href="<%= Url.Action("Funding", "Survey", new { year = Model.Current.Name, id = Model.Project.ProjectVersionId }) %>">Funding</a></li>
	<li ><a <%=Html.IsActionCurrent("Location") ? "class='activetab'" : "" %> href="<%= Url.Action("Location", "Survey", new { year = Model.Current.Name, id = Model.Project.ProjectVersionId }) %>">Location</a></li>--%>
	
</ul>



</div>
