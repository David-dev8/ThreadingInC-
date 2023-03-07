using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Database
{
    public class DatabaseCustomSpeciesReader : ICustomSpeciesReader
    {
        private static readonly string DATABASE_TABLE_NAME = "Species";
        private DatabaseHelper<Species> _database;

        public DatabaseCustomSpeciesReader()
        {
            _database = new DatabaseHelper<Species>();
        }

        public IEnumerable<Species> ReadAll()
        {
            string query = "SELECT * FROM " + DATABASE_TABLE_NAME;
            return _database.Read(createSpecies, query, System.Data.CommandType.Text);
        }

        private Species createSpecies(SqlDataReader dataReader)
        {
            return new Species()
            {
                Name = dataReader["name"].ToString(),
                Image = dataReader["image"].ToString()
            };
        }
    }
}
