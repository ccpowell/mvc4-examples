using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
//using DTS.Extensions;
using DRCOG.Entities;
using DRCOG.Domain;
using DRCOG.Domain.Interfaces;


namespace DRCOG.Domain.ViewModels.Survey
{

    public class DashboardViewModel : BaseViewModel
    {

        public DashboardViewModel()
        {
            this.DashboardItems = new List<DashboardListItem>();            
        }

        public string Year { get; set; }
        public IList<DashboardListItem> DashboardItems { get; set; }        
        public string ErrorMessage { get; set; }
        public Enums.SurveyDashboardListType ListType { get; set; }
        public bool ShowAll { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SelectList GetProjectListTypes()
        {
            var types = from Enums.SurveyDashboardListType t in Enum.GetValues(typeof(Enums.SurveyDashboardListType))
                        select new { ID = t, Name = t.ToString() };

            return new SelectList(types, "ID", "Name", this.ListType.ToString());
        }
        
    }
}
