using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Rank
    {
        private static readonly List<string> _rankDescription = new List<string>() 
        {
            "Thriving with an average of {0:0.00} living animals at any given time",
            "A solid average of {0:0.00} living animals at any given time",
            "Performing at {0:0.00} living animals at any given time",
            "An average of {0:0.00} living animals at any given time"
        };

        public int Position { get; set; }
        public Species Species { get; set; }
        public double Average { get; set; }
        public string Description
        {
            get
            {
                string description = Position <= 3 ? _rankDescription[Position - 1] : _rankDescription[3];
                return string.Format(description, Average);
            }
        }

        public Rank(int position, Species species, double average)
        {
            Position = position;
            Species = species;
            Average = average;
        }   
    }
}
