#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 07/03/2009	DBouwman        1. Initial Creation (DTS).
 * 02/01/2010   DDavidson       2. Multiple imrpovements.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels.TIP;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain;
using System.Data;
using DRCOG.Domain.Models.TIP;

namespace DRCOG.Domain.Interfaces
{
    /// <summary>
    /// Repository for dealing with the TIP Entities.
    /// </summary>    
    public interface ITipRepository : ITransportationRepository
    {

        /// <summary>
        /// Gets a list of TIPSummary instances for listing.
        /// </summary>
        /// <returns></returns>
        //IList<TipStatusModel> GetTIPSummaries();

        /// <summary>
        /// Get a specific TIPSummary
        /// </summary>
        /// <param name="tipId"></param>
        /// <returns></returns>        
        TipSummary GetTIPSummary(string tipYear);

        IEnumerable<string> GetDelayYears(int timePeriodId);
        IEnumerable<Delay> GetDelays(string year, int timePeriodId);
        Delay GetDelay(Delay delay);
        bool UpdateDelay(Delay delay);

        //TipStatusModel GetTIPStatusModelById(int tipId);

        StatusViewModel GetTipStatusViewModel(string tipYear);

        TipListViewModel GetTipListViewModel();

        TipDashboardViewModel GetTipDashboardViewModel(string tipYear, Enums.TIPDashboardListType type);

        IList<FundingSourceModel> GetTipFundingSources(string tipYear);

        FundingSourceModel GetFundingSourceModel(string tipYear, int fundingTypeId);

        /// <summary>
        /// Add a sponsor agency to the TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="organizationId"></param>
        string AddAgencyToTIP(string tipYear, int organizationId);

        /// <summary>
        /// Drop a sponsor agency from the tip.
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="organizationId"></param>
        /// <returns>if false, the agency sponsors projects thus can not be removed</returns>
        string DropAgencyFromTIP(string tipYear, int organizationId);

        /// <summary>
        /// Create a new TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="offset"></param>
        void CreateTip(string tipYear, int offset);

        int CreateProject(string projectName, string facilityName, string tipYear, int sponsorOrganizationId, int amendmentTypeId);

        IList<TipSummary> GetRestoreProjectList(int timePeriodId);

        /// <summary>
        /// Update the database with the new TipStatus information
        /// in the model
        /// </summary>
        /// <param name="model"></param>
        void UpdateTipStatus(TipStatusModel model);

        /// <summary>
        /// Get the Tip Eligible Agencies model
        /// </summary>
        /// <param name="tipId"></param>
        /// <returns></returns>
        SponsorsViewModel GetSponsorsViewModel(string tipYear);

        /// <summary>
        /// Update the database with the Eligible Agencies for a TIP
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>We are sending in just the lists because this is an Ajax callback
        /// and not a form post, and we are just sending back the ID's that need to be changed</remarks>
        void UpdateEligibleAgencies(string tipYear, List<int> AddedOrganizations, List<int> RemovedOrganizations);
               
        /// <summary>
        /// Gets the projects associated with a TIP
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        /// <remarks>Since there are likely a lot of projects, this returns the light-weight <see cref="ProjectSummary"/> and not the fully loaded <see cref="Project"/></remarks>
        IList<TipSummary> GetTIPProjects(TIPSearchModel projectSearchModel);

        /// <summary>
        /// Get a list of projects that can have a new amendment
        /// created, for the specified TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        IList<TipSummary> GetAmendableTIPProjects(string tipYear);

        /// <summary>
        /// Gets a list of the projects with active amendments in the
        /// specified TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        IList<TipSummary> GetActiveTIPProjects(string tipYear);

        /// <summary>
        /// Gets a list of Proposed amendments in TIP year
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        //IList<TipSummary> GetProposedTIPProjects(string tipYear);
        TipReports GetProposedTIPProjects(string tipYear);

        /// <summary>
        /// Gets a list of projects in the waiting list for the current TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        IList<TipSummary> GetWaitingListTIPProjects(string tipYear);


        /// <summary>
        /// Gets a list of projects that can be restored on a given a tipYearDestination
        /// (to be excluded) and a tipYearSource (to pull from).
        /// </summary>
        /// <param name="tipYearDestination"></param>
        /// <param name="tipYearSourceID"></param>
        /// <param name="TIPID"></param>
        /// <returns></returns>
        IList<TipSummary> GetTIPRestoreCandidateProjects(string tipYearDestination, int tipYearSourceID, string tipID);
#if impl
        /// <summary>
        /// Get a single project summary based on the project id
        /// </summary>
        /// <param name="projectId">project id</param>
        /// <returns></returns>
        TipSummary GetProjectSummaryById(int projectId);

        /// <summary>
        /// Get the populated model for a project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        ProjectModel GetProjectById(int projectId);

        /// <summary>
        /// Get the populated model for TIP Table 1 report
        /// </summary>
        /// <returns>IList<TipSortedProjectListModel></returns>
        IList<TipSortedProjectListModel> GetTIPProjectList();
#endif
        ProjectSearchViewModel GetProjectSearchViewModel(string currentProgram, string year);

        //Lookups
        IDictionary<int, string> GetAvailableSponsors();
        IDictionary<int, string> GetAvailableTipYears();

        ReportsViewModel GetReportsViewModel(string currentProgram, string year);

        AmendmentsViewModel GetAmendmentsViewModel(string year);

        String UpdateReportProjectVersionOrder(int reportId, string projects);

        String SetReportProjectVersionOnHold(int reportId, int projectVersionId);

        IList<TipSummary> GetProjectsByAmendmentStatusId(int timePeriodId, Enums.TIPAmendmentStatus amendmentStatus);

        String GetCurrentTimePeriod();

        void SetProjectSearchDefaults(TIPSearchModel model);

        IDictionary<string, string> GetAlopReportList();

        DataTable GetAlopReportResults(string reportId);

    }
}
