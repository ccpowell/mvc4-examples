using System;
using DRCOG.Domain;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Helpers;
using System.Diagnostics;
using System.Transactions;
using System.Data.SqlTypes;
using System.Configuration;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Domain.CustomExceptions;
using DRCOG.Domain.Models;
using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
using DRCOG.Common.Util;

namespace DRCOG.Data
{
    public class UserRepository : BaseRepository, IUserRepositoryExtension
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        //protected readonly Profile Profile;

        public UserRepository()
        {
        }

        public void LoadPerson(ref Person person)
        {

            SqlCommand cmd = new SqlCommand("[dbo].[GetPersonById]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PersonId", person.profile.PersonID.Equals(default(int)) ? (object)DBNull.Value : person.profile.PersonID);

            cmd.Parameters.AddWithValue("@PersonGuid", person.profile.PersonGUID.Equals(default(Guid)) ? (object)DBNull.Value : person.profile.PersonGUID);
            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                //be sure we got a reader                
                while (rdr.Read())
                {
                    person.profile.PersonGUID = rdr["PersonGUID"].ToString().SmartParseDefault<Guid>(default(Guid));
                    person.profile.PersonID = rdr["PersonID"].ToString().SmartParseDefault<int>(default(int));
                    person.HasProjects = rdr["SponsorsProject"].ToString().SmartParseDefault<bool>(default(bool));
                    person.SponsorOrganizationId = rdr["SponsorOrganizationId"].ToString().SmartParseDefault<int>(default(int));
                    person.SponsorOrganizationName = rdr["SponsorOrganization"].ToString();
                }
            }

            if (person.HasProjects)
            {
                person.SponsoredProjectVersionIds = this.SponsoredProjectVersionIds(person.profile.PersonID);
            }

