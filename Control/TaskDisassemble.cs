using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
namespace Control
{
    class TaskDisassemble
    {
        public  bool DisassembleTask(int _iPosition,int _iInitial,int _iTarget,int _iTaryId) {
            DataBaseprogram.db_RgvtaskDao RgvtaskDao = new DataBaseprogram.db_RgvtaskDao();
            DataBaseprogram.db_AgvTaskDao AgvtaskDao  = new DataBaseprogram.db_AgvTaskDao();
            DataBaseprogram.db_TuoDao DTrayInfo = new DataBaseprogram.db_TuoDao();
            DataBaseprogram.db_EquipDao EquipDao = new DataBaseprogram.db_EquipDao();

            #region 出库
            if (_iInitial < 19 && _iTarget < 23) {
                if (!JudgeTask(_iInitial, _iTarget)) {
                    return false;
                }
                //插入任务
                RgvtaskDao.InsertRgvTaskList(_iPosition, _iInitial, _iTarget, _iTaryId);
                //占用目标位置
                object[] Info = new object[] { null, null, _iTarget, null, null, null, null, null, null, DateTime.Now.ToString() };
                DTrayInfo.UpdateTrayInfo(_iInitial, Info);
            }

            else if (_iInitial < 19 && _iTarget > 22) {
                //到托盘库缓存位   
                if (_iTarget == 23)
                {
                    if (!JudgeTask(_iInitial, _iTarget))
                    {
                        return false;
                    }
                    if (!CarryTo23()){
                        return false;
                    }

                    //任务可以执行
                    RgvtaskDao.InsertRgvTaskList(_iPosition, _iInitial, 23, _iTaryId);
                    //更新托盘信息-更新目标位置
                    object[] Info = new object[] { null, null, 23, null, null, null, null, null, null, DateTime.Now.ToString() };
                    DTrayInfo.UpdateTrayInfo(_iInitial, Info);
                }

                else{
                    if (!JudgeTask(_iInitial, _iTarget))
                    {
                        return false;
                    }
                    //判断托盘库缓存位是否被占用
                    if (!CarryTo23()) {
                        return false;
                    }

                    //分解任务
                    RgvtaskDao.InsertRgvTaskList(_iPosition, _iInitial, 23, _iTaryId);
                    //更新托盘信息-更新目标位置
                    object[] Info = new object[] { null, null, 23, null, null, null, null, null, null, DateTime.Now.ToString() };
                    DTrayInfo.UpdateTrayInfo(_iInitial, Info);

                    AgvtaskDao.InsertAgvTaskList( 23, _iTarget, _iTaryId);
                }

            }
            else if (_iInitial==23 && _iTarget>23) {
                //初始位置是托盘库缓存位
                if (!JudgeTask(_iInitial, _iTarget))
                {
                    return false;
                }
                //判断缓存位状态是否是 限出库
                DataSet dsEquipInfo =  EquipDao.QueryEquipById(23);
                if (dsEquipInfo.Tables[0].Rows[0]["equipState"].ToString() != "限出库") {
                    return false;
                }
                object[] Info = new object[] { null, null, _iTarget, null, null, null, null, null, null, DateTime.Now.ToString() };
                DTrayInfo.UpdateTrayInfo(_iTaryId, Info);
                AgvtaskDao.InsertAgvTaskList( 23, _iTarget, _iTaryId);
            }
            #endregion
            return true;
        }

        private  bool CarryTo23() {

            DataBaseprogram.db_TuoDao DTrayInfo = new DataBaseprogram.db_TuoDao();
            DataBaseprogram.db_EquipDao EquipDao = new DataBaseprogram.db_EquipDao();
            //判断当前是否有托盘
            if (DTrayInfo.QueryTaryInfo("position", 23))
            {
                return false;
            }
            //判断目标是够被占用
            if (DTrayInfo.QueryTaryInfo("target", 23))
            {
                return false;
            }
            //判断当前挡水板是否在托盘上
            DataSet dsEquipInfo = EquipDao.QueryEquipById(23);
            if (dsEquipInfo.Tables[0].Rows[0]["equipState"].ToString() == "限入库")
            {
                return false;
            }
            return true;
        }

