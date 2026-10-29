using DSkin.Controls;

using DSkin.Forms;

using LoadStation.Services;

using LoadStation.Sql.pojo;

using System;

using System.Collections.Generic;

using System.Drawing;

using System.IO;

using System.IO.Compression;

using System.Linq;

using System.Threading.Tasks;

using System.Windows.Forms;

using static LoadStation.SQL.SqlSugarHelper;



namespace LoadStation

{

    public partial class TaskOperateForm : DSkinForm

    {

        private PeerTcpCommunicator _communicator;

        private const string DestinationIp = "localhost";

        private const int DestinationPort = 5432;

        private string m_OrderNum = null;

        private string m_ProduceType = null;

        private string m_FrockId = "0";

        private string m_ProduceNo;

        private int m_iTaryNo;

        private int m_iPreferential = 0;

        private int m_iTaskType;

        private bool bolReject = false;



        private bool _disposed = false;

        private Tuo preTuo = null;

        //重装的三种状态

        private int zhuang = 1;

        private long m_CurrentStationTaskId = 0; // 总控 Web API 装卸站任务 ID



        private string nextFrock = "";

        //装操作的初始化函数

        public TaskOperateForm(string sOrderID)

        {

            InitializeComponent();

            m_OrderNum = sOrderID;

            m_iTaskType = 1;//装

        }

        //卸载，重装，翻面

        public TaskOperateForm(int TaryNo, int Type)

        {

            InitializeComponent();

            m_iTaryNo = TaryNo;

            m_iTaskType = Type;

        }

        //根据操作的不同初始化界面

        private async void TaskOperateForm_Load(object sender, EventArgs e)

