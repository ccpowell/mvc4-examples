//======================================================
#region  DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/3/2009 12:55:58 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Web.Security;

namespace DRCOG.Interfaces
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
    }
}
