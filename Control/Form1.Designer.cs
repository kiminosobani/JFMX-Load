namespace Control
{
    partial class FormControl
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button_Start = new System.Windows.Forms.Button();
            this.button_TrayIntoCnc = new System.Windows.Forms.Button();
            this.groupBox_TrayToCnc = new System.Windows.Forms.GroupBox();
            this.button_TrayOutCnc = new System.Windows.Forms.Button();
            this.radioButton_Cnc2 = new System.Windows.Forms.RadioButton();
            this.radioButton_Cnc1 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_AgvTarget = new System.Windows.Forms.RadioButton();
            this.button_AgvRun = new System.Windows.Forms.Button();
            this.radioButton_AgvOrigin = new System.Windows.Forms.RadioButton();
            this.button_AgvPos7 = new System.Windows.Forms.Button();
            this.button_AgvPos6 = new System.Windows.Forms.Button();
            this.button_AgvPos5 = new System.Windows.Forms.Button();
            this.button_AgvPos4 = new System.Windows.Forms.Button();
            this.button_AgvPos3 = new System.Windows.Forms.Button();
            this.button_AgvPos2 = new System.Windows.Forms.Button();
            this.button_AgvPos1 = new System.Windows.Forms.Button();
            this.label_AgvTarget = new System.Windows.Forms.Label();
            this.label_AgvOrigin = new System.Windows.Forms.Label();
            this.groupBox_TrayToCnc.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(33, 477);
            this.button_Start.Margin = new System.Windows.Forms.Padding(4);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(237, 60);
            this.button_Start.TabIndex = 0;
            this.button_Start.Text = "开始";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // button_TrayIntoCnc
            // 
            this.button_TrayIntoCnc.Location = new System.Drawing.Point(23, 110);
            this.button_TrayIntoCnc.Margin = new System.Windows.Forms.Padding(4);
            this.button_TrayIntoCnc.Name = "button_TrayIntoCnc";
            this.button_TrayIntoCnc.Size = new System.Drawing.Size(169, 70);
            this.button_TrayIntoCnc.TabIndex = 3;
            this.button_TrayIntoCnc.Text = "进机床";
            this.button_TrayIntoCnc.UseVisualStyleBackColor = true;
            this.button_TrayIntoCnc.Click += new System.EventHandler(this.Button_TrayIntoCnc_Click);
            // 
            // groupBox_TrayToCnc
            // 
            this.groupBox_TrayToCnc.Controls.Add(this.button_TrayOutCnc);
            this.groupBox_TrayToCnc.Controls.Add(this.radioButton_Cnc2);
            this.groupBox_TrayToCnc.Controls.Add(this.radioButton_Cnc1);
            this.groupBox_TrayToCnc.Controls.Add(this.button_TrayIntoCnc);
            this.groupBox_TrayToCnc.Location = new System.Drawing.Point(33, 591);
            this.groupBox_TrayToCnc.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox_TrayToCnc.Name = "groupBox_TrayToCnc";
            this.groupBox_TrayToCnc.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox_TrayToCnc.Size = new System.Drawing.Size(489, 228);
            this.groupBox_TrayToCnc.TabIndex = 4;
            this.groupBox_TrayToCnc.TabStop = false;
            this.groupBox_TrayToCnc.Text = "托盘进设备";
            // 
            // button_TrayOutCnc
            // 
            this.button_TrayOutCnc.Location = new System.Drawing.Point(244, 110);
            this.button_TrayOutCnc.Margin = new System.Windows.Forms.Padding(4);
            this.button_TrayOutCnc.Name = "button_TrayOutCnc";
            this.button_TrayOutCnc.Size = new System.Drawing.Size(169, 70);
            this.button_TrayOutCnc.TabIndex = 6;
            this.button_TrayOutCnc.Text = "出机床";
            this.button_TrayOutCnc.UseVisualStyleBackColor = true;
            this.button_TrayOutCnc.Click += new System.EventHandler(this.Button_TrayOutCnc_Click);
            // 
            // radioButton_Cnc2
            // 
            this.radioButton_Cnc2.AutoSize = true;
            this.radioButton_Cnc2.Location = new System.Drawing.Point(264, 51);
            this.radioButton_Cnc2.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_Cnc2.Name = "radioButton_Cnc2";
            this.radioButton_Cnc2.Size = new System.Drawing.Size(93, 28);
            this.radioButton_Cnc2.TabIndex = 5;
            this.radioButton_Cnc2.Text = "2#机床";
            this.radioButton_Cnc2.UseVisualStyleBackColor = true;
            this.radioButton_Cnc2.CheckedChanged += new System.EventHandler(this.RadioButton_Cnc2_CheckedChanged);
            // 
            // radioButton_Cnc1
            // 
            this.radioButton_Cnc1.AutoSize = true;
            this.radioButton_Cnc1.Checked = true;
            this.radioButton_Cnc1.Location = new System.Drawing.Point(48, 51);
            this.radioButton_Cnc1.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_Cnc1.Name = "radioButton_Cnc1";
            this.radioButton_Cnc1.Size = new System.Drawing.Size(93, 28);
            this.radioButton_Cnc1.TabIndex = 4;
            this.radioButton_Cnc1.TabStop = true;
            this.radioButton_Cnc1.Text = "1#机床";
            this.radioButton_Cnc1.UseVisualStyleBackColor = true;
            this.radioButton_Cnc1.CheckedChanged += new System.EventHandler(this.RadioButton_Cnc1_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_AgvTarget);
            this.groupBox1.Controls.Add(this.button_AgvRun);
            this.groupBox1.Controls.Add(this.radioButton_AgvOrigin);
            this.groupBox1.Controls.Add(this.button_AgvPos7);
            this.groupBox1.Controls.Add(this.button_AgvPos6);
            this.groupBox1.Controls.Add(this.button_AgvPos5);
            this.groupBox1.Controls.Add(this.button_AgvPos4);
            this.groupBox1.Controls.Add(this.button_AgvPos3);
            this.groupBox1.Controls.Add(this.button_AgvPos2);
            this.groupBox1.Controls.Add(this.button_AgvPos1);
            this.groupBox1.Controls.Add(this.label_AgvTarget);
            this.groupBox1.Controls.Add(this.label_AgvOrigin);
            this.groupBox1.Location = new System.Drawing.Point(864, 477);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(680, 356);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AGV控制";
            // 
            // radioButton_AgvTarget
            // 
            this.radioButton_AgvTarget.AutoSize = true;
            this.radioButton_AgvTarget.Location = new System.Drawing.Point(28, 95);
            this.radioButton_AgvTarget.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_AgvTarget.Name = "radioButton_AgvTarget";
            this.radioButton_AgvTarget.Size = new System.Drawing.Size(89, 28);
            this.radioButton_AgvTarget.TabIndex = 7;
            this.radioButton_AgvTarget.Text = "终点：";
            this.radioButton_AgvTarget.UseVisualStyleBackColor = true;
            this.radioButton_AgvTarget.CheckedChanged += new System.EventHandler(this.RadioButton_AgvTarget_CheckedChanged);
            // 
            // button_AgvRun
            // 
            this.button_AgvRun.Location = new System.Drawing.Point(510, 296);
            this.button_AgvRun.Name = "button_AgvRun";
            this.button_AgvRun.Size = new System.Drawing.Size(164, 46);
            this.button_AgvRun.TabIndex = 11;
            this.button_AgvRun.Text = "运行";
            this.button_AgvRun.UseVisualStyleBackColor = true;
            this.button_AgvRun.Click += new System.EventHandler(this.Button_AgvRun_Click);
            // 
            // radioButton_AgvOrigin
            // 
            this.radioButton_AgvOrigin.AutoSize = true;
            this.radioButton_AgvOrigin.Checked = true;
            this.radioButton_AgvOrigin.Location = new System.Drawing.Point(28, 54);
            this.radioButton_AgvOrigin.Margin = new System.Windows.Forms.Padding(4);
            this.radioButton_AgvOrigin.Name = "radioButton_AgvOrigin";
            this.radioButton_AgvOrigin.Size = new System.Drawing.Size(89, 28);
            this.radioButton_AgvOrigin.TabIndex = 6;
            this.radioButton_AgvOrigin.TabStop = true;
            this.radioButton_AgvOrigin.Text = "起点：";
            this.radioButton_AgvOrigin.UseVisualStyleBackColor = true;
            this.radioButton_AgvOrigin.CheckedChanged += new System.EventHandler(this.RadioButton_AgvOrigin_CheckedChanged);
            // 
            // button_AgvPos7
            // 
            this.button_AgvPos7.Location = new System.Drawing.Point(321, 227);
            this.button_AgvPos7.Name = "button_AgvPos7";
            this.button_AgvPos7.Size = new System.Drawing.Size(120, 64);
            this.button_AgvPos7.TabIndex = 10;
            this.button_AgvPos7.Text = "2#加工设备缓存位";
            this.button_AgvPos7.UseVisualStyleBackColor = true;
            // 
            // button_AgvPos6
            // 
            this.button_AgvPos6.Location = new System.Drawing.Point(174, 227);
            this.button_AgvPos6.Name = "button_AgvPos6";
            this.button_AgvPos6.Size = new System.Drawing.Size(120, 64);
            this.button_AgvPos6.TabIndex = 9;
            this.button_AgvPos6.Text = "1#加工设备缓存位";
            this.button_AgvPos6.UseVisualStyleBackColor = true;
            // 
            // button_AgvPos5
            // 
            this.button_AgvPos5.Location = new System.Drawing.Point(28, 227);
            this.button_AgvPos5.Name = "button_AgvPos5";
            this.button_AgvPos5.Size = new System.Drawing.Size(120, 64);
            this.button_AgvPos5.TabIndex = 8;
            this.button_AgvPos5.Text = "托盘库缓存位";
            this.button_AgvPos5.UseVisualStyleBackColor = true;
            // 
            // button_AgvPos4
            // 
            this.button_AgvPos4.Location = new System.Drawing.Point(475, 147);
            this.button_AgvPos4.Name = "button_AgvPos4";
            this.button_AgvPos4.Size = new System.Drawing.Size(120, 64);
            this.button_AgvPos4.TabIndex = 7;
            this.button_AgvPos4.Text = "2#成品缓存位";
            this.button_AgvPos4.UseVisualStyleBackColor = true;
            // 
            // button_AgvPos3
            // 
            this.button_AgvPos3.Location = new System.Drawing.Point(321, 147);
            this.button_AgvPos3.Name = "button_AgvPos3";
            this.button_AgvPos3.Size = new System.Drawing.Size(120, 64);
            this.button_AgvPos3.TabIndex = 6;
            this.button_AgvPos3.Text = "2#待加工缓存位";
            this.button_AgvPos3.UseVisualStyleBackColor = true;
            // 
            // button_AgvPos2
            // 
            this.button_AgvPos2.Location = new System.Drawing.Point(174, 147);
            this.button_AgvPos2.Name = "button_AgvPos2";
            this.button_AgvPos2.Size = new System.Drawing.Size(120, 64);
            this.button_AgvPos2.TabIndex = 5;
            this.button_AgvPos2.Text = "1#成品缓存位";
            this.button_AgvPos2.UseVisualStyleBackColor = true;
            // 
            // button_AgvPos1
            // 
            this.button_AgvPos1.Location = new System.Drawing.Point(28, 147);
            this.button_AgvPos1.Name = "button_AgvPos1";
            this.button_AgvPos1.Size = new System.Drawing.Size(120, 64);
            this.button_AgvPos1.TabIndex = 4;
            this.button_AgvPos1.Text = "1#待加工缓存位";
            this.button_AgvPos1.UseVisualStyleBackColor = true;
            // 
            // label_AgvTarget
            // 
            this.label_AgvTarget.AutoSize = true;
            this.label_AgvTarget.Location = new System.Drawing.Point(124, 99);
            this.label_AgvTarget.Name = "label_AgvTarget";
            this.label_AgvTarget.Size = new System.Drawing.Size(46, 24);
            this.label_AgvTarget.TabIndex = 3;
            this.label_AgvTarget.Text = "终点";
            // 
            // label_AgvOrigin
            // 
            this.label_AgvOrigin.AutoSize = true;
            this.label_AgvOrigin.Location = new System.Drawing.Point(124, 56);
            this.label_AgvOrigin.Name = "label_AgvOrigin";
            this.label_AgvOrigin.Size = new System.Drawing.Size(46, 24);
            this.label_AgvOrigin.TabIndex = 2;
            this.label_AgvOrigin.Text = "起点";
            // 
            // FormControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1570, 867);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox_TrayToCnc);
            this.Controls.Add(this.button_Start);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormControl";
            this.Text = "Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormControl_FormClosing);
            this.Load += new System.EventHandler(this.FormControl_Load);
            this.groupBox_TrayToCnc.ResumeLayout(false);
            this.groupBox_TrayToCnc.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_TrayIntoCnc;
        private System.Windows.Forms.GroupBox groupBox_TrayToCnc;
        private System.Windows.Forms.RadioButton radioButton_Cnc2;
        private System.Windows.Forms.RadioButton radioButton_Cnc1;
        private System.Windows.Forms.Button button_TrayOutCnc;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label_AgvTarget;
        private System.Windows.Forms.Label label_AgvOrigin;
        private System.Windows.Forms.Button button_AgvPos7;
        private System.Windows.Forms.Button button_AgvPos6;
        private System.Windows.Forms.Button button_AgvPos5;
        private System.Windows.Forms.Button button_AgvPos4;
        private System.Windows.Forms.Button button_AgvPos3;
        private System.Windows.Forms.Button button_AgvPos2;
        private System.Windows.Forms.Button button_AgvPos1;
        private System.Windows.Forms.Button button_AgvRun;
        private System.Windows.Forms.RadioButton radioButton_AgvTarget;
        private System.Windows.Forms.RadioButton radioButton_AgvOrigin;
    }
}

