//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/15/2009 8:36:04 AM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;

namespace DRCOG.Domain.Models
{
    
    /// <summary>
    /// Detail for a Project
    /// </summary>
    public class ProjectModel_Dep
    {
        public string TIPYear { get; set; }
        public int TIPID { get; set; }
        public int ProjectId { get; set; }

        public string Title { get; set; }
        public IList<AmendmentSummary> Amendments { get; set; }
    }
}
