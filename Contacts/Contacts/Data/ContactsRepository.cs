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
        private MongoClient Mc { get; set; }

        public ContactsRepository()
        {
            Mc = new MongoClient();
        }

        private MongoDatabase GetDb()
        {
            var server = Mc.GetServer();
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

        public Models.Contact GetContact(string id)
        {
            var db = GetDb();
            var oid = new ObjectId(id);
            return db.GetCollection<Models.Contact>("contacts").AsQueryable().First(c => c.Id == oid);
        }

        public IEnumerable<Models.Contact> GetContacts()
        {
            var db = GetDb();
            return db.GetCollection<Models.Contact>("contacts").FindAll();
        }

        public bool CreateContactList(Models.ContactList contactList)
        {
            var db = GetDb();
            var collection = db.GetCollection<Models.ContactList>("contactlists");
            //collection.Drop();
            var result = collection.Insert(contactList);
            return result.Ok;
        }

        public IEnumerable<Models.ContactList> GetContactLists()
        {
            var db = GetDb();
            return db.GetCollection<Models.ContactList>("contactlists").FindAll();
        }

        private void FillContacts(Models.ContactList list)
        {
            if (list.ContactIds == null)
            {
                list.ContactIds = new List<ObjectId>();
            }
            if (list.ContactIds.Count == 0)
            {
                return;
            }

            var db = GetDb();
            var all = db.GetCollection<Models.Contact>("contacts").AsQueryable();
            foreach (var id in list.ContactIds)
            {
                list.Contacts.Add(all.First(a => a.Id == id));
            }
        }

        public Models.ContactList GetContactListByName(string name)
        {
            var db = GetDb();
            var list = db.GetCollection<Models.ContactList>("contactlists").AsQueryable().First(cl => cl.Name == name);
            FillContacts(list);
            return list;
        }

        public void AddContact()
        {
#if true
            var db = GetDb();
            var cl = db.GetCollection<Models.ContactList>("contactlists");
            var contactList = cl.AsQueryable().First();
            contactList.ContactIds.AddRange(GetContacts().Select(c => c.Id));
            cl.Save(contactList);

            var foo = cl.AsQueryable().First();
#endif
        }



        void IDisposable.Dispose()
        {
        }

    }
}