using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
namespace Control.DataBaseprogram
{
    class db_EquipDao : CDataBaseManage
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="EquipId"></param>
        /// <returns></returns>
        public DataSet QueryEquipById(int EquipId)
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
                LogHelper.WriteLog(typeof(db_EquipDao), ex);
                return null;
            }
        }
        public DataSet QueryEquip()
        {
            try
            {
                Connect();

                string strSQL = string.Format(@"Select * from equip order by equipId");
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_EquipDao), ex);
                return null;
            }
        }
        /// <summary>
        /// 修改数据
        /// EquipInfo：0设备状态,1订单,3产品类型,4托盘ID
        /// </summary>
        /// <param name="EquipInfo"></param>
        /// <returns></returns>
        public void UpdateEquipInfo(int EquipId, object[] EquipInfo)
        {
            try
            {
                Connect();

                string Lock = string.Format(@"lock table equip write");
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                MySqlCommand upCmd = new MySqlCommand(Lock, _connection);
                Int32 rowsAffectedLock = upCmd.ExecuteNonQuery();

                string sUpdate = @"update equip set ";
                sUpdate += EquipInfo[0] != null ? "equipState = @equipState , " : "";
                sUpdate += EquipInfo[1] != null ? "equipOrder = @equipOrder , " : "";
                sUpdate += EquipInfo[2] != null ? "equipProduct = @equipProduct , " : "";
                sUpdate += EquipInfo[3] != null ? "TrayId = @TrayId , " : "";
                sUpdate += " equipId=@equipId1 where equipId=@equipId";
                adapter = new MySqlDataAdapter();
                thisBuilder = new MySqlCommandBuilder(adapter);


                upCmd = new MySqlCommand(sUpdate, _connection);
                if (EquipInfo[0] != null)
                    upCmd.Parameters.AddWithValue("@equipState", EquipInfo[0]);
                if (EquipInfo[1] != null)
                    upCmd.Parameters.AddWithValue("@equipOrder", EquipInfo[1]);
                if (EquipInfo[2] != null)
                    upCmd.Parameters.AddWithValue("@equipProduct", EquipInfo[2]);
                if (EquipInfo[3] != null)
                    upCmd.Parameters.AddWithValue("@TrayId", EquipInfo[3]);
                upCmd.Parameters.AddWithValue("@equipId1", EquipId);
                upCmd.Parameters.AddWithValue("@equipId", EquipId);

                Int32 rowsAffected = upCmd.ExecuteNonQuery();


                //解锁
                string UnLock = string.Format(@"unlock table");
                adapter = new MySqlDataAdapter();
                thisBuilder = new MySqlCommandBuilder(adapter);
                upCmd = new MySqlCommand(UnLock, _connection);
                Int32 rowsAffectedUnLock = upCmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_EquipDao), ex);
            }

        }
        /// <summary>
        /// 查找空闲设备
        /// </summary>
        /// <returns></returns>
        public DataSet QueryFreeEquip()
        {
            try
            {
                Connect();

                string strSQL = string.Format(@"Select * from equip where equipState = '空闲'");
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_EquipDao), ex);
                return null;
            }

        }

        public DataSet QueryCarstate()
        {
            try
            {
                Connect();

                string strSQL = string.Format(@"Select * from carstate ");
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_EquipDao), ex);
                return null;
            }

        }
        public void DeleteCarstate()
        {
            try
            {
                Connect();

                string sUpdate = string.Format(@"update carstate set OriginPos = 0 , TargetPos = 0 ,TaskType = 0,TrayNo = 0");

                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);
                upCmd.ExecuteNonQuery();

                Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_EquipDao), ex);
            }

        }

        /// <summary>
        /// 更新小车状态（CarState）
        /// </summary>
        public void UpdateCarState(int iOriginPos, int iTargrtPos, int iTaskType, int iTrayNo)
        {
            try
            {
                Connect();

                string sUpdate = string.Format(@"update carstate set OriginPos = @pOriginPos,
                    TargetPos = @pTargetPos,TaskType = @pTaskType,TrayNo = @pTrayNo where Id = 1");
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                MySqlCommand upCmd = new MySqlCommand(sUpdate, _connection);

                upCmd.Parameters.AddWithValue("@pOriginPos", iOriginPos);
                upCmd.Parameters.AddWithValue("@pTargetPos", iTargrtPos);
                upCmd.Parameters.AddWithValue("@pTaskType", iTaskType);
                upCmd.Parameters.AddWithValue("@pTrayNo", iTrayNo);

                Int32 rowsAffected = upCmd.ExecuteNonQuery();


                Close();
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(db_EquipDao), ex);
            }

        }
    }
}
