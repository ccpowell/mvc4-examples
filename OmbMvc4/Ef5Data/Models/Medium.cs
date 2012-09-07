using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Medium
    {
        public System.Guid mediaId { get; set; }
        public System.DateTime dateCreated { get; set; }
        public string fileName { get; set; }
        public string mediaType { get; set; }
        public byte[] file { get; set; }
    }
}
