//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 9/7/2009 9:07:35 AM
// Description:
//
//======================================================
using System;

namespace DRCOG.Domain.Models.TIPProject
{
    /// <summary>
    /// </summary>
    public class CountyShareModel
    {
        public CountyShareModel()
        {
            //default to false - helps with weirdness from html checkbox behavior
            this.Primary = false;
        }

        public string CountyName { get; set; }
        public int? CountyId { get; set; }
        public double? Share { get; set; }
        public bool? Primary { get; set; }
        public int? ProjectId { get; set; }

    }
}
