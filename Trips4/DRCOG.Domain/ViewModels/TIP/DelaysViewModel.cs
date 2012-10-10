using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models.TIP;

namespace DRCOG.Domain.ViewModels.TIP
{
    public class DelaysViewModel : TipBaseViewModel
    {
        public string DelayYear { get; set; }
        public IEnumerable<Delay> Delays { get; set; }
        public IEnumerable<string> DelayYears { get; set; }
    }
}
