using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using OmbMf.Models;

namespace OmbMf.Controllers.Page
{

    public class PageModel<T>
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public IEnumerable<T> aaData { get; set; }
    }


    /// <summary>
    /// This controller handles paged data requests for a jquery datatable.
    /// </summary>
    public class PageController : ApiController
    {
        private OmbMf.Models.OmbudsmanDbContext db = new OmbudsmanDbContext();

        // GET page/Facilities
        public PageModel<Facility> GetFacilities(int iDisplayStart = 0, int iDisplayLength = 10, int onlyOmbudsmanid = 0, int onlyFacilityTypeId = 0)
        {
            var filtered = db.Facilities.Include(f => f.Ombudsman).Include(x => x.FacilityType)
                .Where(f => (onlyOmbudsmanid == 0 || f.OmbudsmanId == onlyOmbudsmanid))
                .Where(f => (onlyFacilityTypeId == 0 || f.FacilityTypeId == onlyFacilityTypeId));
            var filteredCount = filtered.Count();
            var items = filtered
                .OrderBy(f => f.Name)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .AsEnumerable();
            var totalCount = db.Facilities.Count();

            var page = new PageModel<Facility>()
            {
                aaData = items,
                iTotalDisplayRecords = filteredCount,
                iTotalRecords = totalCount
            };
            return page;
        }

        // GET page/Ombudsmen
        public PageModel<Ombudsman> GetOmbudsmen(int iDisplayStart = 0, int iDisplayLength = 10)
        {
            var items = db.Ombudsmen
                .OrderBy(f => f.Name)
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .AsEnumerable();
            var totalRecords = db.Ombudsmen.Count();
            var page = new PageModel<Ombudsman>()
                {
                    aaData = items,
                    iTotalDisplayRecords = totalRecords,
                    iTotalRecords = totalRecords
                };
            return page;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}