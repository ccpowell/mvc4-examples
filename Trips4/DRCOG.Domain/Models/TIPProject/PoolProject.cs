#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 02/25/2010   DTucker         1. Initial Creation.
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion
using System;

namespace DRCOG.Domain.Models.TIPProject
{
    public class PoolProject
    {
        public Int32 PoolProjectID { get; set; }
        public String ProjectName { get; set; }
        public String Description { get; set; }
        public String BeginAt { get; set; }
        public String EndAt { get; set; }
        public Decimal? Cost { get; set; }
        public Int32? PoolMasterVersionID { get; set; }
    }
}
