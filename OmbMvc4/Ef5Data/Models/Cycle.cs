using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Cycle
    {
        public Cycle()
        {
            this.Cycle11 = new List<Cycle>();
            this.CycleProjectVersions = new List<CycleProjectVersion>();
            this.Networks = new List<Network>();
            this.RTPProjectVersions = new List<RTPProjectVersion>();
            this.TimePeriodCycles = new List<TimePeriodCycle>();
        }

        public int id { get; set; }
        public string cycle1 { get; set; }
        public System.Guid cycleGuid { get; set; }
        public Nullable<int> priorCycleId { get; set; }
        public Nullable<int> statusId { get; set; }
        public virtual ICollection<Cycle> Cycle11 { get; set; }
        public virtual Cycle Cycle2 { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<CycleProjectVersion> CycleProjectVersions { get; set; }
        public virtual ICollection<Network> Networks { get; set; }
        public virtual ICollection<RTPProjectVersion> RTPProjectVersions { get; set; }
        public virtual ICollection<TimePeriodCycle> TimePeriodCycles { get; set; }
    }
}
