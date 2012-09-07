using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConModels;
using Castle.Core.Logging;
namespace ConMvc4Site.Parts
{
    public class UserCache
    {
        private object SyncObject = new object();
        private List<User> AllUsers { get; set; }
        private ConRepo.ContactsRepository Users { get; set; }
        private ILogger Logger { get; set; }

        private UserCache() { }

        public UserCache(ConRepo.ContactsRepository repo, ILogger logger)
        {
            Users = repo;
            Logger = logger;
            Refresh();
        }

        /// <summary>
        /// Get a complete list of users.
        /// </summary>
        /// <returns>a copy of all users</returns>
        public List<ConModels.User> GetAllUsers()
        {
            lock (SyncObject)
            {
                return new List<ConModels.User>(AllUsers);
            }
        }
        
        /// <summary>
        /// Rebuild the cache entirely.
        /// </summary>
        public void Refresh()
        {
            Logger.Info("User cache refreshing...");
            lock (SyncObject)
            {
                AllUsers = Users.GetUsers();
            }
            Logger.Info("User cache refreshed.");
        }
#if direct

        private string GetString(IDataReader reader, string column)
        {
            var value = reader[column];
            if (value == DBNull.Value)
                return string.Empty;
            return ((string)value).Trim();
        }

        /// <summary>
        /// Rebuild the cache entirely.
        /// </summary>
        public void Refresh()
        {
            List<ConModels.User> users = new List<ConModels.User>();
            var ubad = new Dictionary<string, List<ConModels.User>>();
            using (IDataReader reader = DataManager.GetDataReader("dbo.Membership_GetUsers", CommandType.StoredProcedure))
            {
                while (reader.Read())
                {
                    var profile = new ConModels.User(true);
                    profile.PersonGUID = (System.Guid)reader["UserID"];
                    profile.UserName = GetString(reader, "UserName");
                    profile.LastName = GetString(reader, "LastName");
                    profile.FirstName = GetString(reader, "FirstName");
                    profile.Organization = GetString(reader, "Organization");
                    profile.SetValue("Title", GetString(reader, "Title"));
                    profile.SetValue("PrimaryContact", GetString(reader, "PrimaryContact"));
                    profile.BusinessEmail = GetString(reader, "LoweredBusinessEmail");
                    profile.RecoveryEmail = GetString(reader, "LoweredEmail");
                    users.Add(profile);

                    var app = GetString(reader, "LoweredApplicationName");
                    List<ConModels.User> uba = null;
                    if (!ubad.TryGetValue(app, out uba))
                    {
                        uba = new List<ConModels.User>();
                        ubad.Add(app, uba);
                    }
                    uba.Add(profile);
                }
            }

            // replace cache
            lock (SyncObject)
            {
                AllUsers = users;
                UsersByApplication = ubad;
            }
        }
#endif

        /// <summary>
        /// Add a user for a given application.
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="application"></param>
        public void AddUser(ConModels.User profile, string application)
        {
            lock (SyncObject)
            {
                var old = AllUsers.FirstOrDefault(p => p.Id == profile.Id);
                if (old != null)
                {
                    throw new Exception("User is already in cache");
                }

                var addit = new ConModels.User();
                addit.Id = profile.Id;
                addit.UserName = profile.UserName ?? string.Empty;
                addit.LastName = profile.LastName ?? string.Empty;
                addit.FirstName = profile.FirstName ?? string.Empty;
                addit.Organization = profile.Organization ?? string.Empty;
                addit.Title = profile.Title ?? string.Empty;
                addit.Phone = profile.Phone ?? string.Empty;
                addit.BusinessEmail = profile.BusinessEmail ?? string.Empty;
                addit.RecoveryEmail = profile.RecoveryEmail ?? string.Empty;
                AllUsers.Add(addit);
            }
        }

        /// <summary>
        /// Update the user in the cache. The PersonGUID is used as the key.
        /// </summary>
        /// <param name="profile">new profile data</param>
        public void UpdateUser(ConModels.User profile)
        {
            lock (SyncObject)
            {
                var old = AllUsers.FirstOrDefault(p => p.Id == profile.Id);
                if (old != null)
                {
                    old.UserName = profile.UserName ?? string.Empty;
                    old.LastName = profile.LastName ?? string.Empty;
                    old.FirstName = profile.FirstName ?? string.Empty;
                    old.Organization = profile.Organization ?? string.Empty;
                    old.Title = profile.Title ?? string.Empty;
                    old.Phone = profile.Phone ?? string.Empty;
                    old.BusinessEmail = profile.BusinessEmail ?? string.Empty;
                    old.RecoveryEmail = profile.RecoveryEmail ?? string.Empty;
                }
            }
        }

        /// <summary>
        /// Delete the user with the given PersonGUID.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUser(Guid id)
        {
            lock (SyncObject)
            {
                AllUsers.RemoveAll(p => p.Id == id);
            }
        }
    }
}