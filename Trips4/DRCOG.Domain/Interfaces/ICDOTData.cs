using System;
namespace DRCOG.Domain.Interfaces
{
    public interface ICdotData
    {
        double BeginMilePost { get; set; }
        int CDOTProjectManager { get; set; }
        string CDOTProjectNumber { get; set; }
        string CMSNumber { get; set; }
        short? CommDistrict { get; set; }
        DateTime ConstructionDate { get; set; }
        int ConstructionRE { get; set; }
        string CorridorID { get; set; }
        double EndMilePost { get; set; }
        string InvestmentCategory { get; set; }
        int LRPNumber { get; set; }
        string Notes { get; set; }
        DateTime ProjectClosed { get; set; }
        int ProjectStage { get; set; }
        DateTime ProjectStageDate { get; set; }
        short? Region { get; set; }
        string RouteSegment { get; set; }
        DateTime ScheduledADDate { get; set; }
        string STIPID { get; set; }
        string STIPProjectDivision { get; set; }
        int SubAccount { get; set; }
        short TPRID { get; set; }
        string TPRAbbr { get; set; }
    }
}
