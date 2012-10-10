using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Common.Domain.Attributes;

namespace DRCOG.Domain.Models
{
    public class SiteUser : Entity<Int64>
    {
        [DomainSignature]
        public virtual String ActiveDirectoryName
        {
            get;
            set;
        }

        public virtual String Name
        {
            get;
            set;
        }

        public virtual String PhoneNumber
        {
            get;
            set;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
