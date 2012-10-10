//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/23/2009 11:19:01 AM
// Description:
//
//======================================================
using System;

namespace DRCOG.Entities
{
    /// <summary>
    /// container class for the dashboard lists
    /// </summary>
    public class DashboardListItem
    {
        //public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int count { get; set; }
        public DateTime? ItemDate { get; set; }
    }
}
