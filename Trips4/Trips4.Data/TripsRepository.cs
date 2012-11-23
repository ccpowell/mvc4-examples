using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.Web.Mvc;

namespace Trips4.Data
{
    public class PlanCycle
    {
    }
    public class TripsRepository
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public int GetRtpPlanYearId(string rtpYear)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var tper = db.TimePeriods.FirstOrDefault(tp => tp.TimePeriod1 == rtpYear && tp.TimePeriodTypeID == 3);
                if (tper == null)
                {
                    throw new Exception("No such RTP Year " + rtpYear);
                }
                return tper.TimePeriodID;
            }
        }

        public IEnumerable<DRCOG.Domain.ViewModels.RTP.PlanCycle> GetRtpPlanCycles(int rtpYearId)
        {
            var result = new List<DRCOG.Domain.ViewModels.RTP.PlanCycle>();
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var tpcs = db.TimePeriodCycles.Where(t => t.TimePeriodId == rtpYearId).OrderByDescending(t => t.ListOrder);
                foreach (var tpc in tpcs)
                {
                    result.Add(new DRCOG.Domain.ViewModels.RTP.PlanCycle()
                    {
                        Description = tpc.Cycle.Description,
                        Id = tpc.CycleId,
                        Name = tpc.Cycle.cycle1,
                        Status = tpc.Cycle.Status.Status1
                    });
                }
            }
            return result;
        }

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

        public DRCOG.Domain.ViewModels.RTP.ReportsViewModel GetReportsViewModel(string year)
        {
            var result = new DRCOG.Domain.ViewModels.RTP.ReportsViewModel();

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

        public void UpdateRtpPlanCycle(DRCOG.Domain.ViewModels.RTP.PlanCycle cycle)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var found = db.Cycles.FirstOrDefault(c => c.id == cycle.Id);
                if (found == null)
                {
                    throw new Exception("No such RTP Plan Cycle");
                }
                found.cycle1 = cycle.Name;
                found.Description = cycle.Description;
                db.SaveChanges();
            }
        }

        public DRCOG.Domain.ViewModels.RTP.PlanCycle GetRtpPlanCycle(int id)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var found = db.Cycles.FirstOrDefault(c => c.id == id);
                if (found == null)
                {
                    return null;
                }
                return new DRCOG.Domain.ViewModels.RTP.PlanCycle()
                {
                    Description = found.Description,
                    Id = found.id,
                    Name = found.cycle1,
                    Status = found.Status.Status1
                };
            }
        }

        /// <summary>
        /// Delete RTP Plan Cycle from RTP Year and then from Cycles. Not currently used in production,
        /// but useful for testing.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteRtpPlanCycle(int id)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var cycle = db.Cycles.FirstOrDefault(c => c.id == id);
                if (cycle == null)
                {
                    throw new Exception("No such RTP Plan Cycle");
                }
                var refs = db.TimePeriodCycles.Where(tpc => tpc.CycleId == id);
                foreach (var t in refs)
                {
                    db.TimePeriodCycles.DeleteObject(t);
                }
                db.Cycles.DeleteObject(cycle);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Create a new PlanCycle and attach it to the given plan year.
        /// </summary>
        /// <param name="cycle">with Name and Description</param>
        /// <param name="rtpYearId">ID of plan year</param>
        /// <returns>ID of created Plan Cycle</returns>
        public int CreateRtpPlanCycle(DRCOG.Domain.ViewModels.RTP.PlanCycle cycle, int rtpYearId)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                // there can be only one cycle in this plan year with a status of New
                var stId = db.StatusTypes.First(st => st.StatusType1 == "Cycle Status").StatusTypeID;
                var newId = db.Status.First(s => s.StatusTypeID == stId && s.Status1 == "New").StatusID;
                var previousNew = db.TimePeriodCycles.FirstOrDefault(tpc => tpc.TimePeriodId == rtpYearId && tpc.Cycle.statusId == newId);
                if (previousNew != null)
                {
                    throw new Exception("There is already a New Plan Cycle");
                }
                var mcycle = new Models.Cycle()
                {
                    cycle1 = cycle.Name,
                    Description = cycle.Description,
                    statusId = newId
                };
                db.Cycles.AddObject(mcycle);

                // find largest ListOrder in these cycles and add 1
                var lo = db.TimePeriodCycles.Where(tpc => tpc.TimePeriodId == rtpYearId).Max(tpc => tpc.ListOrder);
                lo += 1;

                var mtpc = new Models.TimePeriodCycle()
                {
                    Cycle = mcycle,
                    TimePeriodId = (short)rtpYearId,
                    ListOrder = lo
                };
                db.TimePeriodCycles.AddObject(mtpc);

                db.SaveChanges();

                return mcycle.id;
            }
        }
    }
}
