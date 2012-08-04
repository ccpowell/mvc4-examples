using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmbMf.Models
{
    public class DataTablesQuery
    {
        public int iColumns { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iListId { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
        public string sEcho { get; set; }
        public string sSearch { get; set; }
    }
}