            if (person.profile.PersonID.Equals(default(int)) && !person.profile.PersonGUID.Equals(default(Guid)))
            {
                CreatePerson(ref person.profile);
            }
        }

        public bool CheckPersonHasProjects(Person person, int timePeriodId)
        {
            if (person != null)
            {
                SqlCommand cmd = new SqlCommand("[dbo].[CheckPersonSponsorsProject]");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PersonId", person.profile.PersonID.Equals(default(int)) ? (object)DBNull.Value : person.profile.PersonID);
                cmd.Parameters.AddWithValue("@PersonGuid", person.profile.PersonGUID.Equals(default(Guid)) ? (object)DBNull.Value : person.profile.PersonGUID);
                if (!timePeriodId.Equals(default(int))) cmd.Parameters.AddWithValue("@TimePeriodId", timePeriodId);
                using (IDataReader rdr = this.ExecuteReader(cmd))
                {
                    //be sure we got a reader                
                    while (rdr.Read())
                    {
                        person.HasProjects = rdr["SponsorsProject"].ToString().SmartParseDefault<bool>(default(bool));
                    }
                }

                return person.HasProjects;
            }

            return false;
        }



        public List<int> SponsoredProjectVersionIds(int personId)
        {
            List<int> list = new List<int>();

            SqlCommand cmd = new SqlCommand("[Survey].[GetPersonsProjectVersionIds]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PersonId", personId);

            using (IDataReader rdr = this.ExecuteReader(cmd))
            {
                //be sure we got a reader                
                while (rdr.Read())
                {
                    list.Add(rdr["ProjectVersionID"].ToString().SmartParseDefault<int>(default(int)));
                }
            }

            return list;
        }


        public void CreatePerson(ref Profile profile)
        {
            string sqlConn = ConfigurationManager.ConnectionStrings["DRCOG"].ToString();
            using (SqlConnection conn = new SqlConnection(sqlConn))
            {
                conn.Open();

                //Save Person information (UserInfo & Address)

                using (SqlCommand cmd = new SqlCommand("[dbo].[CreatePerson]", conn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.AddWithValue("@PersonGUID", profile.PersonGUID);
                    cmd.Parameters.AddWithValue("@FirstName", profile.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", profile.LastName);
                    if (!String.IsNullOrEmpty(profile.SponsorCode))
                    {
                        cmd.Parameters.AddWithValue("@SponsorCode", profile.SponsorCode);
                    }

                    SqlParameter param = new SqlParameter("@PersonID", SqlDbType.Int);
                    param.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(param);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected < 1)
                        {
                            profile.Success = false;
                            throw new Exception("Save to Person table was not successful");
                        }
                        profile.Success = true;
                        profile.PersonID = (int)cmd.Parameters["@PersonID"].Value;
                    }
                    catch (Exception exc)
                    {
                        Logger.WarnException("Failed to CreatePerson", exc);
                    }

                    // TODO: connect sponsor
                }
            }
        }

        public void ReplaceSponsor(Guid newSponsor, int currentSponsorId)
        {
            throw new NotImplementedException();
        }

        public void LinkUserWithSponsor(Profile profile)
        {
            SqlCommand cmd = new SqlCommand("[dbo].[LinkUserWithSponsor]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PersonGUID", profile.PersonGUID);
            cmd.Parameters.AddWithValue("@SponsorKey", profile.SponsorCode);


            SqlParameter param = new SqlParameter("@PersonID", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            try
            {
                this.ExecuteNonQuery(cmd);
                //if (rowsAffected < 1) { profile.Success = false; throw new UserException("User join was not successful, Sponsor code is invalid or already used"); }
                profile.Success = true;
                //profile.PersonID = (int)cmd.Parameters["@PersonID"].Value;
            }
            catch (Exception exc)
            {
                Logger.WarnException("Failed to LinkUserWithSponsor", exc);
            }
        }

        public bool GetUserApproval(string userName)
        {
            bool retval = false;

            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["GlobalUser"].ConnectionString))
                {
                    conn.Open();

                    string sqlCmd = "SELECT m.IsApproved FROM dbo.aspnet_Users u INNER JOIN dbo.aspnet_Membership m ON m.UserId = u.UserId WHERE u.LoweredUserName = @LoweredUserName";
                    using (SqlCommand cmd = new SqlCommand(sqlCmd, conn) { CommandType = CommandType.Text })
                    {
                        cmd.Parameters.AddWithValue("@LoweredUserName", userName.ToLower());
                        retval = (bool)cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.WarnException("Failed to GetUserApproval", exc);
            }

            return retval;
        }

        public List<Common.Services.MemberShipServiceSupport.Domain.MembershipApplication> GetMembershipApplications()
        {
            throw new NotImplementedException();
        }

        public Common.Services.MemberShipServiceSupport.Domain.MembershipApplication GetMembershipApplication(Guid applicationID)
        {
            throw new NotImplementedException();
        }

        public List<ProfilePropertyValue> GetProfilePropertyValues(Guid userID)
        {
            List<ProfilePropertyValue> list = new List<ProfilePropertyValue>();
            ProfilePropertyValue item;
            DataTable dt = new DataTable();

            SqlCommand cmd = new SqlCommand("[dbo].[lsp_GetAllCustomProfileData]");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@showUnused", System.DBNull.Value);

            try
            {
                dt = this.ExecuteDataTable(cmd);

                foreach (DataRow dr in dt.Rows)
                {
                    item = new ProfilePropertyValue();
                    item.ID = dr[3] != DBNull.Value ? (int?)dr[3] : 0;
                    item.PropertyName = dr[2] != DBNull.Value ? dr[2].ToString() : string.Empty;
                    item.PropertyValue = dr[4] != DBNull.Value ? dr[4].ToString() : string.Empty;
                    item.PropertyID = dr[1] != DBNull.Value ? (int)dr[1] : 0;
                    item.IsUnused = (bool)dr[5];
                    list.Add(item);
                }

                return list;
            }
            catch (Exception exc)
            {
                Logger.WarnException("Failed to GetProfilePropertyValues", exc);
            }
            return list;
        }

        public List<ProfileProperty> GetProfileProperties()
        {
            throw new NotImplementedException();
        }

        public List<ProfileProperty> GetProfileProperties(Guid userID)
        {
            throw new NotImplementedException();
        }

        public int CreateProfileProperty(string propertyName, out string response)
        {
            throw new NotImplementedException();
        }

        public int SaveProfilePropertyValue(Guid userID, int propertyID, string propertyValue, out string propertyName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProfilePropertyValue(Guid userID, int propertyID, string propertyValue)
        {
            throw new NotImplementedException();
        }

        public void DeleteProfilePropertyValue(int profilePropertyValueID)
        {
            throw new NotImplementedException();
        }

        public int CreateProfileProperty(string propertyName, ProfilePropertyType type, out string response)
        {
            throw new NotImplementedException();
        }


        public void RefreshUserCache()
        {
            Logger.Info("RefreshUserCache ignored");
        }
        public void RefreshUserCache(string userName)
        {
            Logger.Info("RefreshUserCache ignored");
        }

        public void RefreshUserCache(string userName, bool delete)
        {
            Logger.Info("RefreshUserCache ignored");
        }




        public Guid GetProfilePropertyUserGuid_ByValue(string propertyValue, string propertyName)
        {
            throw new NotImplementedException();
        }

        public Guid GetProfilePropertyUserGuid_ByValue(string propertyValue, int propertyId)
        {
            throw new NotImplementedException();
        }

        public Guid GetProfilePropertyUserGuid_ByValue(string propertyValue, string propertyName, int propertyId)
        {
            throw new NotImplementedException();
        }

        public List<Profile> GetMembershipUsers(Guid membershipUserId, string[] applications, bool includeMembershipApp)
        {
            throw new NotImplementedException();
        }

        public List<Profile> GetMembershipUsers(Guid membershipUserId, string[] applications, string[] roleIsolators, bool includeMembershipApp)
        {
            throw new NotImplementedException();
        }

        public List<Profile> GetMembershipUsers(Guid membershipUserId, Dictionary<string, object> parameters, bool includeMembershipApp)
        {
            throw new NotImplementedException();
        }

        public List<Profile> GetMembershipUsers(Guid membershipUserId)
        {
            throw new NotImplementedException();
        }


        public List<string> GetMembershipUserApplications(Guid membershipUserId, bool includeMembershipApp)
        {
            throw new NotImplementedException();
        }



    }
}
