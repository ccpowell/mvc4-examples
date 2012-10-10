//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/9/2009 4:15:02 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Model for the RoleDialog 
    /// </summary>
    public class RoleDialogModel// : BaseModel
    {

   
        private Dictionary<int, string> _allRoles;

        public RoleDialogModel()
        {
            _allRoles = new Dictionary<int, string>();
        }

        public Dictionary<int, string> RolesList
        {
            set { _allRoles = value; }
        }

        public SelectList GetRolesSelectList()
        {
            return new SelectList(_allRoles.OrderBy<KeyValuePair<int, string>, string>(x => x.Value), "key", "value");
        }
        
    }
}
