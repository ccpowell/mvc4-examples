#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR	        REMARKS
 * 02/08/2010	DDavidson       1. Initial Creation. 
 * 
 * DESCRIPTION:
 * Manages list of Primary and Secondary sponsors within the General Info tab.
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Collections;
using DRCOG.Domain.Helpers;

namespace DRCOG.Domain.Models.RTP
{
    public class ProjectSponsorsModel
    {
        //public int TipId { get; set; }
        //public bool IsEditable { get; set; }
        //public string SelectedEligibleAgencies { get; set; }

        public ProjectSponsorsModel()
        {
            CurrentAgencies = new List<SponsorOrganization>();
            AvailableAgencies = new List<SponsorOrganization>();
        }

        /// <summary>
        /// Agencies assigned to this project
        /// </summary>
        public IList<SponsorOrganization> CurrentAgencies { get; set; }

        public SponsorOrganization PrimarySponsor { get; set; }

        /// <summary>
        /// Agencies available for this project
        /// </summary>
        public IList<SponsorOrganization> AvailableAgencies { get; set; }


        public IDictionary<int, string> GetAvailableAgenciesList()
        {
            return AvailableAgencies.ToDictionary(x => (int)x.OrganizationId, x => x.OrganizationName);
        }

        public String SponsorContact { get; set; }

        public SelectList GetCurrent1AgencySelectList()
        {
            IEnumerable<SponsorOrganization> primarySponsors = (
                from ps in CurrentAgencies
                where ps.IsPrimary == true
                select ps);
            return new SelectList(primarySponsors, "AgencyId", "AgencyName");
        }

        //public IList<SponsorOrganization> GetCurrent1Agency()
        //{
        //    IEnumerable<SponsorOrganization> primarySponsors = (
        //        from ps in CurrentAgencies
        //        where ps.IsPrimary == true
        //        select ps);
            

        //    return primarySponsors.ToList<SponsorOrganization>();
        //}

        //public SelectList GetCurrent2AgencySelectList()
        //{
        //    IEnumerable<SponsorOrganization> secondarySponsors = (
        //        from ps in CurrentAgencies
        //        where ps.IsPrimary == false
        //        select ps);
        //    return new SelectList(secondarySponsors, "AgencyId", "AgencyName");
        //}

        //public String GetCurrent2AgencyString()
        //{
        //    IEnumerable<String> secondarySponsors = (
        //        from ps in CurrentAgencies
        //        where ps.IsPrimary == false
        //        select ps.OrganizationName);

        //    return secondarySponsors.ToString(",");
        //}
        
        public SelectList GetAvailableAgencySelectList()
        {
            return new SelectList(this.AvailableAgencies, "value", "key");
        }

    }
}
