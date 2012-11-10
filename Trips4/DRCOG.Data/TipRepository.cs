#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 08/06/2009	DBouwman        1. Initial Creation (DTS).
 * 01/26/2010	DDavidson	    2. Reformatted. Added GetProjectSearchViewModel and GetProjectSearchModels.
 * 02/01/2010   DDavidson       3. Added ImprovementType to GetTipDashboardViewModel.
 * 02/17/2010   DDavidson       4. Derived from TransportationRepository. Moved Single lookups to TransportationRepository.
 * 04/26/2010   DDavidson       5. Moved code from GetSponsorsViewModel to TransportationRepository (Multiple Lookups).
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain.ViewModels.TIP;
using DRCOG.Entities;
using System.Linq;
using DRCOG.Domain;
using DRCOG.Domain.Helpers;
using System.Diagnostics;
using System.Transactions;
using DRCOG.Common.Services.Interfaces;
using DRCOG.Common.Util;
using System.Web.Mvc;
using DRCOG.Domain.Models.TIP;

namespace DRCOG.Data
{
    /// <summary>
    /// </summary>
    public class TipRepository : TransportationRepository, ITipRepository
    {
        //private Fakes.FakeTipRepository _fakeTipRepo;
        //private readonly Enums.ApplicationState _appState;
        private readonly IFileRepositoryExtender FileRepository;

        public TipRepository(IFileRepositoryExtender fileRepository)
        {
            _appState = Enums.ApplicationState.TIP;
            FileRepository = fileRepository;
            //_fakeTipRepo = new Fakes.FakeTipRepository();
        }

#region SQL ITipRepository Members

        public IEnumerable<string> GetDelayYears(int timePeriodId)
        {
            List<string> list = new List<string>();

            using (SqlCommand cmd = new SqlCommand("[TIP].[GetDelayYears]"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    while (rdr.Read())
                    {
                        list.Add(rdr["Year"].ToString());
                    }
                }
            }
            return list;
        }

