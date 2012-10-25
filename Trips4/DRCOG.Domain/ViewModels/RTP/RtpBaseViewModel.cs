#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			REMARKS
 * 05/05/2010	DDavidson       1. Initial Creation. 
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.RTP;
using System.Collections.Generic;

namespace DRCOG.Domain.ViewModels.RTP
{
    /// <summary>
    /// </summary>
    public class RtpBaseViewModel
    {

        public RtpBaseViewModel()
        {
            this.RtpSummary = new RtpSummary();
            this.RtpStatus = new RtpStatusModel();
        }

        /// <summary>
        /// Basic summary information needed on all ViewModels
        /// </summary>
        public RtpSummary RtpSummary { get; set; }
        public RtpStatusModel RtpStatus { get; set; }
        public IDictionary<int, string> AvailableSponsors { get; set; }
        /// <summary>
        /// Agencies Eligible for the TIP
        /// </summary>
        //public IList<Organization> EligibleAgencies { get; set; }
        public IDictionary<int, string> EligibleAgencies { get; set; }

        public string ReturnUrl { get; set; }

        public List<KeyValuePair<int, string>> CurrentPlanCycles { get; set; }
        public List<KeyValuePair<int, string>> SurveyYears { get; set; }
    }
}
