using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Simulation
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public World World { get; set; }

        public Simulation(string name, World world) 
        { 
            Name = name;
            World = world;
        }

        public void step()
        {
        }

        private bool isGameOver()
        {
            return GetAnimals().Any();
        }

        private IEnumerable<Animal> GetAnimals()
        {
            return Enumerable.Empty<Animal>();
        }
    }
}
