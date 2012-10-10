using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DRCOG.Domain.Helpers
{
    public static class WildcardExtensions
    {
        public static Regex ToWildcardRegex(this String str)
        {
            return WildcardToRegex(str);
        }

        /// <summary>

        /// Converts a wildcard to a regex.

        /// </summary>

        /// <param name="pattern">The wildcard pattern to convert.</param>

        /// <returns>A regex equivalent of the given wildcard.</returns>

        public static Regex WildcardToRegex(string pattern)
        {
            return new Regex("^" + Regex.Escape(pattern).
             Replace("\\*", ".*").
             Replace("%", ".*").
             Replace("\\?", ".") + "$");
        }
    }  
}
