using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to retrieve data about the world state from storage
    /// </summary>
    public interface IWorldStateReader
    {
        /// <summary>
        /// Retrieves a worldstate from storage
        /// </summary>
        /// <param name="gameName">The name of the world to retrieve</param>
        /// <returns>A task containing the executing get function</returns>
        Task<World> Read(string gameName);
    }
}
