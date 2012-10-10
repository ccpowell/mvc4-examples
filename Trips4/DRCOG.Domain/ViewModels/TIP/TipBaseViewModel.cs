#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			        REMARKS
 * 08/06/2009	Dave Bouwman     1. Initial Creation (DTS). 
 * 01/25/2010	Danny Davidson	2. Reformatted.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;
using System.Collections.Generic;

namespace DRCOG.Domain.ViewModels.TIP
{
    /// <summary>
    /// </summary>
    public class TipBaseViewModel
    {

        public TipBaseViewModel()
        {
            this.TipSummary = new TipSummary();
        }

        /// <summary>
        /// Basic summary information needed on all ViewModels
        /// </summary>
        public TipSummary TipSummary { get; set; }
        public IDictionary<int, string> AvailableSponsors { get; set; }
        public IDictionary<int, string> CurrentSponsors { get; set; }
        public IDictionary<int, string> AmendmentTypes { get; set; }

    }
}
