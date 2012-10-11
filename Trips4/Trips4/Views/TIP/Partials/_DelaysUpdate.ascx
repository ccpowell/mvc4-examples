<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.TIP.Delay>" %>

<% using (Ajax.BeginForm("DelayUpdate", "TIP", null, new AjaxOptions { HttpMethod = "POST" }, new { @id = "form-delayupdate" }))
    { %>

    <%= Html.HiddenFor(x => x.TimePeriod) %>
    <%= Html.HiddenFor(x => x.Year) %>

    <%= Html.HiddenFor(x => x.ProjectFinancialRecordId) %>
    <%= Html.HiddenFor(x => x.FundingResourceId) %>
    <%= Html.HiddenFor(x => x.FundingIncrementId) %>
    <%= Html.HiddenFor(x => x.PhaseId) %>
        
<%= String.Format("<h2>{0} ({1})</h2><h3>{2}: {3} <span style=\"font-weight:bold\">{{ Federal Funding: ${4} }}</span></h3>", Model.ProjectName, Model.TipId, Model.Year, Model.Phase, Model.FederalAmount)%>

<div class="info"><h3>Has this phase been initiated? <span class="editable"><%= Html.CheckBoxFor(x => x.IsInitiated)%></span></h3></div>
        
<p class="editable">
    <%= Html.LabelFor(x => x.MidYearStatus)%>:<br />
    <%= Html.TextAreaFor(x => x.MidYearStatus, new { @class = "resizable", @style = "width:535px; height: 50px; overflow-y: hidden; padding: 10px;" })%>
</p>
<p class="editable">
    <%= Html.LabelFor(x => x.EndYearStatus)%>:<br />
    <%= Html.TextAreaFor(x => x.EndYearStatus, new { @class = "resizable", @style = "width:535px; height: 50px; overflow-y: hidden; padding: 10px;" })%>
</p>
        
<p class="editable">
    <%= Html.LabelFor(x => x.ActionPlan)%>?<br />
    <%= Html.TextAreaFor(x => x.ActionPlan, new { @class = "resizable", @style = "width:535px; height: 50px; overflow-y: hidden; padding: 10px;" })%>
</p>
<p class="editable">
    <%= Html.LabelFor(x => x.MeetingDate)%> <%= Html.EditorFor(x => x.MeetingDate, new { @class = "datepicker", @style = "width:120px; height: 10px;" })%>
</p>

<p class="editable" id="notesContainer">
    <%= Html.LabelFor(x => x.Notes)%>?<br />
    <%= Html.TextAreaFor(x => x.Notes, new { @class = "resizable", @style = "width:535px; height: 50px; overflow-y: hidden; padding: 10px;" })%>
</p>

<p class="editable" id="managementContainer">
    <%= Html.LabelFor(x => x.IsChecked)%> <%= Html.CheckBoxFor(x => x.IsChecked)%>
</p>
<p class="clear">
    <input type="submit" value="Submit" />
</p>
<% } %>

<script type="text/javascript">
$(function () {
    $(".datepicker").datepicker();
});
</script>