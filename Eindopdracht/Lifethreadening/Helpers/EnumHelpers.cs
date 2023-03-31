using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Helpers
{
    /// <summary>
    /// This class contains a set of extension methods for Enumerable objects
    /// </summary>
    public class EnumHelpers
    {
        private static Random _random = new Random();

        /// <summary>
        /// Returns a random ENUM value from the posible values
        /// </summary>
        /// <typeparam name="T">The ENUM type to get a random value for</typeparam>
        /// <returns>A random value for the given ENUM type</returns>
        public static T GetRandom<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(_random.Next(values.Length));
        }
    }
}
