using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ExtensionMethods
{
    /// <summary>
    /// This class is used to contain a set of extension methods for numbers
    /// </summary>
    public static class NumbersExtensions
    {
        private static Random _random = new Random();

        /// <summary>
        /// This method gets a random number within a given range around a given integer
        /// </summary>
        /// <param name="number">The number to use as the mid point</param>
        /// <param name="deviationRange">The range the new number may deviate from the other given number</param>
        /// <returns>A new random number within the given range of the given integer</returns>
        public static int Deviate(this int number, int deviationRange)
        {
            return (int)Deviate((double)number, deviationRange);
        }

        /// <summary>
        /// This method gets a random double within a given range around a given integer
        /// </summary>
        /// <param name="number">The double to use as the mid point</param>
        /// <param name="deviationRange">The range the new double may deviate from the other given double</param>
        /// <returns>A new random double within the given range of the given integer</returns>
        public static double Deviate(this double number, double deviationRange)
        {
            return number + (_random.NextDouble() - 0.5) * deviationRange;
        }

        /// <summary>
        /// This method gives the avarage of 2 numbers
        /// </summary>
        /// <param name="number">The first value</param>
        /// <param name="otherNumber">The second value</param>
        /// <returns>The avarage of the 2 given numbers</returns>
        public static int AverageWith(this int number, int otherNumber)
        {
            return (number + otherNumber) / 2;
        }
    }
}
