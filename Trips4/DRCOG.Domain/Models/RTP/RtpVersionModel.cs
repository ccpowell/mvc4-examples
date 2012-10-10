#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 03/05/2010	DTucker         1. Initial Creation.
 * 
 * DESCRIPTION:
 * Context class for handling Project Versions
 * ======================================================*/
#endregion

using System;
//using DRCOG.Domain.Models.TIPProject.Amendment;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DRCOG.Domain.Models.RTP
{
    public class RtpVersionModel : ProjectModel, IInstance
    {
        private string[] CAN_DELETE_STATUS = { "approved", "submitted", "amended", "proposed" };

        public string RtpYear { get; set; }
        public string RtpId { get; set; }
        public int TimePeriodStatusId { get; set; }
        //public int CycleId { get; set; }
        //public string Cycle { get; set; }
        public Cycle Cycle { get; set; }

        public RtpVersionModel()
        {
            Cycle = new Cycle();
        }

        public IEnumerable<int> InAmendmentCheck = new int[] { (int)Enums.TIPAmendmentStatus.Proposed, (int)Enums.TIPAmendmentStatus.Submitted };
        public bool IsInAmendment
        {
            get { return InAmendmentCheck.Contains(AmendmentStatusId); }
        }
        
        public Boolean CanDelete(String status)
        {
            if (((IList<string>)CAN_DELETE_STATUS).Contains(status.ToLower()))
            {
                return true;
            }
            return false;
        }


        #region IVersionModel Members

        public bool IsEditable()
        {
            if (AmendmentStatusId.Equals((int)Enums.RTPAmendmentStatus.Cancelled)) return false;

            if (IsCycleEditable() && (HttpContext.Current.User.IsInRole("RTP Administrator") || HttpContext.Current.User.IsInRole("Administrator")))// && IsPending)
                return true;
                
            return false;
        }

        public bool IsAmendable()
        {
            if (AmendmentStatusId.Equals((int)Enums.RTPAmendmentStatus.Cancelled)) return false;
            if (IsCycleEditable())// && (IsPending || IsCurrent))
                return true;

            return false;
        }

        public bool IsCycleEditable()
        {
            if (TimePeriodStatusId.Equals((int)Enums.RtpTimePeriodStatus.Current) ||
                TimePeriodStatusId.Equals((int)Enums.RtpTimePeriodStatus.CurrentUnlocked) ||
                TimePeriodStatusId.Equals((int)Enums.RtpTimePeriodStatus.Pending))
            {
                if (!Cycle.NextCycleId.Equals(default(int))) { return false; }
                if (Cycle.StatusId.Equals((int)Enums.RTPCycleStatus.Active) ||
                    (Cycle.StatusId.Equals((int)Enums.RTPCycleStatus.Inactive) && 
                        Cycle.NextCycleId.Equals(default(int)))) 
                { return true; }
            }
            else if (TimePeriodStatusId.Equals((int)Enums.RtpTimePeriodStatus.New))
            {
                var test = Cycle.StatusId;
                return true;
            }

            if ((!TimePeriodStatusId.Equals((int)Enums.RtpTimePeriodStatus.Inactive) ||
                !TimePeriodStatusId.Equals((int)Enums.RtpTimePeriodStatus.Abandoned)) &&
                Cycle.StatusId.Equals((int)Enums.RTPCycleStatus.Pending)) 
            { return true; }
            
            return false;
        }

        public string GetStatus()
        {
            switch (TimePeriodStatusId)
            {
                case (int)Enums.RtpTimePeriodStatus.Abandoned:
                    return "Abandoned";
                case (int)Enums.RtpTimePeriodStatus.Current:
                    return "Current";
                case (int)Enums.RtpTimePeriodStatus.CurrentLocked:
                    return "Current Locked";
                case (int)Enums.RtpTimePeriodStatus.CurrentUnlocked:
                    return "Current Unlocked";
                case (int)Enums.RtpTimePeriodStatus.Inactive:
                    return "Inactive";
                case (int)Enums.RtpTimePeriodStatus.New:
                    return "New";
                case (int)Enums.RtpTimePeriodStatus.Pending:
                    return "Pending";
                default:
                    return "not set";
            }
        }

        private string _nextStatusText;

        public string NextStatusText
        {
            get
            {
                GetNextStatus();
                return _nextStatusText;
            }
        }

        public int GetNextStatus()
        {
            switch (TimePeriodStatusId)
            {
                case (int)Enums.RtpTimePeriodStatus.Current:
                    _nextStatusText = "Lock Plan";
                    return (int)Enums.RtpTimePeriodStatus.CurrentLocked;
                case (int)Enums.RtpTimePeriodStatus.CurrentLocked:
                    _nextStatusText = "Unlock Plan";
                    return (int)Enums.RtpTimePeriodStatus.CurrentUnlocked;
                case (int)Enums.RtpTimePeriodStatus.CurrentUnlocked:
                    _nextStatusText = "Lock Plan";
                    return (int)Enums.RtpTimePeriodStatus.CurrentLocked;
                case (int)Enums.RtpTimePeriodStatus.Inactive:
                    _nextStatusText = "Set Pending";
                    return (int)Enums.RtpTimePeriodStatus.Pending;
                case (int)Enums.RtpTimePeriodStatus.New:
                    _nextStatusText = "Set Pending";
                    return (int)Enums.RtpTimePeriodStatus.Pending;
                case (int)Enums.RtpTimePeriodStatus.Abandoned:
                    _nextStatusText = "Set Pending";
                    return (int)Enums.RtpTimePeriodStatus.Pending;
                case (int)Enums.RtpTimePeriodStatus.Pending:
                    _nextStatusText = "Set Current";
                    return (int)Enums.RtpTimePeriodStatus.Current;
                default:
                    return default(int);
            }
        }

        public int AmendmentStatusId { get; set; }

        public bool CanDrop
        {
            get
            {
                if (AmendmentStatusId.Equals((int)Enums.RTPAmendmentStatus.Pending) && IsCycleEditable())
                    return true;
                return false;
            }
        }
        public bool IsActive { get; set; }
        //public bool IsCurrent { get; set; }
        //public bool IsPending { get; set; }
        //public bool IsEditable { get; set; }
        public bool IsTopStatus { get; set; }
        public int ProjectVersionId { get; set; }
        public string Year { get; set; }

        #endregion
    }
}
