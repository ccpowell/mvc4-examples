using System;
using System.Collections.Generic;

namespace Ef5Contacts.Models
{
    public class aspnet_Users
    {
        public aspnet_Users()
        {
            this.aspnet_PersonalizationPerUser = new List<aspnet_PersonalizationPerUser>();
            this.ContactList_Member = new List<ContactList_Member>();
            this.ContactList_Owner = new List<ContactList_Owner>();
            this.ProfilePropertyValues = new List<ProfilePropertyValue>();
            this.aspnet_Roles = new List<aspnet_Roles>();
        }

        public System.Guid ApplicationId { get; set; }
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LoweredUserName { get; set; }
        public string MobileAlias { get; set; }
        public bool IsAnonymous { get; set; }
        public System.DateTime LastActivityDate { get; set; }
        public virtual aspnet_Applications aspnet_Applications { get; set; }
        public virtual aspnet_Membership aspnet_Membership { get; set; }
        public virtual ICollection<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }
        public virtual aspnet_Profile aspnet_Profile { get; set; }
        public virtual ICollection<ContactList_Member> ContactList_Member { get; set; }
        public virtual ICollection<ContactList_Owner> ContactList_Owner { get; set; }
        public virtual ICollection<ProfilePropertyValue> ProfilePropertyValues { get; set; }
        public virtual ICollection<aspnet_Roles> aspnet_Roles { get; set; }
    }
}
