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

namespace DRCOG.Domain.ViewModels.Survey
{
    /// <summary>
    /// Used for the List of Surveys
    /// </summary>
    public class ListViewModel : BaseViewModel
    {
        /// <summary>
        /// List of Surveys in the system
        /// </summary>
        public IList<DRCOG.Domain.Models.Survey.Survey> SurveyList { get; set; }

    }
}
