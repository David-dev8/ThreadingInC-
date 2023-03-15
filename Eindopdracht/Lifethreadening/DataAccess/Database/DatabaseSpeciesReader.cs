using Lifethreadening.ExtensionMethods;
using Lifethreadening.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls.Primitives;

namespace Lifethreadening.DataAccess.Database
{
    public class DatabaseSpeciesReader : ISpeciesReader
    {
        private static readonly string DATABASE_TABLE_NAME = "Species";
        private DatabaseHelper<Species> _database;

        public DatabaseSpeciesReader()
        {
            _database = new DatabaseHelper<Species>();
        }

        public IEnumerable<Species> ReadAll()
        {
            string query = "SELECT * FROM " + DATABASE_TABLE_NAME;
            return _database.Read(createSpecies, query, CommandType.Text);
        }

        public IEnumerable<Species> ReadByEcosystem(int ecosystemId)
        {
            string query = @"
                SELECT *
                FROM Species
                WHERE id IN (
	                SELECT speciesId
	                FROM EcosystemSpecies
	                WHERE ecosystemId = @ecosystemId
                )
            ";
            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ecosystemId", ecosystemId),
            };
            return _database.Read(createSpecies, query, CommandType.Text, parameters);
        }

        private Species createSpecies(SqlDataReader dataReader)
        {
            return new Species(
                dataReader.GetInt32("id"),
                dataReader.GetString("image"),
                null,
                null,
                0,
                0,
                0,
                0,
                Diet.HERBIVORE,
                new Statistics()
                {
                }
            );
        }
    }
}
