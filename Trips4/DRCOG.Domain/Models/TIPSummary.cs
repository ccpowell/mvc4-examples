#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			        REMARKS
 * 07/01/2009	Dave Bouwman     1. Initial Creation (DTS). 
 * 01/25/2010	Danny Davidson	2. Reformatted.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// Light-weight Summary of the TIP.
    /// Does not and should not contain all the associated collections 
    /// and details of the TIP itself.
    /// </summary>
    public class TipSummary
    {               
        public string TipYear {get; set;}
        public bool IsCurrent { get; set; }
    }
}
