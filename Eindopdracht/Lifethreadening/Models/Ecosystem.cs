using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store data for ecosystems
    /// </summary>
    public class Ecosystem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Difficulty { get; set; }

        /// <summary>
        /// Creates a new ecosystem
        /// </summary>
        /// <param name="id">The ID of the ecosystem</param>
        /// <param name="name">The name of the ecosystem</param>
        /// <param name="image">The image representing the ecosystem</param>
        /// <param name="difficulty">The difficulty rating of the ecosystem</param>
        public Ecosystem(int id, string name, string image, double difficulty)
        {
            Id = id;
            Name = name;
            Image = image;
            Difficulty = difficulty;
        }
    }
}
