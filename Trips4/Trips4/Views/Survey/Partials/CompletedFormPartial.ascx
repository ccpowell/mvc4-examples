<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.Survey.Project>" %>

<div id='completed-form' style='padding:10px; background:#fff;'>
    <h2>Mark your project as completed</h2>
    <label>What year did your project open to the public?</label>
    <%= Html.DrcogTextBox("opentopublic", true, Model.EndConstructionYear, new { style = "width:200px;", @class = "nobind" })%>
    
    <div class="dialog-result" style="display:none;">
      <span></span>.
    </div>
</div>