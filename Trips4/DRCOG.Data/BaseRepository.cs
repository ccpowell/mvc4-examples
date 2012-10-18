#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			REMARKS
 * 08/05/2009	DBouwman        1. Initial Creation (DTS).
 * 01/26/2010	DDavidson	    2. Reformatted. Moved public collection helpers to this class.
 * 02/17/2010   DDavidson       3. Added Single lookups (from TIP Repository) here for derived class use.
 * 03/18/2010   DTucker         4. Added ExecuteDataTable
 * 
 * DESCRIPTION:
 * Base implementation of undifferentiated repository for 
 * general data access and lookup collections.
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using DRCOG.Domain.Interfaces;

namespace DRCOG.Data
{
    public class BaseRepository : IBaseRepository
    {
        private const string _conStrName = "DRCOG"; 
        private string _conStr = null;

        public BaseRepository()
        {
            _conStr = ConfigurationManager.ConnectionStrings[_conStrName].ConnectionString;
        }

        /// <summary>
        /// Connection string used to access the database
        /// </summary>
        protected string ConnectionString {get{return _conStr;}}

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        /// <returns></returns>
        protected IDataReader ExecuteReader(SqlCommand cmd)
        {
            IDataReader rdr = null;

            if (String.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentNullException("_conStrName", "Sql connection string value for key " + _conStrName + " in App.config is null or empty.");
            }

            SqlConnection con = new SqlConnection(this.ConnectionString);

            if (con != null)
            {
                cmd.Connection = con;
                con.Open();
                
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }

            return rdr;
        }

        protected int ExecuteNonQuery(SqlCommand cmd)
        {
            int rowsAffected = default(int);

            if (String.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentNullException("_conStrName", "Sql connection string value for key " + _conStrName + " in App.config is null or empty.");
            }

            SqlConnection con = new SqlConnection(this.ConnectionString);

            if (con != null)
            {
                cmd.Connection = con;
                con.Open();
                rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
            }

            return rowsAffected;
        }

        protected T ExecuteScalar<T>(SqlCommand cmd)
        {
            if (String.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentNullException("_conStrName", "Sql connection string value for key " + _conStrName + " in App.config is null or empty.");
            }
            SqlConnection con = new SqlConnection(this.ConnectionString);
            cmd.Connection = con;
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                object retVal = cmd.ExecuteScalar();

                if (retVal is T)
                    return (T)retVal;
                else if (retVal == DBNull.Value)
                    return default(T);
                else
                    throw new Exception("Object returned was of the wrong type.");
            }
            catch (Exception DbEx)
            {
                throw DbEx;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable ExecuteDataTable(SqlCommand cmd)
        {
            if (String.IsNullOrEmpty(this.ConnectionString))
            {
                throw new ArgumentNullException("_conStrName", "Sql connection string value for key " + _conStrName + " in App.config is null or empty.");
            }

            DataTable table = null;
            SqlConnection con = new SqlConnection(this.ConnectionString);

            if (con != null)
            {
                cmd.Connection = con;
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    table = new DataTable();
                    table.Load(reader);
                }
            }
            return table;
        }

#region PUBLIC HELPERS

        public IDictionary<int, string> GetLookupCollection(string sprocName, string keyFieldName, string valueFieldName)
        {
            return GetLookupCollection(sprocName, keyFieldName, valueFieldName, null);
        }

        //public IList<string> GetLookupCollection(string sprocName, string valueFieldName)
        //{
        //    return GetLookupCollection(sprocName, valueFieldName, null);
        //}

        public T GetLookupSingle<T>(string sprocName, string keyFieldName, IList<SqlParameter> sqlParameters)
            where T : IConvertible
        {
            if (String.IsNullOrEmpty(keyFieldName))
                throw new ArgumentException("keyFieldName is null or empty.", "keyFieldName");
            if (String.IsNullOrEmpty(sprocName))
                throw new ArgumentException("sprocName is null or empty.", "sprocName");

            T result;

            using (SqlCommand command = new SqlCommand(sprocName))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter p in sqlParameters)
                        command.Parameters.Add(p);
                }

