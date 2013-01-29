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
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain.ViewModels.TIPProject;
using System.Collections;
using DRCOG.Domain;
using DRCOG.Domain.Helpers;
using DRCOG.Common.Services.Interfaces;
using DRCOG.Common.Util;
using System.Web.Mvc;

namespace DRCOG.Data
{
    public class ProjectRepository : TipRepository, IProjectRepository
    {
        private readonly IFileRepositoryExtender FileRepository;

        public ProjectRepository(IFileRepositoryExtender fileRepository)
            : base(fileRepository)
        {
            _appState = Enums.ApplicationState.TIP;
            FileRepository = fileRepository;
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
        protected IDictionary<int,string> AvailableImprovementTypes
        {
            get
            {
                return GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
            }
        }
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
        protected IDictionary<int,string> AvailablePools(TipSummary summary)
        {
                return GetPoolNames(1, summary.TipYearTimePeriodID);
        }

        protected IDictionary<int, string> FundingTypes(int timePeriodId)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@TimePeriodId", timePeriodId));
            return GetLookupCollection("[dbo].[GetFundingTypes]", "FundingTypeID", "FundingType",parms);
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

        public int GetActiveProjectVersion(string tipId)
        {
            return GetActiveProjectVersion(tipId, string.Empty, default(int));
        }

        public int GetActiveProjectVersion(string tipId, string year)
        {
            return GetActiveProjectVersion(tipId, year, default(int));
        }

        public int GetActiveProjectVersion(string tipId, int yearId)
        {
            return GetActiveProjectVersion(tipId, string.Empty, yearId);
        }

        public int GetActiveProjectVersion(string tipId, string year, int yearId)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[GetProject]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@TipId", tipId);
                if(!String.IsNullOrEmpty(year))
                    command.Parameters.AddWithValue("@TimePeriod", year);
                if(yearId != default(int))
                    command.Parameters.AddWithValue("@TimePeriodId", yearId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        return rdr["ProjectVersionID"] != DBNull.Value ? (int)rdr["ProjectVersionID"] : default(int);
                    }
                }
                return default(int);
            }
        }

        /// <summary>
        /// Get a TipProjectVersionInfoModel. This is the model behind the
        /// Info Tab
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public InfoModel GetProjectInfo(int versionId, string tipYear)
        {
            InfoModel result = null;
            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectVersionInfo]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@TipYear", tipYear);
                command.Parameters.AddWithValue("@TipProjectVersion", versionId);
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
                        result.ProjectVersionId = rdr["ProjectVersionID"] != DBNull.Value ? (int?)rdr["ProjectVersionID"] : null;
                        result.SponsorContactId = rdr["SponsorContactID"] != DBNull.Value ? (int?)rdr["SponsorContactID"] : null;
                        result.SponsorId = rdr["SponsorID"] != DBNull.Value ? (int?)rdr["SponsorID"] : null;
                        result.SponsorNotes = rdr["SponsorNotes"].ToString();
                        result.TipId = rdr["TipId"].ToString();
                        result.TipYear = rdr["TipYear"].ToString();
                        result.TransportationTypeId = rdr["TransportationTypeID"] != DBNull.Value ? (int?)rdr["TransportationTypeID"] : null;
                        result.SelectionAgencyId = rdr["SelectionAgencyID"] != DBNull.Value ? (int?)rdr["SelectionAgencyID"] : null;
                        result.ProjectPoolId = rdr["ProjectPoolId"] != DBNull.Value ? (int?)rdr["ProjectPoolId"] : null;
                        result.IsRegionallySignificant = rdr["RegionalSignificance"] != DBNull.Value ? (bool)rdr["RegionalSignificance"] : false;
                        result.LocationMapId = rdr["LocationMapID"].ToString().SmartParseDefault<int>(default(int));
                        result.STIPID = rdr["STIPId"].ToString();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Get a list of the projects in the specified TIP Year
        /// </summary>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public IList<ProjectAmendments> GetProjectAmendments(TIPSearchModel projectSearchModel)
        {
            IList<ProjectAmendments> list = new List<ProjectAmendments>();

            SqlCommand cmd = new SqlCommand("[TIP].[GetProjectAmendmentList]");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProjectId", projectSearchModel.ProjectId);
            cmd.Parameters.AddWithValue("@TimePeriodId", projectSearchModel.TipYearID);


            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new ProjectAmendments()
                    {
                        ProjectVersionId = (int)rdr["ProjectVersionId"]
                     ,  AmendmentStatus = rdr["AmendmentStatus"] != DBNull.Value ? rdr["AmendmentStatus"].ToString() : ""
                     ,  AmendmentStatusId = rdr["AmendmentStatusID"].ToString().SmartParseDefault<int>(default(int))
                     ,  AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? (DateTime)rdr["AmendmentDate"] : DateTime.MinValue
                     ,  ProjectName = rdr["ProjectName"] != DBNull.Value ? rdr["ProjectName"].ToString() : ""
                     ,  AmendmentCharacter = rdr["AmendmentCharacter"] != DBNull.Value ? rdr["AmendmentCharacter"].ToString() : ""
                     ,  AmendmentReason = rdr["AmendmentReason"].ToString()
                     ,  VersionStatus = rdr["VersionStatus"] != DBNull.Value ? rdr["VersionStatus"].ToString() : ""
                     ,  Year = rdr["TimePeriod"].ToString()
                    });
                }
            }

            // These are processed via the SQL retrieval code optimization, so only process this one when exclude is checked.
            if ((projectSearchModel.TipYear != null) && (projectSearchModel.Exclude_TipYear))
            {
                list = (from fli in list
                        where (fli.Year != projectSearchModel.TipYear)
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
        /// Gets a human readable summary for the TIP Project Version
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <remarks>
        /// This is used in the project general information block on the right of
        /// the project tabs</remarks>
        /// <returns></returns>
        public TipSummary GetProjectSummary(int versionId)
        {
            TipSummary result = null;
            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectVersionInfoSummary]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@TipProjectVersion", versionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new TipSummary();
                        result.ProjectType = rdr["ImprovementType"].ToString();
                        result.ProjectName = rdr["ProjectName"].ToString();
                        result.COGID = rdr["COGID"].ToString();
                        result.ProjectId = (int)rdr["ProjectId"];

                        //To be editable, a ProjectVersion must be both Current and in the Current TIP
                        // IsVersionCurrent = Active
                        //bool IsVersionCurrent = rdr["IsVersionCurrent"] != DBNull.Value ? (bool)rdr["IsVersionCurrent"] : false;
                        result.IsActive = rdr["IsVersionCurrent"] != DBNull.Value ? (bool)rdr["IsVersionCurrent"] : false;
                        result.IsPending = rdr["IsTipPending"] != DBNull.Value ? (bool)rdr["IsTipPending"] : false;
                        result.IsCurrent = rdr["IsTipCurrent"] != DBNull.Value ? (bool)rdr["IsTipCurrent"] : false;
                        result.IsTopStatus = rdr["IsTopStatus"] != DBNull.Value ? (bool)rdr["IsTopStatus"] : false;
                        int amendmentStatusId = (int)rdr["AmendmentStatusId"];
                        result.AmendmentStatusId = amendmentStatusId;
                        //(Int32)TIPAmendmentStatus.Amended


                        //result.IsEditable = result.IsCurrent; //result.IsTopStatus && 
                            //&& (new int[] { (int)TIPAmendmentStatus.Proposed, (int)TIPAmendmentStatus.Submitted }.Contains(amendmentStatusId));
                        
                        result.AmendmentStatus = rdr["AmendmentStatus"].ToString();

                        result.ProjectVersionId = rdr["ProjectVersionID"] != DBNull.Value ? (int)rdr["ProjectVersionID"] : default(int);
                        result.PreviousVersionId = rdr["PreviousVersionID"].ToString().SmartParseDefault<int>(default(int));
                        result.NextVersionId = rdr["NextVersionID"] != DBNull.Value ? (int?)rdr["NextVersionID"] : null;
                        result.SponsorAgency = rdr["Sponsor"].ToString();
                        result.TipId = rdr["TipId"].ToString();
                        result.TipYear = rdr["TipYear"].ToString();
                        result.NextVersionYear = rdr["NextVersionTipYear"].ToString();
                        result.PreviousVersionYear = rdr["PreviousVersionTipYear"].ToString();
                        result.TipYearTimePeriodID = rdr["TipYearTimePeriodID"] != DBNull.Value ? (short)rdr["TipYearTimePeriodID"] : (short)0;
                        result.ProjectFinancialRecordId = rdr["ProjectFinancialRecordId"].ToString().SmartParseDefault<int>(default(int));
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
        public int CreateProject(string projectName, string facilityName, string tipYear, int sponsorOrganizationId, int amendmentTypeId)
        {
            SqlCommand cmd = new SqlCommand("TIP.CreateProject");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PROJECTNAME", projectName);
            cmd.Parameters.AddWithValue("@SponsorOrganizationId", sponsorOrganizationId);
            cmd.Parameters.AddWithValue("@AmendmentTypeId", amendmentTypeId);
            cmd.Parameters.AddWithValue("@FacilityName", facilityName);
            cmd.Parameters.AddWithValue("@TIPYEAR", tipYear);
            SqlParameter outParam = new SqlParameter("@PROJECTVERSIONID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outParam);
            SqlConnection con = new SqlConnection(this.ConnectionString);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            //return the value
            return (int)cmd.Parameters["@PROJECTVERSIONID"].Value;
        }

        /// <summary>
        /// Copies a Project in the database
        /// </summary>
        /// <param name="projectVersionId"></param>
        /// <returns>ProjectVersionID</returns>
        public Int32 CopyProject(Int32 projectVersionId)
        {
            //throw new NotImplementedException();

            SqlCommand cmd = new SqlCommand("[TIP].[CopyProject]");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CurrentProjectVersionID", projectVersionId);
            SqlParameter outParam = new SqlParameter("@ProjectVersionID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outParam);
            SqlConnection con = new SqlConnection(this.ConnectionString);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return (int)cmd.Parameters["@ProjectVersionID"].Value;
        }

        /// <summary>
        /// Resotre a Project in the database
        /// </summary>
        /// <param name="model"></param>
        /// <param name="tipYear"></param>
        /// <returns>ProjectVersionID</returns>
        public Int32 RestoreProjectVersion(Int32 projectVersionID, string tipYear)
        {
            SqlCommand cmd = new SqlCommand("[TIP].[CopyProject]");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CurrentProjectVersionID", projectVersionID);
            cmd.Parameters.AddWithValue("@DestinationYear", tipYear);
            SqlParameter outParam = new SqlParameter("@ProjectVersionID", SqlDbType.Int);
            outParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outParam);
            SqlConnection con = new SqlConnection(this.ConnectionString);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return (int)cmd.Parameters["@ProjectVersionID"].Value;
        }

        public void RestoreProjectVersionFinancials(Int32 currentProjectVersionId, Int32 newProjectVersionId, string tipYear)
        {
            SqlCommand cmd = new SqlCommand("[TIP].[RestoreProjectVersionFinancials]");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CurrentProjectVersionID", currentProjectVersionId);
            cmd.Parameters.AddWithValue("@NewProjectVersionID", newProjectVersionId);
            cmd.Parameters.AddWithValue("@TimePeriodId", GetYearId(tipYear, Enums.TimePeriodType.TimePeriod));
            SqlConnection con = new SqlConnection(this.ConnectionString);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        /// <summary>
        /// Deletes a ProjectVersion
        /// </summary>
        /// <param name="projectVersionId"></param>
        /// <returns>LocationMap Filename</returns>
        public String DeleteProjectVersion(ProjectAmendments model)
        {
            SqlCommand cmd = new SqlCommand("[TIP].[DeleteProjectVersion]");
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ProjectVersionID", model.ProjectVersionId);
            SqlParameter outParam = new SqlParameter("@fileName", SqlDbType.VarChar,100);
            outParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outParam);
            SqlConnection con = new SqlConnection(this.ConnectionString);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            string fileName = cmd.Parameters["@fileName"].Value != DBNull.Value ? cmd.Parameters["@fileName"].Value.ToString() : String.Empty;
            //string fileName = (String)cmd.Parameters["@fileName"].Value.ToString();
            try
            {
                if (!fileName.Equals(String.Empty))
                {
                    FileHandler.Delete(fileName, model.LocationMapPath);
                }
                else
                {
                    //throw new NoNullAllowedException();
                }
            }
            catch
            {

            }

            return String.Empty; //(String)cmd.Parameters["@fileName"].Value;
        }

        

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
        public InfoViewModel GetProjectInfoViewModel(int versionId, string tipYear)
        {
            var result = new InfoViewModel();

            // get project summary info
            result.InfoModel = GetProjectInfo(versionId, tipYear);
            //result.CdotData = GetCdotData(versionId);
            result.ProjectSummary = GetProjectSummary(versionId);
            result.ProjectSponsorsModel = GetProjectSponsorsModel(versionId, tipYear);
            if (result.InfoModel.SponsorId.HasValue && result.InfoModel.SponsorContactId.HasValue)
            {
                result.ProjectSponsorsModel.SponsorContact = GetSponsorContact(result.InfoModel.SponsorId.Value, result.InfoModel.SponsorContactId.Value);
            }

            if(!result.InfoModel.LocationMapId.Equals(default(int)))
            {
                result.InfoModel.Image = FileRepository.Load(result.InfoModel.LocationMapId);
            }
            
            // fill collections
            result.AvailableAdminLevels = AvailableAdminLevels;// GetLookupCollection("Lookup_GetProjectAdministrativeLevels", "Id", "Label");
            //result.AvailableSponsors = AvailableSponsors;// GetLookupCollection("Lookup_GetSponsorOrganizations", "Id", "Label");
            result.AvailableSponsors = GetAvailableTimePeriodSponsorAgencies(tipYear, _appState).ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
            result.AvailableImprovementTypes = AvailableImprovementTypes;// GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
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

        public DRCOG.Domain.Models.TIPProject.ProjectSponsorsModel GetProjectSponsorsModel(int projectVersionID, string tipYear)
        {
            var model = new DRCOG.Domain.Models.TIPProject.ProjectSponsorsModel();

            // Get Agencies which are eligible to sponsor projects
            SqlCommand cmd = new SqlCommand("[TIP].[GetCurrentProjectSponsorAgencies]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
            cmd.Parameters.AddWithValue("@TipYear", tipYear);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                //rdr.Read();
                //model.PrimarySponsor = new SponsorOrganization()
                //{
                //    OrganizationId = (int)rdr["OrganizationId"],
                //    OrganizationName = rdr["OrganizationName"].ToString(),
                //    IsPrimary = bool.Parse(rdr["Primary"].ToString())
                //};
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

            cmd = new SqlCommand("[TIP].[GetAvailableProjectSponsorAgencies]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);
            cmd.Parameters.AddWithValue("@TipYear", tipYear);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    model.AvailableAgencies.Add(new SponsorOrganization()
                    {
                        OrganizationId = (int)rdr["OrganizationId"],
                        OrganizationName = rdr["OrganizationName"].ToString(),
                        IsPrimary = bool.Parse(rdr["Primary"].ToString())
                    });
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
            result.AvailableImprovementTypes = GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
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
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectVersionInfo]") { CommandType = CommandType.StoredProcedure })
            {
                //model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value
                command.Parameters.AddWithValue("@PROJECTVERSIONID", model.ProjectVersionId);
                command.Parameters.AddWithValue("@TIPYEAR", model.TipYear);
                command.Parameters.AddWithValue("@PROJECTID", model.ProjectId);
                command.Parameters.AddWithValue("@PROJECTNAME", model.ProjectName);
                command.Parameters.AddWithValue("@ProjectTypeID", model.ProjectTypeId);
                //SponsorID is not stored - it's an attribute of the sponsor contact. -DB No, you are wrong. Unrelated concept. -DD
                command.Parameters.AddWithValue("@SponsorID", model.SponsorId != null ? (object) model.SponsorId.Value : (object) DBNull.Value);
                command.Parameters.AddWithValue("@SPONSORCONTACTID", model.SponsorContactId != null ? (object)model.SponsorContactId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ADMINLEVELID", model.AdministrativeLevelId != null ? (object)model.AdministrativeLevelId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@IMPTYPEID", model.ImprovementTypeId != null ? (object)model.ImprovementTypeId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProjectPoolID", model.ProjectPoolId != null ? (object)model.ProjectPoolId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ISPOOLMASTER", model.IsPoolMaster != null ? (object)model.IsPoolMaster.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TRANSTYPEID", model.TransportationTypeId != null ? (object)model.TransportationTypeId.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@SelectionAgencyID", model.SelectionAgencyId != null ? (object) model.SelectionAgencyId.Value : (object) DBNull.Value);
                command.Parameters.AddWithValue("@SPONSORNOTES", model.SponsorNotes);
                command.Parameters.AddWithValue("@DRCOGNOTES", model.DRCOGNotes);
                command.Parameters.AddWithValue("@RegionallySignificant", model.IsRegionallySignificant != null ? (object)model.IsRegionallySignificant.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StipId", model.STIPID);
                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Update ProjectVersion CDOT Data
        /// </summary>
        /// <param name="model"></param>
        public void UpdateCdotData(TipCdotData model, int projectVersionId)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[UpdateCdotData]") { CommandType = CommandType.StoredProcedure })
            {
                //model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                command.Parameters.AddWithValue("@StipId", model.STIPID);
                this.ExecuteNonQuery(command);
            }
        }
        
        /// <summary>
        /// Update the list of Eligible Agencies associated with a particular TIP. DEPRECATED.
        /// </summary>
        /// <param name="model"></param>
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
            var result = new LocationViewModel();

            //Get project summary info
            result.ProjectSummary = GetProjectSummary(versionId);
            //Get the location info
            result.TipProjectLocation = GetProjectLocationModel(versionId, tipYear) ?? new LocationModel();
            result.MuniShares = GetProjectMunicipalityShares(versionId);
            result.CountyShares = GetProjectCountyShares(versionId);
            
            result.CDOTRegions = GetCategories(27).Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() }).ToList();
            result.AffectedProjectDelaysLocation = GetCategories((int)Enums.CategoryType.AffectedProjectDelaysLocation).Select(x => new SelectListItem { Text = x.Value, Value = x.Key.ToString() }).ToList();

            if (result.TipProjectLocation != null && !result.TipProjectLocation.LocationMapId.Equals(default(int)))
            {
                result.TipProjectLocation.Image = FileRepository.Load(result.TipProjectLocation.LocationMapId);
            }

            IList<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("ProjectVersionId",versionId));

            result.AvailableCounties = GetLookupCollection("Lookup_GetAvailableCountyGeographies","Id","Label",parameters);

            IList<SqlParameter> parameters2 = new List<SqlParameter>();
            parameters2.Add(new SqlParameter("ProjectVersionId", versionId));
            result.AvailableMunicipalities = GetLookupCollection("Lookup_GetAvailableMuniGeographies","Id","Label",parameters2);
            
            return result;
        }

        /// <summary>
        /// Get the Location Model
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="tipYear"></param>
        /// <returns></returns>
        public LocationModel GetProjectLocationModel(int versionId, string tipYear)
        {
            LocationModel result = null;
             using (SqlCommand command = new SqlCommand("[TIP].[GetProjectLocation]") { CommandType = CommandType.StoredProcedure })
             {
                 command.Parameters.AddWithValue("@PROJECTVERSIONID", versionId);
                 command.Parameters.AddWithValue("@TIPYEAR", tipYear);

                 using (IDataReader rdr = ExecuteReader(command))
                 {
                     if (rdr.Read())
                     {
                         result = new LocationModel();
                         result.FacilityName = rdr["FacilityName"].ToString();
                         result.Limits = rdr["Limits"].ToString();
                         result.ProjectVersionId = versionId;
                         result.TipYear = tipYear;
                         result.ProjectName = rdr["ProjectName"].ToString();
                         result.ProjectId = rdr["ProjectId"] != DBNull.Value ? (int?)rdr["ProjectId"] : null;
                         result.LocationMapId = rdr["LocationMapID"].ToString().SmartParseDefault<int>(default(int));
                         result.CdotRegionId = rdr["CDOTRegionId"].ToString().SmartParseDefault<int>(default(int));
                         result.AffectedProjectDelaysLocationId = rdr["AffectedProjectDelaysLocationId"].ToString().SmartParseDefault<int>(default(int));
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
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectLocation]") { CommandType = CommandType.StoredProcedure })
            {
                //model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value
                command.Parameters.AddWithValue("@PROJECTVERSIONID", model.ProjectVersionId);
                command.Parameters.AddWithValue("@FACILITYNAME", model.FacilityName);
                command.Parameters.AddWithValue("@LIMITS", model.Limits);
                command.Parameters.AddWithValue("@CDOTRegionId", model.CdotRegionId);
                command.Parameters.AddWithValue("@AffectedProjectDelaysLocationId", model.AffectedProjectDelaysLocationId);
                
                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Create a new county share record (ProjectCountyGeography table)
        /// </summary>
        /// <param name="model"></param>
        public void AddCountyShare(CountyShareModel model)
        {
            ////[TIP].[AddProjectCountyGeography]
            using (SqlCommand command = new SqlCommand("[TIP].[AddProjectCountyGeography]") { CommandType = CommandType.StoredProcedure })
            {
                //model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value
                command.Parameters.AddWithValue("@PROJECTID", model.ProjectId);
                command.Parameters.AddWithValue("@COUNTYID", model.CountyId);
                command.Parameters.AddWithValue("@SHARE", model.Share);
                command.Parameters.AddWithValue("@PRIMARY", model.Primary);

                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Update an existing county share record (ProjectCountyGeography table)
        /// </summary>
        /// <param name="model"></param>
        public void UpdateCountyShare(CountyShareModel model)
        {
            //[TIP].[UpdateProjectCountyGeography]
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectCountyGeography]") { CommandType = CommandType.StoredProcedure })
            {
                //model.Adoption != null ? (object)model.Adoption.Value : (object)DBNull.Value
                command.Parameters.AddWithValue("@PROJECTID", model.ProjectId);
                command.Parameters.AddWithValue("@COUNTYID", model.CountyId);
                command.Parameters.AddWithValue("@SHARE", model.Share);
                command.Parameters.AddWithValue("@PRIMARY", model.Primary);

                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Drops a ProjectCountyGeography record
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="countyId"></param>
        public void DropCountyShare(int projectId, int countyId)
        {
            
            using (SqlCommand command = new SqlCommand("[TIP].[DropProjectCountyGeography]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTID", projectId);
                command.Parameters.AddWithValue("@COUNTYID", countyId);
                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Adds a Muni Share record (ProjectMuniGeography table)
        /// </summary>
        /// <param name="model"></param>
        public void AddMunicipalityShare(MunicipalityShareModel model)
        {
            //[TIP].[AddProjectMuniGeography]
            using (SqlCommand command = new SqlCommand("[TIP].[AddProjectMuniGeography]") { CommandType = CommandType.StoredProcedure })
            {                
                command.Parameters.AddWithValue("@PROJECTID", model.ProjectId);
                command.Parameters.AddWithValue("@MUNIID", model.MunicipalityId);
                command.Parameters.AddWithValue("@SHARE", model.Share);
                command.Parameters.AddWithValue("@PRIMARY", model.Primary);
                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Updates an existing Muni share record (ProjectMuniGeography table)
        /// </summary>
        /// <param name="model"></param>
        public void UpdateMunicipalityShare(MunicipalityShareModel model)
        {
            //[TIP].[UpdateProjectMuniGeography]
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectMuniGeography]") { CommandType = CommandType.StoredProcedure })
            {                
                command.Parameters.AddWithValue("@PROJECTID", model.ProjectId);
                command.Parameters.AddWithValue("@MUNIID", model.MunicipalityId);
                command.Parameters.AddWithValue("@SHARE", model.Share);
                command.Parameters.AddWithValue("@PRIMARY", model.Primary);
                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Drop a ProjectMuniGeography record
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="muniId"></param>
        public void DropMunicipalityShare(int projectId, int muniId)
        {
            //[TIP].[DropProjectMuniGeography]
            using (SqlCommand command = new SqlCommand("[TIP].[DropProjectMuniGeography]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTID", projectId);
                command.Parameters.AddWithValue("@MUNIID", muniId);
                this.ExecuteNonQuery(command);
            }
        }

#endregion

        #region Scope MODELS & METHODS

        public ScopeViewModel GetScopeViewModel(int projectVersionId, string tipYear)
        {
            var result = new ScopeViewModel();
            result.TipProjectScope = this.GetScopeModel(projectVersionId, tipYear);
            result.ProjectSummary = GetProjectSummary(projectVersionId);
            result.Segments = GetProjectSegments(projectVersionId);
            result.PoolProjects = GetPoolProjects(projectVersionId);
            return result;
        }

        public ScopeModel GetScopeModel(int projectVersionId, string tipYear)
        {
            ScopeModel result = null;
            
            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectScope]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                //command.Parameters.AddWithValue("@TipYear", tipYear);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        result = new ScopeModel();
                        result.BeginConstructionYear = rdr["BeginConstructionYear"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["BeginConstructionYear"]) : null;
                        result.OpenToPublicYear = rdr["EndConstructionYear"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["EndConstructionYear"]) : null;
                        result.ProjectDescription = rdr["Scope"].ToString().Trim();
                        result.ProjectId = rdr["ProjectId"] != DBNull.Value ? (int?)rdr["ProjectId"] : null;
                        result.ProjectName = rdr["ProjectName"].ToString(); 
                        result.ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? (int)rdr["ProjectVersionId"] : default(int);
                        result.TipYear = tipYear;
                    }
                }
            }
            return result;

        }

        /// <summary>
        /// Update the project scope information in the database
        /// </summary>
        /// <remarks>Uses the [TIP].[UpdateProjectScope] stored proc.</remarks>
        /// <param name="model"></param>
        public void UpdateProjectScope(ScopeModel model)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectScope]") { CommandType = CommandType.StoredProcedure })
            {                
                command.Parameters.AddWithValue("@ProjectVersionID", model.ProjectVersionId);
                command.Parameters.AddWithValue("@TipYear", model.TipYear);
                command.Parameters.AddWithValue("@BeginConstructionYear", model.BeginConstructionYear != null ? (object)model.BeginConstructionYear.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndConstructionYear", model.OpenToPublicYear != null ? (object)model.OpenToPublicYear.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@ProjectDescription", model.ProjectDescription);
                this.ExecuteNonQuery(command);
            }
        }

        public Int32 AddPoolProject(PoolProject model)
        {
            int retval = 0;
            using (SqlCommand command = new SqlCommand("[TIP].[AddPoolProject]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PoolMasterVersionId", model.PoolMasterVersionID);
                command.Parameters.AddWithValue("@ProjectName", model.ProjectName != null ? (object)model.ProjectName.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Description", model.Description != null ? (object)model.Description.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@BeginAt", model.BeginAt != null ? (object)model.BeginAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndAt", model.EndAt != null ? (object)model.EndAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Cost", model.Cost != null ? (object)model.Cost.Value: (object)DBNull.Value);

                SqlParameter outParam = new SqlParameter("@PoolProjectId", SqlDbType.Int);
                outParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(outParam);

                this.ExecuteNonQuery(command);
                retval = (int)command.Parameters["@PoolProjectId"].Value;
            }
            return retval;
        }

        public void DeletePoolProject(int poolProjectId)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[DeletePoolProject]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PoolProjectId", poolProjectId);
                this.ExecuteNonQuery(command);
            }
        }

        public void UpdatePoolProject(PoolProject model)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[UpdatePoolProject]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PoolProjectId", model.PoolProjectID);
                command.Parameters.AddWithValue("@ProjectName", model.ProjectName != null ? (object)model.ProjectName.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Description", model.Description != null ? (object)model.Description.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@BeginAt", model.BeginAt != null ? (object)model.BeginAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndAt", model.EndAt != null ? (object)model.EndAt.ToString() : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Cost", model.Cost != null ? (object)model.Cost.Value : (object)DBNull.Value);
                this.ExecuteNonQuery(command);
            }
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
                    segmentList.Add(new SegmentModel()
                    {
                        SegmentId = rdr["SegmentID"] != DBNull.Value ? (int)rdr["SegmentID"] : 0
                    ,   ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? (int)rdr["ProjectVersionId"] : 0
                    ,   ImprovementTypeId = rdr["ImprovementTypeId"] != DBNull.Value ? (int?)rdr["ImprovementTypeId"] : null
                    ,   ModelingFacilityTypeId = rdr["ModelingFacilityTypeID"] != DBNull.Value ? (int?)rdr["ModelingFacilityTypeId"] : null
                    ,   PlanFacilityTypeId = rdr["PlanFacilityTypeId"] != DBNull.Value ? (int?)rdr["PlanFacilityTypeId"] : null
                    ,   NetworkId = rdr["NetworkId"] != DBNull.Value ? (int?)rdr["NetworkId"] : null
                    ,   Network = rdr["Network"] != DBNull.Value ? rdr["Network"].ToString() : ""
                    ,   OpenYear = rdr["OpenYear"] != DBNull.Value ? (short?)rdr["OpenYear"] : null
                    ,   FacilityName = rdr["FacilityName"] != DBNull.Value ? rdr["FacilityName"].ToString() : ""
                    ,   StartAt = rdr["StartAt"] != DBNull.Value ? rdr["StartAt"].ToString() : ""
                    ,   EndAt = rdr["EndAt"] != DBNull.Value ? rdr["EndAt"].ToString() : ""
                    ,   Length = rdr["Length"] != DBNull.Value ? (decimal?)rdr["Length"] : null
                    ,   LanesBase = rdr["LanesBase"] != DBNull.Value ? (short?)rdr["LanesBase"] : null
                    ,   LanesFuture = rdr["LanesFuture"] != DBNull.Value ? (short?)rdr["LanesFuture"] : null
                    ,   SpacesFuture = rdr["SpacesFuture"] != DBNull.Value ? (short?)rdr["SpacesFuture"] : null
                    ,   VehiclesFuture = rdr["VehiclesFuture"] != DBNull.Value ? (short?)rdr["VehiclesFuture"] : null
                    ,   Cost = rdr["Cost"] != DBNull.Value ? (decimal?)rdr["Cost"] : null
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
                    , Cost = !String.IsNullOrEmpty(dr["Cost"].ToString()) ? (decimal)dr["Cost"] : (decimal)0
                });
            }
            return poolList;
        }

#endregion

        #region Funding MODELS & METHODS

        public FundingViewModel GetFundingViewModel(int projectVersionId, string tipYear)
        {
            var result = new FundingViewModel();
            result.ProjectSummary = GetProjectSummary(projectVersionId);
            result.TipProjectFunding = GetFunding(projectVersionId);
            result.ProjectFundingHistory = GetFundingHistory(projectVersionId);
            result.ProjectFundingResources = ProjectVersionFundingResources(projectVersionId);
            //result.TipProjectFundingDetail = GetFundingDetail(projectVersionId);
            result.FundingDetailPivotModel = GetFundingDetailPivot(projectVersionId);
            //result.FundingIncrements = GetFundingIncrement(projectVersionId); // Why make two calls for the same data? -DBD
            result.FundingIncrements = result.FundingDetailPivotModel.FundingIncrements;
            result.FundingPhases = GetFundingPhases(projectVersionId, tipYear);
            result.FundingYearsAvailable = GetFundingYearsAvailable(result.ProjectSummary.TipYearTimePeriodID);
            result.FundingPhasesAvailable = GetCategories(25).OrderBy(x => x.Value).Where(x => x.Value.ToLower().Contains("initiate")).ToDictionary(k => k.Key, v => v.Value);

            result.FundingTypes = FundingTypes(result.ProjectSummary.TipYearTimePeriodID);
            result.FundingLevels = FundingLevels;

            return result;
        }

        public IList<FundingModel> GetFunding(int projectVersionID)
        {
            IList<FundingModel> fundingList = new List<FundingModel>();

            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectFunding]") { CommandType = CommandType.StoredProcedure })
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
                        ,   TimePeriodID = rdr["TimePeriodID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["TimePeriodID"]) : null
                        ,   TimePeriod = rdr["TimePeriod"].ToString()
                        ,   Previous = rdr["Previous"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Previous"]) : (double)0.00
                        ,   Future = rdr["Future"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Future"]) : (double)0.00
                        ,   Funding = rdr["TIPFunding"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["TIPFunding"]) : (double)0.00
                        ,   FederalTotal = rdr["FederalTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["FederalTotal"]) : (double)0.00
                        ,   StateTotal = rdr["StateTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["StateTotal"]) : (double)0.00
                        ,   LocalTotal = rdr["LocalTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["LocalTotal"]) : (double)0.00
                        ,   TotalCost = rdr["TotalCost"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["TotalCost"]) : (double)0.00
                        });
                    }
                }
            }

            return fundingList;
        }

        public IList<FundingPhase> GetFundingPhases(int projectVersionId, string tipYear)
        {
            IList<FundingPhase> phases = new List<FundingPhase>();

            DateTime showDelayDate =  base.GetTipStatusViewModel(tipYear).TipStatus.ShowDelayDate.Value;

            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectFundingPhases]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        phases.Add(new FundingPhase()
                        {
                            ProjectFinancialRecordId = rdr["ProjectFinancialRecordId"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            FundingIncrementId = rdr["FundingIncrementID"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            FundingResourceId = rdr["FundingResourceID"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            PhaseId = rdr["PhaseID"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            FundingResource = rdr["FundingType"].ToString()
                            ,
                            Phase = rdr["Phase"].ToString()
                            ,
                            Year = rdr["Year"].ToString()
                            ,
                            IsInitiated = rdr["IsInitiated"].ToString().SmartParseDefault(default(bool))
                            ,
                            ShowDelayDate = showDelayDate
                        });
                    }
                }
            }
            return phases;
        }

        public void DeletePhase(FundingPhase phase)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[DeleteProjectFinancialRecordDetailPhase]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectFinancialRecordId", phase.ProjectFinancialRecordId);
                command.Parameters.AddWithValue("@FundingIncrementId", phase.FundingIncrementId);
                command.Parameters.AddWithValue("@FundingResourceId", phase.FundingResourceId);
                command.Parameters.AddWithValue("@PhaseId", phase.PhaseId);
                this.ExecuteNonQuery(command);
            }
        }

        public void AddPhase(FundingPhase phase)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[AddProjectFinancialRecordDetailPhase]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectFinancialRecordId", phase.ProjectFinancialRecordId);
                command.Parameters.AddWithValue("@FundingIncrementId", phase.FundingIncrementId);
                command.Parameters.AddWithValue("@FundingResourceId", phase.FundingResourceId);
                command.Parameters.AddWithValue("@PhaseId", phase.PhaseId);
                this.ExecuteNonQuery(command);
            }
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
                            ProjectVersionId = rdr["ProjectVersionId"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["ProjectVersionId"]) : null
                        ,   ProjectFinancialRecordID = rdr["ProjectFinancialRecordID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["ProjectFinancialRecordID"]) : null
                        ,   TimePeriodID = rdr["TimePeriodID"] != DBNull.Value ? (int?)Convert.ToInt32(rdr["TimePeriodID"]) : null
                        ,   TimePeriod = rdr["TimePeriod"].ToString()
                        ,   Previous = rdr["Previous"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Previous"]) : (double)0.00
                        ,   Future = rdr["Future"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["Future"]) : (double)0.00
                        ,   Funding = rdr["TIPFunding"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["TIPFunding"]) : (double)0.00
                        ,   FederalTotal = rdr["FederalTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["FederalTotal"]) : (double)0.00
                        ,   StateTotal = rdr["StateTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["StateTotal"]) : (double)0.00
                        ,   LocalTotal = rdr["LocalTotal"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["LocalTotal"]) : (double)0.00
                        ,   TotalCost = rdr["TotalCost"] != DBNull.Value ? (double?)Convert.ToDouble(rdr["TotalCost"]) : (double)0.00
                        ,   AmendmentDate = rdr["AmendmentDate"] != DBNull.Value ? (DateTime?)rdr["AmendmentDate"] : DateTime.MinValue
                        ,   AmendmentStatus = rdr["AmendmentStatus"].ToString()
                        });
                    }
                }
            }

            return fundingList;
        }

        [Obsolete("SP not created",true)]
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
            fundingDetailPivot.FundingDetailTable = new DataTable("FundingDetail");

            using (SqlCommand command = new SqlCommand("TIP.GetProjectFundingDetailPivot") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", projectVersionID);

                fundingDetailPivot.FundingDetailTable = ExecuteDataTable(command);
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

        [Obsolete("SP not created", true)]
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

        public void UpdateFinancialRecord(FundingModel model)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectFinancialRecord]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectFinancialRecordID", model.ProjectFinancialRecordID);
                command.Parameters.AddWithValue("@Previous", model.Previous != null ? (object)model.Previous.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@Future", model.Future != null ? (object)model.Future.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TIPFunding", model.Funding != null ? (object)model.Funding.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@FederalTotal", model.FederalTotal != null ? (object)model.FederalTotal.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@StateTotal", model.StateTotal != null ? (object)model.StateTotal.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@LocalTotal", model.LocalTotal != null ? (object)model.LocalTotal.Value : (object)DBNull.Value);
                command.Parameters.AddWithValue("@TotalCost", model.TotalCost != null ? (object)model.TotalCost.Value : (object)DBNull.Value);
                this.ExecuteNonQuery(command);
            }
        }

        [Obsolete("SP not created", true)]
        public void DeleteFinancialRecord(int projectFinancialRecordId)
        {
            using (SqlCommand command = new SqlCommand("TIP_DeleteProjectFinancialRecord") { CommandType = CommandType.StoredProcedure })
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

        public void DeleteFinancialRecordDetail(int projectVersionID, int fundingResourceId)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[DeleteProjectVersionFundingResources]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionID);
                command.Parameters.AddWithValue("@FundingResourceId", fundingResourceId);
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
                command.Parameters.AddWithValue("@incr01", !model.Incr01.Equals(default(decimal)) ? (object)model.Incr01 : (object)DBNull.Value);
                command.Parameters.AddWithValue("@incr02", !model.Incr02.Equals(default(decimal)) ? (object)model.Incr02 : (object)DBNull.Value);
                command.Parameters.AddWithValue("@incr03", !model.Incr03.Equals(default(decimal)) ? (object)model.Incr03 : (object)DBNull.Value);
                command.Parameters.AddWithValue("@incr04", !model.Incr04.Equals(default(decimal)) ? (object)model.Incr04 : (object)DBNull.Value);
                command.Parameters.AddWithValue("@incr05", !model.Incr05.Equals(default(decimal)) ? (object)model.Incr05 : (object)DBNull.Value);
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

