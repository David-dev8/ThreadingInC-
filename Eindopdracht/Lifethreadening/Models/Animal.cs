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

        public Animal(Location location, Sex sex, Species species) : base(location)
        {
            Hp = MAX_HP;
            Energy = MAX_ENERGY;
            Sex = sex;
            Species = species;
        }

        private void move()
        {
            ProcessIncentive(Behaviour.guide());
        }

        private void act()
        {
            ProcessIncentive(Behaviour.act());
        }

        private void ProcessIncentive(Incentive incentive)
        { 
            if(incentive.Motivation > 0)
            {
                incentive.execute();
            }
        }

        public override bool live()
        {
            move();
            act();
            return Hp > 0;
        }
    }
}
