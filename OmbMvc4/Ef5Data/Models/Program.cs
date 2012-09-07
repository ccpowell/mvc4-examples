using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Program
    {
        public Program()
        {
            this.ProgramInstances = new List<ProgramInstance>();
        }

        public int ProgramID { get; set; }
        public string Program1 { get; set; }
        public virtual ICollection<ProgramInstance> ProgramInstances { get; set; }
    }
}
