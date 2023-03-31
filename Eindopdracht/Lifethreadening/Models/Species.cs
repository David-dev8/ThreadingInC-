using Lifethreadening.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to store data about species
    /// </summary>
    public class Species: ChartNamedEntity
    {
        public int Id { get; set; }

        private string _scientificName;
        public string ScientificName 
        {
            get 
            {
                return _scientificName;
            } 
            set 
            { 
                _scientificName = value;
                NotifyPropertyChanged();
            }
        }

        private string _image;
        public string Image 
        {
            get 
            { 
                return _image;
            }
            set 
            {
                _image = value;
                NotifyPropertyChanged();
            }
        }

        private int _AverageAge;
        public int AverageAge 
        {
            get 
            { 
                return _AverageAge;
            }
            set 
            {
                _AverageAge = value;
                NotifyPropertyChanged();
            } 
        }

        public int MaxAge { get; set; }
        
        private int _breedSize;
        public int BreedSize 
        {
            get 
            { 
                return _breedSize;
            }
            set 
            {
                _breedSize = value;
                NotifyPropertyChanged();
            } 
        }

        private Diet _diet;
        public Diet Diet 
        {
            get 
            {
                return _diet;
            } 
            set 
            { 
                _diet = value;
                NotifyPropertyChanged();
            }
        }

        public Statistics BaseStatistics { get; set; }
        
        public string Description { get; set; }
        public int MaxBreedSize { get; set; }
        public int MinBreedSize { get; set; }

        /// <summary>
        /// Creates a new species object
        /// </summary>
        /// <param name="id">The ID of the species</param>
        /// <param name="name">The name of the species</param>
        /// <param name="description">The description of the species</param>
        /// <param name="scientificName">The scientific name of the species</param>
        /// <param name="image">The image representing the species</param>
        /// <param name="averageAge">The average age of the species</param>
        /// <param name="maxAge">The max age of the species</param>
        /// <param name="maxBreedSize">The maximum breed size of this species</param>
        /// <param name="minBreedSize">The minimum breedsize of this speices</param>
        /// <param name="diet">The diet of this species</param>
        /// <param name="baseStatistics">The base stats of this species</param>
        [JsonConstructor]
        public Species(int id, string name, string description, string scientificName, string image, int averageAge, int maxAge, int maxBreedSize, int minBreedSize, Diet diet, Statistics baseStatistics = null) : base(name)
        {
            Id = id;
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

        /// <summary>
        /// This function checks if a spiecies data is valid to be saved to the database
        /// </summary>
        /// <returns>A collection with all the remaining errors</returns>
        public List<string> CheckIfValid()
        {
            List<string> valid = new List<string>();

            if (Name == "")
            {
                valid.Add("* The spiecies is missing a name");
            }
            if (ScientificName == "")
            {
                valid.Add("* The spiecies is missing a scientific name");
            }
            if (Diet == null)
            {
                valid.Add("* The spiecies is missing a diet");
            }
            if (AverageAge == 0)
            {
                valid.Add("* The spiecies needs to have a lifespan greater than 0");
            }
            if(BreedSize == 0)
            {
                valid.Add("* The spiecies needs to have a breedsize greater than 0");
            }
            if (Image == "")
            {
                valid.Add("* The spiecies is missing an image");
            }
            return valid;
        }
    }
}
