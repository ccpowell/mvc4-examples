using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.Models.TIPProject
{
    /// <summary>
    /// Represents a sorted list of TIP Projects and shallow details.
    /// Also will be the placeholder for page number details.
    /// </summary>
    [Serializable]
    public class TipSortedProjectListModel
    {
        public int TipProjectId { get; set; }
        public string TipId { get; set; }
        public string TipYear { get; set; }
        public string ProjectName { get; set; }
        public int? TipPageNumber { get; set; }
    }
}
