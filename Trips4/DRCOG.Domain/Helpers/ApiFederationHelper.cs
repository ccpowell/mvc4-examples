using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DRCOG.Domain.ServiceInterfaces;
using System.Web.Configuration;

namespace DRCOG.Domain.Helpers
{
    public static class ApiFederationHelper
    {
        private static IApiFederation innerInstance = null;
        public static IApiFederation FederationObject
        {
            get
            {
                if (innerInstance == null)
                    innerInstance = loadFederationType();
                return innerInstance;
            }
        }

        private static IApiFederation loadFederationType()
        {
            string typeName = WebConfigurationManager.AppSettings["apiFederationType"];
            Type loadedType = Type.GetType(typeName);
            return Activator.CreateInstance(loadedType) as IApiFederation;
        }
    }
}


