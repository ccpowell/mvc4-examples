using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class FundingResource
    {
        public FundingResource()
        {
            this.ProjectPools = new List<ProjectPool>();
            this.ProjectFinancialRecordDetailPhases = new List<ProjectFinancialRecordDetailPhase>();
        }

        public int FundingResourceID { get; set; }
        public int FundingTypeID { get; set; }
        public int ProgramID { get; set; }
        public short FundPeriodID { get; set; }
        public Nullable<int> Temp_PreviousResourceID { get; set; }
        public virtual FundingType FundingType { get; set; }
        public virtual ProgramInstance ProgramInstance { get; set; }
        public virtual ICollection<ProjectPool> ProjectPools { get; set; }
        public virtual ICollection<ProjectFinancialRecordDetailPhase> ProjectFinancialRecordDetailPhases { get; set; }
    }
}
