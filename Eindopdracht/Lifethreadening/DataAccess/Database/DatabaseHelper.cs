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
    /// <summary>
    /// This class stores serveral methods used to access the database
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DatabaseHelper<T>
    {
        /// <summary>
        /// This methods executes the given query
        /// </summary>
        /// <param name="commandText">The SQL command</param>
        /// <param name="commandType">The type of command this is</param>
        /// <param name="commandParameters">The command parameters</param>
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

        /// <summary>
        /// This method executes a given query and retrieves the first value in the first row and column
        /// </summary>
        /// <param name="commandText">The SQL command</param>
        /// <param name="commandType">The type of command this is</param>
        /// <param name="commandParameters">The command parameters</param>
        /// <returns> the first value in the first row and column</returns>
        public object ExecuteQueryScalar(string commandText, CommandType commandType, IEnumerable<SqlParameter> commandParameters)
        {
            object returnValue = null;
            using (var connection = DatabaseConnectionManager.GetSqlConnection())
            {
                connection.Open();
                using (var command = new SqlCommand(commandText, connection))
                {
                    command.CommandType = commandType;
                    command.Parameters.AddRange(commandParameters.ToArray());
                    returnValue = command.ExecuteScalar();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// This method Executes a query async
        /// </summary>
        /// <param name="commandText">The SQL command</param>
        /// <param name="commandType">The type of command this is</param>
        /// <param name="commandParameters">The command parameters</param>
        /// <returns>A taks of the executing query</returns>
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

        /// <summary>
        /// This method is used to insert large datatables all at once
        /// </summary>
        /// <param name="dataTable">The data to insert</param>
        /// <param name="tableName">The table to insert it into</param>
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

        /// <summary>
        /// This method is used to insert large datatables all at once async
        /// </summary>
        /// <param name="dataTable">The data to insert</param>
        /// <param name="tableName">The table to insert it into</param>
        /// <returns>A taks of the executing insert</returns>
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

        /// <summary>
        /// This function reads data from the database
        /// </summary>
        /// <param name="objectBuilder">The object builder to build an object</param>
        /// <param name="commandText">The command to execute</param>
        /// <param name="commandType">The type of command this is</param>
        /// <param name="commandParameters">The parameters for the command</param>
        /// <returns>A list of objects read from the DaataBase</returns>
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
                        while (reader.Read())
                        {
                            collection.Add(objectBuilder(reader));
                        }
                    }
                }
            }
            return collection;
        }

        /// <summary>
        /// This function Reads data from the DataBase async
        /// </summary>
        /// <param name="objectBuilder">The object builder to build an object</param>
        /// <param name="commandText">The command to execute</param>
        /// <param name="commandType">The type of command this is</param>
        /// <param name="commandParameters">The parameters for the command</param>
        /// <returns>A task contianing the executing read function</returns>
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
                        while (reader.Read())
                        {
                            collection.Add(objectBuilder(reader));
                        }
                    }
                }
            }
            return collection;
        }
    }
}
