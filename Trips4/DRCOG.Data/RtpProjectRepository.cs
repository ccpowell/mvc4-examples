#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR	        REMARKS
 * 08/06/2009	NKirkes         1. Initial Creation (DTS). 
 * 01/25/2010	DDavidson	    2. Reformatted.
 * 02/03/2010   DDavidson       3. Multiple improvements. Fixed GetProjectInfoViewModel.
 * 02/17/2010   DDavidson       4. Derived from TransportationRepository.
 * 02/25/2010   DTucker         5. Added CopyProject.
 * 03/18/2010   DTucker         6. Added GetPoolProjects.
 * 04/26/2010   DDavidson       7. Fixed GetSponsorContact with appropriate LINQ. Updated GetProjectInfoViewModel 
 *                                  to get list of eligible/available sponsors.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels;
using System.Collections;
using DRCOG.Domain;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.ViewModels.RTP.Project;
using DRCOG.Domain.ViewModels.RTP;
using DRCOG.Domain.Helpers;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.Xml;
using System.IO;
using DRCOG.Common.Util;

namespace DRCOG.Data
{
    public class RtpProjectRepository : RtpRepository, IRtpProjectRepository
    {

        public RtpProjectRepository()
        {
            _appState = Enums.ApplicationState.RTP;
        }

        #region Lookups
        protected IDictionary<int,string> AvailableAdminLevels
        {
            get
            {
                return GetLookupCollection("Lookup_GetProjectAdministrativeLevels", "Id", "Label");
            }
        }
        protected IDictionary<int,string> AvailableSponsors
        {
            get
            {
                return GetLookupCollection("Lookup_GetSponsorOrganizations", "Id", "Label");
            }
        }
        //protected IDictionary<int,string> AvailableImprovementTypes()
        //{
        //    return GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
        //}
        //protected IDictionary<int, string> AvailableImprovementTypes(int rspCodeId)
        //{
        //    List<SqlParameter> sqlParms = new List<SqlParameter>();
        //    sqlParms.Add(new SqlParameter("@RSPCodeId", rspCodeId));
        //    return GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label", sqlParms);
        //}
        protected IDictionary<int,string> AvailableRoadOrTransitTypes
        {
            get
            {
                return GetLookupCollection("Lookup_GetRoadOrTransitCategories", "Id", "Label");
            }
        }
        protected IDictionary<int,string> AvailableSponsorContacts(InfoModel info)
        {
            if (info.SponsorId.HasValue)
            {
                return GetSponsorContacts(info.SponsorId.Value);
            }
            return null;
        }
        protected IDictionary<int,string> AvailableProjectTypes
        {
            get
            {
                 return GetLookupCollection("Lookup_GetProjectTypes", "Id", "Label");
            }
        }
        protected IDictionary<int,string> AvailableSelectionAgencies
        {
            get
            {
                 return GetLookupCollection("Lookup_GetSelectors", "Id", "Label");
            }
        }
        protected IDictionary<int,string> AvailablePools(RtpSummary summary)
        {
                return GetPoolNames(1, summary.RTPYearTimePeriodID);
        }
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


        #endregion

        #region GENERAL METHODS

        public String GetValueByKey(IDictionary<int, string> dictionary, int Id)
        {
            if (dictionary.ContainsKey(Id))
            {
                return dictionary[Id].ToString();
            }
            return String.Empty;
        }

        /// <summary>
        /// Get a RtpProjectVersionInfoModel. This is the model behind the
        /// Info Tab
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public InfoModel GetProjectInfo(int versionId, string year)
        {
            //throw new NotImplementedException();
            InfoModel result = null;
            using (SqlCommand command = new SqlCommand("[RTP].[GetProjectVersionInfo]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@RtpYear", year);
                command.Parameters.AddWithValue("@RtpProjectVersion", versionId);
                command.Parameters.AddWithValue("@IsActive", 1);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new InfoModel();
                        result.AdministrativeLevelId = rdr["AdministrativeLevelID"] != DBNull.Value ? (int?)rdr["AdministrativeLevelID"] : null;
                        result.DRCOGNotes = rdr["DRCOGNotes"].ToString();
                        result.ImprovementTypeId = rdr["ImprovementTypeId"] != DBNull.Value ? (int?)rdr["ImprovementTypeId"] : null;
                        result.PoolMasterVersionID = rdr["PoolMasterVersionID"] != DBNull.Value ? (int?)rdr["PoolMasterVersionID"] : null;
                        result.ProjectId = rdr["ProjectID"] != DBNull.Value ? (int?)rdr["ProjectID"] : null;
                        result.ProjectName = rdr["ProjectName"].ToString();
                        result.ProjectTypeId = rdr["ProjectTypeID"] != DBNull.Value ? (int?)rdr["ProjectTypeID"] : null;
                        result.ProjectType = rdr["ProjectType"].ToString();
                        result.ProjectVersionId = rdr["ProjectVersionID"] != DBNull.Value ? (int)rdr["ProjectVersionID"] : default(int);
                        result.SponsorContactId = rdr["SponsorContactID"] != DBNull.Value ? (int?)rdr["SponsorContactID"] : null;
                        result.SponsorId = rdr["SponsorID"] != DBNull.Value ? (int?)rdr["SponsorID"] : null;
                        result.SponsorNotes = rdr["SponsorNotes"].ToString();
                        result.RtpId = rdr["RtpId"].ToString();
                        result.RtpYear = rdr["RtpYear"].ToString();
                        result.TransportationTypeId = rdr["TransportationTypeID"] != DBNull.Value ? (int?)rdr["TransportationTypeID"] : null;
                        result.SelectionAgencyId = rdr["SelectionAgencyID"] != DBNull.Value ? (int?)rdr["SelectionAgencyID"] : null;
                        result.IsRegionallySignificant = rdr["RegionalSignificance"] != DBNull.Value ? (bool)rdr["RegionalSignificance"] : false;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Get a list of the projects in the specified Year
        /// </summary>
        /// <returns></returns>
        public IList<ProjectAmendments> GetProjectAmendments(RTPSearchModel projectSearchModel)
        {
            IList<ProjectAmendments> list = new List<ProjectAmendments>();

            SqlCommand cmd = new SqlCommand("[RTP].[GetProjectAmendmentList]");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProjectId", projectSearchModel.ProjectId);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new ProjectAmendments()
                    {
                        ProjectVersionId = (int)rdr["ProjectVersionId"]
                        ,
                        AmendmentStatus = rdr["AmendmentStatus"] != DBNull.Value ? rdr["AmendmentStatus"].ToString() : ""
                        ,
                        AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? (DateTime)rdr["AmendmentDate"] : DateTime.Now
                        ,
                        ProjectName = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : ""
                    });
                }
            }

            // These are processed via the SQL retrieval code optimization, so only process this one when exclude is checked.
            if ((projectSearchModel.RtpYear != null) && (projectSearchModel.Exclude_Year))
            {
                list = (from fli in list
                        where (fli.Year != projectSearchModel.RtpYear)
                        select fli).ToList<ProjectAmendments>();
            }

            if (projectSearchModel.AmendmentStatus != null)
            {
                list = (from fli in list
                        where ((fli.AmendmentStatus == projectSearchModel.AmendmentStatus) && (!projectSearchModel.Exclude_AmendmentStatus))
                        || ((fli.AmendmentStatus != projectSearchModel.AmendmentStatus) && (projectSearchModel.Exclude_AmendmentStatus))
                        select fli).ToList<ProjectAmendments>();
            }

            if ((projectSearchModel.ProjectName != null) && (!projectSearchModel.ProjectName.Equals("")))
            {
                list = (from fli in list
                        where ((fli.ProjectName.Contains(projectSearchModel.ProjectName)) && (!projectSearchModel.Exclude_ProjectName))
                        || ((!fli.ProjectName.Contains(projectSearchModel.ProjectName)) && (projectSearchModel.Exclude_ProjectName))
                        select fli).ToList<ProjectAmendments>();
            }

            return list;
        }

