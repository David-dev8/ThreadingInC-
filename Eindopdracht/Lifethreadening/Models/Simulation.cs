using Lifethreadening.Base;
using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models.Behaviours;
using Lifethreadening.Models.Disasters;
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
    // TODO dispose timers
    public class Simulation: Observable, IDisposable
    {
        private const int _amountOfDisasters = 0;
        private Disaster _currentDisaster; // TODO make it bindable
        private const double INITIAL_SPAWN_CHANCE = 0.20;
        private const double DISASTER_CHANCE = 0.05;
        private Random _random = new Random();
        private ISimulationElementFactory _elementFactory;
        private WorldContextService _worldContextService;
        private IDisasterFactory _disasterFactory;
        private bool _stopped = true;

        private Timer _stepTimer;
        private Timer _spawnTimer;
        private Timer _disasterTimer;
        private Timer _saveTimer;

        private static readonly TimeSpan _saveInterval = new TimeSpan(0, 5, 0);

        // One step
        private int secondsForOneDay = 7000;
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
                SetTimerIntervals();
                NotifyPropertyChanged();
            }
        }
        private bool _isGameOver;

        public bool IsGameOver
        {
            get 
            { 
                return _isGameOver; 
            }
            set 
            { 
                _isGameOver = value;
                NotifyPropertyChanged();
            }
        }



        public Simulation(string name, World world) 
        { 
            Name = name;
            World = world;
            _elementFactory = new DatabaseSimulationElementFactory(new RegularBehaviourBuilder());
            _disasterFactory = new RegularDisasterFactory();
            _worldContextService = new WorldContextService(World);
            SetUpTimers();
            Populate();
        }

        private TimerCallback Run(Action action)
        {
            return (state) =>
            {
                if(!_stopped)
                {
                    action();
                }
            };
        }

        private void Step()
        {
            if(!_stopped)
            {
                World.Step();
                if(!GetAnimals().Any())
                {
                    Stop();
                    IsGameOver = true;
                }
            }
        }

        private void Spawn()
        {
            World.GetLocations().GetRandom().AddSimulationElement(_elementFactory.CreateRandomElement(_worldContextService));
        }

        private void LetPotentialDisasterOccur()
        {
            if(_random.NextDouble() < DISASTER_CHANCE)
            {
                _disasterFactory.GetDisaster().
            }
        }

        private void Save()
        {

        }

        private void SetUpTimers()
        {
            _stepTimer = new Timer(Run(Step), null, Timeout.Infinite, Timeout.Infinite);
            _spawnTimer = new Timer(Run(Spawn), null, Timeout.Infinite, Timeout.Infinite);
            _disasterTimer = new Timer(Run(LetPotentialDisasterOccur), null, Timeout.Infinite, Timeout.Infinite);
            _saveTimer = new Timer(Run(Save), null, _saveInterval, _saveInterval);
        }

        public void Start()
        {
            _stopped = false;
            SetTimerIntervals();
        }

        private void SetTimerIntervals()
        {
            //_spawnTimer.Change(2000, secondsForOneDay);
            //_spawnTimer.Change(2000, secondsForOneDay);
            // TODO
            int seconds = secondsForOneDay / _simulationSpeed.Days;
            _stepTimer.Change(seconds, seconds); // TODO Changing while 
        }

        public void Stop()
        {
            _stopped = true;
            _stepTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _spawnTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _disasterTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _saveTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private IEnumerable<Animal> GetAnimals()
        {
            ISet<Animal> animals = new HashSet<Animal>();
            foreach(SimulationElement element in World.SimulationElements)
            {
                if(element is Animal animal)
                {
                    animals.Add(animal);
                }
            }
            return animals;
        }

        private void Populate()
        {
            foreach(Location location in World.GetLocations())
            {
                if(_random.NextDouble() < INITIAL_SPAWN_CHANCE)
                {
                    SimulationElement element = _elementFactory.CreateRandomElement(_worldContextService);
                    if(element != null)
                    {
                        location.AddSimulationElement(element);
                        element.Location = location;
                    }
                }
            }
        }

        public void Dispose()
        {
            _stepTimer.Dispose();
        }
    }
}
