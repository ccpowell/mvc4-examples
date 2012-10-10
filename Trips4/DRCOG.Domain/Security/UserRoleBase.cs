using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace DRCOG.Domain.Security
{
    public abstract class UserRoleBase
    {
        public int PersonID { get; set; }
        public string PersonGUID { get; set; }

        public string[] Roles { get; set; }

        public abstract bool IsInRole(string role);

        public abstract void Load();

        public abstract void Save();
    }
}
