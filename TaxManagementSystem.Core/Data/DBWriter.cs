﻿namespace TaxManagementSystem.Core.Data
{
    using TaxManagementSystem.Core.Data.Connection;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    /// <summary>
    /// 数据库读写装置
    /// </summary>
    public partial class DBWriter : IDisposable
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public SqlConnection Connection
        {
            get;
            private set;
        }
        /// <summary>
        /// 数据库事务
        /// </summary>
        public SqlTransaction Transaction
        {
            get;
            private set;
        }

        public DBWriter()
        {
            this.Connection = DBConnection.Current;
            if (this.Connection.State != ConnectionState.Open)
            {
                this.Connection.Open();
            }
            this.Transaction = this.Connection.BeginTransaction();
        }

        ~DBWriter()
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
            using (SqlCommand cmd = SDLHelper.CreateInsert(value, table))
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
        public bool Insert(SqlCommand cmd, params SqlParameter[] parameters)
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
            return SDLHelper.ExecuteNonQuery(cmd, this.Connection) > 0;
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <returns></returns>
        public object InsertScalar(SqlCommand cmd, params SqlParameter[] parameters)
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
            return SDLHelper.ExecuteScalar(cmd, this.Connection);
        }

        /// <summary>
        /// 插入一行
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Insert(StringBuilder sql, params SqlParameter[] parameters)
        {
            if (sql == null)
            {
                throw new ArgumentNullException("sql");
            }
            using (SqlCommand cmd = new SqlCommand())
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
        public bool Insert(SqlParameter[] parameters, string procedure)
        {
            return this.Update(parameters, procedure);
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="value">参数值</param>
        /// <returns></returns>
        public SqlParameter CreateParameter(string name, object value, DbType? type = null)
        {
            var arg = new SqlParameter(name, value);
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
        public bool Insert(string sql, params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("sql");
            }
            using (SqlCommand cmd = new SqlCommand(sql))
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
        public object InsertScalar(string sql, params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("sql");
            }
            using (SqlCommand cmd = new SqlCommand(sql))
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
            using (SqlCommand cmd = SDLHelper.CreateUpdate(value, table, key, where))
            {
                return this.Update(cmd);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="cmd">命名行</param>
        /// <returns></returns>
        public bool Update(SqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            cmd.Transaction = this.Transaction;
            return SDLHelper.ExecuteNonQuery(cmd, this.Connection) > 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Update(string sql, params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("cmd");
            }
            using (SqlCommand cmd = new SqlCommand(sql))
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
            return this.Update(SDLHelper.GetParameters(value), procedure);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="parameters">参数</param>
        /// <param name="procedure">存储过程</param>
        /// <returns></returns>
        public bool Update(SqlParameter[] parameters, string procedure)
        {
            if (string.IsNullOrEmpty(procedure))
            {
                throw new ArgumentException("procedure");
            }
            using (SqlCommand cmd = new SqlCommand())
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
        public bool Update(StringBuilder sql, params SqlParameter[] parameters)
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
        public int UpdateReturnInt(SqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            cmd.Transaction = this.Transaction;
            return SDLHelper.ExecuteNonQuery(cmd, this.Connection);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sql">语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int UpdateReturnInt(string sql, params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException("cmd");
            }
            using (SqlCommand cmd = new SqlCommand(sql))
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
        public bool Delete(SqlParameter[] parameters, string procedure)
        {
            return this.Update(parameters, procedure);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sql">删除语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public bool Delete(string sql, params SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentException("sql");
            }
            using (SqlCommand cmd = new SqlCommand(sql))
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
        public bool Delete(StringBuilder sql, params SqlParameter[] parameters)
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
        public bool Delete(SqlCommand cmd)
        {
            if (cmd == null)
            {
                throw new ArgumentNullException("cmd");
            }
            cmd.Transaction = this.Transaction;
            //
            return SDLHelper.ExecuteNonQuery(cmd, this.Connection) > 0;
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
