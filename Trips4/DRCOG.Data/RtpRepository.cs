using System;
using DRCOG.Domain;
using DRCOG.Domain.ViewModels.RTP;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using DRCOG.Entities;
using DRCOG.Domain.Models.RTP;
using System.Collections.Generic;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Helpers;
using System.Diagnostics;
using System.Transactions;
using DRCOG.Domain.Models.Survey;
using DRCOG.Domain.Models;
using DRCOG.Common.Util;

namespace DRCOG.Data
{
    public class RtpRepository : TransportationRepository, IRtpRepository
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public RtpRepository()
        {
            _appState = Enums.ApplicationState.RTP;
        }
        
        private IDictionary<int, string> GetPlanScenarios(List<SqlParameter> sqlParams)
        {
            return GetLookupCollection("[RTP].[GetPlanScenarios]", "NetworkID", "Scenario", sqlParams);
        }

        [Obsolete("IsActive Deprecated", true)]
        private IDictionary<int, string> GetPlanScenarios(List<SqlParameter> sqlParams, bool isActive)
        {
            sqlParams.Add(new SqlParameter("@IsActive", isActive));
            return GetLookupCollection("[RTP].[GetPlanScenarios]", "NetworkID", "Scenario", sqlParams);
        }

        public IDictionary<int, string> GetPlanScenariosByCycleId(int cycleId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@CycleId", cycleId));

            return GetPlanScenarios(sqlParams);
        }

        public IDictionary<int, string> GetPlanScenariosForCurrentCycle(int planYearId)
        {
            var cycle = GetCurrentCycle(planYearId); //GetAmendmentDetails(planYearId, RTPCycleStatus.Active);
            
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@CycleId", cycle.Id));
            sqlParams.Add(new SqlParameter("@PlanYearId", planYearId));

            return GetPlanScenarios(sqlParams);
        }

        public IDictionary<int, string> GetPlanScenariosByCycle(string cycle)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@Cycle", cycle));

