using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Trips4.Controllers.Api
{
    public class JsonResult
    {
        public string message { get; set; }
    }

    public class TipDataController : ApiController
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private Trips4.Data.TripsRepository TripsRepository { get; set; }
        public TipDataController(Trips4.Data.TripsRepository trepo)
        {
            TripsRepository = trepo;
        }

        [AuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public JsonResult UpdateStatus(DRCOG.Domain.Models.TipStatusModel model)
        {
            //Send update to repo
            try
            {
                throw new Exception("fooey");
                //_tipRepository.UpdateTipStatus(model);
            }
            catch (Exception ex)
            {
                Logger.WarnException("Could not update TIP Status", ex);
                throw;// return Json(new { message = "Changes could not be stored. An error has been logged." });
            }
            return new JsonResult() { message = "Changes successfully saved." };
        }
    }
}
