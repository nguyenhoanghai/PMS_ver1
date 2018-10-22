using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuAn03_HaiDang
{
    public partial class MessageBox_Show : Form
    {
        private string information="";
        public string Information { set{this.information = value; }}
        public MessageBox_Show()
        {
            InitializeComponent();
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MessageBox_Show_Load(object sender, EventArgs e)
        {
            label3.Text =information;
        }
        
    }
}
