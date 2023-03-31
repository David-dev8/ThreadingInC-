using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to provide context about he simulation world
    /// </summary>
    public class WorldContextService
    {
        private readonly World _world;

        /// <summary>
        /// Creates a world context service
        /// </summary>
        /// <param name="world">The world to create the context service for</param>
        public WorldContextService(World world) 
        { 
            _world = world;
        }

        /// <summary>
        /// This function gets the context of a simulation world
        /// </summary>
        /// <returns></returns>
        public WorldContext GetContext()
        {
            return new WorldContext(_world.Ecosystem, _world.Weather, _world.CurrentDate);
        }
    }
}
