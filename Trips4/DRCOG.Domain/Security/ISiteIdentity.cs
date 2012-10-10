using System;
using DRCOG.Common.Security;

namespace DRCOG.Domain.Security
{
    public interface ISiteIdentity : IGenericUserIdentity
    {
        String PhoneNumber { get; }

        String Email { get; }
    }
}
