using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class LRSScheme
    {
        public LRSScheme()
        {
            this.LRS = new List<LR>();
            this.LRSSchemeAttrs = new List<LRSSchemeAttr>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<LR> LRS { get; set; }
        public virtual ICollection<LRSSchemeAttr> LRSSchemeAttrs { get; set; }
    }
}
