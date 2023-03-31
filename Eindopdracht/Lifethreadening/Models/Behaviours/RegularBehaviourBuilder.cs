using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to build behaviours 
    /// </summary>
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

        /// <summary>
        /// Creates a new beahviour builder
        /// </summary>
        public RegularBehaviourBuilder()
        {
            OpenComposite();
        }

        /// <summary>
        /// This function Adds a breed behaviour to the animal
        /// </summary>
        /// <param name="breedFactory">The factory to use to create the behaviour</param>
        /// <returns>itself</returns>
        public IBehaviourBuilder AddBreed(IBreedFactory breedFactory)
        {
            if(_hasAnimal)
            {
                //Add(new BreedBehaviour(_animal, breedFactory));
            }
            return this;
        }

        /// <summary>
        /// This function Adds a Eat behaviour to the animal
        /// </summary>
        /// <param name="breedFactory">The factory to use to create the behaviour</param>
        /// <returns>itself</returns>
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

        /// <summary>
        /// This function Adds a evade behaviour to the animal
        /// </summary>
        /// <param name="breedFactory">The factory to use to create the behaviour</param>
        /// <returns>itself</returns>
        public IBehaviourBuilder AddEvade()
        {
            if(_hasAnimal)
            {
                //Add(new EvadeBehaviour(_animal));
            }
            return this;
        }

        /// <summary>
        /// This function Adds a rest behaviour to the animal
        /// </summary>
        /// <param name="breedFactory">The factory to use to create the behaviour</param>
        /// <returns>itself</returns>
        public IBehaviourBuilder AddRest()
        {
            if(_hasAnimal)
            {
                Add(new RestBehaviour(_animal));
            }
            return this;
        }

        /// <summary>
        /// This function Adds a wander behaviour to the animal
        /// </summary>
        /// <param name="breedFactory">The factory to use to create the behaviour</param>
        /// <returns>itself</returns>
        public IBehaviourBuilder AddWander(bool isNative, double traumaChance = 0.5)
        {
            if(_hasAnimal)
            {
                Add(new WanderBehaviour(_animal));

                if(_random.NextDouble() < traumaChance)
                {
                    // Has trauma, so inclined to panic
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

        /// <summary>
        /// This function Closes thecomposit behaviour
        /// </summary>
        /// <returns>itself</returns>
        public IBehaviourBuilder CloseComposite()
        {
            if(_hasAnimal && _hasLeaves)
            {
                _tree.Pop();
            }
            return this;
        }

        /// <summary>
        /// This function Opens the composit behaviour
        /// </summary>
        /// <returns>itself</returns>
        public IBehaviourBuilder OpenComposite()
        {
            if(_hasAnimal)
            {
                _tree.Push(new CompositeBehaviour(_animal));
            }
            return this;
        }

        /// <summary>
        /// This function Returns the newest added behaviour
        /// </summary>
        /// <returns>The newest added behaviour</returns>
        public Behaviour GetBehaviour()
        {
            while(_hasLeaves)
            {
                _tree.Pop();
            }
            return _tree.Peek();
        }

        /// <summary>
        /// This function Adds a new behaviour to the composit
        /// </summary>
        /// <param name="behaviour">The behaviour to add</param>
        private void Add(Behaviour behaviour)
        {
            _currentBehaviour.Add(behaviour);
        }

        /// <summary>
        /// This function Creates a behaviour builder for a specific animal
        /// </summary>
        /// <param name="animal">The animal to creat it for</param>
        /// <returns>A Behaviour builder for the given animal</returns>
        public IBehaviourBuilder ForAnimal(Animal animal)
        {
            _animal = animal;
            _tree.Clear();
            OpenComposite();
            return this;
        }
    }
}
