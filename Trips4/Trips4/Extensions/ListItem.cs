//======================================================
#region  Data Transfer Solutions License
//Copyright (c) 2008 Data Transfer Solutions (www.edats.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 12/5/2008 3:36:42 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace DRCOG.Web.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class DTSListItem : SelectListItem   // ListItem no longer in System.Web.Mvc namespace
    {

        ///<summary>
        /// Default Constructor
        /// </summary>
        ///<remarks></remarks>
        public DTSListItem(bool selected, string text, string value)
        {
            base.Selected = selected;
            base.Text = text;
            base.Value = value;
        }
    }
}
