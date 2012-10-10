using System;
using System.Collections.Generic;
using MvcContrib.Pagination;
using DRCOG.Domain.Models.Survey;

namespace DRCOG.Domain.ViewModels.Survey
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectListViewModel : BaseViewModel
    {
        public IPagination<Project> Projects { get; set; }

        public List<Project> ProjectList;
        

        public List<Project> RestorableProjectList;

        /// <summary>
        /// Full RTP
        /// </summary>
        //public RtpBase Rtp { get; set; }
        public string ListCriteria { get; set; }
        public string ListType { get; set; }
    }
}
