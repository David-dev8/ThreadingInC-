using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of traveling in herds
    /// </summary>
    public class HerdBehaviour: Behaviour
    {
        private const int HP_INCREASE_BY_HERDING = 1;

        /// <summary>
        /// Creates a bew herd behaviour
        /// </summary>
        /// <param name="animal">The animal to create this behaviour for</param>
        public HerdBehaviour(Animal animal) : base(animal)
        {
        }

        /// <summary>
        /// This method generates a collection of all specie members in the game
        /// </summary>
        /// <param name="locations">All locations in the game</param>
        /// <returns>A list of all animals of the smae species</returns>
        private IEnumerable<Animal> FindAnimalsOfSameSpecies(IDictionary<Location, Path> locations)
        {
            IList<Animal> animalsOfSameSpecies = new List<Animal>();
            foreach(KeyValuePair<Location, Path> location in locations)
            {
                foreach(SimulationElement element in location.Key.SimulationElements)
                {
                    if(element is Animal animal)
                    {
                        if(animal.Species == Animal.Species)
                        {
                            animalsOfSameSpecies.Add(animal);
                        }
                    }
                }
            }
            return animalsOfSameSpecies;
        }

        public override Incentive guide()
        {
            IDictionary<Location, Path> locations = Animal.DetectSurroundings(); // TODO cache?
            Animal animalOfSameSpecies = FindAnimalsOfSameSpecies(locations).GetRandom();
            
            if(animalOfSameSpecies != null)
            {
                Path pathToFollow = locations[animalOfSameSpecies.Location];
                return new Incentive(() =>
                {
                    Animal.MoveAlong(pathToFollow);
                    // Teaming up restores some hp
                    Animal.AddHp(HP_INCREASE_BY_HERDING);
                }, GetMotivation());
            }

            return null;
        }

        /// <summary>
        /// Returns the motivation the animal has to travel in herds
        /// </summary>
        /// <returns>The ammount of motivation the animal has to travel in herds</returns>
        private int GetMotivation()
        {
            return Animal.Statistics.Intelligence;
        }
    }
}
