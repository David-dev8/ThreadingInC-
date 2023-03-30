using Lifethreadening.DataAccess.Algorithmic;
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
        public IEnumerable<Simulation> ReadAll() 
        {
            DatabaseHelper<Simulation> database = new DatabaseHelper<Simulation>();
            string query = @"
                SELECT S.*, E.id AS ecosystemId, E.name AS ecosystemName, E.image, E.difficulty
                FROM Simulation AS S
                JOIN Ecosystem AS E ON E.id = S.ecosystemId";
            return database.Read(CreateSimulation, query, CommandType.Text);
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
    }
}
