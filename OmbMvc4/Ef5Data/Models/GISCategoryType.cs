using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class GISCategoryType
    {
        public GISCategoryType()
        {
            this.GISCategories = new List<GISCategory>();
        }

        public int GISCategoryTypeID { get; set; }
        public string GISCategoryType1 { get; set; }
        public virtual ICollection<GISCategory> GISCategories { get; set; }
    }
}
