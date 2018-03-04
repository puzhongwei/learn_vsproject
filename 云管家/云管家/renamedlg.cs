using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace 云管家
{
    public partial class renamedlg : Form
    {
        public string name;
        public renamedlg()
        {
            InitializeComponent();
        }
        public renamedlg(string fname)
        {
            InitializeComponent();
            name = fname;
            textBox1.Text = name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
