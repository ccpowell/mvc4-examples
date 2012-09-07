using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class LRSCategory
    {
        public LRSCategory()
        {
            this.LRSSchemeAttrs = new List<LRSSchemeAttr>();
            this.LRSSchemeAttrs1 = new List<LRSSchemeAttr>();
        }

        public int Id { get; set; }
        public int CategoryTypeId { get; set; }
        public string Name { get; set; }
        public virtual LRSCategoryType LRSCategoryType { get; set; }
        public virtual ICollection<LRSSchemeAttr> LRSSchemeAttrs { get; set; }
        public virtual ICollection<LRSSchemeAttr> LRSSchemeAttrs1 { get; set; }
    }
}
