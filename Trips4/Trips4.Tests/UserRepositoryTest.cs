using Trips4.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DRCOG.Domain.Models;

namespace Trips4.Tests
{
    
    
    /// <summary>
    ///This is a test class for UserRepositoryTest and is intended
    ///to contain all UserRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserRepositoryTest
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
        ///A test for CreatePersonForUser
        ///</summary>
        [TestMethod()]
        public void CreatePersonForUserTest()
        {
            UserRepository target = new UserRepository();
            Guid id = Guid.Parse("F543E678-E37F-46EC-8B95-E22B651D866F");
            target.CreatePersonForUser(id, "XXX");
        }

#if cleanudup
        /// <summary>
        ///A test for CreateUserAndPerson
        ///</summary>
        [TestMethod()]
        public void CreateUserAndPersonTest()
        {
            UserRepository target = new UserRepository();
            ShortProfile profile = new ShortProfile()
            {
                FirstName = "sword",
                LastName = "fish",
                UserName = "swordfish",
                RecoveryEmail = "swordfish@drcog.org",
                SponsorCode = "XXX",
                PersonGUID = Guid.Parse("180F39D4-5F7B-4F11-8DF3-CAF135FDCA6A")
            };
            target.CreateUserAndPerson(profile);
            Assert.IsTrue(profile.PersonID > 0);
        }
#endif

        /// <summary>
        ///A test for ValidateUser
        ///</summary>
        [TestMethod()]
        public void ValidateUserTestInvalid()
        {
            UserRepository target = new UserRepository(); 
            string userName = "xxx";
            string password = "yyy"; 
            bool expected = false; 
            bool actual;
            actual = target.ValidateUser(userName, password);
            Assert.AreEqual(expected, actual);
        }
    }
}
