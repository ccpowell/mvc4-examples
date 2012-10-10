using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.Models
{
    public class TIPSearchModel : ProjectSearchModel
    {
        public string TipID { get; set; }
        public string TipYear { get; set; }
        public int? TipYearID { get; set; }

        public bool Exclude_TipID { get; set; }
        public bool Exclude_TipYear { get; set; }

        public string StipId { get; set; }
        public int CdotRegionId { get; set; }
    }
}
