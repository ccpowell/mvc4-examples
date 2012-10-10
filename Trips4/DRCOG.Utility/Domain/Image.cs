using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DRCOG.Utility.Domain
{
    public class Image
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String AlternateText { get; set; }
        public Byte[] File { get; set; }
        public int ContentTypeId { get; set; }
        public String ContentType { get; set; }
        //public String ContentType { get; set; }
    }
}
