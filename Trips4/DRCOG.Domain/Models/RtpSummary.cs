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

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Light-weight Summary of the RTP.
    /// Does not and should not contain all the associated collections 
    /// and details of the RTP itself.
    /// </summary>
    public class RtpSummary
    {               
        public string RtpYear {get; set;}
        public bool IsCurrent { get; set; }
    }
}
