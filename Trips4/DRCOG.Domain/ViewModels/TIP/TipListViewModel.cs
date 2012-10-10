//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/3/2009 4:09:10 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using System.Web;
using System.Linq;

namespace DRCOG.Domain.ViewModels.TIP
{
    /// <summary>
    /// Used for the List of TIPs
    /// </summary>
    public class TipListViewModel : TipBaseViewModel
    {
        /// <summary>
        /// List of TIP's in the system
        /// </summary>
        public IList<TipStatusModel> TIPs { get; set; }

        public bool IsAdmin
        {
            get
            {
                return HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator") ? true : false;
            }
        }

        public TipStatusModel CurrentTip
        {
            get
            {
                return TIPs.Where(x => x.IsCurrent).FirstOrDefault();
            }
        }

    }
}
