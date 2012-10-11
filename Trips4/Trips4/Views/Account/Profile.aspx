<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ProfileViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">My Profile</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
 <link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="view-content-container">
    <h2>My Profile</h2>
    <p>Use this page to change your password. All other information must be changed by a DRCOG administrator</p>
    <%--<%=Html.Hidden("detail_AccountId", Model.Profile.CurrentUser.AccountId)%> --%>
    <%--Hidden field to store customerid because with xhr, the state will change after the accountDetailController is instantiated--%>
    <div >
        <fieldset>
            <legend>Account Information (Read Only)</legend>
            <p>
            <label class="label_top" for="detail_FirstName">First Name:</label>            
            <p class="readonly_field"><%=Html.Encode(Model.Profile.CurrentUser.profile.FirstName)%></p>  
            </p>
            <p>
            <label class="label_top" for="detail_LastName">Last Name:</label>
            <p class="readonly_field"><%=Html.Encode(Model.Profile.CurrentUser.profile.LastName)%></p>              
            </p>
            <p>
            <label class="label_top" for="detail_Email">Email Address:</label>
            <p class="readonly_field"><%=Html.Encode(Model.Profile.CurrentUser.profile.RecoveryEmail)%></p>            
            </p>
         </fieldset>
        </div>
        <div >
         <% using (Html.BeginForm()) { %>
            <fieldset>
                <legend>Change Password</legend>
                <p>New passwords are required to be a minimum of 8 characters in length.</p>
                <%= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.")%>
            <p>
                <label for="currentPassword">Current password:</label>
                <%= Html.Password("currentPassword") %>
                <%= Html.ValidationMessage("currentPassword") %>
            </p>
            <p>
                <label for="newPassword">New password:</label>
                <%= Html.Password("newPassword") %>
                <%= Html.ValidationMessage("newPassword") %>
            </p>
            <p>
                <label for="confirmPassword">Confirm new password:</label>
                <%= Html.Password("confirmPassword") %>
                <%= Html.ValidationMessage("confirmPassword") %>
            </p>
              
            <p>
                <input type="submit" value="Change Password" />
            </p>
            </fieldset>
            <% } %>
        </div>
        
</div>
<div class="clear"></div>
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="MenuContent" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="FooterContent" runat="server">
</asp:Content>
