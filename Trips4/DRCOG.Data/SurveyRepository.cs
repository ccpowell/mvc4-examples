using DRCOG.Domain;
using DRCOG.Domain.ViewModels.Survey;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using DRCOG.Entities;
using DRCOG.Domain.Models.Survey;
using System.Collections.Generic;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Helpers;
using System.Diagnostics;
using System.Transactions;
using DRCOG.Domain.Models;
using DRCOG.Common.Services.MemberShipServiceSupport;
using System.IO;
using System.Xml;
using System.Data.SqlTypes;
using System;
using DRCOG.Common.Util;

namespace DRCOG.Data
{
    public class SurveyRepository : TransportationRepository, ISurveyRepository
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public SurveyRepository()
        {
            _appState = DRCOG.Domain.Enums.ApplicationState.Survey;
        }

        /// <summary>
        /// Populate view model for Plan List
        /// </summary>
        /// <returns></returns>
        public ListViewModel GetListViewModel()
        {
            ListViewModel model = new ListViewModel();

            SqlCommand cmd = new SqlCommand("[Survey].[GetList]");
            cmd.CommandType = CommandType.StoredProcedure;

            IList<Survey> surveyList = new List<Survey>();
            Survey sm = null;
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                //be sure we got a reader                
                while (rdr.Read())
                {
                    sm = new Survey();

                    sm.Id = rdr["TimePeriodId"].ToString().SmartParseDefault<int>(default(int));
                    sm.Name = rdr["TimePeriod"].ToString();
                    sm.OpeningDate = rdr["OpeningDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);
                    sm.ClosingDate = rdr["ClosingDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);
                    sm.AcceptedDate = rdr["AcceptedDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);

                    surveyList.Add(sm);
                }
            }
            model.SurveyList = surveyList;
            return model;
        }

        private DateTime? TryGetDateTime(IDataReader rdr, int col)
        {
            if (col < 0)
            {
                return null;
            }
            if (rdr.IsDBNull(col))
            {
                return null;
            }
            return rdr.GetDateTime(col);
        }

        public DashboardViewModel GetDashboardViewModel(DashboardViewModel model /* string year, Enums.SurveyDashboardListType type */)
        {
            //DashboardViewModel model = new DashboardViewModel();
            //RtpSummary rtpSummary = GetSummary(financialYear);

            //We call different sprocs based on the Enum
            string sprocName = null;
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@Year", model.Year));

            bool hasItemDate = false;

            //if ((model.ShowAll != null) && (model.ShowAll.Equals("true"))) sqlParams.Add(new SqlParameter("@ShowAll", 1));
            //int versionStatusId = rtpSummary.IsAmendable() ? (int)Enums.RTPVersionStatus.Pending : (int)Enums.RTPVersionStatus.Active;
            switch (model.ListType)
            {
                case DRCOG.Domain.Enums.SurveyDashboardListType.MyProjects:
                    sprocName = "[Survey].[GetDashboardListByMyProjects]";
                    sqlParams.Add(new SqlParameter("@PersonGuid", model.Person.profile.PersonGUID));
                    break;
                case DRCOG.Domain.Enums.SurveyDashboardListType.ProjectType:
                    sprocName = "[Survey].[GetDashboardListByProjectType]";
                    break;
                case DRCOG.Domain.Enums.SurveyDashboardListType.Sponsor:
                    sprocName = "[Survey].[GetDashboardListBySponsor2]";
                    if (model.ShowAll)
                    {
                        sqlParams.Add(new SqlParameter("@ShowAll", 1));
                    }
                    hasItemDate = true;
                    break;
                case DRCOG.Domain.Enums.SurveyDashboardListType.UpdateStatus:
                    sprocName = "[Survey].[GetDashboardListByUpdateStatus]";
                    break;
                case DRCOG.Domain.Enums.SurveyDashboardListType.ImprovementType:
                    sprocName = "[Survey].[GetDashboardListByImprovementType]";
                    break;
                //case Enums.SurveyDashboardListType.SponsorWithTipid:
                //    sprocName = "[Survey].[GetDashboardListWithTipId]";
                //    break;
                default:
                    throw new NotImplementedException("Not a valid SurveyDashboardListType");
            }

            SqlCommand cmd = new SqlCommand(sprocName);
            cmd.CommandType = CommandType.StoredProcedure;
            foreach(SqlParameter p in sqlParams)
            {
                cmd.Parameters.Add(p);
            }

            //cmd.Parameters.AddWithValue("@Year", model.Year);
            
            //cmd.Parameters.AddWithValue("@VersionStatusId", versionStatusId); // removed not sure if we need this.
            //if (!rtpSummary.IsPending) cmd.Parameters.AddWithValue("@IsActive", 1);

            model.Current = this.GetSurvey(model.Year);
            if (model.Current == null)
            {
                Logger.Debug("No Survey found for " + model.Year);
                model.Current = new Survey();
            }

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                int ccol = rdr.GetOrdinal("Count");
                int ocol = rdr.GetOrdinal("ItemName");
                int dcol = hasItemDate ? rdr.GetOrdinal("ItemDate") : -1;
                while (rdr.Read())
                {
                    model.DashboardItems.Add(new DashboardListItem()
                    {
                        count = rdr.GetInt32(ccol),
                        ItemName = rdr.GetString(ocol),
                        ItemDate = TryGetDateTime(rdr, dcol)
                    });
                }
            }

            //model.RtpStatus.IsCurrent = financialYear == GetCurrentRtpPlanYear();
            //model.RtpSummary = rtpSummary;

            return model;
        }

        public CreateProjectViewModel GetCreateProjectViewModel(Survey model)
        {
            List<SqlParameter> improvementTypeParams = new List<SqlParameter>();
            improvementTypeParams.Add(new SqlParameter("@TimePeriodId", model.Id));

            return new CreateProjectViewModel()
            {
                Survey = model
                ,
                AvailableImprovementTypes = AvailableImprovementTypes(improvementTypeParams)
            };
        }

        public Survey GetSurvey(string year)
        {
            SqlCommand cmd = new SqlCommand("[Survey].[GetSummary]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Year", year);
            Survey model = new Survey();
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    model.Id = rdr["Id"].ToString().SmartParseDefault<int>(default(int));
                    model.Name = rdr["Name"].ToString();
                    model.OpeningDate = rdr["OpeningDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);
                    model.ClosingDate = rdr["ClosingDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);
                    model.AcceptedDate = rdr["AcceptedDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);
                }
                else
                {
                    model = null;
                }
            }
            return model;
        }

        public Survey GetCurrentSurvey()
        {
            Survey model = new Survey();

            SqlCommand cmd = new SqlCommand("[Survey].[GetCurrentSurvey]");
            cmd.CommandType = CommandType.StoredProcedure;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    
                    model.Id = rdr["TimePeriodId"].ToString().SmartParseDefault<int>(default(int));
                    model.Name = rdr["TimePeriod"].ToString();
                    model.OpeningDate = rdr["OpeningDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);
                    model.ClosingDate = rdr["ClosingDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);
                    //model.AcceptedDate = rdr["AcceptedDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue);
                }
                else
                {
                    model = null;
                }
            }
            return model;
        }

        public string GetCurrentSurveyYear()
        {
            string planYear = String.Empty;

            SqlCommand cmd = new SqlCommand("[Survey].[GetCurrentSurvey]");
            cmd.CommandType = CommandType.StoredProcedure;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    planYear = rdr["TimePeriod"].ToString();
                }
            }
            return planYear;
        }

        public List<Project> GetProjects(SearchModel projectSearchModel)
        {
            List<Project> list = new List<Project>();

            SqlCommand cmd = new SqlCommand("[Survey].[GetProjects]");
            cmd.CommandType = CommandType.StoredProcedure;

            //I will speed up these queries by restricting the list on three items: TipYear, TipYearID or IsActive. -DBD
            if (!projectSearchModel.Exclude_Year) // If we are excluding a TipYear, then we must return everything (no SQL optimization)
            {
                if (!projectSearchModel.YearId.Equals(default(int))) cmd.Parameters.AddWithValue("@YEARID", projectSearchModel.YearId);
            }
            if (projectSearchModel.VersionStatusId > 0)
            {
                //if ((bool)projectSearchModel.ActiveVersion) cmd.Parameters.AddWithValue("@ISACTIVE", 1);
                cmd.Parameters.AddWithValue("@VersionStatusId", projectSearchModel.VersionStatusId);
                //else cmd.Parameters.AddWithValue("@ISACTIVE", 0);
            }
            if (!projectSearchModel.SponsorContactId.Equals(default(int)))
            {
                cmd.Parameters.AddWithValue("@SponsorContactId", projectSearchModel.SponsorContactId);
            }
            if (!projectSearchModel.SponsorAgencyID.Equals(default(int)) || projectSearchModel.SponsorAgencyID.HasValue)
            {
                cmd.Parameters.AddWithValue("@SponsorAgencyId", projectSearchModel.SponsorAgencyID);
            }

            if (projectSearchModel.ShowMySponsorAgencies)
            {
                cmd.Parameters.AddWithValue("@PersonId", projectSearchModel.Profile.PersonID);
            }
            
            //if (!projectSearchModel.SponsorContactId.Equals(default(int)))
            //{
                cmd.Parameters.AddWithValue("@ShowAllForAgency", projectSearchModel.ShowAllForAgency);
            //}

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    Project project = new Project()
                    {
                        ProjectVersionId = rdr["ProjectVersionId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        SponsorName = rdr["Sponsor"].ToString()
                        ,
                        ProjectId = rdr["ProjectId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        TimePeriodId = rdr["TimePeriodId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        TimePeriod = rdr["TimePeriod"].ToString()
                        ,
                        ProjectName = rdr["ProjectName"].ToString()
                        ,
                        COGID = rdr["COGID"].ToString()
                        ,
                        ImprovementType = rdr["ImprovementType"].ToString()
                        ,
                        UpdateStatusId = rdr["UpdateStatusId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        ReportOnlyOpenDate = rdr["OpenYear"].ToString()
                        ,
                        PreviousVersionId = rdr["PreviousProjectVersionID"].ToString().SmartParseDefault<int>(default(int))
                    };
                    list.Add(project);

                }
            }

            // These are processed via the SQL retrieval code optimization, so only process this one when exclude is checked.
            if ((projectSearchModel.Year != null) && (projectSearchModel.Exclude_Year))
            {
                list = (from fli in list
                        where (projectSearchModel.Year.ToWildcardRegex().IsMatch(fli.TimePeriod))
                        select fli).ToList<Project>();
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
                        select fli).ToList<Project>();
            }

            if (projectSearchModel.SponsorAgency != null)
            {
                list = (from fli in list
                        where ((fli.SponsorName == projectSearchModel.SponsorAgency) && (!projectSearchModel.Exclude_SponsorAgency))
                        || ((fli.SponsorName != projectSearchModel.SponsorAgency) && (projectSearchModel.Exclude_SponsorAgency))
                        select fli).ToList<Project>();
            }

            //if (projectSearchModel.AmendmentStatus != null)
            //{
            //    list = (from fli in list
            //            where ((fli.AmendmentStatus == projectSearchModel.AmendmentStatus) && (!projectSearchModel.Exclude_AmendmentStatus))
            //            || ((fli.AmendmentStatus != projectSearchModel.AmendmentStatus) && (projectSearchModel.Exclude_AmendmentStatus))
            //            select fli).ToList<RtpSummary>();
            //}

            //if (projectSearchModel.ProjectType != null)
            //{
            //    list = (from fli in list
            //            where ((fli.ProjectType == projectSearchModel.ProjectType) && (!projectSearchModel.Exclude_ProjectType))
            //            || ((fli.ProjectType != projectSearchModel.ProjectType) && (projectSearchModel.Exclude_ProjectType))
            //            select fli).ToList<RtpSummary>();
            //}

            if ((projectSearchModel.ProjectName != null) && (!projectSearchModel.ProjectName.Equals("")))
            {
                list = (from fli in list
                        where ((projectSearchModel.ProjectName.ToWildcardRegex().IsMatch(fli.ProjectName)) && (!projectSearchModel.Exclude_ProjectName))
                        || ((!projectSearchModel.ProjectName.ToWildcardRegex().IsMatch(fli.ProjectName)) && (projectSearchModel.Exclude_ProjectName))
                        select fli).ToList<Project>();
            }

            return list;
        }

