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
    /// <summary>
    /// This class is used to analyse data about populations
    /// </summary>
    public class PopulationAnalyzer
    {
        public IDictionary<DateTime, IDictionary<Species, int>> SpeciesCount { get; set; }

        /// <summary>
        /// Creates a new population analyser
        /// </summary>
        public PopulationAnalyzer()
        {
            SpeciesCount = new Dictionary<DateTime, IDictionary<Species, int>>{};
        }

        /// <summary>
        /// This function registers a set of animals to the analyser
        /// </summary>
        /// <param name="animals">The colection of animals to add</param>
        /// <param name="currentDate">The date of the animal collection gathering</param>
        public void RegisterAnimals(IEnumerable<Animal> animals, DateTime currentDate)
        {
            // TODO already exists
            if (!SpeciesCount.ContainsKey(currentDate))
            {
                SpeciesCount.Add(currentDate, new Dictionary<Species, int>());
            }

            IDictionary<Species, int> amountOfSpecies = SpeciesCount[currentDate];
            foreach (Animal animal in animals)
            {
                if (amountOfSpecies.ContainsKey(animal.Species))
                {
                    amountOfSpecies[animal.Species]++;
                }
                else
                {
                    amountOfSpecies.Add(animal.Species, 1);
                }
            }
        }

        /// <summary>
        /// This function calculates the most dominant species
        /// </summary>
        /// <returns>A list of Ranks</returns>
        public IEnumerable<Rank> GetDominatingSpecies()
        {
            int position = 1;
            IDictionary<Species, IDictionary<DateTime, int>> speciesCountPerSpecies = GetSpeciesCountPerSpecies();
            return speciesCountPerSpecies.Select(speciesCount =>
            {
                return new { Species = speciesCount.Key, Average = speciesCount.Value.Values.Average() };
            }).OrderByDescending(averageSpeciesCount => averageSpeciesCount.Average)
            .Select(averageSpeciesCount => new Rank(position++, averageSpeciesCount.Species, averageSpeciesCount.Average)); // TODO intabbing?
        }

        // TODO plinq
        /// <summary>
        /// This function calculates the shannon weaver indexes for all the animals
        /// </summary>
        /// <returns>The shannon weaver indexes at every time intervall</returns>
        public IDictionary<DateTime, double> GetShannonWeaverData()
        {
            return GetSpeciesCountWithMissingDates().ToDictionary(speciesCount => speciesCount.Key, speciesCount => CalculateShannonWeaverIndex(speciesCount.Value));
        }

        /// <summary>
        /// This function calculates the shannon weaver index for a set of species
        /// </summary>
        /// <param name="populations">The populations to consider when calculating</param>
        /// <returns>The shannon weaver index for the input</returns>
        private double CalculateShannonWeaverIndex(IDictionary<Species, int> populations)
        {
            int amountOfAnimals = populations.Values.Sum();
            return populations
                .Select(population => amountOfAnimals > 0 ? ((double)population.Value / amountOfAnimals) : 0)
                .Sum(relativePresence => relativePresence > 0 ? ((relativePresence * Math.Log(relativePresence)) * -1) : 0);
        }

        // TODO: Plinq en opdelen??
        /// <summary>
        /// This fucntion gets the population count per species
        /// </summary>
        /// <returns>A colection linking species to their populations at serveral intervals</returns>
        public IDictionary<Species, IDictionary<DateTime, int>> GetSpeciesCountPerSpecies()
        {
            return GetSpeciesCountWithMissingDates()
                .SelectMany(speciesCount => speciesCount.Value, (SpeciesCount, amountPerDate) => new PopulationCount(SpeciesCount.Key, amountPerDate.Key, amountPerDate.Value))
                .Aggregate(new Dictionary<Species, IDictionary<DateTime, int>>(), (seed, value) => {
                    if (!seed.ContainsKey(value.Species))
                    {
                        seed.Add(value.Species, new Dictionary<DateTime, int>());
                    }
                    seed[value.Species].Add(value.Date, value.AmountOfAnimals);
                    return seed;
                });
        }

        /// <summary>
        /// This fucntion gets the population count per species
        /// </summary>
        /// <returns>A colection linking species to their populations at serveral intervals and at missing dates</returns>
        private IDictionary<DateTime, IDictionary<Species, int>> GetSpeciesCountWithMissingDates()
        {
            IEnumerable<Species> species = SpeciesCount.SelectMany(speciesCount => { // TODO naamgeving
                var b = speciesCount.Value.Keys;
                return b;
            }).Distinct();
            var st = SpeciesCount.Select(s =>
            {
                IDictionary<Species, int> t = species.Select(sp => {
                    return new { Key = sp, Value = s.Value.ContainsKey(sp) ? s.Value[sp] : 0 };
                }).ToDictionary(b => b.Key, b => b.Value);
                return new { Key = s.Key, Value = t }; // TODO: join????
            }).ToDictionary(b => b.Key, b => b.Value);
            return st;
        }
    }
}
