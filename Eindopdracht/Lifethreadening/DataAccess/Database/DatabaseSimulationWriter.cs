using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace Lifethreadening.DataAccess.Database
{
    public class DatabaseSimulationWriter : ISimulationWriter
    {
        private DatabaseHelper<Simulation> _database;

        public DatabaseSimulationWriter()
        {
            _database = new DatabaseHelper<Simulation>();
        }

        public async Task Write(Simulation simulation)
        {
            if (simulation.Id == 0)
            {
                CreateSimulation(simulation);
            }
            else
            {
                UpdateSimulation(simulation);
            }

            await UpdateMutations(simulation);
            await UpdatePopulationCount(simulation);
        }

        // TODO ASYNC of SYNC
        private void CreateSimulation(Simulation simulation)
        {
            string query = "INSERT INTO Simulation(dateStarted, dateEnded, ecosystemId, name, fileNameSaveSlot)" + 
                "VALUES (@dateStarted, @dateEnded, @ecosystemId, @name, @fileNameSaveSlot)";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@dateStarted", simulation.StartDate.ToString()),
                new SqlParameter("@dateEnded", simulation.World.CurrentDate.ToString()),
                new SqlParameter("@ecosystemId", simulation.World.Ecosystem.Id),
                new SqlParameter("@name", simulation.Name),
                new SqlParameter("@fileNameSaveSlot", simulation.Filename)
            };
            simulation.Id = (int)_database.ExecuteQueryScalar(query, CommandType.Text, parameters);
        }

        private void UpdateSimulation(Simulation simulation)
        {
            string query = "UPDATE Simulation SET score = @score, dateEnded = @dateEnded, amountOfDisasters = @amountOfDisasters, " + 
                "fileNameSaveSlot = @fileNameSaveSlot WHERE id = @id";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@score", simulation.Score),
                new SqlParameter("@dateEnded", simulation.World.CurrentDate.ToString()),
                new SqlParameter("@amountOfDisasters", simulation.AmountOfDisasters),
                new SqlParameter("@fileNameSaveSlot", simulation.Filename),
                new SqlParameter("@id", simulation.Id)
            };
            _database.ExecuteQuery(query, CommandType.Text, parameters);
        }

        private async Task UpdateMutations(Simulation simulation)
        {
            DeleteMutations(simulation);
            await CreateMutations(simulation);
            await UpdateAffectedStatistics(simulation);
        }

        private void DeleteMutations(Simulation simulation) // TODO deze wel async?
        {
            string query = "DELETE FROM Mutation WHERE simulationId = @simulationId";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@simulationId", simulation.Id),
            };
            _database.ExecuteQuery(query, CommandType.Text, parameters);
        }

        private async Task CreateMutations(Simulation simulation)
        {
            DataTable dataTable = CreateDataTableForMutations(simulation.MutationManager.Mutations, simulation.Id);
            await BulkInsertEntity(dataTable, "Mutation");
        }

        private async Task BulkInsertEntity(DataTable dataTable, string tableName)
        {
            if (dataTable.Rows.Count > 0)
            {
                await _database.BulkInsertAsync(dataTable, tableName);
            }
        }

        private DataTable CreateDataTableForMutations(IEnumerable<Mutation> mutations, int simulationId)
        {
            DataTable newMutationsTable = new DataTable();
            newMutationsTable.Columns.Add(new DataColumn("type"));
            newMutationsTable.Columns.Add(new DataColumn("allel"));
            newMutationsTable.Columns.Add(new DataColumn("proteinBefore"));
            newMutationsTable.Columns.Add(new DataColumn("proteinAfter"));
            newMutationsTable.Columns.Add(new DataColumn("simulationId"));
            newMutationsTable.Columns.Add(new DataColumn("date"));

            foreach (Mutation newMutation in mutations)
            {
                newMutationsTable.Rows.Add(CreateDataRowForMutation(newMutation, newMutationsTable, simulationId));
            }

            return newMutationsTable;
        }

        private DataRow CreateDataRowForMutation(Mutation mutation, DataTable mutationsTable, int simulationId)
        {
            DataRow row = mutationsTable.NewRow();

            row["type"] = mutation.Type.ToString();
            row["allel"] = mutation.Allel;
            row["proteinBefore"] = mutation.ProteinBefore;
            row["proteinAfter"] = mutation.ProteinAfter;
            row["simulationId"] = simulationId;
            row["date"] = mutation.MutationDate;

            return row;
        }

        private async Task UpdateAffectedStatistics(Simulation simulation)
        {
            DeleteAffectedStatistics(simulation);
            await CreateAffectedStatistics(simulation);
        }

        private void DeleteAffectedStatistics(Simulation simulation) // TODO deze wel async?
        {
            string query = "DELETE FROM MutationAffectedStatistic WHERE simulationId = @simulationId";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@simulationId", simulation.Id),
            };
            _database.ExecuteQuery(query, CommandType.Text, parameters);
        }

        private async Task CreateAffectedStatistics(Simulation simulation)
        {

            DataTable dataTable = CreateDataTableForAffectedStatistics(simulation.MutationManager.Mutations, simulation.Id);
            await BulkInsertEntity(dataTable, "MutationAffectedStatistic");
        }

        private DataTable CreateDataTableForAffectedStatistics(IEnumerable<Mutation> mutations, int simulationId)
        {
            DataTable newAffectedStatisticsTable = new DataTable();
            newAffectedStatisticsTable.Columns.Add(new DataColumn("id"));
            newAffectedStatisticsTable.Columns.Add(new DataColumn("statistic"));
            newAffectedStatisticsTable.Columns.Add(new DataColumn("affection"));
            newAffectedStatisticsTable.Columns.Add(new DataColumn("mutationAllel"));
            newAffectedStatisticsTable.Columns.Add(new DataColumn("simulationId"));
            newAffectedStatisticsTable.Columns.Add(new DataColumn("mutationDate"));
            newAffectedStatisticsTable.Columns.Add(new DataColumn("color"));

            foreach (Mutation newMutation in mutations)
            {
                foreach(StatisticInfo statistic in newMutation.Affected)
                {
                    newAffectedStatisticsTable.Rows.Add(CreateDataRowForAffectedStatistics(statistic, newMutation, newAffectedStatisticsTable, simulationId));
                }
            }

            return newAffectedStatisticsTable;
        }

        private DataRow CreateDataRowForAffectedStatistics(StatisticInfo statistic, Mutation mutation, DataTable affectedStatisticsTable, int simulationId)
        {
            DataRow row = affectedStatisticsTable.NewRow();

            row["statistic"] = statistic.Name;
            row["affection"] = statistic.Value;
            row["mutationAllel"] = mutation.Allel;
            row["simulationId"] = simulationId;
            row["mutationDate"] = mutation.MutationDate;
            row["color"] = statistic.Color.ToString();

            return row;
        }

        private async Task UpdatePopulationCount(Simulation simulation)
        {
            DeletePopulationCount(simulation);
            await CreatePopulationCount(simulation);
        }

        private void DeletePopulationCount(Simulation simulation) // TODO deze wel async?
        {
            string query = "DELETE FROM SimulationPopulation WHERE simulationId = @simulationId";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@simulationId", simulation.Id),
            };
            _database.ExecuteQuery(query, CommandType.Text, parameters);
        }

        private async Task CreatePopulationCount(Simulation simulation)
        {
            DataTable dataTable = CreateDataTableForPopulationCount(simulation.PopulationManager.SpeciesCount, simulation.Id);
            await BulkInsertEntity(dataTable, "SimulationPopulation");
        }

        private DataTable CreateDataTableForPopulationCount(IDictionary<DateTime, IDictionary<Species, int>> populationCount, int simulationId)
        {
            DataTable newPopulationCountTable = new DataTable();
            newPopulationCountTable.Columns.Add(new DataColumn("id"));
            newPopulationCountTable.Columns.Add(new DataColumn("speciesId"));
            newPopulationCountTable.Columns.Add(new DataColumn("simulationId"));
            newPopulationCountTable.Columns.Add(new DataColumn("amountOfAnimals"));
            newPopulationCountTable.Columns.Add(new DataColumn("date"));

            foreach (KeyValuePair<DateTime, IDictionary<Species, int>> countPerDate in populationCount)
            {
                foreach(KeyValuePair<Species, int> countPerSpecies in countPerDate.Value)
                {
                    newPopulationCountTable.Rows.Add(CreateDataRowForPopulationCount(countPerDate.Key, countPerSpecies.Key, countPerSpecies.Value, 
                        simulationId, newPopulationCountTable));
                }
            }

            return newPopulationCountTable;
        }

        private DataRow CreateDataRowForPopulationCount(DateTime date, Species species, int amountOfAnimals, int simulationId, DataTable populationCountTable)
        {
            DataRow row = populationCountTable.NewRow();

            row["speciesId"] = species.Id;
            row["simulationId"] = simulationId;
            row["amountOfAnimals"] = amountOfAnimals;
            row["date"] = date;

            return row;
        }
    }
}
