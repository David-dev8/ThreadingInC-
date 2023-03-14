using Lifethreadening.DataAccess.Database;
using Lifethreadening.ExtensionMethods;
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
        const double ANIMAL_CHANCE = 0.50;
        const double VEGETATION_CHANCE = 0.35;
        const double OBSTRUCTION_CHANCE = 0.15;
        const int STATISTICS_DEVIATION = 5;
        private Random _random = new Random();

        public SimulationElement CreateRandomElement(Ecosystem ecosystem)
        {
            double randomNumber = _random.NextDouble();
            double total = 0;
            if((total += ANIMAL_CHANCE) < randomNumber)
            {
                return CreateAnimal(ecosystem);
            }
            else if((total += VEGETATION_CHANCE) < randomNumber)
            {
                return CreateVegetation(ecosystem);
            }
            else if((total += OBSTRUCTION_CHANCE) < randomNumber)
            {
                return CreateObstruction(ecosystem);
            }
            return null;
        }

        public Animal CreateAnimal(Ecosystem ecosystem)
        {
            DatabaseHelper<Species> helper = new DatabaseHelper<Species>();
            IEnumerable<Species> allSpecies = helper.Read(null, null, CommandType.Text);
            Species species = allSpecies.ElementAt(_random.Next(allSpecies.Count()));

            Animal newAnimal = new Animal(GetRandomSex(), species, GenerateStatisticsFromBase(species.BaseStatistics));
            return newAnimal;
        }

        public Obstruction CreateObstruction(Ecosystem ecosystem)
        {
            throw new NotImplementedException();
        }

        public Vegetation CreateVegetation(Ecosystem ecosystem)
        {
            throw new NotImplementedException();
        }

        public Sex GetRandomSex()
        {
            Array values = Enum.GetValues(typeof(Sex));
            return (Sex)values.GetValue(_random.Next(values.Length));
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
