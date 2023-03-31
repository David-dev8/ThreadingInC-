using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public class BreedBehaviour : Behaviour
    {
        private const double BREED_CHANCE = 0.05;
        private readonly IBreedFactory _breedFactory;
        private Random _random = new Random();

        public BreedBehaviour(Animal animal, IBreedFactory breedFactory) : base(animal)
        {
            _breedFactory = breedFactory;
        }

        private IEnumerable<Animal> FindPossiblePartners(IDictionary<Location, Path> locations)
        {
            IList<Animal> possiblePartners = new List<Animal>();
            foreach(KeyValuePair<Location, Path> location in locations)
            {
                foreach(SimulationElement element in location.Key.SimulationElements)
                {
                    if(element is Animal animal)
                    {
                        if(animal.Species == Animal.Species && animal.Sex != Animal.Sex)
                        {
                            possiblePartners.Add(animal);
                        }
                    }
                }
            }
            return possiblePartners;
        }

        public override Incentive Guide()
        {
            if(Animal.Sex == Sex.FEMALE)
            {
                // Prevent both male and females from trying to reproduce at the same time
                return null;
            }
            IDictionary<Location, Path> locations = Animal.DetectSurroundings();
            Animal partner = FindPossiblePartners(locations).GetRandom();

            if(partner != null)
            {
                Path pathToFollow = locations[partner.Location];
                return new Incentive(() =>
                {
                    Animal.MoveAlong(pathToFollow);
                    Breed(partner);
                }, GetMotivation());
            }

            return null;
        }

        private void Breed(Animal partner)
        {
            if(CanReach(partner.Location))
            {
                if(_random.NextDouble() < BREED_CHANCE)
                {
                    IEnumerable<Animal> children = _breedFactory.CreateAnimals(Animal, partner, Animal.ContextService);
                    foreach(Animal child in children)
                    {
                        Animal.Location.AddSimulationElement(child);
                    }
                }
            }
        }

        private int GetMotivation()
        {
            return Animal.Statistics.Intelligence;
        }
    }
}
