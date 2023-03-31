using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of breeding
    /// </summary>
    public class BreedBehaviour : Behaviour
    {
        private const double BREED_CHANCE = 0.05;
        private readonly IBreedFactory _breedFactory;
        private Random _random = new Random();

        /// <summary>
        /// Creates a new breed behaviour
        /// </summary>
        /// <param name="animal">The animal that recieves this behaviour</param>
        /// <param name="breedFactory">The factory to use to create ofspring</param>
        public BreedBehaviour(Animal animal, IBreedFactory breedFactory) : base(animal)
        {
            _breedFactory = breedFactory;
        }


        /// <summary>
        /// Generates a list of all posible breeding partners
        /// </summary>
        /// <param name="locations">The locations and paths to all other specieMembers</param>
        /// <returns>A list of posible partners to breed with</returns>
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

        /// <summary>
        /// This function executes the action 'Breeding' and creates ofspring
        /// </summary>
        /// <param name="partner">The partner to create ofspring with</param>
        private void Breed(Animal partner)
        {
            if(CanReach(partner.Location))
            {
                if(_random.NextDouble() < BREED_CHANCE)
                {
                    IEnumerable<Animal> children = _breedFactory.CreateAnimals(Animal, partner, Animal.ContextService);
                    Location currentLocation = Animal.Location;
                    foreach(Animal child in children)
                    {
                        child.Location = currentLocation;
                        Animal.Location.AddSimulationElement(child);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the ammount of motivation this animal has to breed
        /// </summary>
        /// <returns>The motivation level to breed as an int between 0 and 100</returns>
        private int GetMotivation()
        {
            return Animal.Statistics.Intelligence;
        }
    }
}
