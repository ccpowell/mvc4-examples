using System.Collections.Generic;
using System.Linq;
using Contacts.Data;
using Contacts.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace Contacts.Tests
{
    
    
    /// <summary>
    ///This is a test class for ContactsRepositoryTest and is intended
    ///to contain all ContactsRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ContactsRepositoryTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var repo = new ContactsRepository();
            repo.InitializeCollections();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ListCollections
        ///</summary>
        [TestMethod()]
        public void ListCollectionsTest()
        {
            ContactsRepository target = new ContactsRepository(); 
            IEnumerable<string> actual;
            actual = target.ListCollections();
            Assert.IsTrue(actual.Count() > 0);
        }

        /// <summary>
        ///A test for ContactsRepository Constructor
        ///</summary>
        [TestMethod()]
        public void ContactsRepositoryConstructorTest()
        {
            ContactsRepository target = new ContactsRepository();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for CreateContact
        ///</summary>
        [TestMethod()]
        public void CreateContactTest()
        {
            ContactsRepository target = new ContactsRepository();
            Contact contact = new Contact()
            {
                UserName = "BozoTheClown",
                Organization = "BarnumAndBailey",
                Title = "Clown",
                Phone = "6661231234"
            };
            target.CreateContact(contact);
        }

        /// <summary>
        ///A test for GetContacts
        ///</summary>
        [TestMethod()]
        public void GetContactsTest()
        {
            ContactsRepository target = new ContactsRepository();
            IEnumerable<Contact> actual;
            actual = target.GetContacts();
            Assert.IsTrue(actual.Count() > 0);
        }

        /// <summary>
        ///A test for CreateContactList
        ///</summary>
        [TestMethod()]
        public void CreateContactListTest()
        {
            ContactsRepository target = new ContactsRepository();
            ContactList contactList = new ContactList()
            {
                Name = "MyFirstList"
            };
            target.CreateContactList(contactList);
        }

        /// <summary>
        ///A test for GetContactLists
        ///</summary>
        [TestMethod()]
        public void GetContactListsTest()
        {
            ContactsRepository target = new ContactsRepository();
            IEnumerable<ContactList> actual;
            actual = target.GetContactLists();
            Assert.IsTrue(actual.Count() > 0);
        }

        /// <summary>
        ///A test for Doit
        ///</summary>
        [TestMethod()]
        public void DoitTest()
        {
            ContactsRepository target = new ContactsRepository(); // TODO: Initialize to an appropriate value
            target.Doit();
        }

        /// <summary>
        ///A test for GetContact
        ///</summary>
        [TestMethod()]
        public void GetContactTest()
        {
            ContactsRepository target = new ContactsRepository(); // TODO: Initialize to an appropriate value
            string id = string.Empty; // TODO: Initialize to an appropriate value
            Contact expected = null; // TODO: Initialize to an appropriate value
            Contact actual;
            actual = target.GetContact(id);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetContactListByName
        ///</summary>
        [TestMethod()]
        public void GetContactListByNameTest()
        {
            ContactsRepository target = new ContactsRepository(); 
            string name = "Alpha"; 
            ContactList actual;
            actual = target.GetContactListByName(name);
            Assert.IsNotNull(actual.Contacts);
            Assert.IsTrue(actual.Contacts.Count > 0);
        }


        /// <summary>
        ///A test for DeleteContact
        ///</summary>
        [TestMethod()]
        public void DeleteContactTest()
        {
            ContactsRepository target = new ContactsRepository();
            string name = "BozoTheClown";
            Contact actual = null;
            actual = target.GetContactByName(name);
            Assert.IsNotNull(actual);
            target.DeleteContact(actual.Id);
        }

        /// <summary>
        ///A test for GetContactByName
        ///</summary>
        [TestMethod()]
        public void GetContactByNameTest()
        {
            ContactsRepository target = new ContactsRepository();
            string name = "BozoTheClown";
            Contact actual = null;
            actual = target.GetContactByName(name);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for GetAutoCompleteContact
        ///</summary>
        [TestMethod()]
        public void GetAutoCompleteContactTest()
        {
            ContactsRepository target = new ContactsRepository(); 
            string field = "UserName";
            string prefix = "test"; 
            IEnumerable<Contact> actual;
            actual = target.GetAutoCompleteContact(field, prefix);
            Assert.AreEqual(3, actual.Count());
        }
    }
}
