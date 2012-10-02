using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Proto.Data
{
    public class Repository
    {
        public List<DashboardItem> GetSurveyDashboardItemsBySponsor(string year, int? version = null, bool showEmpty = false)
        {
            var items = new List<DashboardItem>();
            using (var db = new TRIPSEntities())
            {
                var found = db.GetSurveyDashboardListBySponsor2(year, version, showEmpty); 
                items.AddRange(found.Select(f => new DashboardItem()
                {
                    Count = f.Count.Value,
                    ItemDate = f.ItemDate,
                    ItemName = f.ItemName
                }));
            }
            return items;
        }
    }
}
