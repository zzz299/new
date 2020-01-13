using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;
using Microsoft.VisualBasic;
namespace news
{
    public partial class commend : CCSkinMain
    {
        string username = "";
        string newsname = "";
        DBHelper db;
        DataSet ds;
        public commend()
        {

            InitializeComponent();
            db = new DBHelper();
            db.Connection();
        }
        /*传入新闻名字和用户名*/
        public commend(string a,string b)
        {
            newsname = a;
            username = b;
            InitializeComponent();
            db = new DBHelper();
            db.Connection();
            listView1.Items.Clear();
            ds = db.find_commendBynewsname(a);
            int s = 0;
            if (ds.Tables.Count != 0)
            {
                while (s < ds.Tables[0].Rows.Count)
                {
                    ListViewItem lt = new ListViewItem();
                    lt.Text = "";
                    lt.SubItems.Add(ds.Tables[0].Rows[s][0].ToString());
                    lt.SubItems.Add(ds.Tables[0].Rows[s][1].ToString());
                    lt.SubItems.Add(ds.Tables[0].Rows[s][2].ToString());
                    listView1.Items.Add(lt);
                    s++;
                }
            }
            
        }
        /*进行评论*/
        private void button1_Click(object sender, EventArgs e)
        {
            String s = Interaction.InputBox("请输入评论", "评论框", "无", -1, -1);
            if (s != "")
            {
                string time = DateTime.Now.ToLongDateString().ToString();
                Boolean addSuccess = db.addCommend(username, newsname, s, time);
                if (addSuccess)
                {
                    MessageBox.Show("评论成功!");
                }
            }
        }
        /*评论完可刷新*/
        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            ds = db.find_commendBynewsname(newsname);
            int s = 0;
            if (ds.Tables.Count != 0)
            {
                while (s < ds.Tables[0].Rows.Count)
                {
                    ListViewItem lt = new ListViewItem();
                    lt.Text = "";
                    lt.SubItems.Add(ds.Tables[0].Rows[s][0].ToString());
                    lt.SubItems.Add(ds.Tables[0].Rows[s][1].ToString());
                    lt.SubItems.Add(ds.Tables[0].Rows[s][2].ToString());
                    listView1.Items.Add(lt);
                    s++;
                }
            }
        }
    }
}
