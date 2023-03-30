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
    public class DatabaseSpeciesWriter : ISpeciesWriter // TODO alle constante db namen moeten weg en gwn in de query genoemd worden
    {
        private static readonly string DATABASE_TABLE_NAME = "Species";
        private DatabaseHelper<Species> _database;

        public DatabaseSpeciesWriter()
        {
            _database = new DatabaseHelper<Species>();
        }

        public void Create(Species species, int ecosystemId)
        {
            string query = "INSERT INTO " + DATABASE_TABLE_NAME + "(name, image, description, scientificName, averageAge, maxAge, maxBreedSize, minBreedSize, diet, aggression, detection, selfDefence, intelligence, metabolicRate, resilience, size, weight, speed) OUTPUT INSERTED.ID VALUES (@name, @image, @description, @scientificName, @averageAge, @maxAge, @maxBreedSize, @minBreedSize, @diet, @aggression, @detection, @selfDefence, @intelligence, @metabolicRate, @resilience, @size, @weight, @speed)";
            
            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@name", species.Name),
                new SqlParameter("@image", species.Image),
                new SqlParameter("@description", species.Description),
                new SqlParameter("@scientificName", species.ScientificName),
                new SqlParameter("@averageAge", species.AverageAge),
                new SqlParameter("@maxAge", species.MaxAge),
                new SqlParameter("@maxBreedSize", species.MaxBreedSize),
                new SqlParameter("@minBreedSize", species.MinBreedSize),
                new SqlParameter("@diet", species.Diet.ToString()),
                new SqlParameter("@aggression", species.BaseStatistics.Aggresion),
                new SqlParameter("@detection", species.BaseStatistics.Detection),
                new SqlParameter("@selfDefence", species.BaseStatistics.SelfDefence),
                new SqlParameter("@intelligence", species.BaseStatistics.Intelligence),
                new SqlParameter("@metabolicRate", species.BaseStatistics.MetabolicRate),
                new SqlParameter("@resilience", species.BaseStatistics.Resilience),
                new SqlParameter("@size", species.BaseStatistics.Size),
                new SqlParameter("@weight", species.BaseStatistics.Weight),
                new SqlParameter("@speed", species.BaseStatistics.Speed)
            };
            int speciesId = (int)_database.ExecuteQueryScalar(query, CommandType.Text, parameters);

            query = "INSERT INTO [EcosystemSpecies] (ecosystemId, speciesId) VALUES (@eco, @spec)";
            parameters = new List<SqlParameter>
            {
                new SqlParameter("@eco", ecosystemId),
                new SqlParameter("@spec", speciesId)
            };

            _database.ExecuteQuery(query, CommandType.Text, parameters);
        }
    }
}
