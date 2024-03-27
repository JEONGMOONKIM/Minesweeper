using mine_project_1.Properties;
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
using System.IO.Ports;

namespace mine_project_1
{
    public partial class game : Form
    {
        rank kfm = new rank();
        public class DoubleClickButton : Button
        {
            public DoubleClickButton() : base()
            {
                // Set the style so a double click event occurs.
                SetStyle(ControlStyles.StandardClick |
                    ControlStyles.StandardDoubleClick, true);
            }
        }
        public game()
        {
            InitializeComponent();
            Width = 400;
            Height = 550;
            panel1.Top = 100;
            panel1.Left = 0;
            panel1.Width = Width;
            panel1.Height = Height;
            
            CreatePos();
            ButtonCreate();
        }

        private void game_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }



        //선언


    
        int[,] array = new int[10, 10];
        int[,] Pos = new int[10, 10];
        int[,] Visit = new int[10, 10];
        int[,] Cnt = new int[10, 10];
        DoubleClickButton[,] btnarr = new DoubleClickButton[10, 10];
        bool GameOver;
        private void CreatePos()
        {
            //throw new NotImplementedException();
            int i, j;
            Random r = new Random();
            r.Next();
            for (i = 0; i < 10; i++)
            {
                int y = r.Next(1, 10);
                int x = r.Next(1, 10);
                if (Pos[y, x] == 1)
                {
                    i--;
                    continue;
                }
                Pos[y, x] = 1;
            }
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if (i > 0) Cnt[i, j] += Pos[i - 1, j];
                    if (i < 9) Cnt[i, j] += Pos[i + 1, j];
                    if (j > 0) Cnt[i, j] += Pos[i, j - 1];
                    if (j < 9) Cnt[i, j] += Pos[i, j + 1];
                    if (i > 0 && j > 0) Cnt[i, j] += Pos[i - 1, j - 1];
                    if (i > 0 && j < 9) Cnt[i, j] += Pos[i - 1, j + 1];
                    if (i < 9 && j > 0) Cnt[i, j] += Pos[i + 1, j - 1];
                    if (i < 9 && j < 9) Cnt[i, j] += Pos[i + 1, j + 1];
                    if (Pos[i, j] == 1) Cnt[i, j] = -1;
                }
            }
        }

        private void ButtonCreate()
        {
            int i, j;
            int num = 1;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    //private DoubleClickButton button1;
                    DoubleClickButton btn = new DoubleClickButton();
                    btn.Width = 40;
                    btn.Height = 40;
                    btn.Top = i * 40;
                    btn.Left = j * 40;
                    btn.Tag = num;
                    btn.BackColor = Color.Black;// button1.BackColor;
                    btn.BackgroundImage = Properties.Resources.pool;
                    array[i, j] = num;
                    //btn.Text = Cnt[i, j].ToString();
                    btn.Click += new EventHandler(btn_Click);
                    //this.AllowDrop = true;
                    btn.DoubleClick += new EventHandler(btn_DoubleClick);
                    btn.MouseDown += new MouseEventHandler(btn_MouseDown);
                    num++;
                    btnarr[i, j] = btn;
                    panel1.Controls.Add(btn);

                }
            }
        }

        private void btn_DoubleClick(object sender, EventArgs e)
        {
            // throw new NotImplementedException();
            //this.FormBorderStyle = initialStyle;
            if (GameOver) return;


            int x, y;
            int res = CheckVisit((sender as Button).Tag, out x, out y);
            if (res == -1)
            {
                GameOver = true;
                timer1.Stop();
                MessageBox.Show("게임오버");
                Sendmsg("RES 1");
            }
            else if (res == 1) return;

            OpenButton(x, y);
            if (GameCheck())
            {
                timer1.Stop();
                string id = label1.Text;
                int rank = progressBar1.Value;
                Sendmsg("RES 2");
                if (MessageBox.Show($"지뢰 제거 성공\n랭킹에 등록 하냐?\n아이디 = {id}, 점수 = {rank}", "ㅊㅋㅊㅋ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    SqlConnection sqlConn = new SqlConnection();
                    SqlCommand sqlCommand = new SqlCommand();
                    List<string> colName = new List<string>();

                    sqlConn.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=K:\\Data\\[INTEL]AI SW Academy\\project\\mine_project_1\\Properties\\mine.mdf;Integrated Security=True;Connect Timeout=30";
                    sqlConn.Open();
                    sqlCommand.Connection = sqlConn;

                    string sql = $"insert into rank (id, time) values ('{id}','{300-rank}')";
                    SqlCommand cmd = new SqlCommand(sql, sqlConn);
                    SqlDataReader dr = cmd.ExecuteReader();

                    dr.Close();
                    sqlConn.Close();

                    MessageBox.Show("성공");
                }
            }


        }

        private bool GameCheck()
        {
            int cnt = 0;
            int i, j;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if (Visit[i, j] == 2) cnt++;
                    else if (Visit[i, j] == 0) return false;
                }
            }
            if (cnt == 10) return true;
            else return false;
        }

        private void OpenButton(int x, int y)
        {
            //throw new NotImplementedException();
            if (x < 0 || y < 0 || x > 9 || y > 9) return;
            if (Visit[y, x] != 0) return;
            Visit[y, x] = 1;
            btnarr[y, x].BackColor = Color.White;
            btnarr[y, x].BackgroundImage = Properties.Resources.pool;
            if (Cnt[y, x].ToString() == "1") btnarr[y, x].BackgroundImage = Properties.Resources._1;
            else if (Cnt[y, x].ToString() == "2") btnarr[y, x].BackgroundImage = Properties.Resources._2;
            else if (Cnt[y, x].ToString() == "3") btnarr[y, x].BackgroundImage = Properties.Resources._3;
            else if (Cnt[y, x].ToString() == "4") btnarr[y, x].BackgroundImage = Properties.Resources._4;
            else if (Cnt[y, x].ToString() == "0") btnarr[y, x].BackgroundImage = Properties.Resources.he;
            else if (Cnt[y, x].ToString() == "-1") btnarr[y, x].BackgroundImage = Properties.Resources.boom;
            else btnarr[y, x].BackgroundImage = Properties.Resources.he;
            //btnarr[y, x].Text = Cnt[y, x].ToString();
            if (Cnt[y, x] > 0) return;
            OpenButton(x + 1, y);
            OpenButton(x - 1, y);
            OpenButton(x, y + 1);
            OpenButton(x, y - 1);
            OpenButton(x - 1, y - 1);
            OpenButton(x - 1, y + 1);
            OpenButton(x + 1, y - 1);
            OpenButton(x + 1, y + 1);
        }

        private int CheckVisit(object tag, out int x, out int y)
        {
            int num = int.Parse(tag.ToString());
            int i, j;
            y = -1;
            x = -1;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if (array[i, j] == num)
                    {
                        y = i;
                        x = j;
                        if (Cnt[i, j] == -1) return -1;
                        if (Visit[i, j] == 0) return 0;
                        else return 1;
                    }
                }
            }
            return 1;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            //this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Top = 0;
            panel1.Left = 0;
            panel1.Width = Width;
            panel1.Height = Height;
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            if (GameOver) return;
            //
            if (e.Button == MouseButtons.Right)
            {
                //MessageBox.Show((sender as Button).Tag + "번 우클릭");
                if ((sender as Button).BackColor == Color.Red)
                {
                    (sender as Button).BackColor = Color.Black;
                    (sender as Button).BackgroundImage = Properties.Resources.pool;
                }
                else
                {
                    (sender as Button).BackColor = Color.Red;
                    (sender as Button).BackgroundImage = Properties.Resources.flag;
                }
                RightClick((sender as Button).Tag);
            }
        }

        private void RightClick(object tag)
        {
            int num = int.Parse(tag.ToString());
            int i, j;
            for (i = 0; i < 10; i++)
            {
                for (j = 0; j < 10; j++)
                {
                    if (array[i, j] == num)
                    {
                        if (Visit[i, j] == 2) Visit[i, j] = 0;
                        else Visit[i, j] = 2;
                        return;
                    }
                }
            }
            if (GameCheck())
            {
                timer1.Stop();
                int rank = 300 - progressBar1.Value;
                if (MessageBox.Show($"지뢰 제거 성공\n랭킹에 등록 하냐?\n점수 = {rank}", "ㅊㅋㅊㅋ", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show("성공");
                }
                
                
            }
            //MessageBox.Show(num.ToString());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            login lfm = new login();
            kfm.Close();
            this.Hide();
            this.Close(); 
            lfm.ShowDialog();
           
        }

        private void game_Load(object sender, EventArgs e)
        {
            serialPort1.Parity = (Parity)0;
            serialPort1.DataBits = 8;
            serialPort1.StopBits = (StopBits)1;
            serialPort1.BaudRate = 115200;

            string[] Port = System.IO.Ports.SerialPort.GetPortNames();

            serialPort1.PortName = Port[2];

            string info_con = $"{serialPort1.PortName} : {serialPort1.BaudRate} {serialPort1.Parity} {serialPort1.DataBits} {serialPort1.StopBits}";

            serialPort1.Open();

            if (serialPort1.IsOpen)
            {
                MessageBox.Show($"{serialPort1.PortName} OPEN");
            }

            kfm.Show();
            timer1.Start();
        }
        private void Sendmsg(string text)
        {
            text = $"\x02{text}\x03";
            serialPort1.Write(text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 1)
            {
                GameOver = true;
                timer1.Stop();
                MessageBox.Show("타임 오바");
            }
            progressBar1.Value --;
        }


    }
}
