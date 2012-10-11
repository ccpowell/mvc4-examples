using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel;
using System.Resources;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
//using DTS.Web.MVC.Extensions;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Security;


namespace DRCOG.Web.Extensions
{
    //public class MvcHtmlString
    //{
    //    // Fields
    //    private static readonly MvcHtmlStringCreator _creator = GetCreator();
    //    private readonly string _value;
    //    public static readonly MvcHtmlString Empty = Create(string.Empty);

    //    public static MvcHtmlString Create(string value)
    //    {
    //        return _creator(value);
    //    }

    //    private static MvcHtmlStringCreator GetCreator()
    //    {
    //        Type type = typeof(HttpContext).Assembly.GetType("System.Web.IHtmlString");
    //        if (type != null)
    //        {
    //            ParameterExpression expression;
    //            Type type2 = DynamicTypeGenerator.GenerateType("DynamicMvcHtmlString", typeof(MvcHtmlString), new Type[] { type });
    //            return Expression.Lambda<MvcHtmlStringCreator>(Expression.New(type2.GetConstructor(new Type[] { typeof(string) }), new Expression[] { expression = Expression.Parameter(typeof(string), "value") }), new ParameterExpression[] { expression }).Compile();
    //        }
    //        return delegate(string value)
    //        {
    //            return new MvcHtmlString(value);
    //        };
    //    }

    //    public static bool IsNullOrEmpty(MvcHtmlString value)
    //    {
    //        if (value != null)
    //        {
    //            return (value._value.Length == 0);
    //        }
    //        return true;
    //    }

    //    public string ToHtmlString()
    //    {
    //        return this._value;
    //    }

    //    public override string ToString()
    //    {
    //        return this._value;
    //    }

    //    // Nested Types
    //    private delegate MvcHtmlString MvcHtmlStringCreator(string value);
    //}

    internal static class DynamicTypeGenerator
    {
        // Fields
        private static readonly ModuleBuilder _dynamicModule = CreateDynamicModule();

        // Methods
        private static ModuleBuilder CreateDynamicModule()
        {
            CustomAttributeBuilder builder = new CustomAttributeBuilder(typeof(SecurityTransparentAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
            CustomAttributeBuilder[] assemblyAttributes = new CustomAttributeBuilder[] { builder };
            return AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("System.Web.Mvc.{Dynamic}"), AssemblyBuilderAccess.Run, assemblyAttributes).DefineDynamicModule("System.Web.Mvc.{Dynamic}.dll");
        }

        public static Type GenerateType(string dynamicTypeName, Type baseType, IEnumerable<Type> interfaceTypes)
        {
            TypeBuilder newType = _dynamicModule.DefineType("System.Web.Mvc.{Dynamic}." + dynamicTypeName, TypeAttributes.Public, baseType);
            foreach (Type type in interfaceTypes)
            {
                newType.AddInterfaceImplementation(type);
                foreach (MethodInfo info in type.GetMethods())
                {
                    ImplementInterfaceMethod(newType, info);
                }
            }
            foreach (ConstructorInfo info2 in baseType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                switch ((info2.Attributes & MethodAttributes.MemberAccessMask))
                {
                    case MethodAttributes.Family:
                    case MethodAttributes.FamORAssem:
                    case MethodAttributes.Public:
                        ImplementConstructor(newType, info2);
                        break;
                }
            }
            return newType.CreateType();
        }

        private static void ImplementConstructor(TypeBuilder newType, ConstructorInfo baseCtor)
        {
            ParameterInfo[] parameters = baseCtor.GetParameters();
            Type[] parameterTypes = parameters.Select<ParameterInfo, Type>(delegate(ParameterInfo p)
            {
                return p.ParameterType;
            }).ToArray<Type>();
            ConstructorBuilder builder = newType.DefineConstructor((baseCtor.Attributes & ~MethodAttributes.MemberAccessMask) | MethodAttributes.Public, baseCtor.CallingConvention, parameterTypes);
            for (int i = 0; i < parameters.Length; i++)
            {
                builder.DefineParameter(i + 1, parameters[i].Attributes, parameters[i].Name);
            }
            ILGenerator iLGenerator = builder.GetILGenerator();
            for (int j = 0; j <= parameterTypes.Length; j++)
            {
                iLGenerator.Emit(OpCodes.Ldarg_S, (byte)j);
            }
            iLGenerator.Emit(OpCodes.Call, baseCtor);
            iLGenerator.Emit(OpCodes.Ret);
        }

        private static void ImplementInterfaceMethod(TypeBuilder newType, MethodInfo interfaceMethod)
        {
            ParameterInfo[] parameters = interfaceMethod.GetParameters();
            Type[] parameterTypes = parameters.Select<ParameterInfo, Type>(delegate(ParameterInfo p)
            {
                return p.ParameterType;
            }).ToArray<Type>();
            MethodBuilder methodInfoBody = newType.DefineMethod(interfaceMethod.DeclaringType.Name + "." + interfaceMethod.Name, MethodAttributes.VtableLayoutMask | MethodAttributes.HideBySig | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.Private, interfaceMethod.ReturnType, parameterTypes);
            MethodInfo method = newType.BaseType.GetMethod(interfaceMethod.Name, parameterTypes);
            for (int i = 0; i < parameters.Length; i++)
            {
                methodInfoBody.DefineParameter(i + 1, parameters[i].Attributes, parameters[i].Name);
            }
            ILGenerator iLGenerator = methodInfoBody.GetILGenerator();
            for (int j = 0; j <= parameterTypes.Length; j++)
            {
                iLGenerator.Emit(OpCodes.Ldarg_S, (byte)j);
            }
            iLGenerator.Emit(OpCodes.Call, method);
            iLGenerator.Emit(OpCodes.Ret);
            newType.DefineMethodOverride(methodInfoBody, interfaceMethod);
        }
    }

    

