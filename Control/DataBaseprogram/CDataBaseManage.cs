using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using System.Data.SqlClient;

using MySql.Data.MySqlClient;

namespace Control.DataBaseprogram
{
    class CDataBaseManage
    {
        
        public MySqlConnection _connection;

        /// <summary>
        /// 连接数据库
        /// </summary>
        public MySqlConnection Connect()
        {
            String ConnectionString = @"server=localhost;port=3306;user=root;password=''; database=rh-fms2001;Charset=gb2312;";

            try
            {
                if (_connection == null)
                {
                    _connection = new MySqlConnection(ConnectionString);
                    _connection.Open();
                }
                else if (_connection.State == System.Data.ConnectionState.Closed)
                {
                    _connection.Open();
                }
                else if (_connection.State == System.Data.ConnectionState.Broken)
                {
                    _connection.Close();
                    _connection.Open();
                }
                return _connection;

            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(typeof(CDataBaseManage), ex);
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 断开数据库
        /// </summary>
        public bool Close() {
            try
            {
                if (_connection != null)
                    _connection.Close();
                return true;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(CDataBaseManage), ex);
                return false;
            }
        }


        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            String ConnectionString = @"server=localhost\SQLEXPRESS;Initial Catalog=HA09-01;integrated security=SSPI";

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        connection.Close();
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
                        connection.Close();
                        throw e;
                    }
                }
            }
        }

    }
}
