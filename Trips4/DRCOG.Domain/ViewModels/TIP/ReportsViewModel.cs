//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 9/2/2009 11:30:29 AM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.ViewModels.TIP
{
    /// <summary>
    /// </summary>
    public class ReportsViewModel:TipBaseViewModel
    {
        public IDictionary<DateTime, string> AvailableAmendmentDates { get; set; }
        public IList<TipReportProject> CurrentPendingProjects { get; set; }
        public TipReports ReportDetails { get; set; }
        public IDictionary<string, string> AvailableAlopReport { get; set; }


    }
}
