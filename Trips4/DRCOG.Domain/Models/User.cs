//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/2/2009 3:55:45 PM
// Description:
//
//======================================================
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace DRCOG.Domain.Models
{

        /// <summary>
        /// Represents a user in the system
        /// </summary>
        [DataContract]
        [Obsolete("Do not use",true)]
        public class User
        {

            public User()
            {
                // Assign empty list to all related object lists
                this.Roles = new List<Role>() as IList<Role>;
            }

            public User(string login, string password, string firstName, string lastName, string agencyName) : this(login, password, firstName, lastName, agencyName, true) { }

            public User(string login, string password, string firstName, string lastName, string agencyName, bool active) : this(login, password, firstName, lastName, agencyName, true, new List<Role>() as IList<Role>) { }

            public User(string login, string password, string firstName, string lastName, string agencyName, bool active, IList<Role> roles) : this(login, password, firstName, lastName, agencyName, true, roles, 0) { }

            public User(string login, string password, string firstName, string lastName, string agencyName, bool active, IList<Role> roles, int id)
            {
                this.Active = active;
                this.Agency = agencyName;
                this.FirstName = firstName;
                this.Id = id;
                this.LastLogin = null;
                this.LastName = lastName;
                this.Login = login;
                this.PasswordHash = password;
                this.Roles = roles ?? new List<Role>() as IList<Role>; // Null-Coalescing Operator         
            }

            [DataMember(Name = "userId")] //"id" appears to be a reserved word in the dojo grid.
            public int Id { get; set; }


            [DataMember(Name = "login")]
            public string Login { get; set; }


            [DataMember(Name = "passwordHash")]
            public string PasswordHash { get; set; }


            [DataMember(Name = "firstName")]
            public string FirstName { get; set; }

            [DataMember(Name = "lastName")]
            public string LastName { get; set; }


            [DataMember(Name = "agency")]
            public string Agency { get; set; }


            [DataMember(Name = "lastLogin")]
            public DateTime? LastLogin { get; set; }


            [DataMember(Name = "active")]
            public bool Active { get; set; }

            [DataMember(Name = "roles")]
            public IList<Role> Roles { get; set; }

        }

    }

