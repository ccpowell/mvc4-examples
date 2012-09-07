using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class LRSCategoryType
    {
        public LRSCategoryType()
        {
            this.LRSCategories = new List<LRSCategory>();
        }

        public int Id { get; set; }
        public string Category { get; set; }
        public virtual ICollection<LRSCategory> LRSCategories { get; set; }
    }
}
