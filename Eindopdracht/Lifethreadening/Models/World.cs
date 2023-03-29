using Lifethreadening.Base;
using Lifethreadening.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public abstract class World: Observable
    {
        private readonly IWeatherManager _weatherManager;
        private DateTime _currentDate;

        public DateTime StartTime { get; set; }
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

            DateTime dateTime = DateTime.Now;
            CurrentDate = dateTime;
            StartTime = dateTime;
        }

        public void Step()
        {
            foreach(SimulationElement simulationElement in SimulationElements)
            {
                simulationElement.Plan(CreateContext());
            }
            foreach(SimulationElement simulationElement in SimulationElements)
            {
                simulationElement.Act();
            }
            CurrentDate = CurrentDate.Add(StepSize);
        }

        public abstract void CreateWorld();

        public abstract IEnumerable<Location> GetLocations();

        private WorldContext CreateContext()
        {
            return new WorldContext(Weather, CurrentDate);
        }
    }
}
