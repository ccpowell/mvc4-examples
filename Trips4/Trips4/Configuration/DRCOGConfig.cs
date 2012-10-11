//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/10/2009 10:57:42 AM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Trips4.Configuration
{
    /// <summary>
    /// </summary>
    public class DRCOGConfig : ConfigurationSection
    {
        /// <summary>
        /// Gets the config.
        /// </summary>
        /// <returns></returns>
        public static DRCOGConfig GetConfig()
        {
            return ConfigurationManager.GetSection("DRCOG") as DRCOGConfig;
        }
        /// <summary>
        /// Gets the AdminEmail.
        /// </summary>
        /// <value>The AdminEmail.</value>
        [ConfigurationProperty("AdminEmail", IsRequired = true)]
        public string AdminEmail
        {
            get
            {
                return this["AdminEmail"] as string;
            }
        }

        /// <summary>
        /// Instead of trying to work this out dynamically, which is
        /// problematic during unit testing, we'll just specify it in
        /// the config file
        /// </summary>
        [ConfigurationProperty("ChangePasswordBaseUrl", IsRequired = true)]
        public string ChangePasswordBaseUrl
        {
            get
            {
                return this["ChangePasswordBaseUrl"] as string;
            }
        }

        [ConfigurationProperty("EmailConfirmationPage", IsRequired = true)]
        public string EmailConfirmationPage
        {
            get
            {
                return this["EmailConfirmationPage"] as string;
            }
        }

        [ConfigurationProperty("PasswordRecoveryPage", IsRequired = true)]
        public string PasswordRecoveryPage
        {
            get
            {
                return this["PasswordRecoveryPage"] as string;
            }
        }

        /// <summary>
        /// Gets the SMTPServer.
        /// </summary>
        /// <value>The SMTPServer.</value>
        [ConfigurationProperty("SMTPServer", IsRequired = true)]
        public string SMTPServer
        {
            get
            {
                return this["SMTPServer"] as string;
            }
        }

        /// <summary>
        /// Gets the SMTPPort.
        /// </summary>
        /// <value>The SMTPPort.</value>
        [ConfigurationProperty("SMTPPort", IsRequired = true)]
        public int? SMTPPort
        {
            get
            {
                return this["SMTPPort"] as int?;
            }
        }

        /// <summary>
        /// Gets the SMTPUserName.
        /// </summary>
        /// <value>The SMTPUserName.</value>
        [ConfigurationProperty("SMTPUserName", IsRequired = true)]
        public string SMTPUserName
        {
            get
            {
                return this["SMTPUserName"] as string;
            }
        }

        /// <summary>
        /// Gets the SMTPPassword.
        /// </summary>
        /// <value>The SMTPPassword.</value>
        [ConfigurationProperty("SMTPPassword", IsRequired = true)]
        public string SMTPPassword
        {
            get
            {
                return this["SMTPPassword"] as string;
            }
        }

        /// <summary>
        /// Gets the SMTPUseSSL.
        /// </summary>
        /// <value>The SMTPUseSSL.</value>
        [ConfigurationProperty("SMTPUseSSL", IsRequired = true)]
        public bool? SMTPUseSSL
        {
            get
            {
                return this["SMTPUseSSL"] as bool?;
            }
        }

        [ConfigurationProperty("LocationMapPath", IsRequired = true)]
        public string LocationMapPath
        {
            get
            {
                return this["LocationMapPath"] as String;
            }
        }

        /// <summary>
        /// Gets the ProjectMapServiceRestUrl.
        /// </summary>
        /// <value>The ProjectMapServiceRestUrl.</value>
        //[ConfigurationProperty("ProjectMapServiceRestUrl", IsRequired = true)]
        //public string ProjectMapServiceRestUrl
        //{
        //    get
        //    {
        //        string restUrl = this["ProjectMapServiceRestUrl"] as string;

        //        restUrl = (!restUrl.EndsWith("/")) ? restUrl + "/" : restUrl;

        //        return restUrl;
        //    }
        //}

        /// <summary>
        /// Gets the GeometryServiceRestUrl.
        /// </summary>
        /// <value>The GeometryServiceRestUrl.</value>
        //[ConfigurationProperty("GeometryServiceRestUrl", IsRequired = true)]
        //public string GeometryServiceRestUrl
        //{
        //    get
        //    {
        //        string restUrl = this["GeometryServiceRestUrl"] as string;

        //        restUrl = (!restUrl.EndsWith("/")) ? restUrl + "/" : restUrl;

        //        return restUrl;
        //    }
        //}

        /// <summary>
        /// Gets the ProjectMapServiceRestUrl.
        /// </summary>
        /// <value>The ProjectMapServiceRestUrl.</value>
        //[ConfigurationProperty("SiteMapServiceRestUrl", IsRequired = true)]
        //public string SiteMapServiceRestUrl
        //{
        //    get
        //    {
        //        string restUrl = this["SiteMapServiceRestUrl"] as string;

        //        restUrl = (!restUrl.EndsWith("/")) ? restUrl + "/" : restUrl;

        //        return restUrl;
        //    }
        //}

        /// <summary>
        /// Gets the UploadDirectory.
        /// </summary>
        /// <value>The UploadDirectory.</value>
        [ConfigurationProperty("UploadDirectory", IsRequired = true)]
        public string UploadDirectory
        {
            get
            {
                return this["UploadDirectory"] as string;
            }
        }


    }
}
