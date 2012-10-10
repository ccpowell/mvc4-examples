//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/15/2009 3:13:27 PM
// Description:
//
//======================================================
using System;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.Models
{
    public class AmendmentModel
    {
        public TipSummary TipSummary { get; set; }
        public Summary ProjectSummary { get; set; }

        public int AmendmentId { get; set; }
        public int ProjectId { get; set; }
        public int TipId { get; set; }
        public string Status { get; set; }
        public string FundingSource { get; set; }
        public string Reason {get;set;}
        public string Character {get;set;}
        public string Type { get; set; }
    }
}
