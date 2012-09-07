using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class ProgramInstanceProjectType
    {
        public int ProjectTypeID { get; set; }
        public int ProgramID { get; set; }
        public short TimePeriodID { get; set; }
        public virtual ProgramInstance ProgramInstance { get; set; }
        public virtual ProjectType ProjectType { get; set; }
        public virtual ProjectType ProjectType1 { get; set; }
    }
}
