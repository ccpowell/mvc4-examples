using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Security;
using System.Web.Hosting;
using System.Web.Profile;
using System.Web.Security;

namespace DRCOG.Domain.Security
{
     class DRCOGProfileProvider : ProfileProvider
    {
        private string _appName;
        private string _sqlConnectionString;
        private string _readSproc;
        private string _setSproc;
        private int _commandTimeout;

        #region Initializers

        public override void Initialize(string name, NameValueCollection config)
        {

            if (config == null)
                throw new ArgumentNullException("config");
            if (String.IsNullOrEmpty(name))
                name = "DRCOGProfileProvider";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "DRCOGProfileProvider");
            }
            base.Initialize(name, config);

            string temp = config["connectionStringName"];
            if (String.IsNullOrEmpty(temp))
                throw new ProviderException("connectionStringName not specified");
            _sqlConnectionString = GetConnectionString(temp);
            if (String.IsNullOrEmpty(_sqlConnectionString))
            {
                throw new ProviderException("connectionStringName not specified");
            }

            _appName = config["applicationName"];
            if (string.IsNullOrEmpty(_appName))
                _appName = GetDefaultAppName();

            if (_appName.Length > 256)
            {
                throw new ProviderException("Application name too long");
            }

            _setSproc = config["setProcedure"];
            if (String.IsNullOrEmpty(_setSproc))
            {
                throw new ProviderException("setProcedure not specified");
            }

            _readSproc = config["readProcedure"];
            if (String.IsNullOrEmpty(_readSproc))
            {
                throw new ProviderException("readProcedure not specified");
            }

            string timeout = config["commandTimeout"];
            if (string.IsNullOrEmpty(timeout) || !Int32.TryParse(timeout, out _commandTimeout))
            {
                _commandTimeout = 30;
            }

