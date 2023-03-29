using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Rank
    {
        public Species Species { get; set; }
        public double Average { get; set; }

        public Rank(Species species, double average)
        {
            Species = species;
            Average = average;
        }   
    }
}
