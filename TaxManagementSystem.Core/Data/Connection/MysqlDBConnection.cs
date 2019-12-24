namespace TaxManagementSystem.Core.Data.Connection
{
    using Component;
    using System;
    using MySql;
    using MySql.Data;
    using MySql.Data.MySqlClient;

    public sealed partial class MysqlDBConnection
    {
        private static MysqlDBConnection DB_CONNECTION = null;

        public static MysqlDBConnection Current
        {
            get
            {
                if (DB_CONNECTION == null)
                {
                    DB_CONNECTION = new MysqlDBConnection();
                }
                return DB_CONNECTION;
            }
        }

        private MysqlDBConnection() { }
    }

    public partial class MysqlDBConnection
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public string Server
        {
            get;
            set;
        }
        /// <summary>
        /// 数据库
        /// </summary>
        public string Database
        {
            get;
            set;
        }
        /// <summary>
        /// 登陆用户
        /// </summary>
        public string LoginUser
        {
            get;
            set;
        }
        /// <summary>
        /// 登陆密码
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        public override string ToString()
        {
            
            return string.Format("Server={0};Database={1};Uid={2};Pwd={3};",
                this.Server, this.Database, this.LoginUser, this.Password);
        }

        public static implicit operator string(MysqlDBConnection value)
        {
            if (value == null)
                return null;
            return value.ToString();
        }

        private const string WORK_KEY_NAME = "MysqlDBConnection";

        public MySqlConnection Deployment()
        {
            WorkThreadDictionary work = WorkThreadDictionary.Get();
            MySqlConnection connection = null;
            if (work != null)
            {
                connection = work.Get<MySqlConnection>(WORK_KEY_NAME);
            }
            if (connection == null)
            {
                connection = new MySqlConnection();
                if (work != null)
                {
                    work.Set(WORK_KEY_NAME, connection);
                }
                //connection.FireInfoMessageEventOnUserErrors = true;
                connection.Disposed += Connection_Disposed;
                //connection.InfoMessage += Connection_InfoMessage;
            }
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.ConnectionString = this.ToString();
            }
            return connection;
        }

        private static void Connection_InfoMessage(object sender, MySqlInfoMessageEventArgs e)
        {
            using (MySqlConnection connection = (MySqlConnection)sender)
            {
                Connection_Disposed(sender, e);
            }
        }

        public static implicit operator MySqlConnection(MysqlDBConnection value)
        {
            if (value == null)
            {
                return null;
            }
            return value.Deployment();
        }

        private static void Connection_Disposed(object sender, EventArgs e)
        {
            WorkThreadDictionary work = WorkThreadDictionary.Get();
            if (work != null)
            {
                MySqlConnection connection = new MySqlConnection();
                work.Set(WORK_KEY_NAME, connection);
            }
        }

    }
}
