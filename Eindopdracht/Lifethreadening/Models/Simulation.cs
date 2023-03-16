using Lifethreadening.Base;
using Lifethreadening.Models.Behaviours;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Composition;

namespace Lifethreadening.Models
{
    public class Simulation: Observable
    {
        private const double INITIAL_SPAWN_CHANCE = 0.20;
        private Random _random = new Random();
        private Timer _stepTimer;
        private ISimulationElementFactory _elementFactory;
        private bool _stopped = true;
        private TimeSpan _simulationSpeed = new TimeSpan(1, 0, 0, 0);

        public string Name { get; set; }
        public int Score { get; set; }
        public World World { get; set; }
        public TimeSpan SimulationSpeed
        {
            get
            {
                return _simulationSpeed;
            }
            set
            {
                _simulationSpeed = value;
                SetTimer();
                NotifyPropertyChanged();
            }
        }


        public Simulation(string name, World world) 
        { 
            Name = name;
            World = world;
            _stepTimer = new Timer((_) => Step(), null, Timeout.Infinite, Timeout.Infinite);
            _elementFactory = new DatabaseSimulationElementFactory(new RegularBehaviourBuilder());
            Populate();
        }

        public void Step()
        {
            World.Step();
            OnPropertyChanged(nameof(World));
        }

        private bool IsGameOver()
        {
            return GetAnimals().Any();
        }

        private IEnumerable<Animal> GetAnimals()
        {
            return Enumerable.Empty<Animal>();
        }

        private void SetTimer()
        {
            
        }

        public void Start()
        {
            _stepTimer.Change(3000, 1000);
        }

        public void Stop()
        {
            _stepTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void Populate()
        {
            foreach(Location location in World.GetLocations())
            {
                if(_random.NextDouble() < INITIAL_SPAWN_CHANCE)
                {
                    SimulationElement element = _elementFactory.CreateRandomElement(World.Ecosystem);
                    if(element != null)
                    {
                        location.AddSimulationElement(element);
                    }
                }
            }
        }
    }
}
