using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class Game : Form
    {
        //引入winmm.dll动态链接库中的timeGetTime函数
        [DllImport("winmm.dll")]
        public static extern uint timeGetTime();
        private bool flag1;//标记是否为消除模式
        private int num; //记录方阵边长
        private double time = 0;//记录用时
        private int count = 0;//记录点击次数
        private uint startTime;//记录开始时间
        Timer timer = new Timer();//计时器
        private int btn_count = 0;//记录按钮是不是第一次点击

        //设置计时器初始值
        void setTimer()
        {

            startTime = timeGetTime();

            timer.Interval = 1;


            timer.Tick += new EventHandler(timer_Tick);
        }

        Random r = new Random();

        TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();


        public Game(int n, bool f)
        {
            InitializeComponent();
            this.num = n;
            this.flag1 = f;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }


        private void 重新开始ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            tableLayoutPanel.Controls.Clear();

            timer.Stop();
            this.count = 0;
            this.time = 0;
            this.btn_count = 0;
            setTimer();
            label1.Text = "时间:0.000s";
            add_button(num);
        }

        private void Game_Load(object sender, EventArgs e)
        {
            label1.Text = "时间:0.000s";
            add_button(num);
            MessageBox.Show("点击按钮即可开始游戏");
        }

        void timer_Tick(object sender, EventArgs e)
        {
            uint currentTime = timeGetTime();
            this.time = (currentTime - startTime) / 1000.0;
            label1.Text = "时间:" + time.ToString("F3") + "s";

        }


        void add_button(int num)
        {

            tableLayoutPanel.Size = new Size(480, 480);
            tableLayoutPanel.Location = new Point(28, 55);
            tableLayoutPanel.ColumnCount = num;
            tableLayoutPanel.RowCount = num;
            List<int> list = new List<int>();

            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel.ColumnCount; j++)
                {
                    int n = r.Next(num * num) + 1;
                    while (list.Contains(n))
                    {
                        n = r.Next(num * num) + 1;
                    }
                    list.Add(n);
                    Button button = new Button();
                    button.Dock = DockStyle.Fill;
                    button.Text = $"{n}";
                    button.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));//设置字体样式
                    button.Click += new EventHandler(button_Click);
                    tableLayoutPanel.Controls.Add(button, j, i);
                }
            }
            tableLayoutPanel.ColumnStyles.Clear();
            for (int i = 0; i < tableLayoutPanel.ColumnCount; i++)
            {
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / tableLayoutPanel.ColumnCount));
            }

            tableLayoutPanel.RowStyles.Clear();
            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / tableLayoutPanel.RowCount));
            }
            this.Controls.Add(tableLayoutPanel);

        }

        void button_Click(object sender, EventArgs e)
        {
            //如果按钮没点击过
            if (btn_count == 0)
            {
                setTimer();
                timer.Start();//开始计时
            }
            btn_count = 1;//标记按钮为点击过为

            //将事件的发送者转换为Button类型
            Button button = sender as Button;
            if (button != null && button.Text == (count + 1).ToString())
            {
                count++;
                if (flag1)
                {
                    button.Dispose();
                }
                else
                {
                    button.Enabled = false;
                }

                if (count == num * num)
                {
                    timer.Stop();
                    MessageBox.Show($"游戏胜利！时间:{time}");
                    Dao dao = new Dao();
                    string sql = "insert into Game_Info(Time, Game_time, Tag) values('" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + time + "','" + (num - 2) + "');";//构造SQL语句
                    string sql1 = $"SELECT MIN(Game_Time) FROM Game_Info where Tag = {num - 2};";
                    int n = dao.Execute(sql);
                    SqlDataReader dc = dao.read(sql1);
                    if (n > 0)
                    {
                        MessageBox.Show("成绩保存成功");
                        while (dc.Read())
                        {
                            MessageBox.Show($"当前方格最好成绩:{dc[0]}s");
                        }
                    }
                    else
                    {
                        MessageBox.Show("成绩保存失败");

                    }
                    dc.Close();
                    dao.DaoClose();
                }
            }
            else
            {
                MessageBox.Show("请按顺序点击按钮！");
            }
        }

        private void 查看成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();

            this.Hide();
            Score s = new Score();
            s.ShowDialog();
            if (btn_count == 1)
            {
                timer.Start();
            }

            this.Show();
        }

        private void 查询历史成绩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Stop();

            this.Hide();
            History h = new History();
            h.ShowDialog();
            if (btn_count == 1)
            {
                timer.Start();
            }

            this.Show();
        }
    }
}
