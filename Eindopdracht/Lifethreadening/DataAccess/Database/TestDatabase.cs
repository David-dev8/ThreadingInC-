using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace Lifethreadening.DataAccess.Database
{
    public class TestDatabase
    {
        private const string DB_FILE = "lifethreadeningdata.db";
        private readonly string DB_PATH = Path.Combine(ApplicationData.Current.LocalFolder.Path, DB_FILE);

        public void InitializeDatabase()
        {
            if(!File.Exists(DB_PATH))
            {
                CreateDatabase();
            }
            //using (SqliteConnection db = new SqliteConnection($"Filename={DB_PATH}"))
            //{
            //    db.Open();

            //    String tableCommand = "CREATE TABLE Ecosystem (" +
            //        "id integer NOT NULL PRIMARY KEY AUTOINCREMENT," +
            //        "name varchar(255) NOT NULL," +
            //        "image varchar(255) NOT NULL)";

            //    SqliteCommand createTable = new SqliteCommand(tableCommand, db);

            //    createTable.ExecuteReader();
            //}
        }

        // TODO try-catch???
        private void CreateDatabase()
        {
            var assembly = GetType().Assembly;
            using (var resourceStream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().FirstOrDefault(name => name.EndsWith(DB_FILE))))
            using (var fileStream = new FileStream(DB_PATH, FileMode.Create, FileAccess.Write))
            {
                resourceStream.CopyTo(fileStream);
            }
        }
    }
}
