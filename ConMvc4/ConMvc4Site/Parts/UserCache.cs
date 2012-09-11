using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConModels;
using NLog;
namespace ConMvc4Site.Parts
{
    public class UserCache
    {
        private object SyncObject = new object();
        private List<User> AllUsers { get; set; }
        private ConRepo.ContactsRepository Users { get; set; }
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private UserCache() { }

        public UserCache(ConRepo.ContactsRepository repo)
        {
            Users = repo;
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
                AllUsers = new List<User>();
                foreach (var us in Users.GetUsers())
                {
                    var user = new ConModels.User() { Id = us.Id };
                    CopyProperties(user, us);
                    AllUsers.Add(user);
                }
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

        private void CopyProperties(ConModels.User addit, ConModels.User profile)
        {
            addit.UserName = profile.UserName ?? string.Empty;
            addit.LastName = profile.LastName ?? string.Empty;
            addit.FirstName = profile.FirstName ?? string.Empty;
            addit.Organization = profile.Organization ?? string.Empty;
            addit.Title = profile.Title ?? string.Empty;
            addit.Phone = profile.Phone ?? string.Empty;
            addit.BusinessEmail = profile.BusinessEmail ?? string.Empty;
            addit.RecoveryEmail = profile.RecoveryEmail ?? string.Empty;
        }

        /// <summary>
        /// Add a user.
        /// </summary>
        /// <param name="profile"></param>
        public void AddUser(ConModels.User profile)
        {
            lock (SyncObject)
            {
                var old = AllUsers.FirstOrDefault(p => p.Id == profile.Id);
                if (old != null)
                {
                    throw new Exception("User is already in cache");
                }

                var addit = new ConModels.User() { Id = profile.Id };
                CopyProperties(addit, profile);
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
                    CopyProperties(old, profile);
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