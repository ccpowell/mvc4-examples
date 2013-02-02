using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contacts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TestPage1()
        {
            return View("TestPage");
        }
        public ActionResult TestPage2()
        {
            return View("TestPage");
        }
        public ActionResult TestPage3()
        {
            return View("TestPage");
        }
        public ActionResult TestPage4()
        {
            return View("TestPage");
        }
        public ActionResult TestPage5()
        {
            return View("TestPage");
        }
        public ActionResult TestPage6()
        {
            return View("TestPage");
        }
    }
}
