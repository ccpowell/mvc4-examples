//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/23/2009 3:59:22 PM
// Description:
//
//======================================================
using System;

namespace DRCOG.Domain.Models
{
    /// <summary>
    /// </summary>
    public class TipStatusModel
    {
        public TipStatusModel()
        {
            //we want Notes to be an empty string by default b/c we
            //can have a valid item in the DB with an empty Notes field.
            //but ADO.NET does not send NULL parameters into SPROCS
            //so this can throw errors if not set to an empty string.
            this.Notes = "";           
        }

        /// <summary>
        /// Current Status
        /// </summary>
        //public int Status { get; set; }
        public int ProgramId { get; set; }
        public int TimePeriodId { get; set; }
        public string TipYear { get; set; }
        
        public DateTime? PublicHearing { get; set; }
        public DateTime? Adoption { get; set; }
        public DateTime? LastAmended { get; set; }
        public DateTime? GovernorApproval { get; set; }
        public DateTime? USDOTApproval { get; set; }
        public DateTime? EPAApproval { get; set; }        
        public string Notes { get; set; }
        //public string Status { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsPending { get; set; }
        public bool IsPrevious { get; set; }

        public DateTime ShowDelayDate { get; set; }

        public string GetStatus()
        {
            string status = "not set";
            if (this.IsPrevious) { status = "Previous"; }
            if (this.IsCurrent) { status= "Current"; }
            if (this.IsPending) { status =  "Pending"; }
            return status;
        }
        

    }

}
