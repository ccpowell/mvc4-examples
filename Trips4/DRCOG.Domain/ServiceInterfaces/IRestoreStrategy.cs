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

namespace DRCOG.Domain.ServiceInterfaces
{
    public interface IRestoreStrategy
    {
        object Restore(string timePeriod);
    }
}
