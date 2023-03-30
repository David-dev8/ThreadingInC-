using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class PopulationCount
    {
        public DateTime Date { get; set; }
        public Species Species { get; set; }
        public int AmountOfAnimals { get; set; }

        public PopulationCount(DateTime date, Species species, int amountOfAnimals)
        {
            Date = date;
            Species = species;
            AmountOfAnimals = amountOfAnimals;
        }
    }
}
