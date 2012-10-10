﻿#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 03/05/2010	DTucker         1. Initial Creation.
 * 
 * DESCRIPTION:
 * Concrete business rules
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Enums;

namespace DRCOG.Domain.Models.TIPProject.Amendment
{
    public class ProposedToAmended : AmendmentStrategy
    {
        public override TipProjectAmendments UpdateStatus(TipProjectAmendments model)
        {
            model.AmendmentStatusId = (Int32)TIPAmendmentStatus.Amended;
            model.AmendmentTypeId = (Int32)AmendmentType.Administrative;
            model.VersionStatusId = (Int32)TIPVersionStatus.Active;
            return model;
        }

        public override bool RequireProjectCopy()
        {
            return false;
        }

        public override bool RequireVersionStatusUpdate()
        {
            VersionStatus = (Int32)TIPVersionStatus.Inactive;
            
            return true;
        }
    }
}