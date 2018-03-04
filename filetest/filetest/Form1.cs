using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using System.IO;

namespace filetest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //显示子文件夹
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string str=this.textBox1.Text;
            DirectoryInfo theFolder = new DirectoryInfo(@str);
            DirectoryInfo[] dirInfo = theFolder.GetDirectories();
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in dirInfo)
            {
                this.listBox1.Items.Add(str + '\\' + NextFolder.Name);
            }

        }
        //显示子文件夹及文件
        private void button2_Click(object sender, EventArgs e)
        {
            //int index = listBox1.SelectedIndex;
            if (listBox1.SelectedItems.Count != 1)
            {
                if (MessageBox.Show("未选中文件夹", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, MessageBoxOptions.RtlReading) == DialogResult.OK)
                {
                    return;
                }
            }
            else
            {
                string str = listBox1.SelectedItem.ToString();

                //str =this.textBox1.Text+ '\\' + str;
                //显示所有子文件夹和文件
                listBox1.Items.Clear();

                // MessageBox.Show(str);
                DirectoryInfo theFolder = new DirectoryInfo(@str);
                DirectoryInfo[] dirInfo = theFolder.GetDirectories();
                //遍历文件夹
                foreach (DirectoryInfo NextFolder in dirInfo)
                {
                    this.listBox1.Items.Add(NextFolder.Name);
                    FileInfo[] fileInfo = NextFolder.GetFiles();
                    foreach (FileInfo NextFile in fileInfo)  //遍历文件
                        this.listBox1.Items.Add(str + '\\' + NextFolder.Name);
                }

            }
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

       
    }
}
