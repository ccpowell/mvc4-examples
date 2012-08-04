using System;
using System.Collections.Generic;
using System.Data;
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

    public class FacilityModel
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public string OmbudsmanName { get; set; }
        public int OmbudsmanId { get; set; }
    }

    public class OmbudsmanModel
    {
        public int OmbudsmanId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }



    /// <summary>
    /// This controller handles paged data requests for a jquery datatable.
    /// </summary>
    public class PageController : ApiController
    {
        private OmbudsmanEntities db = new OmbudsmanEntities();

        // GET page/Facilities
        public PageModel<FacilityModel> GetFacilities(string iDisplayStart = "0", string iDisplayLength = "25")
        {
            var items = db.Facilities.Include("Ombudsman")
                .Skip("it.Name", iDisplayStart)
                .Top(iDisplayLength)
                .AsEnumerable()
                .Select(f => new FacilityModel()
                {
                    FacilityId = f.FacilityId,
                    Name = f.Name,
                    OmbudsmanId = f.OmbudsmanId ?? 0,
                    OmbudsmanName = f.Ombudsman != null ? f.Ombudsman.Name : ""
                });
            var totalRecords = db.Facilities.Count();
            var page = new PageModel<FacilityModel>()
            {
                aaData = items,
                iTotalDisplayRecords = totalRecords,
                iTotalRecords = totalRecords
            };
            return page;
        }

        // GET page/Ombudsmen
        public PageModel<OmbudsmanModel> GetOmbudsmen(string iDisplayLength = "10", string iDisplayStart = "0")
        {
            var items = db.Ombudsmen
                .Skip("it.Name", iDisplayStart)
                .Top(iDisplayLength)
                .AsEnumerable()
                .Select(f => new OmbudsmanModel()
                {
                    Name = f.Name,
                    OmbudsmanId = f.OmbudsmanId,
                    UserName = f.UserName
                });
            var totalRecords = db.Ombudsmen.Count();
            var page = new PageModel<OmbudsmanModel>()
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