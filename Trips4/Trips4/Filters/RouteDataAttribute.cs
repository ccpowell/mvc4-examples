using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trips4.Filters
{
    public class RouteDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var pp = new Dictionary<string, object>();
            var kvps = filterContext.Controller.ControllerContext.RouteData.Values;
            foreach (var kvp in kvps)
            {
                pp.Add(kvp.Key.ToLower(), kvp.Value);
            }
            filterContext.Controller.ViewBag.RouteData = Newtonsoft.Json.JsonConvert.SerializeObject(pp);
            base.OnActionExecuting(filterContext);
        }
    }
}