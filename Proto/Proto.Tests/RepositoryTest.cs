using Proto.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Proto.Tests
{
    
    
    /// <summary>
    ///This is a test class for RepositoryTest and is intended
    ///to contain all RepositoryTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RepositoryTest
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
        ///A test for GetSurveyDashboardItemsBySponsor
        ///</summary>
        [TestMethod()]
        public void GetSurveyDashboardItemsBySponsorTest()
        {
            Repository target = new Repository(); // TODO: Initialize to an appropriate value
            string year = "2011";
            Nullable<int> version = null;
            bool showEmpty = true; 
            List<DashboardItem> actual;
            actual = target.GetSurveyDashboardItemsBySponsor(year, version, showEmpty);
            Assert.IsTrue(actual.Count > 0);
        }
    }
}
