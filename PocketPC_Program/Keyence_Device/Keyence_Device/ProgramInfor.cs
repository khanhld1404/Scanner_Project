using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Keyence_Device
{
    public partial class ProgramInfor : Form
    {
        public ProgramInfor()
        {
            InitializeComponent();
        }

        private void Btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}