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
using System.Collections;
using DRCOG.Common.Services;
using DRCOG.Common.Services.Interfaces;
using System.Transactions;

namespace DRCOG.Data
{
    public class FileRepository : TransportationRepository, IFileRepositoryExtender
    {
        public FileRepository()
        {
        }

        #region Lookups
        
        #endregion

        #region GENERAL METHODS

        //public Guid Save(Image image)
        //{
        //    Guid retVal = default(Guid);

        //    IList<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@FileName", image.Name));
        //    //sqlParams.Add(new SqlParameter("@AlternateText", image.AlternateText));
        //    sqlParams.Add(new SqlParameter("@File", image.Data));
        //    //sqlParams.Add(new SqlParameter("@MediaType", image.ContentType)); //Not available/working in my code -DBD 12/31/2010
        //    sqlParams.Add(new SqlParameter("@MediaType", image.MediaType));

        //    SqlParameter outParm = new SqlParameter("@MediaId", SqlDbType.UniqueIdentifier);
        //    outParm.Direction = ParameterDirection.Output;
        //    sqlParams.Add(outParm);

        //    using (SqlCommand command = new SqlCommand("[dbo].[CreateMedia]") { CommandType = CommandType.StoredProcedure })
        //    {
        //        command.Parameters.AddRange(sqlParams.ToArray());

        //        try
        //        {
        //            this.ExecuteNonQuery(command);
        //            retVal = (Guid)command.Parameters["@MediaId"].Value;
        //        }
        //        catch (Exception exc)
        //        {
        //            //Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
        //            throw new Exception("Error uploading image.", exc);
        //        }
        //    }

        //    return retVal;
        //}

        public Image Load(int id)
        {
            Image image = null;

            using (SqlCommand command = new SqlCommand("[dbo].[GetMediaById]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@LocationMapId", id);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        image = new Image()
                        {
                            Name = rdr["fileName"].ToString()
                            ,
                            Data = (byte[])rdr["file"]
                            ,
                            MediaType = rdr["mediaType"].ToString()
                            ,
                            Guid = (Guid)rdr["mediaId"]
                        };
                    }
                }
            }

