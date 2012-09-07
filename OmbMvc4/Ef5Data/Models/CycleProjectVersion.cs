using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class CycleProjectVersion
    {
        public int ProjectVersionId { get; set; }
        public int CycleId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public virtual Cycle Cycle { get; set; }
        public virtual RTPProjectVersion RTPProjectVersion { get; set; }
    }
}
