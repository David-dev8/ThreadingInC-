using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.Algorithmic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml.Controls;

namespace Lifethreadening.Models
{
    public class GridWorld : World
    {
        private int _height = 50;
        private int _width = 50;
        public Location[][] Grid { get; set; }

        public GridWorld(Ecosystem ecosystem) : base(ecosystem, new RandomWeatherManager())
        {
            CreateWorld();
        }

        public override void CreateWorld()
        {
            Grid = new Location[_height][];
            for (int i = 0; i < _height; i++)
            {
                var row = new Location[_height];
                for (int j = 0; j < _width; j++)
                {
                    row[j] = new Location();
                }
                Grid[i] = row;
            }
            RegisterNeighbours();
        }

        private void RegisterNeighbours()
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    RegisterNeighbour(Grid[i][j], i - 1, j - 1);
                    RegisterNeighbour(Grid[i][j], i - 1, j);
                    RegisterNeighbour(Grid[i][j], i - 1, j + 1);
                    RegisterNeighbour(Grid[i][j], i, j - 1);
                    RegisterNeighbour(Grid[i][j], i, j + 1);
                    RegisterNeighbour(Grid[i][j], i + 1, j - 1);
                    RegisterNeighbour(Grid[i][j], i + 1, j);
                    RegisterNeighbour(Grid[i][j], i + 1, j + 1);
                }
            }
        }

        private void RegisterNeighbour(Location location, int row, int column)
        {
            Location neighbour = Grid.ElementAtOrDefault(row)?.ElementAtOrDefault(column);
            if(neighbour != null)
            {
                location.Neighbours.Add(neighbour);
            }
        }

        public override IEnumerable<Location> GetLocations()
        {
            IList<Location> allLocations = new List<Location>();
            foreach(Location[] row in Grid)
            {
                foreach(Location location in row)
                {
                    allLocations.Add(location);
                }
            }
            return allLocations;
        }

        public override void Step()
        {
            base.Step();
            OnPropertyChanged(nameof(Grid));
        }
    }
}
