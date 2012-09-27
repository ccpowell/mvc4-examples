using OmbudsmanDb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Ombudsman.Models;
using System.Collections.Generic;
using System.Linq;
namespace Ombudsman.Site.Tests
{


    /// <summary>
    ///This is a test class for OmbudsmanRepositoryTest and is intended
    ///to contain all OmbudsmanRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OmbudsmanRepositoryTest
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

        static OmbudsmanDb.TestInitialize_Result TestData { get; set; }

        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            // set up NLog
            var config = new NLog.Config.LoggingConfiguration();
            var dt = new NLog.Targets.DebuggerTarget()
            {
                Layout = "${longdate} ${uppercase:${level}} ${message}",
                Name = "t"
            };
            config.AddTarget("t", dt);
            config.LoggingRules.Add(new NLog.Config.LoggingRule("*", NLog.LogLevel.Trace, dt));
            NLog.LogManager.Configuration = config;
            var logger = NLog.LogManager.GetCurrentClassLogger();
            logger.Info("Test starting");

            // clear existing test objects, 
            // create new test objects, 
            // get IDs of test objects
            using (var db = new OmbudsmanEntities())
            {
                TestData = db.TestInitialize().Single();
            }
            logger.Info("Facility ID {0}, Ombudsman ID {1}", TestData.Facility1, TestData.Ombudsman1);
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
        ///A test for CreateFacility
        ///</summary>
        [TestMethod()]
        public void CreateFacilityTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            var facility = new Ombudsman.Models.Facility() { Name = "TestFacility 2", FacilityTypeId = 1 };
            int actual;
            actual = target.CreateFacility(facility);
            Assert.IsTrue(actual > 0);
        }

        /// <summary>
        ///A test for OmbudsmanRepository Constructor
        ///</summary>
        [TestMethod()]
        public void OmbudsmanRepositoryConstructorTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            Assert.IsNotNull(target);
        }

        /// <summary>
        ///A test for GetFacilities
        ///</summary>
        [TestMethod()]
        public void GetFacilitiesTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            List<Ombudsman.Models.Facility> actual;
            actual = target.GetFacilities();
            Assert.IsTrue(actual.Count > 0);
        }

        /// <summary>
        ///A test for GetOmbudsmen
        ///</summary>
        [TestMethod()]
        public void GetOmbudsmenTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            List<Ombudsman.Models.Ombudsman> actual;
            actual = target.GetOmbudsmen();
            Assert.IsTrue(actual.Count > 0);
        }
        /// <summary>
        ///A test for GetFacility
        ///</summary>
        [TestMethod()]
        public void GetFacilityTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            int id = TestData.Facility1.Value;
            var actual = target.GetFacility(id);
            Assert.AreEqual(actual.Name, "TestFacility 1");
            Assert.AreEqual(actual.OmbudsmanName, "TestOmbudsman 1");
        }

        /// <summary>
        ///A test for GetFacilitiesForOmbudsman
        ///</summary>
        [TestMethod()]
        public void GetFacilitiesForOmbudsmanTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            int id = TestData.Ombudsman1.Value;
            var actual = target.GetFacilitiesForOmbudsman(id);
            Assert.AreEqual(actual.Count, 1);
            Assert.AreEqual(actual[0].Name, "TestFacility 1");
        }

        /// <summary>
        ///A test for GetFacilityTypes
        ///</summary>
        [TestMethod()]
        public void GetFacilityTypesTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            var actual = target.GetFacilityTypes();
            Assert.AreEqual(3, actual.Count);
        }

        /// <summary>
        ///A test for UpdateFacility
        ///</summary>
        [TestMethod()]
        public void UpdateFacilityTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            Ombudsman.Models.Facility facility = target.GetFacility(TestData.Facility1.Value);
            facility.Address1 = "Out of sight";
            target.UpdateFacility(facility, false);
            Ombudsman.Models.Facility facility2 = target.GetFacility(TestData.Facility1.Value);
            Assert.AreEqual("Out of sight", facility2.Address1);
        }

        /// <summary>
        ///A test for UpdateFacility
        ///</summary>
        [TestMethod()]
        public void UpdateFacilitySucceedsTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            Ombudsman.Models.Facility facility = target.GetFacility(TestData.Facility1.Value);
            facility.IsActive = !facility.IsActive;
            var result = target.UpdateFacility(facility, true);
            Assert.IsTrue(result);
            Ombudsman.Models.Facility facility2 = target.GetFacility(TestData.Facility1.Value);
            Assert.AreEqual(facility.IsActive, facility2.IsActive);
        }

        /// <summary>
        ///A test for UpdateFacility
        ///</summary>
        [TestMethod()]
        public void UpdateFacilityFailsTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            Ombudsman.Models.Facility facility = target.GetFacility(TestData.Facility1.Value);
            facility.IsActive = !facility.IsActive;
            var result = target.UpdateFacility(facility, false);
            Assert.IsFalse(result);
            Ombudsman.Models.Facility facility2 = target.GetFacility(TestData.Facility1.Value);
            Assert.AreEqual(facility.IsActive, !facility2.IsActive);
        }

        /// <summary>
        ///A test for UpdateOmbudsman
        ///</summary>
        [TestMethod()]
        public void UpdateOmbudsmanTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            Ombudsman.Models.Ombudsman ombudsman = target.GetOmbudsman(TestData.Ombudsman1.Value);
            ombudsman.Address1 = "I changed it!";
            target.UpdateOmbudsman(ombudsman, false);
            Ombudsman.Models.Ombudsman ombudsman2 = target.GetOmbudsman(TestData.Ombudsman1.Value);
            Assert.AreEqual("I changed it!", ombudsman2.Address1);
        }


        /// <summary>
        ///A test for UpdateOmbudsman with authorization
        ///</summary>
        [TestMethod()]
        public void UpdateOmbudsmanAuthorizedFailsTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            Ombudsman.Models.Ombudsman ombudsman = target.GetOmbudsman(TestData.Ombudsman1.Value);
            ombudsman.IsActive = !ombudsman.IsActive;
            var result = target.UpdateOmbudsman(ombudsman, false);
            Assert.IsFalse(result);
        }


        /// <summary>
        ///A test for UpdateOmbudsman with authorization
        ///</summary>
        [TestMethod()]
        public void UpdateOmbudsmanAuthorizedSucceedsTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            Ombudsman.Models.Ombudsman ombudsman = target.GetOmbudsman(TestData.Ombudsman1.Value);
            ombudsman.IsActive = !ombudsman.IsActive;
            target.UpdateOmbudsman(ombudsman, true);
            Ombudsman.Models.Ombudsman ombudsman2 = target.GetOmbudsman(TestData.Ombudsman1.Value);
            Assert.AreEqual(ombudsman.IsActive, ombudsman2.IsActive);
        }

        /// <summary>
        ///A test for CreateOmbudsman
        ///</summary>
        [TestMethod()]
        public void CreateOmbudsmanTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository();
            Ombudsman.Models.Ombudsman ombudsman = new Ombudsman.Models.Ombudsman()
            {
                Name = "TestOmbudsman Created",
                UserName = "testombudsmancreated",
                Address1 = "123 somewhere"
            };
            var actual = target.CreateOmbudsman(ombudsman);
            Assert.IsTrue(actual > 0);
        }

        /// <summary>
        ///A test for GetOmbudsmenByName
        ///</summary>
        [TestMethod()]
        public void GetOmbudsmenByNameTest()
        {
            OmbudsmanRepository target = new OmbudsmanRepository(); 
            string term = "testombudsman "; 
            List<Ombudsman.Models.Ombudsman> actual;
            actual = target.GetOmbudsmenByName(term);
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
