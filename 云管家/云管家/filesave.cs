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
    public partial class filesavedlg : Form
    {
        public filesavedlg()
        {
            InitializeComponent();
        }
        public filesavedlg(string filepath)
        {
            InitializeComponent();
            textBox1.Text = filepath;
        }
        public string filename="";
        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
              {
                 string name = sfd.FileName;// 获得文件保存的路径；
                
                //string fileSavePath=name.Substring(0,name.LastIndexOf("\\"));
                textBox1.Text =name;
                filename = textBox1.Text;
               
              }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
