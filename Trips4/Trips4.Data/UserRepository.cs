using System;
using DRCOG.Domain;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Helpers;
using System.Diagnostics;
using System.Data.SqlTypes;
using System.Configuration;
using DRCOG.Domain.CustomExceptions;
using DRCOG.Domain.Models;
using System.Web.Security;
using System.Data.Objects;

namespace Trips4.Data
{
    /// <summary>
    /// Handles user accounts using .net providers and EF. 
    /// </summary>
    /// <remarks>original is in DRCOG.Data</remarks>
    public class UserRepository : IUserRepositoryExtension
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public void AddUserToRole(string userName, string role)
        {
            Roles.AddUserToRole(userName, role);
        }

        private MembershipUser GetMembershipUserByName(string userName)
        {
            var found = Membership.FindUsersByName(userName);
            if (found.Count == 0)
            {
                return null;
            }
            if (found.Count > 1)
            {
                Logger.Error("Multiple users found with name {0}", userName);
                return null;
            }
            var users = new System.Web.Security.MembershipUser[1];
            found.CopyTo(users, 0);
            return users[0];
        }

        public Person GetUserByName(string userName, bool loadRoles)
        {
            var user = GetMembershipUserByName(userName);
            if (user == null)
            {
                throw new Exception("User not found " + userName);
            }
            return GetUserByID((Guid)user.ProviderUserKey, loadRoles);
        }

        public Person GetUserByID(Guid guid, bool loadRoles)
        {
            var person = new Person();

            // get Person data from Trips
            using (var tdb = new Trips4.Data.Models.TRIPSEntities())
            {
                var found = tdb.GetPersonById(null, guid).FirstOrDefault();
                if (found == null)
                {
                    Logger.Info("Person not found (so he cannot sponsor projects) " + guid.ToString());
                }
                else
                {
                    Debug.Assert(found.PersonGUID == guid);
                    person.profile.PersonID = found.PersonID;
                    person.HasProjects = found.SponsorsProject ?? false;
                    person.SponsorOrganizationId = found.SponsorOrganizationId ?? 0;
                    person.SponsorOrganizationName = found.SponsorOrganization;
                }
                if (person.HasProjects)
                {
                    person.SponsoredProjectVersionIds.AddRange(tdb.GetPersonsProjectVersionIds(person.profile.PersonID).Select(x => x.Value));
                }
            }

            // get profile data from Trips_User
            using (var tudb = new Trips4.Data.Models.TRIPS_UserEntities())
            {
                var found = tudb.GetUserById(guid).FirstOrDefault();
                if (found == null)
                {
                    throw new Exception("User not found " + guid.ToString());
                }
                Debug.Assert(guid == found.UserId.Value);

                person.profile.PersonGUID = found.UserId.Value;
                person.profile.FirstName = found.FirstName;
                person.profile.LastName = found.LastName;
                person.profile.Phone = found.PrimaryContact;
                person.profile.RecoveryEmail = found.LoweredEmail; // TODO: is this true?
                person.profile.UserName = found.UserName;
            }

            // get roles from RoleProvider
            if (loadRoles)
            {
                var roles = Roles.GetRolesForUser(person.profile.UserName);
                person.profile.Roles.AddRange(roles);
            }

            return person;
        }

        public Person GetUserByEmail(string emailAddress, bool loadRoles)
        {
            var found = Membership.FindUsersByEmail(emailAddress);
            if (found.Count == 0)
            {
                throw new Exception("User Email not found " + emailAddress);
            }
            if (found.Count > 1)
            {
                Logger.Error("Multiple users found with email address {0}", emailAddress);
                throw new Exception("Multiple users found with email address " + emailAddress);
            }
            var users = new System.Web.Security.MembershipUser[1];
            found.CopyTo(users, 0);
            var user = users[0];
            return GetUserByID((Guid)user.ProviderUserKey, loadRoles);
        }

