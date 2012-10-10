using System;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.ViewModels.TIP
{

    public class ProjectBaseViewModel : TipBaseViewModel
    {
        /// <summary>
        /// This is the basic information model for read/write access
        /// </summary>
        public TipProjectVersionInfoModel TipProjectVersionInfo { get; set; }

        /// <summary>
        /// This is additional summary information required in the
        /// views - this should be treated as ReadOnly 
        /// </summary>
        public Summary Summary { get; set; }
    }
}
