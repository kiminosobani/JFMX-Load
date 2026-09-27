using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using MySql.Data.MySqlClient;
namespace Control.DataBaseprogram
{
    class db_TuoDao : CDataBaseManage
    {
        public DataSet QueryTuoById(int tuoId)
        {
            try
            {
                Connect();

                string strSQL = string.Format(@"Select * from tuo where tuoId = {0}", tuoId);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_TuoDao), ex);
                return null;
            }

        }

        /// <summary>
        /// 检索可加工的产品
        /// </summary>
        public DataSet QueryTaryProcess(string _EquipName)
        {
            try
            {
                int EquipType;
                if (_EquipName.IndexOf("三轴") > -1)
                {
                    EquipType = 3;
                }
                else
                {
                    EquipType = 5;
                }

                Connect();

                string strSQL = string.Format(@"Select * from tuo where tuoId = position and chanState = '待加工' and (task Like '%-J-{0}') and visible != 8 order by preferential"
                , EquipType);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_TuoDao), ex);
                return null;
            }

        }



        /// <summary>
        /// 查询待清洗
        /// </summary>
        /// <param name="chanState"></param>
        /// <returns></returns>
        public DataSet QueryWashProcess(string chanState)
        {
            try
            {

                Connect();

                string strSQL = string.Format(@"Select * from tuo where tuoId = position and chanState = '{0}' and (task Like '%-Q%') and visible != 8  order by preferential"
                , chanState);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_TuoDao), ex);
                return null;
            }

        }
          /// <summary>
        /// 查询待加工
        /// </summary>
        /// <param name="chanState"></param>
        /// <returns></returns>
        public DataSet QueryUnloadProcess(string chanState)
        {
            try
            {

                Connect();

                string strSQL = string.Format(@"Select * from tuo where tuoId = position and chanState = '{0}' and (task Like '%-J%') and visible != 8  order by preferential"
                , chanState);
                MySqlDataAdapter sda = new MySqlDataAdapter("set names gb2312;" + strSQL, _connection);

                DataSet ds = new DataSet();

                sda.Fill(ds, "T_Class");

                Close();
                return ds;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_TuoDao), ex);
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
                string Lock = string.Format(@"lock table tuo write");
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommandBuilder thisBuilder = new MySqlCommandBuilder(adapter);
                MySqlCommand upCmd = new MySqlCommand(Lock, _connection);
                Int32 rowsAffectedLock = upCmd.ExecuteNonQuery();

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
                thisBuilder = new MySqlCommandBuilder(adapter);


                upCmd = new MySqlCommand(sUpdate, _connection);
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
                string UnLock = string.Format(@"unlock table");
                adapter = new MySqlDataAdapter();
                thisBuilder = new MySqlCommandBuilder(adapter);
                upCmd = new MySqlCommand(UnLock, _connection);
                Int32 rowsAffectedUnLock = upCmd.ExecuteNonQuery();

                Close();
                return;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(db_TuoDao), ex);
                return;
            }
        }

        public bool QueryTaryInfo(string _Fiedls,int _iPosition) {

            Connect();
            string strSQL = string.Format(@"Select count(*) from tuo where {0} = {1}", _Fiedls, _iPosition);
            MySqlCommand CountCmd = new MySqlCommand(strSQL, _connection);

            int iCount = 0;
            iCount = Convert.ToInt32(CountCmd.ExecuteScalar());

            Close();
            if (iCount > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
