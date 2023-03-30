using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models;
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
        public Simulation Read()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Simulation> ReadAll()
        {
            DatabaseHelper<Simulation> database = new DatabaseHelper<Simulation>();
            string query = @"
                SELECT S.*, E.id AS ecosystemId, E.name AS ecosystemName, E.image, E.difficulty
                FROM Simulation AS S
                JOIN Ecosystem AS E ON E.id = S.ecosystemId";

            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                //new SqlParameter("@ecosystemId", ecosystemId),
            };
            return database.Read(CreateSimulation, query, CommandType.Text, parameters);
        }

        private Simulation CreateSimulation(SqlDataReader dataReader)
        {
            return new Simulation(
                dataReader.GetInt32("id"),
                dataReader.GetInt32("score"),
                dataReader.GetDateTime("dateStarted"),
                dataReader.GetDateTime("dateEnded"),
                dataReader.GetInt32("amountOfDisasters"),
                dataReader.GetString("name"),
                dataReader.GetString("fileNameSaveSlot"), // TODO saveslot
                new Ecosystem(
                    dataReader.GetInt32("id"),
                    dataReader.GetString("name"),
                    dataReader.GetString("image"),
                    dataReader.GetFloat("difficulty")
                )
    
            );
        }

        private Sim
    }
}
