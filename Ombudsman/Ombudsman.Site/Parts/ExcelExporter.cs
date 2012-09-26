using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using NLog;

namespace Ombudsman.Site.Parts
{
    public class ExcelExporter
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private const int IdPCBH = 3;
        private const int IdNH = 1;
        private const int IdRTF = 2;

        public byte[] GetFacilityListDocument(IEnumerable<Ombudsman.Models.Facility> all)
        {
            using (var pck = new ExcelPackage())
            {
                var pcblist = all
                    .Where(f => f.FacilityTypeId == IdPCBH)
                    .Select(x => new { x.Name, x.OmbudsmanName, x.Address1, x.Address2, x.City, x.ZipCode, x.Phone, x.NumberOfBeds, x.IsActive, x.IsMedicaid, x.IsContinuum });

                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("PersonalCareBoardingHome");
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromCollection(pcblist, true);
                ws.Cells["A1"].Value = "Facility";
                ws.Cells["B1"].Value = "Ombudsman";
                ws.Cells.AutoFitColumns();
                //ws.Column(10).Hidden = true;
                //ws.Column(11).Hidden = true;

                var rtflist = all
                    .Where(f => f.FacilityTypeId == IdRTF)
                    .Select(x => new { x.Name, x.OmbudsmanName, x.Address1, x.Address2, x.City, x.ZipCode, x.Phone, x.NumberOfBeds, x.IsActive, x.IsMedicaid, x.IsContinuum });

                ExcelWorksheet ws1 = pck.Workbook.Worksheets.Add("ResidentialTreatmentFacility");
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws1.Cells["A1"].LoadFromCollection(rtflist, true);
                ws1.Cells["A1"].Value = "Facility";
                ws1.Cells["B1"].Value = "Ombudsman";
                ws1.Cells.AutoFitColumns();

                var nhlist = all
                    .Where(f => f.FacilityTypeId == IdNH)
                    .Select(x => new { x.Name, x.OmbudsmanName, x.Address1, x.Address2, x.City, x.ZipCode, x.Phone, x.NumberOfBeds, x.IsActive, x.IsMedicaid, x.IsContinuum });

                ExcelWorksheet ws2 = pck.Workbook.Worksheets.Add("NursingHome");
                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws2.Cells["A1"].LoadFromCollection(nhlist, true);
                ws2.Cells["A1"].Value = "Facility";
                ws2.Cells["B1"].Value = "Ombudsman";
                ws2.Cells.AutoFitColumns();

                return pck.GetAsByteArray();
            }
        }
    }
}