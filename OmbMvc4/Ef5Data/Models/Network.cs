using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Network
    {
        public Network()
        {
            this.ProjectSegments = new List<ProjectSegment>();
        }

        public int NetworkID { get; set; }
        public string Network1 { get; set; }
        public Nullable<int> cycleId { get; set; }
        public Nullable<System.Guid> networkGuid { get; set; }
        public string Staging { get; set; }
        public virtual Cycle Cycle { get; set; }
        public virtual ICollection<ProjectSegment> ProjectSegments { get; set; }
    }
}
