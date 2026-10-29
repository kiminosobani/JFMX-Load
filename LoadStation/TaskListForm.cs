using DSkin.Controls;
using DSkin.Forms;
using LoadStation.Sql.pojo;
using LoadStation.vo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static LoadStation.SQL.SqlSugarHelper;

namespace LoadStation
{
    public partial class TaskListForm : DSkinForm
    {
        private readonly Color TextColor = System.Drawing.Color.FromArgb(77, 220, 228);
        private readonly Color BackCol = System.Drawing.Color.FromArgb(3, 29, 54);

        private int iTurnFlag = 2;   // 待翻面 (visible = 2)
        private int iUnLoadFlag = 4; // 待卸载 (visible = 4)
        private int iAgain = 3;      // 待重装 (visible = 3)

        // re
        private DSkinButton pre = null;
        private int pageNumber;
        private int totalCount;
        private int totalPage;
        private int pageSize = 5;
        public TaskListForm()
        {
            InitializeComponent();
        }
        private void TaskListForm_Load(object sender, EventArgs e)
        {
            var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text = $"装卸站系统 - v{ver.Major}.{ver.Minor}.{ver.Build}.{ver.Revision}";

            //SqlSugarHelper.CreateEntity("order_record");
            // 设置列表不自动扩展数据
            dSkinDataGridView_Task.AutoGenerateColumns = false;
            // 设置右边数据
            InitLogisticList();
            // 显示 装 页面
            DSkinButton_Load_Click(dSkinButton_Load, null);
        }
        // 装 页面 表头
        private void LoadHeaderSet()
        {
            dSkinDataGridView_Task.Columns.Clear();
            DataGridViewTextBoxColumn cOrderNum = new DataGridViewTextBoxColumn();
            cOrderNum.HeaderText = "订单编号";
            cOrderNum.DataPropertyName = "orderNum";
            cOrderNum.Width = 77;

            DataGridViewTextBoxColumn cProduceName = new DataGridViewTextBoxColumn();
            cProduceName.HeaderText = "产品名称";
            cProduceName.DataPropertyName = "chanType";
            cProduceName.Width = 77;

            DataGridViewTextBoxColumn cTotalCount = new DataGridViewTextBoxColumn();
            cTotalCount.HeaderText = "总数量";
            cTotalCount.DataPropertyName = "totalNum";
            cTotalCount.Width = 65;

            DataGridViewTextBoxColumn cCompleteCount = new DataGridViewTextBoxColumn();
            cCompleteCount.HeaderText = "完成数量";
            cCompleteCount.DataPropertyName = "okNum";
            cCompleteCount.Width = 77;

            DataGridViewTextBoxColumn cStoreCount = new DataGridViewTextBoxColumn();
            cStoreCount.HeaderText = "在库数量";
            cStoreCount.DataPropertyName = "kuNum";
            cStoreCount.Width = 77;

            DataGridViewTextBoxColumn cLoadCount = new DataGridViewTextBoxColumn();
            cLoadCount.HeaderText = "待装数量";
            cLoadCount.DataPropertyName = "zhuang";
            cLoadCount.Width = 77;

            DataGridViewButtonColumn cOperate = new DataGridViewButtonColumn();
            cOperate.HeaderText = "操作";
            cOperate.Width = 37;

            dSkinDataGridView_Task.Columns.Add(cOrderNum);
            dSkinDataGridView_Task.Columns.Add(cProduceName);
            dSkinDataGridView_Task.Columns.Add(cTotalCount);
            dSkinDataGridView_Task.Columns.Add(cCompleteCount);
            dSkinDataGridView_Task.Columns.Add(cStoreCount);
            dSkinDataGridView_Task.Columns.Add(cLoadCount);
            dSkinDataGridView_Task.Columns.Add(cOperate);
        }
        // 翻面 页面 表头
        private void TurnHeaderSet()
        {
            dSkinDataGridView_Task.Columns.Clear();
            DataGridViewTextBoxColumn cOrderNum = new DataGridViewTextBoxColumn();
            cOrderNum.HeaderText = "订单编号";
            cOrderNum.DataPropertyName = "OrderId";
            cOrderNum.Width = 77;

            DataGridViewTextBoxColumn cProduceName = new DataGridViewTextBoxColumn();
            cProduceName.HeaderText = "产品名称";
            cProduceName.DataPropertyName = "chanType";
            cProduceName.Width = 77;

            DataGridViewTextBoxColumn cProduceNum = new DataGridViewTextBoxColumn();
            cProduceNum.HeaderText = "产品编号";
            cProduceNum.DataPropertyName = "chanNo";
            cProduceNum.Width = 77;

            DataGridViewTextBoxColumn cPosition = new DataGridViewTextBoxColumn();
            cPosition.HeaderText = "位置";
            cPosition.DataPropertyName = "position";
            cPosition.Width = 77;

            DataGridViewTextBoxColumn cTrayType = new DataGridViewTextBoxColumn();
            cTrayType.HeaderText = "工装型号";
            cTrayType.DataPropertyName = "FrockID";
            cTrayType.Width = 77;

            DataGridViewButtonColumn cOperate = new DataGridViewButtonColumn();
            cOperate.HeaderText = "操作";
            cOperate.Width = 37;

            dSkinDataGridView_Task.Columns.Add(cOrderNum);
            dSkinDataGridView_Task.Columns.Add(cProduceName);
            dSkinDataGridView_Task.Columns.Add(cProduceNum);
            dSkinDataGridView_Task.Columns.Add(cPosition);
            dSkinDataGridView_Task.Columns.Add(cTrayType);
            dSkinDataGridView_Task.Columns.Add(cOperate);
        }
        // 重装 页面 表头
        private void AgainHeaderSet()
        {
            TurnHeaderSet();
        }
        // 卸 页面 表头
        private void UnLoadHeaderSet()
        {
            TurnHeaderSet();
        }
        //主界面右边数据设置
        private void InitLogisticList()
        {
            List<RgvTaskListVo> taskList = Db.Queryable<Rgvtasklist>().OrderBy(it => it.PriorId).ToList().Select(
                    task => new RgvTaskListVo()
                    {
                        Initial = OverAll.PsInt2Name(task.Initial),
                        Target = OverAll.PsInt2Name(task.Target),
                    }
                ).ToList();
            dSkinDataGridView_Logistics.DataSource = taskList;
            //更新托盘数量显示信息
            DataBaseprogram.Db_TrayInfo db_TrayInfo = new DataBaseprogram.Db_TrayInfo();
            dSkinLabel_Total.Text = Db.Queryable<Tuo>().Where(it => it.visible == 0).Count().ToString();
            dSkinLabel_Process.Text = Db.Queryable<Tuo>().Where(it => it.chanState == "待加工").Count().ToString();
            dSkinLabel_UnLoad.Text = Db.Queryable<Tuo>().Where(it => it.chanState == "待卸载").Count().ToString();
            dSkinLabel_Load.Text = Db.Queryable<Tuo>().Where(it => it.chanState == "待翻面").Count().ToString();
        }
        //装 分页数据获取
        private void InitTaskList_Load()
        {
            List<Order1> list = Db.Queryable<Order1>().Where(it => it.zhuang > 0).ToPageList(pageNumber, pageSize, ref totalCount, ref totalPage);
            dSkinLabel1.Text = string.Format("共 {0} 项任务，共 {1} 页，当前第 {2} 页", totalCount, totalPage, pageNumber);
            dSkinDataGridView_Task.DataSource = list;
            for (int i = 0; i < list.Count; i++)
            {
                dSkinDataGridView_Task.Rows[i].Cells[6].Value = "执行";
            }
        }
        //重装 分页数据获取
        private void InitTaskList_Again()
        {
            List<Tuo> list = Db.Queryable<Tuo>().Where(it => it.visible == iAgain).ToPageList(pageNumber, pageSize, ref totalCount, ref totalPage);
            dSkinLabel1.Text = string.Format("共 {0} 项任务，共 {1} 页，当前第 {2} 页", totalCount, totalPage, pageNumber);
            dSkinDataGridView_Task.DataSource = list;
            for (int i = 0; i < list.Count; i++)
            {
                dSkinDataGridView_Task.Rows[i].Cells[5].Value = "执行";
            }
        }
        //卸载 分页数据获取
        private void InitTaskList_UnLoad()
        {
            List<Tuo> list = Db.Queryable<Tuo>().Where(it => it.visible == iUnLoadFlag).ToPageList(pageNumber, pageSize, ref totalCount, ref totalPage);
            dSkinLabel1.Text = string.Format("共 {0} 项任务，共 {1} 页，当前第 {2} 页", totalCount, totalPage, pageNumber);
            dSkinDataGridView_Task.DataSource = list;
            for (int i = 0; i < list.Count; i++)
            {
                dSkinDataGridView_Task.Rows[i].Cells[5].Value = "执行";
            }
        }
        //翻面 分页数据获取
        private void InitTaskList_Turn()
        {
            List<Tuo> list = Db.Queryable<Tuo>().Where(it => it.visible == iTurnFlag).ToPageList(pageNumber, pageSize, ref totalCount, ref totalPage);
            dSkinLabel1.Text = string.Format("共 {0} 项任务，共 {1} 页，当前第 {2} 页", totalCount, totalPage, pageNumber);
            dSkinDataGridView_Task.DataSource = list;
            for (int i = 0; i < list.Count; i++)
            {
                dSkinDataGridView_Task.Rows[i].Cells[5].Value = "执行";
            }
        }
        // 分页数据获取
        private void InitTask()
        {
            if (pre == dSkinButton_Load)
            {
                InitTaskList_Load();
            }
            else if (pre == dSkinButton_Again)
            {
                InitTaskList_Again();
            }
            else if (pre == dSkinButton_UnLoad)
            {
                InitTaskList_UnLoad();
            }
            else if (pre == dSkinButton_Turn)
            {
                InitTaskList_Turn();
            }
        }
        //上一页
        private void DSkinButton_Back_Click(object sender, EventArgs e)
        {
            pageNumber = pageNumber - 1 > 0 ? pageNumber - 1 : 1;
            InitTask();
        }
        //下一页
        private void DSkinButton_Next_Click(object sender, EventArgs e)
        {
            pageNumber = pageNumber + 1 > totalPage ? totalPage : pageNumber + 1;
            InitTask();
        }
        //绘制执行按钮
        private void DSkinDataGridView_Task_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (pre == dSkinButton_Load && e.ColumnIndex == 6 && e.RowIndex > -1)
            {
                e.Handled = true;
                using (SolidBrush brush = new SolidBrush(BackCol))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }
                using (Pen pen = new Pen(TextColor))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 2, e.CellBounds.Height - 2));
                }
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString("执行", new Font("微软雅黑", 10F), new SolidBrush(TextColor), e.CellBounds, sf);
                ControlPaint.DrawBorder(e.Graphics, new Rectangle(e.CellBounds.X + 2, e.CellBounds.Y + 2, e.CellBounds.Width - 4, e.CellBounds.Height - 4), TextColor, ButtonBorderStyle.Solid);
            }
            else if (pre != dSkinButton_Load && e.ColumnIndex == 5 && e.RowIndex > -1)
            {
                e.Handled = true;
                using (SolidBrush brush = new SolidBrush(BackCol))
                {
                    e.Graphics.FillRectangle(brush, e.CellBounds);
                }
                using (Pen pen = new Pen(TextColor))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 2, e.CellBounds.Height - 2));
                }
                //e.CellStyle.ForeColor = TextColor;
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                e.Graphics.DrawString("执行", new Font("微软雅黑", 10F), new SolidBrush(TextColor), e.CellBounds, sf);
                ControlPaint.DrawBorder(e.Graphics, new Rectangle(e.CellBounds.X + 2, e.CellBounds.Y + 2, e.CellBounds.Width - 4, e.CellBounds.Height - 4), TextColor, ButtonBorderStyle.Solid);
            }
        }
        //执行按钮逻辑
        private void DSkinDataGridView_Task_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // 判断装卸站是否有托盘
            Equip load = Db.Queryable<Equip>().InSingle(22);
            if (load.TrayId != 0)
            {
                MessageBoxEx msg = new MessageBoxEx();
                msg.ShowDialog("错误", "装卸站内有托盘，无法入站！", "", MessageBoxEx.MessageBoxStyle.error);
                return;
            }
            // 判断装卸站是否存在任务
            int iCount = Db.Queryable<Rgvtasklist>().Where(it => it.Initial == 22 || it.Target == 22).Count(); ;
            if (iCount > 0)
            {
                //提醒用户
                MessageBoxEx msg = new MessageBoxEx();
                msg.ShowDialog("错误", "装卸站存在搬运任务，无法入站！", "", MessageBoxEx.MessageBoxStyle.error);
                return;
            }

            if (pre == dSkinButton_Load)
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewColumn column = dSkinDataGridView_Task.Columns[e.ColumnIndex];
                    if (column is DataGridViewButtonColumn)
                    {
                        string OrderNum = dSkinDataGridView_Task.Rows[e.RowIndex].Cells[0].Value.ToString();
                        TaskOperateForm fTaskOperateForm = new TaskOperateForm(OrderNum);
                        fTaskOperateForm.ShowDialog();
                    }
                }
            }
            else
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewColumn column = dSkinDataGridView_Task.Columns[e.ColumnIndex];
                    if (column is DataGridViewButtonColumn)
                    {
                        Tuo selectedTuo = dSkinDataGridView_Task.Rows[e.RowIndex].DataBoundItem as Tuo;
                        int iTaryNo = selectedTuo != null ? selectedTuo.tuoId : Convert.ToInt32(dSkinDataGridView_Task.Rows[e.RowIndex].Cells[3].Value);
                        TaskOperateForm fTaskOperateForm = new TaskOperateForm(iTaryNo, ButtonToType());
                        fTaskOperateForm.ShowDialog();
                    }
                }
            }
            //todo 数据刷新
            InitTask();
        }
        private int ButtonToType()
        {
            if (pre == dSkinButton_Turn)
            {
                return 2;
            }
            else if (pre == dSkinButton_Again)
            {
                return 3;
            }
            else if (pre == dSkinButton_UnLoad)
            {
                return 4;
            }
            return -1;
        }
        //手动控制按钮逻辑
        private void DSkinButton_HM_Click(object sender, EventArgs e)
        {
            ManualControlForm fmanualControlForm = new ManualControlForm();
            fmanualControlForm.ShowDialog();
        }
        //按钮选中效果
        private void StyleReset(object sender)
        {
            DSkinButton dSkinButton = sender as DSkinButton;
            if (dSkinButton == null) return;
            if (pre != null && pre != dSkinButton)
            {
                pre.BackColor = BackCol;
                pre.ForeColor = TextColor;
            }
            dSkinButton.BackColor = TextColor;
            dSkinButton.ForeColor = BackCol;
            pre = dSkinButton;
        }
        //装 页面 按钮
        private void DSkinButton_Load_Click(object sender, EventArgs e)
        {
            StyleReset(sender);
            LoadHeaderSet();
            pageNumber = 1;
            InitTask();
        }
        //卸 页面 按钮
        private void DSkinButton_UnLoad_Click(object sender, EventArgs e)
        {
            StyleReset(sender);
            UnLoadHeaderSet();
            InitTask();
        }
        //翻面 页面 按钮
        private void DSkinButton_Turn_Click(object sender, EventArgs e)
        {
            StyleReset(sender);
            TurnHeaderSet();
            pageNumber = 1;
            InitTask();
        }
        //重装 页面 按钮
        private void DSkinButton_Again_Click(object sender, EventArgs e)
        {
            StyleReset(sender);
            AgainHeaderSet();
            pageNumber = 1;
            InitTask();
        }
        // 关闭 按钮
        private void Button_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #region 控件大小随窗体大小等比例缩放

        private float x;//定义当前窗体的宽度
        private float y;//定义当前窗体的高度

        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }

        //设置双缓冲区、解决闪屏问题
        public static void SetDouble(Control cc)
        {
            cc.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance |
                         System.Reflection.BindingFlags.NonPublic).SetValue(cc, true, null);
        }

        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                SetDouble(this);
                SetDouble(con);
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }

        #endregion 控件大小随窗体大小等比例缩放 
    }
}
