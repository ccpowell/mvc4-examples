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

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class TipEligibleAgenciesModel:TipModel
    {

        public TipEligibleAgenciesModel()
        {
            EligibleAgencies = new List<Organization>();
            AvailableAgencies = new List<Organization>();
        }

        #region TIP Eligible Agencies

        /// <summary>
        /// Agencies Eligible for the TIP
        /// </summary>
        public IList<Organization> EligibleAgencies { get; set; }

        /// <summary>
        /// Agencies Available for the TIP
        /// </summary>
        public IList<Organization> AvailableAgencies { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SelectList GetEligibleAgencySelectList()
        {
            return new SelectList(this.EligibleAgencies, "AgencyId", "AgencyName");
        }

        public SelectList GetAvailableAgencySelectList()
        {
            return new SelectList(this.AvailableAgencies, "AgencyId", "AgencyName");
        }




        #endregion

    }
}
