using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models.Behaviours
{
    public abstract class Behaviour
    {
        // Locations per detection point
        private const int DETECTION_FACTOR = 10;

        public Animal Animal { get; set; }

        public Behaviour(Animal animal) 
        { 
            Animal = animal;
        }

        public abstract Incentive guide();

        public IDictionary<Location, Path> DetectSurroundings()
        {
            IDictionary<Location, Path> possiblePaths = new Dictionary<Location, Path>() { { Animal.Location, new Path(Animal.Location) } };
            int range = (int)Math.Ceiling((double)Animal.Statistics.Detection / DETECTION_FACTOR);
            for(int i = 0; i < range; i++)
            {
                foreach(Path path in possiblePaths.Values)
                {
                    foreach(Location newAdjacentLocation in path.CurrentLocation.Neighbours)
                    {
                        if(!possiblePaths.ContainsKey(newAdjacentLocation))
                        {
                            possiblePaths.Add(newAdjacentLocation, new Path(newAdjacentLocation, path));
                        }
                    }
                }
            }
            return possiblePaths;
        }

        //protected IEnumerable<Location> DetectSurroundingLocations()
        //{

        //    return Animal.Location.GetAdjacent((int)Math.Ceiling(
        //        (double)Animal.Statistics.Detection / DETECTION_FACTOR));

        //    // For each neighbouring entity, or location, we should assign a factor and pick the highest
        //    // For example, the closer, the higher the factor
        //}

        //protected IEnumerable<SimulationElement> DetectSurroundingElements()
        //{
        //    ISet<SimulationElement> elements = new HashSet<SimulationElement>();
        //    foreach(Location location in DetectSurroundingLocations())
        //    {

        //    }
        //}


        protected bool AreLocationsCloseEnough(Location first, Location second)
        {
            return first == second || first.Neighbours.Contains(second);
        }

        public void MoveAlong(Path path)
        {
            Animal.Location = path.GetLocationAt(GetMovementMagntitude());
        }

        private int GetMovementMagntitude()
        {

        }
    }
}
