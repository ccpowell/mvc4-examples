using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmbMvc4.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public int OmbudsmanId { get; set; }
        public virtual Ombudsman Ombudsman { get; set; }
    }
}