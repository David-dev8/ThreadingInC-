using Lifethreadening.DataAccess;
using Lifethreadening.DataAccess.Database;
using Lifethreadening.ExtensionMethods;
using Lifethreadening.Helpers;
using Lifethreadening.Models.Behaviours;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.Models
{
    /// <summary>
    /// This class is used to create simulation elements from the database
    /// </summary>
    public class DatabaseSimulationElementFactory : ISimulationElementFactory
    {
        private const double ANIMAL_CHANCE = 0.10;
        private const double VEGETATION_CHANCE = 0.35;
        private const int STATISTICS_DEVIATION = 5;

        private readonly IBehaviourBuilder _behaviourBuilder;
        private readonly IBreedFactory _breedFactory;
        private readonly INameReader _nameReader;
        private Random _random = new Random();

        /// <summary>
        /// Creates a new database simulation ellement factory 
        /// </summary>
        /// <param name="behaviourBuilder">The behaviour boulder to use</param>
        /// <param name="nameReader">The namereader to use</param>
        public DatabaseSimulationElementFactory(IBehaviourBuilder behaviourBuilder, INameReader nameReader)
        {
            _behaviourBuilder = behaviourBuilder;
            _breedFactory = new EvenlyDistributedParentsBreedFactory(_behaviourBuilder, nameReader);
            _nameReader = nameReader;
        }

        /// <summary>
        /// This function creates a random simulation element
        /// </summary>
        /// <param name="contextService">The context service</param>
        /// <returns>A random simulation element</returns>
        public SimulationElement CreateRandomElement(WorldContextService contextService)
        {
            double randomNumber = _random.NextDouble();
            double total = 0;
            if(randomNumber < (total += ANIMAL_CHANCE))
            {
                return CreateAnimal(contextService);
            }
            else if(randomNumber < (total += VEGETATION_CHANCE))
            {
                return CreateVegetation(contextService);
            }
            else
            {
                return CreateObstruction(contextService);
            }
        }

        /// <summary>
        /// This function Creates an animal at random
        /// </summary>
        /// <param name="contextService">The contectservice</param>
        /// <returns>an animal</returns>
        public Animal CreateAnimal(WorldContextService contextService)
        {
            ISpeciesReader speciesReader = new DatabaseSpeciesReader();
            Species species = speciesReader.ReadByEcosystem(contextService.GetContext().Ecosystem.Id).GetRandom();

            Sex sex = EnumHelpers.GetRandom<Sex>();
            string name = _nameReader.GetName(sex);
            Animal newAnimal = new Animal(name, sex, species, GenerateStatisticsFromBase(species.BaseStatistics), contextService);
            newAnimal.Behaviour = _behaviourBuilder
                .ForAnimal(newAnimal)
                .AddEat(species.Diet)
                .AddBreed(_breedFactory) // TODO
                .AddWander(false)
                .AddRest()
                .AddEvade()
                .GetBehaviour();
            return newAnimal;
        }

        /// <summary>
        /// This function creates an obstruction at random
        /// </summary>
        /// <param name="contextService">The contextservice to use</param>
        /// <returns>A random obstruction</returns>
        public Obstruction CreateObstruction(WorldContextService contextService)
        {
            IObstructionReader obstructionReader = new DatabaseObstructionReader();
            return obstructionReader.ReadByEcosystem(contextService.GetContext().Ecosystem.Id, contextService).GetRandom();
        }

        /// <summary>
        /// This function returns vegitation at random
        /// </summary>
        /// <param name="contextService">The contextservice</param>
        /// <returns>A random vegitation from the database</returns>
        public Vegetation CreateVegetation(WorldContextService contextService)
        {
            IVegetationReader vegetationReader = new DatabaseVegetationReader();
            return vegetationReader.ReadByEcosystem(contextService.GetContext().Ecosystem.Id, contextService).GetRandom();
        }

        /// <summary>
        /// This function creates a new statist object for a spiecies with minor deviations
        /// </summary>
        /// <param name="baseStatistics">The base statistics to use as a base</param>
        /// <returns>A new statisticks object</returns>
        public Statistics GenerateStatisticsFromBase(Statistics baseStatistics)
        {
            Statistics statistics = baseStatistics.Clone();
            statistics.Aggresion.Deviate(STATISTICS_DEVIATION);
            statistics.Speed.Deviate(STATISTICS_DEVIATION);
            statistics.Detection.Deviate(STATISTICS_DEVIATION);
            statistics.Intelligence.Deviate(STATISTICS_DEVIATION);
            return statistics;
        }
    }
}
