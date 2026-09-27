namespace LoadStation
{
    partial class ImageShow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dSkinPictureBox1 = new DSkin.Controls.DSkinPictureBox();
            this.dSkinButton_Last = new DSkin.Controls.DSkinButton();
            this.dSkinButton_Next = new DSkin.Controls.DSkinButton();
            this.dSkinButton_Cancel = new DSkin.Controls.DSkinButton();
            this.SuspendLayout();
            // 
            // dSkinPictureBox1
            // 
            this.dSkinPictureBox1.Image = null;
            this.dSkinPictureBox1.Images = null;
            this.dSkinPictureBox1.Location = new System.Drawing.Point(0, 0);
            this.dSkinPictureBox1.Name = "dSkinPictureBox1";
            this.dSkinPictureBox1.Size = new System.Drawing.Size(1920, 1080);
            this.dSkinPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.dSkinPictureBox1.TabIndex = 0;
            this.dSkinPictureBox1.Text = "dSkinPictureBox1";
            // 
            // dSkinButton_Last
            // 
            this.dSkinButton_Last.ButtonBorderWidth = 1;
            this.dSkinButton_Last.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_Last.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_Last.HoverImage = null;
            this.dSkinButton_Last.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Last.Location = new System.Drawing.Point(1833, 68);
            this.dSkinButton_Last.Name = "dSkinButton_Last";
            this.dSkinButton_Last.NormalImage = global::LoadStation.Properties.Resources.Button_Next;
            this.dSkinButton_Last.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_Last.PressedImage = null;
            this.dSkinButton_Last.Radius = 10;
            this.dSkinButton_Last.ShowButtonBorder = false;
            this.dSkinButton_Last.Size = new System.Drawing.Size(80, 80);
            this.dSkinButton_Last.TabIndex = 1;
            this.dSkinButton_Last.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Last.TextPadding = 0;
            this.dSkinButton_Last.Click += new System.EventHandler(this.DSkinButton_Last_Click);
            // 
            // dSkinButton_Next
            // 
            this.dSkinButton_Next.ButtonBorderWidth = 1;
            this.dSkinButton_Next.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_Next.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_Next.HoverImage = null;
            this.dSkinButton_Next.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Next.Location = new System.Drawing.Point(1833, 197);
            this.dSkinButton_Next.Name = "dSkinButton_Next";
            this.dSkinButton_Next.NormalImage = global::LoadStation.Properties.Resources.Button_Previous;
            this.dSkinButton_Next.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_Next.PressedImage = null;
            this.dSkinButton_Next.Radius = 10;
            this.dSkinButton_Next.ShowButtonBorder = false;
            this.dSkinButton_Next.Size = new System.Drawing.Size(80, 80);
            this.dSkinButton_Next.TabIndex = 2;
            this.dSkinButton_Next.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Next.TextPadding = 0;
            this.dSkinButton_Next.Click += new System.EventHandler(this.DSkinButton_Next_Click);
            // 
            // dSkinButton_Cancel
            // 
            this.dSkinButton_Cancel.ButtonBorderWidth = 1;
            this.dSkinButton_Cancel.DialogResult = System.Windows.Forms.DialogResult.None;
            this.dSkinButton_Cancel.HoverColor = System.Drawing.Color.Empty;
            this.dSkinButton_Cancel.HoverImage = null;
            this.dSkinButton_Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Cancel.Location = new System.Drawing.Point(1833, 318);
            this.dSkinButton_Cancel.Name = "dSkinButton_Cancel";
            this.dSkinButton_Cancel.NormalImage = global::LoadStation.Properties.Resources.check_error;
            this.dSkinButton_Cancel.PressColor = System.Drawing.Color.Empty;
            this.dSkinButton_Cancel.PressedImage = null;
            this.dSkinButton_Cancel.Radius = 10;
            this.dSkinButton_Cancel.ShowButtonBorder = false;
            this.dSkinButton_Cancel.Size = new System.Drawing.Size(80, 80);
            this.dSkinButton_Cancel.TabIndex = 3;
            this.dSkinButton_Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.dSkinButton_Cancel.TextPadding = 0;
            this.dSkinButton_Cancel.Click += new System.EventHandler(this.DSkinButton_Cancel_Click);
            // 
            // ImageShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.ControlBox = false;
            this.Controls.Add(this.dSkinButton_Cancel);
            this.Controls.Add(this.dSkinButton_Next);
            this.Controls.Add(this.dSkinButton_Last);
            this.Controls.Add(this.dSkinPictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ImageShow";
            this.ShowIcon = false;
            this.Text = "ImageShow";
            this.Shown += new System.EventHandler(this.ImageShow_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private DSkin.Controls.DSkinPictureBox dSkinPictureBox1;
        private DSkin.Controls.DSkinButton dSkinButton_Last;
        private DSkin.Controls.DSkinButton dSkinButton_Next;
        private DSkin.Controls.DSkinButton dSkinButton_Cancel;
    }
}