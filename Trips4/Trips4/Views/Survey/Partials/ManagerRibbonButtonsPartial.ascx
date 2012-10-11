<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.Survey.Project>" %>

<span id="pv-<%= Model.ProjectVersionId %>" style="display: none;"></span>
<% if (!Model.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Completed))
   { %>
    
    <% if (!Model.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Reviewed) && !Model.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited) && !Model.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Current))
       { %>
        <div class="btn-reviewed fg-button w75 ui-state-default ui-priority-primary ui-corner-all bg-green" >Reviewed</div>
    <% } %>
    <% if (!Model.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Completed))
       { %>
        <div class="completed fg-button w125 ui-state-default ui-priority-primary ui-corner-all bg-green" >Opened to Public</div>
    <% } %>
<% } %>
<% if (!Model.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Withdrawn))
    { %>
    <div class="btn-withdraw fg-button w75 ui-state-default ui-priority-primary ui-corner-all bg-red" >Withdraw</div>
<% } %>
<% if (Model.UpdateStatusId.Equals((int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited))
    { %>
    <div id="actionbar" class="info" style="display:block;">
        <span>When you have completed all edits, Accept changes here --></span>
        <div class="<% if( Model.IsNew ) { %>btn-current<% } else { %>btn-accept<% } %> fg-button w125 ui-state-default ui-priority-primary ui-corner-all bg-green" >Accept Changes</div>
    </div>
<% } %>

<div class="clear"></div> 
