#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 02/24/2009	DTucker        1. Initial Creation.
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion
using System;

namespace DRCOG.Domain.Models.TIPProject
{
    public class ProjectFinancialRecordDetail
    {
        public Int32 ProjectFinancialRecordID { get; set; }
        public Int32 FundingIncrementID { get; set; }
        public Int32 FundingResourceID { get; set; }
        public Decimal? FederalAmount { get; set; }
        public Decimal? StateAmount { get; set; }
        public Decimal? LocalAmount { get; set; }

        public Int32 FundingTypeID { get; set; }
        public Int32 FundingLevelID { get; set; }
        public Int32 FundingPeriodID { get; set; }

        public Decimal Incr01 { get; set; }
        public Decimal Incr02 { get; set; }
        public Decimal Incr03 { get; set; }
        public Decimal Incr04 { get; set; }
        public Decimal Incr05 { get; set; }
    }
}
