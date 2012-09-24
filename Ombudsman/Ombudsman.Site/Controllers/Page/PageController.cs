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


    public class PageController : ApiController
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public PageController() { }

        public PageModel<Ombudsman.Models.Facility> GetFacilities(int iDisplayStart = 0, int iDisplayLength = 10, string onlyOmbudsmanName = "", int onlyFacilityTypeId = 0)
        {
            var repo = new OmbudsmanDb.OmbudsmanRepository();
            var all = repo.GetFacilities();
            var totalCount = all.Count;
            int? onlyOmbudsmanId = repo.GetOmbudsmanIdFromName(onlyOmbudsmanName);
            var filtered = all
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
            var items = repo.GetOmbudsmenByName(term);

            return items.Take(5).ToList();
        }
#if needed
        public ActionResult DownloadFacilityList(int ombudsmanId)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                var pcblist = GetFacilityEntityList<PersonalCareBoardingHome>(new FacilityEntitySearchCriteria
                {
                    OmbudsmanId = ombudsmanId
                    ,
                    FacilityEntityType = Core.EnumsCustom.FacilityEntityType.PersonalCareBoardingHome
                }).OrderBy(x => x.Name).Select(x => new { x.Name, x.OmbudsmanName, x.Address, x.City, x.County, x.Zip, x.Phone, x.Beds, x.Medicaid, x.Locked, x.Continuum });

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("PersonalCareBoardingHome");
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromCollection(pcblist, true);
                ws.Cells["A1"].Value = "Facility";
                ws.Cells["B1"].Value = "Ombudsman";
                ws.Cells.AutoFitColumns();
                ws.Column(10).Hidden = true;
                ws.Column(11).Hidden = true;

                var rtflist = GetFacilityEntityList<ResidentialTreatmentFacility>(new FacilityEntitySearchCriteria
                {
                    OmbudsmanId = ombudsmanId
                    ,
                    FacilityEntityType = Core.EnumsCustom.FacilityEntityType.ResidentialTreatmentFacility
                }).OrderBy(x => x.Name).Select(x => new { x.Name, x.OmbudsmanName, x.ManagedBy, x.Address, x.City, x.County, x.Zip, x.Phone, x.NumberBeds, x.Gender, x.Population, x.AverageAge, x.AvgLengthOfStay });
                ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("ResidentialTreatmentFacility");
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws1.Cells["A1"].LoadFromCollection(rtflist, true);
                ws1.Cells["A1"].Value = "Facility";
                ws1.Cells["B1"].Value = "Ombudsman";
                ws1.Cells.AutoFitColumns();

                var nhlist = GetFacilityEntityList<NursingHome>(new FacilityEntitySearchCriteria
                {
                    OmbudsmanId = ombudsmanId
                    ,
                    FacilityEntityType = Core.EnumsCustom.FacilityEntityType.NursingHome
                }).OrderBy(x => x.Name).Select(x => new { x.Name, x.OmbudsmanName, x.Address, x.City, x.County, x.Zip, x.Phone, x.NumberBeds, x.Medicaid });
                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("NursingHome");
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws2.Cells["A1"].LoadFromCollection(nhlist, true);
                ws2.Cells["A1"].Value = "Facility";
                ws2.Cells["B1"].Value = "Ombudsman";
                ws2.Cells.AutoFitColumns();

                //Write it back to the client
                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;  filename=FacilityExtract.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.End();
            }

            //list.Add(GetFacilityEntityList<PersonalCareBoardingHome>(new FacilityEntitySearchCriteria { 
            //    OmbudsmanId = ombudsmanId 
            //,   FacilityEntityType = Core.EnumsCustom.FacilityEntityType.PersonalCareBoardingHome
            //}));

            //list.Add(GetFacilityEntityList<ResidentialTreatmentFacility>(new FacilityEntitySearchCriteria
            //{
            //    OmbudsmanId = ombudsmanId
            //    ,
            //    FacilityEntityType = Core.EnumsCustom.FacilityEntityType.ResidentialTreatmentFacility
            //}));

            //list.Add(GetFacilityEntityList<NursingHome>(new FacilityEntitySearchCriteria
            //{
            //    OmbudsmanId = ombudsmanId
            //    ,
            //    FacilityEntityType = Core.EnumsCustom.FacilityEntityType.NursingHome
            //}));

            return RedirectToAction("Index", "Home");
        }
#endif
        public HttpResponseMessage GetFacilityList(int ombudsmanId)
        {
            string localFilePath = @"c:\\tmp\\hoover.xlsx";

            var stream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read);
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "FacilityExtract.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return response;
        }
    }
}