        {

            dSkinButton_PutOut.Enabled = false;

            dSkinButton_PutOut.Visible = false;

            StyleSet();

            #region 装

            if (m_iTaskType == 1)//装

            {

                dSkinLabel_Mark.Text = "装";

                dSkinLabel_OrderNo.Text = m_OrderNum;



                // 优先检查是否走统一 Web REST API (station_task) 流程

                if (ControlApiClient.UseWebStationTaskApi)

                {

                    var result = await ControlApiClient.CreateStationTaskAsync(m_OrderNum, "装");

                    if (!result.Success)

                    {

                        MessageBoxEx messageBoxEx = new MessageBoxEx();

                        messageBoxEx.ShowDialog("提示", result.message ?? "创建装载任务失败", "", MessageBoxEx.MessageBoxStyle.warning);

                        this.Close();

                        return;

                    }



                    var stationTask = result.data;

                    if (stationTask == null)

                    {

                        MessageBoxEx messageBoxEx = new MessageBoxEx();

                        messageBoxEx.ShowDialog("提示", "未获取到有效的装载任务数据", "", MessageBoxEx.MessageBoxStyle.warning);

                        this.Close();

                        return;

                    }



                    m_CurrentStationTaskId = stationTask.id;

                    m_ProduceType = stationTask.part_no;

                    m_FrockId = stationTask.fixture_no ?? "0";

                    m_iTaryNo = stationTask.tray_no;



                    dSkinLabel_ProduceType.Text = m_ProduceType;

                    dSkinLabel_FrockId.Text = @"夹具图号：" + m_FrockId;

                    dSkinLabel_TrayNo.Text = @"托盘编号：" + m_iTaryNo;



                    // 夹具/托盘库存预警拦截

                    if (!string.IsNullOrWhiteSpace(stationTask.warning_msg))

                    {

                        MessageBoxEx messageBoxEx = new MessageBoxEx();

                        messageBoxEx.ShowDialog("提示", stationTask.warning_msg, "", MessageBoxEx.MessageBoxStyle.warning);

                        this.Close();

                        return;

                    }



                    if (m_iTaryNo <= 0)

                    {

                        MessageBoxEx messageBoxEx = new MessageBoxEx();

                        messageBoxEx.ShowDialog("提示", "没有可用的空闲托盘，请等待！", "", MessageBoxEx.MessageBoxStyle.warning);

                        this.Close();

                        return;

                    }



                    // 断点续接：如果任务已经在途（InStation 或 Arrived），直接切换为出站阶段

                    if (stationTask.task_status == "InStation" || stationTask.task_status == "Arrived")

                    {

                        dSkinButton_InTo.Enabled = false;

                        dSkinButton_InTo.Visible = false;

                        dSkinButton_PutOut.Enabled = true;

                        dSkinButton_PutOut.Visible = true;

                        dSkinButton_Cancel.Visible = true;

                        lblAutoCheck.Visible = true;

                    }

                }

                else

                {

                    // 传统降级模式：直写数据库与本地解析

                    Order1 order = Db.Queryable<Order1>().Where(it => it.orderNum == m_OrderNum).ToList()[0];

                    dSkinLabel_ProduceType.Text = order.chanType;

                    Product product = Db.Queryable<Product>().Where(it => it.chanName == order.chanType).ToList()[0];

                    m_ProduceType = product.chanName;

                    List<string> frocks = HttpHelper.ReadTxt($@"\rfmx\加工程序\{product.chanName}\工装清单.txt");

                    if (frocks == null || frocks.Count <= 0)

                    {

                        m_FrockId = "0";

                    }

                    else if(string.IsNullOrWhiteSpace(frocks[0]))

                    {

                        MessageBoxEx messageBoxEx = new MessageBoxEx();

                        messageBoxEx.ShowDialog("提示", "没有读取到夹具配置", "", MessageBoxEx.MessageBoxStyle.warning);

                        this.Close();

                        return;

                    }

                    else

                    {

                        m_FrockId = frocks[0].Split('@')[1];

                    }

                    dSkinLabel_FrockId.Text = @"夹具图号：" + m_FrockId;

                    if (m_FrockId != "0")

                    {

                        Frockinfo frockinfo = Db.Queryable<Frockinfo>().Where(it => it.FrockInfoId == m_FrockId).ToList()[0];

                        int iFrockTotalCount = frockinfo.Count;

                        int iFreeCount = Db.Queryable<Tuo>().Where(it => it.FrockID == m_FrockId && it.visible == 0).Count();

                        int iTotalCount = Db.Queryable<Tuo>().Where(it => it.FrockID == m_FrockId).Count();

                        if (iFreeCount == 0)

                        {

                            if (iTotalCount < iFrockTotalCount)

                            {

                                MessageBoxEx messageBoxEx = new MessageBoxEx();

                                messageBoxEx.ShowDialog("提示", "托盘库上没有空闲的工装，请先装载夹具",

                                    string.Format("{0}类型的工装共{1}件目前已上架{2}", m_FrockId, iFrockTotalCount, iTotalCount), MessageBoxEx.MessageBoxStyle.warning);

                                this.Close();

                                return;

                            }

                            else if (iFrockTotalCount == iTotalCount)

                            {

                                MessageBoxEx messageBoxEx = new MessageBoxEx();

                                messageBoxEx.ShowDialog("提示", "所有夹具全部用完，请等待", "", MessageBoxEx.MessageBoxStyle.warning);

                                this.Close();

                                return;

                            }

                        }

                    }

                    List<Tuo> tuoList = Db.Queryable<Tuo>()

                        .Where(it => it.visible == 0 && it.FrockID == m_FrockId && it.position < 17)

                        .OrderByDescending(it => it.position)

                        .ToList();

                    if (tuoList.Count > 0)

                    {

                        m_iTaryNo = tuoList[0].tuoId;

                    }

                    else

                    {

                        MessageBoxEx messageBoxEx = new MessageBoxEx();

                        messageBoxEx.ShowDialog("提示", "没有空闲托盘，请等待", "", MessageBoxEx.MessageBoxStyle.warning);

                        this.Close();

                        return;

                    }

                    dSkinLabel_TrayNo.Text = @"托盘编号：" + m_iTaryNo;

                }

            }

            #endregion

            #region 卸载或翻面

            else if (m_iTaskType == 2 || m_iTaskType == 4)

            {   //卸 或 翻面

                dSkinLabel_Mark.Text = m_iTaskType == 4 ? "卸" : "翻面";

                Tuo tuo = Db.Queryable<Tuo>().InSingle(m_iTaryNo);

                if (tuo != null)

                {

                    //托盘编号

                    dSkinLabel_TrayNo.Text = @"编号：" + m_iTaryNo.ToString();

                    //产品型号

                    m_ProduceType = tuo.chanType;

                    dSkinLabel_ProduceType.Text = m_ProduceType;

                    //工艺

                    //订单编号

                    m_OrderNum = tuo.OrderId;

                    dSkinLabel_OrderNo.Text = m_OrderNum;

                    //产品编号

                    m_ProduceNo = tuo.chanNo;

                    dSkinTextBox_ProduceNo.Text = m_ProduceNo != null ? m_ProduceNo.ToString() : "";

                    m_FrockId = tuo.FrockID ?? "0";

                    dSkinLabel_FrockId.Text = @"夹具图号：" + m_FrockId;

                }



                // 翻面 / 卸载 Web API 模式

                if ((m_iTaskType == 2 || m_iTaskType == 4) && ControlApiClient.UseWebStationTaskApi)

                {

                    string actionType = m_iTaskType == 4 ? "卸" : "翻";

                    var result = await ControlApiClient.CreateStationTaskAsync(m_OrderNum, actionType, m_iTaryNo);

                    if (!result.Success)

                    {

                        MessageBoxEx messageBoxEx = new MessageBoxEx();

                        messageBoxEx.ShowDialog("提示", result.message ?? $"创建{dSkinLabel_Mark.Text}任务失败", "", MessageBoxEx.MessageBoxStyle.warning);

                        this.Close();

                        return;

                    }



                    var stationTask = result.data;

                    if (stationTask == null)

                    {

                        MessageBoxEx messageBoxEx = new MessageBoxEx();

                        messageBoxEx.ShowDialog("提示", $"未获取到有效的{dSkinLabel_Mark.Text}任务数据", "", MessageBoxEx.MessageBoxStyle.warning);

                        this.Close();

                        return;

                    }



                    m_CurrentStationTaskId = stationTask.id;

                    if (!string.IsNullOrWhiteSpace(stationTask.product_no))

                    {

                        m_ProduceNo = stationTask.product_no;

                        dSkinTextBox_ProduceNo.Text = m_ProduceNo;

                    }



                    // 智能检测：若托盘已在22号装卸站台就绪或处于搬运入站状态，直接激活【出站】按钮

                    Equip station22 = Db.Queryable<Equip>().InSingle(22);

                    bool isAtStation = (station22 != null && station22.TrayId == m_iTaryNo) || (tuo != null && tuo.position == 22);

                    bool isOngoingInStation = stationTask.task_status != "待执行" && stationTask.task_status != "完成";



                    if (isAtStation || isOngoingInStation)

                    {

                        lblAutoCheck.Visible = true;

                        dSkinButton_InTo.Enabled = false;

                        dSkinButton_InTo.Visible = false;

                        dSkinButton_PutOut.Enabled = true;

                        dSkinButton_PutOut.Visible = true;

                        dSkinButton_Cancel.Visible = true;

                    }

                    else

                    {

                        lblAutoCheck.Visible = false;

                        dSkinButton_InTo.Enabled = true;

                        dSkinButton_InTo.Visible = true;

                        dSkinButton_PutOut.Enabled = false;

                        dSkinButton_PutOut.Visible = false;

                        dSkinButton_Cancel.Visible = false;

                    }

                }

            }

            #endregion

            #region 重装

            else if (m_iTaskType == 3)

            {

                dSkinLabel_Mark.Text = "重装";

                Tuo tuo = Db.Queryable<Tuo>().InSingle(m_iTaryNo);

                //托盘编号

                dSkinLabel_TrayNo.Text = @"编号：" + m_iTaryNo.ToString();

                //产品型号

                m_ProduceType = tuo.chanType;

                dSkinLabel_ProduceType.Text = m_ProduceType;

                //工艺

                //订单编号

                m_OrderNum = tuo.OrderId;

                dSkinLabel_OrderNo.Text = m_OrderNum;

                //产品编号

                m_ProduceNo = tuo.chanNo;

                dSkinTextBox_ProduceNo.Text = m_ProduceNo.ToString();

                //读取重装信息

                try

                {

                    string[] ProcessList = tuo.task.Split('-');

                    int current = Convert.ToInt32(ProcessList[0]);

                    //当前工序

                    //打开文件读取工装

                    List<string> frocks = HttpHelper.ReadTxt($@"\rfmx\加工程序\{tuo.chanType}\工装清单.txt");

                    if (frocks == null || frocks.Count <= 0)

                    {

                        //不可能的错误

                        throw new Exception("夹具文件为空");

                    }

                    else

                    {

                        //重装的工装号

                        nextFrock = frocks[current].Split('@')[1];

                    }

                    //检索该工装的托盘

                    List<Tuo> freeTrays = Db.Queryable<Tuo>().Where(it => it.FrockID == nextFrock && it.visible == 0).ToList();

                    if (freeTrays.Count <= 0)

                    {

                        throw new Exception("夹具不足");

                    }

                    freeTrays[0].visible = 9; freeTrays[0].chanState = "待换装";

                    Db.Updateable(freeTrays[0]).ExecuteCommand();



                    //出站时需要更改当前托盘上的工装类型。

                    dSkinLabel5.Text = "需要更换为托盘: " + freeTrays[0].tuoId;

                    zhuang = 1;

                }

                catch (Exception)

                {

                    dSkinLabel5.Text = "夹具不足,需要的夹具号为:" + nextFrock;

                }

            }

            #endregion

            //先判断本地文件加中有没有

            LoadProduceImage(m_ProduceType);

            this.FormClosing += (s, ee) =>

            {

                dSkinPictureBox_Produce.Image?.Dispose();

                dSkinPictureBox_Process.Image?.Dispose();

                components?.Dispose();

                base.Dispose(true);

            };

        }



