using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class v_GetFullyDepictedPoolProjects
    {
        public string COGID { get; set; }
        public int ProjectVersionID { get; set; }
        public Nullable<int> PoolMasterVersionID { get; set; }
        public string FacilityName { get; set; }
        public string ProjectName { get; set; }
        public string TIPID { get; set; }
        public string TIP_Year { get; set; }
        public string Version_Status { get; set; }
    }
}
