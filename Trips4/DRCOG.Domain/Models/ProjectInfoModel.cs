//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: Michael Hayden
// Date Created: 8/4/2009 11:23:22 am
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// </summary>
    public class ProjectInfoModel:ProjectModel
    {
        //public ProjectInfoModel()
        //{
        //    //this.EligibleAgencies = new List<Agency>();
        //    //this.AvailableAgencies = new List<Agency>();
        //}
        
        #region Project Information

        public IList<Organization> Sponser { get; set; }
        
        #endregion
        //#region TIP Eligible Agencies

        ///// <summary>
        ///// Agencies Eligible for the TIP
        ///// </summary>
        //public IList<Agency> EligibleAgencies { get; set; }

        ///// <summary>
        ///// Agencies Available for the TIP
        ///// </summary>
        //public IList<Agency> AvailableAgencies { get; set; }

        //public string SelectedEligibleAgencies { get; set; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public SelectList GetEligibleAgencySelectList()
        //{
        //    return new SelectList(this.EligibleAgencies, "AgencyId", "AgencyName");
        //}

        //public SelectList GetAvailableAgencySelectList()
        //{
        //    return new SelectList(this.AvailableAgencies, "AgencyId", "AgencyName");
        //}

        //#endregion

    }

}
