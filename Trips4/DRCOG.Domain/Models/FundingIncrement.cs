namespace DRCOG.Domain.Models
{
    /// <summary>
    /// The Funding Increment (varies)
    /// </summary>
    /// <remarks>Funding Increment varies for each Program (TIP,RTP).</remarks>
    public class FundingIncrement
    {
        public int? FundingIncrementID { get; set; }
        public string FundingIncrementName { get; set; }
    }
}
