<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.Models.RTP.RtpSummary>" %>

<h2>Create an Amendment</h2>
<div class="error" style="display:none;">
  <span></span>.
</div>
<div id="AmendmentForm" title="Amend Project">
<% using (Html.BeginForm("Amend", "Project", FormMethod.Post, new { @id = "dataForm" })) %>
        <% { %>
            <fieldset>
            <%=Html.ValidationSummary("Unable to amend. Please correct the errors and try again.")%>
            <%= Html.Hidden("ProjectAmendments.ProjectVersionId", Model.ProjectVersionId)%>
            <%= Html.Hidden("ProjectAmendments.PreviousProjectVersionId", Model.PreviousVersionId) %>
                
           <p>
                <label for="ProjectAmendments_AmendmentReason">Reason:</label>
                <%= Html.TextArea("ProjectAmendments.AmendmentReason")%>
           </p>
           <p>
                <label for="ProjectAmendments_AmendmentCharacter">Amendment Character:</label>
                <%= Html.TextArea("ProjectAmendments.AmendmentCharacter")%>
           </p>
                <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primarystate-enabled ui-corner-all" >Process</button>
            </fieldset>
        <%} %>
</div>