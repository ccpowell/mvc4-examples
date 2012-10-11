<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<h2>Step 2: Process Cycle</h2>
<h3 class="boldFont">Select Projects to be amended</h3>
<br />
<fieldset>
    <table>
        <tr>
            <td>Available Projects:</td>
            <td>&nbsp;</td>
            <td>Selected Projects:</td>
        </tr>
        <tr>
            <td>
                <select id="availableProjects" class="w400 nobind" size="10" multiple="multiple">.</select>
            </td>
            <td>
                <a href="#" id="addProject" title="Add Project"><img src="<%=ResolveUrl("~/content/images/24-arrow-next.png")%>" /></a><br />
                <a href="#" id="removeProject" title="Remove Project"><img src="<%=ResolveUrl("~/content/images/24-arrow-previous.png")%>" /></a><br />
            </td>
            <td>
              <select id="selectedProjects" name="CycleAmendment.SelectedProjects" class="w400 nobind" size="10" multiple="multiple"></select>
            </td>
        </tr>
    </table>
</fieldset>
<div class="dialog-result" style="display:none;">
  <span></span>.
</div>
<div class="dialog-bottom">
    <span id="button-process-cycle" class="cboxBtn">Create</span>
    <div class="button-process-cycle-close" id="cboxClose"/>
</div>