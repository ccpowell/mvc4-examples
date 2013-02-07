using Trips4.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DRCOG.Domain.ViewModels.RTP;
using System.Collections.Generic;
using System.Linq;
using DRCOG.Domain.Models;

namespace Trips4.Tests
{
    
    
    /// <summary>
    ///This is a test class for TripsRepositoryTest and is intended
    ///to contain all TripsRepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TripsRepositoryTest
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

#if there_is_no_new_cycle_already
        /// <summary>
        ///A test for Create/Retrieve/Update/Delete RtpPlanCycle
        ///</summary>
        [TestMethod()]
        public void CrudRtpPlanCycleTest()
        {
            TripsRepository target = new TripsRepository();
            var cycle = new DRCOG.Domain.ViewModels.RTP.PlanCycle()
            {
                Name = "Testicle",
                Description = "A Test Cycle"
            };
            string rtpYear = "2035-S";
            int yid = target.GetRtpPlanYearId(rtpYear);
            int cid = target.CreateRtpPlanCycle(cycle, yid);
            Assert.IsTrue(cid > 0, "Create RTP Plan Cycle makes an ID > 0");

            var editCycle = target.GetRtpPlanCycle(cid);
            Assert.AreEqual(editCycle.Status, "New", "Create RTP Plan Cycle sets status to New");
            Assert.IsNotNull(editCycle, "Get RTP Plan Cycle retrieves a PlanCycle");

            editCycle.Name = "TestEdit";
            editCycle.Description = "Edited Test Cycle";
            target.UpdateRtpPlanCycle(editCycle);

            var actual = target.GetRtpPlanCycle(editCycle.Id);
            Assert.AreEqual(editCycle.Name, actual.Name, "UpdateRtpPlanCycle updates Name");
            Assert.AreEqual(editCycle.Description, actual.Description, "UpdateRtpPlanCycle updates Description");

            target.DeleteRtpPlanCycle(cid);
        }
#endif
        /// <summary>
        ///A test for GetTipStatus
        ///</summary>
        [TestMethod()]
        public void GetTipStatusTest()
        {
            TripsRepository target = new TripsRepository(); 
            int rtpYearId = 80; 
            TipStatusModel actual;
            actual = target.GetTipStatus(rtpYearId);
            Assert.IsNotNull(actual);
        }

        /// <summary>
        ///A test for UpdateTipStatus
        ///</summary>
        [TestMethod()]
        public void UpdateTipStatusTest()
        {
            TripsRepository target = new TripsRepository();
            int rtpYearId = 80;
            TipStatusModel actual;
            actual = target.GetTipStatus(rtpYearId);
            Assert.IsNotNull(actual);
            target.UpdateTipStatus(actual);
        }

        /// <summary>
        ///A test for TryCatchError
        ///</summary>
        [TestMethod()]
        public void TryCatchErrorTest()
        {
            TripsRepository target = new TripsRepository();
            try
            {
                target.TryCatchError();
                Assert.Fail("TryCatchError did not throw an error!");
            }
            catch (Exception)
            {
                Assert.IsTrue(true, "it's okay");
            }
        }

        /// <summary>
        ///A test for GetRtpAmendableProjectsCycle
        ///</summary>
        [TestMethod()]
        [Ignore]
        public void GetRtpActivePlanCycleIdTest()
        {
            TripsRepository target = new TripsRepository();
            int actual;
            actual = target.GetRtpActivePlanCycleId();
            Assert.IsTrue(actual > 0);
        }
    }
}
