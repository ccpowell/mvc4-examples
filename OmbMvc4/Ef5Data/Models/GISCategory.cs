using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class GISCategory
    {
        public GISCategory()
        {
            this.ProjectSegments = new List<ProjectSegment>();
            this.ProjectSegments1 = new List<ProjectSegment>();
        }

        public int GISCategoryID { get; set; }
        public int GISCategoryTypeID { get; set; }
        public string GISCategory1 { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public virtual GISCategoryType GISCategoryType { get; set; }
        public virtual ICollection<ProjectSegment> ProjectSegments { get; set; }
        public virtual ICollection<ProjectSegment> ProjectSegments1 { get; set; }
    }
}
