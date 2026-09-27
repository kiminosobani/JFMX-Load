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
    public partial class MessageConfirm : DSkinForm
    {
        public MessageConfirm()
        {
            InitializeComponent();
        }

        public enum MessageBoxResult
        {
            Jump = 6,
            LineUp = 1,
            Cancel = 2
        };

        public DialogResult ShowDialog(string sTitle, string sMessage1)
        {
            this.Text = sTitle;
            dSkinLabel1.Text = sMessage1;
            return this.ShowDialog();
        }

        private void MessageConfirm_Load(object sender, EventArgs e)
        {

        }

        private void DSkinButton_LineUp_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;//LineUp = 1
            //this.Close();
        }

        private void DSkinButton_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;//Cancel = 2
            //this.Close();
        }
    }
}
