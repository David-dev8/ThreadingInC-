using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class RegularBehaviourBuilder : IBehaviourBuilder
    {
        private Random _random = new Random();
        private Animal _animal;
        private Stack<CompositeBehaviour> _tree = new Stack<CompositeBehaviour>();
        private CompositeBehaviour _currentBehaviour
        {
            get
            {
                return _tree.Peek();
            }
        }
        private bool _hasLeaves
        {
            get
            {
                return _tree.Count > 1;
            }
        }
        private bool _hasAnimal
        {
            get
            {
                return _animal != null;
            }
        }

        public RegularBehaviourBuilder()
        {
            OpenComposite();
        }

        public IBehaviourBuilder AddBreed(IBreedFactory breedFactory)
        {
            if(_hasAnimal)
            {
                Add(new BreedBehaviour(_animal, breedFactory));
            }
            return this;
        }

        public IBehaviourBuilder AddEat(Diet diet)
        {
            if(_hasAnimal)
            {
                switch(diet)
                {
                    case Diet.HERBIVORE:
                        Add(new HerbivoreEatBehaviour(_animal));
                        break;
                    case Diet.CARNIVORE:
                        Add(new CarnivoreEatBehaviour(_animal));
                        break;
                    case Diet.OMNIVORE:
                        OpenComposite()
                            .AddEat(Diet.HERBIVORE)
                            .AddEat(Diet.CARNIVORE)
                            .CloseComposite();
                        break;
                }
            }
            return this;
        }

        public IBehaviourBuilder AddEvade()
        {
            if(_hasAnimal)
            {
                Add(new EvadeBehaviour(_animal));
            }
            return this;
        }

        public IBehaviourBuilder AddRest()
        {
            if(_hasAnimal)
            {
                Add(new RestBehaviour(_animal));
            }
            return this;
        }

        public IBehaviourBuilder AddWander(bool isNative, double traumaChance = 0.5)
        {
            if(_hasAnimal)
            {
                if(_random.NextDouble() < traumaChance)
                {
                    // Has trauma, so panic
                    Add(new PanicWanderBehaviour(_animal));
                } 
                else if(!isNative)
                {
                    Add(new CuriousWanderBehaviour(_animal));
                }
                else
                {
                    Add(new WanderBehaviour(_animal));
                }
            }
            return this;
        }

        public IBehaviourBuilder CloseComposite()
        {
            if(_hasAnimal && _hasLeaves)
            {
                _tree.Pop();
            }
            return this;
        }

        public IBehaviourBuilder OpenComposite()
        {
            if(_hasAnimal)
            {
                _tree.Push(new CompositeBehaviour(_animal));
            }
            return this;
        }

        public Behaviour GetBehaviour()
        {
            while(_hasLeaves)
            {
                _tree.Pop();
            }
            return _tree.Peek();
        }

        private void Add(Behaviour behaviour)
        {
            _currentBehaviour.Add(behaviour);
        }

        public IBehaviourBuilder ForAnimal(Animal animal)
        {
            _animal = animal;
            return this;
        }
    }
}
