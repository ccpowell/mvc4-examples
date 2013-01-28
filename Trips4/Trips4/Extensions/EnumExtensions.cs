using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace DRCOG.Web.Extensions
{
    public static class EnumExtensions
    {
        public static string ToPrettyLowerString(this Enum value)
        {
            return value.ToString().ToLower();
        }
    }
}