        private bool JudgeTask(int _iInitial, int _iTarget) {

            DataBaseprogram.db_TuoDao TrayInfo = new DataBaseprogram.db_TuoDao();
            //确认初始位置是否有托盘
            DataSet dsInitial = TrayInfo.QueryTuoById(_iInitial);
            if (Convert.ToInt32(dsInitial.Tables[0].Rows[0]["position"]) != _iInitial)
            {
                return false;
            }
            //确认目标位置是否无托盘
            if (TrayInfo.QueryTaryInfo("position", _iTarget))
            {
                return false;
            }
            //确认目标位置是否已被暂用
            if (TrayInfo.QueryTaryInfo("target", _iTarget))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 托盘入库
        /// </summary>
        public bool AgvIn_Task(int _iTaryId) {

            //先判断当前是否有在进行的任务
            DataBaseprogram.db_AgvTaskDao db_AgvTask = new DataBaseprogram.db_AgvTaskDao();
            DataSet dsAgvTask =  db_AgvTask.QueryAgvTaskList();
            if (OverAll.JudgeDs(dsAgvTask))
                return false;
            //判断当前缓存位状态
            DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
            DataSet dsEquipInfo = db_Equip.QueryEquip();
            string StationState = dsEquipInfo.Tables[0].Rows[2]["equipState"].ToString();//托盘库缓存位
            if ("限入库" == StationState)//机床或者成品缓存位到托盘库缓存位
            {
                //优先搬运机床缓存台任务,查询机床缓存台上是够有托盘
                int iTrayId53 = Convert.ToInt32(dsEquipInfo.Tables[0].Rows[10]["TrayId"]);//加工设备1缓存位
                if (iTrayId53 != 0 && dsEquipInfo.Tables[0].Rows[10]["equipState"].ToString() == "加工完成") {
                    //有任务，添加任务
                    db_AgvTask.InsertAgvTaskList(53, 23, _iTaryId);
                    return true;
                }

                int iTrayId54 = Convert.ToInt32(dsEquipInfo.Tables[0].Rows[11]["TrayId"]);//加工设备2缓存位
                if (iTrayId54 != 0 && dsEquipInfo.Tables[0].Rows[11]["equipState"].ToString() == "加工完成")
                {
                    //有任务，添加任务
                    db_AgvTask.InsertAgvTaskList(54, 23, _iTaryId);
                    return true;
                }

                //如果机床缓存位没有任务，查询成品缓存位是否有任务
                int iTrayId42 = Convert.ToInt32(dsEquipInfo.Tables[0].Rows[6]["TrayId"]);//加工设备1成品缓存位
                if (iTrayId42 != 0) {
                    //有任务，添加任务
                    db_AgvTask.InsertAgvTaskList(43, 23, _iTaryId);
                    return true;
                }

                int iTrayId44 = Convert.ToInt32(dsEquipInfo.Tables[0].Rows[7]["TrayId"]);//加工设备2成品缓存位
                if (iTrayId44 != 0)
                {
                    //有任务，添加任务
                    db_AgvTask.InsertAgvTaskList(44, 23, _iTaryId);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查是够可以把托盘搬运至托盘库缓存位
        /// </summary>
        public int CheckOutBound(int _iTarge) {
            DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
            DataSet dsEquipInfo = db_Equip.QueryEquip();
            //先判断缓存位是否可以放托盘

            string StationState = dsEquipInfo.Tables[0].Rows[2]["equipState"].ToString();//托盘库缓存位
            if ("限入库" == StationState)
                return 0;
           
            //机床上是否空闲
            if(Convert.ToInt32(dsEquipInfo.Tables[0].Rows[_iTarge - 43]["TrayId"]) == 0 && Convert.ToInt32(dsEquipInfo.Tables[0].Rows[_iTarge - 41]["TrayId"]) == 0
                && "空闲" == dsEquipInfo.Tables[0].Rows[_iTarge - 41]["equipState"].ToString())
                return _iTarge + 2;

            //待加工缓存位是否空闲
            if (Convert.ToInt32(dsEquipInfo.Tables[0].Rows[_iTarge - 45]["TrayId"]) == 0)
                return _iTarge - 10;

            return 0; ;
        }
        /// <summary>
        /// 托盘出库，托盘被放到托盘缓存位时触发
        /// </summary>
        public bool AgvOut_Task(int _iTarget,int _iTaryId) {
            //优先放机床
            DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
            DataBaseprogram.db_AgvTaskDao db_AgvTask = new DataBaseprogram.db_AgvTaskDao();
            DataSet dsEquipInfo = db_Equip.QueryEquipById(_iTarget);
            string EquipState = dsEquipInfo.Tables[0].Rows[_iTarget - 43]["equipState"].ToString();
            if ("空闲" == EquipState && Convert.ToInt32(dsEquipInfo.Tables[0].Rows[_iTarget - 43]["TrayId"]) == 0)
            {
                if (Convert.ToInt32(dsEquipInfo.Tables[0].Rows[_iTarget - 41]["TrayId"]) == 0) {
                    db_AgvTask.InsertAgvTaskList(23, _iTarget+2, _iTaryId);
                    return true;
                }
               
            }
            //放到待加工缓存位
            if (Convert.ToInt32(dsEquipInfo.Tables[0].Rows[_iTarget - 47]["TrayId"]) == 0) {
                db_AgvTask.InsertAgvTaskList(23, _iTarget - 10, _iTaryId);
                return true;
            }

            return false;
        }

        public void ToatlTaskDisassemble(string _iFreeEquip) {
            DataBaseprogram.db_TotalTaskListDao db_TotalTaskList = new DataBaseprogram.db_TotalTaskListDao();
            DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
            DataSet dsEquip23 = db_Equip.QueryEquipById(23);
            DataSet dsTotalList = db_TotalTaskList.QueryTotalTaskList();
            if (_iFreeEquip == "Agv")//AGV空闲，拆分Agv任务
            {
                DataBaseprogram.db_AgvTaskDao db_AgvTask = new DataBaseprogram.db_AgvTaskDao();
                //优先搬运机床有关的任务
                DataSet dsEquip53 = db_Equip.QueryEquipById(53);

                if (dsEquip53.Tables[0].Rows[0]["equipState"].ToString() == "空闲") {
                    DataSet dsEquip41 = db_Equip.QueryEquipById(41);
                    if (dsEquip41.Tables[0].Rows[0]["TrayId"].ToString() != "0")
                    {//设备缓存位空闲，且待加工缓存位有托盘
                        for (int i = 0; i < dsTotalList.Tables[0].Rows.Count; i++)
                        {
                            int iInitial = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Inital"]);
                            int iTarget = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Target"]);
                            int iTrayId = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["TrayId"]);

                            if (iInitial == 41 && iTarget == 53)
                            {
                                db_AgvTask.InsertAgvTaskList(41, 53, iTrayId);
                                //分解后半段任务重新 插入totalList
                                db_TotalTaskList.DeleteTotalTaskList(iTrayId);
                                return;

                            }
                        }
                    }
                } else if (dsEquip53.Tables[0].Rows[0]["equipState"].ToString() == "加工完成") {
                    if (dsEquip23.Tables[0].Rows[0]["equipState"].ToString() == "限入库" && dsEquip23.Tables[0].Rows[0]["TrayId"].ToString() == "0") {

                        for (int i = 0; i < dsTotalList.Tables[0].Rows.Count; i++)
                        {
                            int iInitial = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Inital"]);
                            int iTarget = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Target"]);
                            int iTrayId = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["TrayId"]);

                            if (iInitial == 41 && iTarget == 53)
                            {
                                db_AgvTask.InsertAgvTaskList(41, 53, iTrayId);
                                //分解后半段任务重新 插入totalList
                                db_TotalTaskList.DeleteTotalTaskList(iTrayId);
                                return;

                            }
                        }
                    }
                }
                //托盘缓存位的任务
                if (dsEquip23.Tables[0].Rows[0]["equipState"].ToString() == "限入库" && dsEquip23.Tables[0].Rows[0]["TrayId"].ToString() == "0")
                {
                    //托盘放入到缓存位
                    for (int i = 0; i < dsTotalList.Tables[0].Rows.Count; i++)
                    {
                        int iInitial = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Inital"]);
                        int iTarget = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Target"]);
                        int iTrayId = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["TrayId"]);

                        if (iInitial > 40 && iTarget < 19)
                        {
                            db_AgvTask.InsertAgvTaskList(iInitial, 23, iTrayId);
                            //分解后半段任务重新 插入totalList
                            db_TotalTaskList.UpdateTotalTaskList(23, iTarget, iTrayId);
                            return;

                        }
                    }
                }
                if (dsEquip23.Tables[0].Rows[0]["equipState"].ToString() == "限出库" && dsEquip23.Tables[0].Rows[0]["TrayId"].ToString() != "0")
                {
                    //托盘从缓存位取出
                    for (int i = 0; i < dsTotalList.Tables[0].Rows.Count; i++)
                    {
                        int iInitial = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Inital"]);
                        int iTarget = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Target"]);
                        int iTrayId = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["TrayId"]);

                        if (iInitial == 23 && (iTarget == 53 || iTarget == 54))
                        {
                            //判断当前设备是否空闲
                            DataSet dsEquipCnc = db_Equip.QueryEquipById(iTarget);
                            if (dsEquipCnc.Tables[0].Rows[0]["equipState"].ToString() == "加工完成")
                            {
                                db_AgvTask.InsertAgvTaskList(23, iTarget, iTrayId);
                                db_TotalTaskList.DeleteTotalTaskList(iTrayId);     //删除当前任务
                                return;
                            }

                            //判断待加工缓存位是否空闲
                            DataSet dsEquipReady = db_Equip.QueryEquipById(iTarget - 12);
                            if (dsEquipReady.Tables[0].Rows[0]["equipState"].ToString() == "空闲")
                            {
                                db_AgvTask.InsertAgvTaskList(23, iTarget - 12, iTrayId);
                                db_TotalTaskList.UpdateTotalTaskList(iTarget - 12, iTarget, iTrayId);
                                return;
                            }
                        }
                    }
                }
                
                for (int i = 0; i < dsTotalList.Tables[0].Rows.Count; i++)
                {
                    int iInitial = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Inital"]);
                    int iTarget = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Target"]);
                    int iTrayId = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["TrayId"]);

                    if (iInitial == 23 && (iTarget == 53 || iTarget == 54))
                    {
                        //判断当前设备是否空闲
                        DataSet dsEquipCnc = db_Equip.QueryEquipById(iTarget);
                        if (dsEquipCnc.Tables[0].Rows[0]["equipState"].ToString() == "加工完成")
                        {
                            db_AgvTask.InsertAgvTaskList(23, iTarget, iTrayId);
                            db_TotalTaskList.DeleteTotalTaskList(iTrayId);     //删除当前任务
                            return;
                        }

                        //判断待加工缓存位是否空闲
                        DataSet dsEquipReady = db_Equip.QueryEquipById(iTarget - 12);
                        if (dsEquipReady.Tables[0].Rows[0]["equipState"].ToString() == "空闲")
                        {
                            db_AgvTask.InsertAgvTaskList(23, iTarget - 12, iTrayId);
                            db_TotalTaskList.UpdateTotalTaskList(iTarget - 12, iTarget, iTrayId);
                            return;
                        }
                    }
                }
                //检索与Agv有关的任务
                for (int i = 0; i < dsTotalList.Tables[0].Rows.Count; i++)
                {
                    int iInitial = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Inital"]);
                    int iTarget = Convert.ToInt32(dsTotalList.Tables[0].Rows[i]["Target"]);

                    if (iInitial < 22 && iTarget < 22)
                    {
                        //托盘库内搬运，直接插入RgvList

                    }
                }
            }
            else if (_iFreeEquip == "Rgv")//AGV空闲，拆分Rgv任务
            {

            }
            

        }
    }
}
