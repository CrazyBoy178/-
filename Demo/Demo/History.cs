using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    public partial class History : Form
    {

        public History()
        {
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

        // Table 方法用于从数据库中获取数据，并在 DataGridView 中显示
        public void Table(int n)
        {
            dataGridView1.Rows.Clear();//清空旧数据
            // 创建一个数据库操作对象
            Dao dao = new Dao();
            // 构造查询语句
            string sql = $"select * from Game_Info where Tag = {n}";
            // 执行查询，返回 SqlDataReader 对象
            SqlDataReader dc = dao.read(sql);
            // 遍历查询结果，并将其添加到 DataGridView 中
            while (dc.Read())
            {
                dataGridView1.Rows.Add(dc[1].ToString(), dc[2].ToString(),
                    Convert.ToInt32(dc[3].ToString()) + 2);
            }
            if (!dc.Read())
            {
                MessageBox.Show("没有信息");
            }
            // 关闭 SqlDataReader 和数据库连接
            dc.Close();
            dao.DaoClose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(comboBox1.Text);
                if (comboBox1.SelectedItem != null && num >= 3 && num <= 10)
                {
                    // 调用 Table 方法，从数据库中获取数据，并在 DataGridView 中显示
                    Table(num - 2);
                }
                else
                {
                    // 判断输入框是否为空、输入的数字是否合法
                    MessageBox.Show("输入框有误，请重新输入");
                }
            }
            catch
            {
                // 判断输入框是否为空、输入的数字是否合法
                MessageBox.Show("输入框有误，请重新输入");
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            // 关闭当前窗体
            this.Close();
        }
    }
}
