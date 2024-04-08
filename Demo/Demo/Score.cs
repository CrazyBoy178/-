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

namespace Demo
{
    public partial class Score : Form
    {
        public Score()
        {
            // 初始化窗体
            InitializeComponent();
            // 设置窗体的边框样式为固定单元格外观
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            // 设置窗体不能最大化
            this.MaximizeBox = false;
            // 设置窗体不能最小化
            this.MinimizeBox = false;
            // 设置窗体启动位置居中
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Score_Load(object sender, EventArgs e)
        {
            // 创建一个数据库操作对象
            Dao dao = new Dao();

            for (int i = 0; i < 8; i++)
            {
                // 构造查询语句
                string sql = $"SELECT MIN(Game_time) FROM Game_Info WHERE Tag = '{i + 1}';";
                // 执行查询，返回 SqlDataReader 对象
                SqlDataReader dc = dao.read(sql);
                // 创建一个新的 Label 对象，用于显示游戏成绩信息
                Label label = new Label();
                // 设置 Label 的自动调整大小属性为 true
                label.AutoSize = true;
                // 设置 Label 的字体为宋体，大小为 12 号字
                label.Font = new Font("宋体", 12);
                // 设置 Label 的位置和大小
                label.Location = new Point(50, 50 + i * 50);

                // 判断查询结果是否存在
                if (dc.Read())
                {
                    // 如果查询结果为空，则显示“null”
                    if (dc[0].ToString() == "")
                    {
                        label.Text = "方格" + (i + 3) + "*" + (i + 3) + "最好成绩:null";
                    }
                    else
                    {
                        // 否则将最短游戏时间显示在 Label 中，单位为秒
                        label.Text = "方格" + (i + 3) + "*" + (i + 3) + "最好成绩:" + dc[0] + "s";
                    }
                }
                // 将 Label 添加到窗体的控件列表中
                this.Controls.Add(label);
            }
            // 查询最新游戏成绩的详细信息，并将其显示在 label1 中
            string sql1 = "SELECT * FROM Game_Info WHERE Time = (SELECT MAX(Time) FROM Game_Info);";
            SqlDataReader dc1 = dao.read(sql1);
            while (dc1.Read())
            {
                label1.Text = "时间:" + dc1["Time"] + "成绩:" + dc1["Game_time"] + "s 方格" + (Convert.ToInt32(dc1["Tag"].ToString()) + 2) + "*" + (Convert.ToInt32(dc1["Tag"].ToString()) + 2);
            }
            // 关闭数据库连接
            dao.DaoClose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // 关闭当前窗体
            this.Close();
        }
    }
}
