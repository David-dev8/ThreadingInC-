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

        public void Create(Species species)
        {
            string query = "INSERT INTO " + DATABASE_TABLE_NAME + "(name, image, description, scientificName, averageAge, maxAge, maxBreedSize, minBreedSize, diet, aggression, detection, selfDefence, intelligence, metabolicRate, resilience, size, weight, speed) VALUES (@name, @image, @description, @scientificName, @averageAge, @maxAge, @maxBreedSize, @minBreedSize, @diet, @aggression, @detection, @selfDefence, @intelligence, @metabolicRate, @resilience, @size, @weight, @speed)";
            
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
            _database.ExecuteQuery(query, CommandType.Text, parameters);
        }

        // TODO deze methode en alle andere die te maken hebben met bulk aan het einde van het project weghalen, dit is voor nu een template voor het bulk inserten
        public async Task CreateMultiple(IEnumerable<Species> species)
        {
            await _database.BulkInsertAsync(CreateDataTable(species), DATABASE_TABLE_NAME);
        }

        private DataTable CreateDataTable(IEnumerable<Species> species)
        {
            DataTable newSpeciesTable = new DataTable();
            newSpeciesTable.Columns.Add(new DataColumn("id"));
            newSpeciesTable.Columns.Add(new DataColumn("name"));
            newSpeciesTable.Columns.Add(new DataColumn("image"));

            foreach (Species newSpecies in species) 
            {
                newSpeciesTable.Rows.Add(CreateDataRow(newSpecies, newSpeciesTable));
            }

            return newSpeciesTable;
        }

        private DataRow CreateDataRow(Species species, DataTable speciesTable)
        {
            DataRow row = speciesTable.NewRow();

            row["name"] = species.Name;
            row["image"] = species.Image;

            return row;
        }
    }
}
