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
    public partial class rank : Form
    {
        public rank()
        {
            InitializeComponent();
        }

        SqlConnection sqlConn = new SqlConnection();
        SqlCommand sqlCommand = new SqlCommand();
        List<string> colName = new List<string>();

        private void rank_Load(object sender, EventArgs e)
        {
            textBox1.Text += "ID || RANK \r\n";
            sqlConn.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=K:\\Data\\[INTEL]AI SW Academy\\project\\mine_project_1\\Properties\\mine.mdf;Integrated Security=True;Connect Timeout=30";
            sqlConn.Open();
            sqlCommand.Connection = sqlConn;

            string sql = "select * from rank order by 2";
            sqlCommand.CommandText = sql;
            SqlDataReader sr = sqlCommand.ExecuteReader();
            int m = 0;
            int s = 0;
            for (int i = 0; sr.Read(); i++)
            {
                for (int j = 0; j < sr.FieldCount; j++)
                {
                    if (j % 2 == 0)
                        textBox1.Text += sr.GetValue(j).ToString() + " ";
                    else
                    {
                        int t = Convert.ToInt32(sr.GetValue(j).ToString());
                        m = t / 60;
                        s = t % 60;
                        textBox1.Text += $"{m}분 {s}초";
                    }
                    if (j % 2 == 0) textBox1.Text += "|| ";
                }
                textBox1.Text += "\r\n";
            }
            sr.Close();
        }
    }
}
