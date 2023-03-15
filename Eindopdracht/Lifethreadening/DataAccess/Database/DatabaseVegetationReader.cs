using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Lifethreadening.ExtensionMethods;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Database
{
    public class DatabaseVegetationReader : IVegetationReader
    {
        private DatabaseHelper<Vegetation> _database;

        public DatabaseVegetationReader()
        {
            _database = new DatabaseHelper<Vegetation>();
        }

        public IEnumerable<Vegetation> ReadByEcosystem(int ecosystemId)
        {
            string query = @"
                SELECT *
                FROM Vegetation
                WHERE id IN (
	                SELECT vegetationId
	                FROM EcosystemVegetation
	                WHERE ecosystemId = @ecosystemId
                )
            ";
            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ecosystemId", ecosystemId),
            };
            return _database.Read(CreateVegetation, query, CommandType.Text, parameters);
        }

        private Vegetation CreateVegetation(SqlDataReader dataReader)
        {
            return new Vegetation(
                dataReader.GetString("image"),
                dataReader.GetInt32("growth"),
                dataReader.GetInt32("maxNutrition"));
        }
    }
}
