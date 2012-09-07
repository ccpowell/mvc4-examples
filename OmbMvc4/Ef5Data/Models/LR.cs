using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class LR
    {
        public int Id { get; set; }
        public Nullable<int> ProjectSegmentId { get; set; }
        public int LRSSchemeId { get; set; }
        public string Data { get; set; }
        public virtual LRSScheme LRSScheme { get; set; }
        public virtual ProjectSegment ProjectSegment { get; set; }
    }
}
