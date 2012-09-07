using System;
using System.Collections.Generic;

namespace Ef5Data.Models
{
    public class Document
    {
        public int DocumentID { get; set; }
        public string Filename { get; set; }
        public Nullable<int> DocumentTypeID { get; set; }
        public Nullable<System.Guid> MediaGuid { get; set; }
        public virtual DocumentType DocumentType { get; set; }
    }
}
