using Lifethreadening.Base;
using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.Algorithmic;
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
    public class Simulation: Observable
    {
        private string gameName = "first";
        private Disaster _mostRecentDisaster; 
        private const double SPAWN_CHANCE = 0.10;
        private const double DISASTER_CHANCE = 1;
        private Random _random = new Random();
        private World _world;
        private ISimulationElementFactory _elementFactory;
        private IDisasterFactory _disasterFactory;
        private IMutationFactory _mutationFactory;
        private IWorldStateWriter _worldStateWriter;
        private ISimulationWriter _simulationWriter;
        private bool _stopped;

        private Timer _stepTimer;
        private Timer _spawnTimer;
        private Timer _disasterTimer;
        private Timer _mutationTimer;

        // For in game things
        private static readonly TimeSpan _stepInterval = new TimeSpan(1, 0, 0, 0); // TODO static conventions
        private static readonly TimeSpan _disasterInterval = new TimeSpan(50, 0, 0, 0); // TODO change interval en kans op disaster
        private static readonly TimeSpan _mutationInterval = new TimeSpan(5, 0, 0, 0);
        private static readonly TimeSpan _spawnInterval = new TimeSpan(20, 0, 0, 0);


        private TimeSpan _simulationSpeed = new TimeSpan(1, 0, 0, 0);

        public int Id { get; set; }
        public string Filename { get; set; }
        public int AmountOfDisasters { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public DateTime StartDate { get; set; }
        public bool Stopped 
        {
            get
            {
                return _stopped;
            }
            set
            {
                _stopped = value;
                NotifyPropertyChanged();
            }
        }
        public World World
        {
            get
            {
                return _world;
            }
            set
            {
                _world = value;
                NotifyPropertyChanged();
            }
        }
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

        public Simulation(int id, int score, DateTime startDate, int amountOfDisasters, string fileNameSaveSlot, string name, World world)
        { 
            Name = name;
            World = world;
            Id = id;
            Stopped = true;
            Score = score;
            StartDate = startDate;
            AmountOfDisasters = amountOfDisasters;
            Filename = fileNameSaveSlot;

            PopulationManager = new PopulationAnalyzer();
            MutationManager = new MutationAnalyzer();

            _disasterFactory = new RegularDisasterFactory();
            _mutationFactory = new RandomMutationFactory();
            _worldStateWriter = new JSONWorldStateWriter();
            _simulationWriter = new DatabaseSimulationWriter();
            // TODO exceptions tijdens timer
            SetUpTimers();
        }

        public Simulation(Ecosystem ecosystem): this(0, 0, DateTime.Now, 0, "file", "name", new GridWorld(ecosystem))
        {
        }

        private TimerCallback Run(Action action)
        {
            // TODO werkt niet met async
            return (state) =>
            {
                if(!Stopped)
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
                End();
                IsGameOver = true;
            }

            IEnumerable<Animal> animals = GetAllAnimals(World.SimulationElements);
            PopulationManager.RegisterAnimals(animals, World.CurrentDate);
            MutationManager.RegisterMutations(animals); // TODO moet het registreren voor of na een stap?
        }

        private IEnumerable<Animal> GetAllAnimals(IEnumerable<SimulationElement> elements)
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
            if(_random.NextDouble() < SPAWN_CHANCE)
            {
                SimulationElement element = _elementFactory.CreateRandomElement(World.ContextService);
                World.GetLocations().GetRandom().AddSimulationElement(element);
            }
        }

        private void LetPotentialDisasterOccur()
        {
            if(_random.NextDouble() < DISASTER_CHANCE)
            {
                MostRecentDisaster = _disasterFactory.CreateDisaster(World.ContextService);
                AmountOfDisasters++;
                MostRecentDisaster.Strike(World.SimulationElements);
            }
        }

        private async void Mutate()
        {
            Mutation mutation = await _mutationFactory.CreateMutation(World.CurrentDate);
            mutation.Affect(GetAnimals().GetRandom());
            // TODO async (void)
        }

        public async Task Save()
        {
            // TODO prevent two threads from doing the save
            Filename = await _worldStateWriter.Write(gameName, World);
            await _simulationWriter.Write(this);
        }

        private void SetUpTimers()
        {
            _stepTimer = new Timer(Run(Step), null, Timeout.Infinite, Timeout.Infinite);
            _spawnTimer = new Timer((_) => Spawn(), null, Timeout.Infinite, Timeout.Infinite);
            _disasterTimer = new Timer(Run(LetPotentialDisasterOccur), null, Timeout.Infinite, Timeout.Infinite);
            _mutationTimer = new Timer((_) => Mutate(), null, Timeout.Infinite, Timeout.Infinite);
        }

        public async Task Setup(bool populate = true)
        {
            var nameReader = new APINameReader();
            await nameReader.Initialize();
            _elementFactory = new DatabaseSimulationElementFactory(new RegularBehaviourBuilder(), nameReader);
            if(populate)
            {
                Populate();
            }
        }

        public void Start()
        {
            if(!IsGameOver && Stopped)
            {
                Stopped = false;
                SetTimerIntervals();
            }
        }

        private void SetTimerIntervals()
        {
            //_spawnTimer.Change(2000, secondsForOneDay);
            //_spawnTimer.Change(2000, secondsForOneDay);
            // TODO
            
            ChangeTimer(_stepTimer, 1000 * _stepInterval.Divide(_simulationSpeed)); // TODO Changing while 
            ChangeTimer(_disasterTimer, 1000 * _disasterInterval.Divide(_simulationSpeed)); // TODO take into account already running timer
            //ChangeTimer(_disasterTimer, 1000 * _disasterInterval.Divide(_simulationSpeed));
            
            // TODO Does timer run even when changing its period?
        }

        private void ChangeTimer(Timer timer, double milliseconds)
        {
            int roundedMilliSeconds = (int)milliseconds;
            timer.Change(roundedMilliSeconds, roundedMilliSeconds);
        }

        public void End()
        {
            Stopped = true;
            _stepTimer?.Dispose();
            _spawnTimer?.Dispose();
            _disasterTimer?.Dispose();
            _mutationTimer?.Dispose();
        }

        public void Stop()
        {
            Stopped = true;
            _stepTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _spawnTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _disasterTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _mutationTimer.Change(Timeout.Infinite, Timeout.Infinite);
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
                if(_random.NextDouble() < SPAWN_CHANCE)
                {
                    SimulationElement element = _elementFactory.CreateRandomElement(World.ContextService);
                    if(element != null)
                    {
                        location.AddSimulationElement(element);
                        element.Location = location;
                    } // TODO Because this call is not awaited
                }
            }
        }
    }
}
