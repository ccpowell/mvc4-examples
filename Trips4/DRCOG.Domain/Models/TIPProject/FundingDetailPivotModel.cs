using System.Collections.Generic;
using System.Data;

namespace DRCOG.Domain.Models.TIPProject
{
    /// <summary>
    /// Funding Detail Model for pivoted data
    /// </summary>
    /// <remarks>This is used to hold Funding Detail data in non-normalized format 
    /// (Example: for grid-based reports). -DBD 03/04/2010</remarks>
    public class FundingDetailPivotModel : VersionModel
    {
        //public int? ProjectFinancialRecordID { get; set; }
        public IList<FundingIncrement> FundingIncrements { get; set; }
        public IList<FundingDetailModel> FundingDetails { get; set; }
        public DataTable FundingDetailTable { get; set; }
    }
}
