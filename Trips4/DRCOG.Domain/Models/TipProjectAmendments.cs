using System;

using DRCOG.Domain.Models.TIPProject;
using DRCOG.Domain.ServiceInterfaces;
using DRCOG.Domain.Interfaces;
//using DRCOG.Domain.Models.TIPProject.Amendment;
using System.Collections.Generic;


namespace DRCOG.Domain.Models
{
    public class TipProjectAmendments : VersionModel
    {
        #region Private Members

        //protected AmendmentStrategy _amendmentStrategy;
        //protected Int32 _amendmentStatusId { get; set; }
        //private String _locationMapPath;

        #endregion

        public Int32 PreviousProjectVersionId { get; set; }
        public Int32 AmendmentStatusId { get; set; }
        public String AmendmentStatus { get; set; }
        public DateTime AmendmentDate { get; set; }
        public Int32 AmendmentTypeId { get; set; }
        public Int32 VersionStatusId { get; set; }
        public String AmendmentReason { get; set; }
        public String AmendmentCharacter { get; set; }
        public Boolean UpdateLocationMap { get; set; }

        public String LocationMapPath { get; set; }
        public TipProjectAmendments()
        {
            //_amendmentStrategy = new SubmittedToProposed();
        }

        //private void SetAmendmentStrategy(AmendmentStrategy amendmentStrategy)
        //{
        //    this._amendmentStrategy = amendmentStrategy;
        //}

        //protected void PickStrategy(int amendmentStatusId)
        //{
        //    switch (amendmentStatusId)
        //    {
        //        case 10734: // Amended
        //        case 1774:
        //            _amendmentStrategy = new AmendedToSubmitted();
        //            break;
        //        case 10736: // Approved
        //        case 1769:
        //            _amendmentStrategy = new ApprovalToSubmitted();
        //            break;
        //        case 10740: // Proposed
        //        case 1767:
        //            _amendmentStrategy = new ProposedToAmended();
        //            break;
        //        case 10742: // Submitted
        //        case 1770:
        //            _amendmentStrategy = new SubmittedToProposed();
        //            break;
        //    }
        //}

        //public Boolean RequiresProjectCopy(Int32 amendmentStatusId)
        //{
        //    //_amendmentStatusId = amendmentStatusId;
        //    PickStrategy(amendmentStatusId);
        //    return _amendmentStrategy.RequireProjectCopy();

        //}
        
        //public Boolean RequireVersionStatusUpdate()
        //{
        //    if (_amendmentStrategy.RequireVersionStatusUpdate())
        //    {
        //        this.VersionStatusId = _amendmentStrategy.VersionStatus;
        //        return true;
        //    }
        //    return false;
        //}

        //public TipProjectAmendments AmendProject()
        //{
        //    return _amendmentStrategy.UpdateStatus(this);
        //}
    }
}
