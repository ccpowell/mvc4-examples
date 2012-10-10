using System;
using System.ComponentModel;

namespace DRCOG.Domain.Models
{
    public class Organization
    {
        [DisplayName("Source")]
        public int? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string LegalName { get; set; }
    }
}
