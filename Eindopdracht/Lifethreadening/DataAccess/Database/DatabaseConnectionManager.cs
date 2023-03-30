using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Database
{
    /// <summary>
    /// This class stores data partaining to how to set up the connection to the database
    /// </summary>
    public static class DatabaseConnectionManager
    {
        // TODO naar config?
        private static readonly string CONNECTION_STRING = @"Server=.;Database=LifeThreadening;Integrated Security=true";

        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(CONNECTION_STRING);
        }
    }
}
