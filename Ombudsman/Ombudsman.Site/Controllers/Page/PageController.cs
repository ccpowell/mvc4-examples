﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;

namespace Ombudsman.Site.Controllers.Page
{

    public class PageModel<T>
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public IEnumerable<T> aaData { get; set; }
    }


    public class PageController : ApiController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public PageController() { }

        public PageModel<Ombudsman.Models.Facility> GetFacilities(int iDisplayStart = 0, int iDisplayLength = 10, int onlyOmbudsmanid = 0, int onlyFacilityTypeId = 0)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var all = repo.GetFacilities();
            var totalCount = all.Count;
            var filtered = all
                .Where(f => (onlyOmbudsmanid == 0 || f.OmbudsmanId == onlyOmbudsmanid))
                .Where(f => (onlyFacilityTypeId == 0 || f.FacilityTypeId == onlyFacilityTypeId));
            var filteredCount = filtered.Count();

            var items = filtered
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToList();

            var page = new PageModel<Ombudsman.Models.Facility>()
            {
                aaData = items,
                iTotalDisplayRecords = filteredCount,
                iTotalRecords = totalCount
            };
            return page;
        }


        public PageModel<Ombudsman.Models.Ombudsman> GetOmbudsmen(int iDisplayStart = 0, int iDisplayLength = 10)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var filtered = repo.GetOmbudsmen();
            var filteredCount = filtered.Count;

            var items = filtered
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToList();

            var totalCount = filtered.Count;

            var page = new PageModel<Ombudsman.Models.Ombudsman>()
            {
                aaData = items,
                iTotalDisplayRecords = filteredCount,
                iTotalRecords = totalCount
            };
            return page;
        }

        public List<Ombudsman.Models.Ombudsman> GetAcOmbudsman(string term)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var items = repo.GetOmbudsmen();

            return items.Where(omb => omb.Name.ToLower().StartsWith(term.ToLower()))
                .Take(5)
                .ToList();
        }
    }
}