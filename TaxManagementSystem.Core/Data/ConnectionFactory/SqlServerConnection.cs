using TaxManagementSystem.Core.Data.Connection;
using System.Data;
using System.Data.SqlClient;

namespace TaxManagementSystem.Core.Data.ConnectionFactory
{
    public class SqlServerConnection
    {

        /// <summary>
        /// conn
        /// </summary>
        /// <returns></returns>
        public static IDbConnection STDCarFoundationConn
        {
            get { return new SqlConnection() { ConnectionString = DBConnection.Current.ToString() }; }
        }
    }
}
