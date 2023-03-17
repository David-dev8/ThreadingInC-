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
        private static readonly IDictionary<string, Diet> dietDatabaseMapping = new Dictionary<string, Diet>()
        {
            { "Herbivore", Diet.HERBIVORE },
            { "Carnivore", Diet.CARNIVORE },
            { "Omnivore", Diet.HERBIVORE }
        };
        private static readonly string DATABASE_TABLE_NAME = "Species";
        private DatabaseHelper<Species> _database;

        public DatabaseSpeciesReader()
        {
            _database = new DatabaseHelper<Species>();
        }

        public IEnumerable<Species> ReadAll()
        {
            string query = "SELECT * FROM " + DATABASE_TABLE_NAME;
            return _database.Read(CreateSpecies, query, CommandType.Text);
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
            return _database.Read(CreateSpecies, query, CommandType.Text, parameters);
        }

        private Species CreateSpecies(SqlDataReader dataReader)
        {
            return new Species(
                dataReader.GetInt32("id"),
                dataReader.GetString("name"),
                dataReader.GetString("description"),
                dataReader.GetString("scientificName"),
                dataReader.GetString("image"),
                dataReader.GetInt32("averageAge"),
                dataReader.GetInt32("maxAge"),
                dataReader.GetInt32("maxBreedSize"),
                dataReader.GetInt32("minBreedSize"),
                dietDatabaseMapping[dataReader.GetString("diet")],
                new Statistics()
                {
                    Aggresion = dataReader.GetInt32("aggression"),
                    Detection = dataReader.GetInt32("detection"),
                    SelfDefence = dataReader.GetInt32("selfDefence"),
                    Intelligence = dataReader.GetInt32("intelligence"),
                    MetabolicRate = dataReader.GetInt32("metabolicRate"),
                    Resilience = dataReader.GetInt32("resilience"),
                    Size = dataReader.GetInt32("size"),
                    Weight = dataReader.GetInt32("weight"),
                    Speed = dataReader.GetInt32("speed")
                }
            );
        }
    }
}
