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
        // TODO ASYNC of SYNC
        public void Create(string saveSlotLocation, Simulation simulation)
        {
            DatabaseHelper<Simulation> database = new DatabaseHelper<Simulation>();
            string query = "INSERT INTO Simulation(dateStarted, ecosystemId, name, fileNameSaveSlot)" + 
                "VALUES (@dateStarted, @ecosystemId, @name, @fileNameSaveSlot)";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@dateStarted", simulation.World.StartDate),
                new SqlParameter("@ecosystemId", simulation.World.Ecosystem.Id),
                new SqlParameter("@name", simulation.Name),
                new SqlParameter("@fileNameSaveSlot", saveSlotLocation)
            };
            database.ExecuteQuery(query, CommandType.Text, parameters);
        }

        public async Task Update(Simulation simulation)
        {
            await UpdateMutations(simulation);
        }

        private async Task UpdateMutations(Simulation simulation)
        {
            DeleteMutations(simulation);
            await CreateMutations(simulation);
        }

        private void DeleteMutations(Simulation simulation) // TODO deze wel async?
        {
            DatabaseHelper<Mutation> database = new DatabaseHelper<Mutation>();
            string query = "Delete FROM Mutation WHERE simulationId = @simulationId";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@simulationId", simulation.Id),
            };
            database.ExecuteQuery(query, CommandType.Text, parameters);
        }

        // TODO simulation bij elke methode, wat te doen
        private async Task CreateMutations(Simulation simulation)
        {
            DatabaseHelper<Mutation> database = new DatabaseHelper<Mutation>();// TODO database overal
            await database.BulkInsertAsync(CreateDataTableForMutations(simulation.MutationManager.Mutations, simulation.Id), "Mutation");
            
        }

        private DataTable CreateDataTableForMutations(IEnumerable<Mutation> mutations, int simulationId)
        {
            DataTable newMutationsTable = new DataTable();
            newMutationsTable.Columns.Add(new DataColumn("id"));
            newMutationsTable.Columns.Add(new DataColumn("type"));
            newMutationsTable.Columns.Add(new DataColumn("allel"));
            newMutationsTable.Columns.Add(new DataColumn("proteinBefore"));
            newMutationsTable.Columns.Add(new DataColumn("proteinAfter"));
            newMutationsTable.Columns.Add(new DataColumn("simulationId"));
            newMutationsTable.Columns.Add(new DataColumn("Date"));

            foreach (Mutation newMutation in mutations)
            {
                newMutationsTable.Rows.Add(CreateDataRowForMutation(newMutation, newMutationsTable, simulationId));
            }

            return newMutationsTable;
        }

        private DataRow CreateDataRowForMutation(Mutation mutation, DataTable mutationsTable, int simulationId)
        {
            DataRow row = mutationsTable.NewRow();

            row["type"] = mutation.Type;
            row["allel"] = mutation.Allel;
            row["proteinBefore"] = mutation.ProteinBefore;
            row["proteinAfter"] = mutation.ProteinAfter;
            row["simulationId"] = simulationId;
            row["Date"] = mutation.MutationDate;

            return row;
        }
    }
}
