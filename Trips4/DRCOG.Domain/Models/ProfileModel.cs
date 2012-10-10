//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/21/2009 3:31:44 PM
// Description:
//
//======================================================
using System;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Model used by the My Profile page
    /// </summary>
    public class ProfileModel
    {
        public Person CurrentUser { get; set; }
    }
}
