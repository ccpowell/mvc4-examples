using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.ViewModels.TIPProject
{
    public class AmendmentsViewModel : ProjectBaseViewModel
    {
        public InfoModel InfoModel { get; set; }
        //public TipSummary TipSummary { get; set; }
        public ProjectAmendments ProjectAmendments { get; set; }
        public IList<ProjectAmendments> AmendmentList;
    }
}
