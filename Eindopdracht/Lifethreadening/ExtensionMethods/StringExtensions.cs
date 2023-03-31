using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ExtensionMethods
{
    /// <summary>
    /// This class contains a set of extension methods for strings
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// This method converts a string to Titlecase
        /// </summary>
        /// <param name="value">The value to convert to Titlecase</param>
        /// <returns>The given value with Titlecase</returns>
        public static string ToTitleCase(this string value)
        {
            return string.Join(" ", value.Split(" ").Select(v => v.UcFirst()));
        }

        /// <summary>
        /// this method gives the first letter of a string uppercase
        /// </summary>
        /// <param name="value">The value to give uppercase</param>
        /// <returns>The given value with the first letter as uppercase</returns>
        public static string UcFirst(this string value)
        {
            return value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
        }
    }
}
