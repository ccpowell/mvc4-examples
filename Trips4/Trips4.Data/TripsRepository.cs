﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.Web.Mvc;
using DRCOG.Domain;
using System.Transactions;

namespace Trips4.Data
{
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

        public int GetTipYearId(string year)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var tper = db.TimePeriods.FirstOrDefault(tp => tp.TimePeriod1 == year && tp.TimePeriodTypeID == 2);
                if (tper == null)
                {
                    throw new Exception("No such TIP Year " + year);
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
                var years = db.TimePeriods.Join(db.ProgramInstances,
                    tp => tp.TimePeriodID,
                    pi => pi.TimePeriodID,
                    (tp, pi) => tp)
                    .Where(tp => tp.TimePeriodTypeID == (short)Enums.TimePeriodType.Survey);
                foreach (var tp in years.OrderBy(t => t.TimePeriod1))
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
        /// Get the ID of the RTP Plan Cycle used as a source from which to copy projects.
        /// It is the Active cycle in the Current plan.
        /// </summary>
        /// <returns>id of active cycle of current plan</returns>
        public int GetRtpActivePlanCycleId()
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                // get current plan, RtpTimePeriodStatus.Current
                var plan = db.ProgramInstances.FirstOrDefault(
                    tp => tp.StatusId == (int)Enums.RtpTimePeriodStatus.Current &&
                    tp.ProgramID == (int)Enums.TimePeriodType.TimePeriod);
                if (plan == null)
                {
                    throw new Exception("There is no Current RTP Plan");
                }
                var currentPlanId = plan.TimePeriodID;
                // we need to join Cycles and TimePeriodCycles to get Cycle
                var cycle = db.Cycles.Join(db.TimePeriodCycles,
                    c => c.id,
                    tpc => tpc.CycleId,
                    (c, tpc) => new { StatusId = c.statusId, CycleId = c.id, PlanId = tpc.TimePeriodId })
                    .Where(x => x.StatusId == (int)Enums.RTPCycleStatus.Active && x.PlanId == currentPlanId)
                    .FirstOrDefault();
                if (cycle == null)
                {
                    throw new Exception("No Active Cycle in Current RTP Plan");
                }
                return cycle.CycleId;
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
        /// Create a New PlanCycle and attach it to the given plan year.
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

                // see if there is already a new plan cycle for this RTP Year
                var previousNew = db.TimePeriodCycles.FirstOrDefault(tpc => tpc.TimePeriodId == rtpYearId && tpc.Cycle.statusId == newId);
                if (previousNew != null)
                {
                    throw new Exception("There is already a New Plan Cycle");
                }

                byte listOrder = 0;
                int? priorCycleId = null;

                // if this is not the first cycle in the time period, attach it to the end
                // of the two lists - ListOrder and priorCycleId
                var planCycles = db.TimePeriodCycles.Where(tpc => tpc.TimePeriodId == rtpYearId);
                if (planCycles.Count() > 0)
                {
                    // find last cycle in this time period. "last" means highest ListOrder.
                    var lo = planCycles.Max(tpc => tpc.ListOrder);
                    var prior = planCycles.First(tpc => tpc.ListOrder == lo);
                    priorCycleId = prior.CycleId;
                    listOrder = lo ?? 0;
                }

                // find largest ListOrder in these cycles and add 1
                ++listOrder;

                var mcycle = new Models.Cycle()
                {
                    cycle1 = cycle.Name,
                    Description = cycle.Description,
                    statusId = newId,
                    priorCycleId = priorCycleId
                };
                db.Cycles.AddObject(mcycle);

                var mtpc = new Models.TimePeriodCycle()
                {
                    Cycle = mcycle,
                    TimePeriodId = (short)rtpYearId,
                    ListOrder = listOrder
                };
                db.TimePeriodCycles.AddObject(mtpc);

                db.SaveChanges();

                return mcycle.id;
            }
        }



        /// <summary>
        /// This gets/sets Funding Increments for a given time period.
        /// </summary>
        /// <remarks>TODO: cache the Funding Increment labels and IDs?</remarks>
        private class FundingIncrementSetter
        {
            private Trips4.Data.Models.TRIPSEntities Db { get; set; }
            private Models.TimePeriod Tp { get; set; }

            public FundingIncrementSetter(Trips4.Data.Models.TRIPSEntities db, Models.TimePeriod tp)
            {
                Db = db;
                Tp = tp;
            }

            public bool GetFundingIncrement(string label)
            {
                return (Tp.FundingIncrements.SingleOrDefault(fi => fi.FundingIncrement1.Trim() == label) != null);
            }

            public void SetFundingIncrement(bool set, string label)
            {
                var fi = Db.FundingIncrements.Single(f => f.FundingIncrement1 == label);
                if (set)
                {
                    if (!Tp.FundingIncrements.Contains(fi))
                    {
                        Tp.FundingIncrements.Add(fi);
                    }
                }
                else
                {
                    Tp.FundingIncrements.Remove(fi);
                }
            }
        }

        /// <summary>
        /// Get TIP Status
        /// </summary>
        /// <remarks>obsoletes TIP.GetStatus</remarks>
        /// <param name="rtpYearId">ID of the RTP Plan Year</param>
        /// <returns></returns>
        public DRCOG.Domain.Models.TipStatusModel GetTipStatus(int rtpYearId)
        {
            var model = new DRCOG.Domain.Models.TipStatusModel();
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var p = db.ProgramInstances.Single(pi => pi.TimePeriodID == rtpYearId);
                var tp = db.TimePeriods.Single(t => t.TimePeriodID == rtpYearId);
                var tpi = db.TIPProgramInstances.Single(pi => pi.TimePeriodID == rtpYearId && pi.TIPProgramID == p.ProgramID);
                model.Adoption = tpi.AdoptionDate;
                model.EPAApproval = tpi.USEPAApprovalDate;
                model.GovernorApproval = tpi.GovernorApprovalDate;
                model.IsCurrent = p.Current.Value;
                model.IsPending = p.Pending.Value;
                model.IsPrevious = p.Previous.Value;
                model.LastAmended = tpi.LastAmendmentDate;
                model.Notes = p.Notes;
                model.ProgramId = p.ProgramID;
                model.PublicHearing = tpi.PublicHearingDate;
                model.ShowDelayDate = tpi.ShowDelayDate;
                model.TimePeriodId = tpi.TimePeriodID;
                model.TipYear = tp.TimePeriod1;
                model.USDOTApproval = tpi.USDOTApprovalDate;

                foreach (var f in tp.FundingIncrements)
                {
                    Logger.Debug("'" + f.FundingIncrement1 + "'");
                }

                var fs = new FundingIncrementSetter(db, tp);
                model.FundingIncrement_Year_1 = fs.GetFundingIncrement("Year 1");
                model.FundingIncrement_Year_2 = fs.GetFundingIncrement("Year 2");
                model.FundingIncrement_Year_3 = fs.GetFundingIncrement("Year 3");
                model.FundingIncrement_Year_4 = fs.GetFundingIncrement("Year 4");
                model.FundingIncrement_Year_5 = fs.GetFundingIncrement("Year 5");
                model.FundingIncrement_Year_6 = fs.GetFundingIncrement("Year 6");
                model.FundingIncrement_Years_4_6 = fs.GetFundingIncrement("Years 4-6");
                model.FundingIncrement_Years_5_6 = fs.GetFundingIncrement("Years 5-6");
            }
            return model;
        }

        /// <summary>
        /// Update the TIP status.
        /// </summary>
        /// <param name="model"></param>
        public void UpdateTipStatus(DRCOG.Domain.Models.TipStatusModel model)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var p = db.ProgramInstances.Single(pi => pi.TimePeriodID == model.TimePeriodId);
                var tp = db.TimePeriods.Single(t => t.TimePeriodID == model.TimePeriodId);
                var tpi = db.TIPProgramInstances.Single(pi => pi.TimePeriodID == model.TimePeriodId && pi.TIPProgramID == p.ProgramID);
                //p.ClosingDate = ?
                p.Current = model.IsCurrent;
                p.Notes = model.Notes;
                p.Pending = model.IsPending;
                p.Previous = model.IsPrevious;

                tpi.AdoptionDate = model.Adoption;
                tpi.GovernorApprovalDate = model.GovernorApproval;
                tpi.PublicHearingDate = model.PublicHearing;
                tpi.ShowDelayDate = model.ShowDelayDate;
                tpi.USDOTApprovalDate = model.USDOTApproval;
                tpi.USEPAApprovalDate = model.EPAApproval;

                var fs = new FundingIncrementSetter(db, tp);
                fs.SetFundingIncrement(model.FundingIncrement_Year_1, "Year 1");
                fs.SetFundingIncrement(model.FundingIncrement_Year_2, "Year 2");
                fs.SetFundingIncrement(model.FundingIncrement_Year_3, "Year 3");
                fs.SetFundingIncrement(model.FundingIncrement_Year_4, "Year 4");
                fs.SetFundingIncrement(model.FundingIncrement_Year_5, "Year 5");
                fs.SetFundingIncrement(model.FundingIncrement_Year_6, "Year 6");
                fs.SetFundingIncrement(model.FundingIncrement_Years_4_6, "Years 4-6");
                fs.SetFundingIncrement(model.FundingIncrement_Years_5_6, "Years 5-6");

                db.SaveChanges();
            }
        }

        // either find the current pending cycle or promote the new cycle. 
        // if neither exists, throw an exception.
        private Models.Cycle RtpAssurePendingCycle(Trips4.Data.Models.TRIPSEntities db, int rtpPlanYearId)
        {
            var tp = db.TimePeriods.First(t => t.TimePeriodID == rtpPlanYearId);
            var pc = tp.TimePeriodCycles.FirstOrDefault(tpc => tpc.Cycle.statusId == (int)Enums.RTPCycleStatus.Pending);
            if (pc == null)
            {
                pc = tp.TimePeriodCycles.FirstOrDefault(tpc => tpc.Cycle.statusId == (int)Enums.RTPCycleStatus.New);
                if (pc == null)
                {
                    throw new Exception("There is no New or Pending Cycle in the RTP Plan Year");
                }
                pc.Cycle.statusId = (int)Enums.RTPCycleStatus.Pending;
            }
            return pc.Cycle;
        }

        /// <summary>
        /// Copy the given projects into the Pending Cycle of the given RTP Plan Year and mark them Pending.
        /// </summary>
        /// <param name="rtpPlanYearId">ID of the RTP Plan Year</param>
        /// <param name="projects">list of RTP ProjectVersion IDs</param>
        public void RtpAmendProjects(int rtpPlanYearId, IEnumerable<int> projects)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var pc = RtpAssurePendingCycle(db, rtpPlanYearId);
                Logger.Debug("RTP Pending Cycle is " + pc.id.ToString());
                db.SaveChanges();

                foreach (var pid in projects)
                {
                    // Due to crappy data, we expect some operations to fail, but we 
                    // want to do as much as possible.
                    try
                    {
                        Logger.Debug("Copy RTPProjectVersion " + pid.ToString());
                        var result = db.RtpCopyProject(pid, null, rtpPlanYearId, pc.id);
                        var npid = result.First().RTPProjectVersionID;
                        Logger.Debug("created RTPProjectVersion " + npid.ToString());

                        // get the newly created RTPProjectVersion
                        var npv = db.RTPProjectVersions.First(p => p.RTPProjectVersionID == npid);

                        // set status to Pending
                        npv.AmendmentStatusID = (int)Enums.RTPAmendmentStatus.Pending;
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("RtpAmendProjects failed for id {0}", pid);
                        Logger.WarnException(message, ex);
                    }
                }

                db.SaveChanges();
            }
        }


        /// <summary>
        /// Mark the projects Adopted, mark the Pending Cycle Active, and
        /// mark the Active Cycle Inactive.
        /// </summary>
        /// <param name="rtpPlanYearId">ID of the RTP Plan Year</param>
        /// <param name="projects">list of RTP ProjectVersion IDs</param>
        public void RtpAdoptProjects(int rtpPlanYearId, IEnumerable<int> projects)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var pc = RtpAssurePendingCycle(db, rtpPlanYearId);
                Logger.Debug("RTP Pending Cycle is " + pc.id.ToString());
                var tp = db.TimePeriods.First(t => t.TimePeriodID == rtpPlanYearId);

                foreach (var pid in projects)
                {
                    // Due to crappy data, we expect some operations to fail, but we 
                    // want to do as much as possible.
                    try
                    {
                        var npv = db.RTPProjectVersions.First(p => p.RTPProjectVersionID == pid);

                        // set statuses (stati?)
                        npv.AmendmentStatusID = (int)Enums.RTPAmendmentStatus.Amended;
                        npv.VersionStatusID = (int)Enums.RTPVersionStatus.Active;

                        // TODO: set status of previous version, if any
                        //var ppv = db.RTPProjectVersions.First(p => p.RTPProjectVersionID == npv.Pre);
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("RtpAdoptProjects failed for id {0}", pid);
                        Logger.WarnException(message, ex);
                    }
                }

                /// Set the current Active Cycle (if any) to Inactive.
                /// Set the current Pending Cycle to Active.
                var ac = tp.TimePeriodCycles.FirstOrDefault(tpc => tpc.Cycle.statusId == (int)Enums.RTPCycleStatus.Active);
                if (ac != null)
                {
                    ac.Cycle.statusId = (int)Enums.RTPCycleStatus.Inactive;
                }
                pc.statusId = (int)Enums.RTPCycleStatus.Active;

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Copy the given projects into the given RTP Plan Year and mark them Pending.
        /// </summary>
        /// <remarks>seems identical to RtpAmendProjects. question the VersionStatus.</remarks>
        /// <param name="rtpPlanYearId">ID of the RTP Plan Year</param>
        /// <param name="projects">list of RTP ProjectVersion IDs</param>
        public void RtpRestoreProjects(int rtpPlanYearId, IEnumerable<int> projects)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var pc = RtpAssurePendingCycle(db, rtpPlanYearId);
                Logger.Debug("RTP Pending Cycle is " + pc.id.ToString());
                db.SaveChanges();

                foreach (var pid in projects)
                {
                    // Due to crappy data, we expect some operations to fail, but we 
                    // want to do as much as possible.
                    try
                    {
                        Logger.Debug("Copy RTPProjectVersion " + pid.ToString());
                        var result = db.RtpCopyProject(pid, null, rtpPlanYearId, pc.id);
                        var npid = result.First().RTPProjectVersionID;
                        Logger.Debug("created RTPProjectVersion " + npid.ToString());

                        // get the newly created RTPProjectVersion
                        var npv = db.RTPProjectVersions.First(p => p.RTPProjectVersionID == npid);

                        // set status to Pending
                        npv.AmendmentStatusID = (int)Enums.RTPAmendmentStatus.Pending;

                        // TODO: this seems to be the only difference
                        npv.VersionStatusID = (int)Enums.RTPVersionStatus.Pending;
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("RtpRestoreProjects failed for id {0}", pid);
                        Logger.WarnException(message, ex);
                    }
                }

                db.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> RtpGetSponsorOrganizations(int rtpPlanYearId)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                var orgs = db.RTPProgramInstanceSponsors.Where(s => s.TimePeriodID == rtpPlanYearId).Select(r => r.SponsorOrganization.Organization).ToArray();
                return orgs.Select(o => new SelectListItem()
                {
                    Text = o.OrganizationName,
                    Value = o.OrganizationID.ToString()
                }).ToArray();
            }
        }

        public void TryCatchError()
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                db.TryCatchError();
            }
        }

        /// <summary>
        /// Add projects to given Survey. Currently, this must be the latest Survey and must be in 
        /// a New status. 
        /// </summary>
        /// <param name="surveyId"></param>
        /// <param name="projects"></param>
        public void SurveyAmendProjects(int surveyId, IEnumerable<int> projects)
        {
            using (var db = new Trips4.Data.Models.TRIPSEntities())
            {
                foreach (var pid in projects)
                {
                    // Due to crappy data, we expect some operations to fail, but we 
                    // want to do as much as possible.
                    try
                    {
                        Logger.Debug("Copy SurveyProjectVersion " + pid.ToString());
                        var ov = new System.Data.Objects.ObjectParameter("NewProjectVersionId", typeof(int));
                        var result = db.SurveyCopyProject(pid, surveyId, null, ov);
                        var npid = ov.Value;
                        Logger.Debug("created SurveyProjectVersion " + npid.ToString());
                    }
                    catch (Exception ex)
                    {
                        var message = string.Format("SurveyAmendProjects failed for id {0}", pid);
                        Logger.WarnException(message, ex);
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
