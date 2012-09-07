using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class CategoryType
    {
        public CategoryType()
        {
            this.Categories = new List<Category>();
        }

        public int CategoryTypeID { get; set; }
        public string CategoryType1 { get; set; }
        public Nullable<int> ParentCategoryTypeID { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