            config.Remove("commandTimeout");
            config.Remove("connectionStringName");
            config.Remove("applicationName");
            config.Remove("readProcedure");
            config.Remove("setProcedure");
            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException("Unrecognized config attribute:" + attribUnrecognized);
            }
        }

        internal static string GetDefaultAppName()
        {
            try
            {
                string appName = HostingEnvironment.ApplicationVirtualPath;
                if (String.IsNullOrEmpty(appName))
                {
                    appName = System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName;
                    int indexOfDot = appName.IndexOf('.');
                    if (indexOfDot != -1)
                    {
                        appName = appName.Remove(indexOfDot);
                    }
                }

                if (String.IsNullOrEmpty(appName))
                {
                    return "/";
                }
                else
                {
                    return appName;
                }
            }
            catch (SecurityException)
            {
                return "/";
            }
        }

        internal static string GetConnectionString(string specifiedConnectionString)
        {
            if (String.IsNullOrEmpty(specifiedConnectionString))
                return null;

            // Check <connectionStrings> config section for this connection string
            ConnectionStringSettings connObj = ConfigurationManager.ConnectionStrings[specifiedConnectionString];
            if (connObj != null)
                return connObj.ConnectionString;

            return null;
        }

        public override string ApplicationName
        {
            get { return _appName; }
            set
            {
                if (value == null) throw new ArgumentNullException("ApplicationName");
                if (value.Length > 256)
                {
                    throw new ProviderException("Application name too long");
                }
                _appName = value;

            }
        }

        private int CommandTimeout
        {
            get { return _commandTimeout; }
        }

        public static MembershipProvider GetMembershipProvider(string providerName)
        {
            if (string.IsNullOrEmpty(providerName))
            {
                return System.Web.Security.Membership.Provider;
            }
            MembershipProvider provider = System.Web.Security.Membership.Providers[providerName];
            if (provider == null)
            {
                throw new Exception("WebControl_CantFindProvider");
            }
            return provider;
        }

        #endregion

        #region GENERAL HELPER METHODS

        // Container struct for use in aggregating columns for queries
        private struct ProfileColumnData
        {
            public string VariableName;
            public SettingsPropertyValue PropertyValue;
            public object Value;
            public SqlDbType DataType;

            public ProfileColumnData(string var, SettingsPropertyValue pv, object val, SqlDbType type)
            {
                VariableName = var;
                PropertyValue = pv;
                Value = val;
                DataType = type;
            }
        }

        // Helper that just sets up the usual sproc sqlcommand parameters and adds the applicationname/username
        private SqlCommand CreateSprocSqlCommand(string sproc, SqlConnection conn, string username, bool isAnonymous)
        {
            SqlCommand cmd = new SqlCommand(sproc, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = CommandTimeout;
            cmd.Parameters.AddWithValue("@ApplicationName", ApplicationName);
            cmd.Parameters.AddWithValue("@Username", username);
            cmd.Parameters.AddWithValue("@IsUserAnonymous", isAnonymous);
            return cmd;
        }

        private static SqlParameter CreateOutputParam(string paramName, SqlDbType dbType, int size)
        {
            SqlParameter param = new SqlParameter(paramName, dbType);
            param.Direction = ParameterDirection.Output;
            param.Size = size;
            return param;
        }

        #endregion

        #region GET METHODS

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
        {
            SettingsPropertyValueCollection svc = new SettingsPropertyValueCollection();

            if (collection == null || collection.Count < 1 || context == null)
                return svc;

            string username = (string)context["UserName"];
            bool userIsAuthenticated = (bool)context["IsAuthenticated"];
            if (String.IsNullOrEmpty(username))
                return svc;

            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(_sqlConnectionString);
                conn.Open();

                GetProfileDataFromSproc(collection, svc, username, conn, userIsAuthenticated);
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }

            return svc;
        }

        // customProviderData = "Varname;SqlDbType;size"
        private void GetProfileDataFromSproc(SettingsPropertyCollection properties, SettingsPropertyValueCollection svc, string username, SqlConnection conn, bool userIsAuthenticated)
        {
            SqlCommand cmd = null;

            try
            {
                List<ProfileColumnData> columnData = new List<ProfileColumnData>(properties.Count);
                int i = 0;
                foreach (SettingsProperty prop in properties)
                {
                    SettingsPropertyValue value = new SettingsPropertyValue(prop);
                    svc.Add(value);

                    string persistenceData = prop.Attributes["CustomProviderData"] as string;
                    // If we can't find the table/column info we will ignore this data
                    if (String.IsNullOrEmpty(persistenceData))
                    {
                        // REVIEW: Perhaps we should throw instead?
                        continue;
                    }
                    string[] chunk = persistenceData.Split(new char[] { ';' });
                    if (chunk.Length != 3)
                    {
                        // REVIEW: Perhaps we should throw instead?
                        continue;
                    }
                    string varname = chunk[0];
                    // REVIEW: Should we ignore case?
                    string dataType = "string"; // chunk[1];
                    SqlDbType sqlDataType = (SqlDbType)Enum.Parse(typeof(SqlDbType), chunk[1], true);

                    int size = 0;
                    if (!Int32.TryParse(chunk[2], out size))
                    {
                        throw new ArgumentException("Unable to parse as integer: " + chunk[2]);
                    }

                    columnData.Add(new ProfileColumnData(varname, value, null /* not needed for get */, sqlDataType));

                    //Moved the create to here. -DBD 02/19/2010
                    cmd = null;
                    cmd = CreateSprocSqlCommand(_readSproc, conn, username, userIsAuthenticated);
                    cmd.Parameters.RemoveAt("@IsUserAnonymous"); //anonymous flag not needed on get
                    //Now add parameters for my sproc. -DBD
                    cmd.Parameters.AddWithValue("@PropertyName", varname);
                    cmd.Parameters.AddWithValue("@PropertyType", dataType);
                    //TODO:Add size as well?
                    cmd.Parameters.Add(CreateOutputParam("PropertyValue", sqlDataType, size));

                    //We must execute for each property
                    cmd.ExecuteNonQuery();
                    ProfileColumnData colData = columnData[i];
                    object val = cmd.Parameters["PropertyValue"].Value;
                    SettingsPropertyValue propValue = colData.PropertyValue;

                    //Only initialize a SettingsPropertyValue for non-null values
                    if (!(val is DBNull || val == null))
                    {
                        propValue.PropertyValue = val;
                        propValue.IsDirty = false;
                        propValue.Deserialized = true;
                    }
                    i++;
                }
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }

        #endregion

        #region UPDATE METHODS

        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
        {
            string username = (string)context["UserName"];
            bool userIsAuthenticated = (bool)context["IsAuthenticated"];

            if (username == null || username.Length < 1 || collection.Count < 1)
                return;

            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                bool anyItemsToSave = false;

                // First make sure we have at least one item to save
                foreach (SettingsPropertyValue pp in collection)
                {
                    if (pp.IsDirty)
                    {
                        if (!userIsAuthenticated)
                        {
                            bool allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                            if (!allowAnonymous)
                                continue;
                        }
                        anyItemsToSave = true;
                        break;
                    }
                }

                if (!anyItemsToSave)
                    return;

                conn = new SqlConnection(_sqlConnectionString);
                conn.Open();

                List<ProfileColumnData> columnData = new List<ProfileColumnData>(collection.Count);

                foreach (SettingsPropertyValue pp in collection)
                {
                    if (!userIsAuthenticated)
                    {
                        bool allowAnonymous = (bool)pp.Property.Attributes["AllowAnonymous"];
                        if (!allowAnonymous)
                            continue;
                    }

                    //if (!pp.IsDirty && pp.UsingDefaultValue) // Not fetched from DB and not written to
                    //    continue;

                    if (pp.IsDirty)
                    {
                        string persistenceData = pp.Property.Attributes["CustomProviderData"] as string;
                        // If we can't find the table/column info we will ignore this data
                        if (String.IsNullOrEmpty(persistenceData))
                        {
                            // REVIEW: Perhaps we should throw instead?
                            continue;
                        }
                        string[] chunk = persistenceData.Split(new char[] { ';' });
                        if (chunk.Length != 3)
                        {
                            // REVIEW: Perhaps we should throw instead?
                            continue;
                        }
                        string varname = chunk[0];
                        // REVIEW: Should we ignore case?
                        SqlDbType datatype = (SqlDbType)Enum.Parse(typeof(SqlDbType), chunk[1], true);
                        // chunk[2] = size, which we ignore

                        object value = null;

                        if (!pp.IsDirty && pp.UsingDefaultValue) // Not fetched from DB and not written to
                            value = DBNull.Value;
                        else if (pp.Deserialized && pp.PropertyValue == null)
                        { // value was explicitly set to null
                            value = DBNull.Value;
                        }
                        else
                        {
                            value = pp.PropertyValue;
                        }

                        // REVIEW: Might be able to ditch datatype
                        //columnData.Add(new ProfileColumnData(varname, pp, value, datatype));

                        // Running the command for every dirty/non-default parameter. -DBD 02/19/2010
                        cmd = CreateSprocSqlCommand(_setSproc, conn, username, userIsAuthenticated);
                        cmd.Parameters.AddWithValue("@PropertyName", varname);
                        //TODO:Add size as well?
                        cmd.Parameters.AddWithValue("@PropertyValue", value);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
                if (conn != null)
                    conn.Close();
            }
        }

        #endregion

        #region Mangement APIs from ProfileProvider class

        public override int DeleteProfiles(ProfileInfoCollection profiles)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        public override int DeleteProfiles(string[] usernames)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException("This method is not supported for this provider.");
        }

        #endregion

    }
}