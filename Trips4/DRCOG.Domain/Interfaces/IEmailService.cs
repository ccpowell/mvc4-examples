//======================================================
#region  DTSAgile License
//Copyright (c) 2009 DTSAgile (www.DTSAgile.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 7/10/2009 11:37:57 AM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;

namespace DRCOG.Domain.Interfaces
{
    /// <summary>
    /// 
    /// </summary>    
    public interface IEmailService
    {
       
        //void SendEmail(string server, int port, bool useSSL, string smtpUserName, string smtpPassword, string fromAddress, string toAddress, string subject, string body);
        void Send();
        void Send(string fromAddress, string fromDisplayName);
    }
}
