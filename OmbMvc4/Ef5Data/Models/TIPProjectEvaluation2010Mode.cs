using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class TIPProjectEvaluation2010Mode
    {
        public TIPProjectEvaluation2010Mode()
        {
            this.TIPProjectEvaluation2010 = new List<TIPProjectEvaluation2010>();
        }

        public int ModeID { get; set; }
        public string Mode { get; set; }
        public virtual ICollection<TIPProjectEvaluation2010> TIPProjectEvaluation2010 { get; set; }
    }
}
