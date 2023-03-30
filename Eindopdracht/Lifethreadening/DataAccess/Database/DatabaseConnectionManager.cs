using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifethreadening.DataAccess.Database
{
    public static class DatabaseConnectionManager
    {
        private static readonly string CONNECTION_STRING = @"Server=.;Database=LifeThreadening;Integrated Security=true";

        public static SqlConnection GetSqlConnection()
        {
            return new SqlConnection(CONNECTION_STRING);
        }
    }
}
