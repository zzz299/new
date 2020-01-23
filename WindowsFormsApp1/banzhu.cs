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
using System.IO;//窗体美化
namespace news
{
    public partial class banzhu : CCSkinMain
    {
        DBHelper db;
        int zhu;
        int index = -1;
        DataSet newsdata;
        public banzhu()
        {
            InitializeComponent();
        }
        public banzhu(int type2)
        {
            InitializeComponent();
            db = new DBHelper();
            db.Connection();
            zhu = type2;
            newsdata = db.find_newsByType(zhu);
            if(newsdata.Tables[0].Rows.Count>0)
            {
                index = 0;
                loadnews();
            }
        }
        public void loadnews()
        {
            label1.Text = newsdata.Tables[0].Rows[index]["title"].ToString();
            textBox1.Text= newsdata.Tables[0].Rows[index]["context"].ToString();
            string pic = newsdata.Tables[0].Rows[index]["picture"].ToString();
            if (pic == "")
            {
                pic = @"\pic\0.jpg";
            }
            string picpath = System.Windows.Forms.Application.StartupPath + pic;
            pictureBox1.Image = Image.FromFile(picpath);
            if (index>0)
            {
                button3.Visible = true;
            }
            else
            {
                button3.Visible = false;
            }
            if(index+1<newsdata.Tables[0].Rows.Count)
            {
                button4.Visible = true;
            }
            else
            {
                button4.Visible = false;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            --index;
            loadnews();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ++index;
            loadnews();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            newsdata = db.find_newsByTypezan(zhu);
            loadnews();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            newsdata = db.find_newsByType(zhu);
            loadnews();
        }
    }
}