        public IEnumerable<Delay> GetDelays(string year, int timePeriodId)
        {
            List<Delay> list = new List<Delay>();
            
            using (SqlCommand cmd = new SqlCommand("[TIP].[GetDelays]"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CurrentYear", year);
                cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    while (rdr.Read())
                    {
                        list.Add(new Delay()
                        {
                            ProjectVersionId = (int)rdr["TIPProjectVersionId"]
                            ,
                            Year = rdr["Year"].ToString()
                            ,
                            TipId = rdr["TIPID"].ToString()
                            ,
                            Sponsor = rdr["Sponsor"].ToString()
                            ,
                            ProjectName = rdr["ProjectName"].ToString()
                            ,
                            Phase = rdr["Phase"].ToString()
                            ,
                            AffectedProjectDelaysLocation = rdr["AffectedProjectDelaysLocation"].ToString()
                            ,
                            FederalAmount = rdr["FederalAmount"].ToString().SmartParseDefault<double>(default(double))
                            ,
                            Notes = rdr["Notes"].ToString()
                            ,
                            MidYearStatus = rdr["MidYearStatus"].ToString()
                            ,
                            EndYearStatus = rdr["EndYearStatus"].ToString()
                            ,
                            ActionPlan = rdr["ActionPlan"].ToString()
                            ,
                            MeetingDate = rdr["MeetingDate"].ToString().SmartParseDefault<DateTime>(DateTime.UtcNow)
                            ,
                            IsInitiated = rdr["IsInitiated"].ToString().SmartParseDefault<bool>(false)
                            ,
                            IsChecked = rdr["IsChecked"].ToString().SmartParseDefault<bool>(false)
                            ,
                            IsDelay = rdr["IsDelay"].ToString().SmartParseDefault<bool>(false)
                            ,
                            ProjectFinancialRecordId = rdr["ProjectFinancialRecordId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            FundingIncrementId = rdr["FundingIncrementId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            FundingResourceId = rdr["FundingResourceId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            PhaseId = rdr["PhaseId"].ToString().SmartParseDefault<int>(default(int))
                        });
                    }
                }
            }
            return list;
        }

        public Delay GetDelay(Delay delay)
        {
            using (SqlCommand cmd = new SqlCommand("[TIP].[GetDelay]"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectVersionId", delay.ProjectVersionId);
                cmd.Parameters.AddWithValue("@ProjectFinancialRecordId", delay.ProjectFinancialRecordId);
                cmd.Parameters.AddWithValue("@FundingIncrementId", delay.FundingIncrementId);
                cmd.Parameters.AddWithValue("@FundingResourceId", delay.FundingResourceId);
                cmd.Parameters.AddWithValue("@PhaseId", delay.PhaseId);

                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    while (rdr.Read())
                    {
                        delay.ProjectVersionId = (int)rdr["TIPProjectVersionId"];
                        delay.TimePeriodId = rdr["TimePeriodId"].ToString().SmartParseDefault<int>(default(int)); ;
                        delay.TimePeriod = rdr["TimePeriod"].ToString();
                        delay.Year = rdr["Year"].ToString();
                        delay.TipId = rdr["TIPID"].ToString();
                        delay.Sponsor = rdr["Sponsor"].ToString();
                        delay.ProjectName = rdr["ProjectName"].ToString();
                        delay.Phase = rdr["Phase"].ToString();
                        delay.FederalAmount = rdr["FederalAmount"].ToString().SmartParseDefault<double>(default(double));
                        delay.Notes = rdr["Notes"].ToString();
                        delay.MidYearStatus = rdr["MidYearStatus"].ToString();
                        delay.EndYearStatus = rdr["EndYearStatus"].ToString();
                        delay.ActionPlan = rdr["ActionPlan"].ToString();
                        delay.MeetingDate = rdr["MeetingDate"].ToString().SmartParseDefault<DateTime?>(null);
                        delay.IsInitiated = rdr["IsInitiated"].ToString().SmartParseDefault<bool>(false);
                        delay.IsChecked = rdr["IsChecked"].ToString().SmartParseDefault<bool>(false);
                    }
                }
            }
            return delay;
        }

        public bool UpdateDelay(Delay delay)
        {
            int rowsAffected = 0;
            using (SqlCommand cmd = new SqlCommand("[TIP].[UpdateDelay]"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectFinancialRecordId", delay.ProjectFinancialRecordId);
                cmd.Parameters.AddWithValue("@FundingIncrementId", delay.FundingIncrementId);
                cmd.Parameters.AddWithValue("@FundingResourceId", delay.FundingResourceId);
                cmd.Parameters.AddWithValue("@PhaseId", delay.PhaseId);

                cmd.Parameters.AddWithValue("@IsInitiated", delay.IsInitiated);
                cmd.Parameters.AddWithValue("@IsChecked", delay.IsChecked);
                cmd.Parameters.AddWithValue("@MidYearStatus", delay.MidYearStatus);
                cmd.Parameters.AddWithValue("@EndYearStatus", delay.EndYearStatus);
                cmd.Parameters.AddWithValue("@ActionPlan", delay.ActionPlan);
                cmd.Parameters.AddWithValue("@MeetingDate", delay.MeetingDate);
                cmd.Parameters.AddWithValue("@Notes", delay.Notes);

                rowsAffected = this.ExecuteNonQuery(cmd);
            }
            return rowsAffected > -1 ? true : false;
        }

        /// <summary>
        /// Fetches a TipListViewModel that's used on the TipList View
        /// </summary>
        /// <returns></returns>
        public TipListViewModel GetTipListViewModel()
        {
            TipListViewModel model = new TipListViewModel();
            
            SqlCommand cmd = new SqlCommand("[TIP].[GetPrograms]");
            cmd.CommandType = CommandType.StoredProcedure;

            IList<TipStatusModel> tipPrograms = new List<TipStatusModel>();
            TipStatusModel sm = null;
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                //be sure we got a reader                
                while(rdr.Read())
                {
                    sm = new TipStatusModel();                    
                    
                    sm.ProgramId = (int)rdr["ProgramId"];
                    sm.TimePeriodId = Convert.ToInt32(rdr["TimePeriodId"]);
                    sm.TipYear = rdr["TimePeriod"].ToString();
                    sm.Adoption = rdr["AdoptionDate"] != DBNull.Value ? (DateTime?)rdr["AdoptionDate"]:null;
                    sm.EPAApproval = rdr["USEPAApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USEPAApprovalDate"]:null;
                    sm.GovernorApproval = rdr["GovernorApprovalDate"] != DBNull.Value ? (DateTime?)rdr["GovernorApprovalDate"] : null;
                    sm.LastAmended = rdr["LastAmendmentDate"] != DBNull.Value ? (DateTime?)rdr["LastAmendmentDate"] : null;
                    sm.PublicHearing = rdr["PublicHearingDate"] != DBNull.Value ? (DateTime?)rdr["PublicHearingDate"] : null;
                    sm.USDOTApproval = rdr["USDOTApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USDOTApprovalDate"] : null;
                    sm.Notes = rdr["Notes"].ToString();
                    sm.IsCurrent = (bool)rdr["Current"];
                    sm.IsPending = (bool)rdr["Pending"];
                    sm.IsPrevious = (bool)rdr["Previous"];

                    tipPrograms.Add(sm);
                  
                }
            }
            model.TIPs = tipPrograms;
            return model;
        }

        /// <summary>
        /// Create a new TIP
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public void CreateTip(string tipYear, int offset)
        {
            SqlCommand cmd = new SqlCommand("[TIP].[CreateTip]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TipYear", tipYear);
            cmd.Parameters.AddWithValue("@OffsetKey", offset);
            this.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Gets a list of the projects summarized by type...
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="page"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public TipDashboardViewModel GetTipDashboardViewModel(string tipYear, Enums.TIPDashboardListType type)
        {
            TipDashboardViewModel model = new TipDashboardViewModel();
            //We call different sprocs based on the Enum
            string sprocName = "[TIP].[GetDashboardListByAmendmentStatus]";
            switch (type)
            {
                case Enums.TIPDashboardListType.ProjectType:
                    sprocName = "[TIP].[GetDashboardListByProjectType]";
                    break;
                case Enums.TIPDashboardListType.Sponsor:
                    sprocName = "[TIP].[GetDashboardListBySponsor]";
                    break;
                case Enums.TIPDashboardListType.AmendmentStatus:
                    sprocName = "[TIP].[GetDashboardListByAmendmentStatus]";
                    break;
                case Enums.TIPDashboardListType.ImprovementType:
                    sprocName = "[TIP].[GetDashboardListByImprovementType]";
                    break;
                default:
                    sprocName = "[TIP].[GetDashboardListByAmendmentStatus]";
                    break;
            }

            SqlCommand cmd = new SqlCommand(sprocName);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TIPYEAR", SqlDbType.NVarChar));
            cmd.Parameters.Add(new SqlParameter("@IsActive", SqlDbType.Bit));
            cmd.Parameters[0].Value = tipYear;
            cmd.Parameters[1].Value = 1;
            model.TipSummary.TipYear = tipYear;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    model.DashboardItems.Add(new DashboardListItem()
                    {
                        count = (int)rdr["Count"],
                        ItemName = rdr["ItemName"].ToString()
                    });
                }
            }

            return model;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public SponsorsViewModel GetSponsorsViewModel(string tipYear)
        {
            //ToDo: Need to make the two methods that make up this viewModel be from the
            //TransportationRepository so that the ProjectRepository can access them. -DBD
            var model = new SponsorsViewModel();
            model.TipSummary = this.GetTIPSummary(tipYear);

            // Get Agencies which are eligible to sponsor projects
            model.EligibleAgencies = GetCurrentTimePeriodSponsorAgencies(tipYear, _appState);
            model.AvailableAgencies = GetAvailableTimePeriodSponsorAgencies(tipYear,_appState);

            return model;

        }

        public IList<TipSummary> GetRestoreProjectList(int timePeriodId)
        {
            IList<TipSummary> list = new List<TipSummary>();

            try
            {
                using (SqlCommand command = new SqlCommand("[TIP].[GetRestoreProjectList]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

                    using (IDataReader rdr = this.ExecuteReader(command))
                    {
                        while (rdr.Read())
                        {
                            TipSummary summary = new TipSummary()
                            {
                                ProjectId = rdr["ProjectId"] != DBNull.Value ? rdr["ProjectId"].ToString().SmartParseDefault(default(int)) : default(int)
                                ,
                                TipYearTimePeriodID = rdr["TimePeriodId"] != DBNull.Value ? rdr["TimePeriodId"].ToString().SmartParseDefault(default(short)) : default(short)
                                ,
                                TipYear = rdr["TimePeriod"].ToString()
                                ,
                                ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? rdr["ProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
                                ,
                                ProjectName = rdr["ProjectName"].ToString()
                                ,
                                AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? rdr["AmendmentDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue) : DateTime.MinValue
                                ,
                                AmendmentStatusId = rdr["ProjectVersionId"] != DBNull.Value ? rdr["ProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
                                ,
                                AmendmentStatus = rdr["AmendmentStatus"].ToString()
                                ,
                                TipId = rdr["TIPID"].ToString()
                            };
                            list.Add(summary);
                        }
                    }
                }
            }
            catch
            {

            }

            return list;
        }

        /// <summary>
        /// Add a sponsor agency to the TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="organizationId"></param>
        public string AddAgencyToTIP(string tipYear, int organizationId)
        {
            string result = "";
            int programID = 1; //TODO: Get the ProgramID from Session

            SqlCommand cmd = new SqlCommand("[TIP].[InsertProgramInstanceSponsor]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProgramID", programID);
            cmd.Parameters.AddWithValue("@TimePeriodID", GetYearId(tipYear, Enums.TimePeriodType.TimePeriod)); //Needs to be in ID format.
            cmd.Parameters.AddWithValue("@SponsorID", organizationId);

            try
            {
                this.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Drop a sponsor agency from the tip.
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="organizationId"></param>
        /// <returns>if false, the agency sponsors projects thus can not be removed</returns>
        public string DropAgencyFromTIP(string tipYear, int organizationId)
        {
            string result = "";
            int programID = 1; //TODO: Get the ProgramID from Session

            SqlCommand cmd = new SqlCommand("[TIP].[DeleteProgramInstanceSponsor]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProgramID", programID);
            cmd.Parameters.AddWithValue("@TimePeriodID", GetYearId(tipYear, Enums.TimePeriodType.TimePeriod)); //Needs to be in ID format.
            cmd.Parameters.AddWithValue("@SponsorID", organizationId);

            try
            {
                this.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StatusViewModel GetTipStatusViewModel(string tipYear)
        {
            StatusViewModel model = new StatusViewModel();

            SqlCommand cmd = new SqlCommand("[TIP].[GetStatus]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TIPYEAR", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = tipYear;

            model.TipSummary.TipYear = tipYear;
            var sm = new TipStatusModel();

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if(rdr.Read())
                {                    
                    sm.ProgramId = (int)rdr["ProgramId"];
                    sm.TimePeriodId = Convert.ToInt32(rdr["TimePeriodId"]);
                    sm.TipYear = rdr["TimePeriod"].ToString();
                    sm.Adoption = rdr["AdoptionDate"] != DBNull.Value ? (DateTime?)rdr["AdoptionDate"] : null;
                    sm.EPAApproval = rdr["USEPAApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USEPAApprovalDate"] : null;
                    sm.GovernorApproval = rdr["GovernorApprovalDate"] != DBNull.Value ? (DateTime?)rdr["GovernorApprovalDate"] : null;
                    sm.LastAmended = rdr["LastAmendmentDate"] != DBNull.Value ? (DateTime?)rdr["LastAmendmentDate"] : null;
                    sm.PublicHearing = rdr["PublicHearingDate"] != DBNull.Value ? (DateTime?)rdr["PublicHearingDate"] : null;
                    sm.USDOTApproval = rdr["USDOTApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USDOTApprovalDate"] : null;
                    sm.Notes = rdr["Notes"].ToString();
                    sm.IsCurrent = (bool)rdr["Current"];
                    sm.IsPending = (bool)rdr["Pending"];
                    sm.IsPrevious = (bool)rdr["Previous"];
                    sm.ShowDelayDate = rdr["ShowDelayDate"] != DBNull.Value ? (DateTime)rdr["ShowDelayDate"] : default(DateTime);
                }
            }

            model.TipStatus = sm;            
            return model;            
        }

        /// <summary>
        /// Get a list of the projects in the specified TIP Year
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public IList<TipSummary> GetTIPProjects(TIPSearchModel projectSearchModel)
        {
            //Formerly (DTS) accepted a string 'tipYear' variable and called 'TIP-GetTipProjects'.
            //What I would like to do is make a dynamic Linq statement with the parameters from ProjectSearchModel. -DBD
            IList<TipSummary> list = new List<TipSummary>();

            SqlCommand cmd = new SqlCommand("[TIP].[GetProjects2]");
            cmd.CommandType = CommandType.StoredProcedure;

            //cmd.Parameters.Add(new SqlParameter("@TIPYEAR", SqlDbType.NVarChar));
            //cmd.Parameters[0].Value = tipYear;

            //I will speed up these queries by restricting the list on three items: TipYear, TipYearID or IsActive. -DBD
            if (!projectSearchModel.Exclude_TipYear) // If we are excluding a TipYear, then we must return everything (no SQL optimization)
            {
                if (projectSearchModel.TipYear != null) cmd.Parameters.AddWithValue("@TimePeriod", projectSearchModel.TipYear);
                if (projectSearchModel.TipYearID != null) cmd.Parameters.AddWithValue("@TimePeriodId", projectSearchModel.TipYearID);
            }
            if (projectSearchModel.VersionStatusId > 0)
            {
                //if ((bool)projectSearchModel.ActiveVersion) cmd.Parameters.AddWithValue("@ISACTIVE", 1);
                cmd.Parameters.AddWithValue("@VersionStatusId", projectSearchModel.VersionStatusId);
                //else cmd.Parameters.AddWithValue("@ISACTIVE", 0);
            }
            if (!projectSearchModel.AmendmentTypeId.Equals(default(int)))
            {
                cmd.Parameters.AddWithValue("@AmendmentTypeId", projectSearchModel.AmendmentTypeId);
            }
            if (!projectSearchModel.AmendmentStatusID.Equals(default(int)))
            {
                cmd.Parameters.AddWithValue("@AmendmentStatusId", projectSearchModel.AmendmentStatusID);
            }
            if (!projectSearchModel.FundingTypeId.Equals(default(int)))
            {
                cmd.Parameters.AddWithValue("@FundingTypeId", projectSearchModel.FundingTypeId);
            } 
            if (!projectSearchModel.FundingIncrementID.Equals(default(int)))
            {
                cmd.Parameters.AddWithValue("@FundingIncrementId", projectSearchModel.FundingIncrementID);
            }
            if (!String.IsNullOrEmpty(projectSearchModel.ScopeTerm))
            {
                cmd.Parameters.AddWithValue("@ScopeTerm", projectSearchModel.ScopeTerm);
            }
            if (!String.IsNullOrEmpty(projectSearchModel.PoolTerm))
            {
                cmd.Parameters.AddWithValue("@PoolTerm", projectSearchModel.PoolTerm);
            }
            if (!String.IsNullOrEmpty(projectSearchModel.GeographyName))
            {
                cmd.Parameters.AddWithValue("@GeographyName", projectSearchModel.GeographyName);
            }

            if (!projectSearchModel.CdotRegionId.Equals(default(int)))
            {
                cmd.Parameters.AddWithValue("@CdotRegionId", projectSearchModel.CdotRegionId);
            }

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new TipSummary()
                    {
                            SponsorAgency = rdr["Sponsor"].ToString()
                        ,   TipId = rdr["TIPID"].ToString()
                        ,   StipId = rdr["STIPID"].ToString()
                        ,   TipYear = rdr["TipYear"] != DBNull.Value ? rdr["TipYear"].ToString() : "NULL IN DATABASE"
                        ,   Title = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() :"NULL IN DATABASE"
                        ,   ProjectVersionId = (int)rdr["TipProjectVersionId"]
                        ,   AmendmentStatus = rdr["AmendmentStatus"] !=DBNull.Value ? rdr["AmendmentStatus"].ToString():""
                        ,   AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? (DateTime)rdr["AmendmentDate"] : (DateTime)DateTime.MinValue
                        ,   ImprovementType = rdr["ImprovementType"] !=DBNull.Value ? rdr["ImprovementType"].ToString(): ""
                        ,   ProjectType = rdr["ProjectType"] != DBNull.Value ? rdr["ProjectType"].ToString(): ""
                        ,   ProjectName = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : ""
                        ,   COGID = rdr["COGID"] !=DBNull.Value ? rdr["COGID"].ToString() : ""
                        ,   VersionStatus = rdr["ProjectVersionStatus"] != DBNull.Value ? rdr["ProjectVersionStatus"].ToString() : ""
                    });
                }
            }

            // These are processed via the SQL retrieval code optimization, so only process this one when exclude is checked.
            if (!String.IsNullOrEmpty(projectSearchModel.TipYear) && projectSearchModel.Exclude_TipYear)
            {
                list = (from fli in list
                        where (fli.TipYear != projectSearchModel.TipYear)
                        select fli).ToList<TipSummary>();
            }

            if ((!String.IsNullOrEmpty(projectSearchModel.VersionStatus)) && (projectSearchModel.Exclude_ActiveVersion))
            {
                list = (from fli in list
                        where (!fli.VersionStatus.Equals(projectSearchModel.VersionStatus))
                        select fli).ToList<TipSummary>();
            }

            //Now that we have the base data, let's apply the rest of our parameters
            // Trying to list the paramters here by most restrictive first. Should make searches much quicker. -DBD
            if (!String.IsNullOrEmpty(projectSearchModel.COGID))
            {
                list = (from fli in list
                        where ((fli.COGID.ToLower().Contains(projectSearchModel.COGID.ToLower())) && (!projectSearchModel.Exclude_COGID))
                        || ((!fli.COGID.ToLower().Contains(projectSearchModel.COGID.ToLower())) && (projectSearchModel.Exclude_COGID))
                        select fli).ToList<TipSummary>();
            }

            if (!String.IsNullOrEmpty(projectSearchModel.TipID))
            {
                list = (from fli in list
                        where ((fli.TipId.ToLower().Contains(projectSearchModel.TipID.ToLower())) && (!projectSearchModel.Exclude_TipID))
                        || ((!fli.TipId.ToLower().Contains(projectSearchModel.TipID.ToLower())) && (projectSearchModel.Exclude_TipID))
                        select fli).ToList<TipSummary>();
            }

            if (!String.IsNullOrEmpty(projectSearchModel.StipId))
            {
                list = (from fli in list
                        where (fli.StipId.ToLower().Contains(projectSearchModel.StipId.ToLower()))
                        select fli).ToList<TipSummary>();
            }

            if (!String.IsNullOrEmpty(projectSearchModel.SponsorAgency))
            {
                list = (from fli in list
                        where ((fli.SponsorAgency == projectSearchModel.SponsorAgency) && (!projectSearchModel.Exclude_SponsorAgency))
                        || ((fli.SponsorAgency != projectSearchModel.SponsorAgency) && (projectSearchModel.Exclude_SponsorAgency))
                        select fli).ToList<TipSummary>();
            }

            if (!String.IsNullOrEmpty(projectSearchModel.ImprovementType))
            {
                list = (from fli in list
                        where ((fli.ImprovementType == projectSearchModel.ImprovementType) && (!projectSearchModel.Exclude_ImprovementType))
                        || ((fli.ImprovementType != projectSearchModel.ImprovementType) && (projectSearchModel.Exclude_ImprovementType))
                        select fli).ToList<TipSummary>();
            }

            if (!String.IsNullOrEmpty(projectSearchModel.AmendmentStatus))
            {
                list = (from fli in list
                        where ((fli.AmendmentStatus == projectSearchModel.AmendmentStatus) && (!projectSearchModel.Exclude_AmendmentStatus))
                        || ((fli.AmendmentStatus != projectSearchModel.AmendmentStatus) && (projectSearchModel.Exclude_AmendmentStatus))
                        select fli).ToList<TipSummary>();
            }

            if (!String.IsNullOrEmpty(projectSearchModel.ProjectType))
            {
                list = (from fli in list
                        where ((fli.ProjectType == projectSearchModel.ProjectType) && (!projectSearchModel.Exclude_ProjectType))
                        || ((fli.ProjectType != projectSearchModel.ProjectType) && (projectSearchModel.Exclude_ProjectType))
                        select fli).ToList<TipSummary>();
            }

            if (!String.IsNullOrEmpty(projectSearchModel.ProjectName))
            {
                list = (from fli in list
                        where ((fli.ProjectName.ToLower().Contains(projectSearchModel.ProjectName.ToLower())) && (!projectSearchModel.Exclude_ProjectName))
                        || ((!fli.ProjectName.ToLower().Contains(projectSearchModel.ProjectName.ToLower())) && (projectSearchModel.Exclude_ProjectName))
                        select fli).ToList<TipSummary>();
            }

            return list;
        }

        /// <summary>
        /// Get a list of projects that can have a new amendment
        /// created, for the specified TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public IList<TipSummary> GetAmendableTIPProjects(string tipYear)
        {
            IList<TipSummary> list = new List<TipSummary>();
            SqlCommand cmd = new SqlCommand("TIP.GetAmendableProjects");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TIPYEAR", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = tipYear;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new TipSummary()
                    {
                        SponsorAgency = rdr["Sponsor"].ToString()
                     ,  TipId = rdr["TIPID"].ToString()
                     ,  TipYear = tipYear
                     ,  Title = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : "NULL IN DATABASE"
                     ,  ProjectVersionId = (int)rdr["TipProjectVersionId"]
                     ,  AmendmentStatus = rdr["AmendmentStatus"] != DBNull.Value ? rdr["AmendmentStatus"].ToString() : ""
                     ,  AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? (DateTime)rdr["AmendmentDate"] : (DateTime)DateTime.MinValue
                     ,  ProjectType = rdr["ProjectType"] != DBNull.Value ? rdr["ProjectType"].ToString(): ""
                     ,  VersionStatus = rdr["ProjectVersionStatus"] != DBNull.Value ? rdr["ProjectVersionStatus"].ToString() : ""
                     ,  COGID = rdr["COGID"].ToString()
                    });
                }

            }
            return list;
        }

        /// <summary>
        /// Gets a list of the projects with active amendments in the
        /// specified TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public IList<TipSummary> GetActiveTIPProjects(string tipYear)
        {
            IList<TipSummary> list = new List<TipSummary>();
            SqlCommand cmd = new SqlCommand("TIP.GetActiveAmendmentProjects");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TIPYEAR", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = tipYear;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new TipSummary()
                    {
                        SponsorAgency = rdr["Sponsor"].ToString()
                     ,  TipId = rdr["TIPID"].ToString()
                     ,  TipYear = tipYear
                     ,  Title = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : "NULL IN DATABASE"
                     ,  ProjectVersionId = (int)rdr["TipProjectVersionId"]
                     ,  AmendmentStatus = rdr["AmendmentStatus"] != DBNull.Value ? rdr["AmendmentStatus"].ToString() : ""
                     ,  AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? (DateTime)rdr["AmendmentDate"] : (DateTime)DateTime.MinValue
                     ,  ProjectType = rdr["ProjectType"] != DBNull.Value ? rdr["ProjectType"].ToString() : ""
                     ,  VersionStatus = rdr["ProjectVersionStatus"] != DBNull.Value ? rdr["ProjectVersionStatus"].ToString() : ""
                     ,  COGID = rdr["COGID"].ToString()
                    });
                }

            }
            return list;
        }

        private class ReportProject
        {
            public int ReportId { get; set; }
            public string ReportName { get; set; }
            public TipReportProject Project { get; set; }
        }

        /// <summary>
        /// Gets a list of Proposed amendments in TIP year
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public TipReports GetProposedTIPProjects(string tipYear)
        {
            TipReports reports = new TipReports();

            IList<ReportProject> list = new List<ReportProject>();


            SqlCommand cmd = new SqlCommand("[TIP].[GetTipReportProjectVersionDetails]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", GetYearId(tipYear, Enums.TimePeriodType.TimePeriod));

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new ReportProject() 
                    { 
                        ReportId = (int)rdr["ReportId"]
                        ,
                        ReportName = rdr["ReportName"].ToString()
                        , 
                        Project = new TipReportProject()
                        {
                            TipId = rdr["TIPID"].ToString()
                         ,  ProjectVersionId = (int)rdr["ProjectVersionId"]
                         ,  COGID = rdr["COGID"].ToString()
                         ,  ProjectName = rdr["ProjectName"].ToString()
                         ,  SponsorAgency = rdr["SponsorAgency"].ToString()
                         ,  AmendmentTypeId = (int)rdr["AmendmentTypeId"]
                         ,  IsOnHold = rdr["IsOnHold"].ToString().SmartParseDefault<bool>(default(bool))
                        }
                    });
                }
            }

