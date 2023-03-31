using Lifethreadening.Base;
using Lifethreadening.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store data about the world
    /// </summary>
    public abstract class World: Observable
    {
        private readonly IWeatherManager _weatherManager;
        private DateTime _currentDate;

        public DateTime CurrentDate
        {
            get 
            { 
                return _currentDate; 
            }
            set 
            { 
                _currentDate = value;
                NotifyPropertyChanged();
            }
        }

        public TimeSpan StepSize { get; } = new TimeSpan(1, 0, 0, 0);
        public Ecosystem Ecosystem { get; set; }

        [JsonIgnore]
        public WorldContextService ContextService { get; set; }

        public Weather Weather
        {
            get
            {
                return _weatherManager.GetCurrent();
            }
        }

        public IEnumerable<Location> Locations
        {
            get
            {
                return GetLocations();
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

        /// <summary>
        /// Creates a new world
        /// </summary>
        /// <param name="ecosystem">The ecosystem for the new world</param>
        /// <param name="weatherManager">The weather manager for the new world</param>
        public World(Ecosystem ecosystem, IWeatherManager weatherManager)
        {
            Ecosystem = ecosystem;
            _weatherManager = weatherManager;
            CurrentDate = DateTime.Now;
            ContextService = new WorldContextService(this);
        }


        /// <summary>
        /// This function executes a game tick in this world
        /// </summary>
        public virtual void Step()
        {
            foreach(SimulationElement simulationElement in SimulationElements)
            {
                simulationElement.Plan();
            }
            foreach(SimulationElement simulationElement in SimulationElements)
            {
                simulationElement.Act();
            }
            foreach(Location location in Locations)
            {
                location.RemoveNonExistingSimulationElements();
            }
            _weatherManager.Update();
            CurrentDate = CurrentDate.Add(StepSize);
            OnPropertyChanged(nameof(Weather));
            OnPropertyChanged(nameof(Locations));
        }

        /// <summary>
        /// This function is used to set up the world
        /// </summary>
        public abstract void CreateWorld();

        /// <summary>
        /// This function is used to get all the locations in the world
        /// </summary>
        /// <returns>A list of all the mutations in the world</returns>
        public abstract IEnumerable<Location> GetLocations();
    }
}
