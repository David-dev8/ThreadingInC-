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
    /// <summary>
    /// This class retrieves data about obstructions from the database
    /// </summary>
    public class DatabaseObstructionReader : IObstructionReader
    {
        private DatabaseHelper<Obstruction> _database;

        public DatabaseObstructionReader()
        {
            _database = new DatabaseHelper<Obstruction>();
        }

        public IEnumerable<Obstruction> ReadByEcosystem(int ecosystemId, WorldContextService contextService)
        {
            string query = @"
                SELECT *
                FROM Obstruction
                WHERE id IN (
	                SELECT obstructionId
	                FROM EcosystemObstruction
	                WHERE ecosystemId = @ecosystemId
                )
            ";
            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ecosystemId", ecosystemId),
            };
            return _database.Read((dataReader) => CreateObstruction(dataReader, contextService), query, CommandType.Text, parameters);
        }

        private Obstruction CreateObstruction(SqlDataReader dataReader, WorldContextService service)
        {
            return new Obstruction(
                dataReader.GetString("image"),
                service
            );
        }
    }
}
