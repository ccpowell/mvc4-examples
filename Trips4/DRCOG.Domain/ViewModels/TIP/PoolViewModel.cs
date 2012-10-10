//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/20/2009 2:15:07 PM
// Description:
//
//======================================================
using System;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ViewModels.TIP
{
    /// <summary>
    /// </summary>
    public class PoolViewModel:TipBaseViewModel
    {
        /// <summary>
        /// The TIP Year
        /// </summary>
        public string TipYear { get; set; }
        /// <summary>
        /// The Pool
        /// </summary>
        public PoolModel Pool { get; set; }

    }
}
