using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class HerdBehaviour: Behaviour
    {
        private const int HP_INCREASE_BY_HERDING = 1;

        public HerdBehaviour(Animal animal) : base(animal)
        {
        }

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

        public override Incentive Guide()
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

        private int GetMotivation()
        {
            return Animal.Statistics.Intelligence;
        }
    }
}
