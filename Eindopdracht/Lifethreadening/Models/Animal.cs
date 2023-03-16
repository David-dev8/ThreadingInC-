using Lifethreadening.Models.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Text;

namespace Lifethreadening.Models
{
    public class Animal: SimulationElement
    {
        private const int DEFAULT_PRIORITY = 1;
        // Locations per detection point
        private const int DETECTION_FACTOR = 10;
        private const int MAX_HP = 100;
        private const int MAX_ENERGY = 100;

        private int _hp;
        private int _energy;

        public int Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                _hp = value <= MAX_HP ? value : MAX_HP;
            }
        }
        public int Energy
        {
            get
            {
                return _energy;
            }
            set
            {
                _energy = value <= MAX_ENERGY ? value : MAX_ENERGY;
            }
        }
        public int Age { get; set; } = 0;
        public Sex Sex { get; set; }
        public Species Species { get; set; }
        public Statistics Statistics { get; set; }
        public Behaviour Behaviour { get; set; }
        public IList<Mutation> Mutations { get; set; } = new List<Mutation>();

        public Animal(Sex sex, Species species, Statistics statistics): base(DEFAULT_PRIORITY)
        {
            Hp = MAX_HP;
            Energy = MAX_ENERGY;
            Sex = sex;
            Species = species;
            Statistics = statistics;
        }

        protected override Action GetNextAction(WorldContext context)
        {
            if(Behaviour != null)
            {
                Incentive incentive = Behaviour.guide();
                if(incentive != null && incentive.Motivation > 0)
                {
                    return incentive.Action;
                }
            }
            return null;
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
            Location.RemoveSimulationElement(this);
            path.GetLocationAt(Math.Min(GetMaxMovementMagntitude(), path.Length)).AddSimulationElement(this);
        }

        private int GetMaxMovementMagntitude()
        {
            return 1;
        }

        public IDictionary<Location, Path> DetectSurroundings()
        {
            IDictionary<Location, Path> possiblePaths = new Dictionary<Location, Path>() { { Location, new Path(Location) } };
            int range = (int)Math.Ceiling((double)Statistics.Detection / DETECTION_FACTOR);
            for(int i = 0; i < range; i++)
            {
                foreach(Path path in possiblePaths.Values)
                {
                    foreach(Location newAdjacentLocation in path.CurrentLocation.Neighbours)
                    {
                        if(!possiblePaths.ContainsKey(newAdjacentLocation))
                        {
                            possiblePaths.Add(newAdjacentLocation, new Path(newAdjacentLocation, path));
                        }
                    }
                }
            }
            return possiblePaths;
        }
    }
}
