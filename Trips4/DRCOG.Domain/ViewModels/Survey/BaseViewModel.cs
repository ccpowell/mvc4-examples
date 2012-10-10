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
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ViewModels.Survey
{
    /// <summary>
    /// </summary>
    public class BaseViewModel
    {

        public BaseViewModel()
        {
            Current = new DRCOG.Domain.Models.Survey.Survey();
            Person = new Person();
        }

        public DRCOG.Domain.Models.Survey.Survey Current { get; set; }
        //public Profile Person { get; set; }
        public Person Person { get; set; }

        


     
    }
}
