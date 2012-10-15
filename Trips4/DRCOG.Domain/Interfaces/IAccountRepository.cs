using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using System.Web.Security;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Common.Services.MemberShipServiceSupport.Domain;

namespace DRCOG.Domain.Interfaces
{
    /// <summary>
    /// Handles User Profile and Roles using MembershipProvider and RoleProvider and the database if needed.
    /// </summary>    
    public interface IAccountRepository 
    {
        MembershipCreateStatus CreateUserWithProfile(Profile profile, bool isApproved);
        void AddUserToRole(string userName, string role);
        //Profile GetUserByName(string userName, bool loadRoles);
        //Profile GetUserByID(Guid userId, bool loadRoles);
        //Profile GetUserByEmail(string emailAddress, bool loadRoles);
        void UpdateUserApproval(Guid userId, bool isApproved);
        void ChangePassword(Guid userId, string oldPaassword, string newPassword);
        string ResetPassword(Guid userId);
        bool ValidateUser(string userName, string password);
    }
}
