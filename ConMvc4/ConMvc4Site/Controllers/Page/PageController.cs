using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;

namespace ConMvc4Site.Controllers.Page
{

    public class PageModel<T>
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public IEnumerable<T> aaData { get; set; }
    }


    public class PageController : ApiController
    {
        private ConRepo.ContactsRepository Users { get; set; }
        private Parts.UserCache UserCache { get; set; }

        private static Logger Logger = LogManager.GetCurrentClassLogger();
        
        public PageController(ConRepo.ContactsRepository repo, Parts.UserCache cache)
        {
            Users = repo;
            UserCache = cache;
        }

        public PageModel<ConModels.User> GetUsers(int iDisplayStart = 0, int iDisplayLength = 10)
        {
            var filtered = UserCache.GetAllUsers();
            var filteredCount = filtered.Count;

            var items = filtered
                .OrderBy(f => f.UserName)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToList();

            var totalCount = filtered.Count;

            var page = new PageModel<ConModels.User>()
            {
                aaData = items,
                iTotalDisplayRecords = filteredCount,
                iTotalRecords = totalCount
            };
            return page;
        }


        public PageModel<ConModels.ContactList> GetOwnedLists(Guid id, int iDisplayStart = 0, int iDisplayLength = 10)
        {
            var filtered = Users.GetContactListsOwnedBy(id);
            var filteredCount = filtered.Count;

            var items = filtered
                .OrderBy(f => f.Name)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToList();

            var totalCount = filtered.Count;

            var page = new PageModel<ConModels.ContactList>()
            {
                aaData = items,
                iTotalDisplayRecords = filteredCount,
                iTotalRecords = totalCount
            };
            return page;
        }



        public PageModel<ConModels.ContactList> GetPublicLists(int iDisplayStart = 0, int iDisplayLength = 10)
        {
            var filtered = Users.GetPublicContactLists();
            var filteredCount = filtered.Count;

            var items = filtered
                .OrderBy(f => f.Name)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToList();

            var totalCount = filtered.Count;

            var page = new PageModel<ConModels.ContactList>()
            {
                aaData = items,
                iTotalDisplayRecords = filteredCount,
                iTotalRecords = totalCount
            };
            return page;
        }
    }
}
