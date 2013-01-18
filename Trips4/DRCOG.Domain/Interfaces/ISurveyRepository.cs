#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 05/05/2010	DDavidson       1. Initial Creation.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using DRCOG.Domain.ViewModels.Survey;
using DRCOG.Domain;
using DRCOG.Domain.Models.Survey;
using DRCOG.Domain.Models;
using System.Data;

namespace DRCOG.Domain.Interfaces
{
    /// <summary>
    /// Repository for dealing with the RTP Entities.
    /// </summary>    
    public interface ISurveyRepository : ITransportationRepository
    {
        ListViewModel GetListViewModel();
        DashboardViewModel GetDashboardViewModel(DashboardViewModel model /* string year, Enums.SurveyDashboardListType type*/);
        CreateProjectViewModel GetCreateProjectViewModel(Survey model);
        Survey GetSurvey(string year);
        //Summary GetSummary(string year);

        void SetSurveyDates(Survey model);
        Survey GetSurveyDates(Survey model);
        void CloseSurveyNow(Survey model);
        void OpenSurveyNow(Survey model);

        Project GetProjectBasics(int versionId);
        Project GetProjectBasicsBySegment(int segmentId);
        void CheckUpdateStatusId(Project project);

        IEnumerable<SurveyOverview> GetDetailsOverview(int timePeriodId);
        DataTable GetModelerExtractResults(int timePeriodId);

        List<Project> GetProjects(DRCOG.Domain.Models.Survey.SearchModel projectSearchModel);
        InfoViewModel GetProjectInfoViewModel(int versionId, string year);
        Project GetProjectInfo(int versionId, string year);
        void UpdateProjectInfo(Project model);

        ScopeViewModel GetScopeViewModel(int projectVersionId, string year);
        ScopeModel GetScopeModel(int projectVersionId, string year);
        SegmentModel GetSegmentDetails(int segmentId);
        Int32 AddSegment(SegmentModel model);
        void DeleteSegment(int segmentId);
        void UpdateSegment(SegmentModel model);
        void UpdateSegmentSummary(SegmentModel model);
        void UpdateProjectScope(ScopeModel model, Project project);

        LocationViewModel GetProjectLocationViewModel(int projectVersionId, string year);
        LocationModel GetProjectLocationModel(int projectVersionId, string year);
        void UpdateProjectLocationModel(LocationModel model, int projectVersionId);

        FundingViewModel GetFundingViewModel(int projectVersionId, string year);
        void UpdateFinancialRecord(Project model);
        void AddFundingSource(FundingSource model, int projectVersionId);
        void DeleteFundingSource(FundingSource model, int projectVersionId);

        IDictionary<int, string> GetSponsorContacts(int sponsorOrganizationId);
        String GetSponsorContact(int sponsorOrganizationID, int sponsorContactId);

        Survey GetCurrentSurvey();
        string GetCurrentSurveyYear();
        void SetSurveyStatus(Instance version);
        Instance CopyProject(int projectVersionId);
        Instance CopyProject(int projectVersionId, int surveyId);

        //int CreateProject(string projectName, string facilityName, int timePeriodId, int sponsorOrganizationId, int sponsorContactId);
        int CreateProject(string projectName, string facilityName, int timePeriodId, int sponsorOrganizationId, int sponsorContactId, int improvementTypeId, string startAt, string endAt);

        Funding GetFunding(int projectVersionId, string plan);
        
        IList<Contact> GetContact(ContactSearch criteria);

        DRCOG.Domain.Models.ProjectSponsorsModel GetProjectSponsorsModel(int projectVersionID, string year);

        /// <summary>
        /// Get list of projects available for a new survey, using the Current survey
        /// </summary>
        /// <returns>list of available projects from Current survey</returns>
        IList<Project> GetAmendableProjects();

        int CreateSurvey(int planId, string surveyName);

        //IList<RtpSummary> GetRestoreProjectList(int timePeriodId);

        ///// <summary>
        ///// Update the database with the new RtpStatus information
        ///// in the model
        ///// </summary>
        ///// <param name="model"></param>
        //void UpdateRtpStatus(RtpStatusModel model);

        //StatusViewModel GetRtpStatusViewModel(string year);
        
        //string GetCurrentRtpPlanYear();

        //ProjectSearchViewModel GetProjectSearchViewModel(string year, string currentProgram);
        
        //void SetProjectSearchDefaults(RTPSearchModel model);
        SponsorsViewModel GetSponsorsViewModel(string timePeriod);
        //RtpSummary GetSummary(string plan);

        //void UpdateEligibleAgencies(string timePeriod, List<int> AddedOrganizations, List<int> RemovedOrganizations);

        //string AddCycleToTimePeriod(string timePeriod, int cycleId);
        //string RemoveCycleFromTimePeriod(int cycleId);
        //string CreateCycle(string cycle);
        //string UpdateCycleName(int cycleId, string cycle);
        //void CreateRtp(string timePeriod);
        //IDictionary<int, string> GetPlanAvailableProjects(int planId, int cycleId);

        //IDictionary<int, string> GetPlanScenariosByCycleId(int cycleId);
        //IDictionary<int, string> GetPlanScenariosByCycle(string cycle);
        //IDictionary<int, string> GetPlanScenarios(int planYearId);
        //IDictionary<int, string> GetPlanScenariosByOpenYear(string openYear);
        //IDictionary<int, string> GetPlanScenariosForCurrentCycle(int planYearId);

        //String SetActiveCycle(int cycleId, int timePeriodId);

        //int CreateProject(string projectName, string facilityName, string plan, int sponsorOrganizationId);

        //IList<RtpSummary> GetAmendableProjects(int timePeriodId);

        //Cycle GetCycleDetails(int timePeriodId);
        //Cycle GetCycleDetails(int timePeriodId, Enums.RTPCycleStatus status);

        //ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId);
        //ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId, Enums.RTPCycleStatus? status);
        //CycleAmendment GetCurrentCycle(int timePeriodId);

        //int CreateCategory(string categoryName, string shortName, string description, string plan);
        //IList<FundingSourceModel> GetFundingSources(string plan);

        //void SetPlanCurrent(int timePeriodId);

        //String UpdateTimePeriodCycleOrder(string cycles);

        //string UpdateTimePeriodStatusId(int timePeriodId, int statusId);

    }
}
