using Lifethreadening.Models;
using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        //private Simulation CreateSimulation(SqlDataReader dataReader)
        //{
            //return new Simulation(
            //    dataReader.GetInt32("id"),
            //    dataReader.GetString("name"),
            //    dataReader.GetString("description"),
            //    dataReader.GetString("scientificName"),
            //    dataReader.GetString("image"),
            //    dataReader.GetInt32("averageAge"),
            //    dataReader.GetInt32("maxAge"),
            //    dataReader.GetInt32("maxBreedSize"),
            //    dataReader.GetInt32("minBreedSize"),
            //);
        //}
    }
}