        private async void btnTest_Click(object sender, EventArgs e)

        {

            await _communicator.SendAsync("localhost", DestinationPort, "loadstation");

        }



        //根据操作的不同设置不同的颜色风格

        private void StyleSet()

        {

            foreach (Control ctl in Controls)

            {

                if (ctl is DSkin.Controls.DSkinLabel)

                {

                    if (ctl is DSkin.Controls.DSkinButton)

                    {

                        if (m_iTaskType == 1)

                        {

                            ((DSkin.Controls.DSkinButton)ctl).BaseColor = Color.FromArgb(77, 220, 228);

                            ((DSkin.Controls.DSkinButton)ctl).ButtonBorderColor = Color.FromArgb(77, 220, 228);

                        }

                        else if (m_iTaskType == 2)

                        {

                            ((DSkin.Controls.DSkinButton)ctl).BaseColor = Color.FromArgb(255, 128, 0);

                            ((DSkin.Controls.DSkinButton)ctl).ButtonBorderColor = Color.FromArgb(77, 220, 228);

                        }

                        else if (m_iTaskType == 3)

                        {

                            ((DSkin.Controls.DSkinButton)ctl).BaseColor = Color.DodgerBlue;

                            ((DSkin.Controls.DSkinButton)ctl).ButtonBorderColor = Color.FromArgb(77, 220, 228);

                        }

                        else

                        {

                            ((DSkin.Controls.DSkinButton)ctl).BaseColor = Color.AliceBlue;

                            ((DSkin.Controls.DSkinButton)ctl).ButtonBorderColor = Color.FromArgb(77, 220, 228);

                        }

                    }

                    else

                    {

                        if (m_iTaskType == 1)

                        {

                            ctl.ForeColor = Color.FromArgb(77, 220, 228);

                        }

                        else if (m_iTaskType == 2)

                        {

                            ctl.ForeColor = Color.FromArgb(255, 128, 0);

                        }

                        else if (m_iTaskType == 3)

                        {

                            ctl.ForeColor = Color.DodgerBlue;

                        }

                        else

                        {

                            ctl.ForeColor = Color.AliceBlue;

                        }

                    }

                }

            }

        }

