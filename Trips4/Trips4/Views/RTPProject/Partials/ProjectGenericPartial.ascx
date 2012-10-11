<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>
<div id="panel">
<% Html.RenderPartial("~/Views/RTPProject/Partials/ProjectSummaryBoxPartial.ascx", Model); %>
</div>

<% Html.RenderPartial("~/Views/RTPProject/Partials/BreadcrumbPartial.ascx", Model); %>
<div class="clear"></div>
<%Html.RenderPartial("~/Views/RTPProject/Partials/ProjectTabPartial.ascx", Model); %>
