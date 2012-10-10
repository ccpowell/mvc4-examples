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
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain.ViewModels.TIPProject;

namespace DRCOG.Domain.Interfaces
{
    public interface IProjectRepository : ITransportationRepository 
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
        IList<FundingModel> GetFunding(int projectVersionID);
        IList<FundingPhase> GetFundingPhases(int projectVersionId, string tipYear);
        IList<FundingModel> GetFundingHistory(int projectVersionID);
        IList<FundingDetailModel> GetFundingDetail(int projectVersionID);
        FundingDetailPivotModel GetFundingDetailPivot(int projectVersionID);

        ProjectCdotDataViewModel GetCDOTDataViewModel(int projectVersionId, string tipYear);
        TipCdotData GetCdotData(int projectVersionId);
        StrikesViewModel GetStrikesViewModel(int projectVersionId, string tipYear);
        AmendmentsViewModel GetAmendmentsViewModel(int projectVersionId, string tipYear);
        void UpdateAmendmentDetails(ProjectAmendments amendment);
        //int CreateProject(InfoModel projectInfo);
        int CreateProject(string projectName, string facilityName, string tipYear, int sponsorOrganizationId, int amendmentTypeId);
        Int32 CopyProject(Int32 projectVersionId);
        Int32 RestoreProjectVersion(Int32 projectVersionID, string tipYear);
        void RestoreProjectVersionFinancials(Int32 currentProjectVersionId, Int32 newProjectVersionId, string tipYear);
        String DeleteProjectVersion(ProjectAmendments model);
        IDictionary<int, string> GetSponsorContacts(int sponsorOrganizationId);
        String GetSponsorContact(int sponsorOrganizationID, int sponsorContactId);
        Int32 GetProjectAmendmentStatus(Int32 projectVersionId);
        void UpdateProjectAmendmentStatus(ProjectAmendments model);
        void UpdateProjectVersionStatusId(Int32 projectVersionId, Int32 versionStatusId);
        Int32 GetProjectMostRecentAmendment(Int32 projectVersionId);

        void UpdateCurrentSponsors(string projectVersionID, List<int> AddedOrganizations, List<int> RemovedOrganizations);
        string AddAgencyToTIPProject(int projectVersionID, int organizationId, bool isPrimary);
        string DropAgencyFromTIP(int projectVersionID, int organizationId);
        SegmentViewModel GetSegmentViewModel(int projectVersionID);
        IList<SegmentModel> GetProjectSegments(int projectVersionID);
        IList<PoolProject> GetPoolProjects(int projectVersionID);
        Int32 AddPoolProject(PoolProject model);
        void DeletePoolProject(int poolProjectId);
        void UpdatePoolProject(PoolProject model);

        Int32 AddFinancialRecord(FundingModel model);
        void UpdateFinancialRecord(FundingModel model);
        void DeleteFinancialRecord(int projectFinancialRecordId);

        void AddFinancialRecordDetail(int projectVersionID, int fundingPeriodID, int fundingTypeID);
        void DeleteFinancialRecordDetail(int projectVersionID, int fundingResourceId);
        void UpdateFinancialRecordDetail(ProjectFinancialRecordDetail model);
        void DeleteFinancialRecordDetail(ProjectFinancialRecordDetail model);

        DetailViewModel GetDetailViewModel(Int32 projectVersionId, String tipYear);
        TipSummary GetProjectSummary(int versionId);

        IDictionary<int, string> GetFundingYearsAvailable(int timePeriodId);

        void DeletePhase(FundingPhase phase);
        void AddPhase(FundingPhase phase);

        Int32 GetPreviousProjectVersionId(Int32 projectVersionId);

        int GetActiveProjectVersion(string tipId);
        int GetActiveProjectVersion(string tipId, string year);
        int GetActiveProjectVersion(string tipId, int yearId);
        int GetActiveProjectVersion(string tipId, string year, int yearId);

        void UpdateCdotData(TipCdotData model, int projectVersionId);
    }
}
