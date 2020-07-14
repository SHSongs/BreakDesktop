using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BreakDesktopClient
{
    public partial class login : Form
    {

        
        public login()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

  

        private void Join_Click(object sender, EventArgs e)
        {
            String id = textBox1.Text;
            String pass = textBox2.Text;
            String name = textBox3.Text;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String id = textBox1.Text;
            String pass = textBox2.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
