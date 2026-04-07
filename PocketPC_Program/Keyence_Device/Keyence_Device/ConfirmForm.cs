using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.WindowsCE.Forms;
namespace Keyence_Device
{
    public partial class ConfirmForm : Form
    {
        public ConfirmForm(string main, string notify)
        {
            InitializeComponent();
            this.Text = main;
            lab_notify.Text = notify;
        }

        private void btn_no_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_yes_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}