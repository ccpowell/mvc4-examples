using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConRepo;
using NLog;

namespace ConMvc4Site.Controllers.Api
{
    public class UserController : ApiController
    {
        public UserController(ContactsRepository repo, Parts.UserCache cache)
        {
            Users = repo;
            UserCache = cache;
        }
        private ContactsRepository Users { get; set; }
        private Parts.UserCache UserCache { get; set; }

        private static Logger Logger = LogManager.GetCurrentClassLogger();
        

        // GET api/user
        public IEnumerable<ConModels.User> Get()
        {
            var users = UserCache.GetAllUsers();
            return users;
        }

        // GET api/user/5
        public ConModels.User Get(Guid id)
        {
            return Users.GetUserById(id);
        }

        // POST api/user
        public void Post(ConModels.User value)
        {
            Users.CreateUser(value);
            UserCache.AddUser(value);
        }

        // PUT api/user/5
        public void Put(Guid id, ConModels.User value)
        {
            Users.UpdateUser(value);
            UserCache.UpdateUser(value);
        }

        // DELETE api/user/5
        public void Delete(Guid id)
        {
            throw new NotImplementedException("cannot delete users");
        }
    }
}
