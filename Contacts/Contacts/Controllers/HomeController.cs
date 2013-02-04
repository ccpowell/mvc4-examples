using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace Contacts.Controllers
{
    public class HomeController : Controller
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public HomeController()
        {
        }
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        protected Dictionary<string, object> CreatePageParameters()
        {
            return new Dictionary<string, object>(RouteData.Values);
        }
        protected void SetPageParameters(Dictionary<string, object> pp)
        {
            ViewBag.PageParameters = Newtonsoft.Json.JsonConvert.SerializeObject(pp);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }

        public ActionResult TestPage1()
        {
            var pp = new Dictionary<string, object>();
            pp.Add("Hoover", "sucks");
            pp.Add("Number", 42);
            ViewBag.PageParameters = Newtonsoft.Json.JsonConvert.SerializeObject(pp);
            return View("TestPage");
        }
        public ActionResult TestPage2()
        {
            var pp = new { Hoover = "sucks", Number = 42 };
            ViewBag.PageParameters = Newtonsoft.Json.JsonConvert.SerializeObject(pp);
            return View("TestPage");
        }
        public ActionResult TestPage3()
        {
            ViewBag.PageParameters = Newtonsoft.Json.JsonConvert.SerializeObject(RouteData.Values);
            return View("TestPage");
        }
        public ActionResult TestPage4()
        {
            ViewBag.PageParameters = Newtonsoft.Json.JsonConvert.SerializeObject(CreatePageParameters());
            return View("TestPage");
        }

        public ActionResult TestPage5()
        {
            return View("TestPage");
        }

        public ActionResult TestPage6()
        {
            var pp = CreatePageParameters();
            pp.Add("Hoover", "sucks");
            pp.Add("Number", 42);
            SetPageParameters(pp);
            return View("TestPage");
        }
    }
}
