#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 07/07/2010	DTucker        1. Initial Creation.
 * DESCRIPTION:
 * 
 * ======================================================*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Domain.Models.RTP
{
    public class Funding : RtpVersionModel
    {
        public Funding()
        {
            ReportGroupingCategories = new Dictionary<int, string>();
            ReportGroupingCategoriesDetail = new List<PlanReportGroupingCategory>();
        }


        public Decimal ConstantCost { get; set; }
        public Decimal VisionCost { get; set; }
        public Decimal YOECost { get; set; }
        public Decimal Previous { get; set; }
        public Decimal Future { get; set; }
        public Decimal TotalCost { get; set; }
        public Int32 ReportGroupingCategoryId { get; set; }
        public int PlanTypeId { get; set; }

        public IDictionary<int, string> ReportGroupingCategories { get; set; }
        public IList<PlanReportGroupingCategory> ReportGroupingCategoriesDetail { get; set; }
    }

    public class PlanReportGroupingCategory
    {
        public int CategoryId { get; set; }
        public string Category { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
    }

    public class FundingSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
