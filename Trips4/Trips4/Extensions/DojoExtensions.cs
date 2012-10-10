//======================================================
#region  Data Transfer Solutions License
//Copyright (c) 2008 Data Transfer Solutions (www.edats.com)
#endregion
//======================================================
// Author: dbouwman
// Date Created: 12/5/2008 1:32:52 PM
// Description:
//
//======================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;

using DTS.Extensions;

namespace DRCOG.Web.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DojoExtensions
    {
        #region DojoTextbox

        public static string DojoTextbox(this HtmlHelper helper, string id, string text, bool readOnly)
        {
            return DojoTextbox(helper, id, text, readOnly, (object)null /* htmlAttributes */);
        }


        public static string DojoTextbox(this HtmlHelper helper, string id, string text, bool readOnly, object htmlAttributes)
        {
            return DojoTextbox(helper, id, text, readOnly, new RouteValueDictionary(htmlAttributes));
        }

        public static string DojoTextbox(this HtmlHelper helper, string id, string text, bool readOnly, IDictionary<string, object> htmlAttributes)
        {
            string output;
            TagBuilder tagBuilder;
            ClassAttributeFix(ref htmlAttributes);

            if (readOnly)
            {
                tagBuilder = new TagBuilder("div");
                tagBuilder.MergeAttribute("id", id);
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.SetInnerText(text);
                output = tagBuilder.ToString(TagRenderMode.Normal);
            }
            else
            {
                tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.MergeAttribute("dojoType", "dijit.form.TextBox");
                tagBuilder.MergeAttribute("id", id);
                tagBuilder.MergeAttribute("value", text);
                output = tagBuilder.ToString(TagRenderMode.SelfClosing);
            }
            return output;
        }

        #endregion

        #region DojoValidationTextbox

        public static string DojoValidationTextbox(this HtmlHelper helper, string id, string text, bool readOnly, bool required, string regExp, string invalidMessage)
        {
            return DojoValidationTextbox(helper, id, text, readOnly, required, regExp, invalidMessage, (object)null /* htmlAttributes */);
        }


        public static string DojoValidationTextbox(this HtmlHelper helper, string id, string text, bool readOnly, bool required, string regExp, string invalidMessage, object htmlAttributes)
        {
            return DojoValidationTextbox(helper, id, text, readOnly, required, regExp, invalidMessage, new RouteValueDictionary(htmlAttributes));
        }

        public static string DojoValidationTextbox(this HtmlHelper helper, string id, string text, bool readOnly, bool required, string regExp, string invalidMessage, IDictionary<string, object> htmlAttributes)
        {
            string output;
            TagBuilder tagBuilder;
            ClassAttributeFix(ref htmlAttributes);

            if (readOnly)
            {
                tagBuilder = new TagBuilder("div");
                tagBuilder.MergeAttribute("id", id);
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.SetInnerText(text);
                output = tagBuilder.ToString(TagRenderMode.Normal);
            }
            else
            {
                tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttribute("id", id);
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.MergeAttribute("dojoType", "dijit.form.ValidationTextBox");

                if (required)
                {
                    tagBuilder.MergeAttribute("required", "true");
                }

                if (!string.IsNullOrEmpty(regExp))
                {
                    tagBuilder.MergeAttribute("regExp", regExp);
                }

                tagBuilder.MergeAttribute("invalidMessage", invalidMessage);

                tagBuilder.MergeAttribute("value", text);

                output = tagBuilder.ToString(TagRenderMode.SelfClosing);
            }
            return output;
        }

        #endregion

        #region DojoDateTextBox

        public static string DojoDateTextBox(this HtmlHelper helper, string id, DateTime date, bool readOnly)
        {
            return DojoDateTextBox(helper, id, date, readOnly, (object)null);
        }

        public static string DojoDateTextBox(this HtmlHelper helper, string id, DateTime date, bool readOnly, object htmlAttributes)
        {
            return DojoDateTextBox(helper, id, date, readOnly, new RouteValueDictionary(htmlAttributes));
        }

        public static string DojoDateTextBox(this HtmlHelper helper, string id, DateTime date, bool readOnly, IDictionary<string, object> htmlAttributes)
        {
            string output;
            TagBuilder tagBuilder;
            ClassAttributeFix(ref htmlAttributes);

            if (readOnly)
            {
                tagBuilder = new TagBuilder("div");
                tagBuilder.MergeAttribute("id", id);
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.SetInnerText(date.ToShortDateString());
                output = tagBuilder.ToString(TagRenderMode.Normal);
            }
            else
            {
                tagBuilder = new TagBuilder("input");
                tagBuilder.MergeAttributes(htmlAttributes);
                tagBuilder.MergeAttribute("dojoType", "dijit.form.DateTextBox");
                tagBuilder.MergeAttribute("id", id);
                tagBuilder.MergeAttribute("value", date.ToString("yyyy-MM-dd"));
                tagBuilder.MergeAttribute("required", "true");
                tagBuilder.MergeAttribute("promptMessage", "mm/dd/yyyy");
                tagBuilder.MergeAttribute("invalidMessage", "Invalid date. Use mm/dd/yyyy format.");
                output = tagBuilder.ToString(TagRenderMode.SelfClosing);
                // onChange="dojo.byId('oc2').innertHTML=arguments[0]"      
                //<div id="oc2"></div>   
            }
            return output;
        }

        #endregion

        #region DojoFilteringSelect

        public static string DojoFilteringSelect(this HtmlHelper helper, string id, object selectedValue, SelectList selectList, bool readOnly)
        {
            return DojoFilteringSelect(helper, id, selectedValue, selectList, readOnly, (object)null);
        }

        public static string DojoFilteringSelect(this HtmlHelper helper, string id, object selectedValue, SelectList selectList, bool readOnly, object htmlAttributes)
        {
            return DojoFilteringSelect(helper, id, selectedValue, selectList, readOnly, new RouteValueDictionary(htmlAttributes));
        }

        //public static string DojoFilteringSelect(this HtmlHelper helper, string id, object selectedValue, SelectList selectList, bool readOnly, IDictionary<string, object> htmlAttributes)
        //{
        //    string output = "";
        //    TagBuilder builder;
        //    ClassAttributeFix(ref htmlAttributes);

        //    if (readOnly)
        //    {
        //        builder = new TagBuilder("div");
        //        builder.MergeAttribute("id", id);
        //        builder.MergeAttributes(htmlAttributes);
        //        foreach (ListItem item in selectList.Items)
        //        {
        //            if (item.Value.ToString() == selectedValue.ToString().ToLower())
        //            {
        //                builder.SetInnerText(item.Text);
        //            }
        //        }

        //        output = builder.ToString(TagRenderMode.Normal);

        //    }
        //    else
        //    {

        //        StringBuilder listItemBuilder = new StringBuilder();                
        //        foreach (ListItem item in selectList.Items)
        //        {
        //            listItemBuilder.AppendLine(ListItemToOption(item));
        //        }
        //        builder = new TagBuilder("select") { InnerHtml = listItemBuilder.ToString() };
        //        builder.MergeAttributes(htmlAttributes);
        //        builder.MergeAttribute("id", id);
        //        output = builder.ToString(TagRenderMode.Normal);                

        //    }
        //    return output;

        //}
        //private static string ListItemToOption(ListItem item)
        //{
        //    TagBuilder builder = new TagBuilder("option")
        //    {
        //        InnerHtml = System.Web.HttpUtility.HtmlEncode(item.Text)
        //    };
        //    if (item.Value != null)
        //    {
        //        builder.Attributes["value"] = item.Value;
        //    }
        //    if (item.Selected)
        //    {
        //        builder.Attributes["selected"] = "selected";
        //    }
        //    return builder.ToString(TagRenderMode.Normal);
        //}

        public static string DojoFilteringSelect(this HtmlHelper helper, string id, object selectedValue, SelectList selectList, bool readOnly, IDictionary<string, object> htmlAttributes)
        {
            string output = "";
            TagBuilder builder;
            ClassAttributeFix(ref htmlAttributes);

            if (readOnly)
            {
                builder = new TagBuilder("div");
                builder.MergeAttribute("id", id);
                builder.MergeAttributes(htmlAttributes);
                foreach (SelectListItem item in selectList.Items)
                {
                    if (item.Value.ToString() == selectedValue.ToString().ToLower())
                    {
                        builder.SetInnerText(item.Text);
                    }
                }

                output = builder.ToString(TagRenderMode.Normal);

            }
            else
            {

                StringBuilder listItemBuilder = new StringBuilder();
                foreach (SelectListItem item in selectList.Items)
                {
                    listItemBuilder.AppendLine(ListItemToOption(item));
                }
                builder = new TagBuilder("select") { InnerHtml = listItemBuilder.ToString() };
                builder.MergeAttributes(htmlAttributes);
                builder.MergeAttribute("id", id);
                output = builder.ToString(TagRenderMode.Normal);

            }
            return output;

        }

        private static string ListItemToOption(SelectListItem item)
        {
            TagBuilder builder = new TagBuilder("option")
            {
                InnerHtml = System.Web.HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                builder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                builder.Attributes["selected"] = "selected";
            }
            return builder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region DojoMenu

        public static string DojoMenu(this HtmlHelper helper, string id, object menuSchema)
        {
            return DojoMenu(helper, id, menuSchema, (object)null);
        }

        public static string DojoMenu(this HtmlHelper helper, string id, object menuSchema, object htmlAttributes)
        {
            return DojoMenu(helper, id, menuSchema, new RouteValueDictionary(htmlAttributes));
        }

        public static string DojoMenu(this HtmlHelper helper, string id, object menuSchema, IDictionary<string, object> htmlAttributes)
        {
            throw new NotImplementedException();
            //string output = "";
            //TagBuilder builder;

            //StringBuilder menuBuilder = new StringBuilder();
            ////foreach (ListItem item in menuSchema.Items)
            ////{
            ////    menuBuilder.AppendLine(ListItemToOption(item));
            ////}
            //builder = new TagBuilder("div") { InnerHtml = menuBuilder.ToString() };
            //ClassAttributeFix(ref htmlAttributes);
            //builder.MergeAttributes(htmlAttributes);
            //builder.MergeAttribute("id", id);
            //output = builder.ToString(TagRenderMode.Normal);

            //return output;
        }

        #endregion

        #region DojoDataGrid

        //=========================
        // Author: mjuniper
        // Date Created: 01/20/2009
        //=========================

        /// <summary>
        /// Creates markup for a dojo grid and its datastore.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="id">The dom id for the grid.</param>
        /// <param name="data">The data to display in the grid. This object should mirror a dojo.data store.
        /// As such, it must have an items property that is a collection of the items to display,
        /// may also have identifier and label properties.</param>
        /// <param name="structure">The html structure of the grid to display.</param>
        /// <param name="numRows">The number of rows to display.</param>
        /// <param name="isEditable">if set to <c>true</c> the grid will use an ItemFileWriteStore, otherwise it will use an ItemFileReadStore.</param>
        /// <returns>
        /// The markup for a dojo grid and its datastore.
        /// </returns>
        /// <remarks>Whether the grid is editable is determined by the passed in structure and by the isEditable flag.</remarks>
        public static string DojoDataGrid(this HtmlHelper helper, string id, object data, string structure, int? numRows, bool isEditable)
        {
            return DojoDataGrid(helper, id, data, structure, numRows, isEditable, (object)null);
        }

        /// <summary>
        /// Creates markup for a dojo grid and its datastore.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="id">The dom id for the grid.</param>
        /// <param name="data">The data to display in the grid. This object should mirror a dojo.data store.
        /// As such, it must have an items property that is a collection of the items to display,
        /// may also have identifier and label properties.</param>
        /// <param name="structure">The html structure of the grid to display.</param>
        /// <param name="numRows">The number of rows to display.</param>
        /// <param name="isEditable">if set to <c>true</c> the grid will use an ItemFileWriteStore, otherwise it will use an ItemFileReadStore.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>
        /// The markup for a dojo grid and its datastore.
        /// </returns>
        /// <remarks>Whether the grid is editable is determined by the passed in structure and by the isEditable flag.</remarks>
        public static string DojoDataGrid(this HtmlHelper helper, string id, object data, string structure, int? numRows, bool isEditable, object htmlAttributes)
        {
            return DojoDataGrid(helper, id, data, structure, numRows, isEditable, new RouteValueDictionary(htmlAttributes));
        }

        /// <summary>
        /// Creates markup for a dojo grid and its datastore.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="id">The dom id for the grid.</param>
        /// <param name="data">The data to display in the grid. This object should mirror a dojo.data store.
        /// As such, it must have an items property that is a collection of the items to display,
        /// may also have identifier and label properties.</param>
        /// <param name="structure">The html structure of the grid to display.</param>
        /// <param name="numRows">The number of rows to display.</param>
        /// <param name="isEditable">if set to <c>true</c> the grid will use an ItemFileWriteStore, otherwise it will use an ItemFileReadStore.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>
        /// The markup for a dojo grid and its datastore.
        /// </returns>
        /// <remarks>Whether the grid is editable is determined by the passed in structure and by the isEditable flag.</remarks>
        public static string DojoDataGrid(this HtmlHelper helper, string id, object data, string structure, int? numRows, bool isEditable, IDictionary<string, object> htmlAttributes)
        {
            //Validate the arguments
            if (data == null)
            {
                throw new ArgumentNullException("data", "Data parameter was not specified.");
            }

            if (string.IsNullOrEmpty(structure))
            {
                throw new ArgumentNullException("structure", "Structure parameter must be specified.");
            }

            string output;
            TagBuilder builder;

            //build the element for the store
            builder = new TagBuilder("span");
            string storeType = (isEditable) ? "dojo.data.ItemFileWriteStore" : "dojo.data.ItemFileReadStore";
            builder.MergeAttribute("dojoType", storeType);
            builder.MergeAttribute("jsId", id + "_store");
            builder.MergeAttribute("data", data.ToJson());
            output = builder.ToString(TagRenderMode.Normal);

            ////passing in the structure as a javascript object - couldn't make this work
            //structure = "[[ { field: 'Title', name: 'Title', width: '150px' }, { field: 'Year', name: 'Year', width: '50px'}], [ { field: 'Producer', name: 'Producer', colSpan: 2 }]]";
            //builder.MergeAttribute("structure", structure);            

            builder = new TagBuilder("table");
            ClassAttributeFix(ref htmlAttributes);
            builder.MergeAttributes(htmlAttributes, true);

            if (numRows.HasValue)
            {
                builder.MergeAttribute("autoHeight", numRows.Value.ToString());
            }

            builder.MergeAttribute("id", id);
            builder.MergeAttribute("dojoType", /*"dts.widgets.DataGrid"*/"dojox.grid.DataGrid");
            builder.MergeAttribute("store", id + "_store");
            builder.InnerHtml = structure;
            output += builder.ToString(TagRenderMode.Normal);

            return output;
        }

        #endregion

        #region DojoDataStore

        /// <summary>
        /// Creates markup for a dojo datastore.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="id">The dom id for the datastore.</param>
        /// <param name="data">The data behind the datastore. This object should mirror a dojo.data store.
        /// As such, it must have an items property that is a collection of the items to display.</param>
        /// <param name="isEditable">if set to <c>true</c> the grid will use an ItemFileWriteStore, otherwise it will use an ItemFileReadStore.</param>
        /// <returns>
        /// The markup for a dojo datastore.
        /// </returns>
        /// <remarks>Whether the datastore is editable is determined by the passed in isEditable flag.</remarks>
        public static string DojoDataStore(this HtmlHelper helper, string id, object data, bool isEditable)
        {
            //Validate the arguments
            if (data == null)
            {
                throw new ArgumentNullException("data", "Data parameter was not specified.");
            }

            TagBuilder builder = new TagBuilder("span");
            string storeType = (isEditable) ? "dojo.data.ItemFileWriteStore" : "dojo.data.ItemFileReadStore";
            builder.MergeAttribute("dojoType", storeType);
            builder.MergeAttribute("jsId", id + "_store");
            builder.MergeAttribute("data", data.ToJson());
            return builder.ToString(TagRenderMode.Normal);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Converts the _class attribute in htmlAttributes, if present, to class with the same value
        /// </summary>
        /// <param name="htmlAttributes"></param>
        /// <remarks>The attributes are passed in as an anonymous type and class is a reserved word in c#.
        /// However class is a very desirable attribute on an html element!
        /// This allows us to pass in _class and have it create an html element with a class attribute instead.</remarks>
        private static void ClassAttributeFix(ref IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes.ContainsKey("_class"))
            {
                htmlAttributes.Add("class", htmlAttributes["_class"].ToString());
                htmlAttributes.Remove("_class");
            }
        }

        #endregion

    }
}