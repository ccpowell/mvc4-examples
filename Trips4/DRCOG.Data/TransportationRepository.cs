#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			REMARKS
 * 02/17/2010   DDavidson        1. Initial Creation.
 * 
 * DESCRIPTION:
 * Base implementation of Transportation repository for 
 * general data access and lookup collections.
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain;
using DRCOG.Domain.Helpers;
using DRCOG.Domain.Interfaces;
using System.Data.SqlClient;
using System.Data;
using DRCOG.Domain.Models;
using System.IO;
using System.Xml;

using System.Data.SqlTypes;
using DRCOG.Common.Util;

namespace DRCOG.Data
{
    public class TransportationRepository : BaseRepository, ITransportationRepository
    {
        protected Enums.ApplicationState _appState;

#region SINGLE LOOKUPS

        public int GetYearId(string year, DRCOG.Domain.Enums.TimePeriodType timePeriodType)
        {
            int result = 0;
            if (!String.IsNullOrEmpty(year))
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetTimePeriodID");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TIMEPERIOD", year);
                cmd.Parameters.AddWithValue("@TimePeriodTypeId", (int)timePeriodType);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["ID"] != DBNull.Value ? int.Parse(rdr["ID"].ToString()) : 0;
                    else result = 0;
                }
            }
            return result;
        }

        public string GetYear(int yearId)
        {
            string result = "";
            SqlCommand cmd = new SqlCommand("Lookup_GetTimePeriod");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TIMEPERIODID", yearId);
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read()) result = rdr["Value"] != DBNull.Value ? rdr["Value"].ToString() : "";
                else result = "";
            }
            return result;
        }

        public int? GetSponsorAgencyID(string sponsorAgency)
        {
            int? result = null;
            if (!sponsorAgency.Equals(""))
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetSponsorAgencyID");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SPONSORAGENCY", sponsorAgency);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["ID"] != DBNull.Value ? int.Parse(rdr["ID"].ToString()) : (int?)null;
                    else result = null;
                }
            }
            return result;
        }

        public string GetSponsorAgency(int? sponsorAgencyID)
        {
            string result = "";
            if (sponsorAgencyID.HasValue)
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetSponsorAgency");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SPONSORAGENCYID", sponsorAgencyID);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["Value"] != DBNull.Value ? rdr["Value"].ToString() : "";
                    else result = "";
                }
            }
            return result;
        }

        

        public int? GetImprovementTypeID(string improvementType)
        {
            int? result = null;
            if (!improvementType.Equals(""))
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetImprovementTypeID");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IMPROVEMENTTYPE", improvementType);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["ID"] != DBNull.Value ? int.Parse(rdr["ID"].ToString()) : (int?)null;
                    else result = null;
                }
            }
            return result;
        }

        public string GetImprovementType(int? improvementTypeID)
        {
            string result = "";
            if (improvementTypeID.HasValue)
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetImprovementType");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IMPROVEMENTTYPEID", improvementTypeID);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["Value"] != DBNull.Value ? rdr["Value"].ToString() : "";
                    else result = "";
                }
            }
            return result;
        }

        public int? GetProjectTypeID(string projectType)
        {
            int? result = null;
            if (!projectType.Equals(""))
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetProjectTypeID");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectType", projectType);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["ID"] != DBNull.Value ? int.Parse(rdr["ID"].ToString()) : (int?)null;
                    else result = null;
                }
            }
            return result;
        }

        public int GetCategoryId(string category)
        {
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@Category", category));
            return GetLookupSingle<int>("[dbo].[Lookup_GetCategoryId]","CategoryId", paramList);
        }
        public string GetCategory(int categoryId)
        {
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@CategoryId", categoryId));
            return GetLookupSingle<string>("[dbo].[Lookup_GetCategory]", "Category", paramList);
        }

        public IDictionary<int, string> GetCategories(int typeId)
        {
            IList<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@TypeId", typeId));
            return GetLookupCollection("[dbo].[Lookup_CategoriesByType]", "Id", "Label", paramList);
        }


        

        public string GetProjectType(int? projectTypeID)
        {
            string result = "";
            if (projectTypeID.HasValue)
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetProjectType");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProjectTypeID", projectTypeID);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["Value"] != DBNull.Value ? rdr["Value"].ToString() : "";
                    else result = "";
                }
            }
            return result;
        }

        public int? GetStatusID(string status, string statusType)
        {
            int? result = null;
            if (!status.Equals(""))
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetStatusID");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@StatusType", statusType);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["ID"] != DBNull.Value ? int.Parse(rdr["ID"].ToString()) : (int?)null;
                    else result = null;
                }
            }
            return result;
        }

        public string GetStatus(int? statusID, string statusType)
        {
            string result = "";
            if (statusID.HasValue)
            {
                SqlCommand cmd = new SqlCommand("Lookup_GetStatus");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatusID", statusID);
                cmd.Parameters.AddWithValue("@StatusType", statusType);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    if (rdr.Read()) result = rdr["Value"] != DBNull.Value ? rdr["Value"].ToString() : "";
                    else result = "";
                }
            }
            return result;
        }

