using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.ViewModels.RTP;
using OfficeOpenXml;
using System.Web.Mvc;

namespace Trips4.Data
{
    public class TripsRepository
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public byte[] GetRtpModelerExtractDocument(int? timePeriodId, int? excludeBeforeYear)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var extracts = db.RtpModelerExtract(timePeriodId, excludeBeforeYear);
                var results = extracts.ToArray();
                using (var pck = new ExcelPackage())
                {
                    // Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ModelerExtract");

                    // Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells["A1"].LoadFromCollection(results, true);
                    ws.Cells.AutoFitColumns();
                    return pck.GetAsByteArray();
                }
            }
        }

        public byte[] GetSurveyModelerExtractDocument(int? timePeriodId)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var extracts = db.SurveyModelerExtract(timePeriodId);
                var results = extracts.ToArray();
                using (var pck = new ExcelPackage())
                {
                    // Create the worksheet
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ModelerExtract");

                    // Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                    ws.Cells["A1"].LoadFromCollection(results, true);
                    ws.Cells.AutoFitColumns();
                    return pck.GetAsByteArray();
                }
            }
        }

        public IEnumerable<SelectListItem> GetFundingIncrements(int tipYearId)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var tps = db.TimePeriods.Where(tp => tp.TimePeriodID == tipYearId).SingleOrDefault();
                if (tps == null)
                {
                    throw new Exception("No such TimePeriod " + tipYearId);
                }
                return tps.FundingIncrements.Select(fi => new SelectListItem()
                    {
                        Text = fi.FundingIncrement1,
                        Value = fi.FundingIncrementID.ToString()
                    }).ToArray();
            }
        }

        public ReportsViewModel GetReportsViewModel(string year)
        {
            var result = new ReportsViewModel();

            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                result.RtpSummary = new DRCOG.Domain.Models.RTP.RtpSummary();
                var summary = db.RtpGetSummary(year, null).FirstOrDefault();
                if (summary != null)
                {
                    // many things left unset
                    result.RtpSummary.RtpYear = year;
                    result.RtpSummary.RTPYearTimePeriodID = summary.TimePeriodID;
                    result.RtpSummary.TimePeriodStatusId = summary.TimePeriodStatusId ?? 0;
                    result.RtpSummary.Cycle = new DRCOG.Domain.Models.Cycle()
                    {
                        // many things left unset
                        Id = summary.CycleId ?? 0,
                        Name = summary.CycleName,
                        StatusId = summary.CycleStatusId ?? 0,
                        Status = summary.CycleStatus,
                        PriorCycleId = summary.priorCycleId ?? 0,
                        NextCycleId = summary.nextCycleId ?? 0,
                        NextCycleName = summary.nextCycle,
                        NextCycleStatus = summary.nextStatus
                    };
                }
                else
                {
                    Logger.Debug("No summary found for year " + year);
                }

                // TODO: is this the right Time Period ID for this?
                result.CurrentPlanCycles = new List<KeyValuePair<int, string>>();
                result.CurrentPlanCycles.Add(new KeyValuePair<int, string>(0, "All"));
                var cycles = db.RtpGetCurrentPlanCycles(result.RtpSummary.RTPYearTimePeriodID);
                foreach (var cycle in cycles.OrderBy(c => c.cycle))
                {
                    result.CurrentPlanCycles.Add(new KeyValuePair<int, string>(cycle.id, cycle.cycle));
                }

                result.SurveyYears = new List<KeyValuePair<int, string>>();
                foreach (var tp in db.TimePeriods.OrderBy(t => t.TimePeriod1))
                {
                    var name = tp.TimePeriod1;
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = "<No Name>";
                    }

                    result.SurveyYears.Add(new KeyValuePair<int, string>(tp.TimePeriodID, name));
                }
            }

            return result;
        }
    }
}
