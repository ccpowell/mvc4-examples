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
using DRCOG.Domain.Models.Survey;
using System.Collections.Generic;

namespace DRCOG.Domain.ViewModels.Survey
{
    /// <summary>
    /// </summary>
    public class ProjectBaseViewModel : BaseViewModel
    {

        public ProjectBaseViewModel()
        {
            Project = new Project();
        }

        public Project Project { get; set; }

        public IDictionary<int, string> AvailableUpdateStatus { get; set; }

    }
}
