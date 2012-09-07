using ConRepo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ConModels;
using Castle.Core.Logging;
using Castle.Windsor.Installer;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.MicroKernel;
using Castle.Facilities.Logging;

namespace ConMvc4Site.Tests
{

    public class LoggerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            IFacility logging = new LoggingFacility(LoggerImplementation.NLog, "NLog.config");
            container.AddFacility(logging);
        }
    }

    public class ResourceInstaller : IWindsorInstaller
    {
        // other components can be registered here
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // singleton cache because it is shared and readonly
            //var users = new Parts.UserCache(dm);
            //container.Register(Component.For<Parts.UserCache>().Instance(users));
            
            container.Register(Component.For<ContactsRepository>()
                .ImplementedBy<ContactsRepository>());
        }
    }
    
    /// <summary>
    ///This is a test class for ContactsRepositoryTest and is intended
    ///to contain all ContactsRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("NLog.config")]
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


        private static IWindsorContainer container;


        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            container = new WindsorContainer()
                .Install(FromAssembly.This());
            var logger = container.Resolve<ILogger>();
            logger.Debug("Starting tests");
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }
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
        ///A test for ContactsRepository Constructor
        ///</summary>
        [TestMethod()]
        public void ContactsRepositoryConstructorTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>();
            Assert.IsNotNull(target);
        }


        /// <summary>
        ///A test for CreateContactList
        ///</summary>
        [TestMethod()]
        public void CreateContactListTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>();
            var cl = new ConModels.ContactList()
            {
                Name = "testlist",
                IsPrivate = true
            };
            int actual;
            actual = target.CreateContactList(cl);
            Assert.IsTrue(actual > 0);
        }

        /// <summary>
        ///A test for AddOwnerToList
        ///N.B. a list can have a number of owners
        ///</summary>
        [TestMethod()]
        public void AddOwnerToListTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>();
            int listId = 29;
            Guid owner = Guid.Parse("2CF0D46B-C8F3-4051-977A-CC80149C0CE4");
            target.AddOwnerToList(listId, owner);
        }

        /// <summary>
        ///A test for CreateUser and DeleteUser
        ///</summary>
        [TestMethod()]
        public void CreateUserTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>();
            Guid id = Guid.Parse("E4C7BDA6-0D28-4A82-8FD6-D76A212C7E3E");
            User profile = new ConModels.User()
            {
                Id = id,
                RecoveryEmail = "testuser@drcog.org",
                UserName = "testuser",
                Organization = "Testing",
                HomeEmail = "testuser@testing.drcog.org"
            };
            Guid expected = id;
            Guid actual;
            actual = target.CreateUser(profile);
            Assert.AreEqual(expected, actual);

            // get by name
            var byname = target.GetUserByName("testuser");
            Assert.AreEqual("testuser", byname.UserName);
            Assert.AreEqual("testuser@testing.drcog.org", byname.HomeEmail);

            // get by ID
            var byid = target.GetUserById(id);
            Assert.AreEqual("testuser", byname.UserName);
            Assert.AreEqual("testuser@drcog.org", byname.RecoveryEmail);

            // now delete user
            target.DeleteUser(id);
        }

        [TestMethod()]
        public void DeleteUserTest()
        {
            Guid id = Guid.Parse("E4C7BDA6-0D28-4A82-8FD6-D76A212C7E3E");
            ContactsRepository target = container.Resolve<ContactsRepository>();
            target.DeleteUser(id);
        }


        /// <summary>
        ///A test for GetContactListsOwnedBy
        ///</summary>
        [TestMethod()]
        public void GetContactListsOwnedByTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>();
            Guid owner = Guid.Parse("2CF0D46B-C8F3-4051-977A-CC80149C0CE4");
            List<ConModels.ContactList> actual;
            actual = target.GetContactListsOwnedBy(owner);
            Assert.IsTrue(actual.Count > 0);
        }

        /// <summary>
        ///A test for GetPublicContactLists
        ///</summary>
        [TestMethod()]
        public void GetPublicContactListsTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>();
            List<ConModels.ContactList> actual;
            actual = target.GetPublicContactLists();
            Assert.IsTrue(actual.Count > 0);
        }

        /// <summary>
        ///A test for GetUserByName
        ///</summary>
        [TestMethod()]
        public void GetUserByNameTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>(); 
            string name = "testuser"; 
            User actual;
            actual = target.GetUserByName(name);
            Assert.AreEqual("testuser", actual.UserName);
        }

        /// <summary>
        ///A test for GetUserById
        ///</summary>
        [TestMethod()]
        public void GetUserByIdTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>();
            Guid id = Guid.Parse("2CF0D46B-C8F3-4051-977A-CC80149C0CE4");
            User actual;
            actual = target.GetUserById(id);
            Assert.AreEqual("testuser", actual.UserName);
        }

        /// <summary>
        ///A test for GetUsers
        ///</summary>
        [TestMethod()]
        public void GetUsersTest()
        {
            ContactsRepository target = container.Resolve<ContactsRepository>(); 
            List<User> actual;
            actual = target.GetUsers();
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
