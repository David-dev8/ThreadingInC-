using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class Species : NamedEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ScientificName { get; set; }
        public string Image { get; set; }
        public int AverageAge { get; set; }
        public int MaxAge { get; set; }
        public int MaxBreedSize { get; set; }
        public int MinBreedSize { get; set; }
        public Diet Diet { get; set; }
        public Statistics BaseStatistics { get; set; }

        public Species(int id, string name, string description, string scientificName, string image, int averageAge, int maxAge, int maxBreedSize, int minBreedSize, Diet diet, Statistics baseStatistics)
        {
            Id = id;
            Name = name;
            Description = description;
            ScientificName = scientificName;
            Image = image;
            AverageAge = averageAge;
            MaxAge = maxAge;
            MaxBreedSize = maxBreedSize;
            MinBreedSize = minBreedSize;
            Diet = diet;
            BaseStatistics = baseStatistics;
        }

        public override bool Equals(object obj)
        {
            if(obj == null || GetType() != obj.GetType())
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
