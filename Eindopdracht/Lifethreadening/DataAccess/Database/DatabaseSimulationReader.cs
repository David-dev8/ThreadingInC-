using Lifethreadening.DataAccess.Algorithmic;
using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Database
{
    public class DatabaseSimulationReader : ISimulationReader
    {
        private static readonly IDictionary<string, Diet> dietDatabaseMapping = new Dictionary<string, Diet>()
        {
            { "Herbivore", Diet.HERBIVORE },
            { "Carnivore", Diet.CARNIVORE },
            { "Omnivore", Diet.HERBIVORE }
        };

        private static readonly IDictionary<string, MutationType> mutationTypeDatabaseMapping = new Dictionary<string, MutationType>()
        {
            { "Addition", MutationType.ADDITION },
            { "Substitution", MutationType.SUBSTITUTION },
            { "Delocation", MutationType.DELOCATION },
        };

        public IEnumerable<Simulation> ReadAll() 
        {
            DatabaseHelper<Simulation> database = new DatabaseHelper<Simulation>();
            string query = @"
                SELECT S.*, E.id AS ecosystemId, E.name AS ecosystemName, E.image, E.difficulty
                FROM Simulation AS S
                JOIN Ecosystem AS E ON E.id = S.ecosystemId";
            return database.Read(CreateSimulation, query, CommandType.Text);
        }

        public Simulation GetFullDetailsOfSimulation(Simulation simulation)
        {
            simulation.PopulationManager.SpeciesCount = (IDictionary<DateTime, IDictionary<Species, int>>)GetSpeciesCount(simulation);
            simulation.MutationManager.Mutations = GetMutations(simulation);
            return simulation;
        }

        private ISet<Mutation> GetMutations(Simulation simulation)
        {
            IEnumerable<Mutation> mutations = RetrieveMutations(simulation);
            return mutations.GroupBy(mutation => mutation, mutation => mutation.Affected).Select(mutation =>
            {
                mutation.Key.Affected = mutation.SelectMany(affected => affected).ToList();
                return mutation.Key;
            }).ToHashSet();
        }

        private IEnumerable<Mutation> RetrieveMutations(Simulation simulation)
        {
            DatabaseHelper<Mutation> database = new DatabaseHelper<Mutation>();
            string query = @"
                SELECT M.*, MAS.statistic, MAS.affection, MAS.Color
                FROM Mutation AS M
                JOIN MutationAffectedStatistic AS MAS ON M.allel = MAS.mutationAllel AND M.date = MAS.mutationDate AND M.simulationId = MAS.simulationId
                WHERE simulationId = @simulationId";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@simulationId", simulation.Id),
            };
            return database.Read(CreateMutation, query, CommandType.Text, parameters);
        }

        private Dictionary<DateTime, Dictionary<Species, int>> GetSpeciesCount(Simulation simulation)
        {
            IEnumerable<PopulationCount> populationCounts = RetrievePopulationCounts(simulation);
            return populationCounts.GroupBy(populationCount => populationCount.Date)
                .ToDictionary(populationCount => populationCount.Key, populationCount => populationCount.ToDictionary(speciesCount => speciesCount.Species,
                speciesCount => speciesCount.AmountOfAnimals));
        }

        private IEnumerable<PopulationCount> RetrievePopulationCounts(Simulation simulation)
        {
            DatabaseHelper<PopulationCount> database = new DatabaseHelper<PopulationCount>();
            string query = @"
                SELECT S.*, P.amountofAnimals, P.date
                FROM SimulationPopulation AS P
                JOIN Species AS s on S.id = P.speciesId
                WHERE simulationId = @simulationId";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@simulationId", simulation.Id),
            };
            return database.Read(CreatePopulationCount, query, CommandType.Text, parameters);
        }

        private Simulation CreateSimulation(SqlDataReader dataReader)
        {
            return new Simulation(
                dataReader.GetInt32("id"),
                dataReader.GetInt32("score"),
                dataReader.GetDateTime("dateStarted"),
                dataReader.GetInt32("amountOfDisasters"),
                dataReader.GetString("fileNameSaveSlot"),
                dataReader.GetString("name"),
                
                new EmptyWorld(
                    new Ecosystem(
                        dataReader.GetInt32("ecosystemId"),
                        dataReader.GetString("ecosystemName"),
                        dataReader.GetString("image"),
                        dataReader.GetFloat("difficulty")
                    ),
                    dataReader.GetDateTime("dateEnded"),
                    new RandomWeatherManager()
                )
            );
        }

        private Mutation CreateMutation(SqlDataReader dataReader)
        {
            return new Mutation(
                mutationTypeDatabaseMapping[dataReader.GetString("type")],
                dataReader.GetString("allel"),
                dataReader.GetString("proteinBefore"),
                dataReader.GetString("proteinAfter"),
                dataReader.GetDateTime("date"),
                new List<StatisticInfo>()
                {
                    new StatisticInfo(
                        dataReader.GetString("statistic"),
                        ColorHelper.ToColor(dataReader.GetString("name")),
                        dataReader.GetInt32("affection")
                    )
                }
            );
        }

        private PopulationCount CreatePopulationCount(SqlDataReader dataReader)
        {
            return new PopulationCount(
                dataReader.GetDateTime("date"),
                new Species(
                    dataReader.GetInt32("id"),
                    dataReader.GetString("name"),
                    dataReader.GetString("description"),
                    dataReader.GetString("scientificName"),
                    dataReader.GetString("image"),
                    dataReader.GetInt32("averageAge"),
                    dataReader.GetInt32("maxAge"),
                    dataReader.GetInt32("minBreedSize"),
                    dataReader.GetInt32("maxBreedSize"),
                    dietDatabaseMapping[dataReader.GetString("diet")]
                ),
                dataReader.GetInt32("amountOfAnimals")
            );
        }
    }
}
