using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contacts.Models
{
    public class ContactList
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public string Id { get; set; }

        public string OwnerId { get; set; }

        public string Name { get; set; }

        public List<string> ContactIds { get; set; }

        [MongoDB.Bson.Serialization.Attributes.BsonIgnoreAttribute]
        public List<Contact> Contacts { get; set; }

        public ContactList()
        {
            ContactIds = new List<string>();
            Contacts = new List<Contact>();
        }
    }
}