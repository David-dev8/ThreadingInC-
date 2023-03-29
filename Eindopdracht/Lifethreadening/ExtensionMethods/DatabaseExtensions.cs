using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Lifethreadening.ExtensionMethods
{
    public static class DatabaseExtensions
    {
        // TODO of cachen van ordinals
        public static int GetInt32(this SqlDataReader sqlDataReader, string key)
        {
            return sqlDataReader.GetInt32(sqlDataReader.GetOrdinal(key));
        }

        public static string GetString(this SqlDataReader sqlDataReader, string key)
        {
            return sqlDataReader[key].ToString();
        }
    }
}
