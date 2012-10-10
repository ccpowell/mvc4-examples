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
    public class RestoreViewModel : RtpBaseViewModel
    {

        public IList<RtpSummary> ProjectList;

    }
}
