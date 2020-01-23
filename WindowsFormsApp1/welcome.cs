using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;//窗体美化

namespace news
{
    
    public partial class welcome : CCSkinMain
    {
        public int sign_in_state = 0;//0:未登录;1:用户登录;2:管理员登录
        public int limit = -1;//管理员级别：-1普通用户；0最高级别管理员；1新闻审核员；2新闻录入员;3版主
        public string name = "";
        DBHelper db;
        DataSet ds;
        public enum newsType
        {
            社会 = 0,
            国际 = 1,
            军事 = 2,
            历史 = 3,

        }
        public int newsTypechosed = 0;//默认选定0
        public welcome()
        {
            InitializeComponent();
            change_priviliege(0, "", -1);
            db = new DBHelper();
            db.Connection();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form form_sign_in = new sign_in();
            form_sign_in.Tag = this;
            this.Hide();
            form_sign_in.Show();

        }
        /*
         * 按下注册按钮
         */
        private void button2_Click(object sender, EventArgs e)
        {
            Form form_register = new register();
            form_register.Tag = this;
            this.Hide();
            form_register.Show();
        }
        /*按下管理按钮*/
        private void button3_Click(object sender, EventArgs e)
        {
            Form form_manager = new manager(limit);
            form_manager.Show();
        }

        public void change_priviliege(int newsign_in_state, string newname, int newlimit)
        {
            sign_in_state = newsign_in_state;
            if (sign_in_state == 0)//未登录
            {
                skinPanel2.Visible = true;//登录、注册按钮可用
                button5.Visible = false;//隐藏注销按钮
                button3.Visible = false;//管理按钮隐藏
                name = "";
            }
            else if (sign_in_state == 1)//普通用户登录
            {
                skinPanel2.Visible = false;//登录、注册按钮不可用
                button5.Visible = true;//启用注销按钮
                name = newname;
           
            }
            else if (sign_in_state == 2)
            {//管理员登录
                skinPanel2.Visible = false;//登录、注册按钮不可用
                button5.Visible = true;//启用注销按钮
                button3.Visible = true;//管理按钮启用
                name = newname;
                limit = newlimit;
              
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            change_priviliege(0, "", -1);
            MessageBox.Show("注销成功");
        }

        /*加载窗体*/
        private void welcome_Load(object sender, EventArgs e)
        {
            load_news(0);
        }

        /*加载新闻*/
        public void load_news(int a)
        {
            listView1.Items.Clear();
            ds = db.find_newsByType(a);
            int s = 0;
            if (ds.Tables.Count!= 0)
            {
                while (s < ds.Tables[0].Rows.Count)
                {
                    ListViewItem lt = new ListViewItem();
                    //ListViewItem[] lvs = new ListViewItem[5];
                    lt.Text = "";
                    lt.SubItems.Add(ds.Tables[0].Rows[s]["title"].ToString());
                    lt.SubItems.Add(ds.Tables[0].Rows[s]["author"].ToString());
                    lt.SubItems.Add(ds.Tables[0].Rows[s]["context"].ToString());
                    string d = " ";
                    lt.SubItems.Add(d);
                    listView1.Items.Add(lt);
                    s++;
                }
            }
        }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems.Count > 0 && sign_in_state==1)
            {
                string x = listView1.SelectedItems[0].SubItems[1].Text.ToString();//选中行的第一列的值
                string y = listView1.SelectedItems[0].SubItems[2].Text.ToString();//选中行的第二列的值
                string z = listView1.SelectedItems[0].SubItems[3].Text.ToString();//选中行的第三列的值
                Form s = new newsdetail(x,name);
                s.Show();
            }
            if(this.listView1.SelectedItems.Count > 0 && sign_in_state != 1)
            {
                string x = listView1.SelectedItems[0].SubItems[1].Text.ToString();//选中行的第一列的值
                string y = listView1.SelectedItems[0].SubItems[2].Text.ToString();//选中行的第二列的值
                string z = listView1.SelectedItems[0].SubItems[3].Text.ToString();//选中行的第三列的值
                MessageBox.Show(z);
            }

        }
        private void button_Click(object sender, EventArgs e)
        {
            string x = listView1.SelectedItems[0].SubItems[1].Text.ToString();//选中行的第一列的值
            string y = listView1.SelectedItems[0].SubItems[2].Text.ToString();//选中行的第二列的值
            string z = listView1.SelectedItems[0].SubItems[3].Text.ToString();//选中行的第三列的值
            Form s = new newsdetail(x,name);
            s.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            load_news(1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            load_news(0);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            load_news(2);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            load_news(3);
        }
    }
}
