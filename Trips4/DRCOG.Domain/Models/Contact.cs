using System;
using System.Collections.Generic;
using DRCOG.Common.Util;
using System.Runtime.Serialization;

namespace DRCOG.Domain.Models
{
    [DataContract]
    public class Contact
    {
        [DataMember]
        public int PersonId { get; set; }
        [DataMember]
        public Guid PersonGuid { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleInitial { get; set; }
        [DataMember]
        public string Division { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public DateTime CreationDate { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Fax { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public Address Address { get; set; }
        // not persisted
        public string FullName
        {
            get { return String.Format("{0}, {1} ", LastName, FirstName); }
        }

        [DataMember]
        public string PersonShortGuid
        {
            get { return ShortGuid.Encode(PersonGuid); }
            set { this.PersonGuid = ShortGuid.Decode(value); }
        }
    }

    [DataContract]
    public class ContactRole : Contact
    {
        [DataMember]
        public int OrganizationId { get; set; }
        [DataMember]
        public string OrganizationName { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        [DataMember]
        public string Role { get; set; }
    }

    [DataContract]
    public class ContactRoles
    {
        [DataMember]
        public string PersonShortGuid { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Organization { get; set; }
        [DataMember]
        public string Role { get; set; }

        [DataMember]
        public IList<ContactRole> roles { get; set; }
        [DataMember]
        public IDictionary<int, string> roleIndex { get; set; }
        [DataMember]
        public IList<Organization> organizationIndex { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }

    public class ContactSearch
    {
        public int PersonId { get; set; }
        public int TimePeriodId { get; set; }
        public int SponsorAgencyId { get; set; }
        public bool ShowAllForAgency { get; set; }
    }
}