#endregion

#region MULTIPLE LOOKUPS

        public IList<Organization> GetCurrentTimePeriodSponsorAgencies(string timePeriod, Enums.ApplicationState appState)
        {
            IList<Organization> eligibleAgencies = new List<Organization>();

            string sproc = String.Empty;
            switch (appState)
            {
                case Enums.ApplicationState.RTP: sproc = "[RTP].[GetCurrentPlanSponsorAgencies]"; break;
                case Enums.ApplicationState.TIP: sproc = "[TIP].[GetCurrentSponsorAgencies]"; break;
                case Enums.ApplicationState.Survey: sproc = "[Survey].[GetCurrentSponsorAgencies]"; break;
            }

            // Get Agencies which are eligible to sponsor projects
            SqlCommand cmd = new SqlCommand(sproc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TimePeriod", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = timePeriod;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    eligibleAgencies.Add(new Organization()
                    {
                        OrganizationId = (int)rdr["OrganizationId"],
                        OrganizationName = rdr["OrganizationName"].ToString()
                    });
                }
            }
            return eligibleAgencies;
        }

        public IList<Organization> GetAvailableTimePeriodSponsorAgencies(string year, Enums.ApplicationState appState)
        {
            IList<Organization> availableAgencies = new List<Organization>();

            string sproc = String.Empty;
            switch (appState)
            {
                case Enums.ApplicationState.RTP: sproc = "[RTP].[GetAvailableSponsorAgencies]"; break;
                case Enums.ApplicationState.TIP: sproc = "[TIP].[GetAvailableSponsorAgencies]"; break;
                case Enums.ApplicationState.Survey: sproc = "[Survey].[GetAvailableSponsorAgencies]"; break;
            }

            // Get Agencies which are eligible to sponsor projects
            SqlCommand cmd = new SqlCommand(sproc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TimePeriod", SqlDbType.NVarChar));
            cmd.Parameters[0].Value = year;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                   availableAgencies.Add(new Organization()
                    {
                        OrganizationId = (int)rdr["OrganizationId"],
                        OrganizationName = rdr["OrganizationName"].ToString()
                    });
                }
            }
            return availableAgencies;
        }

        protected IDictionary<int, string> AvailableImprovementTypes()
        {
            return GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label");
        }

        public IDictionary<int, string> GetFundingYearsAvailable(int timePeriodId)
        {

            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("@TimePeriodId", timePeriodId));
            IDictionary<int, string> temp = GetLookupCollection("[dbo].[GetTimePeriodYears]", "IncrementId", "Year", sqlParms);

            Dictionary<int, string> ret = new Dictionary<int, string>();

            foreach (KeyValuePair<int,string> kvp in temp)
            {
                ret.Add(kvp.Key, kvp.Value);
            }
            return ret;
        }

        protected IDictionary<int, string> ProjectVersionFundingResources(int projectVersionId)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("@ProjectVersionId", projectVersionId));
            return GetLookupCollection("Lookup_GetProjectVersionFundingResources", "FundingResourceID", "FundingType", sqlParms);
        }
        
        protected IDictionary<int, string> AvailableImprovementTypes(int rspCodeId)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("@RSPCodeId", rspCodeId));

            return AvailableImprovementTypes(sqlParms);
        }

        protected IDictionary<int, string> AvailableImprovementTypes(List<SqlParameter> sqlParms)
        {
            return GetLookupCollection("Lookup_GetImprovementTypes", "Id", "Label", sqlParms);
        }

        protected IList<ImprovementType> GetImprovementTypes(List<SqlParameter> sqlParams)
        {
            List<ImprovementType> result = new List<ImprovementType>();

            var collection = AvailableImprovementTypes(sqlParams);

            foreach (var x in collection)
            {
                result.Add(new ImprovementType() { Id = x.Key, Description = x.Value });
            }
            return result;
        }

        protected IDictionary<int, string> AvailableFundingResources(Enums.ApplicationState program)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("@ProgramId", (int)program));

            return AvailableFundingResources(sqlParms);
        }

        protected IDictionary<int, string> AvailableFundingResources(Enums.ApplicationState program, int timePeriodId)
        {
            List<SqlParameter> sqlParms = new List<SqlParameter>();
            sqlParms.Add(new SqlParameter("@ProgramId", (int)program));
            sqlParms.Add(new SqlParameter("@TimePeriodId", timePeriodId));

            return AvailableFundingResources(sqlParms);
        }

        protected IDictionary<int, string> AvailableFundingResources(List<SqlParameter> sqlParms)
        {
            return GetLookupCollection("[dbo].[Lookup_GetFundingResources]", "FundingResourceID", "FundingType", sqlParms);
        }

        protected IList<FundingResource> GetFundingResources(List<SqlParameter> sqlParams)
        {
            List<FundingResource> result = new List<FundingResource>();

            var collection = AvailableFundingResources(sqlParams);

            foreach (var x in collection)
            {
                result.Add(new FundingResource() { FundingResourceId = x.Key, FundingType = x.Value });
            }
            return result;
        }

        public IDictionary<int, string> AvailableFundingGroups()
        {
            return GetLookupCollection("[dbo].[Lookup_GetFundingGroups]", "FundingGroupID", "FundingGroup");
        }

        public IList<Organization> GetOrganizationsByType(Enums.OrganizationType type)
        {
            List<Organization> list = new List<Organization>();
            SqlCommand cmd = new SqlCommand("[dbo].[Lookup_GetOrganizationsByType]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@OrganizationTypeID", (int)type);

            using (IDataReader rdr = ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    list.Add(new Organization()
                    {
                        OrganizationId = Convert.ToInt32(rdr["OrganizationID"])
                        ,
                        OrganizationName = rdr["OrganizationName"].ToString()
                        ,
                        LegalName = rdr["LegalName"].ToString()
                    });
                }
            }
            return list;
        }

        public IDictionary<int, string> GetSponsorContacts(int sponsorOrganizationID)
        {
            return GetLookupCollection("Lookup_GetSponsorContacts", "Id", "Name", new List<SqlParameter>
              {
                  new SqlParameter { ParameterName = "@OrganizationID", SqlDbType = SqlDbType.Int, Value = sponsorOrganizationID }
              });
        }

        public IDictionary<int, string> GetSponsorContactRoles()
        {
            return GetLookupCollection("[dbo].[Lookup_GetSponsorContactRoles]", "RoleID", "Role");
        }

        public IList<ContactRole> GetSponsorContactRoles(string userShortGuid)
        {
            List<ContactRole> result = new List<ContactRole>();

            using (SqlCommand command = new SqlCommand("[dbo].[GetSponsorContactRolesByUser]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@UserId", ShortGuid.Decode(userShortGuid));

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        result.Add(new ContactRole()
                        {
                            PersonId = rdr["PersonID"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            PersonGuid = rdr["PersonGUID"].ToString().SmartParseDefault<Guid>(default(Guid))
                            ,
                            OrganizationId = rdr["OrganizationID"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            OrganizationName = rdr["OrganizationName"].ToString()
                            ,
                            RoleId = rdr["RoleID"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            Role = rdr["Role"].ToString()
                        });
                    }
                }
            }
            return result;
        }

        public bool SetSponsorContactRole(string userShortGuid, string organization, string role)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[SetSponsorContactRoleByUser]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@UserId", ShortGuid.Decode(userShortGuid));
                command.Parameters.AddWithValue("@Organization", organization);
                command.Parameters.AddWithValue("@Role", role);

                try
                {
                    int rows = ExecuteNonQuery(command);
                    return true;
                }
                catch { return false; }
            }
        }

        public bool DeleteSponsorContactRoleByUser(string userShortGuid, string organization, string role)
        {
            using (SqlCommand command = new SqlCommand("[dbo].[DeleteSponsorContactRoleByUser]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@UserId", ShortGuid.Decode(userShortGuid));
                command.Parameters.AddWithValue("@Organization", organization);
                command.Parameters.AddWithValue("@Role", role);

                try
                {
                    int rows = ExecuteNonQuery(command);
                    return true;
                }
                catch { return false; }
            }
        }

        public String GetSponsorContact(int sponsorOrganizationID, int sponsorContactId)
        {
            IDictionary<int, string> sponsorContacts = GetSponsorContacts(sponsorOrganizationID);
            //string contact = sponsorContacts.Where(x => x.Key.Equals(sponsorContactId)).Select(x => x.Value).ToString();
            if (sponsorContacts.ContainsKey(sponsorContactId))
            {
                string contact = (from c in sponsorContacts
                                  where c.Key.Equals(sponsorContactId)
                                  select c).First<KeyValuePair<int, string>>().Value.ToString();
                return contact;
            } return null;
        }
        
