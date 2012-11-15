using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Contacts.Data
{
    public class ContactsRepository : IDisposable
    {
        private const string ConnectionString = "mongodb://localhost/?safe=true";

        private MongoDatabase GetDb()
        {
            var server = MongoServer.Create(ConnectionString);
            var db = server.GetDatabase("contact");
            return db;
        }
        public IEnumerable<String> ListCollections()
        {
            return GetDb().GetCollectionNames();
        }

        public bool CreateContact(Models.Contact contact)
        {
            var db = GetDb();
            var collection = db.GetCollection<Models.Contact>("contacts");
            var result = collection.Insert(contact);
            return result.Ok;
        }

        public IEnumerable<Models.Contact> GetContacts()
        {
            var db = GetDb();
            //var results = new List<Models.Contact>();
            return db.GetCollection<Models.Contact>("contacts").FindAll();
        }

        void IDisposable.Dispose()
        {
        }
    }
}