        public void SetSurveyStatus(Instance version)
        {
            IList<SqlParameter> parms = new List<SqlParameter>();
            string sproc = string.Empty;

            version.UpdateStatusId = (version.UpdateStatusId == (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Revised)
                ? (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited 
                : version.UpdateStatusId;

            switch (version.UpdateStatusId)
            {
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Completed:
                    sproc = "[Survey].[SetOpenToPublic]";
                    parms.Add(new SqlParameter("@ProjectVersionId", version.ProjectVersionId));
                    parms.Add(new SqlParameter("@EndConstructionYear", version.EndConstructionYear));
                    parms.Add(new SqlParameter("@UpdateStatusId", version.UpdateStatusId));
                    break;
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited:
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Withdrawn:
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Reviewed:
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Accepted:
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Current:
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.AwaitingAction:
                    sproc = "[Survey].[UpdateProjectUpdateStatusId]";
                    parms.Add(new SqlParameter("@ProjectVersionId", version.ProjectVersionId));
                    parms.Add(new SqlParameter("@UpdateStatusId", version.UpdateStatusId));
                    break;
                }

            using (SqlCommand command = new SqlCommand(sproc) { CommandType = CommandType.StoredProcedure })
            {
                foreach (SqlParameter p in parms)
                {
                    command.Parameters.Add(p);
                }
                this.ExecuteNonQuery(command);
            }
        }

        public int CreateSurvey(int planId, string surveyName)
        {
            int timePeriodId = default(int);

            IList<SqlParameter> parms = new List<SqlParameter>();

            parms.Add(new SqlParameter("@TimePeriod", surveyName));
            parms.Add(new SqlParameter("@PlanTimePeriodId", planId));

            using (SqlCommand command = new SqlCommand("[Survey].[Create]") { CommandType = CommandType.StoredProcedure })
            {
                foreach (SqlParameter p in parms)
                {
                    command.Parameters.Add(p);
                }
                SqlParameter outParam = new SqlParameter("@TimePeriodId", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(outParam);

                this.ExecuteNonQuery(command);
                timePeriodId = (int)command.Parameters["@TimePeriodId"].Value;
            }
            return timePeriodId;
        }

        public Instance CopyProject(int projectVersionId)
        {
            return CopyProject(projectVersionId, default(int));
        }

        public Instance CopyProject(int projectVersionId, int surveyId)
        {
            int newProjectVersionId = default(int);

            using (SqlCommand command = new SqlCommand("[Survey].[CopyProject]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                if(!surveyId.Equals(default(int))) 
                {
                    command.Parameters.AddWithValue("@SurveyId", surveyId);
                }

                //result.this.GetProjectBasics(projectVersionId);
                SqlParameter outParam = new SqlParameter("@NewProjectVersionId", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(outParam);

                this.ExecuteNonQuery(command);
                newProjectVersionId = (int)command.Parameters["@NewProjectVersionId"].Value;
            }

            return this.GetProjectBasics(newProjectVersionId);
        }

        /// <summary>
        /// Get projects available for inclusion in a Survey.
        /// </summary>
        /// <param name="timePeriodId"></param>
        /// <returns></returns>
        public IList<Project> GetAmendableProjects()
        {
            IList<Project> list = new List<Project>();

            try
            {
                using (SqlCommand command = new SqlCommand("[Survey].[ProjectsAvailableForNewSurvey]") { CommandType = CommandType.StoredProcedure })
                {
                    //command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

                    using (IDataReader rdr = this.ExecuteReader(command))
                    {
                        while (rdr.Read())
                        {
                            Project project = new Project()
                            {
                                SponsorName = rdr["Sponsor"].ToString()
                                ,
                                ProjectName = rdr["ProjectName"].ToString()
                                ,
                                TimePeriod = rdr["TimePeriod"].ToString()
                                ,
                                COGID = rdr["COGID"].ToString()
                                ,
                                ImprovementType = rdr["ImprovementType"].ToString()
                                ,
                                ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? rdr["ProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
                            };
                            list.Add(project);
                        }
                    }
                }
            }
            catch (Exception exc)
            {

            }

            return list;
        }

        #region Lookups

        protected IDictionary<int, string> AvailableAdminLevels
        {
            get
            {
                return GetLookupCollection("Lookup_GetProjectAdministrativeLevels", "Id", "Label");
            }
        }
        protected IDictionary<int, string> AvailableSponsors
        {
            get
            {
                return GetLookupCollection("Lookup_GetSponsorOrganizations", "Id", "Label");
            }
        }
        protected IDictionary<int, string> AvailableImprovementTypes
        {
            get
            {
                return GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
            }
        }
        protected IDictionary<int, string> AvailableRoadOrTransitTypes
        {
            get
            {
                return GetLookupCollection("Lookup_GetRoadOrTransitCategories", "Id", "Label");
            }
        }
        protected IDictionary<int, string> AvailableSponsorContacts(int sponsorId)
        {
            if (!sponsorId.Equals(default(int)))
            {
                return GetSponsorContacts(sponsorId);
            }
            return null;
        }
        protected IDictionary<int, string> AvailableProjectTypes
        {
            get
            {
                return GetLookupCollection("Lookup_GetProjectTypes", "Id", "Label");
            }
        }
        protected IDictionary<int, string> AvailableSelectionAgencies
        {
            get
            {
                return GetLookupCollection("Lookup_GetSelectors", "Id", "Label");
            }
        }
        //protected IDictionary<int, string> AvailablePools(TipSummary summary)
        //{
        //    return GetPoolNames(1, summary.TipYearTimePeriodID);
        //}
        protected IDictionary<int, string> FundingTypes
        {
            get
            {
                return GetLookupCollection("[dbo].[GetFundingTypes]", "FundingTypeID", "FundingType");
            }
        }
        protected IDictionary<int, string> FundingLevels
        {
            get
            {
                return GetLookupCollection("[dbo].[GetFundingLevels]", "FundingLevelID", "FundingLevel");
            }
        }
        //public IDictionary<int, string> GetSponsorContacts(int sponsorOrganizationID)
        //{
        //    return GetLookupCollection("Lookup_GetSponsorContacts", "Id", "Name", new List<SqlParameter>
        //      {
        //          new SqlParameter { ParameterName = "@OrganizationID", SqlDbType = SqlDbType.Int, Value = sponsorOrganizationID }
        //      });
        //}
        //public String GetSponsorContact(int sponsorOrganizationID, int sponsorContactId)
        //{
        //    IDictionary<int, string> sponsorContacts = GetSponsorContacts(sponsorOrganizationID);
        //    if (sponsorContacts.ContainsKey(sponsorContactId))
        //    {
        //        string contact = (from c in sponsorContacts
        //                          where c.Key.Equals(sponsorContactId)
        //                          select c).First<KeyValuePair<int, string>>().Value.ToString();
        //        return contact;
        //    } return null;
        //}
        public DRCOG.Domain.Models.ProjectSponsorsModel GetProjectSponsorsModel(int projectVersionID, string year)
        {
            var model = new DRCOG.Domain.Models.ProjectSponsorsModel();

            SqlCommand cmd;

            if (!projectVersionID.Equals(default(int)))
            {
                // Get Agencies which are eligible to sponsor projects
                cmd = new SqlCommand("[dbo].[GetCurrentProjectSponsorAgency]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);

                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    while (rdr.Read())
                    {
                        model.CurrentAgencies.Add(new SponsorOrganization()
                        {
                            OrganizationId = (int)rdr["OrganizationId"],
                            OrganizationName = rdr["OrganizationName"].ToString(),
                            IsPrimary = bool.Parse(rdr["Primary"].ToString())
                        });
                    }
                }

                model.PrimarySponsor = model.CurrentAgencies.First(item => item.IsPrimary == true);
            }

            cmd = new SqlCommand("[Survey].[GetAvailableProjectSponsorAgencies]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Year", year);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    model.AvailableAgencies.Add(new SponsorOrganization()
                    {
                        OrganizationId = (int)rdr["OrganizationId"],
                        OrganizationName = rdr["OrganizationName"].ToString(),
                    });
                }
            }

            return model;
        }

        public IList<Contact> GetContact(ContactSearch criteria)
        {
            List<Contact> model = new List<Contact>();

            SqlCommand cmd = new SqlCommand("[Survey].[GetContacts]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@YearID", criteria.TimePeriodId);
            if (!criteria.PersonId.Equals(default(int)))
            {
                cmd.Parameters.AddWithValue("@SponsorContactId", criteria.PersonId);
            }
            if (!criteria.SponsorAgencyId.Equals(default(int)))
            {
                cmd.Parameters.AddWithValue("@SponsorAgencyId", criteria.SponsorAgencyId);
            }
            cmd.Parameters.AddWithValue("@ShowAllForAgency", criteria.ShowAllForAgency);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    model.Add(new Contact()
                    {
                        //Mobile	Email	WebSite
                        PersonId = (int)rdr["PersonID"]
                        ,
                        PersonGuid = rdr["PersonGUID"].ToString().SmartParseDefault<Guid>(default(Guid))
                        ,
                        FirstName = rdr["Firstname"].ToString()
                        ,
                        MiddleInitial = rdr["MiddleInitial"].ToString()
                        ,
                        LastName = rdr["Lastname"].ToString()
                        ,
                        Division = rdr["Division"].ToString()
                        ,
                        Title = rdr["Title"].ToString()
                        ,
                        CreationDate = rdr["CreationDate"].ToString().SmartParseDefault<DateTime>(default(DateTime))
                        ,
                        Comments = rdr["Comments"].ToString()
                        ,
                        Address = new Address()
                        {
                            Street = rdr["Address"].ToString()
                            ,
                            City = rdr["City"].ToString()
                            ,
                            State = rdr["State"].ToString()
                            ,
                            Zip = rdr["Zip"].ToString()
                        }
                        ,
                        Phone = rdr["Voice"].ToString()
                        ,
                        Fax = rdr["Fax"].ToString()
                        ,
                        Email = rdr["Email"].ToString()
                    });
                }
            }

            return model;
        }

        #endregion

        #region Status

        public void SetSurveyDates(Survey model)
        {

            using (SqlCommand command = new SqlCommand("[Survey].[SetSurveyDates]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@TimePeriodId", model.Id);
                if (!model.OpeningDate.Equals(DateTime.MinValue))
                {
                    command.Parameters.AddWithValue("@OpeningDate", model.OpeningDate);
                }
                if (!model.ClosingDate.Equals(DateTime.MinValue))
                {
                    command.Parameters.AddWithValue("@ClosingDate", model.ClosingDate);
                }
                
                this.ExecuteNonQuery(command);
            }
        }

        public Survey GetSurveyDates(Survey model)
        {

            using (SqlCommand command = new SqlCommand("[Survey].[GetSurveyDates]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@TimePeriodId", model.Id);
                
                using (IDataReader rdr = this.ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        model.OpeningDate = rdr["OpeningDate"].ToString().SmartParseDefault<DateTime>(default(DateTime));
                        model.ClosingDate = rdr["ClosingDate"].ToString().SmartParseDefault<DateTime>(default(DateTime));
                    }
                }
            }
            return model;
        }

        public void CloseSurveyNow(Survey model)
        {
            using (SqlCommand command = new SqlCommand("[Survey].[CloseSurveyNow]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@TimePeriodId", model.Id);
                this.ExecuteNonQuery(command);
            }
        }

        public void OpenSurveyNow(Survey model)
        {
            using (SqlCommand command = new SqlCommand("[Survey].[OpenSurveyNow]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@TimePeriodId", model.Id);
                this.ExecuteNonQuery(command);
            }
        }

        

        #endregion

        #region Project Info

        /// <summary>
        /// Get the select lists and the data to edit the "General Info" about a project
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public InfoViewModel GetProjectInfoViewModel(int versionId, string year)
        {
            var result = new InfoViewModel();

            // get project summary info
            result.Project = GetProjectInfo(versionId, year);
            result.Current = GetSurvey(year);
            //result.ProjectSummary = GetProjectSummary(versionId, rtpYear);
            result.ProjectSponsorsModel = GetProjectSponsorsModel(versionId, year);
            if (!result.ProjectSponsorsModel.PrimarySponsor.OrganizationId.Equals(default(int)) && !result.Project.SponsorContactId.Equals(default(int)))
            {
                result.ProjectSponsorsModel.SponsorContact = GetSponsorContact(result.Project.SponsorId, result.Project.SponsorContactId);
            }
            
            // fill collections
            result.AvailableAdminLevels = AvailableAdminLevels;// GetLookupCollection("Lookup_GetProjectAdministrativeLevels", "Id", "Label");

            List<SqlParameter> improvementTypeParams = new List<SqlParameter>();
            improvementTypeParams.Add(new SqlParameter("@TimePeriodId", result.Project.TimePeriodId));
            result.AvailableImprovementTypes = AvailableImprovementTypes(improvementTypeParams);// GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
            result.AvailableRoadOrTransitTypes = AvailableRoadOrTransitTypes; // GetLookupCollection("Lookup_GetRoadOrTransitCategories", "Id", "Label");
            result.AvailableSponsorContacts = AvailableSponsorContacts((int)result.ProjectSponsorsModel.PrimarySponsor.OrganizationId);// GetSponsorContacts(result.InfoModel.SponsorId.Value);
            result.AvailableProjectTypes = AvailableProjectTypes;// GetLookupCollection("Lookup_GetProjectTypes", "Id", "Label");
            result.AvailableSelectionAgencies = AvailableSelectionAgencies; // GetLookupCollection("Lookup_GetSelectors", "Id", "Label");
            
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@StatusTypeID", (int)DRCOG.Domain.Enums.StatusType.SurveyUpdateStatus));
            result.AvailableUpdateStatus = GetLookupCollection("Lookup_GetStatuses", "Id", "Label", paramList);
            //result.AvailablePools = AvailablePools(result.ProjectSummary);// GetPoolNames(1, result.ProjectSummary.TipYearTimePeriodID); // Can get ProgramID from Session. -DBD

            result.AvailableFundingResources = AvailableFundingResources(DRCOG.Domain.Enums.ApplicationState.Survey, result.Project.TimePeriodId);

            result.Project.Funding = GetFunding(versionId, year);
            result.Project.FundingSources = GetProjectFundingSources(versionId);

            return result;
        }

        /// <summary>
        /// Get a RtpProjectVersionInfoModel. This is the model behind the
        /// Info Tab
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public Project GetProjectInfo(int versionId, string year)
        {
            Project result = null;
            using (SqlCommand command = new SqlCommand("[Survey].[GetProjectVersionInfo]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", versionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new Project()
                        {
                            ProjectVersionId = versionId
                            , 
                            TimePeriodId = rdr["TimePeriodID"].ToString().SmartParseDefault(default(int))
                            ,
                            ProjectId = rdr["ProjectID"].ToString().SmartParseDefault(default(int))
                            ,
                            COGID = rdr["COGID"].ToString()
                            ,
                            SponsorContactId = rdr["SponsorContactId"].ToString().SmartParseDefault(default(int))
                            ,
                            SponsorContact = rdr["SponsorContact"].ToString()
                            ,
                            SponsorId = rdr["OrganizationID"].ToString().SmartParseDefault(default(int))
                            ,
                            SponsorName = rdr["SponsorOrganization"].ToString()
                            ,
                            ProjectName = rdr["ProjectName"].ToString()
                            ,
                            Scope = rdr["Scope"].ToString()
                            ,
                            FacilityName = rdr["FacilityName"].ToString()
                            ,
                            Limits = rdr["Limits"].ToString()
                            ,
                            BeginAt = rdr["BeginAt"].ToString()
                            ,
                            EndAt = rdr["EndAt"].ToString()
                            ,
                            SponsorNotes = rdr["SponsorNotes"].ToString()
                            ,
                            DRCOGNotes = rdr["DRCOGNotes"].ToString()
                            ,
                            BeginContructionYear = rdr["BeginConstructionYear"].ToString().SmartParseDefault(default(int))
                            ,
                            LastModifiedDate = rdr["LastModifiedDate"].ToString().SmartParseDefault(default(DateTime))
                            ,
                            AmendmentFundingTypeId = rdr["AmendmentFundingTypeID"].ToString().SmartParseDefault(default(int))
                            ,
                            AmendmentFundingType = rdr["AmendmentFundingType"].ToString()
                            ,
                            TransportationTypeId = rdr["TransportationTypeID"].ToString().SmartParseDefault(default(int))
                            ,
                            TransportationType = rdr["TransportationType"].ToString()
                            ,
                            AdministrativeLevelId = rdr["AdministrativeLevelID"].ToString().SmartParseDefault(default(int))
                            ,
                            AdministrativeLevel = rdr["AdministrativeLevel"].ToString()
                            ,
                            SelectorId = rdr["SelectorID"].ToString().SmartParseDefault(default(int))
                            ,
                            Selector = rdr["Selector"].ToString()
                            ,
                            ImprovementTypeId = rdr["ImprovementTypeId"].ToString().SmartParseDefault(default(int))
                            ,
                            ImprovementType = rdr["ImprovementType"].ToString()
                            ,
                            UpdateStatusId = rdr["UpdateStatusId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            EndConstructionYear = rdr["EndConstructionYear"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            IsNew = rdr["IsNew"].ToString().SmartParseDefault<bool>(default(bool))
                        };
                    }
                }
            }
            result.EligibleContributors = this.GetProjectContributors(versionId);
            return result;
        }

        private IList<int> GetProjectContributors(int projectVersionId)
        {
            List<int> result = new List<int>();
            using (SqlCommand command = new SqlCommand("[Survey].[GetProjectContributors]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        result.Add((int)rdr["PersonID"]);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Get a RtpProjectVersionInfoModel. This is the model behind the
        /// Info Tab
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public Project GetProjectBasics(int versionId)
        {
            Project result = null;
            using (SqlCommand command = new SqlCommand("[Survey].[GetProjectVersionBasics]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", versionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new Project()
                        {
                            ProjectVersionId = versionId
                            ,
                            TimePeriodId = rdr["TimePeriodID"].ToString().SmartParseDefault(default(int))
                            ,
                            ProjectId = rdr["ProjectID"].ToString().SmartParseDefault(default(int))
                            ,
                            COGID = rdr["COGID"].ToString()
                            ,
                            ProjectName = rdr["ProjectName"].ToString()
                            ,
                            LastModifiedDate = rdr["LastModifiedDate"].ToString().SmartParseDefault(default(DateTime))
                            ,
                            SponsorContactId = rdr["SponsorContactId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            UpdateStatusId = rdr["UpdateStatusId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            EndConstructionYear = rdr["EndConstructionYear"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            ImprovementTypeId = rdr["ImprovementTypeID"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            IsNew = rdr["IsNew"].ToString().SmartParseDefault<bool>(default(bool))
                        };
                    }
                }
            }
            result.EligibleContributors = this.GetProjectContributors(versionId); // Added to handle editing permission in Scope tab. -DBD 05/09/2012
            return result;
        }

        public Project GetProjectBasicsBySegment(int segmentId)
        {
            Project result = null;
            using (SqlCommand command = new SqlCommand("[Survey].[GetProjectVersionBasicsBySegment]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@SegmentId", segmentId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new Project()
                        {
                            ProjectVersionId = rdr["ProjectVersionId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            TimePeriodId = rdr["TimePeriodID"].ToString().SmartParseDefault(default(int))
                            ,
                            ProjectId = rdr["ProjectID"].ToString().SmartParseDefault(default(int))
                            ,
                            COGID = rdr["COGID"].ToString()
                            ,
                            ProjectName = rdr["ProjectName"].ToString()
                            ,
                            LastModifiedDate = rdr["LastModifiedDate"].ToString().SmartParseDefault(default(DateTime))
                            ,
                            SponsorContactId = rdr["SponsorContactId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            UpdateStatusId = rdr["UpdateStatusId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            EndConstructionYear = rdr["EndConstructionYear"].ToString().SmartParseDefault<int>(default(int))
                        };
                    }
                }
            }
            return result;
        }

        public void CheckUpdateStatusId(Project project)
        {
            switch (project.UpdateStatusId)
            {
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Carryover:
                case (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Reclassified:
                    project.UpdateStatusId = (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited;
                    UpdateProjectUpdateStatusId(project);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Update the UpdateStatusId for project
        /// </summary>
        /// <param name="model"></param>
        public void UpdateProjectUpdateStatusId(Project model)
        {
            using (SqlCommand command = new SqlCommand("[Survey].[UpdateProjectUpdateStatusId]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", model.ProjectVersionId);
                command.Parameters.AddWithValue("@UpdateStatusId", model.UpdateStatusId);
                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Update the Project Info in the database
        /// </summary>
        /// <param name="model"></param>
        public void UpdateProjectInfo(Project model)
        {
            CheckUpdateStatusId(model);

            using (SqlCommand command = new SqlCommand("[Survey].[UpdateProjectVersionInfo]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTVERSIONID", model.ProjectVersionId);
                command.Parameters.AddWithValue("@TIMEPERIOD", model.TimePeriodId);
                command.Parameters.AddWithValue("@PROJECTID", model.ProjectId);
                command.Parameters.AddWithValue("@PROJECTNAME", model.ProjectName);
                command.Parameters.AddWithValue("@SPONSORID", !model.SponsorId.Equals(default(int)) ? (object)model.SponsorId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SPONSORCONTACTID", !model.SponsorContactId.Equals(default(int)) ? (object)model.SponsorContactId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ADMINLEVELID", !model.AdministrativeLevelId.Equals(default(int)) ? (object)model.AdministrativeLevelId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@IMPTYPEID", !model.ImprovementTypeId.Equals(default(int)) ? (object)model.ImprovementTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TRANSTYPEID", !model.TransportationTypeId.Equals(default(int)) ? (object)model.TransportationTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SelectionAgencyID", !model.SelectorId.Equals(default(int)) ? (object)model.SelectorId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SPONSORNOTES", model.SponsorNotes);
                command.Parameters.AddWithValue("@DRCOGNOTES", model.DRCOGNotes);
                command.Parameters.AddWithValue("@TotalCost", model.Funding.TotalCost);
                command.Parameters.AddWithValue("@UpdateStatusId", model.UpdateStatusId);
                this.ExecuteNonQuery(command);
            }
        }

        #endregion

        #region Scope

        public ScopeViewModel GetScopeViewModel(int projectVersionId, string year)
        {
            var result = new ScopeViewModel();

            // get project summary info
            result.Scope = this.GetScopeModel(projectVersionId, year);
            result.Project = this.GetProjectBasics(projectVersionId); 
            //result.Project = new Project() { ProjectVersionId = projectVersionId };
            result.Current = GetSurvey(year);// GetProjectSummary(projectVersionId, planYear);
            result.Segments = GetProjectSegments(projectVersionId);

            var cycle = GetCurrentCycle(GetYearId(GetCurrentRtpPlanYear(), DRCOG.Domain.Enums.TimePeriodType.PlanYear));

            IList<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CycleId", cycle.Id));

            result.AvailableNetworks = GetLookupCollection("[dbo].[Lookup_GetNetworks]", "Id", "Label", parms);
            
            result.AvailableImprovementTypes = AvailableImprovementTypes(24);
            parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@TypeId", (int)DRCOG.Domain.Enums.GISCategoryType.FacilityType));
            result.AvailableFacilityTypes = GetLookupCollection("[dbo].[Lookup_GISCategoriesByType]", "Id", "Label", parms);

            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@StatusTypeID", (int)DRCOG.Domain.Enums.StatusType.SurveyUpdateStatus));
            result.AvailableUpdateStatus = GetLookupCollection("Lookup_GetStatuses", "Id", "Label", paramList);

            return result;
        }

        public IList<SegmentModel> GetProjectSegments(int projectVersionID)
        {
            IList<SegmentModel> segmentList = new List<SegmentModel>();

            SqlCommand cmd = new SqlCommand("[dbo].[GetProjectVersionSegments]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    string network = rdr["Network"] != DBNull.Value ? rdr["Network"].ToString() : "";
                    string improvementType = rdr["ImprovementType"] != DBNull.Value ? rdr["ImprovementType"].ToString() : "";
                    string planFacilityType = rdr["PlanFacilityType"] != DBNull.Value ? rdr["PlanFacilityType"].ToString() : "";


                    segmentList.Add(new SegmentModel(planFacilityType, improvementType, network)
                    {
                        SegmentId = rdr["SegmentID"] != DBNull.Value ? (int)rdr["SegmentID"] : 0
                    ,
                        ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? (int)rdr["ProjectVersionId"] : 0
                    ,
                        ImprovementTypeId = rdr["ImprovementTypeId"] != DBNull.Value ? (int)rdr["ImprovementTypeId"] : 0
                    ,
                        ModelingFacilityTypeId = rdr["ModelingFacilityTypeID"] != DBNull.Value ? (int)rdr["ModelingFacilityTypeId"] : 0
                    ,
                        PlanFacilityTypeId = rdr["PlanFacilityTypeId"] != DBNull.Value ? (int)rdr["PlanFacilityTypeId"] : 0
                    ,
                        NetworkId = rdr["NetworkId"] != DBNull.Value ? (int)rdr["NetworkId"] : 0
                    ,
                        OpenYear = rdr["OpenYear"] != DBNull.Value ? (short)rdr["OpenYear"] : (short)0
                    ,
                        FacilityName = rdr["FacilityName"] != DBNull.Value ? rdr["FacilityName"].ToString() : ""
                    ,
                        StartAt = rdr["StartAt"] != DBNull.Value ? rdr["StartAt"].ToString() : ""
                    ,
                        EndAt = rdr["EndAt"] != DBNull.Value ? rdr["EndAt"].ToString() : ""
                    ,
                        Length = rdr["Length"] != DBNull.Value ? (decimal)rdr["Length"] : default(decimal)
                    ,
                        LanesBase = rdr["LanesBase"] != DBNull.Value ? (short)rdr["LanesBase"] : (short)0
                    ,
                        LanesFuture = rdr["LanesFuture"] != DBNull.Value ? (short)rdr["LanesFuture"] : (short)0
                    ,
                        SpacesFuture = rdr["SpacesFuture"] != DBNull.Value ? (short)rdr["SpacesFuture"] : (short)0
                    ,
                        VehiclesFuture = rdr["VehiclesFuture"] != DBNull.Value ? (short)rdr["VehiclesFuture"] : (short)0
                    });
                }
            }

            return segmentList;
        }

        public SegmentModel GetSegmentDetails(int segmentId)
        {
            DataTable data;
            using (SqlCommand command = new SqlCommand("[dbo].[GetSegment]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@SegmentId", segmentId);
                data = this.ExecuteDataTable(command);
            }
            SegmentModel model;
            DataRow row = data.Rows[0];
            model = new SegmentModel()
            {
                SegmentId = segmentId
                ,
                FacilityName = row["FacilityName"].ToString()
                ,
                StartAt = row["StartAt"].ToString()
                ,
                EndAt = row["EndAt"].ToString()
                ,
                NetworkId = row["NetworkId"].ToString().SmartParse<int>()
                ,
                ImprovementTypeId = row["ImprovementTypeId"].ToString().SmartParse<int>()
                ,
                PlanFacilityTypeId = row["PlanFacilityTypeId"].ToString().SmartParse<int>()
                ,
                OpenYear = row["OpenYear"].ToString().SmartParse<short>()
                ,
                LanesBase = row["LanesBase"].ToString().SmartParse<short>()
                ,
                LanesFuture = row["LanesFuture"].ToString().SmartParse<short>()
                ,
                SpacesBase = row["SpacesBase"].ToString().SmartParse<short>()
                ,
                SpacesFuture = row["SpacesFuture"].ToString().SmartParse<short>()
                ,
                AssignmentStatusID = row["AssignmentStatusID"].ToString().SmartParse<int>()
                ,
                Length = row["Length"].ToString().SmartParseDefault(default(decimal))
                ,
                ModelingCheck = row["ModelingCheck"].ToString().SmartParseDefault(default(bool))
                ,
                LRSObjectID = row["LRSObjectID"].ToString().SmartParse<int>()
            };
            return model;
        }

        public Int32 AddSegment(SegmentModel model)
        {
            int retval = default(int);
            using (SqlCommand command = new SqlCommand("[dbo].[AddSegment]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", model.ProjectVersionId);
                command.Parameters.AddWithValue("@FacilityName", model.FacilityName != null ? (object)model.FacilityName.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartAt", model.StartAt != null ? (object)model.StartAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndAt", model.EndAt != null ? (object)model.EndAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ImprovementTypeId", model.ImprovementTypeId > 0 ? model.ImprovementTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@PlanFacilityTypeId", model.PlanFacilityTypeId > 0 ? model.PlanFacilityTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@OpenYear", model.OpenYear > 0 ? model.OpenYear : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LanesBase", model.LanesBase > 0 ? model.LanesBase : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LanesFuture", model.LanesFuture > 0 ? model.LanesFuture : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SpacesFuture", model.SpacesFuture > 0 ? model.SpacesFuture : (object)DBNull.Value);
                command.Parameters.AddWithValue("@AssignmentStatusId", model.AssignmentStatusID > 0 ? model.AssignmentStatusID : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Length", model.Length > 0 ? model.Length : (object)DBNull.Value);

                SqlParameter outParam = new SqlParameter("@SegmentId", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(outParam);

                this.ExecuteNonQuery(command);
                retval = (int)command.Parameters["@SegmentId"].Value;
            }
            return retval;
        }

        public void DeleteSegment(int segmentId)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[DeleteSegment]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@SegmentId", segmentId);
                this.ExecuteNonQuery(command);
            }
        }

        public void UpdateSegment(SegmentModel model)
        {

            using (SqlCommand command = new SqlCommand("[dbo].[UpdateSegment]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@SegmentId", model.SegmentId);
                command.Parameters.AddWithValue("@FacilityName", model.FacilityName != null ? (object)model.FacilityName.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartAt", model.StartAt != null ? (object)model.StartAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndAt", model.EndAt != null ? (object)model.EndAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@NetworkId", model.NetworkId > 0 ? model.NetworkId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ImprovementTypeId", model.ImprovementTypeId > 0 ? model.ImprovementTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@PlanFacilityTypeId", model.PlanFacilityTypeId > 0 ? model.PlanFacilityTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ModelingFacilityTypeId", model.ModelingFacilityTypeId > 0 ? model.ModelingFacilityTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@OpenYear", model.OpenYear > 0 ? model.OpenYear : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LanesBase", model.LanesBase > 0 ? model.LanesBase : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LanesFuture", model.LanesFuture > 0 ? model.LanesFuture : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SpacesBase", model.SpacesBase > 0 ? model.SpacesBase : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SpacesFuture", model.SpacesFuture > 0 ? model.SpacesFuture : (object)DBNull.Value);
                command.Parameters.AddWithValue("@AssignmentStatusId", model.AssignmentStatusID > 0 ? model.AssignmentStatusID : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Length", model.Length > 0 ? model.Length : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ModelingCheck", model.ModelingCheck);


                if (!String.IsNullOrEmpty(model.LRSxml))
                {
                    byte[] encodedString = System.Text.Encoding.UTF8.GetBytes(model.LRSxml);
                    MemoryStream ms = new MemoryStream(encodedString);
                    ms.Flush();
                    ms.Position = 0;

                    XmlDocument xml = new XmlDocument();
                    xml.Load(ms);

                    using (XmlNodeReader xnr = new XmlNodeReader(xml))
                    {
                        command.Parameters.Add("@xml", SqlDbType.Xml).Value = new SqlXml(xnr);
                        //command.Parameters.AddWithValue("@xml", !String.IsNullOrEmpty(model.LRSxml) ? model.LRSxml : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@xmlSchemeId", !String.IsNullOrEmpty(model.LRSxml) ? (int)SchemeName.LRSProjects : (object)DBNull.Value);
                    }
                }

                this.ExecuteNonQuery(command);
            }
        }

        public void UpdateSegmentSummary(SegmentModel model)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[UpdateSegmentSummary]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@SegmentId", model.SegmentId);
                command.Parameters.AddWithValue("@FacilityName", model.FacilityName != null ? (object)model.FacilityName.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartAt", model.StartAt != null ? (object)model.StartAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndAt", model.EndAt != null ? (object)model.EndAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@NetworkId", model.NetworkId > 0 ? model.NetworkId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@OpenYear", model.OpenYear > 0 ? model.OpenYear : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LanesBase", model.LanesBase);
                command.Parameters.AddWithValue("@LanesFuture", model.LanesFuture);

                this.ExecuteNonQuery(command);
            }
        }

        public ScopeModel GetScopeModel(int projectVersionId, string year)
        {
            ScopeModel result = null;

            using (SqlCommand command = new SqlCommand("[Survey].[GetProjectScope]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new ScopeModel();
                        result.BeginConstructionYear = rdr["BeginConstructionYear"] != DBNull.Value ? Convert.ToInt32(rdr["BeginConstructionYear"]) : default(int);
                        result.OpenToPublicYear = rdr["EndConstructionYear"] != DBNull.Value ? Convert.ToInt32(rdr["EndConstructionYear"]) : default(int);
                        result.ProjectDescription = rdr["ProjectDescription"].ToString();
                        result.ProjectId = rdr["ProjectId"] != DBNull.Value ? (int)rdr["ProjectId"] : default(int);
                        result.ProjectName = rdr["ProjectName"].ToString();
                        result.ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? (int)rdr["ProjectVersionId"] : default(int);
                        
                    }
                }
            }

            return result;

        }

        public IEnumerable<SurveyOverview> GetDetailsOverview(int timePeriodId)
        {
            List<SurveyOverview> result = new List<SurveyOverview>();

            using (SqlCommand command = new SqlCommand("[Survey].[GetDetailsOverview]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        result.Add(new SurveyOverview()
                        {
                            ProjectVersionId = rdr["SurveyProjectVersionID"] != DBNull.Value ? Convert.ToInt32(rdr["SurveyProjectVersionID"]) : default(int)
                        ,
                            COGID = rdr["COGID"].ToString()
                        ,
                            OrganizationName = rdr["OrganizationName"].ToString()
                        ,
                            ProjectName = rdr["ProjectName"].ToString()
                        ,
                            ImprovementType = rdr["ImprovementType"].ToString()
                        ,
                            Network = rdr["Network"].ToString()
                        ,
                            OpenYear = rdr["OpenYear"].ToString()
                        ,
                            FacilityName = rdr["FacilityName"].ToString()
                        ,
                            StartAt = rdr["StartAt"].ToString()
                        ,
                            EndAt = rdr["EndAt"].ToString()
                        ,
                            LanesBase = rdr["LanesBase"] != DBNull.Value ? Convert.ToInt32(rdr["LanesBase"]) : default(int)
                        ,
                            LanesFuture = rdr["LanesFuture"] != DBNull.Value ? Convert.ToInt32(rdr["LanesFuture"]) : default(int)
                        ,
                            FacilityType = rdr["FacilityType"].ToString()
                        ,
                            ModelingCheck = rdr["ModelingCheck"] != DBNull.Value ? Convert.ToBoolean(rdr["ModelingCheck"]) : default(bool)
                        ,
                            LRSRouteName = rdr["LRSRouteName"].ToString()
                        ,
                            LRSBeginMeasure = rdr["LRSBeginMeasure"].ToString()
                        ,
                            LRSEndMeasure = rdr["LRSEndMeasure"].ToString()
                        });
                    }
                }
            }

            return result;

        }

        public DataTable GetModelerExtractResults(int timePeriodId)
        {
            DataTable result;

            try
            {
                using (SqlCommand command = new SqlCommand("Survey.ModelerExtract") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

                    result = this.ExecuteDataTable(command);
                }
            }
            catch
            {
                result = new DataTable();
            }

            return result;
        }

        /// <summary>
        /// Update the project scope information in the database
        /// </summary>
        /// <remarks>Uses the [dbo].[UpdateProjectScope] stored proc.</remarks>
        /// <param name="model"></param>
        public void UpdateProjectScope(ScopeModel model, Project project)
        {
            CheckUpdateStatusId(project);

            using (SqlCommand command = new SqlCommand("[dbo].[UpdateProjectScope]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", model.ProjectVersionId);
                command.Parameters.AddWithValue("@ProjectDescription", model.ProjectDescription);
                command.Parameters.AddWithValue("@BeginConstructionYear", model.BeginConstructionYear);
                command.Parameters.AddWithValue("@EndConstructionYear", model.OpenToPublicYear);
                command.Parameters.AddWithValue("@Module", 1);

                this.ExecuteNonQuery(command);
            }
        }

        #endregion

        #region Location

        public LocationViewModel GetProjectLocationViewModel(int projectVersionId, string year)
        {
            var result = new LocationViewModel();

            result.Current = GetSurvey(year);
            result.Location = GetProjectLocationModel(projectVersionId, year);
            result.Project = GetProjectBasics(projectVersionId);

            result.MuniShares = GetProjectMunicipalityShares(projectVersionId);
            result.CountyShares = GetProjectCountyShares(projectVersionId);

            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("ProjectVersionId", projectVersionId));
            result.AvailableCounties = GetLookupCollection("Lookup_GetAvailableCountyGeographies", "Id", "Label", parameters);

            IList<SqlParameter> parameters2 = new List<SqlParameter>();
            parameters2.Add(new SqlParameter("ProjectVersionId", projectVersionId));
            result.AvailableMunicipalities = GetLookupCollection("Lookup_GetAvailableMuniGeographies", "Id", "Label", parameters2);

            parameters.Clear();
            parameters.Add(new SqlParameter("@TypeId", 1)); // 1 = Route
            result.AvailableRoutes = GetLookupCollection("[dbo].[Lookup_GISCategoriesByType]", "Id", "Label", parameters);

            return result;
        }

        /// <summary>
        /// Get the Location Model
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public LocationModel GetProjectLocationModel(int projectVersionId, string year)
        {
            LocationModel result = null;
            using (SqlCommand command = new SqlCommand("[Survey].[GetProjectLocation]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new LocationModel();
                        result.FacilityName = rdr["FacilityName"].ToString();
                        result.Limits = rdr["Limits"].ToString();
                        result.RouteId = rdr["RouteID"].ToString().SmartParseDefault<int>(default(int));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Store the Location model attributes - Facility Name and Limits ONLY
        /// </summary>
        /// <param name="model"></param>
        public void UpdateProjectLocationModel(LocationModel model, int projectVersionId)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[UpdateProjectLocation]") { CommandType = CommandType.StoredProcedure })
            {
                //model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value
                command.Parameters.AddWithValue("@PROJECTVERSIONID", projectVersionId);
                command.Parameters.AddWithValue("@FACILITYNAME", model.FacilityName);
                command.Parameters.AddWithValue("@LIMITS", model.Limits);
                command.Parameters.AddWithValue("@RouteId", model.RouteId);

                this.ExecuteNonQuery(command);
            }
        }

        #endregion

        #region Funding

        public FundingViewModel GetFundingViewModel(int projectVersionId, string year)
        {
            var result = new FundingViewModel();

            result.Current = GetSurvey(year);
            result.Project = GetProjectBasics(projectVersionId);
            result.Project.Funding = GetFunding(projectVersionId, year);
            
            //result.PlanTypes = GetLookupCollection("[RTP].[Lookup_GetPlanTypes]", "CategoryID", "Category");
            result.AvailableFundingResources = AvailableFundingResources(DRCOG.Domain.Enums.ApplicationState.Survey);

            result.FundingSources = GetProjectFundingSources(projectVersionId);

            return result;
        }

        public Funding GetFunding(int projectVersionId, string plan)
        {
            Funding funding = null;
            using (SqlCommand command = new SqlCommand("[Survey].[GetProjectFunding]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        funding = new Funding()
                        {
                            TotalCost = rdr["TotalCost"] != DBNull.Value ? Convert.ToDecimal(rdr["TotalCost"]) : (Decimal)0.00
                        };
                    }
                }
            }
            //Funding categories = GetPlanReportGroupingCategories(plan);
            //funding.ReportGroupingCategories = categories.ReportGroupingCategories;
            //funding.ReportGroupingCategoriesDetail = categories.ReportGroupingCategoriesDetail;

            return funding;
        }

        public IList<FundingSource> GetProjectFundingSources(int projectVersionId)
        {
            var fundingSources = new List<FundingSource>();

            // Get Agencies which are eligible to sponsor projects
            SqlCommand cmd = new SqlCommand("[Survey].[GetProjectFundingSources]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionId);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    var temp = new FundingSource()
                    {
                        Id = rdr["FundingResourceID"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        Name = rdr["FundingType"].ToString()
                    };
                    if (temp.Id != default(int)) fundingSources.Add(temp);
                }
            }

            return fundingSources;
        }

        public void UpdateFinancialRecord(Project model)
        {
            using (SqlCommand command = new SqlCommand("[Survey].[UpdateFunding]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", model.ProjectVersionId);
                command.Parameters.AddWithValue("@TotalCost", model.Funding.TotalCost > 0 ? (object)model.Funding.TotalCost : (object)DBNull.Value);
                this.ExecuteNonQuery(command);
            }
        }

        public void AddFundingSource(FundingSource model, int projectVersionId)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[AddProjectFundingSource]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionId);
                command.Parameters.AddWithValue("@FundingResourceId", model.Id);
                this.ExecuteNonQuery(command);
            }
        }

        public void DeleteFundingSource(FundingSource model, int projectVersionId)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[DeleteProjectFundingSource]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionId);
                command.Parameters.AddWithValue("@FundingResourceId", model.Id);
                this.ExecuteNonQuery(command);
            }
        }

        #endregion

        //private IDictionary<int, string> GetPlanScenarios(List<SqlParameter> sqlParams)
        //{
        //    return GetLookupCollection("[RTP].[GetPlanScenarios]", "NetworkID", "Scenario", sqlParams);
        //}


        //public IDictionary<int, string> GetPlanScenariosByCycleId(int cycleId)
        //{
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@CycleId", cycleId));

        //    return GetPlanScenarios(sqlParams);
        //}

        //public IDictionary<int, string> GetPlanScenariosForCurrentCycle(int planYearId)
        //{
        //    var cycle = GetCurrentCycle(planYearId); //GetAmendmentDetails(planYearId, RTPCycleStatus.Active);
            
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@CycleId", cycle.Id));
        //    sqlParams.Add(new SqlParameter("@PlanYearId", planYearId));

        //    return GetPlanScenarios(sqlParams);
        //}

        //public IDictionary<int, string> GetPlanScenariosByCycle(string cycle)
        //{
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@Cycle", cycle));

        //    return GetPlanScenarios(sqlParams);
        //}
        //public IDictionary<int, string> GetPlanScenariosByOpenYear(string openYear)
        //{
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@OpenYear", openYear));

        //    return GetPlanScenarios(sqlParams);
        //}

        //public IDictionary<int, string> GetPlanScenarios(int planYearId)
        //{
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@PlanYearId", planYearId));
            
        //    return GetPlanScenarios(sqlParams);
            
        //}

        //public Cycle GetAmendmentDetails(int timePeriodId, Enums.RTPCycleStatus status)
        //{
        //    return GetCycleDetails(timePeriodId, status);
        //}

        //public ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId)
        //{
        //    return GetCollectionOfCycles(timePeriodId, null);
        //}

        //public ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId, Enums.RTPCycleStatus? status)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[GetCurrentPlanCycle]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
        //    if (status.HasValue)
        //    {
        //        cmd.Parameters.AddWithValue("@StatusId", (int)status);
        //    }


        //    var result = new List<CycleAmendment>();
        //    CycleAmendment cycle = null;

        //    using (IDataReader rdr = this.ExecuteReader(cmd))
        //    {
        //        while (rdr.Read())
        //        {
        //            cycle = new CycleAmendment()
        //            {
        //                Id = rdr["id"] != DBNull.Value ? rdr["id"].ToString().SmartParse<int>() : default(int)
        //                ,
        //                Name = rdr["cycle"].ToString()
        //                ,
        //                StatusId = rdr["statusId"].ToString().SmartParseDefault<int>(default(int))
        //            };
        //            cycle.Status = StringEnum.GetStringValue((Enums.RTPCycleStatus)cycle.StatusId);
        //            result.Add(cycle);
        //        }
        //    }

        //    return result;
        //}

        //public CycleAmendment GetCurrentCycle(int timePeriodId)
        //{
        //    var cycles = GetCollectionOfCycles(timePeriodId);

        //    var value = (CycleAmendment)cycles.SingleOrDefault(x => x.StatusId == (int)Enums.RTPCycleStatus.Active) ?? (CycleAmendment)cycles.SingleOrDefault(x => x.StatusId == (int)Enums.RTPCycleStatus.Pending);

        //    return value ?? new CycleAmendment();
        //}

        //public Cycle GetCycleDetails(int timePeriodId)
        //{
        //    return GetCycleDetails(timePeriodId, Enums.RTPCycleStatus.Active);
        //}

        //public Cycle GetCycleDetails(int timePeriodId, Enums.RTPCycleStatus status)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[GetCurrentPlanCycle]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
        //    cmd.Parameters.AddWithValue("@StatusId", (int)status);


        //    var result = new Cycle();

        //    using (IDataReader rdr = this.ExecuteReader(cmd))
        //    {
        //        if (rdr.Read())
        //        {
        //            result.Id = rdr["id"] != DBNull.Value ? rdr["id"].ToString().SmartParse<int>() : default(int);
        //            result.Name = rdr["cycle"].ToString();
        //            result.StatusId = rdr["statusId"].ToString().SmartParseDefault<int>(default(int));
        //        }
        //    }

        //    return result;
        //}


        ///// <summary>
        ///// Populate view model for Plan List
        ///// </summary>
        ///// <returns></returns>
        //public RtpListViewModel GetRtpListViewModel()
        //{
        //    RtpListViewModel model = new RtpListViewModel();

        //    SqlCommand cmd = new SqlCommand("[RTP].[GetPrograms]");
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    IList<RtpStatusModel> rtpPrograms = new List<RtpStatusModel>();
        //    RtpStatusModel sm = null;
        //    using (IDataReader rdr = this.ExecuteReader(cmd))
        //    {
        //        //be sure we got a reader                
        //        while (rdr.Read())
        //        {
        //            sm = new RtpStatusModel();

        //            sm.ProgramId = (int)rdr["ProgramId"];
        //            sm.TimePeriodId = Convert.ToInt32(rdr["TimePeriodId"]);
        //            sm.Plan = rdr["TimePeriod"].ToString();
        //            sm.Adoption = rdr["AdoptionDate"] != DBNull.Value ? (DateTime?)rdr["AdoptionDate"] : null;
        //            sm.CDOTAction = rdr["CDOTActionDate"] != DBNull.Value ? (DateTime?)rdr["CDOTActionDate"] : null;
        //            sm.LastAmended = rdr["LastAmendmentDate"] != DBNull.Value ? (DateTime?)rdr["LastAmendmentDate"] : null;
        //            sm.PublicHearing = rdr["PublicHearingDate"] != DBNull.Value ? (DateTime?)rdr["PublicHearingDate"] : null;
        //            sm.USDOTApproval = rdr["USDOTApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USDOTApprovalDate"] : null;
        //            sm.Notes = rdr["Notes"].ToString();
        //            //sm.IsCurrent = (bool)rdr["Current"];
        //            //sm.IsPending = (bool)rdr["Pending"];
        //            //sm.IsPrevious = (bool)rdr["Previous"];

        //            sm.RtpSummary = GetSummary(sm.Plan);

        //            rtpPrograms.Add(sm);

        //        }
        //    }
        //    model.RTPs = rtpPrograms;
        //    return model;
        //}


        //#region IRtpRepository Members

        ///// <summary>
        ///// Create a new RTP Plan
        ///// </summary>
        ///// <param name="startYear"></param>
        ///// <param name="endYear"></param>
        ///// <param name="offset"></param>
        ///// <returns></returns>
        //public void CreateRtp(string timePeriod)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[CreatePlan]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@TimePeriod ", timePeriod);
        //    cmd.Parameters.AddWithValue("@StatusId ", Enums.RtpTimePeriodStatus.New);
        //    this.ExecuteNonQuery(cmd);
        //}

        //public void SetPlanCurrent(int timePeriodId)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[SetPlanCurrent]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
        //    this.ExecuteNonQuery(cmd);
        //}

        //public int CreateCategory(string categoryName, string shortName, string description, string plan)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[CreateCategory]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@TimePeriod", plan);
        //    cmd.Parameters.AddWithValue("@CategoryName", categoryName);
        //    cmd.Parameters.AddWithValue("@ShortName", shortName);
        //    cmd.Parameters.AddWithValue("@Description", description);
        //    SqlParameter outParam = new SqlParameter("@CategoryId", SqlDbType.Int);
        //    outParam.Direction = ParameterDirection.Output;
        //    cmd.Parameters.Add(outParam);
        //    this.ExecuteNonQuery(cmd);
        //    return (int)cmd.Parameters["@CategoryId"].Value;
        //}


        

        

        //public IList<RtpSummary> GetRestoreProjectList(int timePeriodId)
        //{
        //    IList<RtpSummary> list = new List<RtpSummary>();

        //    try
        //    {
        //        using (SqlCommand command = new SqlCommand("[RTP].[GetRestoreProjectList]") { CommandType = CommandType.StoredProcedure })
        //        {
        //            command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);

        //            using (IDataReader rdr = this.ExecuteReader(command))
        //            {
        //                while (rdr.Read())
        //                {
        //                    RtpSummary summary = new RtpSummary()
        //                    {
        //                        ProjectId = rdr["ProjectId"] != DBNull.Value ? rdr["ProjectId"].ToString().SmartParseDefault(default(int)) : default(int)
        //                        ,
        //                        RTPYearTimePeriodID = rdr["TimePeriodId"] != DBNull.Value ? rdr["TimePeriodId"].ToString().SmartParseDefault(default(short)) : default(short)
        //                        ,
        //                        RtpYear = rdr["TimePeriod"].ToString()
        //                        ,
        //                        ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? rdr["ProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
        //                        ,
        //                        ProjectName = rdr["ProjectName"].ToString()
        //                        ,
        //                        LastAmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? rdr["AmendmentDate"].ToString().SmartParseDefault<DateTime>(DateTime.MinValue) : DateTime.MinValue
        //                        ,
        //                        AmendmentStatusId = rdr["ProjectVersionId"] != DBNull.Value ? rdr["ProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
        //                        ,
        //                        AmendmentStatus = rdr["AmendmentStatus"].ToString()
        //                    };
        //                    list.Add(summary);
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return list;
        //}

        //public IList<RtpSummary> GetAmendableProjects(int timePeriodId)
        //{
        //    IList<RtpSummary> list = new List<RtpSummary>();

        //    try
        //    {
        //        using (SqlCommand command = new SqlCommand("[RTP].[GetProjects]") { CommandType = CommandType.StoredProcedure })
        //        {
        //            command.Parameters.AddWithValue("@YearID", timePeriodId);
        //            //command.Parameters.AddWithValue("@AmendmentStatusID", (int)RTPAmendmentStatus.Pending);

        //            using (IDataReader rdr = this.ExecuteReader(command))
        //            {
        //                while (rdr.Read())
        //                {
        //                    RtpSummary summary = new RtpSummary()
        //                    {
        //                        SponsorAgency = rdr["Sponsor"].ToString()
        //                        ,
        //                        ProjectName = rdr["ProjectName"].ToString()
        //                        ,
        //                        RtpYear = rdr["RtpYear"].ToString()
        //                        ,
        //                        ProjectVersionId = rdr["RTPProjectVersionId"] != DBNull.Value ? rdr["RTPProjectVersionId"].ToString().SmartParseDefault(default(int)) : default(int)
        //                    };
        //                    list.Add(summary);
        //                }
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }

        //    return list;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public StatusViewModel GetRtpStatusViewModel(string year)
        //{
        //    int intOut;
        //    StatusViewModel model = new StatusViewModel();

        //    SqlCommand cmd = new SqlCommand("[RTP].[GetStatus]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@YEAR", SqlDbType.NVarChar));
        //    cmd.Parameters[0].Value = year;

        //    model.RtpSummary.RtpYear = year;
        //    var sm = new RtpStatusModel();

        //    using (IDataReader rdr = this.ExecuteReader(cmd))
        //    {
        //        if (rdr.Read())
        //        {
        //            sm.ProgramId = (int)rdr["ProgramId"];
        //            sm.TimePeriodId = Convert.ToInt32(rdr["TimePeriodId"]);
        //            sm.Plan = rdr["TimePeriod"].ToString();
        //            sm.Adoption = rdr["AdoptionDate"] != DBNull.Value ? (DateTime?)rdr["AdoptionDate"] : null;
        //            //sm.EPAApproval = rdr["USEPAApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USEPAApprovalDate"] : null;
        //            //sm.GovernorApproval = rdr["GovernorApprovalDate"] != DBNull.Value ? (DateTime?)rdr["GovernorApprovalDate"] : null;
        //            sm.LastAmended = rdr["LastAmendmentDate"] != DBNull.Value ? (DateTime?)rdr["LastAmendmentDate"] : null;
        //            sm.PublicHearing = rdr["PublicHearingDate"] != DBNull.Value ? (DateTime?)rdr["PublicHearingDate"] : null;
        //            sm.CDOTAction = rdr["CDOTActionDate"] != DBNull.Value ? (DateTime?)rdr["CDOTActionDate"] : null;
        //            sm.USDOTApproval = rdr["USDOTApprovalDate"] != DBNull.Value ? (DateTime?)rdr["USDOTApprovalDate"] : null;
        //            sm.Notes = rdr["Notes"].ToString();
        //            //sm.IsCurrent = (bool)rdr["Current"];
        //            //sm.IsPending = (bool)rdr["Pending"];
        //            //sm.IsPrevious = (bool)rdr["Previous"];
        //            sm.BaseYearId = rdr["BaseYearId"] != DBNull.Value ? (Int32.TryParse(rdr["BaseYearId"].ToString(), out intOut) ? Int32.Parse(rdr["BaseYearId"].ToString()) : 0)  : 0;
        //            sm.BaseYear = rdr["BaseYear"] != DBNull.Value ? rdr["BaseYear"].ToString() : String.Empty;
        //            sm.Description = rdr["Description"].ToString();
        //            model.RtpSummary.Cycle = new Cycle()
        //            {
        //                Id = rdr["CycleId"].ToString().SmartParseDefault<int>(default(int))
        //                ,
        //                Name = rdr["CycleName"].ToString()
        //                ,
        //                StatusId = rdr["CycleStatusId"].ToString().SmartParseDefault<int>(default(int))
        //                ,
        //                Status = rdr["CycleStatus"].ToString()
        //            };
        //        }
        //    }
            

        //    model.RtpStatus = sm;

        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@TimePeriodTypeID", Enums.TimePeriodType.Year));
        //    model.AvailableYears = GetLookupCollection("[dbo].[Lookup_GetTimePeriodsByTypeId]", "Value", "Label", sqlParams);

        //    sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@TimePeriodId", sm.TimePeriodId));
        //    model.CurrentPlanCycles = GetLookupCollection("[RTP].[GetCurrentPlanCycles]", "id", "cycle", sqlParams);

        //    sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@TimePeriodId", sm.TimePeriodId));
        //    model.PlanUnusedCycles = GetLookupCollection("[RTP].[GetNextCycleKeys]", "id", "cycle", sqlParams);

        //    model.AvailableCycles = GetLookupCollection("[RTP].[GetAvailablePlanCycles]", "id", "cycle");


        //    //Enums.RTPCycleStatus rtpCycleStatus = model.RtpStatus.IsCurrent ? Enums.RTPCycleStatus.Active : Enums.RTPCycleStatus.Pending;
        //    //model.RtpSummary.Cycle = GetAmendmentDetails(sm.TimePeriodId, rtpCycleStatus);
            
        //    return model;
        //}

        //public String SetActiveCycle(int cycleId, int timePeriodId)
        //{
        //    string result = "";

        //    SqlCommand cmd = new SqlCommand("[RTP].[SetActiveCycle]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CycleId", cycleId);
        //    cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
        //    //cmd.Parameters.AddWithValue("@CycleStatus", RTPCycleStatus.);

        //    try
        //    {
        //        this.ExecuteNonQuery(cmd);

        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }

        //    return result;
        //}

        //public String UpdateTimePeriodCycleOrder(string cycles)
        //{
        //    string result = String.Empty;

        //    var modifiedOrder = new List<int>(GetModifiedArrayIndexes(cycles));

        //    int position = 0;
        //    if (modifiedOrder.Count > 0)
        //    {
        //        using (TransactionScope ts = new TransactionScope())
        //        {
        //            foreach (var item in modifiedOrder)
        //            {
        //                if (!UpdateCycleSort((int)item, ++position))
        //                {
        //                    //error = true;
        //                    throw new Exception("Sort Exception");
        //                }
        //            }
        //            ts.Complete();
        //        }
        //    }

        //    return result;
        //}

        //private bool UpdateCycleSort(int cycleId, int order)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[UpdateCycleSort]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CycleId", cycleId);
        //    cmd.Parameters.AddWithValue("@Order", order);
        //    SqlParameter sqlout = new SqlParameter("@error",SqlDbType.Bit);
        //    sqlout.Direction = ParameterDirection.Output;
        //    cmd.Parameters.Add(sqlout);

            
        //    try
        //    {
        //        this.ExecuteNonQuery(cmd);
        //        if((bool)cmd.Parameters["@error"].Value)
        //            return false;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }

        //    return false;
        //}

        //private IEnumerable<int> GetModifiedArrayIndexes(string positions)
        //{
        //    IEnumerable<string> values = positions.Split(new char[] { ',' });

        //    foreach (var position in values)
        //    {
        //        yield return Convert.ToInt32(position.Replace("cycle_", ""));
        //    }
        //}

        ///// <summary>
        ///// Update the RTP Status in the database
        ///// </summary>
        ///// <param name="model"></param>
        //public void UpdateRtpStatus(RtpStatusModel model)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[UpdateRtpStatus]");
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.AddWithValue("@PROGRAMID", model.ProgramId);
        //    cmd.Parameters.AddWithValue("@TIMEPERIODID", model.TimePeriodId);
        //    cmd.Parameters.AddWithValue("@YEAR", model.Plan);
        //    cmd.Parameters.AddWithValue("@BaseYearId", model.BaseYearId > 0 ? (object)model.BaseYearId : 0);
        //    //cmd.Parameters.AddWithValue("@CURRENT", model.IsCurrent);
        //    //cmd.Parameters.AddWithValue("@PENDING", model.IsPending);
        //    //cmd.Parameters.AddWithValue("@PREVIOUS", model.IsPrevious);
        //    cmd.Parameters.AddWithValue("@NOTES", model.Notes);
        //    cmd.Parameters.AddWithValue("@Description", model.Description);
        //    cmd.Parameters.AddWithValue("@ADOPTIONDATE", model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value);
        //    //cmd.Parameters.AddWithValue("@GOVAPPROVALDATE ", model.GovernorApproval != null ? (object)model.GovernorApproval.Value : (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@PUBLICHEARINGDATE", model.PublicHearing != null ? (object)model.PublicHearing.Value : (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@DOTDATE", model.USDOTApproval != null ? (object)model.USDOTApproval.Value : (object)DBNull.Value);
        //    cmd.Parameters.AddWithValue("@CDOTDATE", model.CDOTAction != null ? (object)model.CDOTAction.Value : (object)DBNull.Value);
            
        //    //cmd.Parameters.AddWithValue("@EPADATE ", model.EPAApproval != null ? (object)model.EPAApproval.Value : (object)DBNull.Value);

        //    this.ExecuteNonQuery(cmd);

        //}

        ///// <summary>
        ///// Add a cycle to a TimePeriod
        ///// </summary>
        ///// <param name="timePeriodId"></param>
        ///// <param name="cycleId"></param>
        //public string AddCycleToTimePeriod(string timePeriod, int cycleId)
        //{
        //    string result = "";

        //    SqlCommand cmd = new SqlCommand("[RTP].[AddCycleToTimePeriod]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@TimePeriodId", GetYearId(timePeriod));
        //    cmd.Parameters.AddWithValue("@CycleId", cycleId);
        //    cmd.Parameters.AddWithValue("@StatusId", Enums.RTPCycleStatus.Inactive);

        //    try
        //    {
        //        this.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }

        //    return result;
        //}

        

        ///// <summary>
        ///// Remove a Cycle from TimePeriod
        ///// </summary>
        ///// <param name="cycleId"></param>
        //public string RemoveCycleFromTimePeriod(int cycleId)
        //{
        //    string result = "";

        //    SqlCommand cmd = new SqlCommand("[RTP].[RemoveCycleFromTimePeriod]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CycleId", cycleId);
        //    cmd.Parameters.AddWithValue("@StatusId", Enums.RTPCycleStatus.New);

        //    try
        //    {
        //        this.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }

        //    return result;
        //}

        //public string UpdateTimePeriodStatusId(int timePeriodId, int statusId)
        //{
        //    string result = "";

        //    SqlCommand cmd = new SqlCommand("[dbo].[UpdateTimePeriodStatusId]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
        //    cmd.Parameters.AddWithValue("@StatusId", statusId);

        //    try
        //    {
        //        this.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }

        //    return result;
        //}


        ///// <summary>
        ///// Create a Cycle
        ///// </summary>
        ///// <param name="cycle"></param>
        //public string CreateCycle(string cycle)
        //{
        //    string result = "";

        //    SqlCommand cmd = new SqlCommand("[RTP].[CreateCycle]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Cycle", cycle);

        //    try
        //    {
        //        this.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }

        //    return result;
        //}

        //public string UpdateCycleName(int cycleId, string cycle)
        //{
        //    string result = String.Empty;

        //    SqlCommand cmd = new SqlCommand("[RTP].[UpdateCycleName]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@CycleId", cycleId);
        //    cmd.Parameters.AddWithValue("@Cycle", cycle);

        //    try
        //    {
        //        this.ExecuteNonQuery(cmd);
        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }

