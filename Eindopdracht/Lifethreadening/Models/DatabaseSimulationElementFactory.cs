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
    public class DatabaseSimulationElementFactory : ISimulationElementFactory
    {
        private const double ANIMAL_CHANCE = 0.50;
        private const double VEGETATION_CHANCE = 0.35;
        private const double OBSTRUCTION_CHANCE = 0.15;
        private const int STATISTICS_DEVIATION = 5;

        private readonly IBehaviourBuilder _behaviourBuilder;
        private readonly IBreedFactory _breedFactory;
        private Random _random = new Random();

        public DatabaseSimulationElementFactory(IBehaviourBuilder behaviourBuilder)
        {
            _behaviourBuilder = behaviourBuilder;
            _breedFactory = new EvenlyDistributedParentsBreedFactory(_behaviourBuilder);
        }

        public SimulationElement CreateRandomElement(Ecosystem ecosystem)
        {
            double randomNumber = _random.NextDouble();
            double total = 0;
            if(randomNumber < (total += ANIMAL_CHANCE))
            {
                return CreateAnimal(ecosystem);
            }
            else if(randomNumber < (total += VEGETATION_CHANCE))
            {
                return CreateVegetation(ecosystem);
            }
            else if(randomNumber < (total += OBSTRUCTION_CHANCE))
            {
                return CreateObstruction(ecosystem);
            }
            return null;
        }

        public Animal CreateAnimal(Ecosystem ecosystem)
        {
            ISpeciesReader speciesReader = new DatabaseSpeciesReader();
            Species species = speciesReader.ReadByEcosystem(ecosystem.Id).GetRandom();

            Animal newAnimal = new Animal(EnumHelpers.GetRandom<Sex>(), species, GenerateStatisticsFromBase(species.BaseStatistics));
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

        public Obstruction CreateObstruction(Ecosystem ecosystem)
        {
            IObstructionReader obstructionReader = new DatabaseObstructionReader();
            return obstructionReader.ReadByEcosystem(ecosystem.Id).GetRandom();
        }

        public Vegetation CreateVegetation(Ecosystem ecosystem)
        {
            IVegetationReader vegetationReader = new DatabaseVegetationReader();
            return vegetationReader.ReadByEcosystem(ecosystem.Id).GetRandom();
        }

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
