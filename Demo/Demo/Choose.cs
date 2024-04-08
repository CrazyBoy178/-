using System;
using System.Windows.Forms;

namespace Demo
{
    public partial class Choose : Form
    {
        private bool flag;// 用于记录游戏模式（true：消除模式；false：普通模式）
        public Choose()
        {
            InitializeComponent();//初始化组件
            this.StartPosition = FormStartPosition.CenterScreen;//设置窗体启动位置为屏幕中央
            this.FormBorderStyle = FormBorderStyle.FixedSingle;//设置窗体边框风格为固定单元边框
            this.MaximizeBox = false;//禁止最大化窗体
            this.MinimizeBox = false;//禁止最小化窗体

        }

        private void button1_Click(object sender, EventArgs e)//关闭按钮Click事件处理函数
        {
            MessageBox.Show("欢迎下次游玩");
            Application.Exit(); //关闭应用程序
        }

        private void button2_Click(object sender, EventArgs e)//开始游戏按钮Click事件处理函数
        {
            try
            {
                if (textBox1.Text != "" && (Convert.ToInt32(textBox1.Text)) >= 3 && (Convert.ToInt32(textBox1.Text)) <= 10)
                {
                    game(Convert.ToInt32(textBox1.Text));//如果输入合法，则调用game方法开始游戏
                }
                else
                {
                    MessageBox.Show("输入框有误，请重新输入");//弹出提示窗口显示输入框有误信息

                }
            }
            catch
            {
                MessageBox.Show("输入框有误，请重新输入");//弹出提示窗口显示输入框有误信息
            }
        }

        public void game(int n)//游戏开始方法
        {
            if (radioButton1.Checked == true)
            {
                flag = true;//判断是否选择随机模式
            }
            if (radioButton2.Checked == true)
            {
                flag = false;//判断是否选择自定义模式
            }
            MessageBox.Show("输入正确,即将开始游戏,祝您游戏愉快!!!");//弹出提示窗口显示输入正确信息
            this.Hide();//隐藏当前窗体
            Game g1 = new Game(n, flag);//创建新的Game窗体对象
            g1.ShowDialog();//以模式对话框形式打开Game窗体
            this.Show();//显示当前窗体
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            History history = new History();
            history.ShowDialog();
            this.Show();
        }
    }
}
