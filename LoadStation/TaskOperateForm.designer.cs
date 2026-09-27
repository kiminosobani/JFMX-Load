namespace LoadStation
{
    partial class TaskOperateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskOperateForm));
            this.dSkinLabel_Title = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel_ProduceType = new DSkin.Controls.DSkinLabel();
            this.dSkinPictureBox_Produce = new DSkin.Controls.DSkinPictureBox();
            this.dSkinPictureBox_Process = new DSkin.Controls.DSkinPictureBox();
            this.dSkinButton_InTo = new DSkin.Controls.DSkinButton();
            this.dSkinButton_PutOut = new DSkin.Controls.DSkinButton();
            this.dSkinButton_Cancel = new DSkin.Controls.DSkinButton();
            this.dSkinLabel_TrayNo = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel_OrderNo = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel_Mark = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel_FrockId = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel_Reject = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel1 = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel2 = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel4 = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel5 = new DSkin.Controls.DSkinLabel();
            this.dSkinTextBox_ProduceNo = new DSkin.Controls.DSkinTextBox();
            this.btnTest = new DSkin.Controls.DSkinButton();
            this.lblAutoCheck = new DSkin.Controls.DSkinLabel();
            this.SuspendLayout();
            // 
            // dSkinLabel_Title
            // 
            this.dSkinLabel_Title.Font = new System.Drawing.Font("Microsoft YaHei", 16F, System.Drawing.FontStyle.Bold);
            this.dSkinLabel_Title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinLabel_Title.Location = new System.Drawing.Point(38, 140);
            this.dSkinLabel_Title.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinLabel_Title.Name = "dSkinLabel_Title";
            this.dSkinLabel_Title.Size = new System.Drawing.Size(139, 31);
            this.dSkinLabel_Title.TabIndex = 3;
            this.dSkinLabel_Title.Text = "装载任务信息";
            // 
            // dSkinLabel_ProduceType
            // 
            this.dSkinLabel_ProduceType.AutoSize = false;
            this.dSkinLabel_ProduceType.Font = new System.Drawing.Font("Microsoft YaHei", 8F, System.Drawing.FontStyle.Bold);
            this.dSkinLabel_ProduceType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinLabel_ProduceType.Location = new System.Drawing.Point(75, 361);
            this.dSkinLabel_ProduceType.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinLabel_ProduceType.Name = "dSkinLabel_ProduceType";
            this.dSkinLabel_ProduceType.Size = new System.Drawing.Size(196, 35);
            this.dSkinLabel_ProduceType.TabIndex = 6;
            this.dSkinLabel_ProduceType.Text = "产品型号：XXXX";
            // 
            // dSkinPictureBox_Produce
            // 
            this.dSkinPictureBox_Produce.BackColor = System.Drawing.Color.LightGray;
            this.dSkinPictureBox_Produce.Image = global::LoadStation.Properties.Resources.NoImage;
            this.dSkinPictureBox_Produce.Images = new System.Drawing.Image[] {
        ((System.Drawing.Image)(global::LoadStation.Properties.Resources.NoImage))};
            this.dSkinPictureBox_Produce.Location = new System.Drawing.Point(340, 140);
            this.dSkinPictureBox_Produce.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinPictureBox_Produce.Name = "dSkinPictureBox_Produce";
            this.dSkinPictureBox_Produce.Size = new System.Drawing.Size(379, 209);
            this.dSkinPictureBox_Produce.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.dSkinPictureBox_Produce.TabIndex = 8;
            this.dSkinPictureBox_Produce.Text = "dSkinPictureBox2";
            this.dSkinPictureBox_Produce.Click += new System.EventHandler(this.DSkinPictureBox_Process_Click);
            this.dSkinPictureBox_Produce.DoubleClick += new System.EventHandler(this.DSkinPictureBox_Process_Click);
            // 
            // dSkinPictureBox_Process
            // 
            this.dSkinPictureBox_Process.BackColor = System.Drawing.Color.LightGray;
            this.dSkinPictureBox_Process.Image = global::LoadStation.Properties.Resources.NoImage;
            this.dSkinPictureBox_Process.Images = new System.Drawing.Image[] {
        ((System.Drawing.Image)(global::LoadStation.Properties.Resources.NoImage))};
            this.dSkinPictureBox_Process.Location = new System.Drawing.Point(340, 369);
            this.dSkinPictureBox_Process.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinPictureBox_Process.Name = "dSkinPictureBox_Process";
            this.dSkinPictureBox_Process.Size = new System.Drawing.Size(379, 204);
            this.dSkinPictureBox_Process.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.dSkinPictureBox_Process.TabIndex = 9;
            this.dSkinPictureBox_Process.Text = "dSkinPictureBox3";
            this.dSkinPictureBox_Process.Click += new System.EventHandler(this.DSkinPictureBox_Process_Click);
            this.dSkinPictureBox_Process.DoubleClick += new System.EventHandler(this.DSkinPictureBox_Process_Click);
            // 
            // dSkinButton_InTo
            // 
            this.dSkinButton_InTo.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinButton_InTo.Borders.AllWidth = 0;
            this.dSkinButton_InTo.Borders.BottomWidth = 0;
            this.dSkinButton_InTo.Borders.LeftWidth = 0;
            this.dSkinButton_InTo.Borders.RightWidth = 0;
            this.dSkinButton_InTo.Borders.TopWidth = 0;
            this.dSkinButton_InTo.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinButton_InTo.ButtonBorderWidth = 0;
            this.dSkinButton_InTo.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_InTo.Font = new System.Drawing.Font("Microsoft YaHei", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinButton_InTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(29)))), ((int)(((byte)(54)))));
            this.dSkinButton_InTo.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_InTo.HoverImage = null;
            this.dSkinButton_InTo.Image = ((System.Drawing.Image)(resources.GetObject("dSkinButton_InTo.Image")));
            this.dSkinButton_InTo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dSkinButton_InTo.ImageOffset = new System.Drawing.Point(10, 0);
            this.dSkinButton_InTo.Location = new System.Drawing.Point(811, 140);
            this.dSkinButton_InTo.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinButton_InTo.Name = "dSkinButton_InTo";
            this.dSkinButton_InTo.NormalImage = null;
            this.dSkinButton_InTo.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_InTo.PressedImage = null;
            this.dSkinButton_InTo.Radius = 0;
            this.dSkinButton_InTo.ShowButtonBorder = true;
            this.dSkinButton_InTo.Size = new System.Drawing.Size(160, 67);
            this.dSkinButton_InTo.TabIndex = 10;
            this.dSkinButton_InTo.Text = "入站";
            this.dSkinButton_InTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_InTo.TextInnerPadding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.dSkinButton_InTo.TextPadding = 0;
            this.dSkinButton_InTo.Click += new System.EventHandler(this.DSkinButton_InTo_Click);
            // 
            // dSkinButton_PutOut
            // 
            this.dSkinButton_PutOut.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinButton_PutOut.Borders.AllWidth = 0;
            this.dSkinButton_PutOut.Borders.BottomWidth = 0;
            this.dSkinButton_PutOut.Borders.LeftWidth = 0;
            this.dSkinButton_PutOut.Borders.RightWidth = 0;
            this.dSkinButton_PutOut.Borders.TopWidth = 0;
            this.dSkinButton_PutOut.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinButton_PutOut.ButtonBorderWidth = 0;
            this.dSkinButton_PutOut.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_PutOut.Font = new System.Drawing.Font("Microsoft YaHei", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinButton_PutOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(29)))), ((int)(((byte)(54)))));
            this.dSkinButton_PutOut.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_PutOut.HoverImage = null;
            this.dSkinButton_PutOut.Image = ((System.Drawing.Image)(resources.GetObject("dSkinButton_PutOut.Image")));
            this.dSkinButton_PutOut.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.dSkinButton_PutOut.ImageOffset = new System.Drawing.Point(10, 0);
            this.dSkinButton_PutOut.Location = new System.Drawing.Point(811, 408);
            this.dSkinButton_PutOut.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinButton_PutOut.Name = "dSkinButton_PutOut";
            this.dSkinButton_PutOut.NormalImage = null;
            this.dSkinButton_PutOut.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_PutOut.PressedImage = null;
            this.dSkinButton_PutOut.Radius = 0;
            this.dSkinButton_PutOut.ShowButtonBorder = true;
            this.dSkinButton_PutOut.Size = new System.Drawing.Size(160, 67);
            this.dSkinButton_PutOut.TabIndex = 12;
            this.dSkinButton_PutOut.Text = " 出站";
            this.dSkinButton_PutOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_PutOut.TextInnerPadding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.dSkinButton_PutOut.TextPadding = 0;
            this.dSkinButton_PutOut.Click += new System.EventHandler(this.DSkinButton_PutOut_Click);
            // 
            // dSkinButton_Cancel
            // 
            this.dSkinButton_Cancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinButton_Cancel.Borders.AllWidth = 0;
            this.dSkinButton_Cancel.Borders.BottomWidth = 0;
            this.dSkinButton_Cancel.Borders.LeftWidth = 0;
            this.dSkinButton_Cancel.Borders.RightWidth = 0;
            this.dSkinButton_Cancel.Borders.TopWidth = 0;
            this.dSkinButton_Cancel.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinButton_Cancel.ButtonBorderWidth = 0;
            this.dSkinButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_Cancel.Font = new System.Drawing.Font("Microsoft YaHei", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinButton_Cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(29)))), ((int)(((byte)(54)))));
            this.dSkinButton_Cancel.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_Cancel.HoverImage = null;
            this.dSkinButton_Cancel.Location = new System.Drawing.Point(811, 507);
            this.dSkinButton_Cancel.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinButton_Cancel.Name = "dSkinButton_Cancel";
            this.dSkinButton_Cancel.NormalImage = null;
            this.dSkinButton_Cancel.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_Cancel.PressedImage = null;
            this.dSkinButton_Cancel.Radius = 0;
            this.dSkinButton_Cancel.ShowButtonBorder = true;
            this.dSkinButton_Cancel.Size = new System.Drawing.Size(160, 67);
            this.dSkinButton_Cancel.TabIndex = 14;
            this.dSkinButton_Cancel.Text = "返回";
            this.dSkinButton_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Cancel.TextPadding = 0;
            this.dSkinButton_Cancel.Visible = false;
            this.dSkinButton_Cancel.Click += new System.EventHandler(this.dSkinButton_Cancel_Click);
            // 
            // dSkinLabel_TrayNo
            // 
            this.dSkinLabel_TrayNo.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);
            this.dSkinLabel_TrayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinLabel_TrayNo.Location = new System.Drawing.Point(38, 262);
            this.dSkinLabel_TrayNo.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinLabel_TrayNo.Name = "dSkinLabel_TrayNo";
            this.dSkinLabel_TrayNo.Size = new System.Drawing.Size(112, 20);
            this.dSkinLabel_TrayNo.TabIndex = 15;
            this.dSkinLabel_TrayNo.Text = "托盘编号：XXXX";
            // 
            // dSkinLabel_OrderNo
            // 
            this.dSkinLabel_OrderNo.AutoSize = false;
            this.dSkinLabel_OrderNo.Font = new System.Drawing.Font("Microsoft YaHei", 8F, System.Drawing.FontStyle.Bold);
            this.dSkinLabel_OrderNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinLabel_OrderNo.Location = new System.Drawing.Point(75, 469);
            this.dSkinLabel_OrderNo.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinLabel_OrderNo.Name = "dSkinLabel_OrderNo";
            this.dSkinLabel_OrderNo.Size = new System.Drawing.Size(196, 35);
            this.dSkinLabel_OrderNo.TabIndex = 16;
            this.dSkinLabel_OrderNo.Text = "订单号：XXXX";
            // 
            // dSkinLabel_Mark
            // 
            this.dSkinLabel_Mark.Font = new System.Drawing.Font("Microsoft YaHei", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinLabel_Mark.ForeColor = System.Drawing.Color.AliceBlue;
            this.dSkinLabel_Mark.Location = new System.Drawing.Point(379, 0);
            this.dSkinLabel_Mark.Name = "dSkinLabel_Mark";
            this.dSkinLabel_Mark.Size = new System.Drawing.Size(134, 81);
            this.dSkinLabel_Mark.TabIndex = 18;
            this.dSkinLabel_Mark.Text = "翻面";
            // 
            // dSkinLabel_FrockId
            // 
            this.dSkinLabel_FrockId.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);
            this.dSkinLabel_FrockId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinLabel_FrockId.Location = new System.Drawing.Point(38, 190);
            this.dSkinLabel_FrockId.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinLabel_FrockId.Name = "dSkinLabel_FrockId";
            this.dSkinLabel_FrockId.Size = new System.Drawing.Size(112, 20);
            this.dSkinLabel_FrockId.TabIndex = 19;
            this.dSkinLabel_FrockId.Text = "工装图号：XXXX";
            // 
            // dSkinLabel_Reject
            // 
            this.dSkinLabel_Reject.Font = new System.Drawing.Font("Microsoft YaHei", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinLabel_Reject.ForeColor = System.Drawing.Color.Red;
            this.dSkinLabel_Reject.Location = new System.Drawing.Point(608, 0);
            this.dSkinLabel_Reject.Name = "dSkinLabel_Reject";
            this.dSkinLabel_Reject.Size = new System.Drawing.Size(192, 81);
            this.dSkinLabel_Reject.TabIndex = 20;
            this.dSkinLabel_Reject.Text = "不合格";
            this.dSkinLabel_Reject.Visible = false;
            // 
            // dSkinLabel1
            // 
            this.dSkinLabel1.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);
            this.dSkinLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinLabel1.Location = new System.Drawing.Point(38, 329);
            this.dSkinLabel1.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinLabel1.Name = "dSkinLabel1";
            this.dSkinLabel1.Size = new System.Drawing.Size(74, 20);
            this.dSkinLabel1.TabIndex = 21;
            this.dSkinLabel1.Text = "产品型号：";
            // 
            // dSkinLabel2
            // 
            this.dSkinLabel2.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);
            this.dSkinLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinLabel2.Location = new System.Drawing.Point(38, 436);
            this.dSkinLabel2.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinLabel2.Name = "dSkinLabel2";
            this.dSkinLabel2.Size = new System.Drawing.Size(60, 20);
            this.dSkinLabel2.TabIndex = 22;
            this.dSkinLabel2.Text = "订单号：";
            // 
            // dSkinLabel4
            // 
            this.dSkinLabel4.Font = new System.Drawing.Font("Microsoft YaHei", 10F, System.Drawing.FontStyle.Bold);
            this.dSkinLabel4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.dSkinLabel4.Location = new System.Drawing.Point(38, 530);
            this.dSkinLabel4.Margin = new System.Windows.Forms.Padding(4);
            this.dSkinLabel4.Name = "dSkinLabel4";
            this.dSkinLabel4.Size = new System.Drawing.Size(74, 20);
            this.dSkinLabel4.TabIndex = 23;
            this.dSkinLabel4.Text = "产品编号：";
            // 
            // dSkinLabel5
            // 
            this.dSkinLabel5.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinLabel5.ForeColor = System.Drawing.Color.DarkOrange;
            this.dSkinLabel5.Location = new System.Drawing.Point(1023, 37);
            this.dSkinLabel5.Name = "dSkinLabel5";
            this.dSkinLabel5.Size = new System.Drawing.Size(2, 39);
            this.dSkinLabel5.TabIndex = 24;
            // 
            // dSkinTextBox_ProduceNo
            // 
            this.dSkinTextBox_ProduceNo.BitmapCache = false;
            this.dSkinTextBox_ProduceNo.Font = new System.Drawing.Font("Microsoft YaHei", 8F);
            this.dSkinTextBox_ProduceNo.Location = new System.Drawing.Point(75, 557);
            this.dSkinTextBox_ProduceNo.Name = "dSkinTextBox_ProduceNo";
            this.dSkinTextBox_ProduceNo.Size = new System.Drawing.Size(196, 22);
            this.dSkinTextBox_ProduceNo.TabIndex = 25;
            this.dSkinTextBox_ProduceNo.TransparencyKey = System.Drawing.Color.Empty;
            this.dSkinTextBox_ProduceNo.WaterFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinTextBox_ProduceNo.WaterText = "";
            this.dSkinTextBox_ProduceNo.WaterTextOffset = new System.Drawing.Point(0, 0);
            // 
            // btnTest
            // 
            this.btnTest.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.btnTest.Borders.AllWidth = 0;
            this.btnTest.Borders.BottomWidth = 0;
            this.btnTest.Borders.LeftWidth = 0;
            this.btnTest.Borders.RightWidth = 0;
            this.btnTest.Borders.TopWidth = 0;
            this.btnTest.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(220)))), ((int)(((byte)(228)))));
            this.btnTest.ButtonBorderWidth = 0;
            this.btnTest.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnTest.Font = new System.Drawing.Font("Microsoft YaHei", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(29)))), ((int)(((byte)(54)))));
            this.btnTest.HoverColor = System.Drawing.Color.Empty;
            this.btnTest.HoverImage = null;
            this.btnTest.Location = new System.Drawing.Point(750, 640);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.NormalImage = null;
            this.btnTest.PressColor = System.Drawing.Color.Empty;
            this.btnTest.PressedImage = null;
            this.btnTest.Radius = 0;
            this.btnTest.ShowButtonBorder = true;
            this.btnTest.Size = new System.Drawing.Size(160, 67);
            this.btnTest.TabIndex = 26;
            this.btnTest.Text = "测试";
            this.btnTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnTest.TextPadding = 0;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblAutoCheck
            // 
            this.lblAutoCheck.Font = new System.Drawing.Font("Microsoft YaHei", 16F, System.Drawing.FontStyle.Bold);
            this.lblAutoCheck.ForeColor = System.Drawing.Color.Red;
            this.lblAutoCheck.Location = new System.Drawing.Point(726, 292);
            this.lblAutoCheck.Name = "lblAutoCheck";
            this.lblAutoCheck.Size = new System.Drawing.Size(293, 31);
            this.lblAutoCheck.TabIndex = 27;
            this.lblAutoCheck.Text = "请在人工装卸完成后完成自检";
            this.lblAutoCheck.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblAutoCheck.Visible = false;
            // 
            // TaskOperateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.CaptionFont = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CaptionShowMode = DSkin.TextShowModes.None;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.lblAutoCheck);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.dSkinTextBox_ProduceNo);
            this.Controls.Add(this.dSkinLabel5);
            this.Controls.Add(this.dSkinLabel4);
            this.Controls.Add(this.dSkinLabel2);
            this.Controls.Add(this.dSkinLabel1);
            this.Controls.Add(this.dSkinLabel_Reject);
            this.Controls.Add(this.dSkinLabel_FrockId);
            this.Controls.Add(this.dSkinLabel_Mark);
            this.Controls.Add(this.dSkinLabel_OrderNo);
            this.Controls.Add(this.dSkinLabel_TrayNo);
            this.Controls.Add(this.dSkinButton_Cancel);
            this.Controls.Add(this.dSkinButton_PutOut);
            this.Controls.Add(this.dSkinButton_InTo);
            this.Controls.Add(this.dSkinPictureBox_Process);
            this.Controls.Add(this.dSkinPictureBox_Produce);
            this.Controls.Add(this.dSkinLabel_ProduceType);
            this.Controls.Add(this.dSkinLabel_Title);
            this.DrawIcon = false;
            this.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MoveMode = DSkin.Forms.MoveModes.None;
            this.Name = "TaskOperateForm";
            this.ShowIcon = false;
            this.ShowSystemButtons = false;
            this.Text = "TaskOperateForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TaskOperateForm_FormClosing);
            this.Load += new System.EventHandler(this.TaskOperateForm_Load);
            this.Shown += new System.EventHandler(this.TaskOperateForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSkin.Controls.DSkinLabel dSkinLabel_Title;
        private DSkin.Controls.DSkinLabel dSkinLabel_ProduceType;
        private DSkin.Controls.DSkinPictureBox dSkinPictureBox_Produce;
        private DSkin.Controls.DSkinPictureBox dSkinPictureBox_Process;
        private DSkin.Controls.DSkinButton dSkinButton_InTo;
        private DSkin.Controls.DSkinButton dSkinButton_PutOut;
        private DSkin.Controls.DSkinButton dSkinButton_Cancel;
        private DSkin.Controls.DSkinLabel dSkinLabel_TrayNo;
        private DSkin.Controls.DSkinLabel dSkinLabel_OrderNo;
        private DSkin.Controls.DSkinLabel dSkinLabel_Mark;
        private DSkin.Controls.DSkinLabel dSkinLabel_FrockId;
        private DSkin.Controls.DSkinLabel dSkinLabel_Reject;
        private DSkin.Controls.DSkinLabel dSkinLabel1;
        private DSkin.Controls.DSkinLabel dSkinLabel2;
        private DSkin.Controls.DSkinLabel dSkinLabel4;
        private DSkin.Controls.DSkinLabel dSkinLabel5;
        private DSkin.Controls.DSkinTextBox dSkinTextBox_ProduceNo;
        private DSkin.Controls.DSkinButton btnTest;
        private DSkin.Controls.DSkinLabel lblAutoCheck;
    }
}