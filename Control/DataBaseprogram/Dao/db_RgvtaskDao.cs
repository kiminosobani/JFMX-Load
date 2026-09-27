using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
namespace Control.DataBaseprogram
{
    class db_RgvtaskDao : CDataBaseManage
    {
        /// <summary>
        /// 查询RGV任务
        /// </summary>
        /// <summary>
        /// 判断当前AGV是否有任务（AgvTaskList）
        /// </summary>
        public bool QueryRgvFree()
        {

            Connect();
            string strSQL = string.Format(@"Select count(*) from rgvtasklist");
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
        public DataSet QueryRgvtask()
        {
            try
            {

                Connect();

                string strSQL = string.Format(@"Select * from rgvtasklist order by PriorId");
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_RgvtaskDao), ex);
                return null;
            }

        }

        /// <summary>
        /// 
        /// 上锁插入新数据 1 表示插入至第一行
        /// </summary>
        /// <param name="iPositon">1:插队;0排队</param>
        public DataSet InsertRgvTaskList(int iPositon, int iInitial, int iTarget,int TrayId)
        {
            try
            {
                Connect();
                //写锁，其他线程无法读写 本线程读写无影响
                //读锁，所有线程无法写 读无影响
                string Lock = string.Format(@"lock table rgvtasklist write");
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                MySqlCommand upCmd = new MySqlCommand(Lock, _connection);
                Int32 rowsAffectedLock = upCmd.ExecuteNonQuery();

                string strSQL = string.Format("SELECT * FROM rgvtasklist order by PriorId");
                //检索
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                DataSet ds = new DataSet();
                sda.Fill(ds, "T_Class");

                //重新排序
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (iPositon == 1)
                    {
                        ds.Tables[0].Rows[i]["PriorId"] = 2 + i;
                    }
                    else
                    {
                        ds.Tables[0].Rows[i]["PriorId"] = 1 + i;
                    }

                }
                //插入新的项
                DataRow NewDr = ds.Tables[0].NewRow();
                if (iPositon == 1)
                {
                    NewDr["PriorId"] = 1;
                }
                else
                {
                    NewDr["PriorId"] = ds.Tables[0].Rows.Count + 1;
                }
                NewDr["Initial"] = iInitial;
                NewDr["Target"] = iTarget;
                NewDr["TrayId"] = TrayId;
                ds.Tables[0].Rows.Add(NewDr);

                //删除原内容
                string strDel = string.Format("delete from rgvtasklist");
                MySqlCommand deleteCommand = new MySqlCommand(strDel, _connection);
                deleteCommand.ExecuteNonQuery();
                //重新插入
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sInsert = string.Format(@"insert into rgvtasklist(PriorId,Initial,Target,TrayId)values('{0}',{1},{2},{3})",
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

                LogHelper.WriteLog(typeof(db_RgvtaskDao), ex);
                return null;
            }
        }

        /// <summary>
        /// 删除小车任务列表（LogisticsTaskList）
        /// </summary>
        public DataSet DeleteLogisticsTaskList()
        {
            try
            {
                Connect();
                string strSQL = string.Format("Delete  from rgvtasklist  Where PriorId = 1");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_RgvtaskDao), ex);
                return null;
            }

        }
        /// <summary>
        /// 重新排序（LogisticsTaskList）
        /// </summary>
        public DataSet RankLogisticsTaskList()
        {
            try
            {
                Connect();

                string Lock = string.Format(@"lock table rgvtasklist write");
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                MySqlCommand upCmd = new MySqlCommand(Lock, _connection);
                Int32 rowsAffectedLock = upCmd.ExecuteNonQuery();

                string strSQL = string.Format("SELECT * FROM rgvtasklist order by PriorId");
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
                string strDel = string.Format("delete from rgvtasklist");
                MySqlCommand deleteCommand = new MySqlCommand(strDel, _connection);
                deleteCommand.ExecuteNonQuery();
                //重新插入
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sInsert = string.Format(@"insert into rgvtasklist(PriorId,Initial,Target,TrayId)values('{0}',{1},{2},{3})",
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

                LogHelper.WriteLog(typeof(db_RgvtaskDao), ex);
                Close();
                return null;
            }
        }
    }
}
