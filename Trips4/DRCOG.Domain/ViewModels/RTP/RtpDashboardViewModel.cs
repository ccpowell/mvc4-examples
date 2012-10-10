using System;
using System.Collections.Generic;
using DRCOG.Domain.Models;
using System.Linq;
using System.Web.Mvc;
using DTS.Extensions;
using DRCOG.Entities;
using DRCOG.Domain;
using DRCOG.Domain.Interfaces;


namespace DRCOG.Domain.ViewModels.RTP
{

    public class RtpDashboardViewModel : RtpBaseViewModel
    {

        public RtpDashboardViewModel()
        {
            this.DashboardItems = new List<DashboardListItem>();            
        }

        public IList<DashboardListItem> DashboardItems { get; set; }        
        public string ErrorMessage { get; set; }
        public Enums.RTPDashboardListType ListType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SelectList GetProjectListTypes()
        {
            var types = from Enums.RTPDashboardListType t in Enum.GetValues(typeof(Enums.RTPDashboardListType))
                        select new { ID = t, Name = t.ToPrettyString() };

            return new SelectList(types, "ID", "Name", this.ListType.ToString());
        }
        
    }
}
