using Common.LogHelper;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DbHelper
{
    public class MySqlHelper
    {
        private static Logger logger = Logger.CreateLogger(typeof(MySqlHelper));
        private static string connstr = @"Host=47.96.172.122;UserName=huabao;Password=huabao2025;Database=huabao;Port=3306;CharSet=utf8;Allow Zero Datetime=true";

        #region 执行查询语句，返回MySqlDataReader
        /// <summary>
        /// 执行查询语句，返回MySqlDataReader
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        public static MySqlDataReader ExecuteReader(string sqlString)
        {
            MySqlConnection connection = new MySqlConnection(connstr);
            MySqlCommand cmd = new MySqlCommand(sqlString, connection);
            MySqlDataReader myReader = null;
            try
            {
                connection.Open();
                myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                connection.Close();
                logger.Error(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (myReader == null)
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        #endregion

        #region 执行带参数的查询语句，返回MySqlDataReader
        /// <summary>
        /// 执行带参数的查询语句，返回MySqlDataReader
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static MySqlDataReader ExecuteReader(string sqlString, params MySqlParameter[] cmdParms)
        {
            MySqlConnection connection = new MySqlConnection(connstr);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataReader myReader = null;
            try
            {
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.SqlClient.SqlException e)
            {
                connection.Close();
                logger.Error(e.Message);
                throw new Exception(e.Message);
            }
            finally
            {
                if (myReader == null)
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        #endregion

        #region 执行sql语句,返回执行行数
        /// <summary>
        /// 执行sql语句,返回执行行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteSql(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        conn.Close();
                        logger.Error(ex.Message);
                        throw ex;
                    }
                    finally
                    {
                        cmd.Dispose();
                        conn.Close();
                    }
                }
            }
        }
        #endregion

        #region 执行带参数的sql语句，并返回执行行数
        /// <summary>
        /// 执行带参数的sql语句，并返回执行行数
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static int ExecuteSql(string sqlString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connstr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        logger.Error(ex.Message);
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        #endregion

        #region  执行带参数的查询语句，返回DataTable
        /// <summary>
        /// 执行带参数的查询语句，返回DataTable
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteQuery(string sql, MySqlParameter[] parameters)
        {
            MySqlConnection connection = new MySqlConnection(connstr);
            DataTable table = new DataTable();

            using (MySqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        table.Load(reader);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    throw;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }

            return table;
        }

        #endregion

        #region  执行带参数的查询语句，返回DataTable
        /// <summary>
        /// 执行带参数的查询语句，返回DataTable
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static DataTable ExecuteQuery(string sql)
        {
            MySqlConnection connection = new MySqlConnection(connstr);
            DataTable table = new DataTable();
         
            using (MySqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = sql;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    { 
                        using (DataSet ds = new DataSet() { EnforceConstraints = false })
                        {
                            ds.Tables.Add(table);
                            table.Load(reader, LoadOption.OverwriteChanges);
                            ds.Tables.Remove(table);
                        }
                    
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    throw;
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }

            }
            return table;
        }

        #endregion

        #region 执行查询语句，返回DataSet
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connstr))
            {
                DataSet ds = new DataSet();
                try
                {
                    conn.Open();
                    MySqlDataAdapter DataAdapter = new MySqlDataAdapter(sql, conn);
                    DataAdapter.Fill(ds);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    throw ex;
                }
                finally
                {
                    conn.Close();
                }
                return ds;
            }
        }
        #endregion

        #region 执行带参数的查询语句，返回DataSet
        /// <summary>
        /// 执行带参数的查询语句，返回DataSet
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sqlString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connstr))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        logger.Error(ex.Message);
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                    return ds;
                }
            }
        }
        #endregion

        #region 执行带参数的sql语句，并返回object
        /// <summary>
        /// 执行带参数的sql语句，并返回object
        /// </summary>
        /// <param name="sqlString"></param>
        /// <param name="cmdParms"></param>
        /// <returns></returns>
        public static object GetSingle(string sqlString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connstr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, sqlString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        logger.Error(e.Message);
                        throw new Exception(e.Message);
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        #endregion

        #region 执行存储过程,返回数据集

        /// <summary>
        /// 执行存储过程,返回数据集
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedureForDataSet(string storedProcName, IDataParameter[] parameters)
        {
            using (MySqlConnection connection = new MySqlConnection(connstr))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                MySqlDataAdapter sqlDA = new MySqlDataAdapter();
                sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                sqlDA.Fill(dataSet);
                connection.Close();
                return dataSet;
            }
        }
        #endregion

        #region 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private static MySqlCommand BuildQueryCommand(MySqlConnection connection, string storedProcName,
            IDataParameter[] parameters)
        {
            MySqlCommand command = new MySqlCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (MySqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            return command;
        }
        #endregion

        #region 装载MySqlCommand对象
        /// <summary>
        /// 装载MySqlCommand对象
        /// </summary>
        private static void PrepareCommand(MySqlCommand cmd, MySqlConnection conn, MySqlTransaction trans, string cmdText,
            MySqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
            {
                cmd.Transaction = trans;
            }
            cmd.CommandType = CommandType.Text; //cmdType;
            if (cmdParms != null)
            {
                foreach (MySqlParameter parm in cmdParms)
                {
                    cmd.Parameters.Add(parm);
                }
            }
        }
        #endregion

        #region 返回DataTable对象
        public static DataTable ExecuteDataTable(string SQLString, params MySqlParameter[] cmdParms)
        {
            using (MySqlConnection connection = new MySqlConnection(connstr))
            {
                MySqlCommand cmd = new MySqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, null);
                using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (MySql.Data.MySqlClient.MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds.Tables[0];
                }
            }
        }
        #endregion
        #region 插入DataTable数据
        public static void BatchInsert(DataTable table, string tablename,int batchSize = 10000)
        { 
                if (table.Rows.Count == 0)
                {
                    return;
                }
            using (MySqlConnection connection = new MySqlConnection(connstr))
            {
                using (MySqlCommand cmd = connection.CreateCommand())
                {
                    try
                    {

                        connection.Open();
                        cmd.CommandText = GenSql(table, tablename);
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception exp)
                    {
                        throw new Exception(exp.Message);
                    }
                    finally
                    {

                        cmd.Dispose();
                        connection.Close();
                    }
                }   
            }

        }
        public static string GenSql(DataTable dt,string tablename)
        {
            List<string> listsql = new List<string>();
            string head= "insert into " + tablename + "(";
            string cols = "";
            string mid = ") values ";
       
            string tail = ";";
            StringBuilder headbuild = new StringBuilder();
            headbuild.Append(head);
            List<string> colnames = new List<string>();
            List<string> values = new List<string>();
            foreach (DataColumn s in  dt.Columns)
            {
                colnames.Add(s.ColumnName); 
            }
            headbuild.Append(string.Join(",", colnames));
            headbuild.Append(mid);
             for (int i=0;i<dt.Rows.Count;i++)
            {
                StringBuilder rowbuild = new StringBuilder("(");
                foreach (DataColumn s in dt.Columns)
                {
                    if (s.DataType == Type.GetType("System.Int32"))
                        rowbuild.Append(dt.Rows[i][s.ColumnName]);
                    else
                    {
                        rowbuild.Append("'");
                        rowbuild.Append(dt.Rows[i][s.ColumnName]);
                        rowbuild.Append("'");
                    }
                    rowbuild.Append(",");
                }
                if (rowbuild.Length > 0)
                    rowbuild.Remove(rowbuild.Length - 1, 1);
                rowbuild.Append(")");
                values.Add(rowbuild.ToString());
            }
            headbuild.Append(string.Join(",", values));
            headbuild.Append(tail);
            return headbuild.ToString();
        }

    #endregion
    }
}
