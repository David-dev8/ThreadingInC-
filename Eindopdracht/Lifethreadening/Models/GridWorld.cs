using Lifethreadening.DataAccess;
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
        public int Height { get; set; }
        public int Width { get; set; }
        public Location[][] Grid { get; set; }

        public GridWorld(Ecosystem ecosystem, IWeatherManager weatherManager, int width = 25, int height = 25) : base(ecosystem, weatherManager)
        {
            Width = width;
            Height = height;
            CreateWorld();
        }

        public override void CreateWorld()
        {
            Grid = new Location[Height][];
            for (int i = 0; i < Height; i++)
            {
                var row = new Location[Width];
                for (int j = 0; j < Width; j++)
                {
                    row[j] = new Location();
                }
                Grid[i] = row;
            }
            RegisterNeighbours();
        }

        private void RegisterNeighbours()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    RegisterNeighbour(Grid[i][j], i + 1, j);
                    RegisterNeighbour(Grid[i][j], i - 1, j);
                    RegisterNeighbour(Grid[i][j], i, j + 1);
                    RegisterNeighbour(Grid[i][j], i, j - 1);
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
