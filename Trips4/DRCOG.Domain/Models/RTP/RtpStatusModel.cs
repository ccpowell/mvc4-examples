#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR			REMARKS
 * 05/05/2010	DDavidson       1. Initial Creation. 
 * 
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion

using System;

namespace DRCOG.Domain.Models.RTP
{
    /// <summary>
    /// </summary>
    public class RtpStatusModel
    {
        public RtpStatusModel()
        {
            this.Notes = "";           
        }

        public RtpSummary RtpSummary { get; set; }

        public int ProgramId { get; set; }
        public int TimePeriodId { get; set; }
        public int BaseYearId { get; set; }
        public string BaseYear { get; set; }
        public string Plan { get; set; }
        public DateTime? PublicHearing { get; set; }
        public DateTime? Adoption { get; set; }
        public DateTime? LastAmended { get; set; }
        public DateTime? CDOTAction { get; set; }
        public DateTime? USDOTApproval { get; set; }     
        public string Notes { get; set; }
        //public bool IsCurrent { get; set; }
        //public bool IsPending { get; set; }
        //public bool IsPrevious { get; set; }
        public string Description { get; set; }

        //public string GetStatus()
        //{
        //    string status = "not set";
        //    if (this.IsPrevious) { status = "Previous"; }
        //    if (this.IsCurrent) { status= "Current"; }
        //    if (this.IsPending) { status =  "Pending"; }
        //    return status;
        //}
    }

}
