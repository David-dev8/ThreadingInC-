using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace Lifethreadening.DataAccess.Database
{
    public class DatabaseHelper<T>
    {
        // TODO try-catch voor elke methode toevoegen
        public void ExecuteQuery(string commandText, CommandType commandType, IEnumerable<SqlParameter> commandParameters)
        {
            using (var connection = DatabaseConnectionManager.GetSqlConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.Parameters.AddRange(commandParameters.ToArray());
                    command.ExecuteNonQuery();
                }
            }   
        }

        public async Task ExecuteQueryAsync(string commandText, CommandType commandType, IEnumerable<SqlParameter> commandParameters)
        {
            using (var connection = DatabaseConnectionManager.GetSqlConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.Parameters.AddRange(commandParameters.ToArray());
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public void BulkInsert(DataTable dataTable, string tableName)
        {
            using (var connection = DatabaseConnectionManager.GetSqlConnection())
            {
                connection.Open();
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }

        public async Task BulkInsertAsync(DataTable dataTable, string tableName)
        {
            using (var connection = DatabaseConnectionManager.GetSqlConnection())
            {
                await connection.OpenAsync();
                using (var bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = tableName;
                    await bulkCopy.WriteToServerAsync(dataTable);
                }
            }
        }

        public IEnumerable<T> Read(Func<SqlDataReader, T> objectBuilder, string commandText, CommandType commandType, 
            IEnumerable<SqlParameter> commandParameters = null)
        {
            commandParameters = commandParameters ?? new List<SqlParameter>();
            IList<T> collection = new List<T>();
            using (var connection = DatabaseConnectionManager.GetSqlConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.Parameters.AddRange(commandParameters.ToArray());
                    using (var reader = command.ExecuteReader())
                    {
                        // TODO Linq werkt niet
                        while (reader.Read())
                        {
                            collection.Add(objectBuilder(reader));
                        }
                        //reader.Select(entity => objectBuilder(entity)).ToList();
                    }
                }
            }
            return collection;
        }

        public async Task<IEnumerable<T>> ReadAsync(Func<SqlDataReader, T> objectBuilder, string commandText, CommandType commandType,
            IEnumerable<SqlParameter> commandParameters = null)
        {
            commandParameters = commandParameters ?? new List<SqlParameter>();
            IList<T> collection = new List<T>();
            using (var connection = DatabaseConnectionManager.GetSqlConnection())
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.Parameters.AddRange(commandParameters.ToArray());
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // TODO Linq werkt niet
                        while (reader.Read())
                        {
                            collection.Add(objectBuilder(reader));
                        }
                        //reader.Select(entity => objectBuilder(entity)).ToList();
                    }
                }
            }
            return collection;
        }
    }
}
