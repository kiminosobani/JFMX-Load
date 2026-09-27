using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;

using MySql.Data.MySqlClient;

namespace LoadStation.DataBaseprogram
{
    class TaskList: CDataBaseManage
    {
        /// <summary>
        /// 查询表-装任务列表（order1）
        /// </summary>
        public DataSet QueryOrder()
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM order1 where totalNum != zhuang");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
               // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }

        }
        public DataSet QueryMeasureError()
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM measureerror");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                DataSet ds = new DataSet();
                sda.Fill(ds, "T_Class");
                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }
        }
        public DataSet CleanMeasureError()
        {
            try
            {
                Connect();
                string strSQL = string.Format("Delete  FROM measureerror");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                DataSet ds = new DataSet();
                sda.Fill(ds, "T_Class");
                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }
        }
        public DataSet QueryMeasureError(int iTaryId)
        {
            try
            {

                Connect();
                string strSQL = string.Format("SELECT * FROM measureerror where TaryId = {0}",iTaryId);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                DataSet ds = new DataSet();
                sda.Fill(ds, "T_Class");
                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }
        }
        public int QueryMeasureErrorCount(int iTaryId)
        {
            try
            {
                Connect();

                string sQuery = string.Format(@"select count(*) from measureerror where TaryId = {0}", iTaryId);

                MySqlCommand CountCmd = new MySqlCommand(sQuery, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());

                Close();
                return iCount;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(TaskList), ex);
                return 0;
            }
        }
        /// <summary>
        /// 查询表-卸任务列表（UnLoadTaskList）
        /// </summary>
        public DataSet QueryUnLoadTaskList()
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM UnLoadTaskList ");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }

        }

        /// <summary>
        /// 查询表-小车任务列表（rgvtasklist）
        /// </summary>
        public DataSet QueryRgvTaskList()
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM rgvtasklist order by PriorId");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }

        }
        /// <summary>
        /// 查询表-小车任务列表（rgvtasklist）
        /// </summary>
        public DataSet QueryrTotalTaskList()
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM totaltask order by Id");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }
        }

        /// <summary>
        /// 查询表-小车任务列表（rgvtasklist）
        /// </summary>
        public DataSet QueryrAgvTaskList()
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM agvtasklist order by PriorId");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }

        }
        public int QueryrgvtasklistCount()
        {
            try
            {
                Connect();

                string sQuery = string.Format(@"select count(*) from rgvtasklist ");

                MySqlCommand CountCmd = new MySqlCommand(sQuery, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());

                Close();
                return iCount;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return 0;
            }

        }
        /// <summary>
        /// 查询表-小车任务列表（rgvtasklist）
        /// </summary>
        public int  Queryrgvtasklist(int iInitial)
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM rgvtasklist where Initial = {0} ", iInitial);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return 1;
            }

        }


        public int Queryrgvtasklist1(int itarget)
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM rgvtasklist where Target = {0} ", itarget);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return 1;
            }

        }

        /// <summary>
        /// 清空列表（rgvtasklist）
        /// </summary>
        public void Deletergvtasklist()
        {
            try
            {
                Connect();
                string DeleteSQL = string.Format("delete  FROM rgvtasklist");
                //检索
                MySqlCommand deleteCommand = new MySqlCommand(DeleteSQL, _connection);

                deleteCommand.ExecuteNonQuery();

                Close();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);

            }
        }
  
        public void InsertTotalTaskList(int iInitial, int iTarget, int iTrayId)
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


                string sInsert = string.Format(@"insert into rgvtasklist(Initial,Target,TrayId)values('{0}',{1},{2})",
                    iInitial, iTarget, iTrayId);

                MySqlCommand insertCommand = new MySqlCommand(sInsert, _connection);

                insertCommand.ExecuteNonQuery();


                string UnLock = string.Format(@"unlock table");
                adapter = new MySqlDataAdapter();
                thisBuilder = new MySqlCommandBuilder(adapter);
                upCmd = new MySqlCommand(UnLock, _connection);
                Int32 rowsAffectedUnLock = upCmd.ExecuteNonQuery();

                Close();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
            }
        }
        /// <summary>
        /// 查询小车状态（carstate）
        /// </summary>
        public DataSet QueryCarState()
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM carstate");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }

        }


        /// <summary>
        /// 插入新数据（agvtasklist）
        /// </summary>
        public DataSet InsertAgvTaskList(int iPositon, int iInitial, int iTarget)
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM agvtasklist order by PriorId");
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
                string strDel = string.Format("delete from agvtasklist");
                MySqlCommand deleteCommand = new MySqlCommand(strDel, _connection);
                deleteCommand.ExecuteNonQuery();
                //重新插入
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string sInsert = string.Format(@"insert into agvtasklist(PriorId,Initial,Target)values('{0}',{1},{2})",
                       ds.Tables[0].Rows[i]["PriorId"], ds.Tables[0].Rows[i]["Initial"], ds.Tables[0].Rows[i]["Target"]);

                    MySqlCommand insertCommand = new MySqlCommand(sInsert, _connection);

                    insertCommand.ExecuteNonQuery();
                }
                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(TaskList), ex);
                return null;
            }
        }
        /// <summary>
    }
}
