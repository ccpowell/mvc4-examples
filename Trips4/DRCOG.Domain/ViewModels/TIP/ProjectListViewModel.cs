//======================================================
#region  Data Transfer Solutions License
//Copyright (c) 2008 Data Transfer Solutions (www.edats.com)
#endregion
//======================================================
// Author: Nick Kirkes
// Date Created: 8/5/2009 3:36:47 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using MvcContrib.Pagination;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.TIPProject;

namespace DRCOG.Domain.ViewModels.TIP
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectListViewModel : TipBaseViewModel
    {
        public IPagination<TipSummary> Projects { get; set; }

        public IList<TipSummary> ProjectList;

        /// <summary>
        /// Full TIP
        /// </summary>
        public TipModel Tip { get; set; }
    }
}
