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
using System.Collections.Generic;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP
{
    /// <summary>
    /// Used for the List of Plans
    /// </summary>
    public class RtpListViewModel : RtpBaseViewModel
    {
        /// <summary>
        /// List of Plans in the system
        /// </summary>
        public IList<RtpStatusModel> RTPs { get; set; }

    }
}
