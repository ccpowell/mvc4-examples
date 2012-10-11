<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DRCOG.Domain.Security.ChangePasswordModel>" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Change Password</h2>
    <p>
        Use the form below to change your password. 
    </p>
    <p>
        New passwords are required to be a minimum of <%=Html.Encode(ViewData["PasswordLength"])%> characters in length.
    </p>
    <%= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.")%>

    <% using (Html.BeginForm()) { %>
        <div>
        <%= Html.HiddenFor(x => x.UserName) %>
            <fieldset>
                <legend>Account Information</legend>
                <p>
                    <label for="currentPassword">Current password:</label>
                    
                    <%= Html.PasswordFor(x => x.OldPassword) %>
                    <%= Html.ValidationMessageFor(x => x.OldPassword) %>
                </p>
                <p>
                    <label for="newPassword">New password:</label>
                    <%= Html.PasswordFor(x => x.NewPassword) %>
                    <%= Html.ValidationMessageFor(x => x.NewPassword) %>
                </p>
                <p>
                    <label for="confirmPassword">Confirm new password:</label>
                    <%= Html.PasswordFor(x => x.ConfirmPassword) %>
                    <%= Html.ValidationMessageFor(x => x.ConfirmPassword)%>
                </p>
                <p>
                    <input type="submit" value="Change Password" />
                </p>
            </fieldset>
        </div>
    <% } %>
</asp:Content>