#endregion

        #region General Actions

        public Scheme GetLRSScheme(int id)
        {
            Scheme scheme = new Scheme();

            SchemeRecord record = null;
            using (SqlCommand command = new SqlCommand("[dbo].[GetLRSSchemeAttr]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@LRSSchemeId", id);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        record = new SchemeRecord()
                        {
                            Id = rdr["Id"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            ColumnName = rdr["COLUMN_NAME"].ToString()
                            ,
                            FriendlyName = rdr["FRIENDLY_NAME"].ToString()
                            ,
                            DataType = rdr["DATA_TYPE"].ToString()
                            ,
                            DisplayType = rdr["DISPLAY_TYPE"].ToString()
                            ,
                            MaxLenght = rdr["CHARACTER_MAXIMUM_LENGTH"].ToString().SmartParseDefault<int>(default(int))
                            ,
                            ColumnDefault = rdr["COLUMN_DEFAULT"].ToString()
                            ,
                            IsNullable = rdr["IS_NULLABLE"].ToString().SmartParseDefault<bool>(false)
                            ,
                            IsRequired = rdr["IS_REQUIRED"].ToString().SmartParseDefault<bool>(false)
                        };
                        scheme.Add(record);
                    }
                }
            }
            return scheme;
        }

        public LRSRecord GetSegmentLRSData(int schemeId, int LRSId)
        {
            LRSRecord record = new LRSRecord() { Columns = new Dictionary<string, string>() };

            using (SqlCommand command = new SqlCommand("[RTP].[GetSegmentLRSData]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@LRSSchemeId", schemeId);
                command.Parameters.AddWithValue("@LRSId", LRSId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        if (record.Id.Equals(default(int))) { record.Id = rdr["Id"].ToString().SmartParseDefault<int>(default(int)); }
                        record.Columns.Add(rdr["NodeName"].ToString(), rdr["Value"].ToString());
                    }
                }
                record.Scheme = GetLRSScheme(schemeId);
            }
            return record;
        }

        public LRSRecords GetSegmentLRSSummary(int schemeId, int segmentId)
        {
            LRSRecords records = new LRSRecords();
            LRSRecord record;

            using (SqlCommand command = new SqlCommand("[RTP].[GetSegmentLRSSummaryData]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@LRSSchemeId", schemeId);
                command.Parameters.AddWithValue("@SegmentId", segmentId);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        record = new LRSRecord() { Columns = new Dictionary<string, string>() };
                        record.Id = rdr["Id"].ToString().SmartParseDefault<int>(default(int));
                        record.Columns.Add("Routename", rdr["Routename"].ToString());
                        record.Columns.Add("BEGINMEASU", rdr["BEGINMEASU"].ToString());
                        record.Columns.Add("ENDMEASURE", rdr["ENDMEASURE"].ToString());
                        record.Scheme = GetLRSScheme(schemeId);
                        records.Add(record);
                        
                        //if (record.Id.Equals(default(int))) { record.Id = rdr["Id"].ToString().SmartParseDefault<int>(default(int)); }
                        //record.Columns.Add(rdr["NodeName"].ToString(), rdr["Value"].ToString());
                    }
                }
            }
            return records;
        }

        

        /// <summary>
        /// Add a sponsor agency TimePeriod
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <param name="organizationId"></param>
        public string AddAgencyToTimePeriod(string timePeriod, int organizationId, Enums.ApplicationState appState)
        {
            string result = "";
            int programID = (int)appState;

            string sproc = String.Empty;
            DRCOG.Domain.Enums.TimePeriodType timePeriodType;
            switch (appState)
            {
                case Enums.ApplicationState.RTP: 
                    sproc = "[RTP].[InsertSponsorOrganization]";
                    timePeriodType = Enums.TimePeriodType.PlanYear;
                    break;
                case Enums.ApplicationState.TIP: 
                    sproc = "[TIP].[InsertSponsorOrganization]";
                    timePeriodType = Enums.TimePeriodType.TimePeriod;
                    break;
                case Enums.ApplicationState.Survey:
                    sproc = "[Survey].[InsertSponsorOrganization]";
                    timePeriodType = Enums.TimePeriodType.Survey;
                    break;
                default: return "Application not specified.";
            }

            SqlCommand cmd = new SqlCommand(sproc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProgramID", programID);
            cmd.Parameters.AddWithValue("@TimePeriodID", GetYearId(timePeriod, timePeriodType)); //Needs to be in ID format.
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
        /// Drop a sponsor agency from TimePeriod.
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <param name="organizationId"></param>
        /// <returns>if false, the agency sponsors projects thus can not be removed</returns>
        public string DropAgencyFromTimePeriod(string timePeriod, int organizationId, Enums.ApplicationState appState)
        {
            string result = "";
            int programID = (int)appState;

            string sproc = String.Empty;
            DRCOG.Domain.Enums.TimePeriodType timePeriodType;
            switch (appState)
            {
                case Enums.ApplicationState.RTP: 
                    sproc = "[RTP].[DeleteSponsorOrganization]";
                    timePeriodType = Enums.TimePeriodType.PlanYear;
                    break;
                case Enums.ApplicationState.TIP: 
                    sproc = "[TIP].[DeleteSponsorOrganization]";
                    timePeriodType = Enums.TimePeriodType.TimePeriod;
                    break;
                case Enums.ApplicationState.Survey:
                    sproc = "[Survey].[DeleteSponsorOrganization]";
                    timePeriodType = Enums.TimePeriodType.Survey;
                    break;
                default: return "Application not specified.";
            }

            SqlCommand cmd = new SqlCommand(sproc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProgramID", programID);
            cmd.Parameters.AddWithValue("@TimePeriodID", GetYearId(timePeriod, timePeriodType));
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
        /// Add a ImprovementType to TimePeriod
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <param name="improvementTypeId"></param>
        public string AddImprovementTypeToTimePeriod(string timePeriod, int improvementTypeId, Enums.ApplicationState appState)
        {
            string result = "";
            int programID = (int)appState;

            DRCOG.Domain.Enums.TimePeriodType timePeriodType;
            switch (appState)
            {
                case Enums.ApplicationState.RTP:
                    timePeriodType = Enums.TimePeriodType.PlanYear;
                    break;
                case Enums.ApplicationState.TIP:
                    timePeriodType = Enums.TimePeriodType.TimePeriod;
                    break;
                case Enums.ApplicationState.Survey:
                    timePeriodType = Enums.TimePeriodType.Survey;
                    break;
                default: return "Application not specified.";
            }

            SqlCommand cmd = new SqlCommand("dbo.InsertProgramInstanceImprovementType");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProgramId", programID);
            cmd.Parameters.AddWithValue("@TimePeriodId", GetYearId(timePeriod, timePeriodType)); //Needs to be in ID format.
            cmd.Parameters.AddWithValue("@ImprovementTypeId", improvementTypeId);

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
        /// Drop an ImprovementType from TimePeriod.
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <param name="improvementTypeId"></param>
        /// <returns>if false, the improvementType is used in a project thus can not be removed</returns>
        public bool DropImprovementTypeFromTimePeriod(string timePeriod, int improvementTypeId, Enums.ApplicationState appState)
        {
            string result = "";
            int programID = (int)appState;

            DRCOG.Domain.Enums.TimePeriodType timePeriodType;
            switch (appState)
            {
                case Enums.ApplicationState.RTP:
                    timePeriodType = Enums.TimePeriodType.PlanYear;
                    break;
                case Enums.ApplicationState.TIP:
                    timePeriodType = Enums.TimePeriodType.TimePeriod;
                    break;
                case Enums.ApplicationState.Survey:
                    timePeriodType = Enums.TimePeriodType.Survey;
                    break;
                default: return false;
            }

            SqlCommand cmd = new SqlCommand("dbo.DeleteProgramInstanceImprovementType");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProgramId", programID);
            cmd.Parameters.AddWithValue("@TimePeriodId", GetYearId(timePeriod, timePeriodType));
            cmd.Parameters.AddWithValue("@ImprovementTypeId", improvementTypeId);

            try
            {
                if (this.ExecuteNonQuery(cmd) > 0)
                {
                    return true;
                };
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }

            return false;
        }

        /// <summary>
        /// Add a FundingResource to TimePeriod
        /// </summary>
        /// <param name="timePeriodId"></param>
        /// <param name="fundingResourceId"></param>
        public string AddFundingResourceToTimePeriod(int timePeriodId, int fundingResourceId)
        {
            string result = "";

            SqlCommand cmd = new SqlCommand("dbo.InsertFundingResourceIntoTimePeriod");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
            cmd.Parameters.AddWithValue("@FundingResourceId", fundingResourceId);

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
        /// Drop a FundingResource from TimePeriod.
        /// </summary>
        /// <param name="timePeriodId"></param>
        /// <param name="fundingResourceId"></param>
        /// <returns>if false, the agency sponsors projects thus can not be removed</returns>
        public bool DropFundingResourceFromTimePeriod(int timePeriodId, int fundingResourceId, Enums.ApplicationState appState)
        {
            string result = "";
            int programID = (int)appState;

            string sproc = String.Empty;
            switch (appState)
            {
                case Enums.ApplicationState.RTP:
                    return false;
                    //sproc = "[RTP].[DeleteSponsorOrganization]";
                    break;
                case Enums.ApplicationState.TIP:
                    return false;
                    //sproc = "[TIP].[DeleteSponsorOrganization]";
                    break;
                case Enums.ApplicationState.Survey:
                    sproc = "[Survey].[DeleteFundingResourceFromTimePeriod]";
                    break;
                default: return false;
            }

            SqlCommand cmd = new SqlCommand(sproc);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
            cmd.Parameters.AddWithValue("@FundingResourceId", fundingResourceId);

            try
            {
                if (this.ExecuteNonQuery(cmd) > 0)
                {
                    return true;
                };
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }

            return false;
        }
        
        /// <summary>
        /// Update an existing county share record (ProjectCountyGeography table)
        /// </summary>
        /// <param name="model"></param>
        public void UpdateCountyShare(CountyShareModel model)
        {
            //[TIP].[UpdateProjectCountyGeography]
            using (SqlCommand command = new SqlCommand("[dbo].[UpdateProjectCountyGeography]") { CommandType = CommandType.StoredProcedure })
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

            using (SqlCommand command = new SqlCommand("[dbo].[DropProjectCountyGeography]") { CommandType = CommandType.StoredProcedure })
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
            using (SqlCommand command = new SqlCommand("[dbo].[AddProjectMuniGeography]") { CommandType = CommandType.StoredProcedure })
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
            using (SqlCommand command = new SqlCommand("[dbo].[UpdateProjectMuniGeography]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTID", model.ProjectId);
                command.Parameters.AddWithValue("@MUNIID", model.MunicipalityId);
                command.Parameters.AddWithValue("@SHARE", model.Share);
                command.Parameters.AddWithValue("@PRIMARY", model.Primary);
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
            using (SqlCommand command = new SqlCommand("[dbo].[AddProjectCountyGeography]") { CommandType = CommandType.StoredProcedure })
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
        /// Drop a ProjectMuniGeography record
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="muniId"></param>
        public void DropMunicipalityShare(int projectId, int muniId)
        {
            //[TIP].[DropProjectMuniGeography]
            using (SqlCommand command = new SqlCommand("[dbo].[DropProjectMuniGeography]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTID", projectId);
                command.Parameters.AddWithValue("@MUNIID", muniId);
                this.ExecuteNonQuery(command);
            }
        }

        public void CreateFundingSource(FundingSourceModel model)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[CreateFundingSource]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FundingGroupId", model.FundingGroup.Id);
            cmd.Parameters.AddWithValue("@SourceAgencyId", model.SourceOrganization.OrganizationId);
            cmd.Parameters.AddWithValue("@RecipientAgencyId", model.RecipentOrganization.OrganizationId);
            cmd.Parameters.AddWithValue("@Descretion", model.IsDiscretionary);
            cmd.Parameters.AddWithValue("@ConformityImpact", model.IsConformityImpact);
            cmd.Parameters.AddWithValue("@ProgramId", model.ProgramId);
            cmd.Parameters.AddWithValue("@TimePeriodId", model.TimePeriodId);
            cmd.Parameters.AddWithValue("@FundingType", model.FundingType);
            cmd.Parameters.AddWithValue("@Code", model.Code);
            cmd.Parameters.AddWithValue("@IsState", model.IsState);
            cmd.Parameters.AddWithValue("@IsLocal", model.IsLocal);
            cmd.Parameters.AddWithValue("@IsFederal", model.IsFederal);
            this.ExecuteNonQuery(cmd);
        }

        public void UpdateFundingSource(FundingSourceModel model)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[UpdateFundingSource]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FundingTypeId", model.FundingTypeId);
            cmd.Parameters.AddWithValue("@FundingGroupId", model.FundingGroup.Id);
            cmd.Parameters.AddWithValue("@SourceAgencyId", model.SourceOrganization.OrganizationId);
            cmd.Parameters.AddWithValue("@RecipientAgencyId", model.RecipentOrganization.OrganizationId);
            cmd.Parameters.AddWithValue("@Descretion", model.IsDiscretionary);
            cmd.Parameters.AddWithValue("@ConformityImpact", model.IsConformityImpact);
            cmd.Parameters.AddWithValue("@ProgramId", model.ProgramId);
            cmd.Parameters.AddWithValue("@TimePeriodId", model.TimePeriodId);
            cmd.Parameters.AddWithValue("@FundingType", model.FundingType);
            cmd.Parameters.AddWithValue("@Code", model.Code);
            cmd.Parameters.AddWithValue("@IsState", model.IsState);
            cmd.Parameters.AddWithValue("@IsLocal", model.IsLocal);
            cmd.Parameters.AddWithValue("@IsFederal", model.IsFederal);
            this.ExecuteNonQuery(cmd);
        }

        public FundingSourceModel GetFundingSource(FundingSourceModel model)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[GetFundingSource]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FundingTypeId", model.FundingTypeId);
            cmd.Parameters.AddWithValue("@TimePeriodId", model.TimePeriodId);
            var dt = this.ExecuteDataTable(cmd);

            var dr = dt.Rows[0];

            model.FundingTypeId = (int)dr["FundingTypeID"];
            model.FundingType = dr["FundingType"].ToString();
            model.Code = dr["Code"].ToString();
            model.FundingGroup.Id = (int)dr["FundingGroupID"];
            model.FundingGroup.Name = dr["FundingGroup"].ToString();
            model.SourceOrganization.OrganizationId = (int)dr["SourceAgencyID"];
            model.SourceOrganization.OrganizationName = dr["SourceAgency"].ToString();
            model.RecipentOrganization.OrganizationId = (int)dr["RecipientAgencyID"];
            model.RecipentOrganization.OrganizationName = dr["RecipientAgency"].ToString();
            model.IsDiscretionary = dr["Discretion"].ToString().SmartParseDefault<bool>(default(bool));
            model.IsConformityImpact = dr["ConformityImpact"].ToString().SmartParseDefault<bool>(default(bool));
            model.IsLocal = dr["Local"].ToString().SmartParseDefault<bool>(default(bool));
            model.IsState = dr["State"].ToString().SmartParseDefault<bool>(default(bool));
            model.IsFederal = dr["Federal"].ToString().SmartParseDefault<bool>(default(bool));

            return model;
        }

        public ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId)
        {
            return GetCollectionOfCycles(timePeriodId, null);
        }

        public ICollection<CycleAmendment> GetCollectionOfCycles(int timePeriodId, Enums.RTPCycleStatus? status)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[GetCurrentPlanCycle]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
            if (status.HasValue)
            {
                cmd.Parameters.AddWithValue("@StatusId", (int)status);
            }


            var result = new List<CycleAmendment>();
            CycleAmendment cycle = null;

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                while (rdr.Read())
                {
                    cycle = new CycleAmendment()
                    {
                        Id = rdr["id"] != DBNull.Value ? rdr["id"].ToString().SmartParse<int>() : default(int)
                        ,
                        Name = rdr["cycle"].ToString()
                        ,
                        StatusId = rdr["statusId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        NextCycleId = rdr["nextCycleId"].ToString().SmartParseDefault<int>(default(int))
                        ,
                        NextCycleName = rdr["nextCycle"].ToString()
                        ,
                        NextCycleStatus = rdr["nextCycleStatus"].ToString()

                    };
                    cycle.Status = StringEnum.GetStringValue((Enums.RTPCycleStatus)cycle.StatusId);
                    result.Add(cycle);
                }
            }

            return result;
        }

        public CycleAmendment GetCurrentCycle(int timePeriodId)
        {
            var cycles = GetCollectionOfCycles(timePeriodId);

            var value = (CycleAmendment)cycles.SingleOrDefault(x => x.StatusId == (int)Enums.RTPCycleStatus.Active) ?? (CycleAmendment)cycles.SingleOrDefault(x => x.StatusId == (int)Enums.RTPCycleStatus.Pending);



            return value ?? new CycleAmendment();
        }

        public Cycle GetCycleDetails(int timePeriodId)
        {
            return GetCycleDetails(timePeriodId, Enums.RTPCycleStatus.Active);
        }

        public Cycle GetCycleDetails(int timePeriodId, Enums.RTPCycleStatus status)
        {
            SqlCommand cmd = new SqlCommand("[RTP].[GetCurrentPlanCycle]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
            cmd.Parameters.AddWithValue("@StatusId", (int)status);


            var result = new Cycle();

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                if (rdr.Read())
                {
                    result.Id = rdr["id"] != DBNull.Value ? rdr["id"].ToString().SmartParse<int>() : default(int);
                    result.Name = rdr["cycle"].ToString();
                    result.StatusId = rdr["statusId"].ToString().SmartParseDefault<int>(default(int));
                }
            }

            return result;
        }

        public string GetCurrentRtpPlanYear()
        {
            string planYear = String.Empty;

            SqlCommand cmd = new SqlCommand("[RTP].[GetCurrentPlanYear]");
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

        #endregion

        /// <summary>
        /// Get the municipality shares for a project
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public IList<MunicipalityShareModel> GetProjectMunicipalityShares(int projectVersionId)
        {
            IList<MunicipalityShareModel> result = new List<MunicipalityShareModel>();
            using (SqlCommand command = new SqlCommand("[dbo].[GetProjectMuniShares]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@PROJECTVERSIONID", projectVersionId);

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
            using (SqlCommand command = new SqlCommand("[dbo].[GetProjectCountyShares]") { CommandType = CommandType.StoredProcedure })
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

    }
}
