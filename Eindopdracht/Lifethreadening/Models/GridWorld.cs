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
        public int Height { get; set; }
        public int Width { get; set; }
        private Location[][] locations;

        public GridWorld(Ecosystem ecosystem, IWeatherManager weatherManager, int width = 10, int height = 10) : base(ecosystem, weatherManager)
        {
            Width = width;
            Height = height;
            CreateWorld();
        }

        public override void CreateWorld()
        {
            locations = new Location[Height][];
            for (int i = 0; i < Height; i++)
            {
                var row = new Location[Width];
                for (int j = 0; j < Width; j++)
                {
                    row[j] = new Location();
                }
                locations[i] = row;
            }
            RegisterNeighbours();
        }

        private void RegisterNeighbours()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
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
