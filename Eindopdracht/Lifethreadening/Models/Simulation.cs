using Lifethreadening.Base;
using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.API;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.DataAccess.JSON;
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
using Windows.Security.Cryptography.Core;
using Windows.UI.Composition;

namespace Lifethreadening.Models
{
    // TODO dispose timers
    // TODO lock mutations
    public class Simulation: Observable, IDisposable
    {
        private string gameName = "first";

        private int _amountOfDisasters = 0;
        private Disaster _mostRecentDisaster; // TODO make it bindable
        private const double INITIAL_SPAWN_CHANCE = 0.10;
        private const double DISASTER_CHANCE = 1;
        private Random _random = new Random();
        private ISimulationElementFactory _elementFactory;
        private WorldContextService _worldContextService;
        private IDisasterFactory _disasterFactory;
        private IMutationFactory _mutationFactory;
        private IWorldStateWriter _worldStateWriter;
        private ISimulationWriter _simulationWriter;
        private bool _stopped = true;

        private Timer _stepTimer;
        private Timer _spawnTimer;
        private Timer _disasterTimer;
        private Timer _mutationTimer;
        private Timer _saveTimer;

        private static readonly TimeSpan _saveInterval = new TimeSpan(0, 0, 10);

        // For in game things
        private static readonly TimeSpan _distasterInterval = new TimeSpan(50, 0, 0, 0); // TODO static conventions
        private static readonly TimeSpan _stepInterval = new TimeSpan(1, 0, 0, 0); // TODO static conventions


        private TimeSpan _simulationSpeed = new TimeSpan(1, 0, 0, 0);

        public string Name { get; set; }
        public int Score { get; set; }
        public World World { get; set; }
        public PopulationAnalyzer PopulationManager { get; set; }
        public MutationAnalyzer MutationManager { get; set; }
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

        public Disaster MostRecentDisaster
        {
            get
            {
                return _mostRecentDisaster;
            }
            set
            {
                _mostRecentDisaster = value;
                NotifyPropertyChanged();
            }
        }

        public Simulation(string name, World world) 
        { 
            Name = name;
            World = world;

            PopulationManager = new PopulationAnalyzer();
            MutationManager = new MutationAnalyzer();

            _disasterFactory = new RegularDisasterFactory();
            _mutationFactory = new RandomMutationFactory();
            _worldContextService = new WorldContextService(World);
            _worldStateWriter = new JSONWorldStateWriter();
            _simulationWriter = new DatabaseSimulationWriter();
            SetUpTimers();
        }

        private TimerCallback Run(Action action)
        {
            // TODO werkt niet met async
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
            World.Step();

            if(!GetAnimals().Any())
            {
                Stop();
                IsGameOver = true;
            }

            IEnumerable<Animal> animals = getAllAnimals(World.SimulationElements);
            PopulationManager.RegisterAnimals(animals, World.Date);
            MutationManager.RegisterMutations(animals); // TODO moet het registreren voor of na een stap?
        }

        private IEnumerable<Animal> getAllAnimals(IEnumerable<SimulationElement> elements)
        {
            IList<Animal> animals = new List<Animal>();
            foreach(SimulationElement element in elements)
            {
                if(element is Animal)
                {
                    animals.Add((Animal) element);
                }
            }
            return animals;
        }

        private void Spawn()
        {
            SimulationElement element = _elementFactory.CreateRandomElement(_worldContextService);
            World.GetLocations().GetRandom().AddSimulationElement(element);
        }

        private void LetPotentialDisasterOccur()
        {
            if(_random.NextDouble() < DISASTER_CHANCE)
            {
                MostRecentDisaster = _disasterFactory.CreateDisaster(_worldContextService);
                _amountOfDisasters++;
                //disaster.Strike(World.SimulationElements);
            }
        }

        private async void Mutate()
        {
            Mutation mutation = await _mutationFactory.CreateMutation();
            mutation.Affect(GetAnimals().GetRandom()); // TODO lock the mutations?
            // TODO async
        }

        private async Task Save()
        {
            string location = await _worldStateWriter.Write(gameName, World);
            await _simulationWriter.Write(location, this);
        }

        private void SetUpTimers()
        {
            _stepTimer = new Timer(Run(Step), null, Timeout.Infinite, Timeout.Infinite);
            _spawnTimer = new Timer((_) => Spawn(), null, Timeout.Infinite, Timeout.Infinite);
            _disasterTimer = new Timer(Run(LetPotentialDisasterOccur), null, Timeout.Infinite, Timeout.Infinite);
            _mutationTimer = new Timer((_) => Mutate(), null, Timeout.Infinite, Timeout.Infinite);
            _saveTimer = new Timer((_) => Save(), null, _saveInterval, _saveInterval);
        }

        public async Task Start()
        {
            var nameReader = new APINameReader();
            await nameReader.Initialize();
            _elementFactory = new DatabaseSimulationElementFactory(new RegularBehaviourBuilder(), nameReader);
            Populate();
            _stopped = false;
            SetTimerIntervals(); // TODO resume?
        }

        private void SetTimerIntervals()
        {
            //_spawnTimer.Change(2000, secondsForOneDay);
            //_spawnTimer.Change(2000, secondsForOneDay);
            // TODO
            
            ChangeTimer(_stepTimer, 1000 * _stepInterval.Divide(_simulationSpeed)); // TODO Changing while 
            ChangeTimer(_disasterTimer, 1000 * _distasterInterval.Divide(_simulationSpeed)); // TODO take into account already running timer
            
            // TODO Does timer run even when changing its period?
        }

        private void ChangeTimer(Timer timer, double milliseconds)
        {
            int roundedMilliSeconds = (int)milliseconds;
            timer.Change(roundedMilliSeconds, roundedMilliSeconds);
        }

        public void Stop()
        {
            _stopped = true;
            _stepTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _spawnTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _disasterTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _mutationTimer.Change(Timeout.Infinite, Timeout.Infinite);
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
                    } // TODO Because this call is not awaited
                }
            }
        }

        public void Dispose()
        {
            _stepTimer.Dispose();
        }
    }
}