            IEnumerable<int> reportIds = list.Select(x => x.ReportId).Distinct();
            foreach (int id in reportIds)
            {
                IList<TipReportProject> items = list.Where(x => x.ReportId == id).Select(x=>x.Project).ToList<TipReportProject>();
                TipReportProjects projects = new TipReportProjects();
                foreach(TipReportProject project in items)
                {
                    projects.Add(project);
                }
                reports.Add(new TipReport()
                {
                    Id = id
                    ,
                    Name = list.First(x => x.ReportId == id).ReportName
                    ,
                    projects = projects
                });
            }

            return reports;
        }

        /// <summary>
        /// Gets a list of projects in the waiting list for the
        /// current TIP
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public IList<TipSummary> GetWaitingListTIPProjects(string tipYear) 
        {
            IList<TipSummary> list = new List<TipSummary>();
            SqlCommand cmd = new SqlCommand("TIP.GetWaitingListProjects");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TIPYEAR", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = tipYear;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new TipSummary()
                    {
                        SponsorAgency = rdr["Sponsor"].ToString()
                     ,  TipId = rdr["TIPID"].ToString()
                     ,  TipYear = tipYear
                     ,  Title = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : "NULL IN DATABASE"
                     ,  ProjectVersionId = (int)rdr["TipProjectVersionId"]
                     ,  AmendmentStatus = rdr["AmendmentStatus"] != DBNull.Value ? rdr["AmendmentStatus"].ToString() : ""
                     ,  AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? (DateTime)rdr["AmendmentDate"] : (DateTime)DateTime.MinValue
                     ,  ProjectType = rdr["ProjectType"] != DBNull.Value ? rdr["ProjectType"].ToString() : ""
                     ,  VersionStatus = rdr["ProjectVersionStatus"] != DBNull.Value ? rdr["ProjectVersionStatus"].ToString() : ""
                     ,  COGID = rdr["COGID"].ToString()
                    });
                }

            }
            return list;

        }

        /// <summary>
        /// Gets a list of projects that can be restored on a given a tipYearDestination
        /// (to be excluded) and a tipYearSource (to pull from).
        /// </summary>
        /// <param name="tipYearDestination"></param>
        /// <param name="tipYearSource"></param>
        /// <returns></returns>
        public IList<TipSummary> GetTIPRestoreCandidateProjects(string tipYearDestination, int tipYearSourceID, string tipID)
        {
            IList<TipSummary> list = new List<TipSummary>();
            SqlCommand cmd = new SqlCommand("TIP.GetRestoreCandidateProjects");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@tipYearDestination", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = tipYearDestination;
            cmd.Parameters.Add(new SqlParameter("@tipYearSourceID", SqlDbType.Int));
            cmd.Parameters[1].Value = tipYearSourceID;
            cmd.Parameters.Add(new SqlParameter("@tipID", SqlDbType.NVarChar));
            cmd.Parameters[2].Value = tipID;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new TipSummary()
                    {
                        ProjectVersionId = (int)rdr["TipProjectVersionId"]
                     ,  TipId = rdr["TIPID"].ToString()
                     ,  TipYear = rdr["TipYear"].ToString()
                     ,  Title = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : "NULL IN DATABASE"
                     ,  ProjectType = rdr["ProjectType"] != DBNull.Value ? rdr["ProjectType"].ToString(): ""
                     ,  SponsorAgency = rdr["Sponsor"].ToString()
                     ,  AmendmentStatus = rdr["AmendmentStatus"] != DBNull.Value ? rdr["AmendmentStatus"].ToString() : ""
                     ,  AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? (DateTime)rdr["AmendmentDate"] : (DateTime)DateTime.MinValue
                     ,  VersionStatus = rdr["ProjectVersionStatus"] != DBNull.Value ? rdr["ProjectVersionStatus"].ToString() : ""
                     ,  COGID = rdr["COGID"].ToString()
                    });
                }

            }
            return list;
        }

        /// <summary>
        /// Update the TIP Status in the database
        /// </summary>
        /// <param name="model"></param>
        public void UpdateTipStatus(TipStatusModel model)
        {
            SqlCommand cmd = new SqlCommand("[TIP].[UpdateStatus]");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PROGRAMID ", model.ProgramId);
            cmd.Parameters.AddWithValue("@TIMEPERIODID ", model.TimePeriodId);
            cmd.Parameters.AddWithValue("@TIPYEAR ", model.TipYear);
            cmd.Parameters.AddWithValue("@CURRENT ", model.IsCurrent);
            cmd.Parameters.AddWithValue("@PENDING ", model.IsPending);
            cmd.Parameters.AddWithValue("@PREVIOUS ", model.IsPrevious);
            cmd.Parameters.AddWithValue("@NOTES ", model.Notes);
            cmd.Parameters.AddWithValue("@ADOPTIONDATE ", model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@GOVAPPROVALDATE ", model.GovernorApproval != null ? (object)model.GovernorApproval.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PUBLICHEARINGDATE ", model.PublicHearing != null ? (object)model.PublicHearing.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DOTDATE ", model.USDOTApproval != null ? (object)model.USDOTApproval.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@EPADATE ", model.EPAApproval != null ? (object)model.EPAApproval.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ShowDelayDate ", !model.ShowDelayDate.Equals(default(DateTime)) ? (object)model.ShowDelayDate : (object)DBNull.Value);

            this.ExecuteNonQuery(cmd);

        }

        /// <summary>
        /// Checks to see if the Agency in question is sponsoring
        /// any projects in the specified TIP. If it is, then
        /// it can not be dropped.
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public bool CanAgencyBeDropped(string tipYear, int organizationId)
        {
            return true;
        }

        /// <summary>
        /// Update the list of Eligible Agencies associated with a particular TIP
        /// </summary>
        /// <param name="model"></param>
        public void UpdateEligibleAgencies(string tipYear, List<int> AddedOrganizations, List<int> RemovedOrganizations)
        {
            //first remove the orgs that were dropped
            foreach (int orgId in RemovedOrganizations)
            {
                SqlCommand cmd = new SqlCommand("[TIP].[DeleteSponsorOrganization]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TIPYEAR ", tipYear);
                cmd.Parameters.AddWithValue("@SPONSORID ", orgId);
                this.ExecuteNonQuery(cmd);
            }

            //now add in the added orgs
            foreach (int orgId in AddedOrganizations)
            {
                SqlCommand cmd = new SqlCommand("[TIP].[InsertSponsorOrganization]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TIPYEAR ", tipYear);
                cmd.Parameters.AddWithValue("@SPONSORID ", orgId);
                this.ExecuteNonQuery(cmd);
            }

        }

        /// <summary>
        /// Gets the model for the TipFundingSource view
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public IList<FundingSourceModel> GetTipFundingSources(string tipYear)
        {
            var model = new FundingSourceViewModel();
            SqlCommand cmd = new SqlCommand("[TIP].[GetFundingSources]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TIPYEAR", tipYear);
            var list = new List<FundingSourceModel>();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    var fs= new FundingSourceModel();
                                        
                    fs.FundingTypeId = (int)rdr["FundingTypeId"];
                    fs.FundingType = rdr["FundingType"].ToString();
                    //fs.FundingLevel = rdr["FundingLevel"] != DBNull.Value ? rdr["FundingLevel"].ToString() : "NULL IN DB";
                    fs.Code= rdr["Code"].ToString();
                    fs.RecipentOrganization = new Organization() { OrganizationName = rdr["Recipient"].ToString() };
                    fs.SourceOrganization= new Organization() { OrganizationName = rdr["Source"].ToString() };
                    fs.TimePeriod = tipYear;
                    fs.Selector = "Not in DB";
                    fs.IsDiscretionary = (bool)rdr["Discretion"];
                    
                    list.Add(fs);
                }

            }
            return list;
            
        }



        /// <summary>
        /// Get a FundingSourceModel 
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="fundingTypeId"></param>
        /// <returns></returns>
        public FundingSourceModel GetFundingSourceModel(string tipYear, int fundingTypeId)
        {
            var model = new FundingSourceModel();
            SqlCommand cmd = new SqlCommand("[TIP].[GetFundingSource]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TIPYEAR", tipYear);
            cmd.Parameters.AddWithValue("@FUNDINGTYPEID", fundingTypeId);
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if(rdr.Read())
                {
                    model.FundingTypeId = (int)rdr["FundingTypeId"];
                    model.FundingType = rdr["FundingType"] != DBNull.Value ? rdr["FundingType"].ToString() : "NULL IN DB";
                    model.Code = rdr["Code"] != DBNull.Value ? rdr["Code"].ToString() : "NULL IN DB";
                    model.RecipentOrganization = rdr["Recipient"] != DBNull.Value ? GetOrganization((int)rdr["Recipient"]) : null;
                    model.SourceOrganization = rdr["Source"] != DBNull.Value ? GetOrganization((int)rdr["Source"]) : null;
                    model.TimePeriod = rdr["TimePeriod"] != DBNull.Value ? rdr["TimePeriod"].ToString() : "NULL IN DB";
                    //model.FundingLevel = rdr["FundingLevel"] != DBNull.Value ? GetFundingLevel((int)rdr["FundingLevel"]) : null;
                    model.IsDiscretionary = (bool)rdr["Discretion"];
                    model.FundingGroup = rdr["FundingGroupId"] != DBNull.Value ? GetFundingGroup((int)rdr["FundingGroupId"]) : null;
                }
            }
            //Now load the resources

            model.Resources = GetFundingResources(tipYear, fundingTypeId);

            return model;
            
        }

        public IList<FundingResource> GetFundingResources(string tipYear, int fundingTypeId)
        {
            //Now get the Resources (the money) associated with this
            SqlCommand cmd = new SqlCommand("[TIP].[GetFundingResources]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TIPYEAR", tipYear);
            cmd.Parameters.AddWithValue("@FUNDINGTYPEID", fundingTypeId);
            IList<FundingResource> list = new List<FundingResource>();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    FundingResource fr = new FundingResource();
                    //fr.Amount = rdr["Amount"] != DBNull.Value ? (double?)rdr["Amount"] : null;
                    fr.Amount = rdr["Amount"] != DBNull.Value ? double.Parse(rdr["Amount"].ToString()) : 0.00;
                    fr.StateWideAmount = rdr["StateWideAmount"] != DBNull.Value ? double.Parse(rdr["StateWideAmount"].ToString()) : 0.00;
                    fr.FundingResourceId = (int)rdr["FundingResourceId"];
                    fr.Period = rdr["FundingIncrementId"] != DBNull.Value ? GetFundingPeriod((int)rdr["FundingIncrementId"]) : null;
                    list.Add(fr);
                }
            }
            return list;
        }

        public FundingPeriod GetFundingPeriod(int id)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[GetFundingLevels]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            FundingPeriod model = new FundingPeriod();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    model.Name = rdr["Name"] != DBNull.Value ? rdr["Name"].ToString() : "";
                    model.Id = rdr["ID"] != DBNull.Value ? (int?)rdr["ID"] : null;
                }
                else
                {
                    model = null;
                }
            }
            return model; 
        }

        public FundingLevel GetFundingLevel(int id)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[GetFundingLevel]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            FundingLevel model = new FundingLevel();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    model.Name = rdr["Name"] != DBNull.Value ? rdr["Name"].ToString() : "";
                    model.Id = rdr["ID"] != DBNull.Value ? (int?)rdr["ID"] : null;
                }
                else
                {
                    model = null;
                }
            }
            return model; 
        }

        public FundingGroup GetFundingGroup(int id)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[GetFundingGroup]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", id);
            FundingGroup model = new FundingGroup();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    model.Name = rdr["Name"] != DBNull.Value ? rdr["Name"].ToString() : "";
                    model.Id = rdr["ID"] != DBNull.Value ? (int?)rdr["ID"] : null;
                }
                else
                {
                    model = null;
                }
            }
            return model; 
        }

        /// <summary>
        /// Get an <see cref="Organization"/>
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public Organization GetOrganization(int organizationId)
        {

            SqlCommand cmd = new SqlCommand("[dbo].[GetOrganization]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ORGANIZATIONID", organizationId);
            Organization model = new Organization();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    model.OrganizationId = organizationId;
                    model.OrganizationName = rdr["OrganizationName"] != DBNull.Value ? rdr["OrganizationName"].ToString() : "NULL IN DB";
                }
                else
                {
                    //return null
                    model = null;
                }
            }
            return model;
        }

        public TipSummary GetTIPSummary(string tipYear)
        {
            //Now get the Resources (the money) associated with this
            SqlCommand cmd = new SqlCommand("[TIP].[GetSummary]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TIPYEAR", tipYear);
            TipSummary model = new TipSummary();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    model.TipYearTimePeriodID = rdr["TimePeriodId"] != DBNull.Value ? (short)rdr["TimePeriodId"] : default(short);  
                    model.TipYear = tipYear;                    
                    model.IsCurrent = rdr["IsCurrent"] != DBNull.Value ? (bool)rdr["IsCurrent"] : false;           
                }
                else
                {
                    //return null
                    model = null;
                }
            }
            return model;
            
        }

        public ProjectSearchViewModel GetProjectSearchViewModel (string currentProgram, string year)
        {
            var result = new ProjectSearchViewModel();

            // get project search model. This is now done in the (Tip)Controller.
            //result.ProjectSearchModel = this.GetProjectSearchModel();

            // fill form collections
            result.AvailableSponsors = GetCurrentTimePeriodSponsorAgencies(year, _appState).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
            //result.AvailableSponsors = GetLookupCollection("Lookup_GetSponsorOrganizations", "Id", "Label");
            result.AvailableTipYears = GetLookupCollection("Lookup_GetTipYears", "Id", "Label");
            result.AvailableImprovementTypes = GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
            result.AvailableProjectTypes = GetLookupCollection("Lookup_GetProjectTypes", "Id", "Label");
            IList<SqlParameter> fundingParam = new List<SqlParameter>();
            fundingParam.Add(new SqlParameter("@ProgramId", (int)StringEnum.Parse(typeof(Enums.ApplicationState), currentProgram)));
            fundingParam.Add(new SqlParameter("@TimePeriodId", result.AvailableTipYears.FirstOrDefault(x=>x.Value == year).Key));
            result.AvailableFundingTypes = GetLookupCollection("Lookup_GetFundingTypes", "FundingTypeID", "FundingType", fundingParam);

            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("@ProgramId", (int)StringEnum.Parse(typeof(Enums.ApplicationState), currentProgram)));
            result.AvailableVersionStatuses = GetLookupCollection("dbo.Lookup_GetVersionStatuses", "Id", "Label", sqlParms);
            result.AvailableGeographies = GetLookupCollection("[dbo].[Lookup_GetGeographies]", "Label");

            var regions = GetCategories(27).Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() }).ToList();
            regions.Insert(0, new SelectListItem { Text = "---(Include all or select from list)---", Value = "" });
            result.CdotRegions = regions;

           //Select the list of statuses appropriate for this program
            string statusType = "";
            switch (currentProgram)
            {
                case "Transportation Improvement Plan":
                    statusType = "Amendment Status";
                    break;
                case "Regional Transportation Plan":
                    statusType = "RTP Amendment Status";
                    break;
                case "Transportation Improvement Survey":
                    statusType = "Survey Amendment Status";
                    break;
                default:
                    statusType = "Amendment Status";
                    break;
            }

             IList<SqlParameter> paramList = new List<SqlParameter>();
             paramList.Add(new SqlParameter("@StatusType", statusType));
             result.AvailableAmendmentStatuses = GetLookupCollection("Lookup_GetStatuses", "Id", "Label", paramList);

            return result;
        }

        public ReportsViewModel GetReportsViewModel(string currentProgram, string year)
        {
            var result = new ReportsViewModel();

            result.CurrentSponsors = GetCurrentTimePeriodSponsorAgencies(year, _appState).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
            result.AvailableAmendmentDates = AvailableAmendmentDates(GetYearId(year, Enums.TimePeriodType.TimePeriod));
            result.AvailableAlopReport = GetAlopReportList();

            result.ReportDetails = GetProposedTIPProjects(year);
            Dictionary<int, string> list = new Dictionary<int, string>();

            //foreach (TipSummary project in result.ReportDetails.Projects)
            //{
            //    list.Add(project.ProjectVersionId, project.TipId + " : " + project.COGID);
            //}
            //result.CurrentPendingProjects = list;
            result.TipSummary = GetTIPSummary(year);

            return result;
        }

        public AmendmentsViewModel GetAmendmentsViewModel(string year)
        {
            var result = new AmendmentsViewModel();
            result.TipSummary.TipYear = year;
            result.CurrentSponsors = GetCurrentTimePeriodSponsorAgencies(year, Enums.ApplicationState.TIP).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
            
            var allowedTypes = new List<Enums.AmendmentType>();
            allowedTypes.Add(Enums.AmendmentType.Administrative);
            allowedTypes.Add(Enums.AmendmentType.Policy);
            result.AmendmentTypes = GenerateDictionary(allowedTypes);
                    
            return result;
        }

        public IDictionary<int, string> GenerateDictionary(IList<Enums.AmendmentType> types)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (Enums.AmendmentType type in types)
            {
                result.Add((int)type, StringEnum.GetStringValue(type));
            }

            return result;
        }

        protected IDictionary<DateTime, string> AvailableAmendmentDates(int year)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("@TimePeriodId", year));
            sqlParms.Add(new SqlParameter("@WithIgnore", true));
            var rawValues = GetLookupCollection("[TIP].[GetAmendmentDates]", "ignore", "AmendmentDate", sqlParms);
            var uniqueValues = rawValues.Values.Distinct().ToList();

            Dictionary<DateTime, string> endValues = new Dictionary<DateTime, string>();

            foreach (string record in uniqueValues)
            {
                DateTime temp = record.SmartParse<DateTime>().Date;
                endValues.Add(temp, temp.ToShortDateString());
            }
            return endValues;
        }

        /// <summary>
        /// Create a new Project in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateProject(string projectName, string facilityName, string tipYear, int sponsorOrganizationId, int amendmentTypeId)
        {
            IProjectRepository _repo = new ProjectRepository(FileRepository);
            return _repo.CreateProject(projectName, facilityName, tipYear, sponsorOrganizationId, amendmentTypeId);
        }
        
        public IDictionary<int, string> GetAvailableSponsors()
        {
            return GetLookupCollection("Lookup_GetSponsorOrganizations", "Id", "Label");
        }

        public IDictionary<int, string> GetAvailableTipYears()
        {
            return GetLookupCollection("[TIP].[GetPrograms]", "TimePeriodId", "TimePeriod");
        }

        public String UpdateReportProjectVersionOrder(int reportId, string projects)
        {
            string result = String.Empty;

            var modifiedOrder = new List<int>(GetModifiedArrayIndexes(projects));

            int position = 0;
            if (modifiedOrder.Count > 0)
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (var item in modifiedOrder)
                    {
                        if (!UpdateReportProjectVersionSort(reportId, (int)item, ++position))
                        {
                            throw new Exception("Sort Exception");
                        }
                    }
                    ts.Complete();
                }
            }

            return result;
        }

        public String SetReportProjectVersionOnHold(int reportId, int projectVersionId)
        {
            string result = String.Empty;

            if (!UpdateReportProjectVersionSort(reportId, projectVersionId, null))
            {
                throw new Exception("Sort Exception");
            }

            return result;
        }


        private bool UpdateReportProjectVersionSort(int reportId, int projectVersionId, int? order)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[UpdateReportProjectVersionSort]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
            cmd.Parameters.AddWithValue("@ReportId", reportId);
            cmd.Parameters.AddWithValue("@Order", order);
            SqlParameter sqlout = new SqlParameter("@error", SqlDbType.Bit);
            sqlout.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(sqlout);


            try
            {
                this.ExecuteNonQuery(cmd);
                if ((bool)cmd.Parameters["@error"].Value)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        private IEnumerable<int> GetModifiedArrayIndexes(string positions)
        {
            IEnumerable<string> values = positions.Split(new char[] { ',' });

            foreach (var position in values)
            {
                yield return Convert.ToInt32(position.Replace("pv_", ""));
            }
        }

        public IList<TipSummary> GetProjectsByAmendmentStatusId(int timePeriodId, Enums.TIPAmendmentStatus amendmentStatus)
        {
            IList<TipSummary> list = new List<TipSummary>();

            try
            {
                using (SqlCommand command = new SqlCommand("[TIP].[GetProjects]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
                    command.Parameters.AddWithValue("@VersionStatusId", (int)Enums.TIPVersionStatus.Pending);
                    command.Parameters.AddWithValue("@AmendmentStatusId", (int)amendmentStatus);

                    using (IDataReader rdr = this.ExecuteReader(command))
                    {
                        while (rdr.Read())
                        {
                            TipSummary summary = new TipSummary()
                            {
                                SponsorAgency = rdr["Sponsor"].ToString()
                                ,
                                ProjectName = rdr["ProjectName"].ToString()
                                ,
                                TipYear = rdr["TipYear"].ToString()
                                ,
                                TipId = rdr["TIPID"].ToString()
                                ,
                                ProjectVersionId = rdr["TIPProjectVersionID"] != DBNull.Value ? rdr["TIPProjectVersionID"].ToString().SmartParseDefault(default(int)) : default(int)
                            };
                            list.Add(summary);
                        }
                    }
                }
            }
            catch
            {

            }

            return list;
        }

        public string GetCurrentTimePeriod()
        {
            string timeperiod = String.Empty;

            SqlCommand cmd = new SqlCommand("[TIP].[GetCurrentTimePeriod]");
            cmd.CommandType = CommandType.StoredProcedure;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    timeperiod = rdr["TimePeriod"].ToString();
                }
            }
            return timeperiod;
        }

        public void SetProjectSearchDefaults(TIPSearchModel model)
        {
            SqlCommand cmd = new SqlCommand("[Tip].[GetSearchDefaults]");
            cmd.CommandType = CommandType.StoredProcedure;
            int testval;
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    model.TipYearID = (!String.IsNullOrEmpty(rdr["Id"].ToString()) && Int32.TryParse(rdr["Id"].ToString(), out testval)) ? Int32.Parse(rdr["Id"].ToString()) : 0;
                    model.TipYear = rdr["Name"].ToString();
                    break; // for now this is how it has to be. Just getting a single record.
                }
            }
        }