    public static class HtmlExtensions
    {
        #region Textbox

        public static MvcHtmlString DrcogTextBox(this HtmlHelper helper, string name, bool isEditable, object value, IDictionary<string, object> htmlAttributes)
        {
            if (htmlAttributes != null)
            {
                ClassAttributeFix(ref htmlAttributes);
            }
            if (isEditable)
            {
                return helper.TextBox(name, value, htmlAttributes);
            }
            else
            {
                //htmlAttributes.Add(new KeyValuePair<string, object>("disabled", "disabled"));
                //return helper.DrcogTextBox(name, value, htmlAttributes);

                return helper.FakeInput(TagName.span, name, value, htmlAttributes);
            }
        }

        public static string SimpleCheckBox(this HtmlHelper helper, string name, bool isChecked, string id, bool isEditable)
        {
            string output;
            TagBuilder tagBuilder;

            tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttribute("id", id);
            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", name);
            if (isChecked)
            {
                tagBuilder.MergeAttribute("checked", "checked");
            }
            if (!isEditable)
            {
                tagBuilder.MergeAttribute("disabled", "disabled");
            }

            output = tagBuilder.ToString(TagRenderMode.Normal);
            return output;
        }

        public static MvcHtmlString DrcogTextBox(this HtmlHelper helper, string name, bool isEditable)
        {
            return DrcogTextBox(helper, name, isEditable, null, null);
        }

        public static MvcHtmlString DrcogTextBox(this HtmlHelper helper, string name, bool isEditable, object value)
        {
            return DrcogTextBox(helper, name, isEditable, value, (object)null);
        }

        public static MvcHtmlString DrcogTextBox(this HtmlHelper helper, string name, bool isEditable, object value, object htmlAttributes)
        {
            return DrcogTextBox(helper, name, isEditable, value, new RouteValueDictionary(htmlAttributes));
        }

        //public static string DrcogTextBox(this HtmlHelper helper, string name, bool isEditable, object value, IDictionary<string, object> htmlAttributes)
        //{
        //    if (htmlAttributes != null)
        //    {
        //        ClassAttributeFix(ref htmlAttributes);
        //    }
        //    if (isEditable)
        //    {
        //        return helper.DrcogTextBox(name, value, htmlAttributes);
        //    }
        //    else
        //    {
        //        htmlAttributes.Add(new KeyValuePair<string, object>("disabled", "disabled"));
        //        return helper.DrcogTextBox(name, value, htmlAttributes);
        //    }
        //}



        public static MvcHtmlString TextArea2(this HtmlHelper helper, string id, bool isEditable, object value, int rows, int cols)
        {
            return TextArea2(helper, id, isEditable, value, rows, cols, new RouteValueDictionary((object)null));
        }

