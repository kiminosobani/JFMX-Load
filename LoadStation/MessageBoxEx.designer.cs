namespace LoadStation
{
    partial class MessageBoxEx
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
            this.dSkinButton_OK = new DSkin.Controls.DSkinButton();
            this.dSkinButton_Cancel = new DSkin.Controls.DSkinButton();
            this.dSkinLabel_big = new DSkin.Controls.DSkinLabel();
            this.dSkinLabel_Small = new DSkin.Controls.DSkinLabel();
            this.SuspendLayout();
            // 
            // dSkinButton_OK
            // 
            this.dSkinButton_OK.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_OK.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_OK.ButtonBorderWidth = 1;
            this.dSkinButton_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.dSkinButton_OK.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinButton_OK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(180)))), ((int)(((byte)(225)))));
            this.dSkinButton_OK.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_OK.HoverImage = null;
            this.dSkinButton_OK.Location = new System.Drawing.Point(278, 246);
            this.dSkinButton_OK.Name = "dSkinButton_OK";
            this.dSkinButton_OK.NormalImage = null;
            this.dSkinButton_OK.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_OK.PressedImage = null;
            this.dSkinButton_OK.Radius = 0;
            this.dSkinButton_OK.ShowButtonBorder = true;
            this.dSkinButton_OK.Size = new System.Drawing.Size(121, 47);
            this.dSkinButton_OK.TabIndex = 0;
            this.dSkinButton_OK.Text = "确认";
            this.dSkinButton_OK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_OK.TextPadding = 0;
            this.dSkinButton_OK.Click += new System.EventHandler(this.DSkinButton_OK_Click);
            // 
            // dSkinButton_Cancel
            // 
            this.dSkinButton_Cancel.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_Cancel.ButtonBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(43)))), ((int)(((byte)(70)))));
            this.dSkinButton_Cancel.ButtonBorderWidth = 1;
            this.dSkinButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.dSkinButton_Cancel.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinButton_Cancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(122)))), ((int)(((byte)(180)))), ((int)(((byte)(225)))));
            this.dSkinButton_Cancel.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_Cancel.HoverImage = null;
            this.dSkinButton_Cancel.Location = new System.Drawing.Point(436, 246);
            this.dSkinButton_Cancel.Name = "dSkinButton_Cancel";
            this.dSkinButton_Cancel.NormalImage = null;
            this.dSkinButton_Cancel.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_Cancel.PressedImage = null;
            this.dSkinButton_Cancel.Radius = 0;
            this.dSkinButton_Cancel.ShowButtonBorder = true;
            this.dSkinButton_Cancel.Size = new System.Drawing.Size(121, 47);
            this.dSkinButton_Cancel.TabIndex = 1;
            this.dSkinButton_Cancel.Text = "取消";
            this.dSkinButton_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Cancel.TextPadding = 0;
            this.dSkinButton_Cancel.Click += new System.EventHandler(this.DSkinButton_Cancel_Click);
            // 
            // dSkinLabel_big
            // 
            this.dSkinLabel_big.AutoSize = false;
            this.dSkinLabel_big.Borders.AllColor = System.Drawing.Color.Transparent;
            this.dSkinLabel_big.Borders.BottomColor = System.Drawing.Color.Transparent;
            this.dSkinLabel_big.Borders.LeftColor = System.Drawing.Color.Transparent;
            this.dSkinLabel_big.Borders.RightColor = System.Drawing.Color.Transparent;
            this.dSkinLabel_big.Borders.TopColor = System.Drawing.Color.Transparent;
            this.dSkinLabel_big.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinLabel_big.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(29)))), ((int)(((byte)(54)))));
            this.dSkinLabel_big.Location = new System.Drawing.Point(26, 60);
            this.dSkinLabel_big.Name = "dSkinLabel_big";
            this.dSkinLabel_big.Size = new System.Drawing.Size(531, 91);
            this.dSkinLabel_big.TabIndex = 4;
            this.dSkinLabel_big.Text = "dSkinLabel1";
            // 
            // dSkinLabel_Small
            // 
            this.dSkinLabel_Small.AutoSize = false;
            this.dSkinLabel_Small.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSkinLabel_Small.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dSkinLabel_Small.Location = new System.Drawing.Point(26, 157);
            this.dSkinLabel_Small.Name = "dSkinLabel_Small";
            this.dSkinLabel_Small.Size = new System.Drawing.Size(531, 73);
            this.dSkinLabel_Small.TabIndex = 5;
            this.dSkinLabel_Small.Text = "dSkinLabel1";
            // 
            // MessageBoxEx
            // 
            this.AcceptButton = this.dSkinButton_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(176)))), ((int)(((byte)(178)))));
            this.CancelButton = this.dSkinButton_Cancel;
            this.CaptionFont = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CaptionHeight = 40;
            this.CaptionShowMode = DSkin.TextShowModes.Ordinary;
            this.ClientSize = new System.Drawing.Size(600, 300);
            this.ControlBox = false;
            this.Controls.Add(this.dSkinLabel_Small);
            this.Controls.Add(this.dSkinLabel_big);
            this.Controls.Add(this.dSkinButton_Cancel);
            this.Controls.Add(this.dSkinButton_OK);
            this.DrawIcon = false;
            this.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.Name = "MessageBoxEx";
            this.Text = "MessageBoxEx";
            this.ResumeLayout(false);

        }

        #endregion

        private DSkin.Controls.DSkinButton dSkinButton_OK;
        private DSkin.Controls.DSkinButton dSkinButton_Cancel;
        private DSkin.Controls.DSkinLabel dSkinLabel_big;
        private DSkin.Controls.DSkinLabel dSkinLabel_Small;
    }
}