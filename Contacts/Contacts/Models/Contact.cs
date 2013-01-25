using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contacts.Models
{
    public class Contact
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public string Id { get; set; }

        public string UserName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
    }
}