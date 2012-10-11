<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.ErrorViewModel>" %>
<div id="errorDisplay">
    <%=ViewData.Model.MessageBody%>
</div>