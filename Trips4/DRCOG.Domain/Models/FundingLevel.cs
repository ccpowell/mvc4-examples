namespace DRCOG.Domain.Models
{
    /// <summary>
    /// The Funding Level (Local/State/Federal/Flexed)
    /// </summary>
    /// <remarks>This *could* be an Enum, but we're not 
    /// 100% sure that the ID's will not change</remarks>
    public class FundingLevel
    {
        /// <summary>
        /// The ID
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// The Level  (Local/State/Federal/Flexed)
        /// </summary>
        public string Name { get; set; }

    }
}
