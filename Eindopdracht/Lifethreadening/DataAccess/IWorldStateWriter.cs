using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess
{
    /// <summary>
    /// This class is used to write things about the world to a datastore
    /// </summary>
    public interface IWorldStateWriter
    {
        /// <summary>
        /// Saves a world state
        /// </summary>
        /// <param name="gameName">The name of the world</param>
        /// <param name="world">The world object to save</param>
        /// <returns>A task containing the executing save function</returns>
        Task<string> Write(string gameName, World world);

        /// <summary>
        /// Deletes a stored world from storage
        /// </summary>
        /// <param name="gameName">The name of the game to delete</param>
        /// <returns>A task containing the executing Delete function</returns>
        Task Delete(string gameName);
    }
}
