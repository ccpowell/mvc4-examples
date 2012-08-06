using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OmbMf.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }

        [Required]
        public string Name { get; set; }
        public int FacilityTypeId { get; set; }
        public FacilityType FacilityType { get; set; }
        public int? OmbudsmanId { get; set; }
        public Ombudsman Ombudsman { get; set; }
    }
}