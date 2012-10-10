namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Funding Detail Model for non-pivoted data
    /// </summary>
    /// <remarks>This is used to hold Funding Detail data in normalized format. -DBD 03/04/2010</remarks>
    public class FundingDetailModel : VersionModel
    {
        //I am envisioning this as a single funding detail record. -DBD 03/04/2010
        public int? ProjectFinancialRecordID { get; set; }

        public int? FundingTypeID { get; set; }
        public string FundingType { get; set; }
        public int? FundingLevelID { get; set; }
        public string FundingLevel { get; set; }
        public int FundingLevelOrder { get; set; }

        public double? FederalAmount { get; set; }
        public double? StateAmount { get; set; }
        public double? LocalAmount { get; set; }

        //public double? Amount { get; set; }
        //public double? StateWideAmount { get; set; }
    }
}
