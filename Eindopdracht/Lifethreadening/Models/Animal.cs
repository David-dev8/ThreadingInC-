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

        private int _hp = 0;
        private int _energy = 0;

        public int Hp
        {
            get
            {
                return _hp;
            }
            set
            {
                if(value <= MAX_HP)
                {
                    _hp = value;
                }
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
                if(value <= MAX_ENERGY)
                {
                    _energy = value;
                }
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
            Incentive incentive = Behaviour.guide();
            return incentive.Motivation > 0 ? incentive.Action : null;
        }

        public override bool StillExistsPhysically()
        {
            return Hp > 0;
        }
    }
}
