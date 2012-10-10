
using System;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.RTP;
using System.Collections.Generic;

namespace DRCOG.Domain.ViewModels.RTP
{
    /// <summary>
    /// </summary>
    public class AmendmentsViewModel : RtpBaseViewModel
    {
        public InfoModel InfoModel { get; set; }
        public ProjectAmendments ProjectAmendments { get; set; }
        public IList<ProjectAmendments> AmendmentList;

    }
}
