using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Lifethreadening.ExtensionMethods
{
    /// <summary>
    /// This class is used to contain a set of extension methods for objects of type sqlDataReader
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// This method reads a value as a big int
        /// </summary>
        /// <param name="sqlDataReader">The reader to use</param>
        /// <param name="key">The key to get the value for</param>
        /// <returns>The value associated with the key in the format of an int32</returns>
        public static int GetInt32(this SqlDataReader sqlDataReader, string key)
        {
            return sqlDataReader.GetInt32(sqlDataReader.GetOrdinal(key));
        }

        /// <summary>
        /// This method reads a value as a date time
        /// </summary>
        /// <param name="sqlDataReader">The reader to use</param>
        /// <param name="key">The key to get the value for</param>
        /// <returns>The value associated with the key in the format of an datetime</returns>
        public static DateTime GetDateTime(this SqlDataReader sqlDataReader, string key)
        {
            return sqlDataReader.GetDateTime(sqlDataReader.GetOrdinal(key));
        }

        /// <summary>
        /// This method reads a value as a string
        /// </summary>
        /// <param name="sqlDataReader">The reader to use</param>
        /// <param name="key">The key to get the value for</param>
        /// <returns>The value associated with the key in the format of an string</returns>
        public static string GetString(this SqlDataReader sqlDataReader, string key)
        {
            return sqlDataReader[key].ToString();
        }

        /// <summary>
        /// This method reads a value as a double
        /// </summary>
        /// <param name="sqlDataReader">The reader to use</param>
        /// <param name="key">The key to get the value for</param>
        /// <returns>The value associated with the key in the format of an double</returns>
        public static double GetDouble(this SqlDataReader sqlDataReader, string key)
        {
            return sqlDataReader.GetDouble(sqlDataReader.GetOrdinal(key));
        }
    }
}
