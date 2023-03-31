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
    /// <summary>
    /// This class is used to represent a simulations world as a grid
    /// </summary>
    public class GridWorld : World
    {
        private int _height;
        private int _width;
        public Location[][] Grid { get; private set; }

        /// <summary>
        /// Creates a new gridworld
        /// </summary>
        /// <param name="ecosystem">The ecosystem of the new world</param>
        /// <param name="weatherManager">The weather manager of the new world</param>
        /// <param name="width">The width in cells of the new world</param>
        /// <param name="height">The height in cells of the new world</param>
        public GridWorld(Ecosystem ecosystem, IWeatherManager weatherManager, int width = 50, int height = 50) : base(ecosystem, weatherManager)
        {
            _height = width;
            _width = height;
            CreateWorld();
        }

        /// <summary>
        /// This function creates a basic grid world
        /// </summary>
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

        /// <summary>
        /// This function links all cells together as locations
        /// </summary>
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

        /// <summary>
        /// This function allows one location to assign another location as its neighbor
        /// </summary>
        /// <param name="location">The location that needs the neigbor</param>
        /// <param name="row"> the row of the neighbor </param>
        /// <param name="column"> the column of the neighbor </param>
        private void RegisterNeighbour(Location location, int row, int column)
        {
            Location neighbour = Grid.ElementAtOrDefault(row)?.ElementAtOrDefault(column);
            if(neighbour != null)
            {
                location.Neighbours.Add(neighbour);
            }
        }

        /// <summary>
        /// This function Gets all locations from the grid world
        /// </summary>
        /// <returns>A collection of all locations in the gridworld</returns>
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

        /// <summary>
        /// This function executes a game tick on the gridworld
        /// </summary>
        public override void Step()
        {
            base.Step();
            OnPropertyChanged(nameof(Grid));
        }
    }
}
