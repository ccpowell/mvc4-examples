//======================================================
#region  Data Transfer Solutions License
//Copyright (c) 2008 Data Transfer Solutions (www.edats.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 10/3/2008 12:57:00 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Text;

namespace DRCOG.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClock
    {
        DateTime GetCurrentTime();
    }
}
