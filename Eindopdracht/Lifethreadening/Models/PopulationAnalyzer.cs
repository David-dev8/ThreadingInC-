using Lifethreadening.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Lifethreadening.Models
{
    public class PopulationAnalyzer
    {
        private const int MINIMUM_REQUIRED_TO_BE_FAIR = 100;
        public IDictionary<DateTime, IDictionary<Species, int>> SpeciesCount { get; set; }

        private ISet<Species> _species
        {
            get
            {
                return SpeciesCount.First().Value.Keys.ToHashSet();
            }
        }

        public PopulationAnalyzer()
        {
            SpeciesCount = new Dictionary<DateTime, IDictionary<Species, int>> { };
        }

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

            foreach (Species s in _species)
            {
                if (!amountOfSpecies.ContainsKey(s))
                {
                    amountOfSpecies.Add(s, 0);
                }
            }
        }

        public IEnumerable<Rank> GetDominatingSpecies()
        {
            int position = 1;
            IDictionary<Species, IDictionary<DateTime, int>> speciesCountPerSpecies = GetSpeciesCountPerSpecies();
            return speciesCountPerSpecies.Select(speciesCount =>
            {
                return new { Species = speciesCount.Key, Average = speciesCount.Value.Values.Average() };
            }).OrderByDescending(averageSpeciesCount => averageSpeciesCount.Average)
            .Select(averageSpeciesCount => new Rank(position++, averageSpeciesCount.Species, averageSpeciesCount.Average));
        }

        public IDictionary<DateTime, double> GetShannonWeaverData()
        {
            return SpeciesCount.AsParallel().Select(speciesCount => 
            { 
                return new { Key = speciesCount.Key, index = CalculateShannonWeaverIndex(speciesCount.Value) }; 
            }).ToDictionary(k => k.Key, k => k.index);
        }

        private double CalculateShannonWeaverIndex(IDictionary<Species, int> populations)
        {
            int amountOfAnimals = populations.Values.Sum();
            return populations
                .RepeatUntilLength(MINIMUM_REQUIRED_TO_BE_FAIR)
                .Select(population => amountOfAnimals > 0 ? ((double)population.Value / amountOfAnimals) : 0)
                .Sum(relativePresence => relativePresence > 0 ? ((relativePresence * Math.Log(relativePresence)) / Math.Sqrt(MINIMUM_REQUIRED_TO_BE_FAIR) * -1) : 0);
        }

        public IDictionary<Species, IDictionary<DateTime, int>> GetSpeciesCountPerSpecies()
        {
            return SpeciesCount
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
    }
}
