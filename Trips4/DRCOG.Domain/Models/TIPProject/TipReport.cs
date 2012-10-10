#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		REMARKS
 * 08/06/2009	Unknown     1. Initial Creation (DTS).
 * 02/03/2010	DDavidson	2. Reformatted. Several improvements.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.ObjectModel;

namespace DRCOG.Domain.Models.TIPProject
{
    public class TipReport : Report
    {
        public TipReportProjects projects { get; set; }

        
    }

    public class TipReportProjects : List<TipReportProject>
    {
        
    }

    public class TipReportProject : TipSummary
    {
        public bool IsOnHold { get; set; }

    }

    public class TipReports : List<TipReport>
    {
        public bool HasCurrentPolicy()
        {
            return this.Exists(x => x.Name == "CurrentPolicy");
        }

        
    }
}
