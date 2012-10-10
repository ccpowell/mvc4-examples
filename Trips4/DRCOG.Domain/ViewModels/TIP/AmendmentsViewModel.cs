//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/3/2009 2:58:48 PM
// Description:
//
//======================================================
using System;
using DRCOG.Domain.Models;
using System.Collections.Generic;

namespace DRCOG.Domain.ViewModels.TIP
{
    /// <summary>
    /// </summary>
    public class AmendmentsViewModel : TipBaseViewModel
    {
        public IDictionary<int, string> RestoreYears { get; set; }

    }
}
