using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.ViewModels.RTP
{
    public class PlanCycle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class PlanCyclesViewModel : RtpBaseViewModel
    {
        public IEnumerable<PlanCycle> Cycles { get; set; }
        public bool ExistsNewPlanCycle { get; set; }
    }
}
