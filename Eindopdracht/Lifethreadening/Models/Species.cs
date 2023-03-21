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
        private string _name;
        public string Name {
            get { 
             return _name;
            } set { 
                _name = value;
                NotifyPropertyChanged();
            }
        }

        private string _scientificName;
        public string ScientificName {
            get {
                return _scientificName;
            } set { 
                _scientificName = value;
                NotifyPropertyChanged();
            }
        }

        private string _image;
        public string Image {
            get { 
                return _image;
            }
            set {
                _image = value;
                NotifyPropertyChanged();
            }
        }

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

        private Diet _diet;
        public Diet Diet {
            get {
                return _diet;
            } set { 
                _diet = value;
                NotifyPropertyChanged();
            }
        }


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
