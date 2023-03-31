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
    /// <summary>
    /// This class represents the core simulation that can be run
    /// </summary>
    public class Simulation: Observable
    {
        private Disaster _mostRecentDisaster; 
        private const double INITIAL_SPAWN_CHANCE = 0.10;
        private const double RUNNING_SPAWN_CHANCE = 0.85;
        private const double DISASTER_CHANCE = 0.40;
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
        private static readonly TimeSpan _stepInterval = new TimeSpan(1, 0, 0, 0);
        private static readonly TimeSpan _disasterInterval = new TimeSpan(120, 0, 0, 0);
        private static readonly TimeSpan _mutationInterval = new TimeSpan(5, 0, 0, 0);
        private static readonly TimeSpan _spawnInterval = new TimeSpan(12, 0, 0, 0);


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

        /// <summary>
        /// Create a simulation based on the given parameters
        /// </summary>
        /// <param name="id">The id of the simulation</param>
        /// <param name="score">The score of the simulation</param>
        /// <param name="startDate">The date at which the simulation started</param>
        /// <param name="amountOfDisasters">The amount of disasters that have occured</param>
        /// <param name="fileNameSaveSlot">The name of the file in which the world state of this simulation is saved</param>
        /// <param name="name">The name of the simulation</param>
        /// <param name="world">The world for the simulation</param>
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
            SetUpTimers();
        }

        /// <summary>
        /// Creates a simulation based on the given parameters
        /// This constructor will also create a world based on the given ecosystem
        /// </summary>
        /// <param name="ecosystem">The ecosystem to create the simulation for</param>
        /// <param name="fileName">The name of the file in which the world state of this simulation is saved</param>
        /// <param name="name">The name of the simulation</param>
        public Simulation(Ecosystem ecosystem, string fileName, string name): this(0, 0, DateTime.Now, 0, fileName, name, new GridWorld(ecosystem))
        {
        }

        /// <summary>
        /// This function takes one step in the simulation
        /// If there are no animals left, this will result in the simulation being game over
        /// </summary>
        private void Step()
        {
            if(!Stopped)
            {
                IEnumerable<Animal> animals = GetAnimals();
                PopulationManager.RegisterAnimals(animals, World.CurrentDate);
                MutationManager.RegisterMutations(animals); // TODO moet het registreren voor of na een stap?
                World.Step();

                // Check if the game has ended
                if(!animals.Any())
                {
                    End();
                    IsGameOver = true;
                }
            }
        }

        /// <summary>
        /// This function returns all animals that exist in the world of this simulation
        /// </summary>
        /// <returns>All animals in the world of this simulation</returns>
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

        /// <summary>
        /// This function places a new animal at a random location of the world, based on the spawn chance
        /// </summary>
        private void Spawn()
        {
            if(!Stopped)
            { 
                if(_random.NextDouble() < RUNNING_SPAWN_CHANCE)
                {
                    SimulationElement element = _elementFactory.CreateAnimal(World.ContextService);
                    Location randomLocation = World.GetLocations().GetRandom();
                    element.Location = randomLocation;
                    randomLocation.AddSimulationElement(element);
                }
            }
        }

        /// <summary>
        /// This function lets a random disaster strike, based on the disaster chance
        /// </summary>
        private void LetPotentialDisasterOccur()
        {
            if(!Stopped)
            {
                if(_random.NextDouble() < DISASTER_CHANCE)
                {
                    MostRecentDisaster = _disasterFactory.CreateDisaster(World.ContextService);
                    AmountOfDisasters++;
                    MostRecentDisaster.Strike(World.SimulationElements);
                }
            }
        }

        /// <summary>
        /// This function creates a mutation that affects a random animal in the simulation
        /// </summary>
        /// <returns>A task</returns>
        private async Task Mutate()
        {
            if(!Stopped)
            {
                Mutation mutation = await _mutationFactory.CreateMutation(World.CurrentDate);
                mutation.Affect(GetAnimals().GetRandom());
            }
        }

        /// <summary>
        /// This function saves the current simulation
        /// </summary>
        /// <returns>A task</returns>
        public async Task Save()
        {
            if(!IsGameOver)
            {
                Filename = await _worldStateWriter.Write(Name, World);
                Stop();
                await _simulationWriter.Write(this);
                Start();
            }
            else
            {
                await _simulationWriter.Write(this);
            }
        }

        /// <summary>
        /// This function sets up the timers by associating each timer with its corresponding function
        /// </summary>
        private void SetUpTimers()
        {
            _stepTimer = new Timer((_) => Step(), null, Timeout.Infinite, Timeout.Infinite);
            _spawnTimer = new Timer((_) => Spawn(), null, Timeout.Infinite, Timeout.Infinite);
            _disasterTimer = new Timer((_) => LetPotentialDisasterOccur(), null, Timeout.Infinite, Timeout.Infinite);
            _mutationTimer = new Timer(async (_) => await Mutate(), null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// This function sets up the simulation
        /// </summary>
        /// <param name="populate">Indicates whether to populate the world with elements, or to just use the existing world and its elements</param>
        /// <returns>A task</returns>
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

        /// <summary>
        /// This function starts the simulation
        /// </summary>
        public void Start()
        {
            if(!IsGameOver && Stopped)
            {
                Stopped = false;
                SetTimerIntervals();
            }
        }

        /// <summary>
        /// This function updates the interval of the timers
        /// </summary>
        private void SetTimerIntervals()
        {
            ChangeTimer(_stepTimer, _stepInterval);
            ChangeTimer(_disasterTimer, _disasterInterval);
            ChangeTimer(_spawnTimer, _spawnInterval);
            ChangeTimer(_mutationTimer, _mutationInterval);
        }

        /// <summary>
        /// This function updates a timer based on an interval, with respect to the current simulationspeed
        /// </summary>
        /// <param name="timer">The timer</param>
        /// <param name="interval">The interval to use for the timer</param>
        private void ChangeTimer(Timer timer, TimeSpan interval)
        {
            double milliseconds = 1000 * interval.Divide(_simulationSpeed);
            int roundedMilliSeconds = (int)milliseconds;
            timer.Change(roundedMilliSeconds, roundedMilliSeconds);
        }

        /// <summary>
        /// This function completely ends the simulation
        /// </summary>
        public void End()
        {
            Stopped = true;
            _stepTimer?.Dispose();
            _spawnTimer?.Dispose();
            _disasterTimer?.Dispose();
            _mutationTimer?.Dispose();
        }

        /// <summary>
        /// This function temporarily stops the simulation
        /// </summary>
        public void Stop()
        {
            Stopped = true;
            _stepTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _spawnTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _disasterTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _mutationTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// This function randomly populates the world of this simulation with elements
        /// </summary>
        private void Populate()
        {
            foreach(Location location in World.GetLocations())
            {
                if(_random.NextDouble() < INITIAL_SPAWN_CHANCE)
                {
                    SimulationElement element = _elementFactory.CreateRandomElement(World.ContextService);
                    if(element != null)
                    {
                        location.AddSimulationElement(element);
                        element.Location = location;
                    }
                }
            }
        }
    }
}
