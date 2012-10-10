#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 03/05/2010	DTucker         1. Initial Creation.
 * 
 * DESCRIPTION:
 * Context class for handling Project Versions
 * ======================================================*/
#endregion

using System;
//using DRCOG.Domain.Models.TIPProject.Amendment;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DRCOG.Domain.Models.TIPProject
{
    public class TipVersionModel : ProjectModel, IInstance
    {
        private string[] CAN_DELETE_STATUS = { "submitted", "proposed" };

        public string TipYear { get; set; }
        public int ProjectFinancialRecordId { get; set; }
        public IEnumerable<int> InAmendmentCheck = new int[] { (int)Enums.TIPAmendmentStatus.Proposed, (int)Enums.TIPAmendmentStatus.Submitted };
        public bool IsInAmendment
        {
            get { return InAmendmentCheck.Contains(AmendmentStatusId) && IsEditable(); }
        }

        public Boolean CanDelete(String status)
        {
            if (((IList<string>)CAN_DELETE_STATUS).Contains(status.ToLower()))
            {
                return true;
            }
            return false;
        }


        #region IInstance Members

        public bool IsEditable()
        {
            if ((IsCurrent || IsPending) && (HttpContext.Current.User.IsInRole("TIP Administrator") || HttpContext.Current.User.IsInRole("Administrator"))) return true;
            return false;
        }

        public int AmendmentStatusId { get; set; }
        public bool IsActive{ get; set; }
        public bool IsCurrent{ get; set; }
        public bool IsPending { get; set; }
        //public bool IsEditable{ get; set; }
        public bool IsTopStatus{ get; set; }
        public int ProjectVersionId{ get; set; }
        public string Year{ get; set; }

        #endregion
    }
}
