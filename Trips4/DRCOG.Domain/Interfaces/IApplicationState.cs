using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.Models;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.Domain.Interfaces
{
    public interface IApplicationState
    {
        string CurrentProgram { get; set; }
        Person CurrentUser { get; set; }
        ProjectSearchModel ProjectSearchModel { get; set; }
    }
}
