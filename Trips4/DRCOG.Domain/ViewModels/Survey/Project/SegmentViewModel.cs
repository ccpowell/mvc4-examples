#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 02/09/2010	DDavidson       1. Initial Creation.
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.Survey;

namespace DRCOG.Domain.ViewModels.Survey
{
    public class SegmentViewModel
    {
        public int ProjectVersionID { get; set; }
        public IList<SegmentModel> Segments { get; set; }
    }
}
