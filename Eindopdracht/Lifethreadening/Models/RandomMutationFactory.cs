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
    /// <summary>
    /// This class is used to create random mutations 
    /// </summary>
    public class RandomMutationFactory : IMutationFactory
    {
        private const double STATISTIC_MUTATION_CHANCE = 0.20;
        private const int STATISTIC_MUTATION_MIN_CHANGE = -10;
        private const int STATISTIC_MUTATION_MAX_CHANGE = 7;
        private Random _random = new Random();

        private readonly IGeneReader _geneReader;

        /// <summary>
        /// Creates a new random mutation factory
        /// </summary>
        public RandomMutationFactory()
        {
            _geneReader = new APIGeneReader();
        }

        /// <summary>
        /// This function creates a new random mutation
        /// </summary>
        /// <param name="currentDate">The date the mutation is created</param>
        /// <returns>A new random mutation</returns>
        public async Task<Mutation> CreateMutation(DateTime currentDate)
        {
            string gene = await _geneReader.GetRandomGene();
            IEnumerable<string> proteins = await _geneReader.GetRandomProteins(2);
            return new Mutation(EnumHelpers.GetRandom<MutationType>(), gene, 
                proteins.ElementAt(0), proteins.ElementAt(1), currentDate, MutateAction);
        }

        /// <summary>
        /// This function generates a random mutation effect
        /// </summary>
        /// <param name="statistics"></param>
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

        /// <summary>
        /// This function returns 0 or a random value between 0 and 100 at random
        /// </summary>
        /// <returns>0 or a random value between 0 and 100</returns>
        private int GetRandomMutationValue()
        {
            return _random.NextDouble() < STATISTIC_MUTATION_CHANCE ?
                (_random.Next(STATISTIC_MUTATION_MIN_CHANGE, STATISTIC_MUTATION_MAX_CHANGE)) : 0;
        }
    }
}
