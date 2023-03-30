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

        public World(Ecosystem ecosystem, IWeatherManager weatherManager)
        {
            Ecosystem = ecosystem;
            _weatherManager = weatherManager;
            CurrentDate = DateTime.Now;
            ContextService = new WorldContextService(this);
        }

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

        public abstract void CreateWorld();

        public abstract IEnumerable<Location> GetLocations();
    }
}
