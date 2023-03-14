using Lifethreadening.Models.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Animal: SimulationElement
    {
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

        public Animal(Sex sex, Species species, Statistics statistics)
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
                if(incentive.Motivation > 0)
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

        public void MoveAlong(Path path)
        {

        }
    }
}
