﻿using System;
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
    public partial class update_news : CCSkinMain
    {
        int news_id;
        DBHelper db;
        DataSet newsdate;
        System.IO.FileInfo file;
        string destinationFile = "";
        public enum newsType
        {
            社会 = 0,
            国际 = 1,
            军事 = 2,
            历史 = 3
        }
        public update_news()
        {
            InitializeComponent();
            db = new DBHelper();
            db.Connection();
        }

        public update_news(int id)
        {
            InitializeComponent();
            news_id = id;
            db = new DBHelper();
            db.Connection();
        }
        /*加载窗体*/
        private void update_news_Load(object sender, EventArgs e)
        {
            newsdate = db.find_newsbyid(news_id);
            if (newsdate.Tables[0].Rows.Count > 0)//新闻存在
            {
                title.Text = newsdate.Tables[0].Rows[0]["title"].ToString();
                author.Text = newsdate.Tables[0].Rows[0]["author"].ToString();
                newstype.SelectedIndex = int.Parse(newsdate.Tables[0].Rows[0]["type"].ToString());
                pic.Text = newsdate.Tables[0].Rows[0]["picture"].ToString();
                dateTimePicker1.Text = newsdate.Tables[0].Rows[0]["time"].ToString();
                string contextFile = newsdate.Tables[0].Rows[0]["context"].ToString();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "图片|*.jpg";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                file = new System.IO.FileInfo(ofd.FileName);
                //其他代码
                pic.Text = ofd.FileName;
                destinationFile = @"\\pic\\" + file.Name;
                try
                {
                    if (file.Exists)
                    {
                        file.CopyTo(System.Windows.Forms.Application.StartupPath + destinationFile, true);
                        MessageBox.Show("图片更改成功");
                    }
                }
                catch
                {

                }
            }
        }

        /*修改新闻按钮*/
        private void addnews_Click(object sender, EventArgs e)
        {
            string tip = "";
            skinLabel6.Text = "";//每次点击都要刷新提示
            Boolean Isinputlegal = true;//输入是否合法
            if (title.Text.Length == 0)
            {
                tip = tip + "标题不能为空；";
                Isinputlegal = false;
            }
            if (author.Text.Length == 0)
            {
                tip = tip + "作者不能为空；";
                Isinputlegal = false;
            }
            if (pic.Text.Length == 0)
            {
                destinationFile = @"\pic\0.jpg";
                tip = tip + "图片不能为空；";
                Isinputlegal = false;
            }
            if (newsarticle.Text.Length == 0)
            {
                tip = tip + "文章内容不能为空；";
            }
            skinLabel6.Text = tip;
            if (Isinputlegal)
            {
                string title_str = title.Text.ToString().Trim();
                int type_int = newstype.SelectedIndex;
                string author_str = author.Text.ToString().Trim();
                string datatime_str = dateTimePicker1.Text.ToString().Trim();
                string contextFile = newsarticle.Text.ToString().Trim();
                //destinationFile = @"\" + destinationFile;
                Boolean context_success = true;
                if (context_success)
                {
                    Boolean addSuccess = db.update_News(news_id, title_str, type_int, author_str, datatime_str, contextFile, destinationFile);
                    switch (addSuccess)
                    {
                        case true:
                            MessageBox.Show("修改成功");

                            break;
                        case false:
                            MessageBox.Show("修改失败");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("文章正文储存失败");
                }

            }
        }
    }
}
