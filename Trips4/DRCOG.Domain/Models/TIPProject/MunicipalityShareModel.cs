//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 9/7/2009 2:03:22 PM
// Description:
//
//======================================================
using System;

namespace DRCOG.Domain.Models.TIPProject
{
    /// <summary>
    /// Project share split for a municipality
    /// </summary>
    public class MunicipalityShareModel
    {
        public MunicipalityShareModel()
        {
            this.Primary = false;
        }
        public string MunicipalityName { get; set; }
        public int? MunicipalityId { get; set; }
        public double? Share { get; set; }
        public bool? Primary { get; set; }
        public int? ProjectId { get; set; }

        
    }
}
