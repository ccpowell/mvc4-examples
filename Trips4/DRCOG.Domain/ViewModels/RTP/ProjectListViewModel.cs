using System;
using System.Collections.Generic;
using MvcContrib.Pagination;
using DRCOG.Domain.Models;
using DRCOG.Domain.Models.RTP;

namespace DRCOG.Domain.ViewModels.RTP
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectListViewModel : RtpBaseViewModel
    {
        public IPagination<RtpSummary> Projects { get; set; }

        public IList<RtpSummary> ProjectList;

        public IList<RtpSummary> RestorableProjectList;

        /// <summary>
        /// Full RTP
        /// </summary>
        public RtpBase Rtp { get; set; }
        public string ListCriteria { get; set; }
        public string ListType { get; set; }
    }
}
