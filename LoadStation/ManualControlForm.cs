using DSkin.Forms;
using LoadStation.Sql.pojo;
using LoadStation.vo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using static LoadStation.SQL.SqlSugarHelper;

namespace LoadStation
{
    public partial class ManualControlForm : DSkinForm
    {
        public ManualControlForm()
        {
            InitializeComponent();
        }

        private void DSkinButton_iTray_Click(object sender, EventArgs e)
        {
            NumberSelectForm fnumberSelectForm = new NumberSelectForm();
            DialogResult dr = fnumberSelectForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                dSkinLabel_iPosition.Text = fnumberSelectForm.ITary.ToString();
            }
        }

        private void DSkinButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DSkinButton_iEquip_Click(object sender, EventArgs e)
        {
            EquipSelectForm fEquipSelectForm = new EquipSelectForm();
            DialogResult dr = fEquipSelectForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string pos = fEquipSelectForm.Position;
                dSkinLabel_iPosition.Text = pos;
            }
        }

        private void DSkinButton2_Click(object sender, EventArgs e)
        {
            EquipSelectForm fEquipSelectForm = new EquipSelectForm();
            DialogResult dr = fEquipSelectForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                dSkinLabel_dPosition.Text = fEquipSelectForm.Position;
            }
        }

        private void DSkinButton_dTray_Click(object sender, EventArgs e)
        {
            NumberSelectForm fnumberSelectForm = new NumberSelectForm();
            DialogResult dr = fnumberSelectForm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                dSkinLabel_dPosition.Text = fnumberSelectForm.ITary.ToString();
            }
        }

        private void DSkinLabel_iPosition_TextChanged(object sender, EventArgs e)
        {
            string iPosition = dSkinLabel_iPosition.Text;
            //查询设备上的托盘信息
            Equip equip = Db.Queryable<Equip>().InSingle(OverAll.Name2Pos(iPosition));
            Tuo tuo = Db.Queryable<Tuo>().InSingle(equip.TrayId);
            if (tuo != null)
            {
                dSkinLabel_iTaryInfo.Text = tuo.tuoId + "";
                string ProduceType = tuo.chanType;
                if (ProduceType == "" || ProduceType == "0")
                    dSkinLabel_iProduceInfo.Text = "无";
                else
                    dSkinLabel_iProduceInfo.Text = ProduceType;
            }
            else
            {
                dSkinLabel_iTaryInfo.Text = equip.equipState;
                dSkinLabel_iProduceInfo.Text = equip.equipState;
            }
        }

        private void DSkinLabel_dPosition_TextChanged(object sender, EventArgs e)
        {
            string dPosition = dSkinLabel_dPosition.Text;
            //查询设备上的托盘信息
            Equip equip = Db.Queryable<Equip>().InSingle(OverAll.Name2Pos(dPosition));
            Tuo tuo = Db.Queryable<Tuo>().InSingle(equip.TrayId);
            if (tuo != null)
            {
                dSkinLabel_dTaryInfo.Text = tuo.tuoId + "";
                string ProduceType = tuo.chanType;
                if (ProduceType == "" || ProduceType == "0")
                    dSkinLabel_dProduceInfo.Text = "无";
                else
                    dSkinLabel_dProduceInfo.Text = ProduceType;
            }
            else
            {
                dSkinLabel_dTaryInfo.Text = equip.equipState;
                dSkinLabel_dProduceInfo.Text = equip.equipState;
            }
        }


        protected bool isNumberic(string message, out int result)
        {
            System.Text.RegularExpressions.Regex rex =
            new System.Text.RegularExpressions.Regex(@"^\d+$");
            result = -1;
            if (rex.IsMatch(message))
            {
                result = int.Parse(message);
                return true;
            }
            else
                return false;
        }

        private async void DSkinButton_Add_Click(object sender, EventArgs e)
        {
            MessageBoxEx msg = new MessageBoxEx();

            string siPosition = dSkinLabel_iPosition.Text;

            int iiPosition = 0;//初始位置
            string siTary = dSkinLabel_iTaryInfo.Text;
            int iiTary = 0;//初始位置
            string sdPosition = dSkinLabel_dPosition.Text;
            int isdPosition = 0;//目标位置


            string TaryId = dSkinLabel_iTaryInfo.Text;
            int iTaryNo;
            if (!isNumberic(siTary, out iiTary))
            {
                msg.ShowDialog("警告", "当前位置没有托盘！", "", MessageBoxEx.MessageBoxStyle.error);
                return;
            }
            else
            {
                iTaryNo = Convert.ToInt32(dSkinLabel_iTaryInfo.Text);//托盘编号
            }
            if (!isNumberic(siPosition, out iiPosition))
            {
                iiPosition = Db.Queryable<Equip>().InSingle(OverAll.Name2Pos(siPosition)).equipId;
            }

            if (!isNumberic(sdPosition, out isdPosition))
            {
                isdPosition = Db.Queryable<Equip>().InSingle(OverAll.Name2Pos(sdPosition)).equipId;
            }

            Equip equipInfo = Db.Queryable<Equip>().InSingle(isdPosition);
            if (equipInfo.TrayId != 0)
            {
                msg.ShowDialog("错误", "终点位置内有托盘，请确认！", "", MessageBoxEx.MessageBoxStyle.error);
                return;
            }

            //检索当前托盘是否已经有任务
            if (Db.Queryable<Rgvtasklist>().Where(it => it.Initial == iTaryNo || it.Target == iTaryNo).Count() > 0)
            {
                msg.ShowDialog("错误", "该托盘已经有任务，请等待！", "", MessageBoxEx.MessageBoxStyle.error);
                return;
            }
            Tuo tuo = Db.Queryable<Tuo>().InSingle(iTaryNo);
            if (tuo.visible != 8)
            {
                DialogResult ds = msg.ShowDialog("错误", "该托盘未手动锁定，请到网页锁定需要操作的托盘！", "", MessageBoxEx.MessageBoxStyle.error);
                return;
            }
            tuo.visible = 8; tuo.chanNo = "0"; tuo.OrderId = "0"; tuo.chanType = "0"; tuo.task = "0";
            tuo.chanState = "手动模式锁定"; tuo.dateStart = DateTime.Now;
            Db.Updateable(tuo).ExecuteCommand();

            if (isdPosition == 15 || isdPosition == 16)
            {
                Equip equip = Db.Queryable<Equip>().InSingle(isdPosition + 8);
                if (equip.equipState != "离线")
                {
                    msg.ShowDialog("错误", "设备未离线！", "", MessageBoxEx.MessageBoxStyle.error);
                    return;
                }
                object[] EquipInfo = new object[4];
                if (equip.TrayId == 0)
                {
                    Db.Insertable<Rgvtasklist>(new Rgvtasklist(iiPosition, isdPosition, iTaryNo, 0, 1, "创建")).ExecuteCommand();
                }
                else
                {
                    msg.ShowDialog("错误", "终点位置内有托盘！", "", MessageBoxEx.MessageBoxStyle.error);
                    return;
                }
            }
            else
            {
                int taskId = Db.Insertable<Rgvtasklist>(new Rgvtasklist(iiPosition, isdPosition, iTaryNo, 0, 1, "创建")).ExecuteReturnIdentity();
                using (TcpClient client = new TcpClient())
                {
                    try
                    {
                        await client.ConnectAsync("localhost", 5432);
                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] data = Encoding.UTF8.GetBytes($"手动搬运:{taskId}");
                            await stream.WriteAsync(data, 0, data.Length);
                            await stream.FlushAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                msg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);
            }
            InitList();
        }

        private void ManualControlForm_Load(object sender, EventArgs e)
        {
            InitList();
        }
        private void InitList()
        {
            List<RgvTaskListVo> taskList = Db.Queryable<Rgvtasklist>().OrderBy(it => it.PriorId).ToList().Select(
                    task => new RgvTaskListVo()
                    {
                        Initial = OverAll.PsInt2Name(task.Initial),
                        Target = OverAll.PsInt2Name(task.Target),
                    }
                ).ToList();
            dSkinDataGridView_Total.DataSource = taskList;
        }
        private void DSkinButton_ReList_Click(object sender, EventArgs e)
        {
            //查询当前是否有运行的程序
            Equip equip = Db.Queryable<Equip>().InSingle(80);
            List<Rgvtasklist> rgvtaskList = Db.Queryable<Rgvtasklist>().ToList();
            if (equip.equipState != "空闲" || rgvtaskList.Count > 0)
            {
                MessageBoxEx messageBoxEx = new MessageBoxEx();
                if (messageBoxEx.ShowDialog("警告", "目前有搬运任务，是否清空当前任务，可能会引发异常", "点击确认清空当前所有任务，包括正在执行的，点击取消退出当前操作", MessageBoxEx.MessageBoxStyle.warning) == DialogResult.Cancel)
                {
                    return;
                }
            }

            //清空任务列表
            Db.Deleteable<Rgvtasklist>().ExecuteCommand();
            //清空当前工作列表
            //刷新显示列表
            InitList();

        }

        private async void DSkinButton_Execute_Click(object sender, EventArgs e)
        {
            MessageBoxEx messageBoxEx = new MessageBoxEx();
            //查询当前小车是否空闲
            Equip rgv = Db.Queryable<Equip>().InSingle(80);
            if (rgv.equipState != "空闲")
            {
                messageBoxEx.ShowDialog("通知", "当前穿梭车正在执行任务，无需启动", "如果穿梭车没有进行任务，请联系相关人员", MessageBoxEx.MessageBoxStyle.warning);
                return;
            }
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    try
                    {
                        await client.ConnectAsync("localhost", 5432);
                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] data = Encoding.UTF8.GetBytes("手动搬运");
                            await stream.WriteAsync(data, 0, data.Length);
                            await stream.FlushAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                MessageBoxEx msg = new MessageBoxEx();
                msg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DSkinButton_Init_Click(object sender, EventArgs e)
        {

            //Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPAddress ipaddress = IPAddress.Parse("10.50.42.41");
            //EndPoint point = new IPEndPoint(ipaddress, 7788);
            //tcpClient.Connect(point);//通过IP和端口号来定位一个所要连接的服务器端

            //byte[] data = new byte[] { 0x02 };
            //tcpClient.Send(data);

            //tcpClient.Close();


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Refresh();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Refresh();
        }

        private async void dSkinButton1_Click(object sender, EventArgs e)
        {
            MessageBoxEx messageBoxEx = new MessageBoxEx();
            List<Rgvtasklist> tasklist = Db.Queryable<Rgvtasklist>().ToList();
            if (tasklist.Count > 0)
            {
                messageBoxEx.ShowDialog("通知", "", "已存在任务，单步任务不能执行多个", MessageBoxEx.MessageBoxStyle.error);
                return;
            }
            string position = comboBox2.Text;
            string action = comboBox1.Text;
            if (position == "" || action == "")
            {
                messageBoxEx.ShowDialog("通知", "", "请选择操作位置和执行的动作", MessageBoxEx.MessageBoxStyle.warning);
                return;
            }
            Equip rgv = Db.Queryable<Equip>().InSingle(80);
            int pos = OverAll.Pos(position);
            int trayId = 0;
            if (pos == -1)
            {
                messageBoxEx.ShowDialog("通知", "", "错误位置", MessageBoxEx.MessageBoxStyle.error);
                return;
            }
            int task = 0;
            if (action == "移动")
            {
                task = 1;
            }
            else if (action == "取托盘")
            {
                Equip target = Db.Queryable<Equip>().InSingle(OverAll.Pos(position));
                if (target.TrayId == 0 || target.equipState == "弃用")
                {
                    messageBoxEx.ShowDialog("通知", "", "目标位置无托盘或者暂时弃用，不能执行取托盘", MessageBoxEx.MessageBoxStyle.error);
                    return;
                }
                task = 2;
                trayId = target.TrayId;
            }
            else if (action == "放托盘")
            {
                if (rgv.TrayId == 0)
                {
                    messageBoxEx.ShowDialog("通知", "", "穿梭车上没有托盘，不能执行放托盘", MessageBoxEx.MessageBoxStyle.error);
                    return;
                }
                Equip target = Db.Queryable<Equip>().InSingle(OverAll.Pos(position));
                if (target.TrayId != 0 || target.equipState == "弃用")
                {
                    messageBoxEx.ShowDialog("通知", "", "目标位置有托盘或者暂时弃用，不能执行放托盘", MessageBoxEx.MessageBoxStyle.error);
                    return;
                }
                task = 3;
                trayId = rgv.TrayId;
            }
            else
            {
                messageBoxEx.ShowDialog("通知", "", "未知操作", MessageBoxEx.MessageBoxStyle.error);
                return;
            }
            int taskId = Db.Insertable(new Rgvtasklist(rgv.equipId, pos, trayId, task, task, "创建")).ExecuteReturnIdentity();
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    await client.ConnectAsync("localhost", 5432);
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes($"单步搬运:{taskId}");
                        await stream.WriteAsync(data, 0, data.Length);
                        await stream.FlushAsync();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            MessageBoxEx msg = new MessageBoxEx();
            msg.ShowDialog1("", "开始执行", "", MessageBoxEx.MessageBoxStyle.normal);
            InitList();
        }

        private void dSkinButton2_Click_1(object sender, EventArgs e)
        {
            MessageBoxEx messageBoxEx = new MessageBoxEx();
            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse(OverAll.SOCKET_IP);
            EndPoint point = new IPEndPoint(ipaddress, 7788);
            tcpClient.Connect(point);//通过IP和端口号来定位一个所要连接的服务器端

            byte[] data = new byte[] { 0x03 };
            tcpClient.Send(data);
            tcpClient.Close();
            messageBoxEx.ShowDialog("通知", "", "开启循环任务", MessageBoxEx.MessageBoxStyle.normal);
            label1.Text = "循环任务启动中";
            label1.ForeColor = Color.Green;
        }

        private void dSkinButton3_Click(object sender, EventArgs e)
        {
            MessageBoxEx messageBoxEx = new MessageBoxEx();
            Socket tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipaddress = IPAddress.Parse(OverAll.SOCKET_IP);
            EndPoint point = new IPEndPoint(ipaddress, 7788);
            tcpClient.Connect(point);//通过IP和端口号来定位一个所要连接的服务器端

            byte[] data = new byte[] { 0x04 };
            tcpClient.Send(data);
            tcpClient.Close();
            messageBoxEx.ShowDialog("通知", "", "关闭循环任务，当前任务结束后不再下发新任务", MessageBoxEx.MessageBoxStyle.normal);
            label1.Text = "循环任务关闭中";
            label1.ForeColor = Color.Red;
        }
    }
}
