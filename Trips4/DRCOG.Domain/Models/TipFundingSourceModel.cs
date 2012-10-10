//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 8/13/2009 3:57:14 PM
// Description:
//
//======================================================
using System;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// </summary>
    public class TipFundingSourceModel
    {
        public int FundingTypeId { get; set; }
        public string FundingType { get; set; }
        public string Code { get; set; }
        public string FundingLevel { get; set; }
        public string RecipentOrganization { get; set; }
        public string SourceOrganizatin { get; set; }
        public bool IsDiscretionary { get; set; }
        public string TipYear { get; set; }
        public string Selector { get; set; }

    }
}
