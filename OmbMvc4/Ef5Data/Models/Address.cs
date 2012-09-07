using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        public int AddressTypeID { get; set; }
        public string Address1 { get; set; }
        public Nullable<int> PersonID { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string CellNumber { get; set; }
        public virtual AddressType AddressType { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual Person Person { get; set; }
        public virtual StreetAddress StreetAddress { get; set; }
    }
}