            return image;
        }

        public Guid Save(File file, int projectVersionId)
        {
            Guid guid = default(Guid);
            using (TransactionScope ts = new TransactionScope())
            {
                guid = this.Save(file);

                if (guid.Equals(default(Guid))) throw new NoNullAllowedException("File was not uploaded");

                IList<SqlParameter> sqlParams = new List<SqlParameter>();
                sqlParams.Add(new SqlParameter("@ProjectVersionId", projectVersionId));
                sqlParams.Add(new SqlParameter("@MediaId", guid));

                using (SqlCommand command = new SqlCommand("[dbo].[LinkProjectVersionMedia]") { CommandType = CommandType.StoredProcedure })
                {
                    command.Parameters.AddRange(sqlParams.ToArray());

                    try
                    {
                        this.ExecuteNonQuery(command);
                    }
                    catch (Exception exc)
                    {
                        //Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
                        throw new Exception("Error joining media to projectversion.", exc);
                    }
                }
                ts.Complete();
            }

            return guid;
        }

        #region IFileRepository Members

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid Save(File file)
        {
            Guid retVal = default(Guid);

            IList<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@FileName", file.Name));
            //sqlParams.Add(new SqlParameter("@AlternateText", image.AlternateText));
            sqlParams.Add(new SqlParameter("@File", file.Data));
            //sqlParams.Add(new SqlParameter("@MediaType", image.ContentType)); //Not available/working in my code -DBD 12/31/2010
            sqlParams.Add(new SqlParameter("@MediaType", file.MediaType));

            SqlParameter outParm = new SqlParameter("@MediaId", SqlDbType.UniqueIdentifier);
            outParm.Direction = ParameterDirection.Output;
            sqlParams.Add(outParm);

            using (SqlCommand command = new SqlCommand("[dbo].[CreateMedia]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddRange(sqlParams.ToArray());

                try
                {
                    this.ExecuteNonQuery(command);
                    retVal = (Guid)command.Parameters["@MediaId"].Value;

                }
                catch (Exception exc)
                {
                    //Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
                    throw new Exception("Error uploading image.", exc);
                }
            }

            return retVal;
        }

        public Image GetById(Guid id)
        {
            Image image = null;

            using (SqlCommand command = new SqlCommand("[dbo].[GetMediaById]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@MediaId", id);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        image = new Image()
                        {
                            Name = rdr["fileName"].ToString()
                            ,
                            Data = (byte[])rdr["file"]
                            ,
                            MediaType = rdr["mediaType"].ToString()
                            ,
                            Guid = (Guid)rdr["mediaId"]
                        };
                    }
                }
            }

            return image;
        }

        #endregion

        //public int Save(Image image)
        //{
        //    int retVal = default(int);

        //    IList<SqlParameter> sqlParams = new List<SqlParameter>();
        //    sqlParams.Add(new SqlParameter("@FileName", image.Name));
        //    sqlParams.Add(new SqlParameter("@AlternateText", image.AlternateText));
        //    sqlParams.Add(new SqlParameter("@Data", image.Data));
        //    sqlParams.Add(new SqlParameter("@DocumentTypeId", (int)Enums.DocumentType.LocationMap));
        //    sqlParams.Add(new SqlParameter("@ContentTypeId", (int)image.ContentTypeId));

        //    SqlParameter outParm = new SqlParameter("@DocumentId", SqlDbType.Int);
        //    outParm.Direction = ParameterDirection.Output;
        //    sqlParams.Add(outParm);

        //    using (SqlCommand command = new SqlCommand("[dbo].[SaveDocument]") { CommandType = CommandType.StoredProcedure })
        //    {
        //        command.Parameters.AddRange(sqlParams.ToArray());
                
        //        try
        //        {
        //            this.ExecuteNonQuery(command);
        //            retVal = (int)command.Parameters["@DocumentId"].Value;
        //        }
        //        catch (Exception exc)
        //        {
        //            //Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
        //            throw new Exception("Error uploading image.", exc);
        //        }
        //    }

        //    return retVal;
        //}

        

        //public void Delete(int id)
        //{
        //    using (SqlCommand cmd = new SqlCommand("[dbo].[DeleteDocument]") { CommandType = CommandType.StoredProcedure })
        //    {
        //        cmd.Parameters.AddWithValue("@DocumentId", id);

        //        try
        //        {
        //            this.ExecuteNonQuery(cmd);
        //        }
        //        catch (Exception exc)
        //        {
        //            Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
        //            throw new Exception("Error while deleting Image", exc);
        //        }
        //    }
        //}

        public void Delete(int id, int projectVersionId)
        {
            using (SqlCommand cmd = new SqlCommand("[dbo].[DeleteDocument]") { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.AddWithValue("@DocumentId", id);
                cmd.Parameters.AddWithValue("@ProjectVersionId", projectVersionId);

                try
                {
                    this.ExecuteNonQuery(cmd);
                }
                catch (Exception exc)
                {
                    //Elmah.ErrorSignal.FromCurrentContext().Raise(exc);
                    throw new Exception("Error while deleting Image", exc);
                }
            }
        }

        public int GetId(string filename)
        {
            int id = default(int);

            using (SqlCommand command = new SqlCommand("[dbo].[Temp_GetImageId]") { CommandType = CommandType.StoredProcedure })
            {
                command.Parameters.AddWithValue("@FileName", filename);

                using (IDataReader rdr = ExecuteReader(command))
                {
                    if (rdr.Read())
                    {
                        id = (int)rdr["DocumentID"];
                    }
                }
            }

            return id;
        }

        #endregion

    }
}
