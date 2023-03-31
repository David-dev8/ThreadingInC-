using Lifethreadening.DataAccess.Algorithmic;
using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models;
using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Database
{
    /// <summary>
    /// This class retrieves data about simulations from the database 
    /// </summary>
    public class DatabaseSimulationReader : ISimulationReader
    {
        private static readonly IDictionary<int, Species> speciesCache = new Dictionary<int, Species>();
        
        private static readonly IDictionary<string, MutationType> mutationTypeDatabaseMapping = new Dictionary<string, MutationType>()
        {
            { "Addition", MutationType.ADDITION },
            { "Substitution", MutationType.SUBSTITUTION },
            { "Delocation", MutationType.DELOCATION },
        };

        private static readonly IDictionary<string, Diet> dietDatabaseMapping = new Dictionary<string, Diet>()
        {
            { "Omnivore", Diet.OMNIVORE },
            { "Carnivore", Diet.CARNIVORE },
            { "Herbivore", Diet.HERBIVORE }
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

        public async Task<Simulation> ReadFullDetails(Simulation simulation)
        {
            simulation.PopulationManager.SpeciesCount = await GetSpeciesCount(simulation);
            simulation.MutationManager.Mutations = await GetMutations(simulation);
            return simulation;
        }

        private async Task<ISet<Mutation>> GetMutations(Simulation simulation)
        {
            IEnumerable<Mutation> mutations = await RetrieveMutations(simulation);
            return mutations.GroupBy(mutation => mutation, mutation => mutation.Affected).Select(mutation =>
            {
                mutation.Key.Affected = mutation.SelectMany(affected => affected).ToList();
                return mutation.Key;
            }).ToHashSet();
        }

        private async Task<IEnumerable<Mutation>> RetrieveMutations(Simulation simulation)
        {
            DatabaseHelper<Mutation> database = new DatabaseHelper<Mutation>();
            string query = @"
                SELECT M.*, MAS.statistic, MAS.affection, MAS.color
                FROM Mutation AS M
                JOIN MutationAffectedStatistic AS MAS ON M.allel = MAS.mutationAllel AND M.date = MAS.mutationDate AND M.simulationId = MAS.simulationId
                WHERE M.simulationId = @simulationId";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@simulationId", simulation.Id),
            };
            return await database.ReadAsync(CreateMutation, query, CommandType.Text, parameters);
        }

        private async Task<Dictionary<DateTime, IDictionary<Species, int>>> GetSpeciesCount(Simulation simulation)
        {
            IEnumerable<PopulationCount> populationCounts = await RetrievePopulationCounts(simulation);
            return populationCounts.GroupBy(populationCount => populationCount.Date)
                .ToDictionary(populationCount => populationCount.Key, populationCount => (IDictionary<Species, int>)populationCount.ToDictionary(speciesCount => speciesCount.Species,
                speciesCount => speciesCount.AmountOfAnimals));
        }

        private async Task<IEnumerable<PopulationCount>> RetrievePopulationCounts(Simulation simulation)
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

            return await database.ReadAsync(CreatePopulationCount, query, CommandType.Text, parameters);
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
                        dataReader.GetDouble("difficulty")
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
                        ColorHelper.ToColor(dataReader.GetString("color")),
                        dataReader.GetInt32("affection")
                    )
                }
            );
        }

        private PopulationCount CreatePopulationCount(SqlDataReader dataReader)
        {
            Species species;
            int id = dataReader.GetInt32("id");
            if (speciesCache.ContainsKey(id))
            {
                species = speciesCache[id];
            }
            else
            {
                species = new Species(
                        dataReader.GetInt32("id"),
                        dataReader["name"].ToString(),
                        dataReader["description"].ToString(),
                        dataReader["scientificName"].ToString(),
                        dataReader["image"].ToString(),
                        dataReader.GetInt32("averageAge"),
                        dataReader.GetInt32("maxAge"),
                        dataReader.GetInt32("minBreedSize"),
                        dataReader.GetInt32("maxBreedSize"),
                        dietDatabaseMapping[dataReader.GetString("diet")]
                    );
                speciesCache.Add(id, species);
            }
            
            return new PopulationCount(
                dataReader.GetDateTime("date"),
                species,
                dataReader.GetInt32("amountOfAnimals")
            );
        }
    }
}
