using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ExtensionMethods
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string value)
        {
            return string.Join(" ", value.Split(" ").Select(v => v.UcFirst()));
        }

        public static string UcFirst(this string value)
        {
            return value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower();
        }
    }
}
