using Lifethreadening.DataAccess;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.ExtensionMethods
{
    public static class CollectionExtensions
    {
        private static Random _random = new Random();

        public static T GetRandom<T>(this IEnumerable<T> elements)
        {
            int count = elements.Count();
            return count > 0 ? elements.ElementAt(_random.Next(elements.Count())) : default(T); 
        }

        public static IEnumerable<T> DequeueMultiple<T>(this Queue<T> stack, int amount)
        {
            IList<T> dequeuedElements = new List<T>();
            for(int i = 0; i < amount; i++)
            {
                dequeuedElements.Add(stack.Dequeue());
            }
            return dequeuedElements;
        }

        public static void EnqueueMultiple<T>(this Queue<T> stack, IEnumerable<T> elements)
        {
            foreach(T element in elements)
            {
                stack.Enqueue(element);
            }
        }
    }
}
