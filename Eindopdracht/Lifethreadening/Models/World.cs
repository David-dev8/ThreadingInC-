using Lifethreadening.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public abstract class World
    {
        private readonly IWeatherManager _weatherManager;

        public DateTime Date { get; set; } = DateTime.Now.Date;
        public Ecosystem Ecosystem { get; set; }
        public Weather Weather
        {
            get
            {
                return _weatherManager.GetCurrent();
            }
        }

        public IEnumerable<SimulationElement> SimulationElements
        {
            get
            {
                var simulationElements = new List<SimulationElement>();
                foreach(Location location in GetLocations())
                {
                    simulationElements.AddRange(location.SimulationElements);
                }
                return simulationElements;
            }
        }

        public World(Ecosystem ecosystem, IWeatherManager weatherManager)
        {
            Ecosystem = ecosystem;
            _weatherManager = weatherManager;
            createWorld();
        }

        public abstract void createWorld();

        public abstract IEnumerable<Location> GetLocations();
    }
}
