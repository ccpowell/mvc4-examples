using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ombudsman.Site;
using Ombudsman.Site.Controllers;

namespace Ombudsman.Site.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        // TODO: Stupid MSTest shouldn't need this but it won't copy the file without it.
        // CCP 10/18/2012
        [DeploymentItem(@"System.Web.Providers.dll")]
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.IndexJq() as ViewResult;

            // Assert
            Assert.IsNotNull(controller);
        }

    }
}
