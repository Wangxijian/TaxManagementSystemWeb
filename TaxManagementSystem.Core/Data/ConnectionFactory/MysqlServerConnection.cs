namespace TaxManagementSystem.Core.Data.ConnectionFactory
{
    using MySql.Data.MySqlClient;
    using System.Data;
    using TaxManagementSystem.Core.Data.Connection;

    public class MysqlServerConnection
    {
        /// <summary>
        /// conn
        /// </summary>
        /// <returns></returns>
        public static IDbConnection Conn
        {
            get { return new MySqlConnection() { ConnectionString = MysqlDBConnection.Current.ToString() }; }
        }

    }
}
