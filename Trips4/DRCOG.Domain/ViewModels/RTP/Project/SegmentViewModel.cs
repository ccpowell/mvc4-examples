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
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP.Project
{
    /// <summary>
    /// ViewModel for Segment partial view.
    /// Decided not to derive from ProjectBaseViewModel because that
    /// was way too much extra info and also it crossed repositories. -DBD
    /// </summary>
    public class SegmentViewModel //: ProjectBaseViewModel
    {
        public int ProjectVersionID { get; set; }
        public IList<SegmentModel> Segments { get; set; }
    }
}
