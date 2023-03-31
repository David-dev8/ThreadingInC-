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

    /// <summary>
    /// This class contains a set of extension methods for objects of type IEnumerable
    /// </summary>
    public static class CollectionExtensions
    {
        private static Random _random = new Random();
        
        /// <summary>
        /// This method returns a random element from the collection
        /// </summary>
        /// <typeparam name="T">The type the retrieved element will be</typeparam>
        /// <param name="elements">The list of elements to get the random element from</param>
        /// <returns>A random element from the given array</returns>
        public static T GetRandom<T>(this IEnumerable<T> elements)
        {
            int count = elements.Count();
            return count > 0 ? elements.ElementAt(_random.Next(elements.Count())) : default(T); 
        }

        /// <summary>
        /// Deques multiple elements at once
        /// </summary>
        /// <typeparam name="T">The type of elements in the queu</typeparam>
        /// <param name="queu">The queu to dequeu from</param>
        /// <param name="amount">The ammount of element to deque</param>
        /// <returns>All the elements that have been dequed</returns>
        public static IEnumerable<T> DequeueMultiple<T>(this Queue<T> queu, int amount)
        {
            IList<T> dequeuedElements = new List<T>();
            for(int i = 0; i < amount; i++)
            {
                dequeuedElements.Add(queu.Dequeue());
            }
            return dequeuedElements;
        }


        /// <summary>
        /// Appends multiple elements to the back of a queu at once
        /// </summary>
        /// <typeparam name="T">The type of elements in the queu</typeparam>
        /// <param name="queu">The queu to append the items to</param>
        /// <param name="elements">A collection with all the elements that need to be appendend</param>
        public static void EnqueueMultiple<T>(this Queue<T> queu, IEnumerable<T> elements)
        {
            foreach(T element in elements)
            {
                queu.Enqueue(element);
            }
        }

        /// <summary>
        /// Fils a collection with a repeating sequence until a the collection reaches a given size
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="elements">The sequence of elements you want to implement</param>
        /// <param name="length">The amount of items this collection shoul be filled up to</param>
        /// <returns>A collection with a length based on the given length containing a repeating sequence of entries</returns>
        public static IEnumerable<T> RepeatUntilLength<T>(this IEnumerable<T> elements, int length)
        {
            return Enumerable.Repeat(new List<T>(elements), (int)Math.Ceiling((double)length / elements.Count())).SelectMany(element => element);
        }
    }
}
