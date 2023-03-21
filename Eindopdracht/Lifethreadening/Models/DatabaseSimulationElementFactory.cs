﻿using Lifethreadening.DataAccess;
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
        private const double ANIMAL_CHANCE = 0.10;
        private const double VEGETATION_CHANCE = 0.35;
        private const int STATISTICS_DEVIATION = 5;

        private readonly IBehaviourBuilder _behaviourBuilder;
        private readonly IBreedFactory _breedFactory;
        private readonly INameReader _nameReader;
        private Random _random = new Random();

        public DatabaseSimulationElementFactory(IBehaviourBuilder behaviourBuilder, INameReader nameReader)
        {
            _behaviourBuilder = behaviourBuilder;
            _breedFactory = new EvenlyDistributedParentsBreedFactory(_behaviourBuilder, nameReader);
            _nameReader = nameReader;
        }

        public async Task<SimulationElement> CreateRandomElement(WorldContextService contextService)
        {
            double randomNumber = _random.NextDouble();
            double total = 0;
            if(randomNumber < (total += ANIMAL_CHANCE))
            {
                return await CreateAnimal(contextService);
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

        public async Task<Animal> CreateAnimal(WorldContextService contextService)
        {
            ISpeciesReader speciesReader = new DatabaseSpeciesReader();
            Species species = speciesReader.ReadByEcosystem(contextService.GetContext().Ecosystem.Id).GetRandom();

            Sex sex = EnumHelpers.GetRandom<Sex>();
            string name = await _nameReader.GetName(sex);
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

        public Obstruction CreateObstruction(WorldContextService contextService)
        {
            IObstructionReader obstructionReader = new DatabaseObstructionReader();
            return obstructionReader.ReadByEcosystem(contextService.GetContext().Ecosystem.Id, contextService).GetRandom();
        }

        public Vegetation CreateVegetation(WorldContextService contextService)
        {
            IVegetationReader vegetationReader = new DatabaseVegetationReader();
            return vegetationReader.ReadByEcosystem(contextService.GetContext().Ecosystem.Id, contextService).GetRandom();
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
