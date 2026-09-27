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
    public partial class NumberSelectForm : DSkinForm
    {
        private bool bolFigure = true;

        public int ITary { get; set; } = 0;

        public NumberSelectForm()
        {
            InitializeComponent();
        }

        private void DSkinButton10_Click(object sender, EventArgs e)
        {
            int iTary = Convert.ToInt32(dSkinLabel_Value1.Text);
            if (iTary > 16 || iTary == 0)
            {
            }
            else
            {
                ITary = iTary;
            }
        }

        private void DSkinButton11_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void NumberSelectForm_Load(object sender, EventArgs e)
        {
            this.dSkinButton0.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton1.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton2.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton3.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton4.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton5.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton6.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton7.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton8.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton9.Click += new System.EventHandler(this.ButtonSelect_Click);
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            DSkin.Controls.DSkinButton SelectButton = (DSkin.Controls.DSkinButton)sender;
            string ButtonName = SelectButton.Name;
            dSkinLabel_Value1.Text = (Convert.ToInt32(dSkinLabel_Value1.Text) * 10
                + Convert.ToInt32(ButtonName.Replace("dSkinButton", ""))).ToString();
            if (Convert.ToInt32(dSkinLabel_Value1.Text) > 16)
            {
                dSkinLabel_Value1.Text = (Convert.ToInt32(dSkinLabel_Value1.Text) % 10).ToString();
            }
        }
    }
}