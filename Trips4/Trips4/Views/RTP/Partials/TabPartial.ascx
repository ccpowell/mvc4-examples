<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>
<ul id="tabnav">
    <%if (!Html.IsActionCurrent("Index")) { %><li class="notab tab-w-image"><a href="<%=Url.Action("Index",new {controller="RTP"}) %>"><% var url = Html.ResolveUrl("~/content/marker/16/previous.png"); %><%=Html.Image(url.ToString(), "RTP Plan List", null) %>Plans</a></li> <% } %>
	<li ><a <%=Html.IsActionCurrent("Dashboard") ? "class='activetab'" : "" %> href="<%=Url.Action("Dashboard","RTP",new {year=Model.RtpYear}) %>"><%= Model.RtpYear%> Breakdown</a></li>
	<li ><a <%=Html.IsActionCurrent("ProjectSearch") ? "class='activetab'" : "" %> href="<%=Url.Action("ProjectSearch","RTP", new {year=Model.RtpYear}) %>">Project Search</a></li>
	<% if ( String.IsNullOrEmpty(Model.Cycle.Id.ToString()) || Model.Cycle.Id == 0 ) { %><li class="faketab">Projects</li> <% } else { %><li ><a <%=Html.IsActionCurrent("ProjectList") ? "class='activetab'" : "" %> href="<%=Url.Action("ProjectList","RTP", new {year=Model.RtpYear}) %>">Projects</a></li> <% } %>
	<li ><a <%=Html.IsActionCurrent("Reports") ? "class='activetab'" : "" %> href="<%=Url.Action("Reports","RTP", new {year=Model.RtpYear}) %>">Reports</a></li>
    <% if (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator")) { %>
	    <li><a <%=Html.IsActionCurrent("Agencies") ? "class='activetab'" : "" %> href="<%=Url.Action("Agencies","RTP", new {year=Model.RtpYear}) %>">Sponsors</a></li>
	    <li><a <%=Html.IsActionCurrent("FundingList") ? "class='activetab'" : "" %> href="<%=Url.Action("FundingList","RTP", new {year=Model.RtpYear}) %>">Funding Sources</a></li>
	    <li ><a <%=Html.IsActionCurrent("Status") ? "class='activetab'" : "" %> href="<%=Url.Action("Status","RTP", new {year=Model.RtpYear}) %>">RTP Status</a></li>
	<% } %>
</ul>
