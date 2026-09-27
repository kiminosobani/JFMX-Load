using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;

namespace Control.DataBaseprogram
{
    class db_AgvTaskDao: CDataBaseManage
    {
        /// <summary>
        /// 判断当前AGV是否有任务（AgvTaskList）
        /// </summary>
        public bool QueryAgvFree()
        {

            Connect();
            string strSQL = string.Format(@"Select count(*) from agvtasklist");
            MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

            int iCount = 0;
            iCount = Convert.ToInt32(CountCmd.ExecuteScalar());

            Close();
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除小车任务列表（LogisticsTaskList）
        /// </summary>
        public DataSet DeleteAgvTaskList()
        {
            try
            {
                Connect();
                string strSQL = string.Format("Delete  from agvtasklist  Where PriorId = 1");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_AgvTaskDao), ex);
                return null;
            }

        }
        /// <summary>
        /// 重新排序（LogisticsTaskList）
        /// </summary>
        public DataSet RankAgvTaskList()
        {
            try
            {
                Connect();

                string Lock = string.Format(@"lock table agvtasklist write");
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                MySqlCommand upCmd = new MySqlCommand(Lock, _connection);
                Int32 rowsAffectedLock = upCmd.ExecuteNonQuery();

                string strSQL = string.Format("SELECT * FROM agvtasklist order by PriorId");
                //检索
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                DataSet ds = new DataSet();
                sda.Fill(ds, "T_Class");
                //重新排序
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ds.Tables[0].Rows[i]["PriorId"] = 1 + i;
                }
                //删除原内容
                string strDel = string.Format("delete from agvtasklist");
                MySqlCommand deleteCommand = new MySqlCommand(strDel, _connection);
                deleteCommand.ExecuteNonQuery();
                //重新插入
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sInsert = string.Format(@"insert into agvtasklist(PriorId,Initial,Target,TrayId)values('{0}',{1},{2},{3})",
                       ds.Tables[0].Rows[i]["PriorId"], ds.Tables[0].Rows[i]["Initial"], ds.Tables[0].Rows[i]["Target"], ds.Tables[0].Rows[i]["TrayId"]);

                    MySqlCommand insertCommand = new MySqlCommand(sInsert, _connection);

                    insertCommand.ExecuteNonQuery();
                }

                string UnLock = string.Format(@"unlock table");
                adapter = new MySqlDataAdapter();
                thisBuilder = new MySqlCommandBuilder(adapter);
                upCmd = new MySqlCommand(UnLock, _connection);


                Int32 rowsAffectedUnLock = upCmd.ExecuteNonQuery();

                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_AgvTaskDao), ex);
                Close();
                return null;
            }
        }
        public void InsertAgvTaskList(int iInitial, int iTarget, int iTrayId)
        {
            try
            {
                Connect();
             
                string sInsert = string.Format(@"insert into agvtasklist(Initial,Target,TrayId)values({0},{1},{2})",
                    iInitial, iTarget, iTrayId);

                    MySqlCommand insertCommand = new MySqlCommand(sInsert, _connection);

                    insertCommand.ExecuteNonQuery();
                
                Close();
                
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_AgvTaskDao), ex);
            }
        }
        public DataSet QueryAgvTaskList() {
            try
            {

                Connect();

                string strSQL = string.Format(@"Select * from agvtasklist order by PriorId");
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_AgvTaskDao), ex);
                return null;
            }
        }
    }
}
