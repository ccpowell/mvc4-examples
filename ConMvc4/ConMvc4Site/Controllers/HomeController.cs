using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using System.Web.Security;

namespace ConMvc4Site.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TestPage()
        {
            // since the user has logged in, we can get his name.
            // we use this to get his account ID
            var username = User.Identity.Name;
            var user = Membership.Provider.GetUser(username, true);
            ViewBag.UserId = ((Guid)user.ProviderUserKey).ToString("N");
            return View();
        }
    }
}
