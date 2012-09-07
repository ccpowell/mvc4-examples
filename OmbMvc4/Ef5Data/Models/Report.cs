using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Report
    {
        public Report()
        {
            this.Processeds = new List<Processed>();
        }

        public System.Guid Id { get; set; }
        public string ReportYear { get; set; }
        public virtual ICollection<Processed> Processeds { get; set; }
    }
}
