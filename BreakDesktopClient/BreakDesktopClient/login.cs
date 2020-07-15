using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BreakDesktopClient
{
    public partial class login : Form
    {
        public string user_id;

        private SqlConnection sqlconn = null;
        private string constr = "SERVER= 127.0.0.1,6975; DATABASE=game;" + "UID=sa2; PASSWORD=5142";


        public login()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void login_Load(object sender, EventArgs e)
        {
            try
            {
                sqlconn = new SqlConnection(constr);
                sqlconn.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

  

        private void Join_Click(object sender, EventArgs e)
        {
            String id = textBox1.Text;
            String pass = textBox2.Text;
            String name = textBox3.Text;

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();

                if (id == "" || pass=="" || name=="")
                {
                    MessageBox.Show("모든 정보를 입력해주세요");
                }
                else {
                    try
                    {
                        command.Connection = conn;
                        command.CommandText = String.Format("INSERT INTO Users(USER_ID,NAME,PASSWORD) VALUES('{0}','{1}',{2})", id, name, pass);
                        command.ExecuteNonQuery();
                        Join_Click(null, null);
                    }
                    catch
                    {
                       
                    }
                }
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String id = textBox1.Text;
            String pass = textBox2.Text;

            using (SqlConnection conn = new SqlConnection(constr))
            {

                conn.Open();

                string query = " Select password from users where user_id =" + id;
                SqlDataAdapter sda = new SqlDataAdapter(query, conn);

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Connection = conn;

                if (id == "")
                {
                    MessageBox.Show("아이디를 다시 확인해주세요");
                }
                else
                {

                    cmd.CommandText = "Select password from users where user_id =  '" + id + "'";

                    object scalarValue = cmd.ExecuteScalar();

                    if (scalarValue == null)
                    {
                        MessageBox.Show("아이디를 다시 확인해주세요");
                        
                    }
                    else
                    {
                        String Pwd = scalarValue.ToString().Trim();

                        if (Pwd == pass)
                        {
                            user_id = pass;
                            MessageBox.Show("로그인성공");
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("잘못된 계정입니다.");
                        }

                    }

                }

            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
