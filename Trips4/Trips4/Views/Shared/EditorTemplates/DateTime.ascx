<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.DateTime?>" %>
<%=Html.DrcogTextBox("", true, (Model.HasValue ? Model.Value.ToString("MM/dd/yyyy") : string.Empty), ViewData) %>
