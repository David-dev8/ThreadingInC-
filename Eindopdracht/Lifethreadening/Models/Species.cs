using Lifethreadening.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lifethreadening.Models
{
    public class Species: Observable
    {
        public string Name { get; set; }
        public string ScientificName { get; set; }
        public string Image { get; set; }

        private int _AverageAge;
        public int AverageAge {
            get { 
                return _AverageAge;
            }
            set {
                _AverageAge = value;
                NotifyPropertyChanged();
            } 
        }


        public int MaxAge { get; set; }
        
        private int _breedSize;
        public int BreedSize {
            get { 
                return _breedSize;
            }
            set {
                _breedSize = value;
                NotifyPropertyChanged();
            } 
        }

        public Diet Diet { get; set; }
        public Statistics BaseStatistics { get; set; }

        public Species()
        {
            Name = "";
            ScientificName = "";
            Image = "";
            AverageAge = 0;
            MaxAge = 0;
            BreedSize = 0;
            Diet = Diet.HERBIVORE;
            BaseStatistics = new Statistics();
        }

        public Species(string name, string scientificName, string image, int averageAge, int maxAge, int breedSize, Diet diet, Statistics baseStatistics)
        {
            Name = name;
            ScientificName = scientificName;
            Image = image;
            AverageAge = averageAge;
            MaxAge = maxAge;
            BreedSize = breedSize;
            Diet = diet;
            BaseStatistics = baseStatistics;
        }
    }
}
