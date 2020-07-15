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

namespace GameManagement
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlconn = null;
        private string constr = "SERVER= 127.0.0.1,6975; DATABASE=game;" + "UID=sa2; PASSWORD=5142";


        public Form1()
        {
            InitializeComponent();

            Create();
        }

        private void Create()
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

        private void button2_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                string sql = "SELECT * FROM Logs";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.Fill(ds, "Logs");
            }

            dataGridView1.DataSource = ds.Tables[0];

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                string sql = "SELECT * FROM Users";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
                adapter.Fill(ds, "Users");
            }

            dataGridView1.DataSource = ds.Tables[0];


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells["USER_ID"].Value.ToString();
            textBox2.Text = dataGridView1.Rows[e.RowIndex].Cells["NAME"].Value.ToString();
            textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells["PASSWORD"].Value.ToString();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            String bookno = textBox1.Text;


            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = conn;
                command.CommandText = "DELETE FROM BOOKS WHERE USER_ID = " + bookno;
                command.ExecuteNonQuery();
            }

            DataSet ds = new DataSet();

            button2_Click(null, null);
        }
    }
}