            return GetPlanScenarios(sqlParams);
        }
        public IDictionary<int, string> GetPlanScenariosByOpenYear(string openYear)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@OpenYear", openYear));

            return GetPlanScenarios(sqlParams);
        }

        public IDictionary<int, string> GetPlanScenarios(int planYearId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@PlanYearId", planYearId));
            
            return GetPlanScenarios(sqlParams);
            
        }

        public Cycle GetAmendmentDetails(int timePeriodId, Enums.RTPCycleStatus status)
        {
            return GetCycleDetails(timePeriodId, status);
        }

        


        /// <summary>
        /// Populate view model for Plan List
        /// </summary>
        /// <returns></returns>
        public RtpListViewModel GetRtpListViewModel()
        {
            RtpListViewModel model = new RtpListViewModel();

            SqlCommand cmd = new SqlCommand("[RTP].[GetPrograms]");
            cmd.CommandType = CommandType.StoredProcedure;

            IList<RtpStatusModel> rtpPrograms = new List<RtpStatusModel>();
            RtpStatusModel sm = null;
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                //be sure we got a reader                
                while (rdr.Read())
                {
                    sm = new RtpStatusModel();

                    sm.ProgramId = (int)rdr["ProgramId"];
                    sm.TimePeriodId = Convert.ToInt32(rdr["TimePeriodId"]);
                    sm.Plan = rdr["TimePeriod"].ToString();
                    sm.Adoption = rdr["AdoptionDate"] != DBNull.Value ? (DateTime?)rdr["AdoptionDate"] : null;
                    sm.CDOTAction = rdr["CDOTActionDate"] != DBNull.Value ? (DateTime?)rdr["CDOTActionDate"] : null;
                    sm.LastAmended = rdr["LastAmendmentDate"] != DBNull.Value ? (DateTime?)rdr["LastAmendmentDate"] : null;
                    sm.PublicHearing = rdr["PublicHearingDate"] != DBNull.Value ? (DateTime?)rdr["PublicHearingDate"] : null;
                    sm.USDOTApproval = rdr["USDOTApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USDOTApprovalDate"] : null;
                    sm.Notes = rdr["Notes"].ToString();
                    //sm.IsCurrent = (bool)rdr["Current"];
                    //sm.IsPending = (bool)rdr["Pending"];
                    //sm.IsPrevious = (bool)rdr["Previous"];

                    sm.RtpSummary = GetSummary(sm.Plan);

                    rtpPrograms.Add(sm);

                }
            }
            model.RTPs = rtpPrograms;
            return model;
        }


        #region IRtpRepository Members

        /// <summary>
        /// Create a new RTP Plan
        /// </summary>
        /// <param name="startYear"></param>
        /// <param name="endYear"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public void CreateRtp(string timePeriod)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[CreatePlan]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriod ", timePeriod);
            cmd.Parameters.AddWithValue("@StatusId ", Enums.RtpTimePeriodStatus.New);
            this.ExecuteNonQuery(cmd);
        }

        public void SetPlanCurrent(int timePeriodId)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[SetPlanCurrent]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
            this.ExecuteNonQuery(cmd);
        }

        public int CreateCategory(string categoryName, string shortName, string description, string plan)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[CreateCategory]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriod", plan);
            cmd.Parameters.AddWithValue("@CategoryName", categoryName);
            cmd.Parameters.AddWithValue("@ShortName", shortName);
            cmd.Parameters.AddWithValue("@Description", description);
            SqlParameter outParam = new SqlParameter("@CategoryId", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outParam);
            this.ExecuteNonQuery(cmd);
            return (int)cmd.Parameters["@CategoryId"].Value;
        }


        public RtpDashboardViewModel GetDashboardViewModel(string financialYear, Enums.RTPDashboardListType type)
        {
            RtpDashboardViewModel model = new RtpDashboardViewModel();
            RtpSummary rtpSummary = GetSummary(financialYear);

            //We call different sprocs based on the Enum
            string sprocName = "[RTP].[GetDashboardListByAmendmentStatus]";
            int versionStatusId = rtpSummary.IsAmendable() ? (int)Enums.RTPVersionStatus.Pending : (int)Enums.RTPVersionStatus.Active;
            switch (type)
            {
                case Enums.RTPDashboardListType.ProjectType:
                    sprocName = "[RTP].[GetDashboardListByProjectType]";
                    break;
                case Enums.RTPDashboardListType.Sponsor:
                    sprocName = "[RTP].[GetDashboardListBySponsor]";
                    break;
                case Enums.RTPDashboardListType.AmendmentStatus:
                    sprocName = "[RTP].[GetDashboardListByAmendmentStatus]";
                    break;
                case Enums.RTPDashboardListType.ImprovementType:
                    sprocName = "[RTP].[GetDashboardListByImprovementType]";
                    break;
                case Enums.RTPDashboardListType.SponsorWithTipid:
                    sprocName = "[RTP].[GetDashboardListWithTipId]";
                    break;
                default:
                    sprocName = "[RTP].[GetDashboardListBySponsor]";
                    break;
            }

            SqlCommand cmd = new SqlCommand(sprocName);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PlanName", financialYear);
            //cmd.Parameters.AddWithValue("@VersionStatusId", versionStatusId); // removed not sure if we need this.
            //if (!rtpSummary.IsPending) cmd.Parameters.AddWithValue("@IsActive", 1);
            model.RtpSummary.RtpYear = financialYear;

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

            //model.RtpStatus.IsCurrent = financialYear == GetCurrentRtpPlanYear();
            model.RtpSummary = rtpSummary;

            return model;
        }

        public IList<RtpSummary> GetRTPProjects(RTPSearchModel projectSearchModel)
        {
            //Formerly (DTS) accepted a string 'tipYear' variable and called 'TIP-GetTipProjects'.
            //What I would like to do is make a dynamic Linq statement with the parameters from ProjectSearchModel. -DBD
            IList<RtpSummary> list = new List<RtpSummary>();

            SqlCommand cmd = new SqlCommand("[RTP].[GetProjects]");
            cmd.CommandType = CommandType.StoredProcedure;

            //I will speed up these queries by restricting the list on three items: TipYear, TipYearID or IsActive. -DBD
            if (!projectSearchModel.Exclude_Year) // If we are excluding a TipYear, then we must return everything (no SQL optimization)
            {
                if (projectSearchModel.RtpYearID != null) cmd.Parameters.AddWithValue("@YEARID", projectSearchModel.RtpYearID);
                if (projectSearchModel.NetworkID > 0) cmd.Parameters.AddWithValue("@NetworkID", projectSearchModel.NetworkID);
            }
            if (projectSearchModel.VersionStatusId > 0)
            {
                cmd.Parameters.AddWithValue("@VersionStatusId", projectSearchModel.VersionStatusId);
            }

            if (projectSearchModel.CycleId > 0)
            {
                cmd.Parameters.AddWithValue("@CycleId", projectSearchModel.CycleId);
            }

            if (projectSearchModel.ShowCancelledProjects)
            {
                cmd.Parameters.AddWithValue("@ExcludeCancelled", !projectSearchModel.ShowCancelledProjects);
            }

            //if ( projectSearchModel.CycleId.Equals(default(int)) ) cmd.Parameters.AddWithValue("@CycleId", projectSearchModel.CycleId);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    RtpSummary summary = new RtpSummary()
                    {
                        SponsorAgency = rdr["Sponsor"].ToString()
                        ,
                        TIPId = rdr["TIPID"].ToString()
                        ,
                        TipTimePeriod = rdr["TIPTimePeriod"].ToString()
                        ,
                        RtpId = rdr["RtpYear"].ToString()
                        ,
                        RtpYear = rdr["RtpYear"] != DBNull.Value ? rdr["RtpYear"].ToString() : "NULL IN DATABASE"
                        ,
                        Title = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : "NULL IN DATABASE"
                        ,
                        ProjectVersionId = (int)rdr["RTPProjectVersionID"]
                        ,
                        AmendmentStatus = rdr["AmendmentStatus"] != DBNull.Value ? rdr["AmendmentStatus"].ToString() : ""
                        ,
                        AmendmentStatusId = rdr["AmendmentStatusId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        ImprovementType = rdr["ImprovementType"] != DBNull.Value ? rdr["ImprovementType"].ToString() : ""
                        ,
                        ProjectType = rdr["ProjectType"] != DBNull.Value ? rdr["ProjectType"].ToString() : ""
                        ,
                        PlanType = rdr["PlanType"].ToString()
                        ,
                        ProjectName = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : ""
                        ,
                        // Set ToUpper to ensure results found. No case sensitive required.
                        COGID = rdr["COGID"] != DBNull.Value ? rdr["COGID"].ToString().ToUpper() : ""
                        ,
                        VersionStatus = rdr["ProjectVersionStatus"] != DBNull.Value ? rdr["ProjectVersionStatus"].ToString() : ""
                    };
                    summary.Cycle.Name = rdr["CycleName"].ToString();
                    summary.Cycle.Id = rdr["CycleId"].ToString().SmartParseDefault<int>(default(int));
                    list.Add(summary);

                }
            }

            // These are processed via the SQL retrieval code optimization, so only process this one when exclude is checked.
            if ((projectSearchModel.RtpYear != null) && (projectSearchModel.Exclude_Year))
            {
                list = (from fli in list
                        where (projectSearchModel.RtpYear.ToWildcardRegex().IsMatch(fli.RtpYear))
                        select fli).ToList<RtpSummary>();
            }

            //if ((projectSearchModel.VersionStatusId > 0) && (projectSearchModel.Exclude_ActiveVersion))
            //{
            //    list = (from fli in list
            //            where (!fli.VersionStatus.Equals(projectSearchModel.VersionStatus))
            //            select fli).ToList<RtpSummary>();
            //}

            

            // Set ToUpper to ensure results found. No case sensitive required.
            if (!String.IsNullOrEmpty(projectSearchModel.COGID))
            {
                projectSearchModel.COGID = projectSearchModel.COGID.ToUpper();
            }

            //Now that we have the base data, let's apply the rest of our parameters
            // Trying to list the paramters here by most restrictive first. Should make searches much quicker. -DBD
            if ((projectSearchModel.COGID != null) && (!projectSearchModel.COGID.Equals("")))
            {
                list = (from fli in list
                        where ((projectSearchModel.COGID.ToWildcardRegex().IsMatch(fli.COGID)) && (!projectSearchModel.Exclude_COGID))
                        || ((!projectSearchModel.COGID.ToWildcardRegex().IsMatch(fli.COGID)) && (projectSearchModel.Exclude_COGID))
                        select fli).ToList<RtpSummary>();
            }

            if ((projectSearchModel.RtpID != null) && (!projectSearchModel.RtpID.Equals("")))
            {
                list = (from fli in list
                        where ((projectSearchModel.RtpID.ToWildcardRegex().IsMatch(fli.RtpId)) && (!projectSearchModel.Exclude_ID))
                        || ((!projectSearchModel.RtpID.ToWildcardRegex().IsMatch(fli.RtpId)) && (projectSearchModel.Exclude_ID))
                        select fli).ToList<RtpSummary>();
            }

            if ((projectSearchModel.TipId != null) && (!projectSearchModel.TipId.Equals("")))
            {
                list = (from fli in list
                        where ((projectSearchModel.TipId.ToWildcardRegex().IsMatch(fli.TIPId)) && (!projectSearchModel.Exclude_TipId))
                        || ((!projectSearchModel.TipId.ToWildcardRegex().IsMatch(fli.TIPId)) && (projectSearchModel.Exclude_TipId))
                        select fli).ToList<RtpSummary>();
            }

            if (projectSearchModel.RequireTipId)
            {
                list = (from fli in list
                        where ((!String.IsNullOrEmpty(fli.TIPId)) && (projectSearchModel.RequireTipId))
                        select fli).ToList<RtpSummary>();
            }

            if (projectSearchModel.SponsorAgency != null)
            {
                list = (from fli in list
                        where ((fli.SponsorAgency == projectSearchModel.SponsorAgency) && (!projectSearchModel.Exclude_SponsorAgency))
                        || ((fli.SponsorAgency != projectSearchModel.SponsorAgency) && (projectSearchModel.Exclude_SponsorAgency))
                        select fli).ToList<RtpSummary>();
            }

            if (projectSearchModel.ImprovementType != null)
            {
                list = (from fli in list
                        where ((fli.ImprovementType == projectSearchModel.ImprovementType) && (!projectSearchModel.Exclude_ImprovementType))
                        || ((fli.ImprovementType != projectSearchModel.ImprovementType) && (projectSearchModel.Exclude_ImprovementType))
                        select fli).ToList<RtpSummary>();
            }

            if (projectSearchModel.AmendmentStatus != null)
            {
                list = (from fli in list
                        where ((fli.AmendmentStatus == projectSearchModel.AmendmentStatus) && (!projectSearchModel.Exclude_AmendmentStatus))
                        || ((fli.AmendmentStatus != projectSearchModel.AmendmentStatus) && (projectSearchModel.Exclude_AmendmentStatus))
                        select fli).ToList<RtpSummary>();
            }

            if (!String.IsNullOrEmpty(projectSearchModel.PlanType))
            {
                list = (from fli in list
                        where ((fli.PlanType == projectSearchModel.PlanType) && (!projectSearchModel.Exclude_PlanType))
                        || ((fli.PlanType != projectSearchModel.PlanType) && (projectSearchModel.Exclude_PlanType))
                        select fli).ToList<RtpSummary>();
            }

            if (projectSearchModel.ProjectType != null)
            {
                list = (from fli in list
                        where ((fli.ProjectType == projectSearchModel.ProjectType) && (!projectSearchModel.Exclude_ProjectType))
                        || ((fli.ProjectType != projectSearchModel.ProjectType) && (projectSearchModel.Exclude_ProjectType))
                        select fli).ToList<RtpSummary>();
            }

            if ((projectSearchModel.ProjectName != null) && (!projectSearchModel.ProjectName.Equals("")))
            {
                list = (from fli in list
                        where ((projectSearchModel.ProjectName.ToWildcardRegex().IsMatch(fli.ProjectName)) && (!projectSearchModel.Exclude_ProjectName))
                        || ((!projectSearchModel.ProjectName.ToWildcardRegex().IsMatch(fli.ProjectName)) && (projectSearchModel.Exclude_ProjectName))
                        select fli).ToList<RtpSummary>();
            }

            return list;
        }

        public IList<RtpSummary> GetRestoreProjectList(int timePeriodId)
        {
            IList<RtpSummary> list = new List<RtpSummary>();

            try
            {
                using (SqlCommand command = new SqlCommand("[RTP].[GetRestoreProjectList]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

                    using (IDataReader rdr = this.ExecuteReader(command))
                    {
                        while (rdr.Read())
                        {
                            RtpSummary summary = new RtpSummary()
                            {
                                ProjectId = rdr["ProjectId"] != DBNull.Value ? rdr["ProjectId"].ToString().SmartParseDefault(default(int)) : default(int)
                                ,
                                RTPYearTimePeriodID = rdr["TimePeriodId"] != DBNull.Value ? rdr["TimePeriodId"].ToString().SmartParseDefault(default(short)) : default(short)
                                ,
                                RtpYear = rdr["TimePeriod"].ToString()
                                ,
                                ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? rdr["ProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
                                ,
                                ProjectName = rdr["ProjectName"].ToString()
                                ,
                                LastAmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? rdr["AmendmentDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue) : DateTime.MinValue
                                ,
                                AmendmentStatusId = rdr["ProjectVersionId"] != DBNull.Value ? rdr["ProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
                                ,
                                AmendmentStatus = rdr["AmendmentStatus"].ToString()
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

        public IList<RtpSummary> GetAmendableProjects(int timePeriodId, int cycleId, bool excludeHasPending)
        {
            return GetAmendableProjects(timePeriodId, cycleId, excludeHasPending, false);
        }

        public IList<RtpSummary> GetAmendableProjects(int timePeriodId, int cycleId, bool excludeHasPending, bool showScenerio)
        {
            IList<RtpSummary> list = new List<RtpSummary>();

            try
            {
                using (SqlCommand command = new SqlCommand("[RTP].[GetProjects]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@YearID", timePeriodId);
                    command.Parameters.AddWithValue("@CycleId", cycleId);
                    command.Parameters.AddWithValue("@ExcludeHasPending", excludeHasPending);
                    command.Parameters.AddWithValue("@ShowScenario", showScenerio);
                    command.Parameters.AddWithValue("@ExcludeCancelled", true);

                    using (IDataReader rdr = this.ExecuteReader(command))
                    {
                        while (rdr.Read())
                        {
                            RtpSummary summary = new RtpSummary()
                            {
                                SponsorAgency = rdr["Sponsor"].ToString()
                                ,
                                ProjectName = rdr["ProjectName"].ToString()
                                ,
                                RtpYear = rdr["RtpYear"].ToString()
                                ,
                                ProjectVersionId = rdr["RTPProjectVersionId"] != DBNull.Value ? rdr["RTPProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
                            };
                            summary.Cycle.Name = rdr["CycleName"].ToString();
                            summary.Cycle.Id = rdr["CycleId"].ToString().SmartParseDefault<int>(default(int));
                            list.Add(summary);
                            //Logger.Debug("Amendment Status " + rdr["AmendmentStatusId"].ToString());
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.WarnException("Exception in RtpRepository.GetAmendableProjects", exc);
            }

            return list;
        }

        public IList<Cycle> GetPlanCycles(int timePeriodId)
        {
            IList<Cycle> list = new List<Cycle>();

            try
            {
                using (SqlCommand command = new SqlCommand("[RTP].[GetCurrentPlanCycles]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

                    using (IDataReader rdr = this.ExecuteReader(command))
                    {
                        while (rdr.Read())
                        {
                            Cycle summary = new Cycle()
                            {
                                Id = rdr["id"].ToString().SmartParse<int>()
                            ,
                                Name = rdr["cycle"].ToString()
                            ,
                                StatusId = rdr["statusId"].ToString().SmartParse<int>()
                            ,
                                Status = rdr["Status"].ToString()
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StatusViewModel GetRtpStatusViewModel(string year)
        {
            int intOut;
            StatusViewModel model = new StatusViewModel();

            SqlCommand cmd = new SqlCommand("[RTP].[GetStatus]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = year;

            model.RtpSummary.RtpYear = year;
            var sm = new RtpStatusModel();

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    sm.ProgramId = (int)rdr["ProgramId"];
                    sm.TimePeriodId = Convert.ToInt32(rdr["TimePeriodId"]);
                    sm.Plan = rdr["TimePeriod"].ToString();
                    sm.Adoption = rdr["AdoptionDate"] != DBNull.Value ? (DateTime?)rdr["AdoptionDate"] : null;
                    //sm.EPAApproval = rdr["USEPAApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USEPAApprovalDate"] : null;
                    //sm.GovernorApproval = rdr["GovernorApprovalDate"] != DBNull.Value ? (DateTime?)rdr["GovernorApprovalDate"] : null;
                    sm.LastAmended = rdr["LastAmendmentDate"] != DBNull.Value ? (DateTime?)rdr["LastAmendmentDate"] : null;
                    sm.PublicHearing = rdr["PublicHearingDate"] != DBNull.Value ? (DateTime?)rdr["PublicHearingDate"] : null;
                    sm.CDOTAction = rdr["CDOTActionDate"] != DBNull.Value ? (DateTime?)rdr["CDOTActionDate"] : null;
                    sm.USDOTApproval = rdr["USDOTApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USDOTApprovalDate"] : null;
                    sm.Notes = rdr["Notes"].ToString();
                    //sm.IsCurrent = (bool)rdr["Current"];
                    //sm.IsPending = (bool)rdr["Pending"];
                    //sm.IsPrevious = (bool)rdr["Previous"];
                    sm.BaseYearId = rdr["BaseYearId"] != DBNull.Value ? (Int32.TryParse(rdr["BaseYearId"].ToString(), out intOut) ? Int32.Parse(rdr["BaseYearId"].ToString()) : 0)  : 0;
                    sm.BaseYear = rdr["BaseYear"] != DBNull.Value ? rdr["BaseYear"].ToString() : String.Empty;
                    sm.Description = rdr["Description"].ToString();
                    model.RtpSummary.Cycle = new Cycle()
                    {
                        Id = rdr["CycleId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        Name = rdr["CycleName"].ToString()
                        ,
                        StatusId = rdr["CycleStatusId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        Status = rdr["CycleStatus"].ToString()
                    };
                }
            }
            

            model.RtpStatus = sm;

            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@TimePeriodTypeID", Enums.TimePeriodType.Year));
            model.AvailableYears = GetLookupCollection("[dbo].[Lookup_GetTimePeriodsByTypeId]", "Value", "Label", sqlParams);

            sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@TimePeriodId", sm.TimePeriodId));
            model.CurrentPlanCycles = GetLookupCollection("[RTP].[GetCurrentPlanCycles]", "id", "cycle", sqlParams);

            sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@TimePeriodId", sm.TimePeriodId));
            model.PlanUnusedCycles = GetLookupCollection("[RTP].[GetNextCycleKeys]", "id", "cycle", sqlParams);

            model.AvailableCycles = GetLookupCollection("[RTP].[GetAvailablePlanCycles]", "id", "cycle");

            model.Surveys = GetPlanSurveys(GetYearId(year, Enums.TimePeriodType.PlanYear));


            //Enums.RTPCycleStatus rtpCycleStatus = model.RtpStatus.IsCurrent ? Enums.RTPCycleStatus.Active : Enums.RTPCycleStatus.Pending;
            //model.RtpSummary.Cycle = GetAmendmentDetails(sm.TimePeriodId, rtpCycleStatus);
            
            return model;
        }

        public Surveys GetPlanSurveys(int timePeriodId)
        {
            SqlCommand cmd = new SqlCommand("[Survey].[GetPlanSurveys]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
            Surveys surveys = new Surveys();

            try
            {
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    while (rdr.Read())
                    {
                        surveys.Add(new Survey()
                        {
                            Id = rdr["SurveyTimePeriodId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            Name = rdr["TimePeriod"].ToString()
                            ,
                            OpeningDate = rdr["OpeningDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue)
                            ,
                            ClosingDate = rdr["ClosingDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue)
                        });
                    }
                }

                return surveys;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public String SetActiveCycle(int cycleId, int timePeriodId)
        {
            string result = "";

            SqlCommand cmd = new SqlCommand("[RTP].[SetActiveCycle]");
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@CycleId", cycleId);
            SqlParameter sqlout = new SqlParameter("@CycleId", SqlDbType.Int);
            sqlout.Value = cycleId;
            sqlout.Direction = ParameterDirection.InputOutput;
            cmd.Parameters.Add(sqlout);
            cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
            //cmd.Parameters.AddWithValue("@CycleStatus", RTPCycleStatus.);

            try
            {
                this.ExecuteNonQuery(cmd);
                result = cmd.Parameters["@CycleId"].Value.ToString();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public String UpdateTimePeriodCycleOrder(string cycles)
        {
            string result = String.Empty;

            var modifiedOrder = new List<int>(GetModifiedArrayIndexes(cycles));

            int position = 0;
            if (modifiedOrder.Count > 0)
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    foreach (var item in modifiedOrder)
                    {
                        if (!UpdateCycleSort((int)item, ++position))
                        {
                            //error = true;
                            throw new Exception("Sort Exception");
                        }
                    }
                    ts.Complete();
                }
            }

            return result;
        }

        private bool UpdateCycleSort(int cycleId, int order)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[UpdateCycleSort]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CycleId", cycleId);
            cmd.Parameters.AddWithValue("@Order", order);
            SqlParameter sqlout = new SqlParameter("@error",SqlDbType.Bit);
            sqlout.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(sqlout);

            
            try
            {
                this.ExecuteNonQuery(cmd);
                if((bool)cmd.Parameters["@error"].Value)
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
                yield return Convert.ToInt32(position.Replace("cycle_", ""));
            }
        }

        /// <summary>
        /// Update the RTP Status in the database
        /// </summary>
        /// <param name="model"></param>
        public void UpdateRtpStatus(RtpStatusModel model)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[UpdateRtpStatus]");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PROGRAMID", model.ProgramId);
            cmd.Parameters.AddWithValue("@TIMEPERIODID", model.TimePeriodId);
            cmd.Parameters.AddWithValue("@YEAR", model.Plan);
            cmd.Parameters.AddWithValue("@BaseYearId", model.BaseYearId > 0 ? (object)model.BaseYearId : 0);
            //cmd.Parameters.AddWithValue("@CURRENT", model.IsCurrent);
            //cmd.Parameters.AddWithValue("@PENDING", model.IsPending);
            //cmd.Parameters.AddWithValue("@PREVIOUS", model.IsPrevious);
            cmd.Parameters.AddWithValue("@NOTES", model.Notes);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@ADOPTIONDATE", model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value);
            //cmd.Parameters.AddWithValue("@GOVAPPROVALDATE ", model.GovernorApproval != null ? (object)model.GovernorApproval.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@PUBLICHEARINGDATE", model.PublicHearing != null ? (object)model.PublicHearing.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@DOTDATE", model.USDOTApproval != null ? (object)model.USDOTApproval.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CDOTDATE", model.CDOTAction != null ? (object)model.CDOTAction.Value : (object)DBNull.Value);
            
            //cmd.Parameters.AddWithValue("@EPADATE ", model.EPAApproval != null ? (object)model.EPAApproval.Value : (object)DBNull.Value);

            this.ExecuteNonQuery(cmd);

        }

        /// <summary>
        /// Add a cycle to a TimePeriod
        /// </summary>
        /// <param name="timePeriodId"></param>
        /// <param name="cycleId"></param>
        public string AddCycleToTimePeriod(string timePeriod, int cycleId)
        {
            string result = "";

            SqlCommand cmd = new SqlCommand("[RTP].[AddCycleToTimePeriod]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", GetYearId(timePeriod, Enums.TimePeriodType.PlanYear));
            cmd.Parameters.AddWithValue("@CycleId", cycleId);
            cmd.Parameters.AddWithValue("@StatusId", Enums.RTPCycleStatus.Inactive);

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
        /// Remove a Cycle from TimePeriod
        /// </summary>
        /// <param name="cycleId"></param>
        public string RemoveCycleFromTimePeriod(int cycleId)
        {
            string result = "";

            SqlCommand cmd = new SqlCommand("[RTP].[RemoveCycleFromTimePeriod]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CycleId", cycleId);
            cmd.Parameters.AddWithValue("@StatusId", Enums.RTPCycleStatus.New);

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

        public string UpdateTimePeriodStatusId(int timePeriodId, int statusId)
        {
            string result = "";

            SqlCommand cmd = new SqlCommand("[dbo].[UpdateTimePeriodStatusId]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
            cmd.Parameters.AddWithValue("@StatusId", statusId);

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
        /// Create a Cycle
        /// </summary>
        /// <param name="cycle"></param>
        public string CreateCycle(string cycle)
        {
            string result = "";

            SqlCommand cmd = new SqlCommand("[RTP].[CreateCycle]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cycle", cycle);

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

        public string UpdateCycleName(int cycleId, string cycle)
        {
            string result = String.Empty;

            SqlCommand cmd = new SqlCommand("[RTP].[UpdateCycleName]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CycleId", cycleId);
            cmd.Parameters.AddWithValue("@Cycle", cycle);

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
        /// Create a new Project in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateProject(string projectName, string facilityName, string plan, int sponsorOrganizationId, int? cycleId)
        {
            int retval = default(int);
            Cycle cycle = cycleId.HasValue ? new Cycle() { Id = cycleId.Value } : GetCurrentCycle(GetYearId(plan, Enums.TimePeriodType.PlanYear));
            try
            {
                using (SqlCommand command = new SqlCommand("[RTP].[CreateProject]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@ProjectName", projectName);
                    command.Parameters.AddWithValue("@SponsorOrganizationId", sponsorOrganizationId);
                    command.Parameters.AddWithValue("@FacilityName", facilityName);
                    command.Parameters.AddWithValue("@Plan", plan);
                    command.Parameters.AddWithValue("@CycleId", cycle.Id);
                    command.Parameters.AddWithValue("@AmendmentStatusId", (int)Enums.RTPAmendmentStatus.Pending);
                    command.Parameters.AddWithValue("@VersionStatusId", (int)Enums.RTPVersionStatus.Pending);
                    //command.Parameters.AddWithValue("@AmendmentTypeId", (int)AmendmentType.Administrative);
                    SqlParameter outParam = new SqlParameter("@ProjectVersionId", SqlDbType.Int);
                    outParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(outParam);
                    this.ExecuteNonQuery(command);
                    retval = (int)command.Parameters["@ProjectVersionId"].Value;
                }
            }
            catch(Exception exc)
            {

            }

            return retval;
        }

        

        public IDictionary<int, string> GetPlanAvailableProjects(int planId, int cycleId)
        {
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@TimePeriodId", planId));
            sqlParams.Add(new SqlParameter("@CycleId", cycleId));

            //return GetLookupCollection("[RTP].[GetPlanAvailableProjects]", "RTPProjectVersionID", "COGID", sqlParams);

            Dictionary<string, string> valueAddonValueSeparator = new Dictionary<string, string>();
            valueAddonValueSeparator.Add("ProjectName", " - ");
            return GetLookupCollection("[RTP].[GetPlanAvailableProjects]", "RTPProjectVersionID", "Sponsor", sqlParams, valueAddonValueSeparator);
        }
        
        

        public ProjectSearchViewModel GetProjectSearchViewModel(string year,string currentProgram)
        {
            var result = new ProjectSearchViewModel();

            // get project search model. This is now done in the (RTP)Controller.
            //result.ProjectSearchModel = this.GetProjectSearchModel();

            // fill form collections
            result.EligibleAgencies = GetCurrentTimePeriodSponsorAgencies(year, _appState).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
            Dictionary<string,string> valueAddonValueSeparator = new Dictionary<string,string>();
            valueAddonValueSeparator.Add("Status", " :: ");
            valueAddonValueSeparator.Add("cycle", " - ");
            result.AvailablePlanYears = GetLookupCollection("[RTP].[Lookup_GetPlanYears]", "Id", "Label", null,valueAddonValueSeparator);
            result.AvailableImprovementTypes = AvailableImprovementTypes(24);
            //result.AvailableImprovementTypes = GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
            result.AvailableProjectTypes = GetLookupCollection("Lookup_GetProjectTypes", "Id", "Label");
            result.AvailableNetworks = GetLookupCollection("[dbo].[Lookup_GetNetworks]", "Id", "Label");

            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@StatusType", "RTP Amendment Status"));
            result.AvailableAmendmentStatuses = GetLookupCollection("Lookup_GetStatuses", "Id", "Label", paramList);
            result.AvailablePlanTypes = GetLookupCollection("[RTP].[Lookup_GetPlanTypes]", "CategoryID", "Category");
            result.RtpSummary = GetSummary(year);
            return result;
        }

        public void SetProjectSearchDefaults(RTPSearchModel model)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[GetSearchDefaults]");
            cmd.CommandType = CommandType.StoredProcedure;
            int testval;
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    model.RtpYearID = (!String.IsNullOrEmpty(rdr["Id"].ToString()) && Int32.TryParse(rdr["Id"].ToString(), out testval)) ? Int32.Parse(rdr["Id"].ToString()) : 0;
                    model.RtpYear = rdr["Plan"].ToString();
                    break; // for now this is how it has to be. Just getting a single record.
                }
            }
        }


        public SponsorsViewModel GetSponsorsViewModel(string plan)
        {
            //ToDo: Need to make the two methods that make up this viewModel be from the
            //TransportationRepository so that the ProjectRepository can access them. -DBD
            var model = new SponsorsViewModel();
            model.RtpSummary = this.GetSummary(plan);

            // Get Agencies which are eligible to sponsor projects
            model.EligibleAgencies = GetCurrentTimePeriodSponsorAgencies(plan, _appState);
            model.AvailableAgencies = GetAvailableTimePeriodSponsorAgencies(plan, _appState);

            return model;

        }

        public RtpSummary GetSummary(string plan)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Plan", plan));

            return GetSummary(parameters, plan);
        }

        public RtpSummary GetSummary(string plan, int cycleId)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@Plan", plan));
            parameters.Add(new SqlParameter("@CycleId", cycleId));

            return GetSummary(parameters, plan);
        }

        protected RtpSummary GetSummary(List<SqlParameter> parameters, string plan)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[GetSummary]");
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (SqlParameter param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            RtpSummary model = new RtpSummary();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    model.RtpYear = plan;
                    //model.IsCurrent = rdr["IsCurrent"] != DBNull.Value ? (bool)rdr["IsCurrent"] : false;
                    //model.IsPending = rdr["IsPending"] != DBNull.Value ? (bool)rdr["IsPending"] : false;
                    model.RTPYearTimePeriodID = rdr["TimePeriodID"].ToString().SmartParseDefault<short>(default(short));
                    model.TimePeriodStatusId = rdr["TimePeriodStatusId"].ToString().SmartParseDefault<int>(default(int));
                    model.Cycle = new Cycle()
                    {
                        Id = rdr["CycleId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        Name = rdr["CycleName"].ToString()
                        ,
                        StatusId = rdr["CycleStatusId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        Status = rdr["CycleStatus"].ToString()
                        ,
                        PriorCycleId = rdr["priorCycleId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        NextCycleId = rdr["nextCycleId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        NextCycleName = rdr["nextCycle"].ToString()
                        ,
                        NextCycleStatus = rdr["nextStatus"].ToString()
                    };
                }
            }


            //model.Cycle = new Cycle();
            //model.Cycle = GetCurrentCycle(GetYearId(plan));
            return model;
        }

        /// <summary>
        /// Update the list of Eligible Agencies associated with a particular RTP
        /// </summary>
        /// <param name="model"></param>
        public void UpdateEligibleAgencies(string plan, List<int> AddedOrganizations, List<int> RemovedOrganizations)
        {
            //first remove the orgs that were dropped
            foreach (int orgId in RemovedOrganizations)
            {
                SqlCommand cmd = new SqlCommand("[RTP].[DeleteSponsorOrganization]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Plan ", plan);
                cmd.Parameters.AddWithValue("@SponsorId ", orgId);
                this.ExecuteNonQuery(cmd);
            }

            //now add in the added orgs
            foreach (int orgId in AddedOrganizations)
            {
                SqlCommand cmd = new SqlCommand("[RTP].[InsertSponsorOrganization]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Plan ", plan);
                cmd.Parameters.AddWithValue("@SponsorId ", orgId);
                this.ExecuteNonQuery(cmd);
            }
        }

        public IList<RTPFundingSourceModel> GetFundingSources(string plan)
        {
            var model = new FundingSourceViewModel();
            SqlCommand cmd = new SqlCommand("[RTP].[GetFundingSources]");
            cmd.CommandType = CommandType.StoredProcedure;
            var list = new List<RTPFundingSourceModel>();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    var fs = new RTPFundingSourceModel();

                    fs.FundingTypeId = (int)rdr["FundingTypeId"];
                    fs.FundingType = rdr["FundingType"].ToString();
                    //fs.FundingLevel = rdr["FundingLevel"] != DBNull.Value ? rdr["FundingLevel"].ToString() : "NULL IN DB";
                    fs.Code = rdr["Code"].ToString();
                    fs.RecipentOrganization.OrganizationName = rdr["Recipient"].ToString();
                    fs.SourceOrganization.OrganizationName = rdr["Source"].ToString();
                    fs.Plan = plan;
                    //fs.Selector = "Not in DB";
                    fs.IsDiscretionary = (bool)rdr["Discretion"];

                    list.Add(fs);
                }

            }
            return list;

        }

        #endregion

    }
}
