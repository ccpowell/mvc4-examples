#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 06/17/2010	DTucker         1. Initial Creation.
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Models;
using System.Collections.Generic;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP.Project
{
    public class DetailViewModel : ProjectBaseViewModel
    {
        public IList<SegmentModel> Segments { get; set; }
        public IDictionary<string, string> GeneralInfo { get; set; }

        public IDictionary<string, string> StringValues { get; set; }

        public ProjectVersionModelBase projectVersionModelBase { get; set; }
        public InfoModel InfoModel { get; set; }
        public DRCOG.Domain.Models.RTP.ProjectSponsorsModel ProjectSponsorsModel { get; set; }
        //public ProjectFinancialRecord projectFinancialRecord { get; set; }
        //public TipProjectCdotData tipProjectCdotData { get; set; }
        public MetroVisionMeasureSponsor metroVisionMeasureSponsor { get; set; }
        //public IList<PoolProject> PoolProjects { get; set; }

        public IList<FundingSource> FundingSources { get; set; }

        public PlanReportGroupingCategory GroupingCategory { get; set; }
    }
}
