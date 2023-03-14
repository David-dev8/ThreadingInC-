using Lifethreadening.DataAccess;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ExtensionMethods
{
    public static class CollectionExtensions
    {
        private static Random _random = new Random();

        public static T GetRandom<T>(this IEnumerable<T> elements)
        {
            return elements.ElementAt(_random.Next(elements.Count())); 
        }
    }
}
