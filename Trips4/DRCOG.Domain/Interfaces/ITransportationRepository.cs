#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 02/17/2010	DDavidson       1. Initial Creation.
 * 
 * DESCRIPTION:
 * Interface for base Transportation repository for 
 * Transportation specific functions that extend across 
 * all derived repositories.
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models;
using DRCOG.Domain;

namespace DRCOG.Domain.Interfaces
{
    public interface ITransportationRepository : IBaseRepository
    {
        //Single lookups
        int GetYearId(string year, DRCOG.Domain.Enums.TimePeriodType timePeriodType);
        string GetYear(int yearId);
        int? GetSponsorAgencyID(string sponsorAgency);
        string GetSponsorAgency(int? sponsorAgencyID);
        int? GetImprovementTypeID(string improvementType);
        string GetImprovementType(int? improvementTypeID);
        int? GetProjectTypeID(string projectType);
        string GetProjectType(int? projectTypeID);
        int? GetStatusID(string status, string statusType);
        string GetStatus(int? statusID, string statusType);
        

        //Multiple lookups
        IList<Organization> GetCurrentTimePeriodSponsorAgencies(string timePeriod, Enums.ApplicationState appState);
        IList<Organization> GetAvailableTimePeriodSponsorAgencies(string timePeriod, Enums.ApplicationState appState);
        
        //IDictionary<int, string> GetPlanScenarios(int planYearId, bool isActive);
        //IDictionary<int, string> GetPlanScenarios(string planYear, bool isActive);

        string AddAgencyToTimePeriod(string timePeriod, int organizationId, Enums.ApplicationState appState);
        string DropAgencyFromTimePeriod(string timePeriod, int organizationId, Enums.ApplicationState appState);

        string AddImprovementTypeToTimePeriod(string timePeriod, int improvementTypeId, Enums.ApplicationState appState);
        bool DropImprovementTypeFromTimePeriod(string timePeriod, int improvementTypeId, Enums.ApplicationState appState);

        string AddFundingResourceToTimePeriod(int timePeriodId, int fundingResourceId);
        bool DropFundingResourceFromTimePeriod(int timePeriodId, int fundingResourceId, Enums.ApplicationState appState);

        void UpdateCountyShare(CountyShareModel model);
        void DropCountyShare(int projectId, int countyId);
        void AddMunicipalityShare(MunicipalityShareModel model);
        void UpdateMunicipalityShare(MunicipalityShareModel model);
        void AddCountyShare(CountyShareModel model);
        void DropMunicipalityShare(int projectId, int muniId);


        int GetCategoryId(string category);
        string GetCategory(int categoryId);
        IDictionary<int, string> GetCategories(int typeId);

        void CreateFundingSource(FundingSourceModel model);
        void UpdateFundingSource(FundingSourceModel model);
        IList<Organization> GetOrganizationsByType(Enums.OrganizationType type);
        IDictionary<int, string> AvailableFundingGroups();

        Scheme GetLRSScheme(int id);

        LRSRecord GetSegmentLRSData(int schemeId, int LRSId);
        LRSRecords GetSegmentLRSSummary(int schemeId, int segmentId);

        IList<ContactRole> GetSponsorContactRoles(string userShortGuid);
        IDictionary<int, string> GetSponsorContactRoles();

        bool SetSponsorContactRole(string userShortGuid, string organization, string role);
        bool DeleteSponsorContactRoleByUser(string userShortGuid, string organization, string role);

        FundingSourceModel GetFundingSource(FundingSourceModel model);

        //IList<Organization> GetCurrentTimePeriodSponsorAgencies(string timePeriod, Enums.ApplicationState appState);
    }
}
