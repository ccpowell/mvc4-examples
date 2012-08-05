using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmbMf.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }
        public string Name { get; set; }
        public Ombudsman Ombudsman { get; set; }
        public int? OmbudsmanId { get; set; }
    }
}