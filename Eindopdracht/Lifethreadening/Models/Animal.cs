using Lifethreadening.Models.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.UI.Text;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store data about an animal
    /// </summary>
    public class Animal: SimulationElement
    {
        // Locking objects for updating energy and hp to prevent race conditions (for operations += and -= for example)
        private object _hpLocker = new object();
        private object _energyLocker = new object();

        private Random _random = new Random();

        private const int DEFAULT_PRIORITY = 1;
        // Locations per detection point
        private const int DETECTION_FACTOR = 10;
        private const int MAX_HP = 100;
        private const int MAX_ENERGY = 100;
        private const int HP_LOSS_FOR_NATURAL_AGING_FACTOR = 3;
        private const int ENERGY_LOSS_PER_STEP = 1;
        private const int ENERGY_WHERE_BELOW_STARTS_TO_LOSE_HP = 10;
        private const int ENERGY_PER_SINGLE_HP_LOSS = 3;

        private int _hp;
        private int _energy;
        private Species _species;

        [JsonInclude]
        public int Hp
        {
            get
            {
                return _hp;
            }
            private set
            {
                _hp = Math.Min(Math.Max(0, value), MAX_HP);
            }
        }
        [JsonInclude]
        public int Energy
        {
            get
            {
                return _energy;
            }
            private set
            {
                _energy = Math.Min(Math.Max(0, value), MAX_ENERGY);
            }
        }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age 
        { 
            get
            {
                DateTime currentDate = ContextService.GetContext().Date;
                int age = currentDate.Year - DateOfBirth.Year;
                // Correct for a leap year
                if(DateOfBirth.Date > currentDate.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }
        public Sex Sex { get; set; }
        public Species Species
        {
            get 
            { 
                return _species; 
            }
            set 
            { 
                _species = value;
                Image = _species.Image;
            }
        }
        public Statistics Statistics { get; set; }
        [JsonIgnore]
        public Behaviour Behaviour { get; set; }
        [JsonInclude]
        public IList<Mutation> Mutations { get; set; } = new List<Mutation>();

        /// <summary>
        /// Creates a new animal
        /// </summary>
        /// <param name="name">The name of the animal</param>
        /// <param name="sex">The sex of the animal</param>
        /// <param name="species">The species of the animal</param>
        /// <param name="statistics">The stats of the animal</param>
        /// <param name="contextService">The context service</param>
        public Animal(string name, Sex sex, Species species, Statistics statistics, WorldContextService contextService) : base(DEFAULT_PRIORITY, species.Image, contextService)
        {
            Name = name;
            Hp = MAX_HP;
            Energy = MAX_ENERGY;
            Sex = sex;
            Species = species;
            Statistics = statistics;
            if(contextService != null)
            {
                DateOfBirth = contextService.GetContext().Date;
            }
        }

        [JsonConstructor]
        public Animal(string name, Sex sex, Species species, Statistics statistics) : this(name, sex, species, statistics, null)
        {
        }

        /// <summary>
        /// This function returns the next action the animal shal undertake
        /// </summary>
        /// <returns></returns>
        protected override Action GetNextAction()
        {
            if(Behaviour != null)
            {
                Incentive incentive = Behaviour.Guide();
                if(incentive != null && incentive.Motivation > 0)
                {
                    return incentive.Action;
                }
            }
            return null;
        }

        /// <summary>
        /// This fucntion makes the animal act out their next action
        /// </summary>
        public override void Act()
        {
            base.Act();

            AddEnergy(-ENERGY_LOSS_PER_STEP);
            int hpToLose = CalculateHpToLoseDueToEnergy() + CalculateHpToLoseDueToNaturalAging();
            if(hpToLose > 0)
            {
                AddHp(-hpToLose);
            }
            
            if(Age > Species.MaxAge)
            {
                Hp = 0;
                Energy = 0;
            }
        }

        /// <summary>
        /// This function calculates how much HP would be lost every tick when the energy is 0
        /// </summary>
        /// <returns>The ammount of HP that should be removed</returns>
        private int CalculateHpToLoseDueToEnergy()
        {
            // If the animal has not much energy left, it will gradually lose hp
            return Math.Max(0, (ENERGY_WHERE_BELOW_STARTS_TO_LOSE_HP - Energy) / ENERGY_PER_SINGLE_HP_LOSS);
        }

        /// <summary>
        /// This function calculate how much HP would be lost as a result of natural ageing
        /// </summary>
        /// <returns>The amount of HP that should be removed</returns>
        private int CalculateHpToLoseDueToNaturalAging()
        {
            // The older the animal is, the more hp it loses to natural causes and the more likely it is to die
            int ageDifferenceFromAverage = Age - Species.AverageAge;
            double ageDifferenceProportinalToMaxAge = ageDifferenceFromAverage / Species.MaxAge;
            return ageDifferenceFromAverage > 0 ? _random.Next(0, (int)(ageDifferenceProportinalToMaxAge * HP_LOSS_FOR_NATURAL_AGING_FACTOR)) : 0;
        }

        /// <summary>
        /// This function checks if an animal is still alive
        /// </summary>
        /// <returns>A boolean value indicating wether the animal is still alive or not</returns>
        public override bool StillExistsPhysically()
        {
            return Hp > 0;
        }

        /// <summary>
        /// This function calculates the nutritional value of the animal
        /// </summary>
        /// <returns>The nutritional value of the animal</returns>
        public override int GetNutritionalValue()
        {
            return (int)(Math.Sqrt(Statistics.Weight) * Math.Sqrt(Statistics.Size));
        }

        /// <summary>
        /// This function deplets the nutritional value of the animal due do eating or deteriorating
        /// </summary>
        /// <returns>The amount the nutritional value should go down</returns>
        public override int DepleteNutritionalValue()
        {
            if(StillExistsPhysically())
            {
                // Cannot get nutrition from a living animal
                return 0;
            }
            int nutrition = GetNutritionalValue();
            Statistics.Size = 0;
            Statistics.Weight = 0;
            return nutrition;
        }

        /// <summary>
        /// This function lets the animal follow a path
        /// </summary>
        /// <param name="path">The path to follow</param>
        public void MoveAlong(Path path)
        {
            if(path != null && path.Length > 0)
            {
                Location.RemoveSimulationElement(this);
                Location = path.GetLocationAt(Math.Min(GetMaxMovementMagntitude(), path.Length));
                Location.AddSimulationElement(this);
            }
        }

        /// <summary>
        /// This function gets the amount of spaces this animal may move
        /// </summary>
        /// <returns></returns>
        private int GetMaxMovementMagntitude()
        {
            return (int) Math.Ceiling(Statistics.Speed / 30.0);
        }

        /// <summary>
        /// This function lets the animal look at its surroundings
        /// </summary>
        /// <returns>A list with all surrounding elements with paths to them</returns>
        public IDictionary<Location, Path> DetectSurroundings()
        {
            int range = (int)Math.Ceiling((double)Statistics.Detection / DETECTION_FACTOR);
            return DetectSurroundings(range);
        }


        /// <summary>
        /// This function lets the animal look at its surroundings
        /// </summary>
        /// <param name="range">The amount of tiles to look outward for</param>
        /// <returns>A list with all surrounding elements with paths to them</returns>
        public IDictionary<Location, Path> DetectSurroundings(int range)
        {
            IDictionary<Location, Path> possiblePaths = new Dictionary<Location, Path>() { { Location, new Path(Location) } };
            for(int i = 0; i < range; i++)
            {
                foreach(Path path in possiblePaths.Values.ToList())
                {
                    foreach(Location newAdjacentLocation in path.CurrentLocation.Neighbours)
                    {
                        if(!newAdjacentLocation.IsObstructed && !possiblePaths.ContainsKey(newAdjacentLocation))
                        {
                            possiblePaths.Add(newAdjacentLocation, new Path(newAdjacentLocation, path));
                        }
                    }
                }
            }
            return possiblePaths;
        }

        /// <summary>
        /// This function adds HP to an animal
        /// </summary>
        /// <param name="hpToAdd">The ammount of HP to be added</param>
        public void AddHp(int hpToAdd)
        {
            lock(_hpLocker)
            {
                Hp += hpToAdd;
            }
        }

        /// <summary>
        /// This function adds ennergy to an animal
        /// </summary>
        /// <param name="energyToAdd">The ammount of Ennergy to be added</param>
        public void AddEnergy(int energyToAdd)
        {
            lock(_energyLocker)
            {
                Energy += energyToAdd;
            }
        }
    }
}
