using Contacts.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Collections.Generic;
using System.Linq;
using Contacts.Models;

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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
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
                FirstName = "Bozo",
                LastName = "Clown",
                Phone = "6661231234"
            };
            bool expected = true; 
            bool actual;
            actual = target.CreateContact(contact);
            Assert.AreEqual(expected, actual);
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
    }
}
