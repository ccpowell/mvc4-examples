//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/15/2009 12:13:52 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;


namespace DRCOG.Domain.ViewModels
{
    /// <summary>
    /// Model for the Amendment List View
    /// </summary>
    public class AmendmentListViewModel 
    {
        /// <summary>
        /// Project Summary
        /// ToDo: Determine if I need to build this. What would such a summary contain?
        /// </summary>
        //public ProjectSummary ProjectSummary { get; set; }

        /// <summary>
        /// Tip Summary
        /// </summary>
        public TipSummary TipSummary { get; set; }

        /// <summary>
        /// List of the amendments associated with this project
        /// </summary>
        public IList<AmendmentSummary> Amendments { get; set; }

    }
}
