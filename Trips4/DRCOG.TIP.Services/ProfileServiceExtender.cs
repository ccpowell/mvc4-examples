using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Common.Services.MemberShipServiceSupport;
using DRCOG.Domain.Interfaces;
using DRCOG.Data;

namespace DRCOG.TIP.Services
{
    public class ProfileServiceExtender : ProfileService
    {
        public ProfileServiceExtender()
            : this(new UserRepository()) { }

        public ProfileServiceExtender(IUserRepositoryExtension userRepository)
            : base(userRepository) { }

    }
}
