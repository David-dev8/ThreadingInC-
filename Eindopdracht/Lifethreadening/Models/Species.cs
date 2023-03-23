using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    // TODO constructor
    public class Species : NamedEntity
    {
        public int Id { get; set; }
        public string ScientificName { get; set; }
        public string Image { get; set; }        
        public int AverageAge { get; set; }
        public int MaxAge { get; set; }
        public int BreedSize { get; set; }
        public Diet Diet { get; set; }
        public Statistics BaseStatistics { get; set; }
        
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Species other = (Species)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
