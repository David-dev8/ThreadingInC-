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
    public class DatabaseSpeciesWriter : ISpeciesWriter
    {
        private static readonly string DATABASE_TABLE_NAME = "Species";
        private DatabaseHelper<Species> _database;

        public DatabaseSpeciesWriter()
        {
            _database = new DatabaseHelper<Species>();
        }
        
        public void Create(Species species)
        {
            string query = "INSERT INTO " + DATABASE_TABLE_NAME + "(name, image) VALUES (@name, @image)";
            IEnumerable<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@name", species.Name),
                new SqlParameter("@image", species.Image)
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
