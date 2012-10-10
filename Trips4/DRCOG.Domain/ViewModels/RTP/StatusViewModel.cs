using System;
using System.Web.Mvc;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.Models.Survey;


namespace DRCOG.Domain.ViewModels.RTP
{
    public class StatusViewModel : RtpBaseViewModel
    {

        //public RtpStatusModel RtpStatus { get; set; }
        public IDictionary<int, string> AvailableYears { get; set; }
        public IDictionary<int, string> AvailableCycles { get; set; }
        public IDictionary<int, string> CurrentPlanCycles { get; set; }
        public IDictionary<int, string> PlanUnusedCycles { get; set; }
        public Surveys Surveys { get; set; }

        public Cycle Cycle { get; set; }

        //public bool IsEditable()
        //{
        //    bool resp = !this.RtpStatus.IsPrevious;
        //    if (!resp && (this.RtpStatus.IsPending || this.RtpStatus.IsCurrent))
        //    {
        //        resp = true;
        //    }
        //    return resp;
        //}
        
        private Dictionary<int, string> _statusList;

        public StatusViewModel()
        {
            _statusList = new Dictionary<int, string>();
        }

        public Dictionary<int, string> StatusList
        {
            get { return _statusList; }
            set { _statusList = value; }
        }

        public SelectList GetAvailableCyclesSelectList()
        {
            return new SelectList(this.AvailableCycles, "value", "key");
        }

        public SelectList GetCurrentPlanCyclesSelectList()
        {
            return new SelectList(this.CurrentPlanCycles, "value", "key");
        }

        public bool IsUnusedCycle(int id)
        {
            return PlanUnusedCycles.ContainsKey(id);
        }

    }
}
