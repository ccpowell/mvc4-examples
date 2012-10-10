using System;
using System.Collections.Generic;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.ViewModels.TIP;

namespace DRCOG.Domain.ViewModels.TIPProject
{
    public class InfoViewModel : ProjectBaseViewModel
    {
        // form specific props
        public IDictionary<int, string> AvailableSponsors { get; set; }
        public IDictionary<int, string> AvailableRoadOrTransitTypes { get; set; }
        public IDictionary<int, string> AvailableSponsorContacts { get; set; }
        public IDictionary<int, string> AvailableAdminLevels { get; set; }
        public IDictionary<int, string> AvailableImprovementTypes { get; set; }
        public IDictionary<int, string> AvailableProjectTypes { get; set; }
        public IDictionary<int, string> AvailablePools { get; set; }
        public IDictionary<int, string> AvailablePoolMasters { get; set; }
        public IDictionary<int, string> AvailableSelectionAgencies { get; set; }

        /// <summary>
        /// The actual data we are working with
        /// </summary>
        public InfoModel InfoModel { get; set; }
        public ProjectSponsorsModel ProjectSponsorsModel { get; set; }
    }

}
