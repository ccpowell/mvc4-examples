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
using DRCOG.Domain.Interfaces;
using System.Collections.Generic;

namespace DRCOG.Domain.Models
{
    public abstract class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
