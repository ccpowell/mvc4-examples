using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.Models.TIPProject
{
    public class FundingPhase
    {
        private DateTime Today = DateTime.UtcNow;

        public int ProjectFinancialRecordId { get; set; }
        public string Year { get; set; }
        public string Phase { get; set; }
        public string FundingResource { get; set; }

        public int FundingIncrementId { get; set; }
        public int PhaseId { get; set; }
        public int FundingResourceId { get; set; }

        public bool IsInitiated { get; set; }
        
        public DateTime ShowDelayDate { get; set; }
        public bool IsDelayed
        {
            get
            {
                int year;

                if (Int32.TryParse(this.Year, out year))
                {

                    return (
                        (
                            (Today.AddYears(-1).Year == year)
                            &&
                            (Today > ShowDelayDate && ShowDelayDate.Year > year)
                        )
                        &&
                        !IsInitiated
                    ) ? true : false;
                }
                else return false;
            }
        }
    }
}
