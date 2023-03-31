using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    /// <summary>
    /// This class is used to contain behavior characteristics partaining to the behaviour of running away from danger
    /// </summary>
    public class RunAwayBehaviour : Behaviour
    {
        /// <summary>
        /// Creates a new running away behaviour
        /// </summary>
        /// <param name="animal">The animal to create the behaviour for</param>
        public RunAwayBehaviour(Animal animal) : base(animal)
        {
        }

        /// <summary>
        /// This method find all potential sources of danger in the world
        /// </summary>
        /// <param name="locations">All locations in the world</param>
        /// <returns>A collection of danger is the world</returns>
        private IEnumerable<Animal> FindThreats(IDictionary<Location, Path> locations)
        {
            IList<Animal> threats = new List<Animal>();
            foreach(KeyValuePair<Location, Path> location in locations)
            {
                foreach(SimulationElement element in location.Key.SimulationElements)
                {
                    if(element is Animal animal)
                    {
                        // The Animal evaluates whether the other animal forms a threat by comparing its aggresion against its own defence capabilities
                        if(animal.Statistics.Aggresion > animal.Statistics.SelfDefence)
                        {
                            threats.Add(animal);
                        }
                    }
                }
            }
            return threats;
        }

        public override Incentive guide()
        {
            IDictionary<Location, Path> locations = Animal.DetectSurroundings(); // TODO cache?
            IEnumerable<Animal> threats = FindThreats(locations);

            if(threats.Any())
            {
                // There is a threat
                // Run along the farthest path
                Path longestPath = locations.Values.OrderByDescending(path => path.Length).First();
                return new Incentive(() =>
                {
                    Animal.MoveAlong(longestPath);
                }, GetMotivation());
            }

            return null;
        }

        /// <summary>
        /// This method gets the motivation the animal has to run away
        /// </summary>
        /// <returns>The ammount of motivation the animal has to run away</returns>
        private int GetMotivation()
        {
            return ((100 - Animal.Statistics.SelfDefence) + (100 - Animal.Hp));
        }
    }
}
