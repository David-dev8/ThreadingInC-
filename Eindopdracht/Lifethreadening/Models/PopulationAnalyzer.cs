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
        public IDictionary<DateTime, IDictionary<Species, int>> SpeciesCount { get; private set; }

        public PopulationAnalyzer()
        {
            SpeciesCount = new Dictionary<DateTime, IDictionary<Species, int>>
            {
                { DateTime.Now, new Dictionary<Species, int>() },
                { DateTime.Now.AddYears(1), new Dictionary<Species, int>() },
                { DateTime.Now.AddYears(2), new Dictionary<Species, int>() },
                { DateTime.Now.AddYears(3), new Dictionary<Species, int>() }
            };
            SpeciesCount.First().Value.Add(new Species() { Name = "Koe", Id= 111 }, 1);
            SpeciesCount.ElementAt(1).Value.Add(new Species() { Name = "Koe", Id = 111 }, 1);
            SpeciesCount.ElementAt(2).Value.Add(new Species() { Name = "Koe", Id = 111 }, 10);
            SpeciesCount.ElementAt(2).Value.Add(new Species() { Name = "Varken", Id = 555 }, 5);
            GetSpeciesCountPerSpecies();
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
                if(amountOfSpecies.ContainsKey(animal.Species))
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
            IDictionary<Species, IDictionary<DateTime, int>> speciesCountPerSpecies = GetSpeciesCountPerSpecies();
            return speciesCountPerSpecies.Select(speciesCount => new Rank(speciesCount.Key, speciesCount.Value.Values.Average()))
                .OrderByDescending(rank => rank.Average);
        }

        // TODO plinq
        public IDictionary<DateTime, double> GetShannonWeaverData()
        {
            return SpeciesCount.ToDictionary(speciesCount => speciesCount.Key, speciesCount => CalculateShannonWeaverIndex(speciesCount.Value));
        }

        // TODO moet het niet keer -1?
        private double CalculateShannonWeaverIndex(IDictionary<Species, int> populations)
        {
            int amountOfAnimals = populations.Values.Sum();
            return populations
                .Select(population => (double) population.Value / amountOfAnimals)
                .Sum(relativePresence => relativePresence * Math.Log(relativePresence)) * -1;
        }

        // TODO: Plinq en opdelen??
        private IDictionary<Species, IDictionary<DateTime, int>> GetSpeciesCountPerSpecies()
        {
            return SpeciesCount.SelectMany(speciesCount => speciesCount.Value, (SpeciesCount, amountPerDate) => new { Date = SpeciesCount.Key, Species = amountPerDate.Key, Quantity = amountPerDate.Value })
                .Aggregate(new Dictionary<Species, IDictionary<DateTime, int>>(), (seed, value) => {
                    if (!seed.ContainsKey(value.Species))
                    {
                        seed.Add(value.Species, new Dictionary<DateTime, int>());
                    }
                    seed[value.Species].Add(value.Date, value.Quantity);
                    return seed;
                });
        }
    }
}
