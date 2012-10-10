using System;
using DRCOG.Domain.Models;
using DRCOG.Common.Services.MemberShipServiceSupport;

namespace DRCOG.Domain.ViewModels
{
    public class BecomeASponsorViewModel
    {
        public Profile Profile { get; set; }
        public ProjectSponsorsModel ProjectSponsorsModel { get; set; }
    }
}
