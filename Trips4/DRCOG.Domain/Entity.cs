using System;
using DRCOG.Common.Domain;

namespace DRCOG.Domain
{
    [Serializable]
    public abstract class Entity<IdT> : CommonEntity<IdT>
    {
    }
}