                result = ExecuteScalar<T>(command);
            }

            return result;
        }

        /// <summary>
        /// Get a lookup collection from the database
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="keyFieldName"></param>
        /// <param name="valueFieldName"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public IDictionary<int, string> GetLookupCollection(string sprocName, string keyFieldName, string valueFieldName, IList<SqlParameter> sqlParameters)
        {
            if (String.IsNullOrEmpty(keyFieldName))
                throw new ArgumentException("keyFieldName is null or empty.", "keyFieldName");
            //if (String.IsNullOrEmpty(valueFieldName))
            //    throw new ArgumentException("valueFieldName is null or empty.", "valueFieldName");
            if (String.IsNullOrEmpty(sprocName))
                throw new ArgumentException("sprocName is null or empty.", "sprocName");

            var result = new Dictionary<int, string>();

            using (SqlCommand command = new SqlCommand(sprocName))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter p in sqlParameters)
                        command.Parameters.Add(p);
                }

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        result.Add(Convert.ToInt32(rdr[keyFieldName]), rdr[valueFieldName].ToString());
                    }
                }
            }

            return result;
        }

        public IList<string> GetLookupCollection(string sprocName, string valueFieldName)
        {
            return GetLookupCollection(sprocName, valueFieldName, new List<SqlParameter>());
        }

        /// <summary>
        /// Get a lookup collection from the database
        /// </summary>
        /// <param name="sprocName"></param>
        /// <param name="keyFieldName"></param>
        /// <param name="valueFieldName"></param>
        /// <param name="sqlParameters"></param>
        /// <returns></returns>
        public IList<string> GetLookupCollection(string sprocName, string valueFieldName, IList<SqlParameter> sqlParameters)
        {
            if (String.IsNullOrEmpty(valueFieldName))
                throw new ArgumentException("valueFieldName is null or empty.", "valueFieldName");
            if (String.IsNullOrEmpty(sprocName))
                throw new ArgumentException("sprocName is null or empty.", "sprocName");

            var result = new List<string>();

            using (SqlCommand command = new SqlCommand(sprocName))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter p in sqlParameters)
                        command.Parameters.Add(p);
                }

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        result.Add(rdr[valueFieldName].ToString());
                    }
                }
            }

            return result;
        }


        public IDictionary<int, string> GetLookupCollection(string sprocName, string keyFieldName, string valueFieldName, IList<SqlParameter> sqlParameters, IDictionary<string, string> valueAddonValueSeparator)
        {
            if (String.IsNullOrEmpty(keyFieldName))
                throw new ArgumentException("keyFieldName is null or empty.", "keyFieldName");
            if (String.IsNullOrEmpty(valueFieldName))
                throw new ArgumentException("valueFieldName is null or empty.", "valueFieldName");
            if (String.IsNullOrEmpty(sprocName))
                throw new ArgumentException("sprocName is null or empty.", "sprocName");

            var result = new Dictionary<int, string>();

            using (SqlCommand command = new SqlCommand(sprocName))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (sqlParameters != null)
                {
                    foreach (SqlParameter p in sqlParameters)
                        command.Parameters.Add(p);
                }

                using (IDataReader rdr = ExecuteReader(command))
                {
                    while (rdr.Read())
                    {
                        string plusValue = String.Empty;
                        foreach (var pair in valueAddonValueSeparator)
                        {
                            if(!String.IsNullOrEmpty(rdr[pair.Key].ToString()))
                            {
                                plusValue += pair.Value + rdr[pair.Key].ToString();
                            }
                        }
                        result.Add(Convert.ToInt32(rdr[keyFieldName]), rdr[valueFieldName].ToString() + plusValue);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Adds parameters to the command object.
        /// </summary>
        /// <param name="command">The command to which the parameters will be added.</param>
        /// <param name="parameters">The parameters to add.</param>
        protected static void AddParameters(IDbCommand command, IEnumerable<KeyValuePair<string, object>> parameters)
        {
            if (parameters != null)
            {
                foreach (var pair in parameters)
                {
                    IDbDataParameter parameter;
                    if (pair.Value is IDbDataParameter)
                    {
                        parameter = pair.Value as IDbDataParameter;
                    }
                    else
                    {
                        parameter = command.CreateParameter();

                        parameter.ParameterName = pair.Key;
                        //Required to properly handle null values for dates and various other nullable
                        //fields so that they do not default to database default values.
                        if (pair.Value == null || DBNull.Value.Equals(pair.Value))
                        {
                            parameter.Value = DBNull.Value;
                        }
                        else
                        {
                            parameter.Value = pair.Value;
                        }
                    }
                    command.Parameters.Add(parameter);
                }
            }
        }


#endregion

    }
}
