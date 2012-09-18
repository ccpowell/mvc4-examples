using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ombudsman.Models
{
    public class Facility
    {
        public int FacilityId { get; set; }

        [Required]
        public string Name { get; set; }

        public int FacilityTypeId { get; set; }
        public string FacilityTypeName { get; set; }

        public int? OmbudsmanId { get; set; }
        public string OmbudsmanName { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool IsActive { get; set; }
        public int? NumberOfBeds { get; set; }
        public bool? IsMedicaid { get; set; }
        public bool? IsContinuum { get; set; }
    }
}
