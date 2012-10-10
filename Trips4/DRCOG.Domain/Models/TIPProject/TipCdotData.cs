using System;
using DRCOG.Domain.Models;
using DRCOG.Domain.Interfaces;
using DRCOG.Domain.Factories;

namespace DRCOG.Domain.Models.TIPProject
{
    public class TipCdotData : CdotDataBase
    {
        public IInstance versionModel;
        public TipCdotData()
        {
            versionModel = GenericAddOnFactory<IInstance, TipVersionModel>.CreateInstance();
        }
    }
}
