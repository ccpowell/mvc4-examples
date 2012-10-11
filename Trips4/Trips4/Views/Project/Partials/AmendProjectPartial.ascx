<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DRCOG.Domain.ViewModels.TIPProject.AmendmentsViewModel>" %>

<h2>Create an Amendment</h2>
<div class="error" style="display:none;">
  <span></span>.
</div>
<div id="AmendmentForm" title="Amend Project">
<% using (Html.BeginForm("Amend", "Project", FormMethod.Post, new { @id = "dataForm" })) %>
        <% { %>
            <fieldset>
            <%=Html.ValidationSummary("Unable to amend. Please correct the errors and try again.")%>
            <%= Html.Hidden("ProjectAmendments.ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
            <%= Html.Hidden("ProjectAmendments.PreviousProjectVersionId", Model.ProjectSummary.PreviousVersionId)%>
                
           <p>
                <label for="ProjectAmendments_AmendmentReason">Reason:</label>
                <%= Html.TextArea("ProjectAmendments.AmendmentReason")%>
           </p>
           <p>
                <label for="ProjectAmendments_AmendmentCharacter">Amendment Character:</label>
                <%= Html.TextArea("ProjectAmendments.AmendmentCharacter")%>
           </p>
           <p>
                <label for="AmendmentTypes">Amendment Type:</label>
                <%= Html.DropDownListFor(x => x.ProjectAmendments.AmendmentTypeId, new SelectList(Model.AmendmentTypes, "Key", "Value"), new { @class = "mediumInputElement big" })%>
            </p>
                <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primarystate-enabled ui-corner-all" >Process</button>
                <button type="button" id="prefillForm" class="fg-button ui-state-default ui-priority-primarystate-enabled ui-corner-all" >AutoFill</button>
            </fieldset>
        <%} %>
</div>

<script type="text/javascript" charset="utf-8">

    $(document).ready(function () {
        var $amendReason, $amendCharacter, $amendType, $amendForm
        , $amendReasonVal, $amendCharacterVal, $amendTypeVal;

        $amendForm = $("#AmendmentForm form");
        $amendReason = $amendForm.find("textarea#ProjectAmendments_AmendmentReason");
        $amendCharacter = $amendForm.find("textarea#ProjectAmendments_AmendmentCharacter");
        $amendType = $amendForm.find("select#ProjectAmendments_AmendmentTypeId");

        // save the values
        $amendReasonVal = $amendReason.val();
        $amendCharacterVal = $amendCharacter.val();
        $amendTypeVal = $amendType.val();

        // clear values
        $amendReason.val("");
        $amendCharacter.val("");
        $amendType.val(0);

        $("button#prefillForm").unbind("click").bind("click", function () {
            $amendReason.val($amendReasonVal);
            $amendCharacter.val($amendCharacterVal);
            $amendType.val($amendTypeVal);
        });
    });

    
</script>