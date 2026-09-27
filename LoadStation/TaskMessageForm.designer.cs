namespace LoadStation
{
    partial class TaskMessageForm
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
            this.dSkinLabel1 = new DSkin.Controls.DSkinLabel();
            this.dSkinButton_Jump = new DSkin.Controls.DSkinButton();
            this.dSkinButton_LineUp = new DSkin.Controls.DSkinButton();
            this.dSkinButton_Cancel = new DSkin.Controls.DSkinButton();
            this.SuspendLayout();
            // 
            // dSkinLabel1
            // 
            this.dSkinLabel1.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinLabel1.Location = new System.Drawing.Point(82, 79);
            this.dSkinLabel1.Name = "dSkinLabel1";
            this.dSkinLabel1.Size = new System.Drawing.Size(426, 34);
            this.dSkinLabel1.TabIndex = 0;
            this.dSkinLabel1.Text = "当前穿梭车共有X项任务进行中，请确认";
            // 
            // dSkinButton_Jump
            // 
            this.dSkinButton_Jump.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_Jump.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_Jump.ButtonBorderWidth = 1;
            this.dSkinButton_Jump.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_Jump.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinButton_Jump.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(180)))), ((int)(((byte)(225)))));
            this.dSkinButton_Jump.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_Jump.HoverImage = null;
            this.dSkinButton_Jump.Location = new System.Drawing.Point(73, 251);
            this.dSkinButton_Jump.Name = "dSkinButton_Jump";
            this.dSkinButton_Jump.NormalImage = null;
            this.dSkinButton_Jump.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_Jump.PressedImage = null;
            this.dSkinButton_Jump.Radius = 10;
            this.dSkinButton_Jump.ShowButtonBorder = true;
            this.dSkinButton_Jump.Size = new System.Drawing.Size(100, 50);
            this.dSkinButton_Jump.TabIndex = 1;
            this.dSkinButton_Jump.Text = "插队";
            this.dSkinButton_Jump.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Jump.TextPadding = 0;
            this.dSkinButton_Jump.Click += new System.EventHandler(this.DSkinButton_Jump_Click);
            // 
            // dSkinButton_LineUp
            // 
            this.dSkinButton_LineUp.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_LineUp.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_LineUp.ButtonBorderWidth = 1;
            this.dSkinButton_LineUp.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_LineUp.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinButton_LineUp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(180)))), ((int)(((byte)(225)))));
            this.dSkinButton_LineUp.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_LineUp.HoverImage = null;
            this.dSkinButton_LineUp.Location = new System.Drawing.Point(237, 251);
            this.dSkinButton_LineUp.Name = "dSkinButton_LineUp";
            this.dSkinButton_LineUp.NormalImage = null;
            this.dSkinButton_LineUp.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_LineUp.PressedImage = null;
            this.dSkinButton_LineUp.Radius = 10;
            this.dSkinButton_LineUp.ShowButtonBorder = true;
            this.dSkinButton_LineUp.Size = new System.Drawing.Size(100, 50);
            this.dSkinButton_LineUp.TabIndex = 2;
            this.dSkinButton_LineUp.Text = "排队";
            this.dSkinButton_LineUp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_LineUp.TextPadding = 0;
            this.dSkinButton_LineUp.Click += new System.EventHandler(this.DSkinButton_LineUp_Click);
            // 
            // dSkinButton_Cancel
            // 
            this.dSkinButton_Cancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_Cancel.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_Cancel.ButtonBorderWidth = 1;
            this.dSkinButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_Cancel.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinButton_Cancel.ForeColor = System.Drawing.Color.White;
            this.dSkinButton_Cancel.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_Cancel.HoverImage = null;
            this.dSkinButton_Cancel.Location = new System.Drawing.Point(447, 251);
            this.dSkinButton_Cancel.Name = "dSkinButton_Cancel";
            this.dSkinButton_Cancel.NormalImage = null;
            this.dSkinButton_Cancel.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_Cancel.PressedImage = null;
            this.dSkinButton_Cancel.Radius = 10;
            this.dSkinButton_Cancel.ShowButtonBorder = true;
            this.dSkinButton_Cancel.Size = new System.Drawing.Size(100, 50);
            this.dSkinButton_Cancel.TabIndex = 3;
            this.dSkinButton_Cancel.Text = "取消";
            this.dSkinButton_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Cancel.TextPadding = 0;
            this.dSkinButton_Cancel.Click += new System.EventHandler(this.DSkinButton_Cancel_Click);
            // 
            // TaskMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(176)))), ((int)(((byte)(178)))));
            this.CaptionBackColors = new System.Drawing.Color[] {
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(162)))), ((int)(((byte)(0)))))};
            this.CaptionColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(29)))), ((int)(((byte)(54)))));
            this.CaptionFont = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold);
            this.CaptionHeight = 35;
            this.CaptionOffset = new System.Drawing.Point(0, -5);
            this.ClientSize = new System.Drawing.Size(582, 353);
            this.ControlBox = false;
            this.Controls.Add(this.dSkinButton_Cancel);
            this.Controls.Add(this.dSkinButton_LineUp);
            this.Controls.Add(this.dSkinButton_Jump);
            this.Controls.Add(this.dSkinLabel1);
            this.DrawIcon = false;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskMessageForm";
            this.Text = "提示";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DSkin.Controls.DSkinLabel dSkinLabel1;
        private DSkin.Controls.DSkinButton dSkinButton_Jump;
        private DSkin.Controls.DSkinButton dSkinButton_LineUp;
        private DSkin.Controls.DSkinButton dSkinButton_Cancel;
    }
}