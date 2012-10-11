<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AccountDetailModel>" %>

<% if (ViewData.Model.AccountDetail != null)
{ %>
    <div id="detailHeader" class="headerFooter">
        
        <span id="detail_FullName"><%= Model.GetFullName()%></span>       
        
        <%=Html.Button("detail_SaveButton", "Save Changes", false, new { _class="detailSaveButton" }) %>
        
        
    </div>
    <%=Html.Hidden("detail_CanEdit", Model.CanEdit) %> <%--Hidden field to store canedit state because with xhr, the state will change after the accountDetailController is instantiated--%>
    <%--<%=Html.Hidden("detail_AccountId", Model.AccountDetail.AccountId)%> <%--Hidden field to store customerid because with xhr, the state will change after the accountDetailController is instantiated--%>--%>
    <div class="contentBox">
        <div class="contentBoxHeader">Basic Information</div>
            <div class="contentBoxContent">
                <label class="label_top" for="detail_FirstName">First Name:</label>
                <%= Html.DrcogTextBox("detail_FirstName", false, Model.AccountDetail.profile.FirstName, new { dojoType = "dijit.form.TextBox", trim = "true", properCase = "true" })%><br />
                <label class="label_top" for="detail_LastName">Last Name:</label>
                <%= Html.DrcogTextBox("detail_LastName", false, Model.AccountDetail.profile.LastName, new { dojoType = "dijit.form.TextBox", trim = "true", properCase = "true" })%><br />
                <label class="label_top" for="detail_Email">Email Address:</label>
                <%= Html.DrcogTextBox("detail_Email", false, Model.AccountDetail.profile.RecoveryEmail, new { dojoType = "dijit.form.TextBox", trim = "true" })%><br />
                <%-- if (ViewData.Model.CurrentUser.IsAdministrator()) { --%>
                <% if (true) { %>
                <div id="resetPasswordLink" dojoType="dijit.form.DropDownButton">
                    <span>Reset password</span>    
                    <div id="resetPwdDlg" dojoType="dijit.TooltipDialog">
                        Are you sure you want to reset the password?
                        <button id="resetPasswordButton" dojoType="dijit.form.Button">OK</button>
                        <button id="resetPasswordCancelButton" dojoType="dijit.form.Button">Cancel</button>
                    </div>
                </div><br />
                <%}
                else
                { %>
                <%=Html.ActionLink("Change password", "ChangePassword", null, new { id = "changePasswordLink" })%>
                <% } %>
                
                <div id="isActiveContainer">
                    <%--<%= Html.CheckBox("detail_IsActive", ViewData.Model.CurrentUser.IsAdministrator(), ViewData.Model.AccountDetail.Active, new { @class = "checkbox" })%>--%>
                    <label id="isActiveLabel" class="label_beside" for="detail_IsActive">Enable Contact Login</label>
                </div>
            </div>
        <div class="contentBoxInstructions">
            Administrators can only activate or deactivate Accounts associted with Contacts. The current system does 
            not include functionality to edit the details of a Contact. Click the reset password link to 
            change your password. 
        </div>
    </div>
    
   
    
    <div class="contentBox">
        <div class="contentBoxHeader">Roles</div>
            <div class="contentBoxContent">
                <%--<%=Html.DojoDataGrid("detail_RolesGrid", 
                    new DRCOG.Domain.Models.RolesDojoDataStore("roleId", "name", Model.AccountDetail.Roles),
                    "<thead><tr><th formatter=\"accountDetailController._formatDeleteGridCell\" width=\"50px\" >Delete</th><th width=\"auto\" field=\"name\">Role</th></tr></thead>",
                    4, true /*Model.CurrentUser.IsAdministrator()*/, new { selectionMode = "none" })%>--%>
                <%=Html.Button("addRoleButton", "Add", false, new { _class = "detailButton" })%>
            </div>
        <div class="contentBoxInstructions">
            Click the Add button to add a role for this user. Click the delete icon next to a role name to remove a role for this user. Users must be assigned to at least one role.
        </div>
    </div>
    
    
    <div  id="detailFooter" class="headerFooter">
        <%=Html.Button("detail_SaveButton2", "Save Changes", false, new { _class="detailSaveButton" })%>
    </div>    
<%}
else
{ %>
    <%--<p>User not set!</p>--%>
<% } %>

<script type="text/javascript">
    params = {};
    //params.newCustomerUrl = '<%= Url.Action("NewCustomer", "Account") %>';
    params.accountDetailUrl = '<%= Url.Action("Detail", "Account") %>';
    params.saveUserUrl = '<%= Url.Action("SaveUser", "Account") %>';
    params.resetPasswordUrl = '<%= Url.Action("ResetPassword", "Account") %>';
    params.roleDialogUrl = '<%= Url.Action("GetRoleDialog", "Account") %>';
    //params.orgDialogUrl = '<%= Url.Action("GetOrgDialog", "Account") %>';
    //params.getBranchOfficesUrl = '<%= Url.Action("GetBranchOfficesForOrganization", "Account") %>';
    //params.isAdmin = '<%--= Model.CurrentUser.IsAdministrator() --%>' === 'True';
    accountDetailController = new dts.account.AccountDetailController(params);
</script>

