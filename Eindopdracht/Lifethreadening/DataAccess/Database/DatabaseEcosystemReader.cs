using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Database
{
    /// <summary>
    /// This class retrieves data about ecosystems from the database
    /// </summary>
    public class DatabaseEcosystemReader : IEcosystemReader
    {
        private static readonly string DATABASE_TABLE_NAME = "Ecosystem";
        private DatabaseHelper<Ecosystem> _database;

        public DatabaseEcosystemReader()
        {
            _database = new DatabaseHelper<Ecosystem>();
        }

        public IEnumerable<Ecosystem> ReadAll()
        {
            string query = "SELECT * FROM " + DATABASE_TABLE_NAME;
            return _database.Read(CreateEcosystem, query, System.Data.CommandType.Text);
        }

        private Ecosystem CreateEcosystem(SqlDataReader dataReader)
        {
            return new Ecosystem(
                dataReader.GetInt32("id"),
                dataReader.GetString("name"),
                dataReader.GetString("image"),
                dataReader.GetDouble("difficulty")
            );
        }
    }
}
