using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to read data about species from storage
    /// </summary>
    public interface ISpeciesReader
    {
        /// <summary>
        /// Retrieves all the species from storage
        /// </summary>
        /// <returns>A list of all Species in storage</returns>
        IEnumerable<Species> ReadAll();

        /// <summary>
        /// Retrieve all species for a specific ecosysytem from storage
        /// </summary>
        /// <param name="ecosystemId">The ecosystem to get the species for</param>
        /// <returns>A list of all species asigned to the given ecosystem</returns>
        IEnumerable<Species> ReadByEcosystem(int ecosystemId);
    }
}
