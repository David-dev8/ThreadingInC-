using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class WorldContextService
    {
        private readonly World _world;

        public WorldContextService(World world) 
        { 
            _world = world;
        }

        public WorldContext GetContext()
        {
            return new WorldContext(_world.Ecosystem, _world.Weather, _world.CurrentDate);
        }
    }
}
