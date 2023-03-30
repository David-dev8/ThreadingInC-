using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to retrieve data about simulations from storage
    /// </summary>
    public interface ISimulationReader
    {
        /// <summary>
        /// Retrieves all simulations from storage
        /// </summary>
        /// <returns>A list of all the simulations</returns>
        IEnumerable<Simulation> ReadAll();
    }
}
