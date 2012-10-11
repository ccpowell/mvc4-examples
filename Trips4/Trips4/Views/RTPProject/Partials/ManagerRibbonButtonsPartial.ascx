<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpVersionModel>" %>

<span id="pv-<%= Model.ProjectVersionId %>" style="display: none;"></span>
<% if (Model.CanDrop)
   { %>
    <div class="btn-drop fg-button w125 ui-state-default ui-priority-primary ui-corner-all bg-red" >Drop from Plan</div>
<% } %>

<% if (Model.AmendmentStatusId.Equals((int)DRCOG.Domain.Enums.RTPAmendmentStatus.Cancelled))
   { %>
    <div class="btn-undodrop fg-button w125 ui-state-default ui-priority-primary ui-corner-all bg-red" >Undo Cancel</div>
<% } %>

<div class="clear"></div> 
