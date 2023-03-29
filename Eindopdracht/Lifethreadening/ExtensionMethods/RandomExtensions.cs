using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ExtensionMethods
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random random, double min = 0, double max = 1)
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
}
