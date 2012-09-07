using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConRepo;
using Castle.Core.Logging;

namespace ConMvc4Site.Controllers.Api
{
    public class UserController : ApiController
    {
        public UserController(ContactsRepository repo, ILogger logger)
        {
            Users = repo;
            Logger = logger;
        }
        private ContactsRepository Users { get; set; }
        private ILogger Logger { get; set; }

        // GET api/user
        public IEnumerable<ConModels.User> Get()
        {
            var users = new List<ConModels.User>();
            return users;
        }

        // GET api/user/5
        public ConModels.User Get(int id)
        {
            return null;
        }

        // POST api/user
        public void Post(ConModels.User value)
        {
        }

        // PUT api/user/5
        public void Put(int id, ConModels.User value)
        {
        }

        // DELETE api/user/5
        public void Delete(Guid id)
        {
        }
    }
}
