using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.Models
{
    public class Cycle
    {
        public int Id { get; set; } //CycleId
        public string Name { get; set; } //Cycle
        public DateTime Date { get; set; }
        public int StatusId { get; set; }
        public int VersionStatusId { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public int PriorCycleId { get; set; }
        public string PriorCycleName { get; set; }
        public string PriorCycleStatus { get; set; }
        public int NextCycleId { get; set; }
        public string NextCycleName { get; set; }
        public string NextCycleStatus { get; set; }

        public IList<CycleAmendment> Projects { get; set; }

        

    }

    public class CycleAmendment : Cycle
    {
        public int AmendmentStatusId { get; set; }
        public int ProjectVersionId { get; set; }
        public int PreviousVersionId { get; set; }
    }



}
