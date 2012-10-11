//======================================================
#region DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/10/2009 11:38:36 AM
// Description:
//
//======================================================
using System;
using DRCOG.Domain.Interfaces;
using System.Net;
using System.Net.Mail;
using DRCOG.Domain.Utility;
using System.Net.Configuration;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

namespace Trips4.Services
{
    /// <summary>
    /// </summary>
    public class EmailService : IEmailService
    {
        readonly string From;
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        readonly string Host;

        readonly MailSettingsSectionGroup MailSettings;

        public EmailService()
        {
            //From = ConfigurationManager.AppSettings["smtpfrom"].ToString();
            //Host = ConfigurationManager.AppSettings["host"].ToString();
        }

        public void Send()
        {
            this.Send("no.reply@drcog.org", "TRIPS Notification Service");
        }

        public void Send(string fromAddress, string fromDisplayName)
        {
            MailMessage email = new MailMessage(new MailAddress(fromAddress, fromDisplayName), new MailAddress(To));
            email.Subject = Subject;
            email.Body = Body;
            email.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient()
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 25,
                Timeout = 60000
            };

            RetryUtility.RetryAction(() => smtp.Send(email), 3, 1000);
        }
    }
}
