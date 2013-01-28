using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using System.Linq;
using System.Web.Mvc;
//using DTS.Extensions;
using DRCOG.Entities;
using DRCOG.Domain;
using DRCOG.Domain.Interfaces;


namespace DRCOG.Domain.ViewModels.TIP
{

    public class TipDashboardViewModel : TipBaseViewModel
    {

        public TipDashboardViewModel()
        {
            this.DashboardItems = new List<DashboardListItem>();            
        }

        public IList<DashboardListItem> DashboardItems { get; set; }        
        public string ErrorMessage { get; set; }
        public Enums.TIPDashboardListType ListType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SelectList GetProjectListTypes()
        {
            var types = from Enums.TIPDashboardListType t in Enum.GetValues(typeof(Enums.TIPDashboardListType)) 
                        where !t.Equals(Enums.TIPDashboardListType.AmendmentStatus)
                        select new { ID = t, Name = t.ToString() };

            return new SelectList(types, "ID", "Name", this.ListType.ToString());
        }
        
    }
}
