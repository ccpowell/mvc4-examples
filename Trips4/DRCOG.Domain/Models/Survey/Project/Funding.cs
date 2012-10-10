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

namespace DRCOG.Domain.Models.Survey
{
    public class Funding
    {
        public Funding()
        {
            //ReportGroupingCategories = new Dictionary<int, string>();
            //ReportGroupingCategoriesDetail = new List<PlanReportGroupingCategory>();
        }

        //public Funding(int projectVersionId)
        //{
        //    this.ProjectVersionId = projectVersionId;
        //}

        //public int ProjectVersionId { get; set; }

        public Decimal ConstantCost { get; set; }
        public Decimal VisionCost { get; set; }
        public Decimal AmendedCost { get; set; }
        public Decimal YOECost { get; set; }
        public Decimal Previous { get; set; }
        public Decimal Future { get; set; }
        public Decimal TotalCost { get; set; }
        public Int32 ReportGroupingCategoryId { get; set; }
        public int PlanTypeId { get; set; }

    }

    public class FundingSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