        private byte[] ExtractSingleFileFromZip(byte[] zipBytes)

        {

            using (var zipStream = new MemoryStream(zipBytes))

            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Read))

            {

                var entry = archive.Entries.FirstOrDefault();

                if (entry == null) throw new Exception("zip 包为空");

                using (var entryStream = entry.Open())

                using (var ms = new MemoryStream())

                {

                    entryStream.CopyTo(ms);

                    return ms.ToArray();

                }

            }

        }

        //图片加载

        private async void LoadProduceImage(string ProduceName)

        {

            string[] producePicPath = HttpHelper.GetAllFilePath(string.Format("\\picture\\{0}\\产品图片", ProduceName));

            string[] processPicPath = HttpHelper.GetAllFilePath(string.Format("\\picture\\{0}\\工艺图片", ProduceName));



            if (producePicPath != null && producePicPath.Length > 0)  // 修正：检查自身 Length

            {

                byte[] zipBytes = await Task.Run(() => HttpHelper.GetSingleRaw(producePicPath[0]));

                if (zipBytes != null && zipBytes.Length > 0)

                {

                    byte[] imageBytes = ExtractSingleFileFromZip(zipBytes);

                    dSkinPictureBox_Produce.Image?.Dispose();

                    dSkinPictureBox_Produce.Image = Image.FromStream(new MemoryStream(imageBytes));

                }

            }



            if (processPicPath != null && processPicPath.Length > 0)

            {

                byte[] zipBytes = await Task.Run(() => HttpHelper.GetSingleRaw(processPicPath[0]));

                if (zipBytes != null && zipBytes.Length > 0)

                {

                    byte[] imageBytes = ExtractSingleFileFromZip(zipBytes);

                    dSkinPictureBox_Process.Image?.Dispose();

                    dSkinPictureBox_Process.Image = Image.FromStream(new MemoryStream(imageBytes));

                }

            }

        }



        //入站按钮点击事件

        private async void DSkinButton_InTo_Click(object sender, EventArgs e)

        {

            // Web API 模式 (装、翻面、卸载)

            if ((m_iTaskType == 1 || m_iTaskType == 2 || m_iTaskType == 4) && ControlApiClient.UseWebStationTaskApi && m_CurrentStationTaskId > 0)

            {

                var res = await ControlApiClient.InStationAsync(m_CurrentStationTaskId, m_iTaryNo, m_FrockId);

                if (!res.Success)

                {

                    MessageBoxEx messageBoxEx = new MessageBoxEx();

                    messageBoxEx.ShowDialog("错误", res.message ?? "入站请求失败！", "", MessageBoxEx.MessageBoxStyle.warning);

                    return;

                }



                MessageBoxEx msg = new MessageBoxEx();

                msg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);

                lblAutoCheck.Visible = true;

                dSkinButton_InTo.Enabled = false;

                dSkinButton_InTo.Visible = false;

                dSkinButton_PutOut.Enabled = true;

                dSkinButton_PutOut.Visible = true;

                dSkinButton_Cancel.Visible = true;

                return;

            }

            Equip load = Db.Queryable<Equip>().InSingle(22);

            //判断装卸站是否有托盘

            if (load.TrayId != 0)

            {

                MessageBoxEx messageBoxEx = new MessageBoxEx();

                messageBoxEx.ShowDialog("错误", "托盘已经入站，请勿重复操作！", "", MessageBoxEx.MessageBoxStyle.warning);

                return;

            }

            List<Rgvtasklist> taskList = Db.Queryable<Rgvtasklist>().Where(it => it.Initial == m_iTaryNo || it.Target == m_iTaryNo).ToList();

            //判断要操作的托盘是否已经有搬运任务

            if (taskList.Count > 0)

            {

                MessageBoxEx msg = new MessageBoxEx();

                msg.ShowDialog("错误", "托盘正在搬运中，请等待！", "", MessageBoxEx.MessageBoxStyle.normal);

                return;

            }

            //翻面或重装任务

            //if (m_iTaskType == 5 && zhuang == 2)

            //{

            //    List<Tuo> freeTrays = Db.Queryable<Tuo>().Where(it => it.visible == 9 && it.chanState == "待换装").ToList();

            //    if (freeTrays == null || freeTrays.Count <= 0)

            //    {

            //        throw new Exception("工装不足");

            //    }

            //    m_iTaryNo = freeTrays[0].tuoId;

            //    dSkinLabel_FrockId.Text = @"工装图号：" + nextFrock;

            //    dSkinLabel_TrayNo.Text = @"编号：" + m_iTaryNo.ToString();

            //}

            //更新托盘状态

            Tuo tuo = Db.Queryable<Tuo>().InSingle(m_iTaryNo);

            if (preTuo == null)

            {

                preTuo = tuo;

            }

            else

            {

                tuo.visible = preTuo.visible; tuo.chanNo = preTuo.chanNo;

                tuo.OrderId = preTuo.OrderId; tuo.chanType = preTuo.chanType;

                tuo.task = preTuo.task;

            }

            tuo.visible = m_iTaskType;

            string state;

            if (m_iTaskType == 1)

                state = "待装载";

            else if (m_iTaskType == 2)

                state = "待翻面";

            else if (m_iTaskType == 3)

                state = "待重装";

            else

                state = "待卸载";

            tuo.chanState = state;

            tuo.dateStart = DateTime.Now;

            Db.Updateable(tuo).ExecuteCommand();

            //更新 装卸站状态

            load.equipState = "等待托盘";

            Db.Updateable(load).ExecuteCommand();

            //插入rgv任务

            int taskId = Db.Insertable(new Rgvtasklist(tuo.position, 22, m_iTaryNo, 0, 1, "创建")).ExecuteReturnIdentity(); 

            //发送执行命令给总控

            try

            {

                await _communicator.SendAsync(DestinationIp, DestinationPort, $"入站:{taskId.ToString()}");

                MessageBoxEx msg = new MessageBoxEx();

                msg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);

            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.ToString());

            }

            lblAutoCheck.Visible = true;

            //dSkinButton_InTo.Enabled = false;

            //dSkinButton_InTo.Visible = false;

            //dSkinButton_PutOut.Enabled = true;

            //dSkinButton_PutOut.Visible = true;

            //dSkinButton_Cancel.Visible = true;

            return;

        }

        //出站按钮点击事件 产量记录todo 

        private async void DSkinButton_PutOut_Click(object sender, EventArgs e)

        {

             // 确认rgv出站 

            MessageConfirm fa = new MessageConfirm();

            DialogResult sss = fa.ShowDialog("警告", "穿梭车是否空闲？");

            if (DialogResult.Cancel == sss)

            {

                return;

            }

            // 确认是否入站完成

            Equip load = Db.Queryable<Equip>().InSingle(22);

            if (load.TrayId == 0)

            {

                MessageBoxEx messageBoxEx = new MessageBoxEx();

                messageBoxEx.ShowDialog("错误", "还未入站，请先入站或等待入站完成！", "", MessageBoxEx.MessageBoxStyle.warning);

                return;

            }

            // 确认托盘是否存在任务

            List<Rgvtasklist> taskList = Db.Queryable<Rgvtasklist>().Where(it => it.Initial == m_iTaryNo || it.Target == m_iTaryNo).ToList();

            //判断要操作的托盘是否已经有任务

            if (taskList.Count > 0)

            {

                MessageBoxEx msg = new MessageBoxEx();

                msg.ShowDialog("错误", "托盘存在搬运任务，请等待！", "", MessageBoxEx.MessageBoxStyle.normal);

                return;

            }

            //查询托盘

            Tuo tuo = Db.Queryable<Tuo>().InSingle(m_iTaryNo);

            //查询订单

            Order1 order = Db.Queryable<Order1>().Where(it => it.orderNum == m_OrderNum).ToList()[0];

            // 装

            if (m_iTaskType == 1)

            {

                if (string.IsNullOrWhiteSpace(dSkinTextBox_ProduceNo.Text))

                {

                    MessageBoxEx msg = new MessageBoxEx();

                    msg.ShowDialog("提示", "请输入产品编号！", "", MessageBoxEx.MessageBoxStyle.normal);

                    return;

                }

                m_ProduceNo = dSkinTextBox_ProduceNo.Text.Trim();



                // Web API 模式

                if (ControlApiClient.UseWebStationTaskApi && m_CurrentStationTaskId > 0)

                {

                    var res = await ControlApiClient.OutStationAsync(m_CurrentStationTaskId, m_iTaryNo, m_ProduceNo);

                    if (!res.Success)

                    {

                        MessageBoxEx msg = new MessageBoxEx();

                        msg.ShowDialog("错误", res.message ?? "出站处理失败！", "", MessageBoxEx.MessageBoxStyle.error);

                        return;

                    }



                    MessageBoxEx successMsg = new MessageBoxEx();

                    successMsg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);

                    dSkinButton_Cancel.Enabled = true;

                    dSkinButton_Cancel.Visible = true;

                    dSkinButton_PutOut.Enabled = false;

                    dSkinButton_PutOut.Visible = false;

                    this.Close();

                    return;

                }



                // 传统降级模式：直写数据库

                tuo.visible = 11; tuo.OrderId = order.orderNum; tuo.chanType = order.chanType; tuo.chanNo = m_ProduceNo;

                tuo.preferential = Convert.ToInt32(order.youXian); tuo.task = "0"; tuo.chanState = "装载完成"; tuo.dateStart = DateTime.Now;

                int recordCount = Db.Queryable<Orderre>().Where(it => it.orderNum == m_OrderNum).Count();

                if (recordCount == 0)

                {

                    Db.Insertable(new Orderre(m_OrderNum, order.chanType, order.totalNum, 0)).ExecuteCommand();

                }

                Db.Insertable(new OrderRecord(tuo)).ExecuteCommand();

                order.zhuang--;

                order.kuNum--;

                if (order.startTime == null)

                {

                    order.startTime = DateTime.Now;

                }

            }

            // 卸

            else if (m_iTaskType == 4)

            {

                m_ProduceNo = dSkinTextBox_ProduceNo.Text.Trim();



                // Web API 模式

                if (ControlApiClient.UseWebStationTaskApi && m_CurrentStationTaskId > 0)

                {

                    var res = await ControlApiClient.OutStationAsync(m_CurrentStationTaskId, m_iTaryNo, m_ProduceNo);

                    if (!res.Success)

                    {

                        MessageBoxEx msg = new MessageBoxEx();

                        msg.ShowDialog("错误", res.message ?? "卸载出站处理失败！", "", MessageBoxEx.MessageBoxStyle.error);

                        return;

                    }



                    MessageBoxEx successMsg = new MessageBoxEx();

                    successMsg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);

                    dSkinButton_Cancel.Enabled = true;

                    dSkinButton_Cancel.Visible = true;

                    dSkinButton_PutOut.Enabled = false;

                    dSkinButton_PutOut.Visible = false;

                    this.Close();

                    return;

                }



                // 传统降级模式：直写数据库与 TCP 发送

                OrderRecord or = Db.Queryable<OrderRecord>().Where(it => it.orderId == tuo.OrderId && it.chanNo == tuo.chanNo).ToList()[0];

                tuo.visible = 14; tuo.OrderId = "0"; tuo.chanType = "0";

                tuo.chanNo = "0"; tuo.preferential = 0; tuo.task = "0"; tuo.chanState = "卸载完成"; tuo.dateStart = DateTime.Now;

                //更新历史订单

                Orderre orderre = Db.Queryable<Orderre>().Where(it => it.orderNum == m_OrderNum).ToList()[0];

                orderre.okNum++;

                orderre.hegeNum++;

                orderre.okTime = DateTime.Now;

                Db.Updateable(orderre).ExecuteCommand();

                //更新订单

                order.okNum++;

                order.hegeNum++;

                if (order.okNum == orderre.totalNum)

                {

                    order.endTime = DateTime.Now;

                }

                //更新订单记录

                or.chanState = "完成"; or.task = "0"; or.updateTime = DateTime.Now;

                Db.Updateable<OrderRecord>(or).ExecuteCommand();

                //todo 产量信息记录



            }

            // 翻面

            else if (m_iTaskType == 2)

            {

                m_ProduceNo = dSkinTextBox_ProduceNo.Text.Trim();

                if (string.IsNullOrWhiteSpace(m_ProduceNo))

                {

                    MessageBoxEx msg = new MessageBoxEx();

                    msg.ShowDialog("提示", "产品编号不能为空！", "", MessageBoxEx.MessageBoxStyle.normal);

                    return;

                }



                // Web API 模式

                if (ControlApiClient.UseWebStationTaskApi && m_CurrentStationTaskId > 0)

                {

                    var res = await ControlApiClient.OutStationAsync(m_CurrentStationTaskId, m_iTaryNo, m_ProduceNo);

                    if (!res.Success)

                    {

                        MessageBoxEx msg = new MessageBoxEx();

                        msg.ShowDialog("错误", res.message ?? "出站处理失败！", "", MessageBoxEx.MessageBoxStyle.error);

                        return;

                    }



                    MessageBoxEx successMsg = new MessageBoxEx();

                    successMsg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);

                    dSkinButton_Cancel.Enabled = true;

                    dSkinButton_Cancel.Visible = true;

                    dSkinButton_PutOut.Enabled = false;

                    dSkinButton_PutOut.Visible = false;

                    this.Close();

                    return;

                }



                tuo.visible = 12; 

                tuo.chanState = "翻面完成";

                tuo.dateStart = DateTime.Now;

            }

            //换装

            else if (m_iTaskType == 3)

            {

                if (zhuang == 1)

                {

                    tuo.visible = 0; tuo.OrderId = "0"; tuo.chanType = "0";

                    tuo.chanNo = "0"; tuo.preferential = 0; tuo.task = "0"; tuo.chanState = "空闲"; tuo.dateStart = DateTime.Now;

                    zhuang = 2;



                }

                else if (zhuang == 2)

                {

                    tuo.dateStart = DateTime.Now;

                    zhuang = 3; 

                }

            }

            preTuo = null;

            // 更新托盘状态

            Db.Updateable(tuo).ExecuteCommand();

            // 更新订单状态

            Db.Updateable(order).ExecuteCommand();

            //交给Control新建任务

            //MoveOutTrayToEmptyStation(load);

            //Db.Insertable(new Rgvtasklist(22, m_iTaryNo, m_iTaryNo)).ExecuteCommand();

            try

            {

                await _communicator.SendAsync(DestinationIp, DestinationPort, $"出站:{tuo.tuoId}");

                MessageBoxEx msg = new MessageBoxEx();

                msg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);

            }

            catch (Exception ex)

            {

                MessageBoxEx msg = new MessageBoxEx();

                msg.ShowDialog("错误", ex.ToString(), "", MessageBoxEx.MessageBoxStyle.error);

                return;

            }

            dSkinButton_Cancel.Enabled = true;

            dSkinButton_Cancel.Visible = true;

            dSkinButton_PutOut.Enabled = false;

            dSkinButton_PutOut.Visible = false;

        }

        //返回按钮点击事件 

        private void dSkinButton_Cancel_Click(object sender, EventArgs e)

        {

            Equip load = Db.Queryable<Equip>().InSingle(22);

            if (load.TrayId != 0)

            {

                MessageBoxEx msg = new MessageBoxEx();

                msg.ShowDialog("提示", "当前装卸站中有产品，无法关闭！", "", MessageBoxEx.MessageBoxStyle.warning);

                return;

            }

            if (zhuang == 2)

            {

                MessageBoxEx msg = new MessageBoxEx();

                msg.ShowDialog("提示", "换装中，无法关闭！", "", MessageBoxEx.MessageBoxStyle.warning);

                return;

            }

            // 如果处于 Web API 模式且尚未呼叫入站，尝试取消当前创建的临时任务

            if ((m_iTaskType == 1 || m_iTaskType == 2 || m_iTaskType == 4) && ControlApiClient.UseWebStationTaskApi && m_CurrentStationTaskId > 0)

            {

                try

                {

                    ControlApiClient.CancelStationTaskAsync(m_CurrentStationTaskId).Wait(1000);

                }

                catch { }

            }

            this.Close();

        }

        //点击图片查看大图

        private void DSkinPictureBox_Process_Click(object sender, EventArgs e)

        {

            string[] picPath;

            if (dSkinPictureBox_Produce == sender as DSkinPictureBox)

            {

                picPath = HttpHelper.GetAllFilePath(string.Format("\\picture\\{0}\\产品图片", m_ProduceType));

            }

            else

            {

                picPath = HttpHelper.GetAllFilePath(string.Format("\\picture\\{0}\\工艺图片", m_ProduceType));

            }

            if (picPath == null || picPath.Length < 1) return;

            ImageShow imageShow = new ImageShow(picPath);

            imageShow.ShowDialog();

        }



        private void Communicator_OnMessageReceived(string message, string endpoint)

        {

            if (this.InvokeRequired)

            {

                this.Invoke(new Action(() => Communicator_OnMessageReceived(message, endpoint)));

                return;

            }

            Console.WriteLine($"from {endpoint}, received: {message}");

            lblAutoCheck.Visible = true;   // ← 加这行

            if (message == "自检通过")

            {

                lblAutoCheck.ForeColor = Color.Green;

                lblAutoCheck.Text = "自检通过，可以继续执行出站";

                dSkinButton_PutOut.Enabled = true;

                dSkinButton_PutOut.Visible = true;

            }

            else

            {

                lblAutoCheck.Text = message ;

            }

        }

        private async void TaskOperateForm_Shown(object sender, EventArgs e)

        {

            _communicator?.Dispose();   // 先清理旧的

            _communicator = new PeerTcpCommunicator(4321);

            _communicator.OnMessageReceived += Communicator_OnMessageReceived;



            await _communicator.StartListeningAsync();

        }



        private void TaskOperateForm_FormClosing(object sender, FormClosingEventArgs e)

        {

            _communicator?.Dispose();

        }

    }

}

