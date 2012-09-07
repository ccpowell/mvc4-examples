using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class PoolProject
    {
        public int PoolProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string BeginAt { get; set; }
        public string EndAt { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public Nullable<int> PoolMasterVersionID { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
    }
}
