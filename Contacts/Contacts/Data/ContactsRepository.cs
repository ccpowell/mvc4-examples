using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;

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

        public void CreateContact(Models.Contact contact)
        {
            var db = GetDb();
            var collection = db.GetCollection<Models.Contact>("contacts");
            contact.Id = ObjectId.GenerateNewId().ToString();
            var result = collection.Insert(contact);
            if (!result.Ok)
            {
                throw new Exception("CreateContact failed " + result.ErrorMessage);
            }
        }


        public void UpdateContact(Models.Contact contact)
        {
            var db = GetDb();
            var collection = db.GetCollection<Models.Contact>("contacts");
            var result = collection.Save(contact);
            if (!result.Ok)
            {
                throw new Exception("UpdateContact failed " + result.ErrorMessage);
            }
        }


        public Models.Contact GetContact(string id)
        {
            var db = GetDb();
            return db.GetCollection<Models.Contact>("contacts").AsQueryable().First(c => c.Id == id);
        }

        public Models.Contact GetContactByName(string name)
        {
            var db = GetDb();
            return db.GetCollection<Models.Contact>("contacts").AsQueryable().First(c => c.UserName == name);
        }

        public IEnumerable<Models.Contact> GetContacts()
        {
            var db = GetDb();
            return db.GetCollection<Models.Contact>("contacts").FindAll();
        }

        public void CreateContactList(Models.ContactList contactList)
        {
            var db = GetDb();
            var collection = db.GetCollection<Models.ContactList>("contactlists");
            contactList.Id = ObjectId.GenerateNewId().ToString();
            var result = collection.Save(contactList);
            //return result.Ok;
        }

        public void DeleteContact(string id)
        {
            var db = GetDb();
            var clists = db.GetCollection<Models.ContactList>("contactlists");
            var ids = new BsonArray();
            ids.Add(id);
            var queryContains = Query.In("ContactIds", ids);
            foreach (var clist in clists.Find(queryContains))
            {
                clist.ContactIds.Remove(id);
                clists.Save(clist);
            }

            var contacts = db.GetCollection<Models.Contact>("contacts");
            var queryId = Query.EQ("_id", id);
            var result = contacts.Remove(queryId);
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
                list.ContactIds = new List<string>();
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

        public Models.ContactList GetContactList(string id)
        {
            var db = GetDb();
            var list = db.GetCollection<Models.ContactList>("contactlists").AsQueryable().First(cl => cl.Id == id);
            FillContacts(list);
            return list;
        }

        public void Doit()
        {
#if needed
            var db = GetDb();
            var cl = db.GetCollection<Models.ContactList>("contactlists");
            cl.Drop();
            var c = db.GetCollection<Models.Contact>("contacts");
            c.Drop();
#endif
        }

        public void AddContacts(Models.ContactList cl, IEnumerable<string> ids)
        {
            var db = GetDb();
            var coll = db.GetCollection<Models.ContactList>("contactlists");
            var list = coll.AsQueryable().First(c => c.Id == cl.Id);
            list.ContactIds.AddRange(ids.Where(i => !i.In(list.ContactIds)));
            coll.Save(list);
        }

        public void UpdateContactList(Models.ContactList cl)
        {
            var db = GetDb();
            var coll = db.GetCollection<Models.ContactList>("contactlists");
            coll.Save(cl);
        }


        void IDisposable.Dispose()
        {
        }

        private Models.Contact CreateFakeContact(int i, string organization)
        {
            return new Models.Contact()
            {
                Email = string.Format("TestUser{0}@testing.drcog.org", i),
                UserName = string.Format("TestUser{0}", i),
                Organization = organization,
                Title = "Tester",
                Phone = string.Format("123123{0:D4}", i),
                Id = ObjectId.GenerateNewId().ToString()
            };
        }


        /// <summary>
        /// Initialize the collections for testing.
        /// Currently all old documents are deleted, then new ones are inserted.
        /// </summary>
        public void InitializeCollections()
        {
            var db = GetDb();
            var contactLists = db.GetCollection<Models.ContactList>("contactlists");
            contactLists.Drop();

            var contacts = db.GetCollection<Models.Contact>("contacts");
            contacts.Drop();

            for (int i = 0; i < 50; i++)
            {
                contacts.Insert(CreateFakeContact(i, "TestOrg Alpha"));
            }

            for (int i = 0; i < 50; i++)
            {
                contacts.Insert(CreateFakeContact(i, "TestOrg Beta"));
            }

            var cl = new Models.ContactList()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Alpha"
            };
            var ids = contacts.AsQueryable().Where(c => c.Organization.Contains("Alpha")).Select(co => co.Id);
            cl.ContactIds.AddRange(ids);
            contactLists.Save(cl);
            cl = new Models.ContactList()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = "Beta"
            };
            ids = contacts.AsQueryable().Where(c => c.Organization.Contains("Alpha")).Select(co => co.Id);
            cl.ContactIds.AddRange(ids);
            contactLists.Save(cl);
        }
    }
}