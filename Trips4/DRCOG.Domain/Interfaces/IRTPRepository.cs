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
using DRCOG.Domain.ViewModels.RTP;
using DRCOG.Domain;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.Interfaces
{
    /// <summary>
    /// Repository for dealing with the RTP Entities.
    /// </summary>    
    public interface IRtpRepository : ITransportationRepository
    {
        //StatusViewModel GetRtpStatusViewModel(string tipYear);
        RtpListViewModel GetRtpListViewModel();

        RtpDashboardViewModel GetDashboardViewModel(string financialYear, Enums.RTPDashboardListType type);

        IList<RtpSummary> GetRTPProjects(RTPSearchModel projectSearchModel);
        IList<RtpSummary> GetRestoreProjectList(int timePeriodId);

        /// <summary>
        /// Update the database with the new RtpStatus information
        /// in the model
        /// </summary>
        /// <param name="model"></param>
        void UpdateRtpStatus(RtpStatusModel model);

        StatusViewModel GetRtpStatusViewModel(string year);
        
        string GetCurrentRtpPlanYear();

        ProjectSearchViewModel GetProjectSearchViewModel(string year, string currentProgram);
        
        void SetProjectSearchDefaults(RTPSearchModel model);
        SponsorsViewModel GetSponsorsViewModel(string plan);
        RtpSummary GetSummary(string plan);
        RtpSummary GetSummary(string plan, int cycleId);

        /// <summary>
        /// Update the database with the Eligible Agencies for a TIP
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>We are sending in just the lists because this is an Ajax callback
        /// and not a form post, and we are just sending back the ID's that need to be changed</remarks>
        void UpdateEligibleAgencies(string tipYear, List<int> AddedOrganizations, List<int> RemovedOrganizations);

        string AddCycleToTimePeriod(string timePeriod, int cycleId);
        string RemoveCycleFromTimePeriod(int cycleId);
        string CreateCycle(string cycle);
        string UpdateCycleName(int cycleId, string cycle);
        void CreateRtp(string timePeriod);
        IDictionary<int, string> GetPlanAvailableProjects(int planId, int cycleId);

        IDictionary<int, string> GetPlanScenariosByCycleId(int cycleId);
        IDictionary<int, string> GetPlanScenariosByCycle(string cycle);
        IDictionary<int, string> GetPlanScenarios(int planYearId);
        IDictionary<int, string> GetPlanScenariosByOpenYear(string openYear);
        IDictionary<int, string> GetPlanScenariosForCurrentCycle(int planYearId);

        String SetActiveCycle(int cycleId, int timePeriodId);

        int CreateProject(string projectName, string facilityName, string plan, int sponsorOrganizationId, int? cycleId);

        IList<RtpSummary> GetAmendableProjects(int timePeriodId, int cycleId, bool excludeHasPending);
        IList<RtpSummary> GetAmendableProjects(int timePeriodId, int cycleId, bool excludeHasPending, bool showScenerio);
        IList<Cycle> GetPlanCycles(int timePeriodId);

        Cycle GetCycleDetails(int timePeriodId);
        Cycle GetCycleDetails(int timePeriodId, Enums.RTPCycleStatus status);

        ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId);
        ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId, Enums.RTPCycleStatus? status);
        CycleAmendment GetCurrentCycle(int timePeriodId);

        int CreateCategory(string categoryName, string shortName, string description, string plan);
        IList<RTPFundingSourceModel> GetFundingSources(string plan);

        void SetPlanCurrent(int timePeriodId);

        String UpdateTimePeriodCycleOrder(string cycles);

        string UpdateTimePeriodStatusId(int timePeriodId, int statusId);

        // moved to RtpRepository2
        // ReportsViewModel GetReportsViewModel(string year);
    }
}
