//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/23/2009 1:46:49 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Entities;
namespace DRCOG.Models
{
    /// <summary>
    /// </summary>
    public class DashboardListModel
    {
        public IList<DashboardListItem> DashboardItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string ErrorMessage { get; set; }
        public string ListType { get; set; }

    }
}
