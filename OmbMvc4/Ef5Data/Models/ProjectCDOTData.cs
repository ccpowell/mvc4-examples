using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProjectCDOTData
    {
        public int ProjectVersionID { get; set; }
        public Nullable<short> Region { get; set; }
        public Nullable<short> CommDistrict { get; set; }
        public string RouteSegment { get; set; }
        public Nullable<double> BeginMilePost { get; set; }
        public Nullable<double> EndMilePost { get; set; }
        public string STIPID { get; set; }
        public string STIPProjectDivision { get; set; }
        public Nullable<int> CDOTProjectManager { get; set; }
        public string TPRAbbr { get; set; }
        public Nullable<short> TPRID { get; set; }
        public Nullable<int> LRPNumber { get; set; }
        public string InvestmentCategory { get; set; }
        public string CorridorID { get; set; }
        public string CDOTProjectNumber { get; set; }
        public Nullable<int> SubAccount { get; set; }
        public Nullable<int> ConstructionRE { get; set; }
        public string CMSNumber { get; set; }
        public Nullable<System.DateTime> ScheduledADDate { get; set; }
        public Nullable<int> ProjectStage { get; set; }
        public Nullable<System.DateTime> ProjectStageDate { get; set; }
        public Nullable<System.DateTime> ConstructionDate { get; set; }
        public Nullable<System.DateTime> ProjectClosed { get; set; }
        public string Notes { get; set; }
        public virtual ProjectVersion ProjectVersion { get; set; }
    }
}
