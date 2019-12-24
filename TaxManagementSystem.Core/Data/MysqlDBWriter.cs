namespace TaxManagementSystem.Core.Data
{
    using TaxManagementSystem.Core.Data.Connection;
    using System;
    using MySql;
    using MySql.Data;
    using MySql.Data.MySqlClient;
    using System.Text;
    using System.Data;

    /// <summary>
    /// 数据库读写装置
    /// </summary>
    public partial class MysqlDBWriter : IDisposable
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public MySqlConnection Connection
        {
            get;
            private set;
        }
        /// <summary>
        /// 数据库事务
        /// </summary>
        public MySqlTransaction Transaction
        {
            get;
            private set;
        }

        public MysqlDBWriter()
        {
            this.Connection = MysqlDBConnection.Current;
            if (this.Connection.State !=  System.Data.ConnectionState.Open)
            {
                this.Connection.Open();
            }
            this.Transaction = this.Connection.BeginTransaction();
        }

        ~MysqlDBWriter()
        {
            this.Dispose();
        }

        /// <summary>
        /// 回滚写入
        /// </summary>
        public void Rollback()
        {
            if (this.Transaction == null || this.Connection == null)
            {
                throw new InvalidOperationException();
            }
            this.Transaction.Rollback();
        }

        /// <summary>
        /// 提交写入
        /// </summary>
        public void Commit()
        {
            if (this.Transaction == null || this.Connection == null)
            {
                throw new InvalidOperationException();
            }
            this.Transaction.Commit();
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <returns></returns>
        public bool Insert(object value, string table)
        {
            if (value == null || string.IsNullOrEmpty(table))
            {
                throw new ArgumentException();
            }
            using (MySqlCommand cmd = MysqlSDLHelper.CreateInsert(value, table))
            {
                return this.Insert(cmd);
            }
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <param name="procedure">存储过程</param>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public bool Insert(string procedure, object value)
        {
            return this.Update(procedure, value);
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <returns></returns>
        public bool Insert(MySqlCommand cmd, params MySqlParameter[] parameters)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            cmd.Transaction = this.Transaction;
            return MysqlSDLHelper.ExecuteNonQuery(cmd, this.Connection) > 0;
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <returns></returns>
        public object InsertScalar(MySqlCommand cmd, params MySqlParameter[] parameters)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            cmd.Transaction = this.Transaction;
            return MysqlSDLHelper.ExecuteScalar(cmd, this.Connection);
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Insert(StringBuilder sql, params MySqlParameter[] parameters)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql");
            }
            using (MySqlCommand cmd = new MySqlCommand())
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return this.Insert(cmd, parameters);
            }
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <param name="procedure">存储过程</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Insert(MySqlParameter[] parameters, string procedure)
        {
            return this.Update(parameters, procedure);
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public MySqlParameter CreateParameter(string name, object value, DbType? type = null)
        {
            var arg = new MySqlParameter(name, value);
            if (type != null)
                arg.DbType = (DbType)type;
            return arg;
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public bool Insert(string sql, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("sql");
            }
            using (MySqlCommand cmd = new MySqlCommand(sql))
            {
                return this.Insert(cmd, parameters);
            }
        }

        /// <summary>
        /// 插入一行 返回首行首列
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public object InsertScalar(string sql, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("sql");
            }
            using (MySqlCommand cmd = new MySqlCommand(sql))
            {
                return this.InsertScalar(cmd, parameters);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="value">映射的对象</param>
        /// <param name="table">表</param>
        /// <param name="key">主键</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public bool Update(object value, string table, string key, string where)
        {
            if (value == null || string.IsNullOrEmpty(table) || string.IsNullOrEmpty(key))
            {
                throw new ArgumentException();
            }
            using (MySqlCommand cmd = MysqlSDLHelper.CreateUpdate(value, table, key, where))
            {
                return this.Update(cmd);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="cmd">命名行</param>
        /// <returns></returns>
        public bool Update(MySqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            cmd.Transaction = this.Transaction;
            return MysqlSDLHelper.ExecuteNonQuery(cmd, this.Connection) > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Update(string sql, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("cmd");
            }
            using (MySqlCommand cmd = new MySqlCommand(sql))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return this.Update(cmd);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="procedure">存储过程</param>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public bool Update(string procedure, object value)
        {
            if (string.IsNullOrEmpty(procedure))
            {
                throw new ArgumentException("procedure");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            return this.Update(MysqlSDLHelper.GetParameters(value), procedure);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <param name="procedure">存储过程</param>
        /// <returns></returns>
        public bool Update(MySqlParameter[] parameters, string procedure)
        {
            if (string.IsNullOrEmpty(procedure))
            {
                throw new ArgumentException("procedure");
            }
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return this.Update(cmd);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Update(StringBuilder sql, params MySqlParameter[] parameters)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql");
            }
            return this.Update(sql.ToString(), parameters);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="cmd">命名行</param>
        /// <returns></returns>
        public int UpdateReturnInt(MySqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            cmd.Transaction = this.Transaction;
            return MysqlSDLHelper.ExecuteNonQuery(cmd, this.Connection);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int UpdateReturnInt(string sql, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("cmd");
            }
            using (MySqlCommand cmd = new MySqlCommand(sql))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return this.UpdateReturnInt(cmd);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="table">表</param>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public bool Delete(string table, string where)
        {
            if (string.IsNullOrEmpty(table))
            {
                throw new ArgumentException("table");
            }
            string sql = string.Format("DELETE {0}", table);
            if (!string.IsNullOrEmpty(where))
            {
                sql += string.Format(" WHERE {1}", where);
            }
            return this.Delete(sql);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="procedure">存储</param>
        /// <param name="value">参数</param>
        /// <returns></returns>
        public bool Delete(string procedure, object value)
        {
            return this.Update(procedure, value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <param name="procedure">存储过程</param>
        /// <returns></returns>
        public bool Delete(MySqlParameter[] parameters, string procedure)
        {
            return this.Update(parameters, procedure);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sql">删除语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Delete(string sql, params MySqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("sql");
            }
            using (MySqlCommand cmd = new MySqlCommand(sql))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                return this.Delete(cmd);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sql">删除语句</param>
        /// <returns></returns>
        public bool Delete(StringBuilder sql, params MySqlParameter[] parameters)
        {
            if (sql == null)
            {
                throw new ArgumentException("sql");
            }
            return this.Delete(sql.ToString(), parameters);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public bool Delete(MySqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            cmd.Transaction = this.Transaction;
            //
            return MysqlSDLHelper.ExecuteNonQuery(cmd, this.Connection) > 0;
        }

        /// <summary>
        /// 释放所持有的资源
        /// </summary>
        public void Dispose()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Dispose();
                this.Transaction = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
