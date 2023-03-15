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
        private Location[][] _locations;

        public GridWorld(Ecosystem ecosystem, IWeatherManager weatherManager, int width = 25, int height = 25) : base(ecosystem, weatherManager)
        {
            Width = width;
            Height = height;
            CreateWorld();
        }

        public override void CreateWorld()
        {
            _locations = new Location[Height][];
            for (int i = 0; i < Height; i++)
            {
                var row = new Location[Width];
                for (int j = 0; j < Width; j++)
                {
                    row[j] = new Location();
                }
                _locations[i] = row;
            }
            RegisterNeighbours();
        }

        private void RegisterNeighbours()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    RegisterNeighbour(_locations[i][j], i + 1, j);
                    RegisterNeighbour(_locations[i][j], i - 1, j);
                    RegisterNeighbour(_locations[i][j], i, j + 1);
                    RegisterNeighbour(_locations[i][j], i, j - 1);
                }
            }
        }

        private void RegisterNeighbour(Location location, int row, int column)
        {
            Location neighbour = _locations.ElementAtOrDefault(row)?.ElementAtOrDefault(column);
            if(neighbour != null)
            {
                location.Neighbours.Add(neighbour);
            }
        }

        public override IEnumerable<Location> GetLocations()
        {
            IList<Location> allLocations = new List<Location>();
            foreach(Location[] row in _locations)
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
