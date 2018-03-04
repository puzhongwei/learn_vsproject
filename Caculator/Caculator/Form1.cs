using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Caculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text =textBox1.Text + " " + btn.Text + " ";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text =textBox1.Text + " " + btn.Text + " ";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text += btn.Text;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text =textBox1.Text + " " + btn.Text + " ";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            textBox1.Text = textBox1.Text + " " + btn.Text + " ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button16_Click(object sender, EventArgs e)
        {
            Single r;
            string t = textBox1.Text;
            int space = t.IndexOf(' ');
            string s1 = t.Substring(0, space);
            char op = Convert.ToChar(t.Substring(space + 1, 1));
            string s2=t.Substring(space+3);
            Single arg1 = Convert.ToSingle(s1);
            Single arg2 = Convert.ToSingle(s2);
            switch (op)
            {
                case '+':
                    r = arg1 + arg2;
                    break;
                case '-':
                    r = arg1 - arg2;
                    break;
                case '*':
                    r = arg1 * arg2;
                    break;
                case '/':
                    if (arg2 == 0)
                    {
                        throw new ApplicationException();
                    }
                    else
                    { 
                    r = arg1 / arg2;
                    break;
                    }
                    break;
                default:
                    throw new ApplicationException();
            }
            textBox1.Text = r.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
