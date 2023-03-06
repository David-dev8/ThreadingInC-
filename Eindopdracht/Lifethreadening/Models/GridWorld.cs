using Lifethreadening.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class GridWorld : World
    {
        private int _height;
        private int _width;
        private Location[][] locations;

        public GridWorld(Ecosystem ecosystem, IWeatherManager weatherManager, int width = 10, int height = 10) : base(ecosystem, weatherManager)
        {
            _width = width;
            _height = height;
        }

        public override void createWorld()
        {
            locations = new Location[_height][];
            for (int i = 0; i < _height; i++)
            {
                var row = new Location[_width];
                for (int j = 0; j < _width; j++)
                {
                    row[j] = new Location();
                }
                locations[i] = row;
            }
            RegisterNeighbours();
        }

        private void RegisterNeighbours()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    locations[i][j].Neighbours.Add(locations[i][j]);
                    locations[i][j].Neighbours.Add(locations[i][j]);
                    locations[i][j].Neighbours.Add(locations[i][j]);
                    locations[i][j].Neighbours.Add(locations[i][j]);
                }
            }
        }

        public override IEnumerable<Location> GetLocations()
        {
            ISet<Location> allLocations = new HashSet<Location>();
            foreach(Location[] row in locations)
            {
                foreach(Location location in row)
                {
                    allLocations.Add(location);
                }
            }
            return allLocations;
        }
    }
}
