<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.TIPProject.TipSummary>" %>
<ul id="tabnav">
    <%if (!Html.IsActionCurrent("TipList"))
      { %><li class="notab tab-w-image"><a href="<%=Url.Action("TipList", "TIP",new {tipYear=Model.TipYear}) %>">
          <% var url = Html.ResolveUrl("~/content/marker/16/previous.png"); %><%=Html.Image(url.ToString(), "TIP List", null) %>TIP
          List</a></li>
    <% } %>
    <li><a <%=Html.IsActionCurrent("Dashboard") ? "class='activetab'" : "" %> href="<%=Url.Action("Dashboard","TIP",new {tipYear=Model.TipYear}) %>">
        <%= Model.TipYear %>
        Dashboard</a></li>
    <li><a <%=Html.IsActionCurrent("ProjectSearch") ? "class='activetab'" : "" %> href="<%=Url.Action("ProjectSearch","TIP", new {tipYear=Model.TipYear}) %>">
        Project Search</a></li>
    <li><a <%=Html.IsActionCurrent("ProjectList") ? "class='activetab'" : "" %> href="<%=Url.Action("ProjectList","TIP", new {tipYear=Model.TipYear}) %>">
        Projects</a></li>
    <% if (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))
       { %>
    <li><a <%=Html.IsActionCurrent("Amendments") ? "class='activetab'" : "" %> href="<%=Url.Action("Amendments","TIP", new {tipYear=Model.TipYear}) %>">
        Amendments</a></li>
    <% } %>
    <li>
        <% if (Request.IsAuthenticated)
           { %>
        <a <%=Html.IsActionCurrent("Reports") ? "class='activetab'" : "" %> href="<%=Url.Action("Reports","TIP", new {tipYear=Model.TipYear}) %>">
            Reports</a>
        <% }
           else
           { %>
        <a href="http://www.drcog.org/index.cfm?page=TransportationImprovementProgram%28TIP%29"
            target="_blank" title="TIP Reports">Reports</a>
        <% } %>
    </li>
    <% if (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))
       { %>
    <li><a <%=Html.IsActionCurrent("Agencies") ? "class='activetab'" : "" %> href="<%=Url.Action("Agencies","TIP", new {tipYear=Model.TipYear}) %>">
        Sponsors</a></li>
    <%--<li ><a <%=Html.IsActionCurrent("PoolList") ? "class='activetab'" : "" %> href="<%=Url.Action("PoolList","TIP", new {tipYear=Model.TipYear}) %>">Pools</a></li>--%>
    <li><a <%=Html.IsActionCurrent("FundingList") ? "class='activetab'" : "" %> href="<%=Url.Action("FundingList","TIP", new {tipYear=Model.TipYear}) %>">
        Funding Sources</a></li>
    <li><a <%=Html.IsActionCurrent("Status") ? "class='activetab'" : "" %> href="<%=Url.Action("Status","TIP", new {tipYear=Model.TipYear}) %>">
        TIP Status</a></li>
    <li><a <%=Html.IsActionCurrent("Delays") ? "class='activetab'" : "" %> href="<%=Url.Action("Delays","TIP") %>">
        Delays</a></li>
    <% } %>
</ul>
