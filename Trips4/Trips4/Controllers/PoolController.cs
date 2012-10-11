//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/20/09
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using DRCOG.Domain.Models;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain.Interfaces;
//using Trips4.Filters;
using DRCOG.Domain.ViewModels.TIP;
using DRCOG.Common.Web.MvcSupport.Attributes;

namespace Trips4.Controllers
{
    [Authorize]
    //[RoleAuth]
    //[RemoteRequireHttps]
    public class PoolController : ControllerBase
    {

        /// <summary>
        /// Detail/Edit view for a pool
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string year, int id)
        {
            var pm = new PoolModel();
            var model = new PoolViewModel();
            model.Pool = pm;
            model.TipYear = year;

            return View("Detail", model);
        }

        /// <summary>
        /// Display the create screen for a pool
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(string year)
        {
            var pm = new PoolModel();
            var model = new PoolViewModel();
            model.Pool = pm;
            model.TipYear = year;


            return View("Create", model);
        }

        /// <summary>
        /// Create the new Funding Source in the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator, TIP Administrator")]
        public ActionResult Create()
        {
            //route back to the Funding Sources list for the specified TIP
            return View();
        }
    }
}
