using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSkin.Forms;

namespace LoadStation
{
    public partial class MessageBoxEx : DSkinForm
    {
        public enum MessageBoxStyle
        {
            normal = 0,
            warning = 1,
            error = 2
        };
        /**********************变量******************************/
        System.Drawing.Color[] NormalColor = new System.Drawing.Color[] {
    System.Drawing.Color.LimeGreen,
    System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))))};

        System.Drawing.Color[] WarningColor = new System.Drawing.Color[] {
    System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0))))),
    System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))))};

        System.Drawing.Color[] ErrorColor = new System.Drawing.Color[] {
    System.Drawing.Color.Red,
    System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))))};
        /****************************************************/
        public MessageBoxEx()
        {
            InitializeComponent();
        }
        /// <summary>
        /// ShowDialog
        /// </summary>
        /// <param name="sTitle">messagebox标题</param>
        /// <param name="sMessage1">messagebox内容-大</param>
        /// <param name="sMessage2">messagebox内容-小</param>
        /// <param name="sType">messagebox图标，默认为warn</param>
        public DialogResult ShowDialog(string sTitle, string sMessage1, string sMessage2, MessageBoxStyle eStyle)
        {
            //string sIcon;
            switch (eStyle)
            {
                case MessageBoxStyle.normal:
                    //sIcon = @"../../res/BtnRes/check-ok.png";
                    this.CaptionBackColors = NormalColor;
                    break;
                case MessageBoxStyle.warning:
                    //sIcon = @"../../res/BtnRes/check-warn.png";
                    this.CaptionBackColors = WarningColor;
                    break;
                default:
                    //sIcon = @"../../res/BtnRes/check-error.png";
                    this.CaptionBackColors = ErrorColor;
                    break;
            }

            //using (FileStream image = new FileStream(sIcon, FileMode.Open))
            //{
            //    dSkinPictureBox_Icon.Image = Image.FromStream(image);
            //}

            this.Text = sTitle;
            dSkinLabel_big.Text = sMessage1;
            dSkinLabel_Small.Text = sMessage2;
            return this.ShowDialog();
        }

        public DialogResult ShowDialog1(string sTitle, string sMessage1, string sMessage2, MessageBoxStyle eStyle)
        {
            //string sIcon;
            switch (eStyle)
            {
                case MessageBoxStyle.normal:
                    //sIcon = @"../../res/BtnRes/check-ok.png";
                    this.CaptionBackColors = NormalColor;
                    break;
                case MessageBoxStyle.warning:
                    //sIcon = @"../../res/BtnRes/check-warn.png";
                    this.CaptionBackColors = WarningColor;
                    break;
                default:
                    //sIcon = @"../../res/BtnRes/check-error.png";
                    this.CaptionBackColors = ErrorColor;
                    break;
            }

            //using (FileStream image = new FileStream(sIcon, FileMode.Open))
            //{
            //    dSkinPictureBox_Icon.Image = Image.FromStream(image);
            //}

            this.Text = sTitle;
            dSkinLabel_big.Text = sMessage1;
            dSkinLabel_Small.Text = sMessage2;

            dSkinButton_Cancel.Visible = false;


            return this.ShowDialog();
        }

        private void DSkinButton_OK_Click(object sender, EventArgs e)
        {

        }

        private void DSkinButton_Cancel_Click(object sender, EventArgs e)
        {

        }
    }
}
