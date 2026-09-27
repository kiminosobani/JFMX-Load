using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
namespace Control.DataBaseprogram
{
    class db_TotalTaskListDao : CDataBaseManage
    {
        /// <summary>
        /// 
        /// 上锁插入新数据 1 表示插入至第一行
        /// </summary>
        public DataSet InsertTotalTaskList(int iPositon, int iInitial, int iTarget)
        {
            try
            {
                Connect();
                //写锁，其他线程无法读写 本线程读写无影响
                //读锁，所有线程无法写 读无影响
                string Lock = string.Format(@"lock table logisticstasklist write");
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                MySqlCommand upCmd = new MySqlCommand(Lock, _connection);
                Int32 rowsAffectedLock = upCmd.ExecuteNonQuery();

                string strSQL = string.Format("SELECT * FROM logisticstasklist order by PriorId");
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
                ds.Tables[0].Rows.Add(NewDr);

                //删除原内容
                string strDel = string.Format("delete from logisticstasklist");
                MySqlCommand deleteCommand = new MySqlCommand(strDel, _connection);
                deleteCommand.ExecuteNonQuery();
                //重新插入
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sInsert = string.Format(@"insert into logisticstasklist(PriorId,Initial,Target)values('{0}',{1},{2})",
                       ds.Tables[0].Rows[i]["PriorId"], ds.Tables[0].Rows[i]["Initial"], ds.Tables[0].Rows[i]["Target"]);

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

                LogHelper.WriteLog(typeof(db_TotalTaskListDao), ex);
                return null;
            }
        }


        public DataSet QueryTotalTaskList()
        {
            try
            {

                Connect();

                string strSQL = string.Format(@"Select * from totaltask order by Id");
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_TotalTaskListDao), ex);
                return null;
            }
        }

        public void UpdateTotalTaskList(int _iInital,int _iTarget,int _iTrayId)
        {
            try
            {


                Connect();
                string sUpdate = string.Format(@"update totaltask set Inital = {0} , Target = {1} where TaryId = {2}", _iInital, _iTarget, _iTrayId);

                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                Int32 rowsAffected = upCmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_TotalTaskListDao), ex);
            }

        }

        public void DeleteTotalTaskList(int _iTrayId) {
            try
            {
                Connect();
                string strSQL = string.Format("Delete  from totaltask  Where TrayId = {0}", _iTrayId);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_TotalTaskListDao), ex);
            }
        }
    }
}
