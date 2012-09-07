using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace ConModels
{
    public class User
    {
        public Guid Id { get; set; }

        [DisplayName("Home Address")]
        public string Address { get; set; }

        [DisplayName("Alternate Email")]
        public virtual string AlternateEmail { get; set; }

        [DisplayName("Business Email")]
        public virtual string BusinessEmail { get; set; }

        [DisplayName("Home City")]
        public string City { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Personal Email")]
        public virtual string HomeEmail { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Organization")]
        public string Organization { get; set; }

        [DisplayName("Primary Contact Number")]
        public string Phone { get; set; }

        [DisplayName("Recovery Email")]
        public virtual string RecoveryEmail { get; set; }

        [DisplayName("Sponsor Code")]
        public string SponsorCode { get; set; }

        [DisplayName("Home State")]
        public string State { get; set; }

        [DisplayName("Home Unit")]
        public string Unit { get; set; }

        [DisplayName("UserName")]
        public string UserName { get; set; }

        [DisplayName("Title")]
        public string Title { get; set; }

        [DisplayName("Home Zipcode")]
        public string ZipCode { get; set; }

        [DisplayName("Comment")]
        public string Comment { get; set; }
    }
}
