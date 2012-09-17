using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ombudsman.Models
{
    public class Ombudsman
    {
        public int OmbudsmanId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
