using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ExtensionMethods
{
    /// <summary>
    /// This class contains a set of extension methods for objects of type Random 
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// This method generates a random Double value
        /// </summary>
        /// <param name="random">The random object to use</param>
        /// <param name="min">The lowest inclusive value the random value may have</param>
        /// <param name="max">The highest exclusive value the random value may have</param>
        /// <returns>A random double value between the 2 given values</returns>
        public static double NextDouble(this Random random, double min = 0, double max = 1)
        {
            return random.NextDouble() * (max - min) + min;
        }
    }
}
