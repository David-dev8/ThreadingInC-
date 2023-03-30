using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Ecosystem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Difficulty { get; set; }

        public Ecosystem(string name, string image, string difficulty)
        {        
            Name = name;
            Image = image;
            Difficulty = difficulty;
        }
    }
}
