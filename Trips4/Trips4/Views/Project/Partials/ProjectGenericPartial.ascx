<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.TIPProject.TipSummary>" %>
<div id="panel">
<% Html.RenderPartial("~/Views/Project/Partials/ProjectSummaryBoxPartial.ascx", Model); %>
</div>

<% Html.RenderPartial("~/Views/Project/Partials/BreadcrumbPartial.ascx", Model); %>
<div class="clear"></div>
<%Html.RenderPartial("~/Views/Project/Partials/ProjectTabPartial.ascx", Model); %>
