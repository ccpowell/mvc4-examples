//======================================================
#region  Data Transfer Solutions License
//Copyright (c) 2008 Data Transfer Solutions (www.edats.com)
#endregion
//======================================================
// Author: Nick Kirkes
// Date Created: 8/6/2009 2:40:30 PM
// Description:
//
//======================================================
using System;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain.Models.TIPProject;
using System.Collections.Generic;

namespace DRCOG.Domain.ViewModels.TIPProject
{
    /// <summary>
    /// Base class for all Project related Views
    /// </summary>
    public class ProjectBaseViewModel
    {
        /// <summary>
        /// Basic information needed across the Project Views.
        /// </summary>
        public TipSummary ProjectSummary { get; set; }
        public IDictionary<int, string> AmendmentTypes { get; set; }
    }

}
