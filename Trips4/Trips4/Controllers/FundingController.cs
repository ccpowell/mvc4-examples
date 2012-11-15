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
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Models;
using Trips4;
//using Trips4.Filters;
using DRCOG.Domain.ViewModels;
using DRCOG.Domain.ViewModels.TIP;
//using DRCOG.Common.Service.MemberShipServiceSupport.Interfaces;
//using DRCOG.Common.Web.MvcSupport.Attributes;

namespace Trips4.Controllers
{
    [Trips4.Filters.SessionAuthorizeAttribute]
    //[RemoteRequireHttps]
    public class FundingController : ControllerBase
    {

        private ITipRepository _tipRepository;

        public FundingController(ITipRepository tipRepository, ITripsUserRepository userRepository)
            : base("FundingController", userRepository)
        {
            _tipRepository = tipRepository;
        }

        /// <summary>
        /// Detail/Edit view for a funding record
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult Detail(string year, int id)
        {
            //var fsm = new FundingSourceModel();

            var fsm = _tipRepository.GetFundingSourceModel(year, id);

            var model = new FundingSourceViewModel();
            model.FundingSource = fsm;
            model.TipYear = year;

            return View("Detail",model);
        }

        /// <summary>
        /// Display the create screen for a funding source
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create(string year)
        {
            var fsm = new FundingSourceModel();
            var model = new FundingSourceViewModel();
            model.FundingSource = fsm;
            model.TipYear = year;

            return View("Create", model);
        }

        /// <summary>
        /// Create the new Funding Source in the database
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Trips4.Filters.SessionAuthorizeAttribute(Roles = "Administrator, TIP Administrator")]
        public ActionResult Create()
        {
            //route back to the Funding Sources list for the specified TIP
            return View();
        }


    }
}
