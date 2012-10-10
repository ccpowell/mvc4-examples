using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ViewModels.TIPProject
{
    public class VersionDetailsJson
    {
        public InfoModel InfoModel { get; set; }
        public FundingModel TipProjectFunding { get; set; }
        public IDictionary<string, string> GeneralInfo { get; set; }

        public string ProjectUrl { get; set; }
    }
}