#endregion

        //#region FAKED ITipRepository Members

        //public IList<TipStatusModel> GetTIPSummaries()
        //{
        //    return _fakeTipRepo.GetTIPSummaries();
        //}

        //public TipStatusModel GetTIPStatusModelById(int tipId)
        //{
        //    return _fakeTipRepo.GetTIPStatusModelById(tipId);
        //}

        //public TipSummary GetProjectSummaryById(int projectId)
        //{
        //    return _fakeTipRepo.GetProjectSummaryById(projectId);
        //}

        //public ProjectModel GetProjectById(int projectId)
        //{
        //    throw new NotImplementedException();
        //}

        //public IList<TipSortedProjectListModel> GetTIPProjectList()
        //{
        //    return _fakeTipRepo.GetTIPProjectList();
        //}

        //#endregion



        public IDictionary<string, string> GetAlopReportList()
        {
            IDictionary<string, string> list = new Dictionary<string, string>();

            try
            {
                using (SqlCommand command = new SqlCommand("alop.ProcessRawData") { CommandType = CommandType.StoredProcedure })
                {
                    using (IDataReader rdr = this.ExecuteReader(command))
                    {
                        while (rdr.Read())
                        {
                            list.Add(new KeyValuePair<string, string>(
                                "0", "Error: " + rdr["TipId"].ToString()
                                ));
                        }
                    }
                }
                using (SqlCommand command = new SqlCommand("SELECT * FROM alop.Report ORDER BY ReportYear DESC") { CommandType = CommandType.Text })
                {
                    using (IDataReader rdr = this.ExecuteReader(command))
                    {
                        while (rdr.Read())
                        {
                            list.Add(new KeyValuePair<string, string>(
                                Common.Util.ShortGuid.Encode(rdr["Id"].ToString()), rdr["ReportYear"].ToString()
                                ));
                        }
                    }
                }
            }
            catch
            {

            }

            return list;
        }

        public DataTable GetAlopReportResults(string reportId)
        {
            DataTable result;

            try
            {
                using (SqlCommand command = new SqlCommand("alop.GetReportResults") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@ReportId", Common.Util.ShortGuid.Decode(reportId));

                    result = this.ExecuteDataTable(command);
                }
            }
            catch
            {
                result = new DataTable();
            }

            return result;
        }
    }
}
