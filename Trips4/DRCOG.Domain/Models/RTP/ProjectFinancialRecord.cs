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

namespace DRCOG.Domain.Models.RTP
{
    [Obsolete("No Applicable", true)]
    public class ProjectFinancialRecord : RtpVersionModel
    {
        public ProjectFinancialRecordDetail ProjectFinancialRecordDetail { get; set; }
        
        public Int32 ProjectFinancialRecordId { get; set; }
        public Int32? FundPeriodId { get; set; }
        public Decimal? Previous { get; set; }
        public Decimal? Future { get; set; }
        public Decimal? TIPFunding { get; set; }
        public Decimal? FederalTotal { get; set; }
        public Decimal? StateTotal { get; set; }
        public Decimal? LocalTotal { get; set; }
        public Decimal? TotalCost { get; set; }
        public Int32? Temp_PreviousAmendId { get; set; }
    }
}