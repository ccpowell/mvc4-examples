<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<RoleDialogModel>" %>

<div>
    <fieldset>
        <div class="standardInput">          
            <label class="label_beside" for="roleDialog_Role">Role:</label>
            <%=Html.DropDownList("roleDialog_Role", ViewData.Model.GetRolesSelectList()) %>
        </div>
        
        <div class="dialogControls">
            <hr />
            <span class="dialogCancelBtn">Cancel</span>
            <input id="saveRoleBtn" class="dialogButton" type="button" disabled="disabled" value="Save" />
        </div>
    </fieldset>
</div>
