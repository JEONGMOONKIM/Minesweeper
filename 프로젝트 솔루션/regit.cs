using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mine_project_1
{
    public partial class regit : Form
    {
        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        List<string> colName = new List<string>();
        public regit()
        {
            InitializeComponent();
        }

        private void bt_login_Click(object sender, EventArgs e)
        {
            if (tb_id.Text == "")
            {
                MessageBox.Show("성명을 입력 하라");
                return;
            }
            if (tb_pass.Text == "")
            {
                MessageBox.Show("암구호를 입력 하라");
                return;
            }
            string sql = $"insert into user_g (id, pass) values ('{tb_id.Text}','{tb_pass.Text}')";
            SqlCommand cmd = new SqlCommand(sql, sqlConn);
            SqlDataReader dr = cmd.ExecuteReader();
            
            dr.Close();
            sqlConn.Close();

            MessageBox.Show("합 류 성 공");

            login lfm = new login();
            this.Hide();
            this.Close();
            lfm.Show();

        }

        private void regit_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=K:\\Data\\[INTEL]AI SW Academy\\project\\mine_project_1\\Properties\\mine.mdf;Integrated Security=True;Connect Timeout=30";
            sqlConn.Open();
            sqlCommand.Connection = sqlConn;
            label6.Text = "DB ON";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            login lfm = new login();
            this.Hide();
            this.Close();
            lfm.ShowDialog();
        }
    }
}
