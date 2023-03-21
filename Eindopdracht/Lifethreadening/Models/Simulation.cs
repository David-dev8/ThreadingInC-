using Lifethreadening.Base;
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
        private Timer _timer;
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
                SetTimer();
                NotifyPropertyChanged();
            }
        }


        public Simulation(string name, World world) 
        { 
            Name = name;
            World = world;
            _timer = new Timer((_) => Step(), null, Timeout.Infinite, Timeout.Infinite);
            PopulationManager= new PopulationAnalyzer();
            MutationManager= new MutationAnalyzer();
        }

        public void Step()
        {
            World.Step();

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
            _timer.Change(1000, 10000);
        }
    }
}