        /// <summary>
        /// Gets a human readable summary for the RTP Project Version
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <remarks>
        /// This is used in the project general information block on the right of
        /// the project tabs</remarks>
        /// <returns></returns>
        public RtpSummary GetProjectSummary(int versionId, string plan)
        {
            //throw new NotImplementedException();
            RtpSummary result = null;
            using (SqlCommand command = new SqlCommand("[RTP].[GetProjectVersionInfoSummary]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@RtpProjectVersion", versionId);
                command.Parameters.AddWithValue("@Year", plan);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new RtpSummary();
                        result.ProjectType = rdr["ImprovementType"].ToString();
                        result.ProjectName = rdr["ProjectName"].ToString();
                        result.COGID = rdr["COGID"].ToString();
                        result.ProjectId = (int)rdr["ProjectId"];

                        //To be editable, a ProjectVersion must be both Current and in the Current TIP
                        // IsVersionCurrent = Active
                        //bool IsVersionCurrent = rdr["IsVersionCurrent"] != DBNull.Value ? (bool)rdr["IsVersionCurrent"] : false;
                        result.IsActive = rdr["IsVersionCurrent"] != DBNull.Value ? (bool)rdr["IsVersionCurrent"] : false;
                        //result.IsCurrent = rdr["IsRtpCurrent"] != DBNull.Value ? (bool)rdr["IsRtpCurrent"] : false;
                        //result.IsPending = rdr["IsPending"] != DBNull.Value ? (bool)rdr["IsPending"] : false;
                        result.IsTopStatus = rdr["IsTopStatus"] != DBNull.Value ? (bool)rdr["IsTopStatus"] : false;
                        result.AmendmentStatusId = (int)rdr["AmendmentStatusId"];
                        //(Int32)TIPAmendmentStatus.Amended

                        //result.IsEditable = result.IsCurrent;
                            //&& (new int[] { (int)TIPAmendmentStatus.Proposed, (int)TIPAmendmentStatus.Submitted }.Contains(amendmentStatusId));

                        result.AmendmentStatus = rdr["AmendmentStatus"].ToString();

                        result.ProjectVersionId = rdr["ProjectVersionID"] != DBNull.Value ? (int)rdr["ProjectVersionID"] : default(int);
                        result.PreviousVersionId = rdr["PreviousVersionID"].ToString().SmartParseDefault<int>(default(int));
                        result.NextVersionId = rdr["NextVersionID"] != DBNull.Value ? (int?)rdr["NextVersionID"] : null;
                        result.SponsorAgency = rdr["Sponsor"].ToString();
                        result.RtpYear = rdr["RtpYear"].ToString();
                        result.NextVersionYear = rdr["NextVersionRtpYear"].ToString();
                        result.PreviousVersionYear = rdr["PreviousVersionRtpYear"].ToString();
                        result.RTPYearTimePeriodID = rdr["RtpYearTimePeriodID"] != DBNull.Value ? (short)rdr["RtpYearTimePeriodID"] : (short)0;
                        result.ProjectType = rdr["ProjectType"].ToString();
                        result.PlanType = rdr["PlanType"].ToString();
                        
                        result.AdoptionDate = rdr["AdoptionDate"] != DBNull.Value ? (DateTime)rdr["AdoptionDate"] : DateTime.MinValue;
                        result.LastAmendmentDate = rdr["LastAmendmentDate"] != DBNull.Value ? (DateTime)rdr["LastAmendmentDate"] : DateTime.MinValue;
                        result.RtpId = result.RTPYearTimePeriodID.ToString();
                        result.CategoryId = rdr["RTPCategoryID"] != DBNull.Value ? (int)rdr["RTPCategoryID"] : Int32.MinValue;
                        result.TimePeriodStatusId = rdr["TimePeriodStatusId"].ToString().SmartParseDefault<int>(default(int));
                    }
                }
            }

            // Set cycle info
            result.Cycle = GetProjectCycleInfo(versionId);
            //result.Cycle.Name = cycleInfo.Cycle;
            //result.Cycle.Id = cycleInfo.CycleId;

            return result;
        }

        public Cycle GetProjectCycleInfo(int versionId)
        {
            Cycle result = null;
            using (SqlCommand command = new SqlCommand("[RTP].[GetProjectVersionCycle]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@RtpProjectVersion", versionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new Cycle();
                        result.Id = rdr["CycleId"] != DBNull.Value ? (int)rdr["CycleId"] : Int32.MinValue;
                        result.Name = rdr["Cycle"] != DBNull.Value ? rdr["Cycle"].ToString() : String.Empty;
                        result.Status = rdr["Status"] != DBNull.Value ? rdr["Status"].ToString() : String.Empty;
                        result.Reason = rdr["Reason"] != DBNull.Value ? rdr["Reason"].ToString() : String.Empty;
                        result.Date = rdr["Date"] != DBNull.Value ? (DateTime)rdr["Date"] : DateTime.MinValue;
                        result.StatusId = rdr["statusId"].ToString().SmartParseDefault<int>(default(int));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Create a new Project in the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int CreateProject(string projectName, string facilityName, string tipYear, int sponsorOrganizationId)
        {
            throw new NotImplementedException();
            //SqlCommand cmd = new SqlCommand("TIP.CreateProject");
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@PROJECTNAME ", projectName);
            //cmd.Parameters.AddWithValue("@SponsorOrganizationId ", sponsorOrganizationId);
            //cmd.Parameters.AddWithValue("@FacilityName ", facilityName);
            ////cmd.Parameters.AddWithValue("@SPONSORID  ", model.SponsorId != null ? (object)model.SponsorId.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@SPONSORCONTACTID  ", model.SponsorContactId != null ? (object)model.SponsorContactId.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@ADMINLEVELID  ", model.AdministrativeLevelId != null ? (object)model.AdministrativeLevelId.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@PROJECTTYPEID  ", model.ProjectTypeId != null ? (object)model.ProjectTypeId.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@IMPTYPEID  ", model.ImprovementTypeId != null ? (object)model.ImprovementTypeId.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@PROJECTPOOLID ", model.ProjectPoolId != null ? (object)model.ProjectPoolId.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@ISPOOLMASTER ", model.IsPoolMaster != null ? (object)model.IsPoolMaster.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@SELECTIONAGENCYID  ", model.SelectionAgencyId != null ? (object)model.IsPoolMaster.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@TRANSTYPEID ", model.TransportationTypeId != null ? (object)model.IsPoolMaster.Value : (object)DBNull.Value);
            ////cmd.Parameters.AddWithValue("@SPONSORNOTES  ", model.SponsorNotes);
            //cmd.Parameters.AddWithValue("@TIPYEAR  ", tipYear);
            //SqlParameter outParam = new SqlParameter("@PROJECTVERSIONID", SqlDbType.Int);
            //outParam.Direction = ParameterDirection.Output;
            //cmd.Parameters.Add(outParam);
            //SqlConnection con = new SqlConnection(this.ConnectionString);
            //cmd.Connection = con;
            //con.Open();
            //cmd.ExecuteNonQuery();
            //con.Close();
            ////return the value
            //return (int)cmd.Parameters["@PROJECTVERSIONID"].Value;
        }

        public RtpSummary CopyProject(Int32 cycleId, Int32 projectVersionId)
        {
            return CopyProject(String.Empty, cycleId, projectVersionId);
        }

        public RtpSummary CopyProject(Int32 projectVersionId)
        {
            return CopyProject(String.Empty, default(Int32), projectVersionId);
        }

        /// <summary>
        /// Copies a Project in the database
        /// </summary>
        /// <param name="projectVersionId"></param>
        /// <returns>ProjectVersionID</returns>
        public RtpSummary CopyProject(string plan, Int32 cycleId, Int32 projectVersionId)
        {
            CycleAmendment cycle = GetCurrentCycle(GetYearId(plan, Enums.TimePeriodType.PlanYear));

            RtpSummary result = null;
            using (SqlCommand command = new SqlCommand("[RTP].[CopyProject]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@CurrentProjectVersionId", projectVersionId);
                if(!String.IsNullOrEmpty(plan))
                    command.Parameters.AddWithValue("@TimePeriod", plan);
                if(!cycleId.Equals(default(Int32)) )
                    command.Parameters.AddWithValue("@CycleId", cycleId);

                using (IDataReader rdr = this.ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new RtpSummary()
                        {
                            SponsorAgency = rdr["Sponsor"].ToString()
                            ,
                            TIPId = rdr["TIPID"].ToString()
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
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Deletes a ProjectVersion
        /// </summary>
        /// <param name="projectVersionId"></param>
        /// <param name="expectedStatus"></param>
        /// <returns></returns>
        public Boolean DeleteProjectVersion(int projectVersionId/*, Enums.RTPAmendmentStatus expectedStatus*/)
        {
            try
            {
                using (SqlCommand command = new SqlCommand("[RTP].[DeleteProjectVersion]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@ProjectVersionID", projectVersionId);
                    this.ExecuteNonQuery(command);
                }
                return true;
            }
            catch
            {

            }
            return false;

            //string fileName = (String)cmd.Parameters["@fileName"].Value;
            //try
            //{
            //    if (!fileName.Equals(String.Empty))
            //    {
            //        FileHandler.Delete(fileName, model.LocationMapPath);
            //    }
            //    else
            //    {
            //        throw new NoNullAllowedException();
            //    }
            //}
            //catch
            //{

            //}

            //return (String)cmd.Parameters["@fileName"].Value;
        }

        /// <summary>
        /// Restore a ProjectVersion
        /// </summary>
        /// <param name="projectVersionID"></param>
        /// <param name="plan"></param>
        /// <returns>RtpSummary</returns>
        public RtpSummary RestoreProjectVersion(int projectVersionID, string plan)
        {
            Cycle cycle = null;

            //result = new RtpSummary()
            //{
            //    RtpYear = plan,
            //    Title = "Test Project",
            //    ProjectVersionId = 20818,
            //    COGID = "ADCO-2010-04",
            //    AmendmentStatus = "Pending",
            //    ImprovementType = "ImprovementType",
            //    SponsorAgency = "Adams County"
            //};

            cycle = GetCurrentCycle(GetYearId(plan, Enums.TimePeriodType.PlanYear));

            return CopyProject(plan, cycle.Id, projectVersionID);
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
        //    //string contact = sponsorContacts.Where(x => x.Key.Equals(sponsorContactId)).Select(x => x.Value).ToString();
        //    if (sponsorContacts.ContainsKey(sponsorContactId))
        //    {
        //        string contact = (from c in sponsorContacts
        //                          where c.Key.Equals(sponsorContactId)
        //                          select c).First<KeyValuePair<int, string>>().Value.ToString();
        //        return contact;
        //    } return null;
        //}

        public IDictionary<int, string> GetPoolNames(int programID, short timePeriodID)
        {
            return GetLookupCollection("Lookup_GetPoolNames", "Id", "Label", new List<SqlParameter> 
                { 
                    new SqlParameter { ParameterName = "@ProgramID", SqlDbType = SqlDbType.Int, Value = programID },
                    new SqlParameter { ParameterName = "@TimePeriodID", SqlDbType = SqlDbType.SmallInt, Value = timePeriodID }   
                });
        }

        #endregion

        #region GeneralInfo METHODS

        /// <summary>
        /// Get the select lists and the data to edit the "General Info" about a project
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public InfoViewModel GetProjectInfoViewModel(int versionId, string rtpYear)
        {
            var result = new InfoViewModel();

            // get project summary info
            result.InfoModel = GetProjectInfo(versionId, rtpYear);
            result.ProjectSummary = GetProjectSummary(versionId, rtpYear);
            result.ProjectSponsorsModel = GetProjectSponsorsModel(versionId, rtpYear);
            if (result.InfoModel.SponsorId.HasValue && result.InfoModel.SponsorContactId.HasValue)
            {
                result.ProjectSponsorsModel.SponsorContact = GetSponsorContact(result.InfoModel.SponsorId.Value, result.InfoModel.SponsorContactId.Value);
            }
            
            // fill collections
            result.AvailableAdminLevels = AvailableAdminLevels;// GetLookupCollection("Lookup_GetProjectAdministrativeLevels", "Id", "Label");
            //result.AvailableSponsors = AvailableSponsors;// GetLookupCollection("Lookup_GetSponsorOrganizations", "Id", "Label");
            //result.AvailableSponsors = GetAvailableSponsorAgencies(rtpYear, _appState).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
            result.AvailableImprovementTypes = AvailableImprovementTypes(24);// GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
            result.AvailableRoadOrTransitTypes = AvailableRoadOrTransitTypes; // GetLookupCollection("Lookup_GetRoadOrTransitCategories", "Id", "Label");
            //Fixed this next line. -DBD 02/03/2010
            result.AvailableSponsorContacts = AvailableSponsorContacts(result.InfoModel);// GetSponsorContacts(result.InfoModel.SponsorId.Value);
            //result.AvailableSponsorContacts =
            //    ((result.InfoModel.SponsorContactId > 0) && (result.InfoModel.SponsorId > 0)) ?
            //        GetSponsorContacts(result.InfoModel.SponsorId.Value)
            //        : new Dictionary<int, string>(); 
            result.AvailableProjectTypes = AvailableProjectTypes;// GetLookupCollection("Lookup_GetProjectTypes", "Id", "Label");
            result.AvailableSelectionAgencies = AvailableSelectionAgencies; // GetLookupCollection("Lookup_GetSelectors", "Id", "Label");
            result.AvailablePools = AvailablePools(result.ProjectSummary);// GetPoolNames(1, result.ProjectSummary.TipYearTimePeriodID); // Can get ProgramID from Session. -DBD

            
            return result;
        }

        public IList<FundingSource> GetProjectFundingSources(int projectVersionId)
        {
            var fundingSources = new List<FundingSource>();

            // Get Agencies which are eligible to sponsor projects
            SqlCommand cmd = new SqlCommand("[RTP].[GetProjectFundingSources]");
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

        public DRCOG.Domain.Models.RTP.ProjectSponsorsModel GetProjectSponsorsModel(int projectVersionID, string year)
        {
            var model = new DRCOG.Domain.Models.RTP.ProjectSponsorsModel();

            // Get Agencies which are eligible to sponsor projects
            SqlCommand cmd = new SqlCommand("[dbo].[GetCurrentProjectSponsorAgency]");
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

            cmd = new SqlCommand("[RTP].[GetAvailableProjectSponsorAgencies]");
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
            cmd.Parameters.AddWithValue("@Year", year);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    model.AvailableAgencies.Add(new SponsorOrganization()
                    {
                        OrganizationId = (int)rdr["OrganizationId"],
                        OrganizationName = rdr["OrganizationName"].ToString(),
                        //IsPrimary = bool.Parse(rdr["Primary"].ToString())
                    });
                }
            }

            return model;
        }

        public Funding GetPlanReportGroupingCategories(string year)
        {
            var model = new Funding();

            // Get Agencies which are eligible to sponsor projects
            SqlCommand cmd = new SqlCommand("[RTP].[Lookup_GetPlanReportGroupingCategories]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriod", year);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    model.ReportGroupingCategoriesDetail.Add(new PlanReportGroupingCategory()
                    {
                        CategoryId = (int)rdr["CategoryID"],
                        Category = rdr["Category"].ToString(),
                        ShortTitle = rdr["ShortTitle"].ToString(),
                        Description = rdr["Description"].ToString()
                    });
                    model.ReportGroupingCategories.Add((int)rdr["CategoryID"], rdr["Category"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// Gets a view model that can be used in the CreateProject view.
        /// Just contains the pick lists needed.
        /// </summary>
        /// <returns></returns>
        public InfoViewModel GetCreateProjectViewModel()
        {
            var result = new InfoViewModel();
            // fill collections
            result.AvailableAdminLevels = GetLookupCollection("Lookup_GetProjectAdministrativeLevels", "Id", "Label");
            result.AvailableSponsors = GetLookupCollection("Lookup_GetSponsorOrganizations", "Id", "Label");
            result.AvailableImprovementTypes = AvailableImprovementTypes(24);
            result.AvailableRoadOrTransitTypes = GetLookupCollection("Lookup_GetRoadOrTransitCategories", "Id", "Label");
            result.AvailableProjectTypes = GetLookupCollection("Lookup_GetProjectTypes", "Id", "Label");
            result.AvailableSelectionAgencies = GetLookupCollection("Lookup_GetSelectors", "Id", "Label");
            return result;
        }

        /// <summary>
        /// Update the Project Info in the database
        /// </summary>
        /// <param name="model"></param>
        public void UpdateProjectInfo(InfoModel model)
        {
            using (SqlCommand command = new SqlCommand("[RTP].[UpdateProjectVersionInfo]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTVERSIONID", model.ProjectVersionId);
                command.Parameters.AddWithValue("@TIMEPERIOD", model.RtpYear);
                command.Parameters.AddWithValue("@PROJECTID", model.ProjectId);
                command.Parameters.AddWithValue("@PROJECTNAME", model.ProjectName);
                command.Parameters.AddWithValue("@ProjectTypeID", model.ProjectTypeId);
                command.Parameters.AddWithValue("@SPONSORID", model.SponsorId != null ? (object)model.SponsorId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SPONSORCONTACTID", model.SponsorContactId != null ? (object)model.SponsorContactId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ADMINLEVELID", model.AdministrativeLevelId != null ? (object)model.AdministrativeLevelId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@IMPTYPEID", model.ImprovementTypeId != null ? (object)model.ImprovementTypeId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TRANSTYPEID", model.TransportationTypeId != null ? (object)model.TransportationTypeId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SelectionAgencyID", model.SelectionAgencyId != null ? (object)model.SelectionAgencyId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SPONSORNOTES", model.SponsorNotes);
                command.Parameters.AddWithValue("@DRCOGNOTES", model.DRCOGNotes);
                command.Parameters.AddWithValue("@RegionallySignificant", model.IsRegionallySignificant != null ? (object)model.IsRegionallySignificant.Value : (object)DBNull.Value);
                this.ExecuteNonQuery(command);
            }
        }
        
        /// <summary>
        /// Update the list of Eligible Agencies associated with a particular TIP. DEPRECATED.
        /// </summary>
        /// <param name="model"></param>
        [Obsolete("Not used",true)]
        public void UpdateCurrentSponsors(string projectVersionID, List<int> AddedOrganizations, List<int> RemovedOrganizations)
        {
            //first remove the orgs that were dropped
            foreach (int orgId in RemovedOrganizations)
            {
                SqlCommand cmd = new SqlCommand("[TIP].[DeleteProjectSponsorOrganization]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
                cmd.Parameters.AddWithValue("@SponsorID", orgId);
                this.ExecuteNonQuery(cmd);
            }

            //now add in the added orgs
            foreach (int orgId in AddedOrganizations)
            {
                SqlCommand cmd = new SqlCommand("[TIP].[InsertProjectSponsorOrganization]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
                cmd.Parameters.AddWithValue("@SponsorID", orgId);
                this.ExecuteNonQuery(cmd);
            }

        }

        /// <summary>
        /// Add a sponsor agency to the TIP Project.
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="organizationId"></param>
        public string AddAgencyToTIPProject(int projectVersionID, int organizationId, bool isPrimary)
        {
            string result = "";
            SqlCommand cmd = new SqlCommand("[TIP].[InsertProjectSponsorOrganization]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
            cmd.Parameters.AddWithValue("@SponsorID", organizationId);
            cmd.Parameters.AddWithValue("@IsPrimary", isPrimary);

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
        /// Drop a sponsor agency from the TIP Project.
        /// </summary>
        /// <param name="tipYear"></param>
        /// <param name="organizationId"></param>
        /// <returns>if false, the agency sponsors projects thus can not be removed</returns>
        public string DropAgencyFromTIP(int projectVersionID, int organizationId)
        {
            string result = "";
            SqlCommand cmd = new SqlCommand("[TIP].[DeleteProjectSponsorOrganization]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
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


#endregion

        #region Location MODELS & METHODS
        /// <summary>
        /// Fetch a Location View Model for the specified TIP Project Version
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public LocationViewModel GetProjectLocationViewModel(int versionId, string tipYear)
        {
            //throw new NotImplementedException();
            var result = new LocationViewModel();

            //Get project summary info
            result.ProjectSummary = GetProjectSummary(versionId, tipYear);
            //Get the location info
            result.RtpProjectLocation = GetProjectLocationModel(versionId, tipYear);
            result.MuniShares = GetProjectMunicipalityShares(versionId);
            result.CountyShares = GetProjectCountyShares(versionId);
            result.RtpCdotData = GetCdotData(versionId);

            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("ProjectVersionId", versionId));
            result.AvailableCounties = GetLookupCollection("Lookup_GetAvailableCountyGeographies", "Id", "Label", parameters);

            IList<SqlParameter> parameters2 = new List<SqlParameter>();
            parameters2.Add(new SqlParameter("ProjectVersionId", versionId));
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
        public LocationModel GetProjectLocationModel(int versionId, string year)
        {
            //throw new NotImplementedException();
            LocationModel result = null;
            using (SqlCommand command = new SqlCommand("[RTP].[GetProjectLocation]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTVERSIONID", versionId);
                command.Parameters.AddWithValue("@YEAR", year);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new LocationModel();
                        result.FacilityName = rdr["FacilityName"].ToString();
                        result.Limits = rdr["Limits"].ToString();
                        result.ProjectVersionId = versionId;
                        result.RtpYear = year;
                        result.ProjectName = rdr["ProjectName"].ToString();
                        result.ProjectId = rdr["ProjectId"] != DBNull.Value ? (int?)rdr["ProjectId"] : null;
                        result.RouteId = rdr["RouteID"] != DBNull.Value ? (int)rdr["RouteID"] : 0;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Get the municipality shares for a project
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IList<MunicipalityShareModel> GetProjectMunicipalityShares(int versionId)
        {
            IList<MunicipalityShareModel> result = new List<MunicipalityShareModel>();
            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectMuniShares]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTVERSIONID", versionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        MunicipalityShareModel model = new MunicipalityShareModel();
                        model.MunicipalityId = rdr["MuniGeographyId"] != DBNull.Value ? (int?)rdr["MuniGeographyId"] : null;
                        model.MunicipalityName = rdr["MuniName"].ToString();
                        model.Primary = rdr["Primary"] != DBNull.Value ? (bool?)rdr["Primary"] : null;
                        model.Share = rdr["Share"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Share"]) : null;
                        model.ProjectId = rdr["ProjectId"] != DBNull.Value ? (int?)rdr["ProjectId"] : null;
                        result.Add(model);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Get the county shares for a project
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IList<CountyShareModel> GetProjectCountyShares(int versionId)
        {
            IList<CountyShareModel> result = new List<CountyShareModel>();
            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectCountyShares]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTVERSIONID", versionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        CountyShareModel model = new CountyShareModel();
                        model.CountyId = rdr["CountyGeographyId"] != DBNull.Value ? (int?)rdr["CountyGeographyId"] : null;
                        model.CountyName = rdr["CountyName"].ToString();
                        model.Primary = rdr["Primary"] != DBNull.Value ? (bool?)rdr["Primary"] : null;
                        model.Share = rdr["Share"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Share"]) : null;
                        model.ProjectId = rdr["ProjectId"] != DBNull.Value ? (int?)rdr["ProjectId"] : null;
                        result.Add(model);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Store the Location model attributes - Facility Name and Limits ONLY
        /// </summary>
        /// <param name="model"></param>
        public void UpdateProjectLocationModel(LocationModel model)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[UpdateProjectLocation]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTVERSIONID", model.ProjectVersionId);
                command.Parameters.AddWithValue("@FACILITYNAME", model.FacilityName);
                command.Parameters.AddWithValue("@LIMITS", model.Limits);
                command.Parameters.AddWithValue("@RouteId", model.RouteId);
                
                this.ExecuteNonQuery(command);
            }
        }

        

#endregion

        #region Scope MODELS & METHODS

        public ScopeViewModel GetScopeViewModel(int projectVersionId, string planYear)
        {
            var result = new ScopeViewModel();
            result.RtpProjectScope = this.GetScopeModel(projectVersionId, planYear);
            result.ProjectSummary = GetProjectSummary(projectVersionId, planYear);
            result.Segments = GetProjectSegments(projectVersionId);
            //result.PoolProjects = GetPoolProjects(projectVersionId);

            result.AvailableNetworks = GetPlanScenariosByCycleId(result.ProjectSummary.Cycle.Id);// GetLookupCollection("dbo.Lookup_GetNetworks", "Id", "Label");
            foreach(SegmentModel sm in result.Segments)
            {
                if( !result.AvailableNetworks.ContainsKey(sm.NetworkId) )
                result.AvailableNetworks.Add(new KeyValuePair<int, string>(sm.NetworkId, sm.Network));
            }

            result.AvailableImprovementTypes = AvailableImprovementTypes(24);
            IList<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@TypeId", (int)Enums.GISCategoryType.FacilityType));
            result.AvailableFacilityTypes = GetLookupCollection("[dbo].[Lookup_GISCategoriesByType]", "Id", "Label", parms);
            
            return result;
        }

        public ScopeModel GetScopeModel(int projectVersionId, string year)
        {
            ScopeModel result = null;

            using (SqlCommand command = new SqlCommand("[RTP].[GetProjectScope]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                command.Parameters.AddWithValue("@Year", year);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new ScopeModel();
                        result.BeginConstructionYear = rdr["BeginConstructionYear"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["BeginConstructionYear"]) : null;
                        //result.OpenToPublicYear = rdr["EndConstructionYear"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["EndConstructionYear"]) : null;
                        result.ProjectDescription = rdr["ProjectDescription"].ToString();
                        result.ShortDescription = rdr["ShortDescription"].ToString();
                        result.ProjectId = rdr["ProjectId"] != DBNull.Value ? (int?)rdr["ProjectId"] : null;
                        result.ProjectName = rdr["ProjectName"].ToString();
                        result.ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? (int)rdr["ProjectVersionId"] : default(int);
                        result.RtpYear = year;
                    }
                }
            }

            return result;

        }

        //protected LRS GetProjectLRS(int projectVersionId)
        //{
        //    LRS result = new LRS();

        //    using (SqlCommand command = new SqlCommand("[RTP].[GetProjectLRS]") { CommandType = CommandType.StoredProcedure })
        //    {
        //        command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

        //        using (IDataReader rdr = ExecuteReader(command))
        //        {
        //            if (rdr.Read())
        //            {
        //                result.RouteName = rdr["Routename"].ToString();
        //                result.BeginMeasure = rdr["BEGINMEASU"] != DBNull.Value ? (double)rdr["BEGINMEASU"] : default(double);
        //                result.EndMeasure = rdr["ENDMEASURE"] != DBNull.Value ? (double)rdr["ENDMEASURE"] : default(double);
        //                result.Comments = rdr["Comments"].ToString();
        //                result.Offset = rdr["Offset"] != DBNull.Value ? (int)rdr["Offset"] : default(int);
        //            }
        //        }
        //    }

        //    return result;

        //}

        /// <summary>
        /// Update the project scope information in the database
        /// </summary>
        /// <remarks>Uses the [RTP].[UpdateProjectScope] stored proc.</remarks>
        /// <param name="model"></param>
        public void UpdateProjectScope(ScopeModel model)
        {
            using (SqlCommand command = new SqlCommand("[RTP].[UpdateProjectScope]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", model.ProjectVersionId);
                command.Parameters.AddWithValue("@ProjectDescription", model.ProjectDescription);
                command.Parameters.AddWithValue("@ShortDescription", model.ShortDescription);

                this.ExecuteNonQuery(command);
            }
        }

        public Int32 AddSegment(SegmentModel model)
        {
            int retval = 0;
            using (SqlCommand command = new SqlCommand("[RTP].[AddSegment]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", model.ProjectVersionId);
                command.Parameters.AddWithValue("@FacilityName", model.FacilityName != null ? (object)model.FacilityName.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartAt", model.StartAt != null ? (object)model.StartAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndAt", model.EndAt != null ? (object)model.EndAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@NetworkId", model.NetworkId > 0 ? model.NetworkId.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ImprovementTypeId", model.ImprovementTypeId > 0 ? model.ImprovementTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@PlanFacilityTypeId", model.PlanFacilityTypeId > 0 ? model.PlanFacilityTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ModelingFacilityTypeId", model.ModelingFacilityTypeId > 0 ? model.ModelingFacilityTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@OpenYear", model.OpenYear > 0 ? model.OpenYear : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LanesBase", model.LanesBase > 0 ? model.LanesBase : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LanesFuture", model.LanesFuture > 0 ? model.LanesFuture : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SpacesFuture", model.SpacesFuture > 0 ? model.SpacesFuture : (object)DBNull.Value);
                command.Parameters.AddWithValue("@AssignmentStatusId", model.AssignmentStatusID > 0 ? model.AssignmentStatusID : (object)DBNull.Value);

                //command.Parameters.AddWithValue("@RouteName", !String.IsNullOrEmpty(model.LRS.RouteName) ? model.LRS.RouteName : (object)DBNull.Value);
                //command.Parameters.AddWithValue("@BeginMeasure", model.LRS.BeginMeasure > 0 ? model.LRS.BeginMeasure : 0);
                //command.Parameters.AddWithValue("@EndMeasure", model.LRS.EndMeasure > 0 ? model.LRS.EndMeasure : 0);
                //command.Parameters.AddWithValue("@Comments", !String.IsNullOrEmpty(model.LRS.Comments) ? model.LRS.Comments : (object)DBNull.Value);
                //command.Parameters.AddWithValue("@Offset", model.LRS.Offset > 0 ? model.LRS.Offset : (object)DBNull.Value);
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
            using (SqlCommand command = new SqlCommand("[RTP].[DeleteSegment]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@SegmentId", segmentId);
                this.ExecuteNonQuery(command);
            }
        }

        public void UpdateSegment(SegmentModel model)
        {

            using (SqlCommand command = new SqlCommand("[RTP].[UpdateSegment]") { CommandType = CommandType.StoredProcedure })
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
                command.Parameters.AddWithValue("@SpacesFuture", model.SpacesFuture > 0 ? model.SpacesFuture : (object)DBNull.Value);
                command.Parameters.AddWithValue("@AssignmentStatusId", model.AssignmentStatusID > 0 ? model.AssignmentStatusID : (object)DBNull.Value);

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
                
                

                //command.Parameters.AddWithValue("@RouteName", !String.IsNullOrEmpty(model.LRS.RouteName) ? model.LRS.RouteName : (object)DBNull.Value);
                //command.Parameters.AddWithValue("@BeginMeasure", model.LRS.BeginMeasure > 0 ? model.LRS.BeginMeasure : 0);
                //command.Parameters.AddWithValue("@EndMeasure", model.LRS.EndMeasure > 0 ? model.LRS.EndMeasure : 0);
                //command.Parameters.AddWithValue("@Comments", !String.IsNullOrEmpty(model.LRS.Comments) ? model.LRS.Comments : (object)DBNull.Value);
                //command.Parameters.AddWithValue("@Offset", model.LRS.Offset > 0 ? model.LRS.Offset : (object)DBNull.Value);
                this.ExecuteNonQuery(command);
            }
        }

        public void UpdateSegmentSummary(SegmentModel model)
        {
            using (SqlCommand command = new SqlCommand("[RTP].[UpdateSegmentSummary]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@SegmentId", model.SegmentId);
                command.Parameters.AddWithValue("@FacilityName", model.FacilityName != null ? (object)model.FacilityName.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartAt", model.StartAt != null ? (object)model.StartAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndAt", model.EndAt != null ? (object)model.EndAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@NetworkId", model.NetworkId > 0 ? model.NetworkId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@OpenYear", model.OpenYear > 0 ? model.OpenYear : (object)DBNull.Value);
                this.ExecuteNonQuery(command);
            }
        }

        public void AddLRSRecord(SegmentModel model)
        {

            using (SqlCommand command = new SqlCommand("[dbo].[AddLRSRecord]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@SegmentId", model.SegmentId);

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

        public void DeleteLRSRecord(int lrsId)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[DeleteLRSRecord]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@LrsId", lrsId);
                this.ExecuteNonQuery(command);
            }
        }

        public void UpdateLRSRecord(SegmentModel model)
        {

            using (SqlCommand command = new SqlCommand("[dbo].[UpdateLRSRecord]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@LRSId", model.LRSRecord.Id);
                command.Parameters.AddWithValue("@SegmentId", model.SegmentId);

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

        public SegmentModel GetSegmentDetails(int segmentId)
        {
            DataTable data;
            using (SqlCommand command = new SqlCommand("[RTP].[GetSegment]") { CommandType = CommandType.StoredProcedure })
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
                ModelingFacilityTypeId = row["ModelingFacilityTypeID"].ToString().SmartParse<int>()
                ,
                OpenYear = row["OpenYear"].ToString().SmartParse<short>()
                ,
                LanesBase = row["LanesBase"].ToString().SmartParse<short>()
                ,
                LanesFuture = row["LanesFuture"].ToString().SmartParse<short>()
                ,
                SpacesFuture = row["SpacesFuture"].ToString().SmartParse<short>()
                ,
                AssignmentStatusID = row["AssignmentStatusID"].ToString().SmartParse<int?>()
                //,
                //LRS = new LRS()
                //{
                //    RouteName = row["Routename"].ToString()
                //    ,
                //    BeginMeasure = row["BeginMeasure"].ToString().SmartParse<double>()
                //    ,
                //    EndMeasure = row["EndMeasure"].ToString().SmartParse<double>()
                //    ,
                //    Comments = row["Comments"].ToString()
                //    ,
                //    Offset = row["Offset"].ToString().SmartParse<int>()
                //}
            };
            return model;
        }

        public SegmentViewModel GetSegmentViewModel(int projectVersionID)
        {
            SegmentViewModel result = new SegmentViewModel();

            //result.ProjectSummary = GetProjectSummary(versionId, tipYear); //This would require me to cross Repository boundary.
            result.ProjectVersionID = projectVersionID;
            result.Segments = GetProjectSegments(projectVersionID);

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
                        Staging = rdr["Staging"].ToString()
                    ,   
                        OpenYear = rdr["OpenYear"] != DBNull.Value ? (short)rdr["OpenYear"] : (short)0
                    ,   
                        FacilityName = rdr["FacilityName"] != DBNull.Value ? rdr["FacilityName"].ToString() : ""
                    ,   
                        StartAt = rdr["StartAt"] != DBNull.Value ? rdr["StartAt"].ToString() : ""
                    ,   
                        EndAt = rdr["EndAt"] != DBNull.Value ? rdr["EndAt"].ToString() : ""
                    ,   
                        Length = rdr["Length"] != DBNull.Value ? (decimal?)rdr["Length"] : null
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

        public IList<PoolProject> GetPoolProjects(int projectVersionID)
        {
            IList<PoolProject> poolList = new List<PoolProject>();

            SqlCommand cmd = new SqlCommand("[dbo].[GetProjectVersionPoolProjects]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);

            DataTable dt = this.ExecuteDataTable(cmd);
            foreach(DataRow dr in dt.Rows)
            {

                poolList.Add(new PoolProject()
                {
                    PoolProjectID = (int)dr["PoolProjectID"]
                    , ProjectName = dr["ProjectName"].ToString()
                    , Description = dr["Description"].ToString()
                    , BeginAt = dr["BeginAt"].ToString()
                    , EndAt = dr["EndAt"].ToString()
                    , Cost = (decimal)dr["Cost"]
                });
            }
            return poolList;
        }

#endregion

        #region Funding MODELS & METHODS

        public FundingViewModel GetFundingViewModel(int projectVersionId, string plan)
        {
            var result = new FundingViewModel();
            result.ProjectSummary = GetProjectSummary(projectVersionId, plan);
            result.ProjectFunding = GetFunding(projectVersionId, plan);
            result.PlanTypes = GetLookupCollection("[RTP].[Lookup_GetPlanTypes]", "CategoryID", "Category");
            result.AvailableFundingResources = AvailableFundingResources(Enums.ApplicationState.RTP);

            result.FundingSources = GetProjectFundingSources(projectVersionId);

            //result.ProjectFundingHistory = GetFundingHistory(projectVersionId);

            return result;
        }

        public Funding GetFunding(int projectVersionId, string plan)
        {
            Funding funding = null;
            using (SqlCommand command = new SqlCommand("[RTP].[GetProjectFunding]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        funding = new Funding()
                        {
                            ProjectVersionId = projectVersionId
                        ,
                            ConstantCost = rdr["ConstantCost"] != DBNull.Value ? (Decimal)Convert.ToDouble(rdr["ConstantCost"]) : (Decimal)0.00
                        ,
                            VisionCost = rdr["VisionCost"] != DBNull.Value ? (Decimal)Convert.ToDouble(rdr["VisionCost"]) : (Decimal)0.00
                        ,
                            YOECost = rdr["YOECost"] != DBNull.Value ? (Decimal)Convert.ToDouble(rdr["YOECost"]) : (Decimal)0.00
                        ,
                            Previous = rdr["Previous"] != DBNull.Value ? (Decimal)Convert.ToDouble(rdr["Previous"]) : (Decimal)0.00
                        ,
                            Future = rdr["Future"] != DBNull.Value ? (Decimal)Convert.ToDouble(rdr["Future"]) : (Decimal)0.00
                        ,
                            TotalCost = rdr["TotalCost"] != DBNull.Value ? (Decimal)Convert.ToDouble(rdr["TotalCost"]) : (Decimal)0.00
                        ,
                            PlanTypeId = rdr["PlanTypeId"] != DBNull.Value ? (int)rdr["PlanTypeId"] : Int32.MinValue
                        ,
                            ReportGroupingCategoryId = rdr["RTPCategoryID"] != DBNull.Value ? (int)rdr["RTPCategoryID"] : Int32.MinValue
                        };
                    }
                }
            }
            Funding categories = GetPlanReportGroupingCategories(plan);
            funding.ReportGroupingCategories = categories.ReportGroupingCategories;
            funding.ReportGroupingCategoriesDetail = categories.ReportGroupingCategoriesDetail;

            return funding;
        }

        

        public PlanReportGroupingCategory GetPlanReportGroupingCategoryDetails(int categoryId)
        {
            var result = new PlanReportGroupingCategory();

            SqlCommand cmd = new SqlCommand("[RTP].[Lookup_GetPlanReportGroupingCategoryDetails]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    result.ShortTitle = rdr["ShortTitle"] != DBNull.Value ? rdr["ShortTitle"].ToString() : String.Empty;
                    result.Description = rdr["Description"] != DBNull.Value ? rdr["Description"].ToString() : String.Empty;
                }
            }

            return result;
        }

        public IList<FundingModel> GetFundingHistory(int projectVersionID)
        {
            IList<FundingModel> fundingList = new List<FundingModel>();

            using (SqlCommand command = new SqlCommand("TIP.GetProjectFundingHistory") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
                //command.Parameters.AddWithValue("@TipYear", tipYear);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        fundingList.Add(new FundingModel()
                        {
                            ProjectFinancialRecordID = rdr["ProjectFinancialRecordID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["ProjectFinancialRecordID"]) : null
                        ,
                            TimePeriodID = rdr["TimePeriodID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["TimePeriodID"]) : null
                        ,
                            TimePeriod = rdr["TimePeriod"].ToString()
                        ,
                            Previous = rdr["Previous"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Previous"]) : (double)0.00
                        ,
                            Future = rdr["Future"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Future"]) : (double)0.00
                        ,
                            Funding = rdr["TIPFunding"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["TIPFunding"]) : (double)0.00
                        ,
                            FederalTotal = rdr["FederalTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["FederalTotal"]) : (double)0.00
                        ,
                            StateTotal = rdr["StateTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["StateTotal"]) : (double)0.00
                        ,
                            LocalTotal = rdr["LocalTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["LocalTotal"]) : (double)0.00
                        ,
                            TotalCost = rdr["TotalCost"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["TotalCost"]) : (double)0.00
                        });
                    }
                }
            }

            return fundingList;
        }

        [Obsolete("SP not created", true)]
        public IList<FundingDetailModel> GetFundingDetail(int projectVersionID)
        {
            IList<FundingDetailModel> fundingDetailList = new List<FundingDetailModel>();

            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectFundingDetail]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        fundingDetailList.Add(new FundingDetailModel()
                        {
                            ProjectFinancialRecordID = rdr["ProjectFinancialRecordID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["ProjectFinancialRecordID"]) : null
                        //,   FundingIncrementID = rdr["FundingIncrementID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["FundingIncrementID"]) : null
                        //,   FundingIncrement = rdr["FundingIncrement"] !=DBNull.Value ? rdr["FundingIncrement"].ToString() : ""
                        ,   FundingTypeID = rdr["FundingTypeID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["FundingTypeID"]) : null
                        ,   FundingType = rdr["FundingType"] !=DBNull.Value ? rdr["FundingType"].ToString() : ""
                        ,   FederalAmount = rdr["FederalAmount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["FederalAmount"]) : (double)0.00
                        ,   StateAmount = rdr["StateAmount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["StateAmount"]) : (double)0.00
                        ,   LocalAmount = rdr["LocalAmount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["LocalAmount"]) : (double)0.00
                        //,   Amount = rdr["Amount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Amount"]) : (double)0.00
                        //,   StateWideAmount = rdr["StateWideAmount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["StateWideAmount"]) : (double)0.00
                        });
                    }
                }
            }
            return fundingDetailList;
        }

        public FundingDetailPivotModel GetFundingDetailPivot(int projectVersionID)
        {
            FundingDetailPivotModel fundingDetailPivot = new FundingDetailPivotModel();
            fundingDetailPivot.ProjectVersionId = projectVersionID;
            //DataTable fundingDetailTable = new DataTable("FundingDetail");
            fundingDetailPivot.FundingDetailTable = new DataTable("FundingDetail");

            using (SqlCommand command = new SqlCommand("TIP.GetProjectFundingDetailPivot") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);

                fundingDetailPivot.FundingDetailTable = ExecuteDataTable(command);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        fundingDetailPivot.FundingDetails.Add(new FundingDetailModel()
                        {
                            ProjectFinancialRecordID = rdr["ProjectFinancialRecordID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["ProjectFinancialRecordID"]) : null
                        ,
                            FundingTypeID = rdr["FundingTypeID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["FundingTypeID"]) : null
                        ,
                            FundingType = rdr["FundingType"] != DBNull.Value ? rdr["FundingType"].ToString() : ""
                        ,
                            FundingLevelID = rdr["FundingLevelOrder"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["FundingLevelOrder"]) : null //Using this temporarily. -DBD
                        ,
                            FundingLevel = rdr["FundingLevel"] != DBNull.Value ? rdr["FundingLevel"].ToString() : ""
                        ,
                            FundingLevelOrder = rdr["FundingLevelOrder"] != DBNull.Value ? (int)Convert.ToInt32(rdr["FundingLevelOrder"]) : 10 // Put it at the end. -DBD
                        ,
                            FederalAmount = rdr["FederalAmount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["FederalAmount"]) : (double)0.00
                        ,
                            StateAmount = rdr["StateAmount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["StateAmount"]) : (double)0.00
                        ,
                            LocalAmount = rdr["LocalAmount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["LocalAmount"]) : (double)0.00
                            //,   Amount = rdr["Amount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Amount"]) : (double)0.00
                            //,   StateWideAmount = rdr["StateWideAmount"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["StateWideAmount"]) : (double)0.00
                        });
                    }
                }
            }

            using (SqlCommand command = new SqlCommand("[TIP].[GetFundingIncrements]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    fundingDetailPivot.FundingIncrements = new List<FundingIncrement>();
                    while (rdr.Read())
                    {
                        fundingDetailPivot.FundingIncrements.Add(new FundingIncrement() 
                        { 
                            FundingIncrementID = rdr["FundingIncrementID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["FundingIncrementID"]) : null
                        ,   FundingIncrementName = rdr["FundingIncrement"] != DBNull.Value ? rdr["FundingIncrement"].ToString() : ""
                        });
                    }
                }
            }

            return fundingDetailPivot;
        }

        public IList<FundingIncrement> GetFundingIncrement(int projectVersionID)
        {
            IList<FundingIncrement> fundingIncrementList = new List<FundingIncrement>();

            using (SqlCommand command = new SqlCommand("[TIP].[GetFundingIncrements]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        fundingIncrementList.Add(new FundingIncrement()
                        {
                            FundingIncrementID = rdr["FundingIncrementID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["FundingIncrementID"]) : null
                        ,   FundingIncrementName = rdr["FundingIncrement"].ToString()
                        });
                    }
                }
            }

            return fundingIncrementList;
        }

        [Obsolete("SP not created",true)]
        public Int32 AddFinancialRecord(FundingModel model)
        {
            int retval = 0;
            using (SqlCommand command = new SqlCommand("TIP_AddProjectFinancialRecord") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@Previous", model.Previous != null ? (object)model.Previous.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Future", model.Future != null ? (object)model.Future.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TIPFunding", model.Funding != null ? (object)model.Funding.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@FederalTotal", model.FederalTotal != null ? (object)model.FederalTotal.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StateTotal", model.StateTotal != null ? (object)model.StateTotal.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LocalTotal", model.LocalTotal != null ? (object)model.LocalTotal.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TotalCost", model.TotalCost != null ? (object)model.TotalCost.Value : (object)DBNull.Value);

                SqlParameter outParam = new SqlParameter("@ProjectFinancialRecordID", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(outParam);

                this.ExecuteNonQuery(command);
                retval = (int)command.Parameters["@ProjectFinancialRecordID"].Value;
            }
            return retval;
        }

        public void UpdateFinancialRecord(Funding model)
        {
            using (SqlCommand command = new SqlCommand("[RTP].[UpdateFunding]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", model.ProjectVersionId);
                command.Parameters.AddWithValue("@ConstantCost", model.ConstantCost > 0 ? (object)model.ConstantCost : (object)DBNull.Value);
                command.Parameters.AddWithValue("@VisionCost", model.VisionCost > 0 ? (object)model.VisionCost : (object)DBNull.Value);
                command.Parameters.AddWithValue("@YOECost", model.YOECost > 0 ? (object)model.YOECost : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Future", model.Future > 0 ? (object)model.Future : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Previous", model.Previous > 0 ? (object)model.Previous : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TotalCost", model.TotalCost > 0 ? (object)model.TotalCost : (object)DBNull.Value);
                command.Parameters.AddWithValue("@PlanTypeId", model.PlanTypeId > 0 ? (object)model.PlanTypeId : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ReportGroupingCategoryId", model.ReportGroupingCategoryId > 0 ? (object)model.ReportGroupingCategoryId : (object)DBNull.Value);
                this.ExecuteNonQuery(command);
            }
        }

        [Obsolete("SP does not exist",true)]
        public void DeleteFinancialRecord(int projectFinancialRecordId)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[DeleteProjectFinancialRecord]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectFinancialRecordID", projectFinancialRecordId);
                this.ExecuteNonQuery(command);
            }
        }

        public void AddFinancialRecordDetail(int projectVersionID, int fundingPeriodID, int fundingTypeID)
        {
            using (SqlCommand command = new SqlCommand("TIP.AddProjectFinancialRecordDetail") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
                command.Parameters.AddWithValue("@fundingPeriodID", fundingPeriodID);
                command.Parameters.AddWithValue("@fundingTypeID", fundingTypeID);
                this.ExecuteNonQuery(command);
            }
        }

        public void UpdateFinancialRecordDetail(ProjectFinancialRecordDetail model)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectFinancialRecordDetail]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@projectFinancialRecordID", model.ProjectFinancialRecordID);
                command.Parameters.AddWithValue("@fundingLevelID", model.FundingLevelID);
                command.Parameters.AddWithValue("@fundingPeriodID", model.FundingPeriodID);
                command.Parameters.AddWithValue("@fundingTypeID", model.FundingTypeID);
                command.Parameters.AddWithValue("@incr01", model.Incr01 != null ? (object)model.Incr01 : (object)DBNull.Value);
                command.Parameters.AddWithValue("@incr02", model.Incr02 != null ? (object)model.Incr02 : (object)DBNull.Value);
                command.Parameters.AddWithValue("@incr03", model.Incr03 != null ? (object)model.Incr03 : (object)DBNull.Value);
                command.Parameters.AddWithValue("@incr04", model.Incr04 != null ? (object)model.Incr04 : (object)DBNull.Value);
                command.Parameters.AddWithValue("@incr05", model.Incr05 != null ? (object)model.Incr05 : (object)DBNull.Value);
                this.ExecuteNonQuery(command);
            }
        }

        public void DeleteFinancialRecordDetail(ProjectFinancialRecordDetail model)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[DeleteProjectFinancialRecordDetail]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@projectFinancialRecordID", model.ProjectFinancialRecordID);
                command.Parameters.AddWithValue("@fundingLevelID", model.FundingLevelID);
                command.Parameters.AddWithValue("@fundingPeriodID", model.FundingPeriodID);
                command.Parameters.AddWithValue("@fundingTypeID", model.FundingTypeID);
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

        public void UpdateFundingSource(FundingSource model, int projectVersionId)
        {

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

        #region Amendment MODELS & METHODS

        public AmendmentsViewModel GetAmendmentsViewModel(int projectVersionId, string tipYear)
        {
            var result = new AmendmentsViewModel();

            // get project summary info
            result.InfoModel = GetProjectInfo(projectVersionId, tipYear);
            result.RtpSummary = GetProjectSummary(projectVersionId, tipYear);

            RTPSearchModel search = new RTPSearchModel()
            {
                ProjectId = (Int32)result.RtpSummary.ProjectId
            };
            result.AmendmentList = GetProjectAmendments(search);

            return result;
        }

        /// <summary>
        /// Update a project's Amendment Status
        /// </summary>
        /// <param name="model"></param>
        public void UpdateProjectAmendmentStatus(CycleAmendment model)
        {
            using (SqlCommand command = new SqlCommand("[RTP].[UpdateProjectAmendmentStatus]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", model.ProjectVersionId);
                command.Parameters.AddWithValue("@AmendmentStatusId", model.AmendmentStatusId);
                command.Parameters.AddWithValue("@VersionStatusId", model.VersionStatusId);
                this.ExecuteNonQuery(command);
            }
        }

        public void UpdateProjectVersionStatusId(Int32 projectVersionId, Int32 versionStatusId)
        {
            using (SqlCommand command = new SqlCommand("[RTP].[UpdateProjectVersionStatusId]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                command.Parameters.AddWithValue("@VersionStatusId", versionStatusId);
                this.ExecuteNonQuery(command);
            }
        }

        public Int32 GetProjectAmendmentStatus(Int32 projectVersionId)
        {
            int rval = 0;
            using (SqlCommand command = new SqlCommand("[RTP].[Lookup_GetProjectVersionStatus]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                rval = this.ExecuteScalar<Int32>(command);
            }
            return rval;
        }

        public Int32 GetProjectMostRecentAmendment(Int32 projectVersionId)
        {
            int rval = 0;
            using (SqlCommand command = new SqlCommand("[TIP].[GetMostRecentAmendmentProjectVersionId]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                rval = this.ExecuteScalar<Int32>(command);
            }
            return rval;
        }

#endregion

        #region Report METHODS

        public IDictionary<string,string> GetProjectGeneralInfo(int versionId)
        {
            IDictionary<string, string> result = null;

            using (SqlCommand command = new SqlCommand("[RTP].[GetProjectDetails]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", versionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new Dictionary<string,string>();

                        result.Add("ProjectName", rdr["ProjectName"].ToString());
                        result.Add("ProjectStatus", rdr["ProjectStatus"].ToString());
                        result.Add("AmendmentStatus", rdr["AmendmentStatus"].ToString());
                        result.Add("PrimarySponsor", rdr["PrimarySponsor"].ToString());
                        result.Add("SponsorContact", rdr["SponsorContact"].ToString());
                        result.Add("AdminLevel", rdr["AdminLevel"].ToString());
                        result.Add("ProjectType", rdr["ProjectType"].ToString());
                        result.Add("ImprovementType", rdr["ImprovementType"].ToString());
                        result.Add("RoadOrTransit", rdr["RoadOrTransit"].ToString());
                        result.Add("PoolName", rdr["PoolName"].ToString());
                        result.Add("SelectionAgency", rdr["SelectionAgency"].ToString());
                        result.Add("RegionalSignificance", rdr["RegionalSignificance"].ToString());
                        result.Add("SponsorNotes", rdr["SponsorNotes"].ToString());
                        result.Add("DRCOGNotes", rdr["DRCOGNotes"].ToString());
                        result.Add("FacilityName", rdr["FacilityName"].ToString());
                        result.Add("Limits", rdr["Limits"].ToString());
                        result.Add("Scope", rdr["Scope"].ToString());
                        result.Add("BeginConstructionYear", rdr["BeginConstructionYear"].ToString());
                        result.Add("ConstantCost", rdr["ConstantCost"].ToString());
                        result.Add("ShortDescription", rdr["ShortDescription"].ToString());
                        //result.Add("EndConstructionYear", rdr["EndConstructionYear"].ToString());
                    }
                }
            }
            return result;
        }

        public DetailViewModel GetDetailViewModel(Int32 projectVersionId, String year)
        {
            //throw new NotImplementedException();
            
            var result = new DetailViewModel();
            result.StringValues = new Dictionary<string, string>();
            result.GeneralInfo = GetProjectGeneralInfo(projectVersionId);

            InfoViewModel InfoViewModel = GetProjectInfoViewModel(projectVersionId, year);
            SegmentViewModel SegmentViewModel = GetSegmentViewModel(projectVersionId);

            

            // get project summary info
            result.ProjectSummary = GetProjectSummary(projectVersionId, year);
            result.InfoModel = InfoViewModel.InfoModel;
            result.ProjectSponsorsModel = InfoViewModel.ProjectSponsorsModel;

            result.GroupingCategory = GetPlanReportGroupingCategoryDetails(result.ProjectSummary.CategoryId);
            
            //result.StringValues.Add("AdminLevel",GetValueByKey(AvailableAdminLevels, result.InfoModel.AdministrativeLevelId.Value));
            //result.StringValues.Add("ProjectType", GetValueByKey(AvailableProjectTypes, result.InfoModel.ProjectTypeId.Value));
            //result.StringValues.Add("ImprovementType", GetValueByKey(AvailableImprovementTypes, result.InfoModel.ImprovementTypeId.Value));
            //result.StringValues.Add("RoadOrTransit", GetValueByKey(AvailableRoadOrTransitTypes, result.InfoModel.TransportationTypeId.Value));
            //result.StringValues.Add("PoolName", GetValueByKey(AvailablePools(result.ProjectSummary), result.InfoModel.ProjectPoolId.Value));
            result.Segments = GetProjectSegments(projectVersionId);
            //result.PoolProjects = GetPoolProjects(projectVersionId);

            result.FundingSources = GetProjectFundingSources(projectVersionId);

            return result;
        }

        //public ProjectDetailViewModel GetProjectDetailViewModel(int projectVersionId, string tipYear)
        //{
        //    var result = new ProjectDetailViewModel();

        //    // get project summary info
        //    result.ProjectSummary = GetProjectSummary(projectVersionId, tipYear);

        //    return result;
        //}

#endregion

        protected RtpCdotData GetCdotData(int projectVersionId)
        {
            RtpCdotData rtpCdotData = rtpCdotData = new RtpCdotData();

            SqlCommand cmd = new SqlCommand("[dbo].[GetCDOTData]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

            DataTable dt = this.ExecuteDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                rtpCdotData.Region = row["Region"] != DBNull.Value ? (short)row["Region"] : (short)0;
                rtpCdotData.CommDistrict = row["CommDistrict"] != DBNull.Value ? (short)row["CommDistrict"] : (short)0;
                rtpCdotData.RouteSegment = row["RouteSegment"].ToString();
                rtpCdotData.BeginMilePost = row["BeginMilePost"] != DBNull.Value ? (double)row["BeginMilePost"] : 0.0;
                rtpCdotData.EndMilePost = row["EndMilePost"] != DBNull.Value ? (double)row["EndMilePost"] : 0.0;
                rtpCdotData.STIPID = row["STIPID"].ToString();
                rtpCdotData.STIPProjectDivision = row["STIPProjectDivision"].ToString();
                rtpCdotData.CDOTProjectManager = row["CDOTProjectManager"] != DBNull.Value ? (int)row["CDOTProjectManager"] : 0;
                rtpCdotData.TPRAbbr = row["TPRAbbr"].ToString();
                rtpCdotData.TPRID = row["TPRID"] != DBNull.Value ? (short)row["TPRID"] : (short)0;
                rtpCdotData.LRPNumber = row["LRPNumber"] != DBNull.Value ? (int)row["LRPNumber"] : 0;
                rtpCdotData.InvestmentCategory = row["InvestmentCategory"].ToString();
                rtpCdotData.CorridorID = row["CorridorID"].ToString();
                rtpCdotData.CDOTProjectNumber = row["CDOTProjectNumber"].ToString();
                rtpCdotData.SubAccount = row["SubAccount"] != DBNull.Value ? (int)row["SubAccount"] : 0;
                rtpCdotData.ConstructionRE = row["ConstructionRE"] != DBNull.Value ? (int)row["ConstructionRE"] : 0;
                rtpCdotData.CMSNumber = row["CMSNumber"].ToString();
                rtpCdotData.ScheduledADDate = row["ScheduledADDate"] != DBNull.Value ? (DateTime)row["ScheduledADDate"] : DateTime.MinValue;
                rtpCdotData.ProjectStage = row["ProjectStage"] != DBNull.Value ? (int)row["ProjectStage"] : 0;
                rtpCdotData.ProjectStageDate = row["ProjectStageDate"] != DBNull.Value ? (DateTime)row["ProjectStageDate"] : DateTime.MinValue;
                rtpCdotData.ConstructionDate = row["ConstructionDate"] != DBNull.Value ? (DateTime)row["ConstructionDate"] : DateTime.MinValue;
                rtpCdotData.ProjectClosed = row["ProjectClosed"] != DBNull.Value ? (DateTime)row["ProjectClosed"] : DateTime.MinValue;
                rtpCdotData.Notes = row["Notes"].ToString();
            }

            return rtpCdotData;
        }

        // need to be implemented
        public ProjectCdotDataViewModel GetCDOTDataViewModel(int projectVersionId, string year)
        {
            var result = new ProjectCdotDataViewModel();

            // get project summary info
            result.ProjectSummary = GetProjectSummary(projectVersionId, year);
            result.RtpCdotData = GetCdotData(projectVersionId);
            return result;
        }

        //public StrikesViewModel GetStrikesViewModel(int projectVersionId, string tipYear)
        //{
        //    var result = new StrikesViewModel();

        //    // get project summary info
        //    result.ProjectSummary = GetProjectSummary(projectVersionId, tipYear);

        //    return result;
        //}



        

    }
}