        /// <summary>
        /// Extension for TextArea which supports disabled
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="id"></param>
        /// <param name="isEditable"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString TextArea2(this HtmlHelper helper, string id, bool isEditable, object value, int rows, int cols, object htmlAttributes)
        {
            MvcHtmlString output;
            TagBuilder tagBuilder;
            IDictionary<string, object> htmlDict = new RouteValueDictionary(htmlAttributes);

            if (!isEditable) 
            {
                if (htmlDict.ContainsKey("class"))
                {
                    var temp = htmlDict["class"].ToString();
                    htmlDict["class"] = temp + " nobind";

                    //still needs to be tested
                }
            }
            ClassAttributeFix(ref htmlDict);


            if (isEditable)
            {
                tagBuilder = new TagBuilder("textarea");
                tagBuilder.MergeAttribute("id", id);
                tagBuilder.MergeAttribute("name", id);
                tagBuilder.MergeAttributes(htmlDict);

                tagBuilder.SetInnerText(value != null ? value.ToString() : String.Empty);
                output = MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.Normal));
            }
            else
            {
                output = helper.FakeTextArea(TagName.div, id, value, rows, htmlDict);
                //tagBuilder = new TagBuilder("textarea");
                //tagBuilder.MergeAttribute("id", id);
                //tagBuilder.MergeAttribute("name", id);
                //tagBuilder.MergeAttribute("rows", rows.ToString());
                //tagBuilder.MergeAttribute("cols", cols.ToString());
                //tagBuilder.MergeAttribute("readonly", "readonly");
                //tagBuilder.MergeAttributes(htmlDict);
                //tagBuilder.SetInnerText(value.ToString());
                //output = tagBuilder.ToString(TagRenderMode.Normal);
            }
            return output;
        }
       


        #endregion

        #region CustomBuilders

        public enum TagName
        {
            span
            ,
            div
        }

        public static MvcHtmlString FakeInput(this HtmlHelper helper, TagName tag, string id, object value, IDictionary<string, object> htmlAttributes)
        {
            string output;
            TagBuilder tagBuilder;
            value = value ?? String.Empty;

            tagBuilder = new TagBuilder(EnumExtensions.ToPrettyLowerString(tag));
            tagBuilder.MergeAttribute("id", id);
            
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.AddCssClass("fakeinput");
            tagBuilder.InnerHtml = value.ToString();

            output = tagBuilder.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(output);
        }

        public static MvcHtmlString FakeTextArea(this HtmlHelper helper, TagName tag, string id, object value, int rows, IDictionary<string, object> htmlAttributes)
        {
            string output;
            TagBuilder tagBuilder;
            value = value ?? String.Empty;

            tagBuilder = new TagBuilder(EnumExtensions.ToPrettyLowerString(tag));
            tagBuilder.MergeAttribute("id", id);
            //htmlAttributes.Add("height", (rows * 13) + "px");

            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.AddCssClass("faketextarea");
            tagBuilder.InnerHtml = value.ToString();

            output = tagBuilder.ToString(TagRenderMode.Normal);
            return MvcHtmlString.Create(output);
        }

        #endregion

        #region DropDownList
        //Adding an id and an isEditable argument to each of the System.Web.Mvc.HtmlHelper.DrcogTextBox overloads

        public static MvcHtmlString DropDownList(this HtmlHelper helper, string name, bool isEditable, SelectList selectList)
        {
            return DropDownList(helper, name, isEditable, selectList, (object)null);
        }

