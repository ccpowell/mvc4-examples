using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace ConRepo
{
    public class ContactsRepository
    {
        private Castle.Core.Logging.ILogger Logger { get; set; }

        public ContactsRepository(Castle.Core.Logging.ILogger logger)
        {
            Logger = logger;
        }

        // TODO: put this into a class and cache the Property IDs before saving/fetching
        private void SavePropertyValue(TRIPS_UserEntities db, aspnet_Users user, string name, string value)
        {
            var pp = db.ProfileProperties.Single(p => p.ProfilePropertyName == name);
            if (string.IsNullOrWhiteSpace(value))
            {
                var ppv = user.ProfilePropertyValues.SingleOrDefault(p => p.ProfilePropertyID == pp.ProfilePropertyID);
                if (ppv != null)
                {
                    user.ProfilePropertyValues.Remove(ppv);
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
                    user.ProfilePropertyValues.Add(new ProfilePropertyValue()
                    {
                        ProfileProperty = pp,
                        ProfilePropertyValue1 = value,
                        DateCreated = DateTime.Now,
                        DateUpdated = DateTime.Now
                    });
                }
            }
        }

        private void SavePropertyValues(TRIPS_UserEntities db, aspnet_Users user, ConModels.User profile)
        {
            SavePropertyValue(db, user, "LastName", profile.LastName);
            SavePropertyValue(db, user, "FirstName", profile.FirstName);
            SavePropertyValue(db, user, "BusinessEmailAddress", profile.BusinessEmail);
            SavePropertyValue(db, user, "HomeEmailAddress", profile.HomeEmail);
            SavePropertyValue(db, user, "AlternateEmailAddress", profile.AlternateEmail);
            SavePropertyValue(db, user, "Comment", profile.Comment);
            SavePropertyValue(db, user, "PrimaryContact", profile.Phone);
            SavePropertyValue(db, user, "Organization", profile.Organization);
            SavePropertyValue(db, user, "Title", profile.Title);
        }

        private string GetPropertyValue(TRIPS_UserEntities db, aspnet_Users user, string name)
        {
            var pp = db.ProfileProperties.Single(p => p.ProfilePropertyName == name);
            var ppv = user.ProfilePropertyValues.SingleOrDefault(p => p.ProfilePropertyID == pp.ProfilePropertyID);
            if (ppv != null)
            {
                return ppv.ProfilePropertyValue1.Trim();
            }
            return null;
        }

        private void GetPropertyValues(TRIPS_UserEntities db, aspnet_Users user, ConModels.User profile)
        {
            profile.LastName = GetPropertyValue(db, user, "LastName");
            profile.FirstName = GetPropertyValue(db, user, "FirstName");
            profile.BusinessEmail = GetPropertyValue(db, user, "BusinessEmailAddress");
            profile.HomeEmail = GetPropertyValue(db, user, "HomeEmailAddress");
            profile.AlternateEmail = GetPropertyValue(db, user, "AlternateEmailAddress");
            profile.Comment = GetPropertyValue(db, user, "Comment");
            profile.Phone = GetPropertyValue(db, user, "PrimaryContact");
            profile.Organization = GetPropertyValue(db, user, "Organization");
            profile.Title = GetPropertyValue(db, user, "Title");
        }


        public Guid CreateUser(ConModels.User profile)
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
            if (profile.Id.Equals(Guid.Empty))
            {
                profile.Id = Guid.NewGuid();
            }

            // use providers to create a user
            var password = "test123";
            MembershipCreateStatus membershipStatus;
            Membership.CreateUser(profile.UserName, password, profile.RecoveryEmail,
                null, null, true, profile.Id, out membershipStatus);
            if (membershipStatus != MembershipCreateStatus.Success)
            {

                throw new Exception("Membership Creation failed: " + membershipStatus.ToString());
            }

            if (!Membership.ValidateUser(profile.UserName, password))
            {
                Logger.Warn("Cannot validate user " + profile.UserName);
            }

            // save extra properties
            using (var db = new TRIPS_UserEntities())
            {
                // get the aspnet_User that was just created
                var user = db.aspnet_Users.Single(u => u.UserName == profile.UserName);
                SavePropertyValues(db, user, profile);
                db.SaveChanges();
            }

            // give the new user roles
            string[] roles = { "Contact Manager", "Contact", "Viewer", "SponsorRoleManager" };
            Roles.AddUserToRoles(profile.UserName, roles);

            // return ID of new user
            return profile.Id;
        }


        private ConModels.User GetUserModel(TRIPS_UserEntities db, aspnet_Users au)
        {
            string email = null;
            if (au.aspnet_Membership != null)
            {
                email = au.aspnet_Membership.Email;
            }
            var user = new ConModels.User()
            {
                Id = au.UserId,
                UserName = au.UserName,
                RecoveryEmail = email
            };
            GetPropertyValues(db, au, user);
            return user;
        }

        public ConModels.User GetUserById(Guid id)
        {
            using (var db = new TRIPS_UserEntities())
            {
                var au = db.aspnet_Users.SingleOrDefault(u => u.UserId == id);
                if (au != null)
                {
                    return GetUserModel(db, au);
                }
            }
            return null;
        }

        public ConModels.User GetUserByName(string name)
        {
            using (var db = new TRIPS_UserEntities())
            {
                var au = db.aspnet_Users.SingleOrDefault(u => u.UserName == name);
                if (au != null)
                {
                    return GetUserModel(db, au);
                }
            }
            return null;
        }

        public List<ConModels.User> GetUsers()
        {
            var users = new List<ConModels.User>();

            using (var db = new TRIPS_UserEntities())
            {
                foreach (var au in db.aspnet_Users)
                {
                    users.Add(GetUserModel(db, au));
                }
            }

            return users;
        }

        public void DeleteUser(Guid id)
        {
            using (var db = new TRIPS_UserEntities())
            {
                // get the aspnet_User that was just created
                var user = db.aspnet_Users.Single(u => u.UserId == id);

                // remove the property values for this user
                foreach (var ppv in db.ProfilePropertyValues.Where(p => p.UserID == id))
                {
                    db.ProfilePropertyValues.DeleteObject(ppv);
                }

                db.SaveChanges();

                Membership.DeleteUser(user.UserName, true);
            }

        }

        public List<ConModels.ContactList> GetContactListsOwnedBy(Guid owner)
        {
            var lists = new List<ConModels.ContactList>();
            using (var db = new TRIPS_UserEntities())
            {
                lists.AddRange(db.ContactList_Owner
                    .Where(o => o.UserId == owner)
                    .Select(l => new ConModels.ContactList()
                    {
                        Id = l.ContactList_Id,
                        Name = l.ContactList.Name
                    }));
            }
            return lists;
        }


        public List<ConModels.ContactList> GetPublicContactLists()
        {
            var lists = new List<ConModels.ContactList>();
            using (var db = new TRIPS_UserEntities())
            {
                lists.AddRange(db.ContactLists
                    .Where(o => o.IsPrivate == false)
                    .Select(l => new ConModels.ContactList()
                    {
                        Id = l.Id,
                        Name = l.Name
                    }));
            }
            return lists;
        }



        public int CreateContactList(ConModels.ContactList list)
        {
            using (var db = new TRIPS_UserEntities())
            {
                var cl = new ContactList()
                {
                    DateCreated = DateTime.UtcNow,
                    LastModified = DateTime.UtcNow,
                    IsActive = true,
                    IsPrivate = list.IsPrivate,
                    Name = list.Name
                };
                db.AddToContactLists(cl);
                db.SaveChanges();
                return cl.Id;
            }
        }

        public void AddOwnerToList(int listId, Guid owner)
        {
            using (var db = new TRIPS_UserEntities())
            {
                var clo = new ContactList_Owner()
                {
                    DateCreated = DateTime.UtcNow,
                    ContactList_Id = listId,
                    UserId = owner
                };
                db.AddToContactList_Owner(clo);
                db.SaveChanges();
            }
        }
    }
}
