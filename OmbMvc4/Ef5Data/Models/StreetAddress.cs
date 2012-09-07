using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class StreetAddress
    {
        public int StreetAddressID { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public virtual Address Address { get; set; }
    }
}
