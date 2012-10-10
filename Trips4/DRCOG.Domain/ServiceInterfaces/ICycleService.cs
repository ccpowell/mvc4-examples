#region INFORMATION
/*======================================================
 * Copyright (c) 2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 07/07/2010	DDavidson       1. Initial Creation.
 * 
 * DESCRIPTION:
 * Interface that describes RestoreStrategy Implementation
 * ======================================================*/
#endregion

using System;
using DRCOG.Domain.Models.RTP;
using DRCOG.Domain.Models;

namespace DRCOG.Domain.ServiceInterfaces
{
    public interface ICycleService
    {
        Cycle GetCurrentCycle(Int32 TimePeriodId);
    }
}
