using System;
using DRCOG.Domain.Helpers;

namespace DRCOG.Domain
{
    public class Enums
    {
        public enum Roles
        {
            SystemAdministrator = 1,
            DRCOGUser = 2,
            SponsorUser = 3
        }

        public enum TimePeriodType
        {
            Year = 1
            ,
            TimePeriod = 2
            ,
            PlanYear = 3
            ,
            Survey = 4
        }

        public enum StatusType
        {
            SurveyUpdateStatus = 21
        }

        /// <summary>
        /// Enum describing classification of Dashboard search groupings.
        /// </summary>
        public enum TIPDashboardListType
        {
            Sponsor = 1,
            ProjectType = 2,
            AmendmentStatus = 3,
            ImprovementType = 4
        }

        public enum RTPDashboardListType
        {
            Sponsor = 1,
            ProjectType = 2,
            AmendmentStatus = 3,
            ImprovementType = 4,
            SponsorWithTipid = 5
        }

        public enum SurveyDashboardListType
        {
            MyProjects = 0,
            Sponsor = 1,
            ProjectType = 2,
            UpdateStatus = 3,
            ImprovementType = 4
        }

        public enum OrganizationType
        {
            Sponsor = 2,
            Authority =	3,
            City = 4,
            County = 5,
            FederalAgency =	6,
            Group =	7,
            NonProfit =	8,
            RegionalAgency = 9,
            SpecialDistrict = 10,
            StateAgency = 11,
            TIP = 12,
            Town = 13,
            FundingSourceAgency = 14,
            SelectionAgency = 15,
        }

        /// <summary>
        /// Enum describing roles in the system.
        /// </summary>
        public enum ApplicationState
        {
            [StringValue("Regional Transportation Plan")]
            RTP = 2,
            [StringValue("Transportation Improvement Plan")]
            TIP = 1,
            [StringValue("Transportation Improvement Survey")]
            Survey = 3,
            Default = 0
        }

        public enum TIPAmendmentStatus
        {
            Amended = 1774,
            Approved = 1769,
            Proposed = 1767,
            Restored = 1771,
            Submitted = 1770,
            Adopted = 12783
        }

        public enum RTPAmendmentStatus
        {
            //NotApplicable = 1776,
            [StringValue("Amended")]
            Amended = 12737,
            [StringValue("Approved")]
            Approved = 12739,
            [StringValue("Pending")]
            Pending = 12741,
            //Proposed = 11731,
            [StringValue("Submitted")]
            Submitted = 12745,
            [StringValue("Cancelled")]
            Cancelled = 12778
            //Accepted = 11738
        }

        public enum RTPCycleStatus
        {
            [StringValue("Active")]
            Active = 12759
            ,
            [StringValue("Inactive")]
            Inactive = 12760
            ,
            [StringValue("Pending")]
            Pending = 12758
            ,
            [StringValue("New")]
            New = 12761
        }

        public enum GISCategoryType
        {
            Route = 1,
            CDOTCorridor = 2,
            CDOTRegion = 3,
            FacilityType = 4,
            Facility = 5,
            ModelFacilityType = 6,
            PlanFacilityType = 7
        }

        public enum CategoryType
        {
            CDOTRegion = 27
            ,
            Phase = 25
            ,
            RTPCategory = 24
            ,
            FundingSource = 23
            ,
            AmendmentReason = 21
            ,
            AffectedProjectDelaysLocation = 28
        }

        public enum AmendmentType
        {
            [StringValue("Administrative")]
            Administrative = 545,// 184
            [StringValue("NotApplicable")]
            NotApplicable = 546,// 185
            [StringValue("Policy")]
            Policy = 547// 186
        }
        public enum TIPVersionStatus
        {
            Active = 3736,
            Pending = 3737,
            Inactive = 3738
        }
        public enum RTPVersionStatus
        {
            Active = 3736,
            Inactive = 3738,
            Pending = 3737
        }

        public enum SurveyVersionStatus
        {
            Active = 3736,
            Inactive = 3738,
            Pending = 3737
        }

        public enum RtpTimePeriodStatus
        {
            Current = 12763
            ,
            Pending = 12764
            ,
            CurrentUnlocked = 12765
            ,
            CurrentLocked = 12766
            ,
            Abandoned = 12767
            ,
            Inactive = 12768
            ,
            New = 12769
        }

        public enum SurveyUpdateStatus
        {
            [StringValue("Carryover")]
            Carryover = 6699
            ,
            [StringValue("Reviewed")]
    		Reviewed = 6701
            ,
            [StringValue("BeingEdited")]
	    	Edited = 6702
		    ,
            [StringValue("Accepted")]
            Accepted = 12775
            ,
            [StringValue("New")]
            Current = 12776
            ,
            [StringValue("Withdrawn")]
            Withdrawn = 6703
		    ,
            [StringValue("Completed")]
            Completed = 6704
            ,
            [StringValue("Reclassified")]
            Reclassified = 12770
            ,
            [StringValue("Awaiting Action")]
            AwaitingAction = 12777
            ,
            Revised = 1000 // not in database. used only for logic
        }

        public enum SurveyActionStatus
        {
            Survey = 6694
            ,
            InProcess = 6695
            ,
            Potential = 6696
            ,
            Ineligible = 6697
            ,
            Retired = 6698
            ,
            Replaced = 8671
        }

        public enum SurveyTimePeriodStatus
        {
            Current = 12771
            ,
            Pending = 12772
            ,
            Inactive = 12773
            ,
            Unlocked = 12774
        }

        public enum TipTimePeriodStatus
        {
            Current = 12779
            ,
            Pending = 12780
            ,
            Inactive = 12781
            ,
            CurrentPrior = 12782
        }
    }
}
