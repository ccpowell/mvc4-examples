using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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
        public PageModel<ConModels.User> GetUsers(int iDisplayStart = 0, int iDisplayLength = 10)
        {
            var filtered = new List<ConModels.User>();
            filtered.Add(new ConModels.User()
            {
                Id = Guid.NewGuid(),
                UserName = "goober",
                Organization = "Nogoodniks, Inc.",
                RecoveryEmail = "goober@drcog.org",
                Phone = "5555551212"
            });
            filtered.Add(new ConModels.User()
            {
                Id = Guid.NewGuid(),
                UserName = "grubby",
                Organization = "Nogoodniks, Inc.",
                RecoveryEmail = "grubby@drcog.org",
                Phone = "2225551212"
            });
            var filteredCount = filtered.Count;
            var items = filtered
                .OrderBy(f => f.UserName)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToList();

            var totalCount = 42;

            var page = new PageModel<ConModels.User>()
            {
                aaData = items,
                iTotalDisplayRecords = filteredCount,
                iTotalRecords = totalCount
            };
            return page;
        }
    }
}