        public void UpdateUserApproval(Guid userId, bool isApproved)
        {
            var found = Membership.GetUser(userId);
            if (found == null)
            {
                throw new Exception("User not found " + userId.ToString());
            }
            if (found.IsApproved != isApproved)
            {
                found.IsApproved = isApproved;
                Membership.UpdateUser(found);
            }
        }

        public void ChangePassword(Guid id, string oldPassword, string newPassword)
        {
            var found = Membership.GetUser(id);
            if (found == null)
            {
                throw new Exception("User not found " + id.ToString());
            }
            var result = found.ChangePassword(oldPassword, newPassword);
            if (!result)
            {
                throw new Exception("Failed to change password.");
            }
        }

        public string ResetPassword(Guid id)
        {
            var found = Membership.GetUser(id);
            if (found == null)
            {
                throw new Exception("User not found " + id.ToString());
            }
            return found.ResetPassword();
        }

        public bool ValidateUser(string userName, string password)
        {
            return Membership.ValidateUser(userName, password);
        }

        public bool CheckPersonHasProjects(Person person, int timePeriodId)
        {
            if (person.profile.PersonID <= 0)
            {
                // a non-person cannot sponsor a project
                return false;
            }

            using (var tdb = new Trips4.Data.Models.TRIPSEntities())
            {
                var found = tdb.CheckPersonSponsorsProject(person.profile.PersonID, null, timePeriodId).FirstOrDefault();
                if (found == null)
                {
                    throw new Exception("CheckPersonSponsorsProject did not return a result.");
                }
                return found.Value;
            }
        }

        [Obsolete]
        public void CreatePerson(ShortProfile profile)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public void ReplaceSponsor(Guid newSponsor, int currentSponsorId)
        {
            throw new NotImplementedException();
        }

        [Obsolete]
        public void LinkUserWithSponsor(ShortProfile profile)
        {
            throw new NotImplementedException();
        }

        public bool GetUserApproval(string userName)
        {
            var muser = GetMembershipUserByName(userName);
            if (muser == null)
            {
                throw new Exception("User not found " + userName);
            }
            return muser.IsApproved;
        }

        // admin and testing functions
        public void CreatePersonForUser(Guid guid, string sponsor)
        {
            string first = null;
            string last = null;
            using (var tudb = new Trips4.Data.Models.TRIPS_UserEntities())
            {
                var found = tudb.GetUserById(guid).FirstOrDefault();
                if (found == null)
                {
                    throw new Exception("User not found " + guid.ToString());
                }
                Debug.Assert(guid == found.UserId.Value);

                first = found.FirstName;
                last = found.LastName;
            }
            using (var tdb = new Trips4.Data.Models.TRIPSEntities())
            {
                var pid = new ObjectParameter("PersonID", 0);
                tdb.CreatePerson(guid, pid, first, last, sponsor);
            }
        }

        // TODO: put this into a class and cache the Property IDs before saving/fetching
        private void SavePropertyValue(Trips4.Data.Models.TRIPS_UserEntities db, Trips4.Data.Models.aspnet_Users user, string name, string value)
        {
            var pp = db.ProfileProperties.Single(p => p.ProfilePropertyName == name);
            if (string.IsNullOrWhiteSpace(value))
            {
                var ppv = user.ProfilePropertyValues.SingleOrDefault(p => p.ProfilePropertyID == pp.ProfilePropertyID);
                if (ppv != null)
                {
                    // TODO: cannot just remove it. Why?
                    ppv.ProfilePropertyValue1 = string.Empty;
                    ppv.DateUpdated = DateTime.Now;
                }
            }
            else
            {
                var ppv = user.ProfilePropertyValues.SingleOrDefault(p => p.ProfilePropertyID == pp.ProfilePropertyID);
                if (ppv != null)
                {
                    ppv.ProfilePropertyValue1 = value;
                    ppv.DateUpdated = DateTime.Now;
                }
                else
                {
                    user.ProfilePropertyValues.Add(new Trips4.Data.Models.ProfilePropertyValue()
                    {
                        ProfileProperty = pp,
                        ProfilePropertyValue1 = value,
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now
                    });
                }
            }
        }

