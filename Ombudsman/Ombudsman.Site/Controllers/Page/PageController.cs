using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NLog;
using System.IO;

namespace Ombudsman.Site.Controllers.Page
{

    public class PageModel<T>
    {
        public int iTotalRecords { get; set; }
        public int iTotalDisplayRecords { get; set; }
        public IEnumerable<T> aaData { get; set; }
    }

    [Authorize]
    public class PageController : ApiController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public PageModel<Ombudsman.Models.Facility> GetFacilities(int iDisplayStart = 0, int iDisplayLength = 10, string onlyFacilityName = "", string onlyOmbudsmanName = "", int onlyFacilityTypeId = 0, int onlyFacilityIsActive = 0)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var all = repo.GetFacilities();
            var totalCount = all.Count;
            int? onlyOmbudsmanId = repo.GetOmbudsmanIdFromName(onlyOmbudsmanName);
            var filtered = all
                .Where(f => (string.IsNullOrWhiteSpace(onlyFacilityName) || f.Name.ToLower().Contains(onlyFacilityName.ToLower())))
                .Where(f => (onlyFacilityIsActive == 0 || f.IsActive == (1 == onlyFacilityIsActive)))
                .Where(f => (onlyOmbudsmanId == null || f.OmbudsmanId == onlyOmbudsmanId))
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


        public PageModel<Ombudsman.Models.Ombudsman> GetOmbudsmen(int iDisplayStart = 0, int iDisplayLength = 10, int onlyOmbudsmanIsActive = 0)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var all = repo.GetOmbudsmen();
            var totalCount = all.Count;
            var filtered = all
                .Where(f => (onlyOmbudsmanIsActive == 0 || f.IsActive == (1 == onlyOmbudsmanIsActive)));
            var filteredCount = filtered.Count();

            var items = filtered
                .Skip(iDisplayStart)
                .Take(iDisplayLength)
                .ToList();

            var page = new PageModel<Ombudsman.Models.Ombudsman>()
            {
                aaData = items,
                iTotalDisplayRecords = filteredCount,
                iTotalRecords = totalCount
            };
            return page;
        }

        /// <summary>
        /// Return 5 suggestions for an Ombudsman given a search term.
        /// </summary>
        /// <param name="term">first part of name</param>
        /// <returns>top 5 names</returns>
        public List<Ombudsman.Models.Ombudsman> GetAcOmbudsman(string term)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var items = repo.GetOmbudsmenByName(term);

            return items.Take(5).ToList();
        }

        /// <summary>
        /// Get an Excel document containing the facilities for an ombudsman, or all.
        /// </summary>
        /// <param name="ombudsmanId"></param>
        /// <returns></returns>
        public HttpResponseMessage GetFacilityList(string onlyFacilityName = "", string onlyOmbudsmanName = "", int onlyFacilityTypeId = 0, int onlyFacilityIsActive = 0)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var all = repo.GetFacilities();
            int? onlyOmbudsmanId = repo.GetOmbudsmanIdFromName(onlyOmbudsmanName);
            var filtered = all
                .Where(f => (string.IsNullOrWhiteSpace(onlyFacilityName) || f.Name.ToLower().Contains(onlyFacilityName.ToLower())))
                .Where(f => ((onlyFacilityIsActive == 0) || f.IsActive == (1 == onlyFacilityIsActive)))
                .Where(f => ((onlyOmbudsmanId == null) || f.OmbudsmanId == onlyOmbudsmanId))
                .Where(f => ((onlyFacilityTypeId == 0) || f.FacilityTypeId == onlyFacilityTypeId));

            var ee = new Parts.ExcelExporter();
            var bytes = ee.GetFacilityListDocument(filtered);
            var response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(bytes);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "FacilityExtract.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return response;
        }
    }
}
