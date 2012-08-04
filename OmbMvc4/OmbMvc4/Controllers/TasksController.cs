using System.Web.Mvc;

namespace OmbMvc4.Controllers
{
    public class TasksController : Controller
    {
        //
        // GET: /Tasks/

        public ActionResult Index()
        {
            return View();
        }
    }
}
