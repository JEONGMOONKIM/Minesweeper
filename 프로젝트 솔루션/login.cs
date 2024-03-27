using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mine_project_1
{
    public partial class login : Form
    {
        game gfm = new game();
        regit rfm = new regit();
        
        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        List<string> colName = new List<string>();
        public login()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            sqlConn.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=K:\\Data\\[INTEL]AI SW Academy\\project\\mine_project_1\\Properties\\mine.mdf;Integrated Security=True;Connect Timeout=30";
            sqlConn.Open();
            sqlCommand.Connection = sqlConn;
            label6.Text = "DB ON";
        }


        private void bt_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void bt_login_Click(object sender, EventArgs e)
        {
            
            

            bool db_id = false;
            bool db_pass = false;
            
            string sql = "select * from user_g";
            SqlCommand cmd = new SqlCommand(sql, sqlConn);
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["id"].ToString() == tb_id.Text)
                {
                    db_id = true;
                    if (dr["pass"].ToString() == tb_pass.Text)
                    {
                        db_pass = true;
                    }
                }
            }

            dr.Close();

            if (db_id == false)
            {
                MessageBox.Show("존재 하지 않는 성 명 이 다 !");
            }
            else if(db_id == true && db_pass == false)
            {
                MessageBox.Show("틀 린 암 구 호 !");
            }
            else if(db_id == true && db_pass == true)
            {
                MessageBox.Show("립 장 성 공");
                gfm.Show();
                gfm.label1.Text = tb_id.Text;
                this.Hide();
                
            }
        }

        public void login_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqlConn.Close();
        }

        public void bt_regit_Click_1(object sender, EventArgs e)
        {
            sqlConn.Close();
            rfm.Show();
        }
    }
}