        private void SavePropertyValues(Trips4.Data.Models.TRIPS_UserEntities db, Trips4.Data.Models.aspnet_Users user, ShortProfile profile)
        {
            SavePropertyValue(db, user, "LastName", profile.LastName);
            SavePropertyValue(db, user, "FirstName", profile.FirstName);
            SavePropertyValue(db, user, "PrimaryContact", profile.Phone);
            SavePropertyValue(db, user, "Organization", profile.Organization);
            SavePropertyValue(db, user, "Title", profile.Title);
        }

        private string GetPropertyValue(Trips4.Data.Models.TRIPS_UserEntities db, Trips4.Data.Models.aspnet_Users user, string name)
        {
            var pp = db.ProfileProperties.Single(p => p.ProfilePropertyName == name);
            var ppv = user.ProfilePropertyValues.SingleOrDefault(p => p.ProfilePropertyID == pp.ProfilePropertyID);
            if (ppv != null)
            {
                return ppv.ProfilePropertyValue1.Trim();
            }
            return null;
        }

        private void GetPropertyValues(Trips4.Data.Models.TRIPS_UserEntities db, Trips4.Data.Models.aspnet_Users user, ShortProfile profile)
        {
            profile.LastName = GetPropertyValue(db, user, "LastName");
            profile.FirstName = GetPropertyValue(db, user, "FirstName");
            profile.Phone = GetPropertyValue(db, user, "PrimaryContact");
            profile.Organization = GetPropertyValue(db, user, "Organization");
            profile.Title = GetPropertyValue(db, user, "Title");
        }


        public void CreateUserAndPerson(ShortProfile profile)
        {
            // the recovery email is required.
            if (string.IsNullOrWhiteSpace(profile.RecoveryEmail))
            {
                throw new Exception("Recovery Email is required.");
            }

            // unique user name is required. Hint: use RecoveryEmail.
            if (string.IsNullOrWhiteSpace(profile.UserName))
            {
                throw new Exception("UserName is required.");
            }

            // GUID must be, ummm, unique
            if (profile.PersonGUID.Equals(Guid.Empty))
            {
                profile.PersonGUID = Guid.NewGuid();
            }

            // use providers to create a user
            var password = "!!Test123";
            MembershipCreateStatus membershipStatus;
            var muser = Membership.CreateUser(profile.UserName, password, profile.RecoveryEmail,
                "What is the answer to the ultimate question?", "42", true, profile.PersonGUID, out membershipStatus);
            if (membershipStatus != MembershipCreateStatus.Success)
            {
                throw new Exception("Membership Creation failed: " + membershipStatus.ToString());
            }

            if (!Membership.ValidateUser(profile.UserName, password))
            {
                Logger.Warn("Cannot validate user " + profile.UserName);
            }

            // save extra properties
            using (var db = new Trips4.Data.Models.TRIPS_UserEntities())
            {
                // get the aspnet_User that was just created
                var user = db.aspnet_Users.Single(u => u.UserName == profile.UserName);
                SavePropertyValues(db, user, profile);
                db.SaveChanges();
            }

            // give the new user roles
            string[] roles = { "Contact Manager", "Contact", "Viewer", "SponsorRoleManager", "Administrator" };
            Roles.AddUserToRoles(profile.UserName, roles);

            using (var tdb = new Trips4.Data.Models.TRIPSEntities())
            {
                var pid = new ObjectParameter("PersonID", 0);
                tdb.CreatePerson(profile.PersonGUID, pid, profile.FirstName, profile.LastName, profile.SponsorCode);
                profile.PersonID = (int)pid.Value;
            }
        }
    }
}
