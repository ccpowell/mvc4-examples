#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 08/05/2009	NKirkes         1. Initial Creation (DTS).
 * 02/01/2010   DDavidson       2. Multiple imrpovements.
 * 02/17/2010   DDavidson       3. Added Funding methods.
 * 02/25/2010   DTucker         4. Added CopyProject, GetProjectDetailViewModel.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using DRCOG.Domain.ViewModels.RTP.Project;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels.RTP;
using DRCOG.Domain;

namespace DRCOG.Domain.Interfaces
{
    public interface IRtpProjectRepository : ITransportationRepository 
    {
        InfoViewModel GetProjectInfoViewModel(int projectVersionId, string tipYear);
        InfoViewModel GetCreateProjectViewModel();
        InfoModel GetProjectInfo(int projectVersionId, string tipYear);
        void UpdateProjectInfo(InfoModel model);

        //ProjectDetailViewModel GetProjectDetailViewModel(Int32 projectVersionId);

        LocationViewModel GetProjectLocationViewModel(int projectVersionId, string tipYear);
        LocationModel GetProjectLocationModel(int projectVersionId, string tipYear);
        void UpdateProjectLocationModel(LocationModel model);
        //void AddCountyShare(CountyShareModel model);
        //void UpdateCountyShare(CountyShareModel model);
        //void DropCountyShare(int projectId, int countyId);        
        //void AddMunicipalityShare(MunicipalityShareModel model);
        //void UpdateMunicipalityShare(MunicipalityShareModel model);
        //void DropMunicipalityShare(int projectId, int muniId);

        ScopeViewModel GetScopeViewModel(int projectVersionId, string tipYear);
        ScopeModel GetScopeModel(int projectVersionId, string tipYear);
        void UpdateProjectScope(ScopeModel model);

        FundingViewModel GetFundingViewModel(int projectVersionId, string tipYear);
        Funding GetFunding(int projectVersionID, string plan);
        PlanReportGroupingCategory GetPlanReportGroupingCategoryDetails(int categoryId);
        IList<FundingModel> GetFundingHistory(int projectVersionID);
        IList<FundingDetailModel> GetFundingDetail(int projectVersionID);
        FundingDetailPivotModel GetFundingDetailPivot(int projectVersionID);

        ProjectCdotDataViewModel GetCDOTDataViewModel(int projectVersionId, string year);
        //StrikesViewModel GetStrikesViewModel(int projectVersionId, string tipYear);
        AmendmentsViewModel GetAmendmentsViewModel(int projectVersionId, string tipYear);
        //int CreateProject(InfoModel projectInfo);
        int CreateProject(string projectName, string facilityName, string tipYear, int sponsorOrganizationId);
        //Int32 CopyProject(Int32 projectVersionId);
        RtpSummary CopyProject(Int32 cycleId, Int32 projectVersionId);
        RtpSummary CopyProject(Int32 projectVersionId);
        RtpSummary CopyProject(string plan, Int32 cycleId, Int32 projectVersionId);
        Boolean DeleteProjectVersion(Int32 projectVersionId/*, Enums.RTPAmendmentStatus expectedStatus*/);
        IDictionary<int, string> GetSponsorContacts(int sponsorOrganizationId);
        String GetSponsorContact(int sponsorOrganizationID, int sponsorContactId);
        Int32 GetProjectAmendmentStatus(Int32 projectVersionId);
        void UpdateProjectAmendmentStatus(CycleAmendment model);
        void UpdateProjectVersionStatusId(Int32 projectVersionId, Int32 versionStatusId);
        Int32 GetProjectMostRecentAmendment(Int32 projectVersionId);

        void UpdateCurrentSponsors(string projectVersionID, List<int> AddedOrganizations, List<int> RemovedOrganizations);
        string AddAgencyToTIPProject(int projectVersionID, int organizationId, bool isPrimary);
        string DropAgencyFromTIP(int projectVersionID, int organizationId);
        SegmentViewModel GetSegmentViewModel(int projectVersionID);
        IList<SegmentModel> GetProjectSegments(int projectVersionID);
        IList<PoolProject> GetPoolProjects(int projectVersionID);
        Int32 AddSegment(SegmentModel model);
        void DeleteSegment(int segmentId);
        void UpdateSegment(SegmentModel model);
        void UpdateSegmentSummary(SegmentModel model);
        SegmentModel GetSegmentDetails(int segmentId);

        Int32 AddFinancialRecord(FundingModel model);
        void UpdateFinancialRecord(Funding model);
        void DeleteFinancialRecord(int projectFinancialRecordId);

        void AddFinancialRecordDetail(int projectVersionID, int fundingPeriodID, int fundingTypeID);
        void UpdateFinancialRecordDetail(ProjectFinancialRecordDetail model);
        void DeleteFinancialRecordDetail(ProjectFinancialRecordDetail model);

        DetailViewModel GetDetailViewModel(Int32 projectVersionId, String tipYear);

        IList<ProjectAmendments> GetProjectAmendments(RTPSearchModel projectSearchModel);
        Cycle GetProjectCycleInfo(int versionId);

        //void UpdateProjectScope(ScopeModel model);
        
        //void UpdateProjectLocationModel(LocationModel model);

        //object GetStrikesViewModel(int id, string year);

        //object GetCDOTDataViewModel(int id, string year);

        void AddFundingSource(FundingSource model, int projectVersionId);
        void DeleteFundingSource(FundingSource model, int projectVersionId);
        
        RtpSummary RestoreProjectVersion(int projectVersionID, string plan);


        //Int32 GetOriginalSubmittedVersionId(Int32 projectVersionId);
        //Scheme GetLRSScheme(int id);
        //LRSRecord GetSegmentLRSData(int schemeId, int segmentId);

        void UpdateLRSRecord(SegmentModel model);
        void AddLRSRecord(SegmentModel model);
        void DeleteLRSRecord(int lrsId);
        
    }
}
