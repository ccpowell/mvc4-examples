//======================================================
#region  DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/3/2009 9:59:42 AM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>    
    public interface IAccountRepository : IBaseRepository
    {

        ///// <summary>
        ///// Authorizes the user.
        ///// </summary>
        ///// <param name="username">The username.</param>
        ///// <param name="password">The password.</param>
        ///// <returns>Returns the user if exists in the repository; otherwise returns <c>null</c>.</returns>
        //Account AuthorizeUser(string username, string password);

        ///// <summary>
        ///// Gets the list of roles for a specific account
        ///// </summary>
        ///// <param name="accountId"></param>
        ///// <returns></returns>
        //IList<Role> GetRolesForAccount(int accountId);

        ///// <summary>
        ///// Gets an account instance using the user's email address.
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns>Account</returns>
        //Account GetAccountBy(string email);

        ///// <summary>
        ///// Changes the password to the specified string.
        ///// </summary>
        ///// <param name="accountId">The id of the account.</param>
        ///// <param name="newPassword">The new password.</param>
        //void ChangePassword(int accountId, string newPassword);


        ///// <summary>
        ///// Gets the roles from the repository.
        ///// </summary>
        ///// <returns>Returns a list of roles.</returns>
        //IList<Role> GetRoles();

        /////// <summary>
        /////// Gets the role from the repository.
        /////// </summary>
        /////// <param name="userId">The role ID.</param>
        /////// <returns>Returns a role.</returns>
        ////Role GetRoleById(int roleId);

        /////// <summary>
        /////// Gets the user and their roles from the repository.
        /////// </summary>
        /////// <param name="username">The username.</param>
        /////// <param name="password">The password.</param>
        /////// <returns>Returns a user and their roles.</returns>
        ////Account GetUser(string username, string password);

        ///// <summary>
        ///// Gets the user and their roles from the repository.
        ///// </summary>
        ///// <param name="userId">The user ID.</param>
        ///// <returns>Returns a user and their roles.</returns>
        //Account GetUserById(int userId);

  



        /////// <summary>
        /////// Checks if this user exists in the repository.
        /////// </summary>
        /////// <param name="username">The username.</param>
        /////// <param name="password">The password.</param>
        /////// <returns>
        /////// 	<c>true</c> if the specified user exists in the repository; otherwise, <c>false</c>.
        /////// </returns>
        ////bool IsUser(string username, string password);

        /////// <summary>
        /////// Determines whether user is in admin role.
        /////// </summary>
        /////// <param name="username">The username.</param>
        /////// <param name="password">The password.</param>
        /////// <returns>
        /////// 	<c>true</c> if specified user is in admin role; otherwise, <c>false</c>.
        /////// </returns>
        ////bool IsAdministrator(string username, string password);

        /////// <summary>
        /////// Determines whether user is in admin role.
        /////// </summary>
        /////// <param name="user">The user object.</param>
        /////// <returns>
        /////// 	<c>true</c> if specified user is in admin role; otherwise, <c>false</c>.
        /////// </returns>
        ////bool IsAdministrator(Account user);


        ///// <summary>
        ///// Search for accounts based on the type
        ///// </summary>
        ///// <param name="search_type">Contact or Login</param>
        ///// <param name="search_FirstName"></param>
        ///// <param name="search_LastName"></param>
        ///// <param name="search_email"></param>
        ///// <param name="page"></param>
        ///// <returns></returns>
        //IList<Account> SearchForAccounts(string search_type, string search_FirstName, string search_LastName, string search_email);

        
        /////// <summary>
        /////// Updates user properties in the repository.
        /////// </summary>
        /////// <param name="loginName">Name of the user login.</param>
        /////// <param name="passwordHash">The password hash.</param>
        /////// <param name="userName">Name of the user.</param>
        /////// <param name="agency">The agency.</param>
        /////// <param name="roleIds">The role ids the user should be a member of.</param>
        /////// <param name="active">if set to <c>true</c> [active].</param>
        /////// <returns>
        /////// The User object with roles fetched from the repository.
        /////// </returns>
        ////Account UpdateUser(string loginName, string passwordHash, string firstName, string lastName, string agency, int[] roleIds, bool active);



        /////// <summary>
        /////// Deletes the user and their user roles from the repository.
        /////// </summary>
        /////// <param name="user">The user.</param>
        /////// <returns></returns>
        ////bool DeleteUser(Account user);

        /////// <summary>
        /////// Sets the user activation.
        /////// </summary>
        /////// <param name="userActivationList">The user activation list.</param>
        /////// <returns></returns>
        ////bool SetUserActivation(Dictionary<int, bool> userActivationList);


        ///// <summary>
        ///// Resets the password to a random string.
        ///// </summary>
        ///// <param name="accountId">The id of the account.</param>
        ///// <returns>The new password.</returns>
        //string ResetPassword(int accountId);


        ///// <summary>
        ///// Gets the map toolbar containing those menus and menu items available to the current user based on his or her role(s).
        ///// </summary>
        ///// <param name="user">The user.</param>
        ///// <param name="rootUrl">The root URL.</param>
        ///// <param name="isIE"><c>true</c> if the current browser is Internet Explorer; otherwise <c>false</c>.</param>
        ///// <returns></returns>
        ////Toolbar GetMapToolbar(User user, string rootUrl, bool isIE);
    }
}
