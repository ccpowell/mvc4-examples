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
using DRCOG.Domain.Interfaces;
using DRCOG.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace DRCOG.Domain.Models.Survey
{
    public class Instance : InstanceSecurity, IInstance
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public int SponsorContactId { get; set; }
        public string SponsorContact { get; set; }
        public int ProjectVersionId { get; set; }
        public int PreviousVersionId { get; set; }
        public int TimePeriodId { get; set; }
        public string TimePeriod { get; set; }
        public int AmendmentStatusId { get; set; }
        
        public bool IsActive { get; set; }
        public bool IsTopStatus { get; set; }
        public string Scope { get; set; }
        public string FacilityName { get; set; }
        public string Limits { get; set; }

        [DisplayName("Start At:")]
        public string BeginAt { get; set; }
        [DisplayName("End At:")]
        public string EndAt { get; set; }
        public string SponsorNotes { get; set; }
        public string DRCOGNotes { get; set; }
        public int BeginContructionYear { get; set; }
        public int EndConstructionYear { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public int AmendmentFundingTypeId { get; set; }
        public string AmendmentFundingType { get; set; }
        public int TransportationTypeId { get; set; }
        public string TransportationType { get; set; }
        public int AdministrativeLevelId { get; set; }
        public string AdministrativeLevel { get; set; }
        public int SelectorId { get; set; }
        public string Selector { get; set; }
        public int ImprovementTypeId { get; set; }
        public string ImprovementType { get; set; }


        public Instance() { }

        

        //public string GetStatus()
        //{
        //    switch (TimePeriodStatusId)
        //    {
        //        case (int)Enums.RtpTimePeriodStatus.Abandoned:
        //            return "Abandoned";
        //        case (int)Enums.RtpTimePeriodStatus.Current:
        //            return "Current";
        //        case (int)Enums.RtpTimePeriodStatus.CurrentLocked:
        //            return "Current Locked";
        //        case (int)Enums.RtpTimePeriodStatus.CurrentUnlocked:
        //            return "Current Unlocked";
        //        case (int)Enums.RtpTimePeriodStatus.Inactive:
        //            return "Inactive";
        //        case (int)Enums.RtpTimePeriodStatus.New:
        //            return "New";
        //        case (int)Enums.RtpTimePeriodStatus.Pending:
        //            return "Pending";
        //        default:
        //            return "not set";
        //    }
        //}

        //public int GetNextStatus()
        //{
        //    switch (TimePeriodStatusId)
        //    {
        //        case (int)Enums.RtpTimePeriodStatus.Current:
        //            return (int)Enums.RtpTimePeriodStatus.CurrentLocked;
        //        case (int)Enums.RtpTimePeriodStatus.CurrentLocked:
        //            return (int)Enums.RtpTimePeriodStatus.CurrentUnlocked;
        //        case (int)Enums.RtpTimePeriodStatus.CurrentUnlocked:
        //            return (int)Enums.RtpTimePeriodStatus.CurrentLocked;
        //        case (int)Enums.RtpTimePeriodStatus.Inactive:
        //            return (int)Enums.RtpTimePeriodStatus.Pending;
        //        case (int)Enums.RtpTimePeriodStatus.New:
        //            return (int)Enums.RtpTimePeriodStatus.Pending;
        //        case (int)Enums.RtpTimePeriodStatus.Abandoned:
        //            return (int)Enums.RtpTimePeriodStatus.Pending;
        //        default:
        //            return default(int);
        //    }
        //}

        

    }
}
