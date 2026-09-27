using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Opc.Ua;
using Opc.Ua.Client;
using SinumerikOpcUaAPI;
using SinumerikVariables;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Control
{
    public partial class FormControl : Form
    {
        #region 工位编号
        /*
         * 1-12 托盘库
         * 80穿梭车
         * 21装卸站、22清洗房、23缓存位
         * 31AGV1、32AGV2、30AGV
         * 41-44 线外缓存位
         * 51机床1、52机床2、53机床1缓存位、54机床2缓存位
         */
        #endregion
        #region OPC变量

        private Server m_UaServer_M_1 = null;
        private Subscription m_UaSubscipition_M_1 = null;
        private Session m_Session_M_1 = null;
        private Subscription m_AlarmSub_M_1 = null;
        private MonitoredItem m_AlarmMonitorItem_M_1 = null;

        private Server m_UaServer_M_2 = null;
        private Subscription m_UaSubscipition_M_2 = null;
        private Session m_Session_M_2 = null;
        private Subscription m_AlarmSub_M_2 = null;
        private MonitoredItem m_AlarmMonitorItem_M_2 = null;
      
        private Server m_UaServer_Plc = null;
        private Subscription m_UaSubscipition_Plc = null;
        private Session m_Session_Plc = null;
        private Subscription m_AlarmSub_Plc = null;
        private MonitoredItem m_AlarmMonitorItem_Plc = null;
      
        private Server m_UaServer_Wash = null;
        private Subscription m_UaSubscipition_Wash = null;
        private Session m_Session_Wash = null;
        private Subscription m_AlarmSub_Wash = null;
        private MonitoredItem m_AlarmMonitorItem_Wash = null;
        #endregion
        List<MonitoredItem> monitoredItems_M_1 = new List<MonitoredItem>();
        List<MonitoredItem> monitoredItems_M_2 = new List<MonitoredItem>();
        List<MonitoredItem> monitoredItems_Plc = new List<MonitoredItem>();
        List<MonitoredItem> monitoredItems_Wash = new List<MonitoredItem>();
        #region 设备IP
        static private string IP_M1 = "opc.tcp://10.50.42.9:4840";
        static private string IP_M2 = "opc.tcp://10.50.42.11:4840";
        static private string IP_PLC = "opc.tcp://10.50.42.1:4840";
        static private string IP_Wash = "opc.tcp://10.50.42.12:4840";
        bool[] m_ConnectState = new bool[5];
        #endregion

        delegate void deleMethod_KeepAlive(Session sender, KeepAliveEventArgs e);
        delegate void deleMethod_ServerCertificate(CertificateValidator cert, CertificateValidationEventArgs e);

    

        #region socket 装卸站
        private Socket ServerSocket_Load = null;//服务端  
        private Dictionary<string, MySession> dic_ClientSocket_Load = new Dictionary<string, MySession>();//tcp客户端字典
        private Dictionary<string, Thread> dic_ClientThread_Load = new Dictionary<string, Thread>();//线程字典,每新增一个连接就添加一条线程
        private bool Flag_Listen_Load = true;//监听客户端连接的标志
        #endregion

        #region 结构体
        struct itemTag
        {
            public object Tag;
            public string Text;
            public itemTag(object tag, string text)
            {
                this.Tag = tag;
                this.Text = text;
            }
            public override string ToString()
            {
                return this.Text;
            }
        }
        #endregion

        #region AGV
        public string Middle { get; } = "/rcms/services/rest/hikRpcService/";
        public string ListenerPort { get; } = "http://10.50.39.97:8080/agvCallback/";
        public string ListenerPortWarning { get; } = "http://10.50.39.97:8080/service/rest/agvCallbackService/warnCallback/";
        private HttpListener httpListener;
        private HttpListener httpListenerWarning;
        private Thread listenerThread;
        private Thread listenerThreadWarning;
        #endregion 
        public FormControl()
        {
            InitializeComponent();
        }

        #region OPC UA初始化
        //MachineIp ip地址
        private void ConnectOpc(string MachineIp, Server UaServer, ref Session UaSession, ushort NameSpace, deleMethod_KeepAlive Notification_KeepAlive, deleMethod_ServerCertificate Notification_ServerCertificate)
        {

            if (UaSession != null && !UaSession.Disposed)
            {

            }
            else
            {
                try
                {
                    EndpointDescription m_SelectedEndpoint;

                    ApplicationDescriptionCollection servers = UaServer.FindServers(MachineIp);
                    foreach (ApplicationDescription ad in servers)
                    {
                        foreach (string url in ad.DiscoveryUrls)
                        {
                            if (url == MachineIp)
                            {
                                EndpointDescriptionCollection endpoints = UaServer.GetEndpoints(url);
                                foreach (EndpointDescription ep in endpoints)
                                {
                                    string securityPolicy = ep.SecurityPolicyUri.Remove(0, 42);
                                 
                                    if (ep.EndpointUrl == MachineIp && ep.SecurityMode == Opc.Ua.MessageSecurityMode.Sign && securityPolicy == "#Basic128Rsa15")
                                    {

                                        string s = "[" + ad.ApplicationName + "] " + " [" + ep.SecurityMode + "] " + " [" + securityPolicy + "] " + " [" + ep.EndpointUrl + "]";
                                        itemTag item = new itemTag(ep, s);

                                        m_SelectedEndpoint = (EndpointDescription)item.Tag;
                                        UaServer.NAMESPACE_INDEX = NameSpace;
                                        UaServer.KeepAliveNotification += new KeepAliveEventHandler(Notification_KeepAlive);
                                        UaServer.CertificateValidationNotification += new CertificateValidationEventHandler(Notification_ServerCertificate);
                                        UaServer.Connect(m_SelectedEndpoint, true, "OpcUaClient", "RIFA1234");
                                        UaSession = UaServer.Session;
                                        return;
                                    }
                                    
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }
        private void ConnectOpc_Plc(string MachineIp, Server UaServer, ref Session UaSession, ushort NameSpace, deleMethod_KeepAlive Notification_KeepAlive, deleMethod_ServerCertificate Notification_ServerCertificate)
        {

            if (UaSession != null && !UaSession.Disposed)
            {

            }
            else
            {
                try
                {
                    EndpointDescription m_SelectedEndpoint;

                    ApplicationDescriptionCollection servers = UaServer.FindServers(MachineIp);
                    foreach (ApplicationDescription ad in servers)
                    {
                        foreach (string url in ad.DiscoveryUrls)
                        {
                            if (url == MachineIp)
                            {
                                EndpointDescriptionCollection endpoints = UaServer.GetEndpoints(url);
                                foreach (EndpointDescription ep in endpoints)
                                {
                                    string securityPolicy = ep.SecurityPolicyUri.Remove(0, 42);

                                    if (ep.EndpointUrl == MachineIp && ep.SecurityMode == Opc.Ua.MessageSecurityMode.None && securityPolicy == "#None")
                                    {

                                        string s = "[" + ad.ApplicationName + "] " + " [" + ep.SecurityMode + "] " + " [" + securityPolicy + "] " + " [" + ep.EndpointUrl + "]";
                                        itemTag item = new itemTag(ep, s);

                                        m_SelectedEndpoint = (EndpointDescription)item.Tag;
                                        UaServer.NAMESPACE_INDEX = NameSpace;
                                        UaServer.KeepAliveNotification += new KeepAliveEventHandler(Notification_KeepAlive);
                                        UaServer.CertificateValidationNotification += new CertificateValidationEventHandler(Notification_ServerCertificate);
                                        UaServer.Connect(m_SelectedEndpoint, true, "OpcUaClient", "RIFA1234");
                                        UaSession = UaServer.Session;
                                        return;
                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }
        /// <summary>
        /// 通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Notification_KeepAlive_M_1(Session sender, KeepAliveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new KeepAliveEventHandler(Notification_KeepAlive_M_1), sender, e);
                return;
            }

            try
            {
                // check for events from discarded sessions.
                if (!Object.ReferenceEquals(sender, m_Session_M_1))
                {
                    return;
                }

                // check for disconnected session.
                if (ServiceResult.IsBad(e.Status))
                {
                    // try reconnecting using the existing session state
                    m_Session_M_1.Reconnect();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        /// <summary>
        /// 证书
        /// </summary>
        /// <param name="cert"></param>
        /// <param name="e"></param>
        private void Notification_ServerCertificate_M_1(CertificateValidator cert, CertificateValidationEventArgs e)
        {
            //Handle certificate here
            //To accept a certificate manually move it to the root folder (Start > mmc.exe > add snap-in > certificates)
            //Or handle via UAClientCertForm

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CertificateValidationEventHandler(Notification_ServerCertificate_M_1), cert, e);
                return;
            }

            //使用自动应答，
            e.Accept = true;
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(e.Certificate);
            store.Close();
        }
        private void Notification_KeepAlive_M_2(Session sender, KeepAliveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new KeepAliveEventHandler(Notification_KeepAlive_M_2), sender, e);
                return;
            }

            try
            {
                // check for events from discarded sessions.
                if (!Object.ReferenceEquals(sender, m_Session_M_2))
                {
                    return;
                }

                // check for disconnected session.
                if (ServiceResult.IsBad(e.Status))
                {
                    // try reconnecting using the existing session state
                    m_Session_M_2.Reconnect();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        private void Notification_ServerCertificate_M_2(CertificateValidator cert, CertificateValidationEventArgs e)
        {
            //Handle certificate here
            //To accept a certificate manually move it to the root folder (Start > mmc.exe > add snap-in > certificates)
            //Or handle via UAClientCertForm

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CertificateValidationEventHandler(Notification_ServerCertificate_M_2), cert, e);
                return;
            }

            //使用自动应答，
            e.Accept = true;
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(e.Certificate);
            store.Close();
        }
       
        private void Notification_KeepAlive_Plc(Session sender, KeepAliveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new KeepAliveEventHandler(Notification_KeepAlive_Plc), sender, e);
                return;
            }

            try
            {
                // check for events from discarded sessions.
                if (!Object.ReferenceEquals(sender, m_Session_Plc))
                {
                    return;
                }

                // check for disconnected session.
                if (ServiceResult.IsBad(e.Status))
                {
                    // try reconnecting using the existing session state
                    m_Session_Plc.Reconnect();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        private void Notification_ServerCertificate_Plc(CertificateValidator cert, CertificateValidationEventArgs e)
        {
            //Handle certificate here
            //To accept a certificate manually move it to the root folder (Start > mmc.exe > add snap-in > certificates)
            //Or handle via UAClientCertForm

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CertificateValidationEventHandler(Notification_ServerCertificate_Plc), cert, e);
                return;
            }

            //使用自动应答，
            e.Accept = true;
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(e.Certificate);
            store.Close();
        }
        private void Notification_KeepAlive_Wash(Session sender, KeepAliveEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new KeepAliveEventHandler(Notification_KeepAlive_Wash), sender, e);
                return;
            }

            try
            {
                // check for events from discarded sessions.
                if (!Object.ReferenceEquals(sender, m_Session_Wash))
                {
                    return;
                }

                // check for disconnected session.
                if (ServiceResult.IsBad(e.Status))
                {
                    // try reconnecting using the existing session state
                    m_Session_Wash.Reconnect();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        private void Notification_ServerCertificate_Wash(CertificateValidator cert, CertificateValidationEventArgs e)
        {
            //Handle certificate here
            //To accept a certificate manually move it to the root folder (Start > mmc.exe > add snap-in > certificates)
            //Or handle via UAClientCertForm

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CertificateValidationEventHandler(Notification_ServerCertificate_Wash), cert, e);
                return;
            }

            //使用自动应答，
            e.Accept = true;
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            store.Add(e.Certificate);
            store.Close();
        }

        #endregion

        private void FormControl_Load(object sender, EventArgs e)
        {
            //打开socket监听，监听装卸站软件
            //OpenServer_Load();

            m_ConnectState[0] = OverAll.Ping("10.50.42.9");//机床1
            m_ConnectState[1] = OverAll.Ping("10.50.42.11");//机床2
            //m_ConnectState[0] = false;
            //m_ConnectState[1] = false;
            m_ConnectState[2] = OverAll.Ping("10.50.42.1");//PLC
            m_ConnectState[3] = false;//OverAll.Ping("10.50.42.12");//清洗房
            m_ConnectState[4] = OverAll.Ping("10.50.42.7");//RGV

            //CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "执行"), 0);
            //CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "状态字段"), 0);
            //CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "拿地址"), 0);
            //CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "放地址"), 0);
            //timer1.Start();

            if (m_ConnectState[0])
            {
                m_UaServer_M_1 = new Server();
                ConnectEquip(1);
            }
            if (m_ConnectState[1])
            {
                m_UaServer_M_2 = new Server();
                ConnectEquip(2);
            }
            if (m_ConnectState[2])
            {
                m_UaServer_Plc = new Server();
                ConnectEquip(3);
            }
            //if (m_ConnectState[3])
            //{
            //    m_UaServer_Wash = new Server();
            //    ConnectEquip(4);
            //}
            //if (m_ConnectState[4])
            //{
            //    timer1.Start();
            //}

            AddSubItem(m_ConnectState);
            ButtonInit();
        }

        private void ButtonInit() {
            
            this.button_AgvPos1.Click += new System.EventHandler(this.Button_AgvPos_Click);
            this.button_AgvPos2.Click += new System.EventHandler(this.Button_AgvPos_Click);
            this.button_AgvPos3.Click += new System.EventHandler(this.Button_AgvPos_Click);
            this.button_AgvPos4.Click += new System.EventHandler(this.Button_AgvPos_Click);
            this.button_AgvPos5.Click += new System.EventHandler(this.Button_AgvPos_Click);
            this.button_AgvPos6.Click += new System.EventHandler(this.Button_AgvPos_Click);
            this.button_AgvPos7.Click += new System.EventHandler(this.Button_AgvPos_Click);


        }
        private void AddSubItem(bool[] OpenIdex)
        {
         //添加监控
            #region 设备1
            if (OpenIdex[0])
            {
                //查询频率
                m_UaSubscipition_M_1 = m_UaServer_M_1.Subscribe(250);


                monitoredItems_M_1.Add(m_UaServer_M_1.AddMonitoredItem(m_UaSubscipition_M_1, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_CNC, "机床在线程序运行结束"), 2).ToString(), @"pCom", 1));
                monitoredItems_M_1.Add(m_UaServer_M_1.AddMonitoredItem(m_UaSubscipition_M_1, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_CNC, "机床上下料准备"), 2).ToString(), @"CarryReady", 1));
                monitoredItems_M_1.Add(m_UaServer_M_1.AddMonitoredItem(m_UaSubscipition_M_1, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_CNC, "托盘交换门关闭-状态"), 2).ToString(), @"DoorClose", 1));
                monitoredItems_M_1.Add(m_UaServer_M_1.AddMonitoredItem(m_UaSubscipition_M_1, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_CNC, "托盘零点锁紧-状态"), 2).ToString(), @"ZeroLoosenClose", 1));

                m_UaSubscipition_M_1.AddItems(monitoredItems_M_1);
                m_UaSubscipition_M_1.ApplyChanges();
                m_UaServer_M_1.ItemChangedNotification += new MonitoredItemNotificationEventHandler(ValueChanged_M_1);

                // AlarmrSubInit(m_UaServer_5_1,m_Session_5_1,ref m_AlarmSub_5_1,ref m_AlarmMonitorItem_5_1, ValueChanged_5_1);
            }
            #endregion
            #region 设备2
            if (OpenIdex[1])
            {
                m_UaSubscipition_M_2 = m_UaServer_M_2.Subscribe(250);
                monitoredItems_M_2.Add(m_UaServer_M_2.AddMonitoredItem(m_UaSubscipition_M_2, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_CNC, "机床在线程序运行结束"), 2).ToString(), @"pCom", 1));

                monitoredItems_M_2.Add(m_UaServer_M_2.AddMonitoredItem(m_UaSubscipition_M_2, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_CNC, "托盘交换门关闭-状态"), 2).ToString(), @"DoorClose", 1));
                monitoredItems_M_2.Add(m_UaServer_M_2.AddMonitoredItem(m_UaSubscipition_M_2, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_CNC, "托盘零点锁紧-状态"), 2).ToString(), @"ZeroLoosenClose", 1));

                m_UaSubscipition_M_2.AddItems(monitoredItems_M_2);
                m_UaSubscipition_M_2.ApplyChanges();
                m_UaServer_M_2.ItemChangedNotification += new MonitoredItemNotificationEventHandler(ValueChanged_M_2);
            }
            #endregion
            #region PLC
            if (OpenIdex[2])
            {
                m_UaSubscipition_Plc = m_UaServer_Plc.Subscribe(250);

                monitoredItems_Plc.Add(m_UaServer_Plc.AddMonitoredItem(m_UaSubscipition_Plc, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_PLC, "1#叉臂动作完成"), 3).ToString(), @"ForkComp1", 1));
                monitoredItems_Plc.Add(m_UaServer_Plc.AddMonitoredItem(m_UaSubscipition_Plc, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_PLC, "2#叉臂动作完成"), 3).ToString(), @"ForkComp2", 1));
                monitoredItems_Plc.Add(m_UaServer_Plc.AddMonitoredItem(m_UaSubscipition_Plc, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_PLC, "1#零点吹气"), 3).ToString(), @"1#BlowGas", 1));
                monitoredItems_Plc.Add(m_UaServer_Plc.AddMonitoredItem(m_UaSubscipition_Plc, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_PLC, "2#零点吹气"), 3).ToString(), @"2#BlowGas", 1));
                m_UaSubscipition_Plc.AddItems(monitoredItems_Plc);
                m_UaSubscipition_Plc.ApplyChanges();
                m_UaServer_Plc.ItemChangedNotification += new MonitoredItemNotificationEventHandler(ValueChanged_Plc);
            }

            #endregion
            #region 清洗房
            if (OpenIdex[3])
            {
                m_UaSubscipition_Wash = m_UaServer_Wash.Subscribe(250);

                monitoredItems_Wash.Add(m_UaServer_Wash.AddMonitoredItem(m_UaSubscipition_Wash, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_Wash, "清洗完成"), 2).ToString(), @"Wash_Com", 1));
                monitoredItems_Wash.Add(m_UaServer_Wash.AddMonitoredItem(m_UaSubscipition_Wash, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_Wash, "托盘交换门关闭"), 2).ToString(), @"doorClose", 1));
                monitoredItems_Wash.Add(m_UaServer_Wash.AddMonitoredItem(m_UaSubscipition_Wash, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_Wash, "托盘交换门开启"), 2).ToString(), @"Door_Open", 1));
                monitoredItems_Wash.Add(m_UaServer_Wash.AddMonitoredItem(m_UaSubscipition_Wash, new NodeId(AddressDic.obtainAddress(AddressDic.EQUIP_Wash, "机床处于联机状态"), 2).ToString(), @"OnLine", 1));
               
                m_UaSubscipition_Wash.AddItems(monitoredItems_Wash);
                m_UaSubscipition_Wash.ApplyChanges();
                m_UaServer_Wash.ItemChangedNotification += new MonitoredItemNotificationEventHandler(ValueChanged_Wash);
                //AlarmrSubInit(m_UaServer_Wash, m_Session_Wash, ref m_AlarmSub_Wash, ref m_AlarmMonitorItem_Wash, ValueChanged_3_2);
            }
            #endregion           
        }
        private void ConnectEquip(int iEquipNo = 0)
        {
            if (iEquipNo == 1 || iEquipNo == 0)
            {
                if (m_Session_M_1 != null && !m_Session_M_1.Disposed)
                {
                    DisconnectServer(iEquipNo);
                }
                ConnectOpc(IP_M1, m_UaServer_M_1, ref m_Session_M_1,2, Notification_KeepAlive_M_1, Notification_ServerCertificate_M_1);

            }
            if (iEquipNo == 2 || iEquipNo == 0)
            {
                if (m_Session_M_2 != null && !m_Session_M_2.Disposed)
                {
                    DisconnectServer(iEquipNo);
                }
                ConnectOpc(IP_M2, m_UaServer_M_2, ref m_Session_M_2, 2, Notification_KeepAlive_M_2, Notification_ServerCertificate_M_2);
            }
            if (iEquipNo == 3 || iEquipNo == 0)
            {
                if (m_Session_Plc != null && !m_Session_Plc.Disposed)
                {
                    DisconnectServer(iEquipNo);
                }
                ConnectOpc_Plc(IP_PLC, m_UaServer_Plc, ref m_Session_Plc, 3, Notification_KeepAlive_Plc, Notification_ServerCertificate_Plc);
            }
            if (iEquipNo == 4 || iEquipNo == 0)
            {
                if (m_Session_Wash != null && !m_Session_Wash.Disposed)
                {
                    DisconnectServer(iEquipNo);
                }
                ConnectOpc_Plc(IP_Wash, m_UaServer_Wash, ref m_Session_Wash, 3, Notification_KeepAlive_Wash, Notification_ServerCertificate_Wash);
            }
        }
        private void DisconnectServer(int iEquipNo = 0)
        {
            if (iEquipNo == 1 || iEquipNo == 0)
            {
                try
                {
                    m_UaSubscipition_M_1.Delete(true);
                }
                catch
                {
                    ;
                }
                m_UaServer_M_1.Disconnect();
                m_Session_M_1 = m_UaServer_M_1.Session;
            }
            else if (iEquipNo == 2 || iEquipNo == 0)
            {
                try
                {
                    m_UaSubscipition_M_2.Delete(true);
                }
                catch
                {
                    ;
                }
                m_UaServer_M_2.Disconnect();
                m_Session_M_2 = m_UaServer_M_2.Session;
            }
            else if (iEquipNo == 3 || iEquipNo == 0)
            {
                try
                {
                    m_UaSubscipition_Plc.Delete(true);
                }
                catch
                {
                    ;
                }
                m_UaServer_Plc.Disconnect();
                m_Session_Plc = m_UaServer_Plc.Session;
            }
            else if (iEquipNo == 4 || iEquipNo == 0)
            {
                try
                {
                    m_UaSubscipition_Wash.Delete(true);
                }
                catch
                {
                    ;
                }
                m_UaServer_Wash.Disconnect();
                m_Session_Wash = m_UaServer_Wash.Session;
            }
        }

        #region 装卸站通讯
        /// <summary>
        /// 装卸站tcp通讯
        /// </summary>
        private bool OpenServer_Load()
        {
            try
            {
                Flag_Listen_Load = true;
                // 创建负责监听的套接字，注意其中的参数；
                ServerSocket_Load = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // 创建包含ip和端口号的网络节点对象；
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("10.50.42.41"), 7788);
                try
                {
                    // 将负责监听的套接字绑定到唯一的ip和端口上；
                    ServerSocket_Load.Bind(endPoint);
                }
                catch
                {
                    return false;
                }
                // 设置监听队列的长度；
                ServerSocket_Load.Listen(20);
                // 创建负责监听的线程；
                Thread Thread_ServerListen = new Thread(ListenConnecting_Load);
                Thread_ServerListen.IsBackground = true;
                Thread_ServerListen.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭服务
        /// </summary>
        public void CloseServer_Load()
        {
            lock (dic_ClientSocket_Load)
            {
                foreach (var item in dic_ClientSocket_Load)
                {
                    item.Value.Close();//关闭每一个连接
                }
                dic_ClientSocket_Load.Clear();//清除字典
            }
            lock (dic_ClientThread_Load)
            {
                foreach (var item in dic_ClientThread_Load)
                {
                    item.Value.Abort();//停止线程
                }
                dic_ClientThread_Load.Clear();
            }
            Flag_Listen_Load = false;
            //ServerSocket.Shutdown(SocketShutdown.Both);//服务端不能主动关闭连接,需要把监听到的连接逐个关闭
            if (ServerSocket_Load != null)
                ServerSocket_Load.Close();

        }
        /// <summary>
        /// 监听客户端请求的方法；
        /// </summary>
        private void ListenConnecting_Load()
        {
            while (Flag_Listen_Load)  // 持续不断的监听客户端的连接请求；
            {
                try
                {
                    Socket sokConnection = ServerSocket_Load.Accept(); // 一旦监听到一个客户端的请求，就返回一个与该客户端通信的 套接字；
                                                                  // 将与客户端连接的 套接字 对象添加到集合中；
                    string str_EndPoint = sokConnection.RemoteEndPoint.ToString();
                    MySession myTcpClient = new MySession() { TcpSocket = sokConnection };
                    //创建线程接收数据
                    Thread th_ReceiveData = new Thread(ReceiveData_Load);
                    th_ReceiveData.IsBackground = true;
                    th_ReceiveData.Start(myTcpClient);
                    //把线程及客户连接加入字典
                    dic_ClientThread_Load.Add(str_EndPoint, th_ReceiveData);
                    dic_ClientSocket_Load.Add(str_EndPoint, myTcpClient);
                }
                catch
                {

                }
                Thread.Sleep(200);
            }
        }
        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="sokConnectionparn"></param>
        private void ReceiveData_Load(object sokConnectionparn)
        {
            MySession tcpClient = sokConnectionparn as MySession;
            Socket socketClient = tcpClient.TcpSocket;
            bool Flag_Receive = true;

            while (Flag_Receive)
            {
                try
                {
                    // 定义一个2M的缓存区；
                    byte[] arrMsgRec = new byte[1024 * 1024 * 2];
                    // 将接受到的数据存入到输入  arrMsgRec中；
                    int length = -1;
                    try
                    {
                        length = socketClient.Receive(arrMsgRec); // 接收数据，并返回数据的长度；
                    }
                    catch
                    {
                        Flag_Receive = false;
                        // 从通信线程集合中删除被中断连接的通信线程对象；
                        string keystr = socketClient.RemoteEndPoint.ToString();
                        dic_ClientSocket_Load.Remove(keystr);//删除客户端字典中该socket
                        dic_ClientThread_Load[keystr].Abort();//关闭线程
                        dic_ClientThread_Load.Remove(keystr);//删除字典中该线程

                        tcpClient = null;
                        socketClient = null;
                        break;
                    }
                    byte[] buf = new byte[length];
                    Array.Copy(arrMsgRec, buf, length);
                    lock (tcpClient.m_Buffer)
                    {
                        tcpClient.AddQueue(buf);
                        if (length > 0)
                        {
                            //System.Diagnostics.Debug.WriteLine("receive");
                            if (buf[0] == 0x02)
                            {
                                //设备复位 搬运当前托盘到托盘库
                                //CarReset();
                            } else if (buf[0] == 0x01) {
                                Thread thread = new Thread(new ThreadStart(ThreadStartTask));//装卸站接受信号
                                thread.Start();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(FormControl), ex);
                }
                Thread.Sleep(100);
            }
        }
        #endregion


        public void ValueChanged_M_1(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                // We have to call an invoke method 
                if (this.InvokeRequired)
                {
                    // Asynchronous execution of the valueChanged delegate
                    this.BeginInvoke(new MonitoredItemNotificationEventHandler(ValueChanged_M_1), monitoredItem, e);
                    return;
                }
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (notification == null)
                {
                    return;
                }

                // Get the according item
                string monitoredName = monitoredItem.DisplayName;

                if ("pCom" == monitoredName)
                {
                    //加工完成
                    //获取目标数据
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);

                    if (monitoredVale == true)
                    {
                        //读取当前走的是什么程序
                        //任务完成
                        int monitoredVale2 = Convert.ToInt32(notification.Value.WrappedValue.Value);

                        if (monitoredVale2 != 1)
                            return;
                        //取到当前的托盘编号
                        DataBaseprogram.db_EquipDao db_EquipDao = new DataBaseprogram.db_EquipDao();
                        DataSet dsEquipInfo = db_EquipDao.QueryEquipById(51);
                        int TrayId = Convert.ToInt32(dsEquipInfo.Tables[0].Rows[0]["TrayId"]);
                        string TaskType = JudgeTask(m_UaServer_M_1);

                        object[] EquipInfo = new object[4];
                        DataBaseprogram.db_TuoDao db_TuoDao = new DataBaseprogram.db_TuoDao();
                        if (TaskType == "取托盘") {
                            MessageBox.Show("取托盘 完成");
                        }
                        else if (TaskType == "放托盘") {
                            MessageBox.Show("放托盘 完成");
                        }
                        else if (TaskType == "加工")
                        {
                            try
                            {
                                //判断是否是手动任务
                                DataSet dsTrayInfo = db_TuoDao.QueryTuoById(TrayId);
                                List<string> OnLine_NodeID = new List<string>() { new NodeId(@"/Plc/DB97.DBX4.5", 2).ToString() };
                                List<String> Online_Value = m_UaServer_M_1.ReadValues(OnLine_NodeID);
                                if (dsTrayInfo.Tables[0].Rows[0]["visible"].ToString() == "8" && Convert.ToBoolean(Online_Value[0]))
                                    return;
                                EquipInfo[0] = "加工完成"; EquipInfo[1] = 0; EquipInfo[2] = 0; EquipInfo[3] = null;
                                // db_Product.UpdateProcessTime("EndTime", 51);
                                db_EquipDao.UpdateEquipInfo(51, EquipInfo);
                                OverAll.ProductRecord(dsTrayInfo.Tables[0].Rows[0]["chantype"].ToString(), dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString(),
                                    dsTrayInfo.Tables[0].Rows[0]["chanNo"].ToString(), "加工完成", string.Format("完成工序:{0}", dsTrayInfo.Tables[0].Rows[0]["task"].ToString()));


                                //推送搬运任务

                                //先放到 成品缓存位 再放到装卸站缓存位 
                                //查询成品缓存位是否空闲
                                //判断加工设备1 成品缓存位有没有空位
                                DataSet equip62 = new DataBaseprogram.db_EquipDao().QueryEquipById(OverAll.getEquipId("加工设备1成品缓存位"));
                                //缓存位为空且无托盘
                                if (OverAll.EQUIP_STATE_FREE.Equals(equip62.Tables[0].Rows[0]["equipState"].ToString()) && Convert.ToInt32(equip62.Tables[0].Rows[0]["TrayId"]) == 0)
                                {
                                    //修改成品缓存位1状态 防止反复写入
                                    object[] UpdateInfo = new object[4];
                                    UpdateInfo[0] = OverAll.EQUIP_STATE_TO_BE_PLACED; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = null;
                                    new DataBaseprogram.db_EquipDao().UpdateEquipInfo(OverAll.getEquipId("加工设备1成品缓存位"), UpdateInfo);
                                    //添加搬运任务 至任务列表
                                    //new DataBaseprogram.db_LogisticstaskDao().InsertAgvtasklist(0, OverAll.getEquipId("加工设备1"), OverAll.getEquipId("加工设备1成品缓存位"));
                                    //todo AGV任务下发  
                                    AgvTask.genAgvSchedulingTask(OverAll.getEquipId("加工设备1"), OverAll.getEquipId("加工设备1成品缓存位"));
                                }


                            }
                            catch (Exception s1)
                            {
                                LogHelper.WriteLog(typeof(Form), s1);
                            }
                        }

                    }
                }
                else if ("CarryReady" == monitoredName) {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);
                    if (monitoredVale == true) {

                        DataBaseprogram.db_EquipDao db_EquipDao = new DataBaseprogram.db_EquipDao();
                        DataSet dsEquipInfo = db_EquipDao.QueryEquipById(51);
                        int TrayId = Convert.ToInt32(dsEquipInfo.Tables[0].Rows[0]["TrayId"]);
                        string TaskType = JudgeTask(m_UaServer_M_1);
                        object[] EquipInfo = new object[4];
                        DataBaseprogram.db_TuoDao db_TuoDao = new DataBaseprogram.db_TuoDao();

                        if (TaskType == "取托盘")
                        {

                            //记录
                            DataSet dsTrayInfo = db_TuoDao.QueryTuoById(TrayId);
                            OverAll.ProductRecord(dsTrayInfo.Tables[0].Rows[0]["chantype"].ToString(), dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString(),
                                dsTrayInfo.Tables[0].Rows[0]["chanNo"].ToString(), "机床取托盘就绪", string.Format("位置:{0}", 51));

                            //通知PLC进行托盘叉取
                            List<string> OnLine_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(2, "1#托盘进出机床"), 3).ToString() };
                            List<String> Online_Value = new List<string>();
                            Online_Value.Add("2");
                            m_UaServer_Plc.WriteValues(Online_Value, OnLine_NodeID);

                            OnLine_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "机床上下料开始"), 2).ToString() };
                            Online_Value = new List<string>();
                            Online_Value.Add("True");
                            m_UaServer_M_1.WriteValues(Online_Value, OnLine_NodeID);

                        }
                        else if (TaskType == "放托盘")
                        {
                            //记录
                            DataSet dsTrayInfo = db_TuoDao.QueryTuoById(TrayId);
                            OverAll.ProductRecord(dsTrayInfo.Tables[0].Rows[0]["chantype"].ToString(), dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString(),
                                dsTrayInfo.Tables[0].Rows[0]["chanNo"].ToString(), "机床放托盘就绪", string.Format("位置:{0}", 51));

                            //通知PLC进行托盘叉取
                            List<string> OnLine_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(2, "1#托盘进出机床"), 3).ToString() };
                            List<String> Online_Value = new List<string>();
                            Online_Value.Add("1");
                            m_UaServer_Plc.WriteValues(Online_Value, OnLine_NodeID);

                            OnLine_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "机床上下料开始"), 2).ToString() };
                            Online_Value = new List<string>();
                            Online_Value.Add("True");
                            m_UaServer_M_1.WriteValues(Online_Value, OnLine_NodeID);


                        }
                    }
                }
                else if ("DoorClose" == monitoredName)
                {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);
                    if (monitoredVale == true)
                    {
                        //信号复位
                        List<string> CNC_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "托盘交换门关闭-控制"), 2).ToString() };
                        List<String> CNC_Value = new List<string>();
                        CNC_Value.Add("False");
                        m_UaServer_M_1.WriteValues(CNC_Value, CNC_NodeID);

                        string TaskType = JudgeTask(m_UaServer_M_1);
                        if (TaskType == "放托盘")
                        {
                            DataBaseprogram.db_EquipDao db_EquipDao = new DataBaseprogram.db_EquipDao();
                            DataBaseprogram.db_TuoDao db_TuoDao = new DataBaseprogram.db_TuoDao();
                            DataSet dsEquipInfo = db_EquipDao.QueryEquipById(51);
                            int TrayId = Convert.ToInt32(dsEquipInfo.Tables[0].Rows[0]["TrayId"]);
                            DataSet dsTrayInfo = db_TuoDao.QueryTuoById(TrayId);

                            //更新信息
                            //更新缓存位信息
                            object[] UpdateInfo = new object[4];
                            UpdateInfo[0] = OverAll.EQUIP_STATE_LOCK; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = 0;
                            db_EquipDao.UpdateEquipInfo(53, UpdateInfo);
                            //更新机床信息
                            UpdateInfo[0] = OverAll.EQUIP_STATE_PROCESSED; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = TrayId;
                            db_EquipDao.UpdateEquipInfo(51, UpdateInfo);
                            //更新托盘信息
                            object[] Info = new object[] { null, 51, null, null, null, null, null, null, OverAll.EQUIP_STATE_PROCESSED, DateTime.Now.ToString() };
                            db_TuoDao.UpdateTrayInfo(TrayId, Info);

                            ////运行加工程序
                            //if (Convert.ToInt32(dsTrayInfo.Tables[0].Rows[0]["visible"]) != 3)//标识是手动的,不进行自动加工
                            //{
                            //    return;
                            //}

                            //Process.StartProcess(m_UaServer_M_1, 51, TrayId);
                        }
                        else if (TaskType == "取托盘")
                        {
                            DataBaseprogram.db_EquipDao db_EquipDao = new DataBaseprogram.db_EquipDao();
                            DataBaseprogram.db_TuoDao db_TuoDao = new DataBaseprogram.db_TuoDao();
                            DataSet dsEquipInfo = db_EquipDao.QueryEquipById(51);
                            int TrayId = Convert.ToInt32(dsEquipInfo.Tables[0].Rows[0]["TrayId"]);
                            DataSet dsTrayInfo = db_TuoDao.QueryTuoById(TrayId);

                            //更新信息
                            //更新缓存位信息
                            object[] UpdateInfo = new object[4];
                            UpdateInfo[0] = OverAll.EQUIP_STATE_LOCK; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = 0;
                            db_EquipDao.UpdateEquipInfo(53, UpdateInfo);
                            //更新机床信息
                            UpdateInfo[0] = OverAll.EQUIP_STATE_FREE; UpdateInfo[1] = null; UpdateInfo[2] = null; UpdateInfo[3] = TrayId;
                            db_EquipDao.UpdateEquipInfo(51, UpdateInfo);
                            //更新托盘信息
                            object[] Info = new object[] { null, 51, null, null, null, null, null, null, OverAll.EQUIP_STATE_PROCESSED, DateTime.Now.ToString() };
                            db_TuoDao.UpdateTrayInfo(TrayId, Info);


                            //添加搬运任务
                        }
                    }
                }
                else if ("ZeroLoosenClose" == monitoredName)
                {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);
                    if (monitoredVale == true)
                    {
                        //零点松开，通知PLC开始叉进机床
                        List<string> CNC_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(5, "托盘零点锁紧-控制"), 2).ToString() };
                        List<String> CNC_Value = new List<string>();
                        CNC_Value.Add("False");
                        m_UaServer_M_1.WriteValues(CNC_Value, CNC_NodeID);
                    }


                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        public void ValueChanged_M_2(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                // We have to call an invoke method 
                if (this.InvokeRequired)
                {
                    // Asynchronous execution of the valueChanged delegate
                    this.BeginInvoke(new MonitoredItemNotificationEventHandler(ValueChanged_M_2), monitoredItem, e);
                    return;
                }
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (notification == null)
                {
                    return;
                }

                // Get the according item
                string monitoredName = monitoredItem.DisplayName;

                if ("Measure_Com" == monitoredName)
                {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);

                    if (monitoredVale == true)//测量完成,允许下料
                    {
                        //开门
                        List<string> NodeList_Door = new List<string>() { new NodeId(AddressDic.obtainAddress(10, "检测工位_门开指令（选配）"), 3).ToString() };
                        List<String> ValueList_Door = new List<string>() { "True" };
                        m_UaServer_Plc.WriteValues(ValueList_Door, NodeList_Door);

                    }
                }


            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        public void ValueChanged_Plc(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                // We have to call an invoke method 
                if (this.InvokeRequired)
                {
                    // Asynchronous execution of the valueChanged delegate
                    this.BeginInvoke(new MonitoredItemNotificationEventHandler(ValueChanged_Plc), monitoredItem, e);
                    return;
                }
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (notification == null)
                {
                    return;
                }
                // Get the according item
                string monitoredName = monitoredItem.DisplayName;
               
                
                if ("ForkComp1" == monitoredName)
                {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);

                    if (monitoredVale == true)
                    {

                        //锁零点
                        List<string> OnLine_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "托盘零点锁紧-控制"), 2).ToString() ,
                        new NodeId(AddressDic.obtainAddress(1, "托盘交换门关闭-控制"), 2).ToString(),new NodeId(AddressDic.obtainAddress(1, "机床上下料完成"), 2).ToString()};
                        List<String> Online_Value = new List<string>();
                        
                        Online_Value.Add("True");
                        Online_Value.Add("True");
                        Online_Value.Add("True");
                        m_UaServer_M_1.WriteValues(Online_Value, OnLine_NodeID);

                        //走加工
                        
                    }
                }
                else if ("ForkComp2" == monitoredName)
                {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);

                    if (monitoredVale == true)
                    {
                        //开门
                        List<string> OnLine_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "托盘零点锁紧-控制"), 2).ToString() ,
                        new NodeId(AddressDic.obtainAddress(1, "托盘交换门关闭-控制"), 2).ToString(),new NodeId(AddressDic.obtainAddress(1, "机床上下料完成"), 2).ToString()};
                        List<String> Online_Value = new List<string>();

                        Online_Value.Add("True");
                        Online_Value.Add("True");
                        Online_Value.Add("True");
                        m_UaServer_M_2.WriteValues(Online_Value, OnLine_NodeID);


                    }
                }
                else if ("1#BlowGas" == monitoredName)
                {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);

                    List<string> CNC_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "零点吹气打开"), 2).ToString() ,
                    new NodeId(AddressDic.obtainAddress(1, "零点吹气关闭"), 2).ToString()};

                    List<String> CNC_Value = new List<string>();
                    if (monitoredVale)
                    {

                        CNC_Value.Add("True");
                        CNC_Value.Add("False");
                    }
                    else {
                        CNC_Value.Add("False");
                        CNC_Value.Add("True");
                    }
                    m_UaServer_M_1.WriteValues(CNC_Value, CNC_NodeID);
                }
                else if ("2#BlowGas" == monitoredName)
                {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);

                    List<string> CNC_NodeID = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "零点吹气打开"), 2).ToString() ,
                    new NodeId(AddressDic.obtainAddress(1, "零点吹气关闭"), 2).ToString()};

                    List<String> CNC_Value = new List<string>();
                    if (monitoredVale)
                    {

                        CNC_Value.Add("True");
                        CNC_Value.Add("False");
                    }
                    else
                    {
                        CNC_Value.Add("False");
                        CNC_Value.Add("True");
                    }
                    m_UaServer_M_2.WriteValues(CNC_Value, CNC_NodeID);
                }
            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        //清洗站
        public void ValueChanged_Wash(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                // We have to call an invoke method 
                if (this.InvokeRequired)
                {
                    // Asynchronous execution of the valueChanged delegate
                    this.BeginInvoke(new MonitoredItemNotificationEventHandler(ValueChanged_Wash), monitoredItem, e);
                    return;
                }
                MonitoredItemNotification notification = e.NotificationValue as MonitoredItemNotification;
                if (notification == null)
                {
                    return;
                }

                // Get the according item
                string monitoredName = monitoredItem.DisplayName;

                if ("Wash_Com" == monitoredName)
                {
                    bool monitoredVale = Convert.ToBoolean(notification.Value.WrappedValue.Value);

                    if (monitoredVale == true)
                    {
                        //开门
                        List<string> NodeList_Door = new List<string>() { new NodeId(AddressDic.obtainAddress(10, "清洗工位_门开指令"), 3).ToString() };
                        List<string> ValueList_Door = new List<string>() { "True" };
                        m_UaServer_Wash.WriteValues(ValueList_Door, NodeList_Door);

                    }
                } 
                else if ("Door_Open" == monitoredName) {
                    //todo
                    //添加下料任务
                    List<string> NodeList_Door = new List<string>() { new NodeId(AddressDic.obtainAddress(20, "控制_下料完成"), 3).ToString() };
                    List<string> ValueList_Door = new List<string>() { "True" };
                    m_UaServer_Wash.WriteValues(ValueList_Door, NodeList_Door);
                }


            }
            catch (Exception ex)
            {

                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        /// <summary>
        /// 判断当前机床运行的程序
        /// </summary>
        private string JudgeTask(Server UaServer, int iServerType = 5)
        {

            List<string> NodeList_MPF = new List<string>();
            NodeList_MPF.Add(new NodeId(@"/Channel/ProgramPointer/progName", 2).ToString());
            List<string> ValueList_MPF = new List<string>();

            ValueList_MPF = UaServer.ReadValues(NodeList_MPF);

            //查询主程序
            if (ValueList_MPF[0] == "_N_EXCHANGE_IN_SPF")
            {
               
                    return "放托盘";
               

            }
            else if (ValueList_MPF[0] == "_N_EXCHANGE_OUT_SPF") {
                return "取托盘";
            }
            else
            {

                // DataBaseprogram.Db_TrayInfo db_TrayInfo = new DataBaseprogram.Db_TrayInfo();
                // DataSet dsTrayInfo = db_TrayInfo.QueryTrayInfo(iTrayId);
                // string ProgramName = Process.GetMpfName(dsTrayInfo.Tables[0].Rows[0]["chanType"].ToString(), dsTrayInfo.Tables[0].Rows[0]["task"].ToString(), iEquipId);
                // ProgramName ="_N_" +  ProgramName.Replace(".","_");
                // if (ValueList_MPF[0] == ProgramName)
                // {
                return "加工";
                // }
                // else
                // {
                //     return "";
                //}
            }


        }
        private void ThreadStartTask()
        {
            //分解totallist
            DataBaseprogram.db_AgvTaskDao db_AgvTask = new DataBaseprogram.db_AgvTaskDao();
            DataBaseprogram.db_RgvtaskDao db_Rgvtask = new DataBaseprogram.db_RgvtaskDao();
            bool AgvFree = db_AgvTask.QueryAgvFree();
            bool RgvFree = db_Rgvtask.QueryRgvFree();
            if (!AgvFree && !RgvFree) {
                //AGV和Rgv 都空闲
            }
            //判断RGV是否空闲
            DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
            DataSet dsEquipInfo = db_Equip.QueryEquipById(80);
            if (dsEquipInfo.Tables[0].Rows[0]["equipState"].ToString() == OverAll.EQUIP_STATE_FREE) {
                ThreadStartTask_Rgv();
            }
            //DataBaseprogram.db_AgvTaskDao db_AgvTask = new DataBaseprogram.db_AgvTaskDao();
            if (!db_AgvTask.QueryAgvFree()) {
                ThreadStartTask_Agv();
            }

       
        }

        private void ThreadStartTask_Rgv() {
            //查找任务
            //判断任务列表是否为空
            DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
            DataBaseprogram.db_RgvtaskDao db_RgvtaskDao = new DataBaseprogram.db_RgvtaskDao();
            DataSet rgvTask = db_RgvtaskDao.QueryRgvtask();
            if (rgvTask.Tables[0].Rows.Count > 0)
            {
                //发布列表待完成任务
                int initial = Convert.ToInt32(rgvTask.Tables[0].Rows[0]["Initial"]);
                int target = Convert.ToInt32(rgvTask.Tables[0].Rows[0]["Target"]);
                //todo  寄存器地址，集合数据类型的下标，数据值
                int initialPos = (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "初始位置");
                int targetPos = (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "目标位置");
                CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, initialPos, initial);
                CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, targetPos, target);

                CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "执行"), 1);
                Thread.Sleep(2000);
                CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "执行"), 0);
                db_RgvtaskDao.DeleteLogisticsTaskList();
                int iTrayId;
                if (initial < 19)
                {
                    iTrayId = initial;
                }
                else
                {
                    DataSet ds = db_Equip.QueryEquipById(initial);
                    iTrayId = Convert.ToInt32(ds.Tables[0].Rows[0]["TrayId"]);
                }
                db_Equip.UpdateCarState(initial, target, 0, iTrayId);
                //初始位置准备工作
                GetPrepare(initial);
            }
        }
        private void ThreadStartTask_Agv()
        {
        }
        /// <summary>
        /// 拿位置的准备工作
        /// </summary>
        public void GetPrepare(int iInitialPos) {
            if (iInitialPos < 19)
            {//托盘库，不需要准备工作
            } else if (iInitialPos == 21)
            {//装卸站
             //1、测量桁架复位
             //2、开门
                List<string> NodeIdList = new List<string>();
                List<string> ValueList = new List<string>();
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());
                ValueList.Add("True");
                ValueList.Add("True");
                m_UaServer_Plc.WriteValues(ValueList, NodeIdList);
            }
            else if (iInitialPos == 22)
            {//清洗房
             //开门
                List<string> NodeIdList = new List<string>();
                List<string> ValueList = new List<string>();
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());
                ValueList.Add("True");
                m_UaServer_Wash.WriteValues(ValueList, NodeIdList);
            }
            else if (iInitialPos == 23)
            {//装卸站缓存位，不需要准备工作
            }

        }
        public void GetTrayCheck() {

        }
        public void PutTrayCheck()
        {

        }
        private void FormControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                m_UaSubscipition_M_1.Delete(true);
                m_UaSubscipition_M_2.Delete(true);
                m_UaSubscipition_Plc.Delete(true);
                m_UaSubscipition_Wash.Delete(true);
            }
            catch
            {
                ;
            }
            if (m_ConnectState[0])
                DisconnectServer(1);
            if (m_ConnectState[1])
                DisconnectServer(2);
            if (m_ConnectState[2])
                DisconnectServer(3);
            if (m_ConnectState[3])
                DisconnectServer(4);

        }
        private void RgvPutComplete(int _iTrayId, int _iEquipId) {
            try
            {
                //判断当前完成的是哪个任务。
                //通过carstate这张表去查询 当前任务相关的信息
                DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
              

                //查询是否还有下到工序
                string sNextProcess = null;
                bool ProcessEnd = Process.ProcessPlus(_iTrayId, ref sNextProcess);

                DataBaseprogram.db_TuoDao cTrayInfo = new DataBaseprogram.db_TuoDao();
                DataSet dsTrayInfo = cTrayInfo.QueryTuoById(_iTrayId);
                int TrayVisible = Convert.ToInt32(dsTrayInfo.Tables[0].Rows[0]["visible"]);

                #region 托盘库
                if (_iEquipId < 19)
                {

                    /*更新托盘状态,到托盘库3种情况
                     * 1、产品卸载完后到托盘库，此时空托盘，visible==1
                     * 2、产品工序完成，等待卸载，ProcessEnd==true，visible==2
                     * 3、有后续工序，ProcessEnd==false，visible==3
                    */

                    //0:状态visible;1:位置position;2:目标位置;3:订单编号OrderId;4:产品名称ChanType;5:产品编号chanNo;
                    // 6:优先度:preferential;7:产品进度task;8:产品状态chanState;9更新时间dateStart

                    #region 更新托盘
                    object[] Info = new object[] { null, _iTrayId, 0, null, null, null, sNextProcess, OverAll.ProcessTransition(sNextProcess), DateTime.Now.ToString() };
                    if (1 == TrayVisible)
                    {
                        //清空托盘中的产品信息
                        Info = new object[] { 0, _iTrayId, 0, "", "" ,"", 0, "", "", DateTime.Now.ToString() };
                    }
                    else {
                        if (ProcessEnd)
                        {
                            //任务结束,待卸载
                            Info = new object[] { 2, _iTrayId, 0, null, null, null, null, "", "待卸载", DateTime.Now.ToString() };
                            
                        }
                        else
                        {
                            //还有后续工序
                            //如果是待清洗、待加工需要检索当前是否有空闲设备，如果有需要生成搬运任务
                            if (OverAll.ProcessTransition(sNextProcess) == "待加工")
                            {
                                //查询有没有可以加工的空闲设备
                                DataSet dsEquip51 = db_Equip.QueryEquipById(51);
                                DataSet dsEquip52 = db_Equip.QueryEquipById(52);
                                if (dsEquip51.Tables[0].Rows[0]["equipState"].ToString() == "空闲")
                                {
                                    if (Process.ProcessJudge(51, dsTrayInfo.Tables[0].Rows[0]["chanType"].ToString()))
                                    {
                                        //修改托盘状态
                                        Info = new object[] { null, _iTrayId, 51, null, null, null, sNextProcess, "待加工锁定", DateTime.Now.ToString() };
                                        //添加搬运任务
                                        DataBaseprogram.db_RgvtaskDao db_Rgvtask = new DataBaseprogram.db_RgvtaskDao();
                                        db_Rgvtask.InsertRgvTaskList(0, _iTrayId, 51, _iTrayId);
                                        //更新机床状态
                                        object[] EquipInfo = new object[4];
                                        EquipInfo[0] = "待加工"; EquipInfo[1] = dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString();
                                        EquipInfo[2] = dsTrayInfo.Tables[0].Rows[0]["chanType"].ToString(); EquipInfo[3] = _iTrayId;
                                        db_Equip.UpdateEquipInfo(51, EquipInfo);
                                    }
                                }
                                else if (dsEquip52.Tables[0].Rows[0]["equipState"].ToString() == "空闲")
                                {
                                    if (Process.ProcessJudge(52, dsTrayInfo.Tables[0].Rows[0]["chanType"].ToString()))
                                    {
                                        //修改托盘状态
                                        Info = new object[] { null, _iTrayId, 52, null, null, null, sNextProcess, "待加工锁定", DateTime.Now.ToString() };
                                        //添加搬运任务
                                        DataBaseprogram.db_RgvtaskDao db_Rgvtask = new DataBaseprogram.db_RgvtaskDao();
                                        db_Rgvtask.InsertRgvTaskList(0, _iTrayId, 52, _iTrayId);
                                        //更新机床状态
                                        object[] EquipInfo = new object[4];
                                        EquipInfo[0] = "待加工"; EquipInfo[1] = dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString();
                                        EquipInfo[2] = dsTrayInfo.Tables[0].Rows[0]["chanType"].ToString(); EquipInfo[3] = _iTrayId;
                                        db_Equip.UpdateEquipInfo(51, EquipInfo);
                                    }
                                }

                            }
                            else if (OverAll.ProcessTransition(sNextProcess) == "待清洗")
                            {
                                DataSet dsEquip = db_Equip.QueryEquipById(22);
                                if (dsEquip.Tables[0].Rows[0]["equipState"].ToString() == "空闲")
                                {
                                    //修改托盘状态
                                    Info = new object[] { null, _iTrayId, 22, null, null, null,null, sNextProcess, "待清洗锁定", DateTime.Now.ToString() };
                                    //添加搬运任务
                                    DataBaseprogram.db_RgvtaskDao db_Rgvtask = new DataBaseprogram.db_RgvtaskDao();
                                    db_Rgvtask.InsertRgvTaskList(0, _iTrayId, 22, _iTrayId);
                                    //更新机床状态
                                    object[] EquipInfo = new object[4];
                                    EquipInfo[0] = "待清洗"; EquipInfo[1] = dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString();
                                    EquipInfo[2] = dsTrayInfo.Tables[0].Rows[0]["chanType"].ToString(); EquipInfo[3] = _iTrayId;
                                    db_Equip.UpdateEquipInfo(22, EquipInfo);

                                }
                            }
                        }
                    }
                    cTrayInfo.UpdateTrayInfo(_iTrayId, Info);
                    #endregion
                    
                }
                #endregion
                #region 装卸站
                else if (_iEquipId == 21)
                {
                    string ProcessStep;
                    if (TrayVisible == 1)
                    {
                        ProcessStep = "装载中";
                    }
                    else if (TrayVisible == 2)
                    {
                        ProcessStep = "卸载中";
                    }
                    else if (TrayVisible == 3)
                    {
                        ProcessStep = "翻面中";
                    }
                    else {
                        ProcessStep = "";
                    }

                    //更新托盘信息
                    object[] Info = new object[] { null, 21, 0, null, null, null, null,null, ProcessStep, DateTime.Now.ToString() };
                    cTrayInfo.UpdateTrayInfo(_iTrayId, Info);

                    //更新装卸站信息
                    object[] EquipInfo = new object[4];

                    EquipInfo[0] = ProcessStep; EquipInfo[1] = dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString();
                    EquipInfo[2] = dsTrayInfo.Tables[0].Rows[0]["chanType"].ToString(); EquipInfo[3] = _iTrayId;
                    db_Equip.UpdateEquipInfo(22, EquipInfo);
                }
                #endregion
                #region 清洗房
                else if (_iEquipId == 22)
                {
                    //更新托盘信息
                    object[] Info = new object[] { null, 22, 0, null, null, null, null,null, "清洗中", DateTime.Now.ToString() };
                    cTrayInfo.UpdateTrayInfo(_iTrayId, Info);

                    //更新装卸站信息
                    object[] EquipInfo = new object[4];

                    EquipInfo[0] = "清洗中"; EquipInfo[1] = dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString();
                    EquipInfo[2] = dsTrayInfo.Tables[0].Rows[0]["chanType"].ToString(); EquipInfo[3] = _iTrayId;
                    db_Equip.UpdateEquipInfo(22, EquipInfo);

                    //开始清洗
                    List<string> NodeIdList = new List<string>();
                    List<string> ValueList = new List<string>();
                    NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());
                    ValueList.Add("True");
                    m_UaServer_Wash.WriteValues(ValueList, NodeIdList);
                }
                #endregion
                #region 缓存位
                else if (_iEquipId == 23)
                {
                    //发布搬运命令
                }
                #endregion


                //清空当前任务，并记录
                db_Equip.DeleteCarstate();
            }
            catch (Exception ex) {
                LogHelper.WriteLog(typeof(FormControl), ex);
            }
        }
        private void RgvGetComplete(int _iTrayId,int _iEquipId) {
            //更新设备信息
            DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
            object[] EquipInfo = new object[4];
            EquipInfo[0] = "空闲"; EquipInfo[1] = "";
            EquipInfo[2] = ""; EquipInfo[3] = 0;
            db_Equip.UpdateEquipInfo(_iEquipId, EquipInfo);
            //更新托盘信息
            DataBaseprogram.db_TuoDao cTrayInfo = new DataBaseprogram.db_TuoDao();
            object[] Info = new object[] { null, 80, null, null, null, null, null, null,null, DateTime.Now.ToString() };
            cTrayInfo.UpdateTrayInfo(_iTrayId, Info);
            //设备动作
            if (_iEquipId == 22) {
                //清洗房
                List<string> NodeIdList = new List<string>();
                List<string> ValueList = new List<string>();
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());
                ValueList.Add("True");
                m_UaServer_Wash.WriteValues(ValueList, NodeIdList);
            } else if (_iEquipId == 22) {
                //装卸站
                List<string> NodeIdList = new List<string>();
                List<string> ValueList = new List<string>();
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());
                ValueList.Add("True");
                m_UaServer_Plc.WriteValues(ValueList, NodeIdList);
            }

        }
        private bool ActionJudge(int iPosition) {
            if (iPosition < 19 || iPosition == 23)
            {
                return true;
            }
            else if (iPosition == 21) {//装卸站
                List<string> NodeIdList = new List<string>();
                List<string> ValueList = new List<string>();
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());//三坐标是否已经复位

                ValueList = m_UaServer_Plc.ReadValues(NodeIdList);
                if (ValueList[0] == "True")
                    return true;
                else
                    return false;
            } else if (iPosition == 22) {//清洗房
                List<string> NodeIdList = new List<string>();
                List<string> ValueList = new List<string>();
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());
                NodeIdList.Add(new NodeId(AddressDic.obtainAddress(10, ""), 3).ToString());

                ValueList = m_UaServer_Plc.ReadValues(NodeIdList);
                if (ValueList[0] == "True" && ValueList[1] == "True" && ValueList[2] == "True")
                    return true;
                else
                    return false;
            }
            return false;
        }
        #region AGV搬运
        private void StartHttpListener()
        {
            try
            {
                httpListener = new HttpListener();
                httpListenerWarning = new HttpListener();
                httpListener.Prefixes.Add(ListenerPort);
                httpListenerWarning.Prefixes.Add(ListenerPortWarning);
                httpListener.Start();
                httpListenerWarning.Start();

                listenerThread = new Thread(ListenForRequests);
                listenerThread.Start();
                listenerThreadWarning = new Thread(ListenWarningForRequests);
                listenerThreadWarning.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"启动HTTP监听器时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ListenForRequests()
        {
            try
            {
                while (httpListener.IsListening)
                {
                    HttpListenerContext context = httpListener.GetContext();
                    HttpListenerRequest request = context.Request;
                    // 处理请求
                    using (Stream body = request.InputStream)
                    using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
                    {
                        string formattedJson = reader.ReadToEnd();
                        JObject jsonObject = JObject.Parse(formattedJson);
                        Invoke(new Action(() =>
                        {
                            //agv任务状态
                            string agvState = (string)jsonObject["method"];
                            if (agvState == "end") {
                                string MapCode = (string)jsonObject["mapDataCode"];
                                //更新托盘信息
                                DataBaseprogram.db_AgvTaskDao db_AgvTaskDao = new DataBaseprogram.db_AgvTaskDao();
                                DataSet dsAgvTaskList = db_AgvTaskDao.QueryAgvTaskList();
                                int iTrayId = Convert.ToInt32(dsAgvTaskList.Tables[0].Rows[0]["TrayId"]);
                                int iTarget = Convert.ToInt32(dsAgvTaskList.Tables[0].Rows[0]["Target"]);
                                DataBaseprogram.db_TuoDao db_TuoDao = new DataBaseprogram.db_TuoDao();
                                object[] Info = new object[] { null, iTarget, 0, null, null, null, null, null, "AGV搬运中", DateTime.Now.ToString() };
                                db_TuoDao.UpdateTrayInfo(iTrayId, Info);
                                //删除当前任务
                                //db_AgvTaskDao.DeleteAgvTaskList();
                                //db_AgvTaskDao.RankAgvTaskList();
                                //执行后续搬运任务
                                //如果目标地点是机床缓存位，进行托盘交换任务
                                if (iTarget == 53) {

                                    string MainProgram = "EXCHANGE_IN.SPF";
                                    SinumerikOpcUaAPI.Server.MethodCallResult m_MethodCallResult = m_UaServer_M_1.MethodCallSelectProgram(string.Format(@"Sinumerik/FileSystem/Part Program/TRAYCARRY/{0}", MainProgram), 1);

                                    ////装载程序
                                    List<string> NodeList_Load = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "装载程序"), 2).ToString() };
                                    List<String> ValueList_Load = new List<string>() { "True" };
                                    m_UaServer_M_1.WriteValues(ValueList_Load, NodeList_Load);
                                    //执行
                                    List<string> NodeList_Start = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "执行程序"), 2).ToString() };
                                    List<String> ValueList_Start = new List<string>() { "True" };
                                    m_UaServer_M_1.WriteValues(ValueList_Start, NodeList_Start);
                                } else if (iTarget == 54) {

                                    string MainProgram = "EXCHANGE_IN.SPF";
                                    SinumerikOpcUaAPI.Server.MethodCallResult m_MethodCallResult = m_UaServer_M_2.MethodCallSelectProgram(string.Format(@"Sinumerik/FileSystem/Part Program/TRAYCARRY/{0}", MainProgram), 1);

                                    ////装载程序
                                    List<string> NodeList_Load = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "装载程序"), 2).ToString() };
                                    List<String> ValueList_Load = new List<string>() { "True" };
                                    m_UaServer_M_2.WriteValues(ValueList_Load, NodeList_Load);
                                    //执行
                                    List<string> NodeList_Start = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "执行程序"), 2).ToString() };
                                    List<String> ValueList_Start = new List<string>() { "True" };
                                    m_UaServer_M_2.WriteValues(ValueList_Start, NodeList_Start);
                                }
                            } else if (agvState == "outbin") {
                                //更新托盘信息
                                DataBaseprogram.db_AgvTaskDao db_AgvTaskDao = new DataBaseprogram.db_AgvTaskDao();
                                DataSet dsAgvTaskList = db_AgvTaskDao.QueryAgvTaskList();
                                int iTrayId = Convert.ToInt32(dsAgvTaskList.Tables[0].Rows[0]["TrayId"]);

                                DataBaseprogram.db_TuoDao db_TuoDao = new DataBaseprogram.db_TuoDao();
                                object[] Info = new object[] { null, 30, null, null, null, null, null, null, "AGV搬运中", DateTime.Now.ToString() };
                                db_TuoDao.UpdateTrayInfo(iTrayId, Info);
                            }
                        }));
                    }
                    // 返回响应
                    HttpListenerResponse response = context.Response;
                    response.StatusCode = 200;
                    response.ContentType = "application/json";
                    response.ContentEncoding = Encoding.UTF8;
                    Stream output = response.OutputStream;
                    JObject rp = new JObject()
                    {
                        { "code","0" },
                        { "message", "成功" },
                        { "reqCode", Guid.NewGuid().ToString() }
                    };
                    byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rp));
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
            catch (HttpListenerException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理HTTP请求时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListenWarningForRequests()
        {
            try
            {
                while (httpListener.IsListening)
                {
                    HttpListenerContext context = httpListenerWarning.GetContext();
                    HttpListenerRequest request = context.Request;
                    // 处理请求
                    using (Stream body = request.InputStream)
                    using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
                    {
                        //string formattedJson = MyJsonFormat(reader.ReadToEnd());
                        string formattedJson = reader.ReadToEnd();
                        Invoke(new Action(() =>
                        {
                            //rtbResp.ForeColor = Color.Red;
                            //rtbResp.Text = formattedJson;
                        }));
                    }
                    // 返回响应
                    HttpListenerResponse response = context.Response;
                    response.StatusCode = 200;
                    response.ContentType = "application/json";
                    response.ContentEncoding = Encoding.UTF8;
                    Stream output = response.OutputStream;
                    JObject rp = new JObject()
                    {
                        { "code","0" },
                        { "message", "成功" }
                    };
                    byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rp));
                    output.Write(buffer, 0, buffer.Length);
                    output.Close();
                }
            }
            catch (HttpListenerException)
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理HTTP请求时发生错误：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
        private void Timer1_Tick(object sender, EventArgs e)
        {
            //todo 读RGV的完成信号
            List<int> StateVaule = CncHelper.readIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "状态字段"));

            if (StateVaule[0] == 2)
            {
                try
                {
                    //拿完成，记录
                    DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
                    DataSet ds = db_Equip.QueryCarstate();
                    int iTrayId = Convert.ToInt32(ds.Tables[0].Rows[0]["TrayNo"]);
                    int iOriginPos = Convert.ToInt32(ds.Tables[0].Rows[0]["OriginPos"]);

                    DataSet dsTrayInfo = new DataBaseprogram.db_TuoDao().QueryTuoById(iTrayId);
                    OverAll.ProductRecord(dsTrayInfo.Tables[0].Rows[0]["chantype"].ToString(), dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString(),
                                     dsTrayInfo.Tables[0].Rows[0]["chanNo"].ToString(), "拿完成", string.Format("位置:{0}", iOriginPos));

                    RgvGetComplete(iTrayId, iOriginPos);
                    CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "状态字段"), 0);//状态地址写0
                }
                catch (Exception ex) {
                    LogHelper.WriteLog(typeof(FormControl), ex);
                }

                
            }
            else if (StateVaule[0] == 4) {
                try
                {
                    //放完成，记录
                    DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
                    DataSet ds = db_Equip.QueryCarstate();
                    int iTargetPos = Convert.ToInt32(ds.Tables[0].Rows[0]["TargetPos"]);
                    int iTrayId = Convert.ToInt32(ds.Tables[0].Rows[0]["TrayNo"]);

                    DataSet dsTrayInfo = new DataBaseprogram.db_TuoDao().QueryTuoById(iTrayId);
                    OverAll.ProductRecord(dsTrayInfo.Tables[0].Rows[0]["chantype"].ToString(), dsTrayInfo.Tables[0].Rows[0]["OrderId"].ToString(),
                                     dsTrayInfo.Tables[0].Rows[0]["chanNo"].ToString(), "放完成", string.Format("位置:{0}", iTargetPos));

                    CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "初始位置"), 0);//起始地址写0
                    CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "目标位置"), 0);//目标地址写0
                    CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "状态字段"), 0);//状态地址写0

                    DataBaseprogram.db_TuoDao cTrayInfo = new DataBaseprogram.db_TuoDao();
                    if (dsTrayInfo.Tables[0].Rows[0]["visible"].ToString() == "8")
                    {
                        //手动模式托盘不做处理
                        //更新任务列表
                        db_Equip.DeleteCarstate();
                        //更新RGV
                        object[] EquipInfo = new object[4];
                        EquipInfo[0] = "空闲"; EquipInfo[1] = "0";EquipInfo[2] = "0"; EquipInfo[3] = 0;
                        db_Equip.UpdateEquipInfo(80, EquipInfo);
                        //更新设备
                        EquipInfo[0] = "手动模式"; EquipInfo[1] = "0"; EquipInfo[2] = "0"; EquipInfo[3] = iTrayId;
                        db_Equip.UpdateEquipInfo(iTargetPos, EquipInfo);
                    }
                    else {
                        RgvPutComplete(iTrayId, iTargetPos);
                    }
                    db_Equip.UpdateCarState(0, 0, 0, 0);
                    Thread thread = new Thread(new ThreadStart(ThreadStartTask));//装卸站接受信号
                    thread.Start();
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(FormControl), ex);
                }
            }
            else if (StateVaule[0] == 1)
            {//拿到位
                try
                {
                    //查询位置
                    DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
                    DataSet ds = db_Equip.QueryCarstate();
                    int iOriginPos = Convert.ToInt32(ds.Tables[0].Rows[0]["OriginPos"]);
                    //判断是否有动作
                    if (ActionJudge(iOriginPos))
                    {
                        CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "状态字段"), 0);//状态地址写0
                        CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "拿地址"), 1);//可以拿地址写1

                    }
                } 
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(FormControl), ex);
                }
            }
            else if(StateVaule[0] == 3)
            {//放到位
                try { 
                    //查询位置
                    DataBaseprogram.db_EquipDao db_Equip = new DataBaseprogram.db_EquipDao();
                    DataSet ds = db_Equip.QueryCarstate();
                    int iOriginPos = Convert.ToInt32(ds.Tables[0].Rows[0]["TargetPos"]);
                    //判断是否有动作
                    if (ActionJudge(iOriginPos))
                    {
                        CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "状态字段"), 0);//状态地址写0
                        CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "放地址"), 1);//可以拿地址写1
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteLog(typeof(FormControl), ex);
                }
            }
            
        }
        #region 界面逻辑
        private void Button_Start_Click(object sender, EventArgs e)
        {
            ThreadStartTask();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CncHelper.writeIndex(AddressDic.EQUIP_RGV_Macro, (int)AddressDic.obtainAddress(AddressDic.EQUIP_RGV, "状态字段"), 0);//状态地址写0
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DataBaseprogram.db_TuoDao db_TuoDao = new DataBaseprogram.db_TuoDao();
            object[] Info = new object[] { null, 51, null, null, null, null, null,null, "待加工锁定", DateTime.Now.ToString() };
            db_TuoDao.UpdateTrayInfo(12, Info);
        }

        private void Button_TrayIntoCnc_Click(object sender, EventArgs e)
        {
            #region 控制机床跑程序
            Server UaServer;
            if (radioButton_Cnc1.Checked == true)
            {
                UaServer = m_UaServer_M_1;
            }
            else {
                UaServer = m_UaServer_M_2;
            }
            
            string MainProgram = "EXCHANGE_IN.SPF";
            SinumerikOpcUaAPI.Server.MethodCallResult m_MethodCallResult = UaServer.MethodCallSelectProgram(string.Format(@"Sinumerik/FileSystem/Part Program/TRAYCARRY/{0}", MainProgram), 1);


            ////装载程序
            List<string> NodeList_Load = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "装载程序"), 2).ToString() };
            List<String> ValueList_Load = new List<string>() { "True" };
            UaServer.WriteValues(ValueList_Load, NodeList_Load);
            //执行
            List<string> NodeList_Start = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "执行程序"), 2).ToString() };
            List<String> ValueList_Start = new List<string>() { "True" };
            UaServer.WriteValues(ValueList_Start, NodeList_Start);
            #endregion
        }

        private void Button_TrayOutCnc_Click(object sender, EventArgs e)
        {
            #region 控制机床跑程序
            Server UaServer;
            if (radioButton_Cnc1.Checked == true)
            {
                UaServer = m_UaServer_M_1;
            }
            else
            {
                UaServer = m_UaServer_M_2;
            }

            string MainProgram = "EXCHANGE_OUT.SPF";
            SinumerikOpcUaAPI.Server.MethodCallResult m_MethodCallResult = UaServer.MethodCallSelectProgram(string.Format(@"Sinumerik/FileSystem/Part Program/TRAYCARRY/{0}", MainProgram), 1);


            ////装载程序
            List<string> NodeList_Load = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "装载程序"), 2).ToString() };
            List<String> ValueList_Load = new List<string>() { "True" };
            UaServer.WriteValues(ValueList_Load, NodeList_Load);
            //执行
            List<string> NodeList_Start = new List<string>() { new NodeId(AddressDic.obtainAddress(1, "执行程序"), 2).ToString() };
            List<String> ValueList_Start = new List<string>() { "True" };
            UaServer.WriteValues(ValueList_Start, NodeList_Start);
            #endregion
        }

        private void RadioButton_Cnc1_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_Cnc1.Checked)
                radioButton_Cnc2.Checked = false;
        }

        private void RadioButton_Cnc2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_Cnc2.Checked)
                radioButton_Cnc1.Checked = false;
        }

        private void RadioButton_AgvOrigin_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_AgvOrigin.Checked)
                radioButton_AgvTarget.Checked = false;
        }

        private void RadioButton_AgvTarget_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_AgvTarget.Checked)
                radioButton_AgvOrigin.Checked = false;
        }
        private void Button_AgvPos_Click(object sender, EventArgs e) {

            Button SelectButton = (Button)sender;
            if (radioButton_AgvOrigin.Checked)
            {

                label_AgvOrigin.Text = SelectButton.Text;
            }
            else {
                label_AgvTarget.Text = SelectButton.Text;
            }

        }

        private void Button_AgvRun_Click(object sender, EventArgs e)
        {
            string agvCode = "";
            string uuid = Guid.NewGuid().ToString();
            string from = OverAll.AgvName2Pos(label_AgvOrigin.Text);
            string to = OverAll.AgvName2Pos(label_AgvTarget.Text);
            string req = AgvHelper.genAgvSchedulingTask1(uuid, from, to, "", "100001", "", "板件", agvCode);
           
        }
        #endregion
    }
}
