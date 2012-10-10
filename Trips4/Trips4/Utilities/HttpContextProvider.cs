using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using DRCOG.Interfaces;

namespace DRCOG.Web
{
    /// <summary>
    /// From CodeCampServer example
    /// </summary>
    public class HttpContextProvider : IHttpContextProvider
    {

        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        public HttpContextBase GetCurrentHttpContext()
        {
            return new HttpContextWrapper(HttpContext.Current);
        }

        public HttpSessionStateBase GetHttpSession()
        {
            var session = GetCurrentHttpContext().Session;
            if (session.IsNewSession)
            {
                Logger.Debug("Session is new.");
            }
            return session;
        }
    }
}