        //    return result;
        //}
        
        /// <summary>
        /// Create a new Project in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateProject(string projectName, string facilityName, int timePeriodId, int sponsorOrganizationId, int sponsorContactId, int improvementTypeId, string startAt, string endAt)
        {
            int retval = default(int);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (SqlCommand command = new SqlCommand("[Survey].[CreateProject]") { CommandType = CommandType.StoredProcedure })
                    {
                        command.Parameters.AddWithValue("@ProjectName", projectName);
                        command.Parameters.AddWithValue("@SponsorOrganizationId", sponsorOrganizationId);
                        if (!sponsorContactId.Equals(default(int))) command.Parameters.AddWithValue("@SponsorContactId", sponsorContactId);
                        command.Parameters.AddWithValue("@FacilityName", facilityName);
                        command.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
                        //command.Parameters.AddWithValue("@AmendmentStatusId", (int)Enums.RTPAmendmentStatus.Pending);
                        command.Parameters.AddWithValue("@VersionStatusId", (int)DRCOG.Domain.Enums.SurveyVersionStatus.Pending);
                        //command.Parameters.AddWithValue("@SponsorContactId", sponsorContactId);
                        command.Parameters.AddWithValue("@UpdateStatusId", (int)DRCOG.Domain.Enums.SurveyUpdateStatus.Edited);
                        command.Parameters.AddWithValue("@ImprovementTypeId", improvementTypeId);
                    
                        //command.Parameters.AddWithValue("@ActionStatusId", default(int));
                        //command.Parameters.AddWithValue("@AmendmentTypeId", (int)AmendmentType.Administrative);
                        SqlParameter outParam = new SqlParameter("@ProjectVersionId", SqlDbType.Int);
                        outParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outParam);
                        this.ExecuteNonQuery(command);
                        retval = (int)command.Parameters["@ProjectVersionId"].Value;
                    }
                    if (retval.Equals(default(int))) throw new Exception("ProjectVersionId was not returned");

