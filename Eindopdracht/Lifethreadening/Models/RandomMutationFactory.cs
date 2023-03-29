using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.API;
using Lifethreadening.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    public class RandomMutationFactory : IMutationFactory
    {
        private const double STATISTIC_MUTATION_CHANCE = 0.20;
        private const int STATISTIC_MUTATION_MIN_CHANGE = -10;
        private const int STATISTIC_MUTATION_MAX_CHANGE = 7;
        private Random _random = new Random();

        private readonly IGeneReader _geneReader;

        public RandomMutationFactory()
        {
            _geneReader = new APIGeneReader();
        }

        public async Task<Mutation> CreateMutation()
        {
            string gene = await _geneReader.GetRandomGene(); // TODO rename allel to gene
            IEnumerable<string> proteins = await _geneReader.GetRandomProteins(2);
            return new Mutation(EnumHelpers.GetRandom<MutationType>(), gene, 
                proteins.ElementAt(0), proteins.ElementAt(1), MutateAction);
        }

        private void MutateAction(Statistics statistics)
        {
            statistics.Weight += GetRandomMutationValue();
            statistics.Size += GetRandomMutationValue();
            statistics.Speed += GetRandomMutationValue();
            statistics.Aggresion += GetRandomMutationValue();
            statistics.Detection += GetRandomMutationValue();
            statistics.Resilience += GetRandomMutationValue();
            statistics.Intelligence += GetRandomMutationValue();
            statistics.SelfDefence += GetRandomMutationValue();
            statistics.MetabolicRate += GetRandomMutationValue();
        }

        private int GetRandomMutationValue()
        {
            return _random.NextDouble() < STATISTIC_MUTATION_CHANCE ?
                (_random.Next(STATISTIC_MUTATION_MIN_CHANGE, STATISTIC_MUTATION_MAX_CHANGE)) : 0;
        }
    }
}
