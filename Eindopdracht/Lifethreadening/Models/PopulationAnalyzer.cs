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
        public IDictionary<DateTime, IDictionary<Species, int>> SpeciesCount { get; private set; } // TODO private field maken

        public PopulationAnalyzer()
        {
            SpeciesCount = new Dictionary<DateTime, IDictionary<Species, int>>{};
        }

        public void RegisterAnimals(IEnumerable<Animal> animals, DateTime currentDate)
        {
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
        public IDictionary<DateTime, double> GetShannonWeaverData()
        {
            return GetSpeciesCountWithMissingDates().ToDictionary(speciesCount => speciesCount.Key, speciesCount => CalculateShannonWeaverIndex(speciesCount.Value));
        }

        private double CalculateShannonWeaverIndex(IDictionary<Species, int> populations)
        {
            int amountOfAnimals = populations.Values.Sum();
            return populations
                .Select(population => amountOfAnimals > 0 ? ((double)population.Value / amountOfAnimals) : 0)
                .Sum(relativePresence => relativePresence > 0 ? ((relativePresence * Math.Log(relativePresence)) * -1) : 0);
        }

        // TODO: Plinq en opdelen??
        public IDictionary<Species, IDictionary<DateTime, int>> GetSpeciesCountPerSpecies()
        {
            return GetSpeciesCountWithMissingDates().SelectMany(speciesCount => speciesCount.Value, (SpeciesCount, amountPerDate) => new { Date = SpeciesCount.Key, Species = amountPerDate.Key, Quantity = amountPerDate.Value })
                .Aggregate(new Dictionary<Species, IDictionary<DateTime, int>>(), (seed, value) => {
                    if (!seed.ContainsKey(value.Species))
                    {
                        seed.Add(value.Species, new Dictionary<DateTime, int>());
                    }
                    seed[value.Species].Add(value.Date, value.Quantity);
                    return seed;
                });
        }

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
