using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;

namespace LoadStation.DataBaseprogram
{
    class db_ProduceInfo : CDataBaseManage
    {
        /// <summary>
        /// 查询表-订单（order1）
        /// </summary>
        public DataSet QueryOrder1ByOrderNum(string OrderNum)
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM order1 where orderNum = '{0}'", OrderNum);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return null;
            }
        }
        public void UpdateOrderNum(string OrderNum)
        {
            try
            {
                

                Connect();
                string sUpdate = string.Format(@"update order1 set okNum = okNum + 1  where orderNum = '{0}'", OrderNum);

                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                Int32 rowsAffected = upCmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
            }

        }

        public void UpdateOrderChanNo(string OrderNum,string chanNo,string zhuang)
        {
            try
            {
                // OverAll.DebugLogState(EquipID.ToString(), UpdateInfo);
                Connect();

                string sUpdate = string.Format(@"update order1 set chanNo='{0}',zhuang ='{1}'  where orderNum = '{2}'", chanNo, zhuang,OrderNum);

                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                Int32 rowsAffected = upCmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
            }

        }

        /// <summary>
        /// 查询表-产品信息（chan）
        /// </summary>
        public DataSet QueryChanByChanType(string sChanType)
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM product where chanName = '{0}'", sChanType);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return null;
            }
        }

        /// <summary>
        /// 查询表-产品信息（chan）
        /// </summary>
        public Dictionary<string, int> QueryChanAB()
        {
            try
            {
                Connect();
                string strSQL = string.Format("select chanName,count(*) as count from product  group by chanName");

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Dictionary<string, int> ChanABInfo = new Dictionary<string, int>();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ChanABInfo.Add(ds.Tables[0].Rows[i]["chanName"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[i]["count"]));
                }
                Close();
                return ChanABInfo;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return null;
            }

        }
        /// <summary>
        /// 查询表-设备状态信息
        /// </summary>
        public DataSet QueryEquipInfo(string EquipName)
        {
            try
            {
                Connect();

                string strSQL = string.Format(@"Select * from equip where equipName = '{0}'", EquipName);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return null;
            }

        }
        public DataSet QueryEquipInfo(int EquipId)
        {
            try
            {
                Connect();

                string strSQL = string.Format(@"Select * from equip where equipId = {0}", EquipId);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return null;
            }

        }


        public void updateMeasureState()
        {
            try
            {
                Connect();

                string sUpdate = string.Format(@"update equip set equipState='待测量' where equipId = 57");

                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                Int32 rowsAffected = upCmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
            }

        }

        /// <summary>
        /// 查询工装编号（）
        /// </summary>
        public DataSet QueryFrockID(string ProductName)
        {
            try
            {
                Connect();

                string strSQL = string.Format(@"Select * from product where chanName = '{0}'", ProductName);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return null;
            }
        }
       
        public void HarvestPlus_Day(bool Reject = false)
        {
            try
            {
                string Day = DateTime.Now.ToString("MM-dd");
                Connect();
                string strSQL = string.Format(@"Select count(*) from harvest where date = '{0}'", Day);
                MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());

                if (iCount > 0)
                {
                    //增加
                    string sUpdate;
                    if (!Reject) {
                        sUpdate = string.Format(@"update harvest set Harvest = Harvest + 1 where date = '{0}'", Day);
                    } else {
                        sUpdate = string.Format(@"update harvest set Reject = Reject + 1 where date = '{0}'", Day);
                    }
                  

                    MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                    upCmd.ExecuteNonQuery();
                }
                else
                {
                    //插入
                    strSQL = string.Format("SELECT * FROM harvest order by Id");
                    //检索
                    MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                    DataSet ds = new DataSet();
                    sda.Fill(ds, "T_Class");
                    //重新排序
                    for (int i = 0; i < ds.Tables[0].Rows.Count - 1; i++)
                    {
                        ds.Tables[0].Rows[i]["Id"] = i + 1;
                        ds.Tables[0].Rows[i]["date"] = ds.Tables[0].Rows[i + 1]["date"];
                        ds.Tables[0].Rows[i]["Harvest"] = ds.Tables[0].Rows[i + 1]["Harvest"];
                        ds.Tables[0].Rows[i]["Reject"] = ds.Tables[0].Rows[i + 1]["Reject"];
                    }
                    ds.Tables[0].Rows[6]["Id"] = 7;
                    ds.Tables[0].Rows[6]["date"] = Day;
                    if (!Reject)
                    {
                        ds.Tables[0].Rows[6]["Harvest"] = 1;
                        ds.Tables[0].Rows[6]["Reject"] = 0;
                    }
                    else {
                        ds.Tables[0].Rows[6]["Harvest"] = 0;
                        ds.Tables[0].Rows[6]["Reject"] = 1;
                    }
                    
                    //删除原内容
                    string strDel = string.Format("delete from harvest");
                    MySqlCommand deleteCommand = new MySqlCommand(strDel, _connection);
                    deleteCommand.ExecuteNonQuery();
                    //重新插入
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sInsert = string.Format(@"insert into harvest(Id,date,Harvest,Reject)values({0},'{1}',{2},{3})",
                           ds.Tables[0].Rows[i]["Id"], ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["Harvest"],ds.Tables[0].Rows[i]["Reject"]);

                        MySqlCommand insertCommand = new MySqlCommand(sInsert, _connection);

                        insertCommand.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
            }
        }

        public void HarvestPlus_Month(bool Reject = false)
        {
            try
            {
                string Day = DateTime.Now.ToString("yy-MM");
                Connect();
                string strSQL = string.Format(@"Select count(*) from harvestmonth where date = '{0}'", Day);
                MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());

                if (iCount > 0)
                {
                    //增加
                    string sUpdate;
                    if (!Reject)
                    {
                        sUpdate = string.Format(@"update harvestmonth set Harvest = Harvest + 1 where date = '{0}'", Day);
                    }
                    else
                    {
                        sUpdate = string.Format(@"update harvestmonth set Reject = Reject + 1 where date = '{0}'", Day);
                    }


                    MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                    upCmd.ExecuteNonQuery();
                }
                else
                {
                    //插入
                    strSQL = string.Format("SELECT * FROM harvestmonth order by Id");
                    //检索
                    MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                    DataSet ds = new DataSet();
                    sda.Fill(ds, "T_Class");
                    //重新排序
                    for (int i = 0; i < ds.Tables[0].Rows.Count - 1; i++)
                    {
                        ds.Tables[0].Rows[i]["Id"] = i + 1;
                        ds.Tables[0].Rows[i]["date"] = ds.Tables[0].Rows[i + 1]["date"];
                        ds.Tables[0].Rows[i]["Harvest"] = ds.Tables[0].Rows[i + 1]["Harvest"];
                        ds.Tables[0].Rows[i]["Reject"] = ds.Tables[0].Rows[i + 1]["Reject"];
                    }
                    ds.Tables[0].Rows[11]["Id"] = 12;
                    ds.Tables[0].Rows[11]["date"] = Day;
                    if (!Reject)
                    {
                        ds.Tables[0].Rows[11]["Harvest"] = 1;
                        ds.Tables[0].Rows[11]["Reject"] = 0;
                    }
                    else
                    {
                        ds.Tables[0].Rows[11]["Harvest"] = 0;
                        ds.Tables[0].Rows[11]["Reject"] = 1;
                    }

                    //删除原内容
                    string strDel = string.Format("delete from harvestmonth");
                    MySqlCommand deleteCommand = new MySqlCommand(strDel, _connection);
                    deleteCommand.ExecuteNonQuery();
                    //重新插入
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string sInsert = string.Format(@"insert into harvestmonth(Id,date,Harvest,Reject)values({0},'{1}',{2},{3})",
                           ds.Tables[0].Rows[i]["Id"], ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["Harvest"], ds.Tables[0].Rows[i]["Reject"]);

                        MySqlCommand insertCommand = new MySqlCommand(sInsert, _connection);

                        insertCommand.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
            }
        }

        /// <summary>
        /// 查询表-订单记录（orderre）
        /// </summary>
        public int QueryOrderRecordCount(string orderNum)
        {
            try
            {
                Connect();
                string strSQL = string.Format(@"Select count(*) from orderre where orderNum = '{0}'", orderNum);
                MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());
                Close();
                return iCount;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return 0;
            }

        }
        /// <summary>
        /// 查询表-订单记录（orderre）
        /// </summary>
        public DataSet QueryOrderRecord(string orderNum,string CreateTime)
        {
            try
            {
                Connect();
                string strSQL = string.Format(@"Select * from orderre where orderNum = '{0}' and createTime = '{1}'", orderNum, CreateTime);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");
                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return null;
            }

        }
        /// <summary>
        /// 插入表-订单记录（orderre）
        /// </summary>
        public void InsertOrderReNum(string OrderNum)
        {
            try
            {
                DataSet OrderInfo = QueryOrder1ByOrderNum(OrderNum);
                int ReId =  QueryOrderReMaxId() + 1;
                Connect();

                string sInsert = string.Format(@"insert into orderre(orderNum,chanType,totalNum,okNum,userId,createTime,hegeNum,okTime,reId)values
                                                                    ('{0}','{1}',{2},{3},'{4}','{5}',{6},'{7}',ReId)",
                     OrderNum, OrderInfo.Tables[0].Rows[0]["chanType"].ToString(), OrderInfo.Tables[0].Rows[0]["totalNum"].ToString(), 0,
                     OrderInfo.Tables[0].Rows[0]["userId"].ToString(), OrderInfo.Tables[0].Rows[0]["createTime"].ToString(), 0, "0");

                MySqlCommand insertCommand = new MySqlCommand(sInsert, _connection);

                insertCommand.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
            }

        }
        public int QueryOrderReMaxId() {
            try
            {
                Connect();
                string strSQL = string.Format(@"Select MAX(reId) from orderre");

                MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());

                Close();
                return iCount;
           
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
                return 999;
            }
        }

        public void UpdateOrderRe(string OrderNum,bool bolReject)
        {
            try
            {
                DataSet OrderInfo = QueryOrder1ByOrderNum(OrderNum);
                DataSet RecordInfo = QueryOrderRecord(OrderNum, OrderInfo.Tables[0].Rows[0]["createTime"].ToString());

                int CurrentCount = Convert.ToInt32(RecordInfo.Tables[0].Rows[0]["okNum"]);
                int TotalCount = Convert.ToInt32(RecordInfo.Tables[0].Rows[0]["totalNum"]);
                int OkCount = Convert.ToInt32(RecordInfo.Tables[0].Rows[0]["hegeNum"]);
                if (!bolReject) {
                    OkCount++;
                }
                Connect();
                string sUpdate;
                if (TotalCount == CurrentCount + 1) {
                    //所有任务已经完成
                    sUpdate = string.Format(@"update orderre set okNum = totalNum ,okTime = '{0}',hegeNum = {1} where orderNum = '{2}' and createTime = '{3}'",
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), OkCount,
                      OrderNum, OrderInfo.Tables[0].Rows[0]["createTime"]);
                }
                else{
                    sUpdate = string.Format(@"update orderre set okNum = {0} ,hegeNum = {1}  where orderNum = '{2}' and createTime = '{3}'", CurrentCount + 1, OkCount,
                      OrderNum,  OrderInfo.Tables[0].Rows[0]["createTime"]);
                }
                

                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                Int32 rowsAffected = upCmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_ProduceInfo), ex);
            }

        }
    }
}
