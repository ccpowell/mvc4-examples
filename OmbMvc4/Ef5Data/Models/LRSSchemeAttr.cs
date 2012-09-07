using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class LRSSchemeAttr
    {
        public int Id { get; set; }
        public int LRSSchemeId { get; set; }
        public string COLUMN_NAME { get; set; }
        public string FRIENDLY_NAME { get; set; }
        public Nullable<int> DATA_TYPE { get; set; }
        public Nullable<int> DISPLAY_TYPE { get; set; }
        public Nullable<int> CHARACTER_MAXIMUM_LENGTH { get; set; }
        public string COLUMN_DEFAULT { get; set; }
        public bool IS_NULLABLE { get; set; }
        public bool IS_REQUIRED { get; set; }
        public virtual LRSCategory LRSCategory { get; set; }
        public virtual LRSCategory LRSCategory1 { get; set; }
        public virtual LRSScheme LRSScheme { get; set; }
    }
}
