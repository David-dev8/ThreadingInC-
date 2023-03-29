using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ExtensionMethods
{
    public static class NumbersExtensions
    {
        private static Random _random = new Random();

        public static int Deviate(this int number, int deviationRange)
        {
            return (int)Deviate((double)number, deviationRange);
        }

        public static double Deviate(this double number, double deviationRange)
        {
            return number + (_random.NextDouble() - 0.5) * deviationRange;
        }

        public static int AverageWith(this int number, int otherNumber)
        {
            return (number + otherNumber) / 2;
        }
    }
}
