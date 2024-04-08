using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo
{
    internal class Dao
    {
        SqlConnection sc;//Sql连接对象
        public SqlConnection connect()
        {
            string str = @"Data Source=LAPTOP-2F32NPK8 ;Initial Catalog=GameDB;Integrated Security=True";//数据库连接字符串
            sc = new SqlConnection(str);//创建数据库连接对象
            sc.Open();//打开数据库
            return sc;//返回数据库
        }
        public SqlCommand command(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, connect());//创建SqlCommand对象，连接到数据库
            return cmd;//返回SqlCommand对象
        }

        public int Execute(string sql)//更新操作
        {
            return command(sql).ExecuteNonQuery(); //执行更新操作，并返回受影响的行数
        }

        public SqlDataReader read(string sql)//读取操作
        {
            return command(sql).ExecuteReader(); //执行查询操作，并返回SqlDataReader对象
        }

        public void DaoClose()
        {
            sc.Close();//关闭数据库连接
        }
    }
}
