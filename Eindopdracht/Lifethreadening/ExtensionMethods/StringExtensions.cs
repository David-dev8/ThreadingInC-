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
            return string.Join(" ", value.Split(" ").Select(v => v.Substring(0, 1).ToUpper() + v.Substring(1).ToLower()));
        }
    }
}