#endregion

        #region Amendment MODELS & METHODS

        public AmendmentsViewModel GetAmendmentsViewModel(int projectVersionId, string tipYear)
        {
            var result = new AmendmentsViewModel();

            // get project summary info
            result.InfoModel = GetProjectInfo(projectVersionId, tipYear);
            result.ProjectSummary = GetProjectSummary(projectVersionId);

            TIPSearchModel search = new TIPSearchModel()
            {
                ProjectId = (Int32)result.ProjectSummary.ProjectId
                ,
                TipYearID = GetYearId(tipYear, Enums.TimePeriodType.TimePeriod)
            };
            result.AmendmentList = GetProjectAmendments(search);
            result.ProjectAmendments = result.AmendmentList.FirstOrDefault() ?? new ProjectAmendments();
            //result.ProjectAmendments.ProjectVersionId = projectVersionId;

            var allowedTypes = new List<Enums.AmendmentType>();
            allowedTypes.Add(Enums.AmendmentType.Administrative);
            allowedTypes.Add(Enums.AmendmentType.Policy);
            result.AmendmentTypes = new Dictionary<int, string>();
            foreach (Enums.AmendmentType type in allowedTypes)
            {
                result.AmendmentTypes.Add((int)type, StringEnum.GetStringValue(type));
            }

            return result;
        }

        public void UpdateAmendmentDetails(ProjectAmendments amendment)
        {
            using (SqlCommand command = new SqlCommand("TIP.UpdateAmendmentDetails") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionID", amendment.ProjectVersionId);
                command.Parameters.AddWithValue("@AmendmentReason", amendment.AmendmentReason);
                command.Parameters.AddWithValue("@AmendmentCharacter", amendment.AmendmentCharacter);
                this.ExecuteNonQuery(command);
            }
        }

        /// <summary>
        /// Update a project's Amendment Status
        /// </summary>
        /// <param name="model"></param>
        public void UpdateProjectAmendmentStatus(ProjectAmendments model)
        {
            //throw new NotImplementedException();
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectAmendmentStatus]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", model.ProjectVersionId);
                command.Parameters.AddWithValue("@AmendmentStatusId", model.AmendmentStatusId);
                command.Parameters.AddWithValue("@AmendmentTypeId", model.AmendmentTypeId);
                command.Parameters.AddWithValue("@VersionStatusId", model.VersionStatusId);
                command.Parameters.AddWithValue("@AmendmentReason", String.IsNullOrEmpty(model.AmendmentReason) ? null : model.AmendmentReason);
                command.Parameters.AddWithValue("@AmendmentCharacter", String.IsNullOrEmpty(model.AmendmentCharacter) ? null : model.AmendmentCharacter);
                this.ExecuteNonQuery(command);
            }

            if (model.UpdateLocationMap)
            {
                string fileName = String.Empty;
                string newName = String.Empty;

                using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectVersionLocationMap]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddWithValue("@ProjectVersionId", model.ProjectVersionId);

                    using (IDataReader rdr = ExecuteReader(command))
                    {
                        if (rdr.Read())
                        {
                            fileName = rdr["fileName"].ToString();
                            newName = rdr["newName"].ToString();
                        }
                    }
                }
                try
                {
                    if (!fileName.Equals(String.Empty) && !newName.Equals(String.Empty))
                    {
                        FileHandler.Copy(fileName, newName, model.LocationMapPath, model.LocationMapPath);
                    }
                }
                catch
                {

                }
            }
        }

        public void UpdateProjectVersionStatusId(Int32 projectVersionId, Int32 versionStatusId)
        {
            using (SqlCommand command = new SqlCommand("[TIP].[UpdateProjectVersionStatusId]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                command.Parameters.AddWithValue("@VersionStatusId", versionStatusId);
                this.ExecuteNonQuery(command);
            }
        }

        public Int32 GetProjectAmendmentStatus(Int32 projectVersionId)
        {
            int rval = 0;
            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectAmendmentStatus]") { CommandType = CommandType.StoredProcedure })
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

        public Int32 GetPreviousProjectVersionId(Int32 projectVersionId)
        {
            int rval = 0;
            using (SqlCommand command = new SqlCommand("[TIP].[GetPreviousProjectVersionId]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);
                command.Parameters.AddWithValue("@IsActive", true);
                rval = this.ExecuteScalar<Int32>(command);
            }
            return rval;
        }

#endregion

        #region Report METHODS

        public IDictionary<string,string> GetProjectGeneralInfo(int versionId)
        {
            IDictionary<string, string> result = null;

            using (SqlCommand command = new SqlCommand("[TIP].[GetProjectDetails]") { CommandType = CommandType.StoredProcedure })
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
                        result.Add("EndConstructionYear", rdr["EndConstructionYear"].ToString());
                        result.Add("STIPID", rdr["STIPID"].ToString());
                    }
                }
            }
            return result;
        }

        public DetailViewModel GetDetailViewModel(Int32 projectVersionId, String tipYear)
        {
            var result = new DetailViewModel();
            result.StringValues = new Dictionary<string, string>();
            result.GeneralInfo = GetProjectGeneralInfo(projectVersionId);

            InfoViewModel InfoViewModel = GetProjectInfoViewModel(projectVersionId, tipYear);
            SegmentViewModel SegmentViewModel = GetSegmentViewModel(projectVersionId);

            // get project summary info
            result.ProjectSummary = GetProjectSummary(projectVersionId);
            result.InfoModel = InfoViewModel.InfoModel;
            result.ProjectSponsorsModel = InfoViewModel.ProjectSponsorsModel;
            //result.StringValues.Add("AdminLevel",GetValueByKey(AvailableAdminLevels, result.InfoModel.AdministrativeLevelId.Value));
            //result.StringValues.Add("ProjectType", GetValueByKey(AvailableProjectTypes, result.InfoModel.ProjectTypeId.Value));
            //result.StringValues.Add("ImprovementType", GetValueByKey(AvailableImprovementTypes, result.InfoModel.ImprovementTypeId.Value));
            //result.StringValues.Add("RoadOrTransit", GetValueByKey(AvailableRoadOrTransitTypes, result.InfoModel.TransportationTypeId.Value));
            //result.StringValues.Add("PoolName", GetValueByKey(AvailablePools(result.ProjectSummary), result.InfoModel.ProjectPoolId.Value));
            result.Segments = GetProjectSegments(projectVersionId);
            result.PoolProjects = GetPoolProjects(projectVersionId);

            TIPSearchModel search = new TIPSearchModel()
            {
                ProjectId = (Int32)result.ProjectSummary.ProjectId
                ,
                TipYearID = GetYearId(tipYear, Enums.TimePeriodType.TimePeriod)
            };
            result.AmendmentList = GetProjectAmendments(search);

            result.MuniShares = GetProjectMunicipalityShares(projectVersionId);
            result.CountyShares = GetProjectCountyShares(projectVersionId);

            result.FundingDetailPivotModel = GetFundingDetailPivot(projectVersionId);
            result.TipProjectFunding = GetFunding(projectVersionId).FirstOrDefault();

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

        public TipCdotData GetCdotData(int projectVersionId)
        {
            TipCdotData cdotData = null;

            SqlCommand cmd = new SqlCommand("[dbo].[GetCDOTData]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

            cdotData = new TipCdotData();
            DataTable dt = this.ExecuteDataTable(cmd);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                
                cdotData.Region = row["Region"] != DBNull.Value ? (short)row["Region"] : (short)0;
                cdotData.CommDistrict = row["CommDistrict"] != DBNull.Value ? (short)row["CommDistrict"] : (short)0;
                cdotData.RouteSegment = row["RouteSegment"].ToString();
                cdotData.BeginMilePost = row["BeginMilePost"] != DBNull.Value ? (double)row["BeginMilePost"] : 0.0;
                cdotData.EndMilePost = row["EndMilePost"] != DBNull.Value ? (double)row["EndMilePost"] : 0.0;
                cdotData.STIPID = row["STIPID"].ToString();
                cdotData.STIPProjectDivision = row["STIPProjectDivision"].ToString();
                cdotData.CDOTProjectManager = row["CDOTProjectManager"] != DBNull.Value ? (int)row["CDOTProjectManager"] : 0;
                cdotData.TPRAbbr = row["TPRAbbr"].ToString();
                cdotData.TPRID = row["TPRID"] != DBNull.Value ? (short)row["TPRID"] : (short)0;
                cdotData.LRPNumber = row["LRPNumber"] != DBNull.Value ? (int)row["LRPNumber"] : 0;
                cdotData.InvestmentCategory = row["InvestmentCategory"].ToString();
                cdotData.CorridorID = row["CorridorID"].ToString();
                cdotData.CDOTProjectNumber = row["CDOTProjectNumber"].ToString();
                cdotData.SubAccount = row["SubAccount"] != DBNull.Value ? (int)row["SubAccount"] : 0;
                cdotData.ConstructionRE = row["ConstructionRE"] != DBNull.Value ? (int)row["ConstructionRE"] : 0;
                cdotData.CMSNumber = row["CMSNumber"].ToString();
                cdotData.ScheduledADDate = row["ScheduledADDate"] != DBNull.Value ? (DateTime)row["ScheduledADDate"] : DateTime.MinValue;
                cdotData.ProjectStage = row["ProjectStage"] != DBNull.Value ? (int)row["ProjectStage"] : 0;
                cdotData.ProjectStageDate = row["ProjectStageDate"] != DBNull.Value ? (DateTime)row["ProjectStageDate"] : DateTime.MinValue;
                cdotData.ConstructionDate = row["ConstructionDate"] != DBNull.Value ? (DateTime)row["ConstructionDate"] : DateTime.MinValue;
                cdotData.ProjectClosed = row["ProjectClosed"] != DBNull.Value ? (DateTime)row["ProjectClosed"] : DateTime.MinValue;
                cdotData.Notes = row["Notes"].ToString();

            }
            return cdotData;
        }

        public ProjectCdotDataViewModel GetCDOTDataViewModel(int projectVersionId, string tipYear)
        {
            var result = new ProjectCdotDataViewModel();

            // get project summary info
            result.ProjectSummary = GetProjectSummary(projectVersionId);
            result.TipCdotData = GetCdotData(projectVersionId);
            return result;
        }

        public StrikesViewModel GetStrikesViewModel(int projectVersionId, string tipYear)
        {
            var result = new StrikesViewModel();

            // get project summary info
            result.ProjectSummary = GetProjectSummary(projectVersionId);

            return result;
        }
        
    }
}