                    SegmentModel segmentModel = new SegmentModel()
                    {
                        ProjectVersionId = retval
                        ,
                        FacilityName = facilityName
                        ,
                        StartAt = startAt
                        ,
                        EndAt = endAt
                    };
                    int segmentid = this.AddSegment(segmentModel);
                    if (segmentid.Equals(default(int))) throw new Exception("Segment was not created");
                    ts.Complete();
                }
            }
            catch (Exception exc)
            {

            }

            return retval;
        }

        

        //public IDictionary<int, string> GetPlanAvailableProjects(int planId, int cycleId)
        //{
        //    List<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@TimePeriodId", planId));
        //    sqlParams.Add(new SqlParameter("@CycleId", cycleId));

        //    //return GetLookupCollection("[RTP].[GetPlanAvailableProjects]", "RTPProjectVersionID", "COGID", sqlParams);

        //    Dictionary<string, string> valueAddonValueSeparator = new Dictionary<string, string>();
        //    valueAddonValueSeparator.Add("ProjectName", " - ");
        //    return GetLookupCollection("[RTP].[GetPlanAvailableProjects]", "RTPProjectVersionID", "Sponsor", sqlParams, valueAddonValueSeparator);
        //}
        
        

        //public ProjectSearchViewModel GetProjectSearchViewModel(string year,string currentProgram)
        //{
        //    var result = new ProjectSearchViewModel();

        //    // get project search model. This is now done in the (RTP)Controller.
        //    //result.ProjectSearchModel = this.GetProjectSearchModel();

        //    // fill form collections
        //    result.EligibleAgencies = GetCurrentTimePeriodSponsorAgencies(year, _appState).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
        //    Dictionary<string,string> valueAddonValueSeparator = new Dictionary<string,string>();
        //    valueAddonValueSeparator.Add("Status", " :: ");
        //    valueAddonValueSeparator.Add("cycle", " - ");
        //    result.AvailablePlanYears = GetLookupCollection("[RTP].[Lookup_GetPlanYears]", "Id", "Label", null,valueAddonValueSeparator);
        //    result.AvailableImprovementTypes = AvailableImprovementTypes(24);
        //    //result.AvailableImprovementTypes = GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
        //    result.AvailableProjectTypes = GetLookupCollection("Lookup_GetProjectTypes", "Id", "Label");
        //    result.AvailableNetworks = GetLookupCollection("[dbo].[Lookup_GetNetworks]", "Id", "Label");

        //    IList<SqlParameter> paramList = new List<SqlParameter>();
        //    paramList.Add(new SqlParameter("@StatusType", "RTP Amendment Status"));
        //    result.AvailableAmendmentStatuses = GetLookupCollection("Lookup_GetStatuses", "Id", "Label", paramList);
        //    result.AvailablePlanTypes = GetLookupCollection("[RTP].[Lookup_GetPlanTypes]", "CategoryID", "Category");
        //    result.RtpSummary = GetSummary(year);
        //    return result;
        //}

        //public void SetProjectSearchDefaults(RTPSearchModel model)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[GetSearchDefaults]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    int testval;
        //    using (IDataReader rdr = this.ExecuteReader(cmd))
        //    {
        //        while (rdr.Read())
        //        {
        //            model.RtpYearID = (!String.IsNullOrEmpty(rdr["Id"].ToString()) && Int32.TryParse(rdr["Id"].ToString(), out testval)) ? Int32.Parse(rdr["Id"].ToString()) : 0;
        //            model.RtpYear = rdr["Plan"].ToString();
        //            break; // for now this is how it has to be. Just getting a single record.
        //        }
        //    }
        //}

        public SponsorsViewModel GetSponsorsViewModel(string timePeriod)
        {
            //ToDo: Need to make the two methods that make up this viewModel be from the
            //TransportationRepository so that the ProjectRepository can access them. -DBD
            var model = new SponsorsViewModel();
            model.Current = this.GetSurvey(timePeriod);

            // Get Agencies which are eligible to sponsor projects
            model.EligibleAgencies = GetCurrentTimePeriodSponsorAgencies(timePeriod, _appState);
            model.AvailableAgencies = GetAvailableTimePeriodSponsorAgencies(timePeriod, _appState);

            List<SqlParameter> improvementTypeParams = new List<SqlParameter>();
            improvementTypeParams.Add(new SqlParameter("@TimePeriodId", model.Current.Id));
            model.EligibleImprovementTypes = GetImprovementTypes(improvementTypeParams);

            model.AvailableImprovementTypes = GetImprovementTypes(new List<SqlParameter>()).ToList();
            HashSet<int> improvementTypeIds = new HashSet<int>(model.EligibleImprovementTypes.Select(x => x.Id));
            (model.AvailableImprovementTypes as List<ImprovementType>).RemoveAll(x => improvementTypeIds.Contains(x.Id));

            List<SqlParameter> fundingResourceParams = new List<SqlParameter>();
            fundingResourceParams.Add(new SqlParameter("@TimePeriodId", model.Current.Id));
            fundingResourceParams.Add(new SqlParameter("@ProgramId", (int)DRCOG.Domain.Enums.ApplicationState.Survey));
            model.EligibleFundingResources = GetFundingResources(fundingResourceParams);

            fundingResourceParams = new List<SqlParameter>();
            fundingResourceParams.Add(new SqlParameter("@ProgramId", (int)DRCOG.Domain.Enums.ApplicationState.Survey));
            model.AvailableFundingResources = GetFundingResources(fundingResourceParams);
            HashSet<int> fundingResourceIds = new HashSet<int>(model.EligibleFundingResources.Select(x => (int)x.FundingResourceId));
            (model.AvailableFundingResources as List<FundingResource>).RemoveAll(x => fundingResourceIds.Contains((int)x.FundingResourceId));


            return model;
        }
        
        //public RtpSummary GetSummary(string plan)
        //{
        //    SqlCommand cmd = new SqlCommand("[RTP].[GetSummary]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@Plan", plan);
        //    RtpSummary model = new RtpSummary();
        //    using (IDataReader rdr = this.ExecuteReader(cmd))
        //    {
        //        if (rdr.Read())
        //        {
        //            model.RtpYear = plan;
        //            //model.IsCurrent = rdr["IsCurrent"] != DBNull.Value ? (bool)rdr["IsCurrent"] : false;
        //            //model.IsPending = rdr["IsPending"] != DBNull.Value ? (bool)rdr["IsPending"] : false;
        //            model.RTPYearTimePeriodID = rdr["TimePeriodID"].ToString().SmartParseDefault<short>(default(short));
        //            model.TimePeriodStatusId = rdr["TimePeriodStatusId"].ToString().SmartParseDefault<int>(default(int));
        //            model.Cycle = new Cycle()
        //            {
        //                Id = rdr["CycleId"].ToString().SmartParseDefault<int>(default(int))
        //                ,
        //                Name = rdr["CycleName"].ToString()
        //                ,
        //                StatusId = rdr["CycleStatusId"].ToString().SmartParseDefault<int>(default(int))
        //                ,
        //                Status = rdr["CycleStatus"].ToString()
        //            };
        //        }
        //        //else
        //        //{
        //        //    model = null;
        //        //}
        //    }


        //    model.Cycle = new Cycle();
        //    model.Cycle = GetCurrentCycle(GetYearId(plan));
        //    return model;

        //}

        ///// <summary>
        ///// Update the list of Eligible Agencies associated with a particular Survey
        ///// </summary>
        ///// <param name="model"></param>
        //public void UpdateEligibleAgencies(string plan, List<int> AddedOrganizations, List<int> RemovedOrganizations)
        //{
        //    //first remove the orgs that were dropped
        //    foreach (int orgId in RemovedOrganizations)
        //    {
        //        SqlCommand cmd = new SqlCommand("[RTP].[DeleteSponsorOrganization]");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Plan ", plan);
        //        cmd.Parameters.AddWithValue("@SponsorId ", orgId);
        //        this.ExecuteNonQuery(cmd);
        //    }

        //    //now add in the added orgs
        //    foreach (int orgId in AddedOrganizations)
        //    {
        //        SqlCommand cmd = new SqlCommand("[RTP].[InsertSponsorOrganization]");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Plan ", plan);
        //        cmd.Parameters.AddWithValue("@SponsorId ", orgId);
        //        this.ExecuteNonQuery(cmd);
        //    }
        //}

        //public IList<FundingSourceModel> GetFundingSources(string plan)
        //{
        //    var model = new FundingSourceViewModel();
        //    SqlCommand cmd = new SqlCommand("[RTP].[GetFundingSources]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    var list = new List<FundingSourceModel>();
        //    using (IDataReader rdr = this.ExecuteReader(cmd))
        //    {
        //        while (rdr.Read())
        //        {
        //            var fs = new FundingSourceModel();

        //            fs.FundingTypeId = (int)rdr["FundingTypeId"];
        //            fs.FundingType = rdr["FundingType"].ToString();
        //            //fs.FundingLevel = rdr["FundingLevel"] != DBNull.Value ? rdr["FundingLevel"].ToString() : "NULL IN DB";
        //            fs.Code = rdr["Code"].ToString();
        //            fs.RecipentOrganization = rdr["Recipient"].ToString();
        //            fs.SourceOrganizatin = rdr["Source"].ToString();
        //            fs.Plan = plan;
        //            fs.Selector = "Not in DB";
        //            fs.IsDiscretionary = (bool)rdr["Discretion"];

        //            list.Add(fs);
        //        }

        //    }
        //    return list;

        //}

        //#endregion


    }
}
