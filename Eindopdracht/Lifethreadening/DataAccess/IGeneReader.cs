using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to retrieve data about genes from storage
    /// </summary>
    public interface IGeneReader
    {
        /// <summary>
        /// Retrieves a random Gene name
        /// </summary>
        /// <returns>A random gene</returns>
        Task<string> GetRandomGene();

        /// <summary>
        /// Retrieves a random protein name
        /// </summary>
        /// <param name="amount">The ammount of random protein names to retrieve</param>
        /// <returns>A list with random protein names</returns>
        Task<IEnumerable<string>> GetRandomProteins(int amount);
    }
}
