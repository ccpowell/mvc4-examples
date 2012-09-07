using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Logging;

namespace ConMvc4Site.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(ILogger logger)
        {
            Logger = logger;
        }
        private ILogger Logger { get; set; }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TestPage()
        {
            return View();
        }
    }
}
