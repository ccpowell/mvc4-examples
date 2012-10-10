using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.ViewModels.Survey
{
    public class CreateProjectViewModel : ProjectBaseViewModel
    {
        public IDictionary<int, string> AvailableImprovementTypes { get; set; }
        public DRCOG.Domain.Models.Survey.Survey Survey { get; set; }
    }
}
