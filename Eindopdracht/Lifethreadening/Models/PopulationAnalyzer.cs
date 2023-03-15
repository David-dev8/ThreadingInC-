using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    // Plinq voor de algemen loop door elke data shannon te berekenen
    public class PopulationAnalyzer
    {
        // TODO reverse date, (species, int)
        public IDictionary<Species, IDictionary<DateTime, int>> SpeciesCount { get; private set; }

        public void RegisterAnimals(IEnumerable<Animal> animals, DateTime currentDate)
        {
            // TODO GROUP BY???
            foreach(Animal animal in animals)
            {
                if(!SpeciesCount.ContainsKey(animal.Species))
                {
                    SpeciesCount.Add(animal.Species, new Dictionary<DateTime, int>());
                }

                IDictionary<DateTime, int> amountOfAnimalsPerDate = SpeciesCount[animal.Species];
                if (amountOfAnimalsPerDate.ContainsKey(currentDate))
                {
                    amountOfAnimalsPerDate[currentDate]++;
                }
                else
                {
                    amountOfAnimalsPerDate.Add(currentDate, 1);
                }
            }
        }

        public IEnumerable<Rank> GetDominatingSpecies()
        {
            // TODO average in aparte methode
            return SpeciesCount.Select(keyValue => new Rank(keyValue.Key, keyValue.Value.Values.Average()))
                .OrderByDescending(rank => rank.Average);
        }

        private double CalculateShannonWeaverIndex(IDictionary<Species, int> populations)
        {
            int amountOfAnimals = populations.Values.Sum();
            return populations
                .Select(population => population.Value / amountOfAnimals)
                .Sum(relativePresence => relativePresence * Math.Log(relativePresence));
        }

        //private double CalculateRelativePresence(int amountOfAllSpecies, int amountOfSingleSpecies)
        //{
        //    return 
        //}
    }
}
