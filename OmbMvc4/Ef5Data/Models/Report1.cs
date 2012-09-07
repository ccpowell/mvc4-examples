using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Report1
    {
        public Report1()
        {
            this.ReportProjectVersionSortings = new List<ReportProjectVersionSorting>();
        }

        public int Id { get; set; }
        public short TimePeriodId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public virtual TimePeriod TimePeriod { get; set; }
        public virtual ICollection<ReportProjectVersionSorting> ReportProjectVersionSortings { get; set; }
    }
}
