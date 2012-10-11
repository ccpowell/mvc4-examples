<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>

<ul id="tabnav">
    <li class="notab tab-w-image">
        <% if (Model.Cycle.StatusId.Equals((int)DRCOG.Domain.Enums.RTPCycleStatus.Active))
           { %>
            <a href="<%=Url.Action("ProjectList","RTP",new {year=Model.RtpYear}) %>">
        <% }
           else
           { %>
            <a href="<%=Url.Action("ProjectList","RTP",new {year=Model.RtpYear, cycleid=Model.Cycle.Id}) %>">
        <% } %>
                <% var url = Html.ResolveUrl("~/content/marker/16/previous.png"); %><%=Html.Image(url.ToString(), "RTP Plan List", null) %><%= Model.RtpYear%> Project List</a></li>
    <li ><a <%=Html.IsActionCurrent("Details") ? "class='activetab'" : "" %> href="<%= Url.Action("Details", "RTPProject", new { year = Model.RtpYear, id = Model.ProjectVersionId }) %>">Project Description</a></li>
	
    <% if(Request.IsAuthenticated && (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))) { %>
        <li ><a <%=Html.IsActionCurrent("Info") ? "class='activetab'" : "" %> href="<%= Url.Action("Info", "RTPProject", new { year = Model.RtpYear, id = Model.ProjectVersionId }) %>">General Info</a></li>
        <li ><a <%=Html.IsActionCurrent("Scope") ? "class='activetab'" : "" %> href="<%= Url.Action("Scope", "RTPProject", new { year = Model.RtpYear, id = Model.ProjectVersionId }) %>">Scope</a></li>
	    <li ><a <%=Html.IsActionCurrent("Funding") ? "class='activetab'" : "" %> href="<%= Url.Action("Funding", "RTPProject", new { year = Model.RtpYear, id = Model.ProjectVersionId }) %>">Plan Info</a></li>
	    <li ><a <%=Html.IsActionCurrent("Location") ? "class='activetab'" : "" %> href="<%= Url.Action("Location", "RTPProject", new { year = Model.RtpYear, id = Model.ProjectVersionId }) %>">Location</a></li>
	<% } %>
	
	<%--<li class="faketab">--%><%--<a <%=Html.IsActionCurrent("Amendments") ? "class='activetab'" : "" %> href="<%= Url.Action("Amendments", "RTPProject", new { year = Model.RtpYear, id = Model.ProjectVersionId }) %>">--%><%--Amendments--%><%--</a>--%><%--</li>--%>
	<%--<li ><a <%=Html.IsActionCurrent("Modeling") ? "class='activetab'" : "" %> href="<%= Url.Action("Modeling", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Modeling</a></li>--%>
	<%--<li ><a <%=Html.IsActionCurrent("CDOTData") ? "class='activetab'" : "" %> href="<%= Url.Action("CDOTData", "RTPProject", new { year = Model.RtpYear, id = Model.ProjectVersionId }) %>">CDOT Data</a></li>--%>
	<%--<li ><a <%=Html.IsActionCurrent("Strikes") ? "class='activetab'" : "" %> href="<%= Url.Action("Strikes", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Strikes</a></li>
	<li ><a <%=Html.IsActionCurrent("Reports") ? "class='activetab'" : "" %> href="<%= Url.Action("Reports", "Project", new { tipYear = Model.TipYear, id = Model.ProjectVersionId }) %>">Reports</a></li>--%>
</ul>
