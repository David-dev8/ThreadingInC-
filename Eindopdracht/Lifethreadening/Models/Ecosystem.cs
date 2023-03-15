using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Ecosystem
    {
        public string Name { get; set; }
        public string Image { get; set; }

        public Ecosystem(string name, string image)        {
            Name = name;
            Image = image;
        }
    }
}
