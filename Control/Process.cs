using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Data;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
using Opc.Ua.Client;
using SinumerikOpcUaAPI;
using SinumerikVariables;

namespace Control
{
    class Process
    {
        /// <summary>
        /// 开始加工
        /// </summary>
        public static void StartProcess(Server _UaServer, int _EquipId, int _iTrayNo) {
            //return;
            try
            {
                DataBaseprogram.db_EquipDao cdb_Product = new DataBaseprogram.db_EquipDao();
                /*********************判断加工是否需要换产品*********************************/
                //检索当前加工的产品
                DataBaseprogram.db_TuoDao db_TrayInfo = new DataBaseprogram.db_TuoDao();
                DataSet dsTaryInfo = db_TrayInfo.QueryTuoById(_iTrayNo);
                string ProduceName = dsTaryInfo.Tables[0].Rows[0]["chanType"].ToString();
                string ProcessName = dsTaryInfo.Tables[0].Rows[0]["task"].ToString();

                Thread.Sleep(5000);
            
                string MainProgram = InitializeProcess(_UaServer, ProduceName, ProcessName, _EquipId);
                SinumerikOpcUaAPI.Server.MethodCallResult m_MethodCallResult = _UaServer.MethodCallSelectProgram(string.Format(@"Sinumerik/FileSystem/ExtendedDrives/DEV_2/{0}", MainProgram), 1);


                ////装载程序
                List<string> NodeList_Load = new List<string>() { new NodeId(AddressDic.obtainAddress(5, "装载程序"), 2).ToString() };
                List<String> ValueList_Load = new List<string>() { "True" };
                _UaServer.WriteValues(ValueList_Load, NodeList_Load);
                //执行
                List<string> NodeList_Start = new List<string>() { new NodeId(AddressDic.obtainAddress(5, "执行程序"), 2).ToString() };
                List<String> ValueList_Start = new List<string>() { "True" };
                _UaServer.WriteValues(ValueList_Start, NodeList_Start);


                object[] UpdateInfo = new object[4];
                UpdateInfo[0] = "加工中"; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = null;
                cdb_Product.UpdateEquipInfo(_EquipId, UpdateInfo);


           
                OverAll.ProductRecord(dsTaryInfo.Tables[0].Rows[0]["chantype"].ToString(), dsTaryInfo.Tables[0].Rows[0]["OrderId"].ToString(),
                dsTaryInfo.Tables[0].Rows[0]["chanNo"].ToString(), "开始加工", string.Format("加工设备:{0}", _EquipId));

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(Process), ex);
            }
        }
        /// <summary>
        /// 加工初始化
        /// </summary>
        public static string  InitializeProcess(Server UaServer, string _ProductName,string _ProcessName,int _EquipId)
        {
            
            //DataBaseprogram.Db_Product cdb_Product = new DataBaseprogram.Db_Product();
            //DataSet dsProductList = cdb_Product.QueryProductInfo(ProductName);
            //得到加工程序及刀具列表名称
            //string ProgramName = dsProductList.Tables[0].Rows[0]["mainPro"].ToString();
            //string ToolList = dsProductList.Tables[0].Rows[0]["daoJu"].ToString();

            //string ProgramPath = @"D:\016523\My Documents\rfmx\" + ProductName + @"\加工程序\" + ProgramName;
            //string ToolListPath = @"D:\016523\My Documents\rfmx\" + ProductName + @"\刀具清单\" + ToolList;


            /***********************************************导入加工程序********************************************/
            #region 导入加工程序
            //string MainProgram = null;
            ////删除目录中的文件
            //SinumerikOpcUaAPI.Server.MethodCallResult m_MethodCallResult;
            //List<string> ProgramFold = new List<string>();
            ////m_MethodCallResult = UaServer.MethodCallDeleteFile("Sinumerik/FileSystem/Part Program/AUTOPROCESS/PROCESS");

            //if (TraverseProgram(UaServer, @"Sinumerik/FileSystem/Part Program/AUTOPROCESS/PROCESS", "", ref ProgramFold) != 0)
            //{
            //    for (int i = 0; i < ProgramFold.Count; i++)
            //    {
            //        m_MethodCallResult = UaServer.MethodCallDeleteFile(ProgramFold[i]);
            //    }

            //    //删除整个目录（AutoProcess）
            //    m_MethodCallResult = UaServer.MethodCallDeleteFile("Sinumerik/FileSystem/Part Program/AUTOPROCESS/PROCESS");
            //}
            ////重新创建文件夹
            //m_MethodCallResult = UaServer.MethodCallCreateNewDir("Sinumerik/FileSystem/Part Program/AUTOPROCESS", "PROCESS");

            ////导入加工程序
            //string sourceFolder = string.Format(@"C:\Users\Lenovo\Documents\rfmx\{0}\加工程序\{1}\{2}", _ProductName, _ProcessName, _EquipId > 52 ? 3 : 5);
            //DirectoryInfo theMainFolder = new DirectoryInfo(sourceFolder);
            //foreach (FileInfo file in theMainFolder.GetFiles())
            //{
            //    if (file.Name.IndexOf(".MPF") > 0 || file.Name.IndexOf(".SPF") > 0)
            //    {
            //        if (file.Name.IndexOf(".MPF") > 0)
            //            MainProgram = file.Name;
            //        string MainFileName = "Sinumerik/FileSystem/Part Program/AUTOPROCESS/PROCESS" + @"/" + file.Name;
            //        byte[] FileData;
            //        FileData = UaServer.ReadFile(file.FullName);
            //        m_MethodCallResult = UaServer.MethodCallCopyFileToServer("/Methods/CopyFileToServer", MainFileName, FileData, true);
            //    }
            //}
            #endregion


            #region 导入加工程序(新,网盘模式)
            string MainProgram = null;
            //删除目录中的文件

            List<string> ProgramFold = new List<string>();
            string ShareFold = string.Format(@"C:\Users\Lenovo\Documents\PROCESS\{0}-{1}", _EquipId > 52 ? 3 : 5, _EquipId > 52 ? _EquipId - 52 : _EquipId - 50);

            string[] filenames = Directory.GetFiles(ShareFold);

            foreach (string file in filenames)
            {
                File.Delete(file);
            }
            string ProcessType = _ProcessName.Split('-')[2];
            //导入加工程序
            string sourceFolder = string.Format(@"C:\Users\Lenovo\Documents\rfmx\{0}\加工程序\{1}\{2}", _ProductName, _ProcessName, ProcessType);
            DirectoryInfo theMainFolder = new DirectoryInfo(sourceFolder);
            CopyFolder(sourceFolder, ShareFold);
            foreach (FileInfo file in theMainFolder.GetFiles())
            {
                if (file.Name.IndexOf(".MPF") > 0 || file.Name.IndexOf(".SPF") > 0)
                {
                    if (file.Name.IndexOf(".MPF") > 0)
                        MainProgram = file.Name;
                }
            }
            #endregion

            /***********************************************导入刀具列表********************************************/
            #region 导入刀具列表
            //List<string> variables = new List<string>() { new NodeId(@"/Tool/Catalogue/numTools[u1]", 2).ToString() };
            //List<String> values = UaServer.ReadValues(variables);
            //int toolNumbers = Convert.ToUInt16(values[0]);

            //List<String> toolNo_NodeID = new List<string>();
            //List<String> toolNo_Value = new List<string>();
            //if (toolNumbers != 0)
            //{
            //    for (int i = 1; i <= toolNumbers; i++)
            //    {
            //        toolNo_NodeID.Add(new NodeId(string.Format(@"/Tool/Catalogue/toolNo[u1,{0}]", i.ToString()), 2).ToString());
            //    }
            //    toolNo_Value = UaServer.ReadValues(toolNo_NodeID);

            //    for (int i = 0; i < toolNumbers; i++)
            //    {
            //        UaServer.MethodCallDeleteTool("1", toolNo_Value[i]);
            //    }
            //}
            ////导入新的刀具清单

            //toolNo_NodeID.Clear(); toolNo_Value.Clear();
            ////List<String> toolIdent_NodeID = new List<string>(); List<String> toolIdent_Value = new List<string>();
            //List<String> edgeData_NodeID = new List<string>(); List<String> edgeData_Value = new List<string>();

            ////读取刀具清单
            //Dictionary<string, string[]> ToolInfo = OverAll.ReadCutterFile(ToolListPath);
            ////数据库检索对应的刀具

            //foreach (var myKY in ToolInfo)
            //{
            //    UaServer.MethodCallCreateTool("1", myKY.Key);


            //    string ss = myKY.Key.ToString();
            //    List<String> ddd = new List<string>();
            //    List<String> sss = new List<string>();
            //    ddd.Add(new NodeId(string.Format(@"/Tool/Data/toolState[u1,{0}]", ss), 2).ToString());
            //    sss.Add("3");
            //    UaServer.WriteValues(sss, ddd);

            //    List<String> ddd1 = new List<string>();
            //    List<String> sss1 = new List<string>();
            //    ddd1.Add(new NodeId(string.Format(@"/Tool/Data/toolIdent[u1,{0}]", ss), 2).ToString());
            //    sss1.Add(ss);
            //    UaServer.WriteValues(sss1, ddd1);


            //    for (int i = 1; i < 26; i++)
            //    {
            //        if (myKY.Value[i - 1] != "")
            //        {
            //            edgeData_NodeID.Add(new NodeId(string.Format(@"/Tool/Compensation/edgeData[u1,c{0},{1}]", myKY.Key, i), 2).ToString());
            //            edgeData_Value.Add(myKY.Value[i - 1]);
            //        }

            //    }
            //    UaServer.WriteValues(edgeData_Value, edgeData_NodeID);

            //}
            #endregion
            return MainProgram;
        }
        /// <summary>
        /// 复制文件夹及文件
        /// </summary>
        /// <param name="sourceFolder">原文件路径</param>
        /// <param name="destFolder">目标文件路径</param>
        /// <returns></returns>
        public static int CopyFolder(string sourceFolder, string destFolder)
        {
            try
            {
                //如果目标路径不存在,则创建目标路径
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }
                //得到原文件根目录下的所有文件
                string[] files = System.IO.Directory.GetFiles(sourceFolder);
                foreach (string file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    System.IO.File.Copy(file, dest);//复制文件
                }
                //得到原文件根目录下的所有文件夹
                string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = System.IO.Path.GetFileName(folder);
                    string dest = System.IO.Path.Combine(destFolder, name);
                    CopyFolder(folder, dest);//构建目标路径,递归复制文件
                }
                return 1;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return 0;
            }

        }
        private static int TraverseProgram(Server UaServer, string nodeBrowseBoot, string Index, ref List<string> ProgramFold)
        {
            try
            {
                ReferenceDescriptionCollection refDescCol = UaServer.BrowseNode(new NodeId(nodeBrowseBoot, 2).ToString());

                if (refDescCol.Count != 0)
                {
                    foreach (ReferenceDescription tempRefDesc in refDescCol)
                    {
                        if (tempRefDesc.NodeClass == NodeClass.Object)
                        {
                            ProgramFold.Add(tempRefDesc.NodeId.Identifier.ToString());
                        }
                    }
                    //int iIndex = 0;
                    //for (int i = 0; i < refDescCol.Count; i++)
                    //{
                    //    if (refDescCol[i].NodeClass == NodeClass.Object)
                    //    {
                    //        iIndex++;
                    //        string Name = Index + iIndex.ToString() + "@" + refDescCol[i].DisplayName.ToString();
                    //        ProgramFold.Add(Name);

                    //        if (refDescCol[i].DisplayName.ToString().Contains(".MPF") || refDescCol[i].DisplayName.ToString().Contains(".SPF"))
                    //        {
                    //        }
                    //        else
                    //        {
                    //            TraverseProgram(UaServer, nodeBrowseBoot + @"/" + refDescCol[i].DisplayName.ToString(), Index + iIndex.ToString() + ".", ref ProgramFold);
                    //        }
                    //    }
                    //}
                }
                return refDescCol.Count;
            }
            catch (Exception ex)
            {
                
                LogHelper.WriteLog(typeof(Process), ex);
                return 0;
            }
        }

       
        public static int SearchProcessTask(string _EquipName)
        {
            
            int iTaryId = 0;
            //工序满足的托盘
            DataBaseprogram.db_TuoDao db_TuoDao = new DataBaseprogram.db_TuoDao();
            DataSet dsTaryInfo = db_TuoDao.QueryTaryProcess(_EquipName);
            int equipType = 0;

        //todo 暂时不知道干嘛的
       /*     DataBaseprogram.db_Product db_Product = new DataBaseprogram.db_Product();
            DataSet dsProcessSet =  db_Product.QueryProcessSet();
            int iProcessSet = Convert.ToInt32(dsProcessSet.Tables[0].Rows[0]["Process"]);
            if (iProcessSet == 0) {
                return iTaryId;
            }*/

            //判断是否有合适的刀具
            int iIndex = 0;
            for (; iIndex < dsTaryInfo.Tables[0].Rows.Count; iIndex++)
            {
                if (Meet_Tool(_EquipName, dsTaryInfo.Tables[0].Rows[iIndex]["chanType"].ToString(), dsTaryInfo.Tables[0].Rows[iIndex]["task"].ToString(), equipType))
                {
                    break;
                }
            }
            if (iIndex == dsTaryInfo.Tables[0].Rows.Count)
            {
                //没有可满足的产品可以加工，报警
                return iTaryId;
            }
            else
            {
                //添加搬运任务，并修改托盘状态
                DataBaseprogram.db_TotalTaskListDao db_TotalTaskLis = new DataBaseprogram.db_TotalTaskListDao();
                int iTarget;
             
                _EquipName = _EquipName.Replace("五轴", "5");
                iTarget = Convert.ToInt32(_EquipName);
                
                //修改托盘状态锁定
                iTaryId = Convert.ToInt32(dsTaryInfo.Tables[0].Rows[iIndex]["tuoId"]);
                DataBaseprogram.db_TuoDao cTrayInfo = new DataBaseprogram.db_TuoDao();
                object[] Info = new object[] { 3, null, null, null, null, null, null,null, "待加工锁定",DateTime.Now.ToString() };
                cTrayInfo.UpdateTrayInfo(iTaryId, Info);//锁定托盘，避免重复分配任务
                DataBaseprogram.db_EquipDao db_Product = new DataBaseprogram.db_EquipDao();

                object[] UpdateInfo = new object[4];
                UpdateInfo[0] = "待加工"; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = null;
                db_Product.UpdateEquipInfo(iTarget,UpdateInfo);
                //优先执行加工任务
                db_TotalTaskLis.InsertTotalTaskList(1, iTaryId, iTarget);
                return iTaryId;
            }
        }

        public static int SearchWashTask()
        {


            int iTaryId = 0;
            //工序满足的托盘
            //查看是否已经有待测量任务

            DataBaseprogram.db_TuoDao db_TrayInfo = new DataBaseprogram.db_TuoDao();

            DataSet dsTaryInfo = db_TrayInfo.QueryWashProcess("待清洗");
 
            if (dsTaryInfo.Tables[0].Rows.Count==0)
            {
                //没有可满足的产品可以加工，报警
                return iTaryId;
            }
            else
            {
                //添加搬运任务，并修改托盘状态
                DataBaseprogram.db_RgvtaskDao db_Task = new DataBaseprogram.db_RgvtaskDao();
                //修改托盘状态锁定
                iTaryId = Convert.ToInt32(dsTaryInfo.Tables[0].Rows[0]["tuoId"]);
                DataBaseprogram.db_TuoDao cTrayInfo = new DataBaseprogram.db_TuoDao();
                object[] Info = new object[] { 3, null, null, null, null, null, null,null, "待清洗锁定", DateTime.Now.ToString() };
                cTrayInfo.UpdateTrayInfo(iTaryId, Info);//锁定托盘，避免重复分配任务

                DataBaseprogram.db_EquipDao db_Product = new DataBaseprogram.db_EquipDao();
                object[] UpdateInfo = new object[4];
                UpdateInfo[0] =OverAll.TUO_STATE_TO_BE_CLEANED; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = null;
                db_Product.UpdateEquipInfo(OverAll.getEquipId("清洗房"), UpdateInfo);
               // db_Task.InsertRgvtasklist(0, iTaryId, OverAll.getEquipId("清洗房"));
                return iTaryId;
            }
        }
        /// <summary>
        /// 搜索装卸站任务
        /// </summary>
        /// <returns></returns>
         public static int SearchUnloadTask()
        {


            int iTaryId = 0;
            //工序满足的托盘
            //查看是否已经有待测量任务

            DataBaseprogram.db_TuoDao db_TrayInfo = new DataBaseprogram.db_TuoDao();

            DataSet dsTaryInfo = db_TrayInfo.QueryUnloadProcess(OverAll.TUO_STATE_TO_BE_PROCESSED);
 
            if (dsTaryInfo.Tables[0].Rows.Count==0)
            {
                //没有可满足的产品可以加工，报警
                return iTaryId;
            }
            else
            {
                //添加搬运任务，并修改托盘状态
                DataBaseprogram.db_RgvtaskDao db_Task = new DataBaseprogram.db_RgvtaskDao();
                //修改托盘状态锁定
                iTaryId = Convert.ToInt32(dsTaryInfo.Tables[0].Rows[0]["tuoId"]);
                DataBaseprogram.db_TuoDao cTrayInfo = new DataBaseprogram.db_TuoDao();
                object[] Info = new object[] { 3, null, null, null, null, null, null,null, OverAll.TUO_STATE_TO_BE_CLEANED_LOOK, DateTime.Now.ToString() };
                cTrayInfo.UpdateTrayInfo(iTaryId, Info);//锁定托盘，避免重复分配任务

                DataBaseprogram.db_EquipDao db_Product = new DataBaseprogram.db_EquipDao();
                object[] UpdateInfo = new object[4];
                UpdateInfo[0] = OverAll.TUO_STATE_TO_BE_CLEANED; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = null;
                db_Product.UpdateEquipInfo(56, UpdateInfo);
              //  db_Task.InsertRgvtasklist(0, iTaryId, OverAll.getEquipId("清洗房"));
                return iTaryId;
            }
        }

        public static bool Meet_Tool(string _EquipName, string _produce, string _Process,int EquipType)
        {
            ////基于产品和工序得到刀具使用清单
            //List<string> ToolList = new List<string>();

            //string ToolPath = string.Format(@"C:\Users\Lenovo\Documents\rfmx\{0}\加工程序\{1}\{2}\ToolList.txt", _produce, _Process, EquipType);
            //if (!System.IO.File.Exists(ToolPath))
            //{
            //    //创建刀具文件
            //    ToolManage.CreateToolList(_produce); ;
            //}
            //foreach (string ToolNo in File.ReadAllLines(ToolPath))
            //{
            //    ToolList.Add(ToolNo);
            //}
            ////判断刀具是否满足
            //List<string> NoMeet = new List<string>();//不满足的刀
            //DataBaseprogram.Db_Tool db_Tool = new DataBaseprogram.Db_Tool();
            //for (int i = 0; i < ToolList.Count; i++)
            //{
            //    if (!db_Tool.QueryToolNo(_EquipName, ToolList[i], 1))
            //        NoMeet.Add(ToolList[i]);
            //}
            //if (NoMeet.Count != 0)
            //    return false;
            return true;
        }

        /// <summary>
        /// 工序+1
        /// </summary>
        /// <param name="_iTaryId">托盘ID</param>
        /// <param name="_Process">+1后的工序号</param>
        public static bool ProcessPlus(int _iTaryId, ref string _Process)
        {
            DataBaseprogram.db_TuoDao db_TrayInfo = new DataBaseprogram.db_TuoDao();
            DataSet dsTaryInfo = db_TrayInfo.QueryTuoById(_iTaryId);
            string ProduceName = dsTaryInfo.Tables[0].Rows[0]["chanType"].ToString();
            if (ProduceName == "0")
            {
                return true;
            }

            DataBaseprogram.db_ProductDao db_Product = new DataBaseprogram.db_ProductDao();
            DataSet dsProduceInfo = db_Product.QueryProductInfo(ProduceName);
            int iProcessIndex = Convert.ToInt32(dsTaryInfo.Tables[0].Rows[0]["Task"].ToString().Split('-')[0]);
            int iProcessCount = Convert.ToInt32(dsProduceInfo.Tables[0].Rows[0]["stepNum"]);
            Dictionary<int, string> ProcessList = new Dictionary<int, string>();
            for (int i = 0; i < iProcessCount; i++)
            {
                //ProcessList.Add(i + 1, dsProduceInfo.Tables[0].Rows[0][string.Format(@"step{0}", i + 1)].ToString());
                if (i < 15)
                {
                    ProcessList.Add(i + 1, dsProduceInfo.Tables[0].Rows[0][string.Format(@"step{0}", i + 1)].ToString());
                }
                else
                {
                    ProcessList.Add(i + 1, dsProduceInfo.Tables[0].Rows[0][string.Format(@"step{0}", i + 11)].ToString());
                }
            }

            if (iProcessIndex < ProcessList.Count)
            {
                _Process = ProcessList[iProcessIndex + 1];
                return false;
            }
            else
            {
                return true;
            }
        }

        public static Dictionary<int,string> GetProcess(string _ProduceName) {
            Dictionary<int, string> ProcessList = new Dictionary<int, string>();
            string sourceFolder = string.Format(@"C:\Users\Lenovo\Documents\rfmx\{0}\加工程序", _ProduceName);
            string[] folders = System.IO.Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = System.IO.Path.GetFileName(folder);
                string[] NameS = name.Split('-');
                ProcessList.Add(Convert.ToInt32(NameS[0]), name);
            }
            return ProcessList;
        }

        public static string GetMpfName(string _ProduceName, string _ProcessName, int _EquipId)
        {

            string sourceFolder = string.Format(@"C:\Users\Lenovo\Documents\rfmx\{0}\加工程序\{1}\{2}", _ProduceName, _ProcessName, _EquipId > 52 ? 3 : 5);
            string[] files = System.IO.Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = System.IO.Path.GetFileName(file);
                if (name.IndexOf(".MPF") > 0)
                    return name;

            }
            return null;
        }

        /// <summary>
        /// 判断设备是否可以加工此产品
        /// </summary>
        /// <param name="_iEquipId">设备ID</param>
        /// <param name="_sProduce">产品名称</param>
        public static bool ProcessJudge(int _iEquipId,string _sProduce) {

            return true;
        }
    }
}
