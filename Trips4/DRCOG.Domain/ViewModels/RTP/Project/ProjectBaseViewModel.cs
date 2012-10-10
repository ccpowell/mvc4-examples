
using System;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP.Project
{
    /// <summary>
    /// Base class for all Project related Views
    /// </summary>
    public class ProjectBaseViewModel
    {
        /// <summary>
        /// Basic information needed across the Project Views.
        /// </summary>
        public RtpSummary ProjectSummary { get; set; }


    }

}