        public static MvcHtmlString DropDownList(this HtmlHelper helper, string name, bool isEditable, SelectList selectList, object htmlAttributes)
        {
            return DropDownList(helper, name, isEditable, selectList, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString DropDownList(this HtmlHelper helper, string name, bool isEditable, SelectList selectList, string optionLabel, object htmlAttributes)
        {
            var atts = new RouteValueDictionary(htmlAttributes);

            if (!isEditable)
            {
                atts.Add("disabled", "disabled");
            }

            return helper.DropDownList(name, selectList, optionLabel, atts);
        }

        public static MvcHtmlString DropDownList(this HtmlHelper helper, string name, bool isEditable, SelectList selectList, IDictionary<string, object> htmlAttributes)
        {
            ClassAttributeFix(ref htmlAttributes);

            if (!isEditable)
            {
                htmlAttributes.Add(new KeyValuePair<string, object>("disabled", "disabled"));
            }

            return helper.DropDownList(name, selectList, htmlAttributes);
        }
        public static MvcHtmlString DropDownList(this HtmlHelper helper, string name, bool isEditable, object selectedValue, SelectList selectList, object htmlAttributes)
        {
            return DropDownList(helper, name, isEditable, selectedValue, selectList, new RouteValueDictionary(htmlAttributes));

        }
        public static MvcHtmlString DropDownList(this HtmlHelper helper, string name, bool isEditable, object selectedValue, SelectList selectList, IDictionary<string, object> htmlAttributes)
        {
            ClassAttributeFix(ref htmlAttributes);

            if (!isEditable)
            {
                htmlAttributes.Add(new KeyValuePair<string, object>("disabled", "disabled"));
            }

            return helper.DropDownList(name, selectList, selectedValue.ToString(), htmlAttributes);


        }
        #endregion

        #region CheckBox
        //Adding an id and an isEditable argument to each of the System.Web.Mvc.HtmlHelper.CheckBox overloads

        public static MvcHtmlString CheckBox(this HtmlHelper helper, string name, bool isEditable, bool isChecked)
        {
            return CheckBox(helper, name, isEditable, isChecked, (object)null);
        }

        public static MvcHtmlString CheckBox(this HtmlHelper helper, string name, bool isEditable, bool isChecked, object htmlAttributes)
        {
            return CheckBox(helper, name, isEditable, isChecked, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString CheckBox(this HtmlHelper helper, string name, bool isEditable, bool isChecked, IDictionary<string, object> htmlAttributes)
        {
            ClassAttributeFix(ref htmlAttributes);
            if (isEditable)
            {
                return helper.CheckBox(name, isChecked, htmlAttributes);
            }
            else
            {
                htmlAttributes.Add(new KeyValuePair<string, object>("disabled", "disabled"));
                return helper.CheckBox(name, isChecked, htmlAttributes);
            }
        }

        #endregion

        #region CheckBoxWithValue

        public static string CheckBoxWithValue(this HtmlHelper htmlHelper, string name, string value, bool isChecked/*, bool setId*/, object htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("input");
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttribute("value", value);
            builder.MergeAttribute("name", name);
            if (isChecked)
            {
                builder.MergeAttribute("checked", "checked");
            }
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.Normal);
        }


        #endregion

        #region Button
        //HTML button element - pass a boolean enabled param to specify whether it is enabled or not

        public static string Button(this HtmlHelper helper, string name, string label, bool enabled)
        {
            return Button(helper, name, label, enabled, (object)null);
        }

        public static string Button(this HtmlHelper helper, string name, string label, bool enabled, object htmlAttributes)
        {
            return Button(helper, name, label, enabled, new RouteValueDictionary(htmlAttributes));
        }

        public static string Button(this HtmlHelper helper, string name, string label, bool enabled, IDictionary<string, object> htmlAttributes)
        {
            string output = "";
            TagBuilder tagBuilder = new TagBuilder("button");
            tagBuilder.MergeAttribute("id", name);
            tagBuilder.MergeAttribute("name", name);

            ClassAttributeFix(ref htmlAttributes);
            tagBuilder.MergeAttributes(htmlAttributes);

            tagBuilder.SetInnerText(label);

            if (!enabled) { tagBuilder.MergeAttribute("disabled", "disabled"); }

            output = tagBuilder.ToString(TagRenderMode.Normal);

            return output;
        }

        #endregion

        #region Image

        /// <summary>
        /// Images the specified helper.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="url">The URL.</param>
        /// <param name="altText">The alternate text.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>
        /// <remarks>
        /// <b>Usage</b>
        /// <code>
        /// <%= Html.Image( Url.Content( "~/Content/images/img.png" ), "alt text", new { id = "myImage", border = "0" } ) %>
        /// </code>
        /// </returns>
        /// </remarks>
        public static string Image(this HtmlHelper helper, string url, string altText, object htmlAttributes)
        {
            TagBuilder builder = new TagBuilder("image");
            builder.Attributes.Add("src", url);
            builder.Attributes.Add("alt", altText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }


        #endregion

        #region ActionLink

        public static MvcHtmlString SecureActionLink(this HtmlHelper htmlHelper, string linkText, string action, string controller, object htmlAttributes, bool enabled)
        {
            if (enabled)
            {
                return htmlHelper.ActionLink(linkText, action, controller, htmlAttributes);
            }
            else
            {
                TagBuilder builder = new TagBuilder("span");
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                builder.InnerHtml = linkText;
                return MvcHtmlString.Create(builder.ToString());
            }
        }

        public static MvcHtmlString ResolveUrl(this HtmlHelper htmlHelper, string url)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            return MvcHtmlString.Create(urlHelper.Content(url));
        }




        public static bool IsActionCurrent(this HtmlHelper htmlHelper, string action)
        {
            return (htmlHelper.ViewContext.RouteData.Values["action"].ToString().ToLower() == action.ToLower());


        }

        #region ImageActionLink

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string actionName, string imgSrc, string alt)
        {
            return ImageActionLink(helper, actionName, null, imgSrc, alt, (object)null, (object)null, (object)null);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string actionName, string imgSrc, string alt, object routeValues, object htmlAttributes, object imageAttributes)
        {
            return ImageActionLink(helper, actionName, null, imgSrc, alt, routeValues, htmlAttributes, imageAttributes);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string actionName, string controllerName, string imgSrc, string alt)
        {
            return ImageActionLink(helper, actionName, controllerName, imgSrc, alt, (object)null, (object)null, (object)null);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string actionName, string controllerName, string imgSrc, string alt, object routeValues, object htmlAttributes, object imageAttributes)
        {
            string img = helper.Image(imgSrc, alt, imageAttributes);
            string html = string.Format(helper.ActionLink("{0}", actionName, controllerName, routeValues, htmlAttributes).ToString(), img);

            return MvcHtmlString.Create(html);
        }

        #endregion

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
            if (htmlAttributes != null)
            {
                if (htmlAttributes.ContainsKey("_class"))
                {
                    htmlAttributes.Add("class", htmlAttributes["_class"].ToString());
                    htmlAttributes.Remove("_class");
                }

                if (htmlAttributes.ContainsKey("@class"))
                {
                    htmlAttributes.Add("class", htmlAttributes["@class"].ToString());
                    htmlAttributes.Remove("@class");
                }
            }
        }

        #endregion



        public static string DropDownList(this HtmlHelper helper, string name, bool isEditable, SelectList selectList, string optionLabel, object htmlAttributes, bool singleLine)
        {
            RouteValueDictionary dictionary = new RouteValueDictionary(htmlAttributes);
            if (!isEditable)
            {
                dictionary.Add("disabled", "disabled");
            }

            return helper.SelectInternal(optionLabel, name, selectList, false, ((IDictionary<string, object>) dictionary));
        }

        //private static void ClassAttributeFix(ref IDictionary<string, object> htmlAttributes)
        //{
        //    if (htmlAttributes != null)
        //    {
        //        if (htmlAttributes.ContainsKey("_class"))
        //        {
        //            htmlAttributes.Add("class", htmlAttributes["_class"].ToString());
        //            htmlAttributes.Remove("_class");
        //        }
        //        if (htmlAttributes.ContainsKey("@class"))
        //        {
        //            htmlAttributes.Add("class", htmlAttributes["@class"].ToString());
        //            htmlAttributes.Remove("@class");
        //        }
        //    }
        //}



        private static string SelectInternal(this HtmlHelper htmlHelper, string optionLabel, string name, IEnumerable<SelectListItem> selectList, bool allowMultiple, IDictionary<string, object> htmlAttributes)
        {
            ModelState state;
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(MvcResources.Common_NullOrEmpty, "name");
            }
            bool flag = false;
            if (selectList == null)
            {
                selectList = htmlHelper.GetSelectData(name);
                flag = true;
            }
            object obj2 = allowMultiple ? htmlHelper.GetModelStateValue(name, typeof(string[])) : htmlHelper.GetModelStateValue(name, typeof(string));
            if (!flag && (obj2 == null))
            {
                obj2 = htmlHelper.ViewData.Eval(name);
            }
            if (obj2 != null)
            {
                IEnumerable source = allowMultiple ? (obj2 as IEnumerable) : ((IEnumerable)new object[] { obj2 });
                HashSet<string> set = new HashSet<string>(source.Cast<object>().Select<object, string>(delegate(object value)
                {
                    return Convert.ToString(value, CultureInfo.CurrentCulture);
                }), StringComparer.OrdinalIgnoreCase);
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (SelectListItem item in selectList)
                {
                    item.Selected = (item.Value != null) ? set.Contains(item.Value) : set.Contains(item.Text);
                    list.Add(item);
                }
                selectList = list;
            }
            StringBuilder builder = new StringBuilder();
            if (optionLabel != null)
            {
                SelectListItem item2 = new SelectListItem();
                item2.Text = optionLabel;
                item2.Value = string.Empty;
                item2.Selected = false;
                builder.Append(ListItemToOption(item2));
            }
            foreach (SelectListItem item3 in selectList)
            {
                builder.Append(ListItemToOption(item3));
            }
            TagBuilder builder3 = new TagBuilder("select");
            builder3.InnerHtml = builder.ToString();
            TagBuilder builder2 = builder3;
            builder2.MergeAttributes<string, object>(htmlAttributes);
            builder2.MergeAttribute("name", name);
            builder2.GenerateId(name);
            if (allowMultiple)
            {
                builder2.MergeAttribute("multiple", "multiple");
            }
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out state) && (state.Errors.Count > 0))
            {
                builder2.AddCssClass(HtmlHelper.ValidationInputCssClassName);
            }
            return builder2.ToString(TagRenderMode.Normal);
        }






        private static IEnumerable<SelectListItem> GetSelectData(this HtmlHelper htmlHelper, string name)
        {
            object obj2 = null;
            if (htmlHelper.ViewData != null)
            {
                obj2 = htmlHelper.ViewData.Eval(name);
            }
            if (obj2 == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, MvcResources.HtmlHelper_MissingSelectData, new object[] { name, "IEnumerable<SelectListItem>" }));
            }
            IEnumerable<SelectListItem> enumerable = obj2 as IEnumerable<SelectListItem>;
            if (enumerable == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, MvcResources.HtmlHelper_WrongSelectDataType, new object[] { name, obj2.GetType().FullName, "IEnumerable<SelectListItem>" }));
            }
            return enumerable;
        }

        internal static string ListItemToOption(SelectListItem item)
        {
            TagBuilder builder2 = new TagBuilder("option");
            builder2.InnerHtml = HttpUtility.HtmlEncode(item.Text);
            TagBuilder builder = builder2;
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


        private static object GetModelStateValue(this HtmlHelper self, string key, Type destinationType)
        {
            ModelState state;
            if (self.ViewData.ModelState.TryGetValue(key, out state) && (state.Value != null))
            {
                return state.Value.ConvertTo(destinationType, null);
            }
            return null;
        }

        private static class MvcResources
        {
            private static CultureInfo resourceCulture;
            private static ResourceManager resourceMan;

            [EditorBrowsable(EditorBrowsableState.Advanced)]
            internal static CultureInfo Culture
            {
                get
                {
                    return resourceCulture;
                }
                set
                {
                    resourceCulture = value;
                }
            }

            [EditorBrowsable(EditorBrowsableState.Advanced)]
            internal static ResourceManager ResourceManager
            {
                get
                {
                    if (object.ReferenceEquals(resourceMan, null))
                    {
                        Assembly mvcAssembly = Assembly.Load("System.Web.Mvc, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                        ResourceManager manager = new ResourceManager("System.Web.Mvc.Resources.MvcResources", mvcAssembly);
                        resourceMan = manager;
                    }
                    return resourceMan;
                }
            }

            internal static string Common_NullOrEmpty
            {
                get
                {
                    return ResourceManager.GetString("Common_NullOrEmpty", resourceCulture);
                }
            }

            internal static string HtmlHelper_MissingSelectData
            {
                get
                {
                    return ResourceManager.GetString("HtmlHelper_MissingSelectData", resourceCulture);
                }
            }

            internal static string HtmlHelper_WrongSelectDataType
            {
                get
                {
                    return ResourceManager.GetString("HtmlHelper_WrongSelectDataType", resourceCulture);
                }
            }
        }

 

 





 




 

    }
}
