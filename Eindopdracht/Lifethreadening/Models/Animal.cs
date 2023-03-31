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

        private int CalculateHpToLoseDueToEnergy()
        {
            // If the animal has not much energy left, it will gradually lose hp
            return Math.Max(0, (ENERGY_WHERE_BELOW_STARTS_TO_LOSE_HP - Energy) / ENERGY_PER_SINGLE_HP_LOSS);
        }

        private int CalculateHpToLoseDueToNaturalAging()
        {
            // The older the animal is, the more hp it loses to natural causes and the more likely it is to die
            int ageDifferenceFromAverage = Age - Species.AverageAge;
            double ageDifferenceProportinalToMaxAge = ageDifferenceFromAverage / Species.MaxAge;
            return ageDifferenceFromAverage > 0 ? _random.Next(0, (int)(ageDifferenceProportinalToMaxAge * HP_LOSS_FOR_NATURAL_AGING_FACTOR)) : 0;
        }

        public override bool StillExistsPhysically()
        {
            return Hp > 0;
        }

        public override int GetNutritionalValue()
        {
            return (int)(Math.Sqrt(Statistics.Weight) * Math.Sqrt(Statistics.Size));
        }

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

        public void MoveAlong(Path path)
        {
            if(path != null && path.Length > 0)
            {
                Location.RemoveSimulationElement(this);
                Location = path.GetLocationAt(Math.Min(GetMaxMovementMagntitude(), path.Length));
                Location.AddSimulationElement(this);
            }
        }

        private int GetMaxMovementMagntitude()
        {
            return (int) Math.Ceiling(Statistics.Speed / 30.0);
        }

        public IDictionary<Location, Path> DetectSurroundings()
        {
            int range = (int)Math.Ceiling((double)Statistics.Detection / DETECTION_FACTOR);
            return DetectSurroundings(range);
        }

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

        public void AddHp(int hpToAdd)
        {
            lock(_hpLocker)
            {
                Hp += hpToAdd;
            }
        }

        public void AddEnergy(int energyToAdd)
        {
            lock(_energyLocker)
            {
                Energy += energyToAdd;
            }
        }
    }
}
