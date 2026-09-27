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
    public partial class EquipSelectForm : DSkinForm
    {
        public string Position { get; set; }
        public EquipSelectForm()
        {
            InitializeComponent();
        }

        private void EquipSelectForm_Load(object sender, EventArgs e)
        {
            this.dSkinButton3.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton4.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton6.Click += new System.EventHandler(this.ButtonSelect_Click);
            this.dSkinButton10.Click += new System.EventHandler(this.ButtonSelect_Click);
        }
        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            DSkin.Controls.DSkinButton SelectButton = (DSkin.Controls.DSkinButton)sender;

            dSkinLabel_Value1.Text = SelectButton.Text;
            Position = SelectButton.Text;


        }
        private void DSkinButton_Sure_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
