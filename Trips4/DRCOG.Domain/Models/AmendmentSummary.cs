//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/15/2009 12:14:39 PM
// Description:
//
//======================================================
using System;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// </summary>
    public class AmendmentSummary
    {
        public int AmendmentId { get; set; }
        //public int ProjectId { get; set; }
        //public int TipId { get; set; }
        public ProjectModel Project { get; set; }
        public TipModel Tip { get; set; }
        public string FundPeriod { get; set; }
        public DateTime? Date { get; set; }
        public string AmendmentType { get; set; }
        public bool IsCurrent { get; set; }
        public string Status { get; set; }
    }
}
