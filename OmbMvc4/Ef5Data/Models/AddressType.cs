using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class AddressType
    {
        public AddressType()
        {
            this.Addresses = new List<Address>();
        }

        public int AddressTypeID { get; set; }
        public string AddressType1 { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
