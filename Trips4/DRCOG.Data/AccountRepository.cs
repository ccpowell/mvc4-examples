#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			        REMARKS
 * 08/04/2009	Nick Kirkes             1. Initial Creation (DTS). 
 * 01/25/2010	Danny Davidson	2. Reformatted.
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

namespace DRCOG.Data
{
    /// <summary>
    /// Account Repository
    /// </summary>
    public class AccountRepository : BaseRepository, IAccountRepository
    {


       
        ///// <summary>
        ///// Retreives an account by the ID
        ///// </summary>
        ///// <param name="accountId"></param>
        ///// <returns></returns>
        //public Account GetAccountById(int accountId)
        //{
        //    SqlCommand cmd = new SqlCommand("[dbo].[GetPersonById]");
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@PERSONID", SqlDbType.Int));
        //    cmd.Parameters[0].Value = accountId;         

        //    using (IDataReader rdr = this.ExecuteReader(cmd))
        //    {
        //        //be sure we got a reader                
        //        if (rdr.Read())
        //        {
        //            Account account = new Account();
        //            account.AccountId = (int)rdr["PersonId"];
        //            account.FirstName = rdr["FirstName"].ToString();
        //            account.LastName = rdr["LastName"].ToString();
        //            account.Login = rdr["Address"].ToString();
        //            account.PasswordHash = rdr["Password"].ToString();
        //            account.Roles = this.GetRolesForAccount(accountId);
        //            return account;
        //        }
        //        else
        //        {
        //            //No rows returned, auth failed
        //            return null;
        //        }
        //    }
        //}

        ///// <summary>
        ///// Authorize a user
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //public Account AuthorizeUser(string username, string password)
        //{
        //    using (SqlCommand cmd = new SqlCommand("[dbo].[AuthenticateLogin]"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@LOGIN", SqlDbType.NVarChar));
        //        cmd.Parameters[0].Value = username;
        //        cmd.Parameters.Add(new SqlParameter("@HASHEDPWD", SqlDbType.NVarChar));
        //        cmd.Parameters[1].Value = password;
        //        using (IDataReader rdr = this.ExecuteReader(cmd))
        //        {
        //            //be sure we got a reader                
        //            if (rdr.Read())
        //            {
        //                Account account = new Account();
        //                account.AccountId = (int)rdr["PersonId"];
        //                account.FirstName = rdr["FirstName"].ToString();
        //                account.LastName = rdr["LastName"].ToString();
        //                account.OrganizationName = rdr["OrganizationName"].ToString();
        //                account.Login = username;
        //                account.PasswordHash = password;
        //                account.Roles = this.GetRolesForAccount(account.AccountId);
        //                return account;
        //            }
        //            else
        //                //No rows returned, auth failed
        //                return null;
        //        }
        //    }
        //}


        ///// <summary>
        ///// Get a list of Roles for an account
        ///// </summary>
        ///// <param name="accountId"></param>
        ///// <returns></returns>
        //public IList<Role> GetRolesForAccount(int accountId)
        //{
        //    using (SqlCommand cmd = new SqlCommand("[dbo].[GetRolesForAccount]"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@PERSONID", SqlDbType.Int));
        //        cmd.Parameters[0].Value = accountId;
        //        IList<Role> roles = new List<Role>();
        //        using (IDataReader rdr = this.ExecuteReader(cmd))
        //        {
        //            //be sure we got a reader                
        //            if (rdr.Read())
        //            {
        //                Role r = new Role();
        //                r.Name = rdr["Role"].ToString();
        //                r.RoleId = (int)rdr["RoleId"];
        //                roles.Add(r);
        //            }
        //        }
        //        return roles;
        //    }
        //}


        ///// <summary>
        ///// Change the password for an account. Assumes the password is already hashed
        ///// </summary>
        ///// <param name="accountId"></param>
        ///// <param name="newPassword"></param>
        //public void ChangePassword(int accountId, string newPassword)
        //{
        //    using (SqlCommand cmd = new SqlCommand("[TIP].[ChangePassword]"))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add(new SqlParameter("@PERSONID", SqlDbType.Int));
        //        cmd.Parameters[0].Value = accountId;
        //        cmd.Parameters.Add(new SqlParameter("@HASHEDPWD", SqlDbType.NVarChar));
        //        cmd.Parameters[1].Value = newPassword;
        //        this.ExecuteNonQuery(cmd);
        //    }
        //}

        /////// <summary>
        /////// Update an existing account. We only handle very simple stuff
        /////// at this point. Will need to be extended on further contracts
        /////// </summary>
        /////// <param name="account"></param>
        ////public void UpdateAccount(Account account)
        ////{

        ////    SqlCommand cmd = new SqlCommand("[TIP].[ChangePassword]");
            
        ////    cmd.CommandType = CommandType.StoredProcedure;
            
        ////    cmd.Parameters.Add(new SqlParameter("@PERSONID", SqlDbType.Int));
        ////    cmd.Parameters[0].Value = account.AccountId;
        ////    cmd.Parameters.Add(new SqlParameter("@PASSWORD", SqlDbType.NVarChar));
        ////    cmd.Parameters[1].Value = account.PasswordHash;
        ////    cmd.Parameters.Add(new SqlParameter("@FIRSTNAME", SqlDbType.NVarChar));
        ////    cmd.Parameters[2].Value = account.FirstName;
        ////    cmd.Parameters.Add(new SqlParameter("@LASTNAME", SqlDbType.NVarChar));
        ////    cmd.Parameters[3].Value = account.LastName;

        ////    //Do the update
        ////    this.ExecuteNonQuery(cmd);

        ////    //For TIP Internal, the roles are not editable

        ////}

        ////A bunch of Account stuff was stubbed out during TIP Internal - they are left as notimplemented

        //public string ResetPassword(int accountId)
        //{
        //    throw new NotImplementedException();
        //}

        //public IList<Role> GetRoles()
        //{
        //    throw new NotImplementedException();
        //}

        //public Account GetUserById(int userId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Role GetRoleById(int roleId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Account GetUser(string username, string password)
        //{
        //    throw new NotImplementedException();
        //}

        //public IList<Account> SearchForAccounts(string search_type, string search_FirstName, string search_LastName, string search_email)
        //{
        //    throw new NotImplementedException();
        //}

        //public Account GetAccountBy(string email)
        //{
        //    using (SqlCommand cmd = new SqlCommand("[dbo].[GetAccountByEmail]") { CommandType = CommandType.StoredProcedure })
        //    {
        //        cmd.Parameters.Add(new SqlParameter("@LOGIN", SqlDbType.NVarChar));
        //        cmd.Parameters["@LOGIN"].Value = email;
        //        using (IDataReader rdr = ExecuteReader(cmd))
        //        {
        //            //be sure we got a reader                
        //            if (rdr.Read())
        //            {
        //                Account account = new Account();
        //                account.AccountId = (int)rdr["PersonId"];
        //                account.FirstName = rdr["FirstName"].ToString();
        //                account.LastName = rdr["LastName"].ToString();
        //                account.OrganizationName = rdr["OrganizationName"].ToString();
        //                return account;
        //            }
        //            else
        //                //No rows returned, couldn't find account
        //                return null;
        //        }
        //    }
        //}
    }
}
