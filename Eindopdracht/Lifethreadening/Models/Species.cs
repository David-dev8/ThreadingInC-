using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Species
    {
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string Image { get; set; }        
        public int AverageAge { get; set; }
        public int MaxAge { get; set; }
        public int BreedSize { get; set; }
        public Diet Diet { get; set; }
        public Statistics BaseStatistics { get; set; }
    }
}
