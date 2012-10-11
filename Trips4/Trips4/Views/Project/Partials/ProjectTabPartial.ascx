
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.TIPProject.TipSummary>" %>
<ul id="tabnav">
    <li class="notab tab-w-image">
        <a href="<%=Url.Action("ProjectList","TIP",new {year=Model.TipYear}) %>"><% var url = Html.ResolveUrl("~/content/marker/16/previous.png"); %><%=Html.Image(url.ToString(), "RTP Plan List", null) %><%= Model.TipYear%> Project List</a>
    </li>
    <li ><a <%=Html.IsActionCurrent("Details") ? "class='activetab'" : "" %> href="<%= Url.Action("Details", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Project Description</a></li>
	<li ><a <%=Html.IsActionCurrent("Info") ? "class='activetab'" : "" %> href="<%= Url.Action("Info", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">General Info</a></li>
	<li ><a <%=Html.IsActionCurrent("Location") ? "class='activetab'" : "" %> href="<%= Url.Action("Location", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Location</a></li>
	<li ><a <%=Html.IsActionCurrent("Scope") ? "class='activetab'" : "" %> href="<%= Url.Action("Scope", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Scope</a></li>
	<li ><a <%=Html.IsActionCurrent("Funding") ? "class='activetab'" : "" %> href="<%= Url.Action("Funding", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Funding</a></li>
	<li ><a <%=Html.IsActionCurrent("Amendments") ? "class='activetab'" : "" %> href="<%= Url.Action("Amendments", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Amendments</a></li>
	<%--<li ><a <%=Html.IsActionCurrent("Modeling") ? "class='activetab'" : "" %> href="<%= Url.Action("Modeling", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Modeling</a></li>--%>
    <%--<% if(Request.IsAuthenticated && (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))) { %>
        <li ><a <%=Html.IsActionCurrent("CDOTData") ? "class='activetab'" : "" %> href="<%= Url.Action("CDOTData", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">CDOT Data</a></li>
    <% } %>--%>
	<%--<li ><a <%=Html.IsActionCurrent("Strikes") ? "class='activetab'" : "" %> href="<%= Url.Action("Strikes", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Strikes</a></li>
	<li ><a <%=Html.IsActionCurrent("Reports") ? "class='activetab'" : "" %> href="<%= Url.Action("Reports", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Reports</a></li>--%>
</ul>
