//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/23/2009 4:05:52 PM
// Description:
//
//======================================================

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ViewModels.RTP
{
    public class SponsorsViewModel : RtpBaseViewModel
    {
        public SponsorsViewModel()
        {
            EligibleAgencies = new List<Organization>();
            AvailableAgencies = new List<Organization>();
        }


        public bool IsEditable { get; set; }

        /// <summary>
        /// Agencies Eligible for the TIP
        /// </summary>
        public IList<Organization> EligibleAgencies { get; set; }

        /// <summary>
        /// Agencies Available for the TIP
        /// </summary>
        public IList<Organization> AvailableAgencies { get; set; }

        public string SelectedEligibleAgencies { get; set; }

        public SelectList GetEligibleAgencySelectList()
        {
            return new SelectList(this.EligibleAgencies, "OrganizationId", "OrganizationName");
        }

        public SelectList GetAvailableAgencySelectList()
        {
            return new SelectList(this.AvailableAgencies, "AgencyId", "AgencyName");
        }

    }
}
