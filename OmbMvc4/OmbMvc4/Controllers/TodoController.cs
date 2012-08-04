using System.Web.Http;
using System.Web.Http.Data.EntityFramework;
using System.Web.Mvc;
using System.Web.Routing;

namespace OmbMvc4.Controllers
{
    public partial class TodoController : DbDataController<OmbMvc4.Models.TodoContext>
    {
        // Any code added here will apply to all entity types managed by this data controller
    }

    // This provides context-specific route registration
    public class TodoRouteRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Todo"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
			RouteTable.Routes.MapHttpRoute(
                "Todo", // Route name
                "api/Todo/{action}", // URL with parameters
                new { controller = "Todo" } // Parameter defaults
            );
        }
    }
}
