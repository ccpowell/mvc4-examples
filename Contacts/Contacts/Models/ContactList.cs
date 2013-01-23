using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Contacts.Models
{
    public class ContactList
    {
        [MongoDB.Bson.Serialization.Attributes.BsonId]
        public MongoDB.Bson.ObjectId Id { get; set; }

        public string Name { get; set; }

        public List<MongoDB.Bson.ObjectId> Contacts { get; set; }
    }
}