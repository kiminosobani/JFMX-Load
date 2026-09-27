using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
namespace LoadStation.DataBaseprogram
{
    class Db_TrayInfo:CDataBaseManage
    {

        public DataSet QueryFreeTray(string FrockId)
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM tuo where visible = '0' and FrockId = '{0}'  order by tuoId asc", FrockId);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return null;
            }
        }

        /// <summary>
        /// 检索托盘信息（tuo）
        /// </summary>
        public DataSet QueryTrayInfo(int iTrayNo)
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM tuo where tuoId =  {0}", iTrayNo);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return null;
            }
        }

        /// <summary>
        /// 检索托盘信息（tuo）
        /// 查询需要翻面或者卸的任务
        /// </summary>
        /// <param name="iType">0=翻面，1=卸</param>
        //public DataSet QueryTrayInfoComplete(int iType = 0)
        //{
        //    try
        //    {
        //        db_ProduceInfo cdb_ProduceInfo = new db_ProduceInfo();
        //        Dictionary<string, int> dicABInfo = cdb_ProduceInfo.QueryChanAB();

        //        Connect();
        //        string strSQL = string.Format("SELECT * FROM tuo where visible =  9");

        //        MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
        //        DataSet ds = new DataSet();
        //        sda.Fill(ds, "T_Class");

        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
        //            if (Convert.ToInt32(ds.Tables[0].Rows[i]["AB"]) < dicABInfo[ds.Tables[0].Rows[i]["chanType"].ToString()])
        //            {
        //                if (iType != 0)
        //                {
        //                    ds.Tables[0].Rows.RemoveAt(i);
        //                    i--;
        //                }
        //            }
        //            else {
        //                if (iType == 0)
        //                {
        //                    ds.Tables[0].Rows.RemoveAt(i);
        //                    i--;
        //                }
        //            }
        //        }



        //        Close();
        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {

        //        System.Diagnostics.Debug.WriteLine(ex.ToString());
        //        return null;
        //    }
        //}
        /// <summary>
        /// 检索托盘信息（tuo）
        /// 查询需要翻面或者卸的任务
        /// </summary>
        /// <param name="iType">0=空，1=有产品，2=翻面，3=卸,4=清理</param>
        public DataSet QueryTrayInfoComplete(int iType = 0)
        {
            try
            {
                db_ProduceInfo cdb_ProduceInfo = new db_ProduceInfo();
                Dictionary<string, int> dicABInfo = cdb_ProduceInfo.QueryChanAB();

                Connect();
                string strSQL = string.Format("SELECT * FROM tuo where visible =  {0}",iType);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                DataSet ds = new DataSet();
                sda.Fill(ds, "T_Class");
                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return null;
            }
        }

        /// <summary>
        /// 更新托盘信息（tuo）
        /// </summary>
        /// <param name="UpdateInfo">0:状态visible;1:位置position;2:目标位置;3:订单编号OrderId;4:产品名称ChanType;5:产品编号chanNo;
        /// 6:优先度:preferential;7:产品进度task;8:产品状态chanState;9更新时间dateStart</param>
        /// 状态visible：0空托盘；1空，待装；2待卸；3工序进行中；8手动，加工不处理
        public void UpdateTrayInfo(int iTrayNo, object[] UpdateInfo)
        {
            try
            {
                Connect();

                //写锁，其他线程无法读写 本线程读写无影响
                //读锁，所有线程无法写 读无影响
                //string Lock = string.Format(@"lock table tuo write");
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                //MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                //MySqlCommand upCmd = new MySqlCommand(Lock, _connection);
                //Int32 rowsAffectedLock = upCmd.ExecuteNonQuery();

                string sUpdate = @"update tuo set ";
                sUpdate += UpdateInfo[0] != null ? "visible = @pVisible ," : "";
                sUpdate += UpdateInfo[1] != null ? "position = @pPosition ," : "";
                sUpdate += UpdateInfo[2] != null ? "target = @pTarget ," : "";
                sUpdate += UpdateInfo[3] != null ? "OrderId = @pOrderId ," : "";
                sUpdate += UpdateInfo[4] != null ? "chanType = @pChanType ," : "";
                sUpdate += UpdateInfo[5] != null ? "chanNo= @pChanNo ," : "";
                sUpdate += UpdateInfo[6] != null ? "preferential= @ppreferential ," : "";
                sUpdate += UpdateInfo[7] != null ? "task= @pTask ," : "";
                sUpdate += UpdateInfo[8] != null ? "chanState= @pChanState ," : "";
                sUpdate += UpdateInfo[9] != null ? "dateStart= @pdateStart" : "";
                sUpdate += " where tuoId=@ptuoId";
                adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);


                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                if (UpdateInfo[0] != null)
                    upCmd.Parameters.AddWithValue("@pVisible", UpdateInfo[0]);
                if (UpdateInfo[1] != null)
                    upCmd.Parameters.AddWithValue("@pPosition", UpdateInfo[1]);
                if (UpdateInfo[2] != null)
                    upCmd.Parameters.AddWithValue("@pTarget", UpdateInfo[2]);
                if (UpdateInfo[3] != null)
                    upCmd.Parameters.AddWithValue("@pOrderId", UpdateInfo[3]);
                if (UpdateInfo[4] != null)
                    upCmd.Parameters.AddWithValue("@pChanType", UpdateInfo[4]);
                if (UpdateInfo[5] != null)
                    upCmd.Parameters.AddWithValue("@pChanNo", UpdateInfo[5]);
                if (UpdateInfo[6] != null)
                    upCmd.Parameters.AddWithValue("@ppreferential", UpdateInfo[6]);
                if (UpdateInfo[7] != null)
                    upCmd.Parameters.AddWithValue("@pTask", UpdateInfo[7]);
                if (UpdateInfo[8] != null)
                    upCmd.Parameters.AddWithValue("@pChanState", UpdateInfo[8]);
                if (UpdateInfo[9] != null)
                    upCmd.Parameters.AddWithValue("@pdateStart", UpdateInfo[9]);
                upCmd.Parameters.AddWithValue("@ptuoId", iTrayNo);

                Int32 rowsAffected = upCmd.ExecuteNonQuery();

                //解锁
                //string UnLock = string.Format(@"unlock table");
                //adapter = new MySqlDataAdapter();
                //thisBuilder = new MySqlCommandBuilder(adapter);
                //upCmd = new MySqlCommand(UnLock, _connection);
                //Int32 rowsAffectedUnLock = upCmd.ExecuteNonQuery();

                Close();
                return;
            }
            catch (Exception ex)
            {
               

                Close();
                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return;
            }
        }
        /// <summary>
        /// 更新托盘工装绑定
        /// </summary>
        public void UpdateTrayFrockId(int iTrayNo,string FrockId)
        {
            try
            {

                Connect();

                string sUpdate = string.Format(@"update tuo set FrockId = @pFrockId where tuoID = {0}",iTrayNo);
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);

                upCmd.Parameters.AddWithValue("@pFrockId", FrockId);

                Int32 rowsAffected = upCmd.ExecuteNonQuery();
                Close();
                return;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return;
            }
        }

        /// <summary>
        /// 更新托盘角度
        /// </summary>
        public void UpdateTrayAngle(int iTrayNo, string _Angle)
        {
            try
            {

                Connect();

                string sUpdate = string.Format(@"update tuo set chanAngle = {0} where tuoID = {1}", _Angle,iTrayNo);

                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                upCmd.ExecuteNonQuery();
                Close();
                return;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return;
            }
        }

        /// <summary>
        /// 检索工装总数量
        /// </summary>
        public DataSet QueryFrockCount(string FrockId)
        {
            try
            {
                Connect();
                string strSQL = string.Format("SELECT * FROM frockinfo where frockInfoId = '{0}'", FrockId);

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");


                Close();
                return ds;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return null;
            }
        }
        /// <summary>
        /// 检索已使用的工装总数量
        /// </summary>
        public int QueryTaryFrockCount(string FrockId,int iState = 99)
        {
            try
            {
                Connect();
                string strSQL;
                if (iState == 99)
                {
                    strSQL = string.Format("SELECT count(*) FROM tuo where FrockID = '{0}'", FrockId);
                }
                else {
                    strSQL = string.Format("SELECT count(*) FROM tuo where FrockID = '{0}' and visible = {1}", FrockId, iState);
                }
                

                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);
                // SqlDataAdapter sda = new SqlDataAdapter(strSQL, _connection);

                MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);
               

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());
                Close();
                return iCount;
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return 0;
            }
        }

        public int QueryTaryCount(string Type,string Value, string ValueType) {
            try
            {
                Connect();
                string strSQL;
                if (ValueType == "int")
                {
                    strSQL = string.Format("SELECT count(*) FROM tuo where {0} = {1}", Type, Value);
                }
                else
                {
                    strSQL = string.Format("SELECT count(*) FROM tuo where {0} = '{1}'", Type, Value);
                }


                MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());
                Close();
                return iCount;

            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return 0;
            }
        }

        public int QueryTuoTask(string chanState)
        {
            try
            {

                Connect();

                string strSQL = string.Format(@"Select * from tuo where tuoId = position and chanState = '{0}'", chanState);
                MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());
                Close();

                Close();
                return iCount;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return 0;
            }

        }

        public bool QueryTaryInfo(string _Fiedls, int _iPosition)
        {
            try
            {
                Connect();
                string strSQL = string.Format(@"Select count(*) from tuo where {0} = {1}", _Fiedls, _iPosition);
                MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

                int iCount = 0;
                iCount = Convert.ToInt32(CountCmd.ExecuteScalar());

                if (iCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
                return false;
            }
        }
        public void ExchangeTuo(int orginNo,int targetNo)
        {
            try
            {
                
                DataSet dataSet1 = QueryTrayInfo(orginNo);
                Connect();
                string sUpdate = string.Format(@"update tuo set visible ='{0}',OrderId = '{1}',chanType = '{2}',chanNo = '{3}',
                    preferential ={4},task ='{5}',chanState = '{6}' where tuoId = {7}",
                    dataSet1.Tables[0].Rows[0]["visible"].ToString(), dataSet1.Tables[0].Rows[0]["OrderId"].ToString(),
                    dataSet1.Tables[0].Rows[0]["chanType"].ToString(), dataSet1.Tables[0].Rows[0]["chanNo"].ToString(),
                    dataSet1.Tables[0].Rows[0]["preferential"].ToString(), dataSet1.Tables[0].Rows[0]["task"].ToString(),
                    dataSet1.Tables[0].Rows[0]["chanState"].ToString(), targetNo);

                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                upCmd.ExecuteNonQuery();

                Close();

               

                //更新位卸载状态
                object[] TrayInfo = new object[10];
                TrayInfo[0] = 0; TrayInfo[1] = 80; TrayInfo[2] = 0; TrayInfo[3] = 0;
                TrayInfo[4] = 0; TrayInfo[5] = 0; TrayInfo[6] = 0; TrayInfo[7] = 0; TrayInfo[8] = "空"; TrayInfo[9] = DateTime.Now.ToString();

                UpdateTrayInfo(orginNo, TrayInfo);


               
            }
            catch(Exception ex)
            {
                LogHelper.WriteLog(typeof(Db_TrayInfo), ex);
            }
        }
    }
}
