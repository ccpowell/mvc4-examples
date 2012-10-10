#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 08/05/2009	NKirkes        1. Initial Creation (DTS).
 * 02/25/2010   DTucker        2. Modified to hold Detailed Project View
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.ViewModels.TIP;
using System.Collections.Generic;
using DRCOG.Common.Services;

namespace DRCOG.Domain.ViewModels.TIPProject
{
    public class DetailViewModel : ProjectBaseViewModel
    {
        public IList<SegmentModel> Segments { get; set; }
        public IDictionary<string, string> GeneralInfo { get; set; }

        public IDictionary<string, string> StringValues { get; set; }

        public ProjectVersionModelBase projectVersionModelBase { get; set; }
        public InfoModel InfoModel { get; set; }
        public DRCOG.Domain.Models.TIPProject.ProjectSponsorsModel ProjectSponsorsModel { get; set; }
        public ProjectFinancialRecord projectFinancialRecord { get; set; }
        //public TipProjectCdotData tipProjectCdotData { get; set; }
        public MetroVisionMeasureSponsor metroVisionMeasureSponsor { get; set; }
        public IList<PoolProject> PoolProjects { get; set; }

        public IList<ProjectAmendments> AmendmentList;
        public IList<CountyShareModel> CountyShares { get; set; }
        public IList<MunicipalityShareModel> MuniShares { get; set; }

        public FundingDetailPivotModel FundingDetailPivotModel { get; set; }
        public FundingModel TipProjectFunding { get; set; }
    }
}
