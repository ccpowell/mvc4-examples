using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ReportProjectVersionSorting
    {
        public int ReportId { get; set; }
        public int ProjectVersionId { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
        public virtual Report1 Report1 { get; set; }
    }
}
