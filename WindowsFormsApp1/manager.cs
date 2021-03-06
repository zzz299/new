﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin;//窗体美化
using System.IO;

namespace news
{
    public partial class manager : CCSkinMain
    {
        DBHelper db;
        System.IO.FileInfo file;
        string destinationFile = "";
        int limit = -1;//-1初始值；0最高级别管理员；1新闻审核员
        public enum newsType
        {
            社会 = 0,
            国际 = 1,
            军事 = 2,
            历史 = 3
        }
        public manager()
        {
            InitializeComponent();
            newstype.SelectedIndex = (int)newsType.社会;
            skinComboBox1.SelectedIndex = 0;
            db = new DBHelper();
            db.Connection();
        }

        public manager(int newlimit)
        {
            InitializeComponent();
            newstype.SelectedIndex = (int)newsType.社会;
            skinComboBox1.SelectedIndex = 0;
            db = new DBHelper();
            db.Connection();
            this.limit = newlimit;
            if (this.limit == 1)//为新闻审核员
            {
                skinTabPage1.Parent = null;
                skinTabPage2.Parent = null;
                skinTabPage3.Parent = null;
                skinTabPage4.Parent = null;
            }
            else if (this.limit == 2)
            {
                skinTabPage3.Parent = null;
                skinTabPage4.Parent = null;
                skinTabPage5.Parent = null;
            }
        }

        /*上传图片*/
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
                        MessageBox.Show("图片上传成功");
                    }
                }
                catch
                {

                }
            }
        }

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
                string context = newsarticle.Text.ToString().Trim();
                string destinationFile = pic.Text.ToString().Trim();
                Boolean context_success = true;
                if (context_success)
                {
                    Boolean addSuccess = db.addNews(title_str, type_int, author_str, datatime_str, context, destinationFile);
                    switch (addSuccess)
                    {
                        case true:
                            MessageBox.Show("添加成功");
                            title.Text = "";
                            author.Text = "";
                            pic.Text = "";
                            newsarticle.Text = "";
                            break;
                        case false:
                            MessageBox.Show("添加失败");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("文章存储失败");
                }

            }

        }
        /*添加管理员和普通用户的切换*/
        private void skinRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (skinRadioButton1.Checked)
                skinPanel1.Visible = true;
            else
                skinPanel1.Visible = false;
        }
        /*添加用户*/
        private void skinButton1_Click(object sender, EventArgs e)
        {
            string name = "";
            string password = "";
            string sex = "";
            string email = "";
            if (skinRadioButton1.Checked)//普通用户
            {
                name = name_TextBox.Text.ToString().Trim();
                password = password_TextBox.Text.ToString().Trim();
                if (skinRadioButton4.Checked)
                    sex = "男";
                else
                    sex = "女";
                email = skinTextBox5.Text.ToString().Trim();
                email = email + skinComboBox1.Text.Trim();

                int register_state = db.register(name, password, sex, email);

                switch (register_state)
                {
                    case -1:
                        MessageBox.Show("添加用户失败");
                        break;
                    case 0:
                        MessageBox.Show("用户名已存在");
                        break;
                    case 1:
                        MessageBox.Show("添加用户成功");
                        break;
                }
            }
            else
            {
                name = name_TextBox.Text.ToString().Trim();
                password = password_TextBox.Text.ToString().Trim();
                string limited = comboBox1.Text.ToString().Trim();
                int register_state = db.addAdmin(name, password,limited);

                switch (register_state)
                {
                    case -1:
                        MessageBox.Show("添加管理员失败");
                        break;
                    case 0:
                        MessageBox.Show("管理员名已存在");
                        break;
                    case 1:
                        MessageBox.Show("添加管理员成功");
                        break;
                }
            }
        }
        /*加载表格 */
        private void manager_Load(object sender, EventArgs e)
        {
            skinDataGridView1.DataSource = db.show_usertable().Tables[0];
            skinDataGridView2.DataSource = db.show_newstable().Tables[0];
            skinDataGridView3.DataSource = db.show_newsUnpassed().Tables[0];
        }
        /*双击任一单元格显示修改页面*/
        private void skinDataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id;
            id = int.Parse(this.skinDataGridView1[0, e.RowIndex].Value.ToString());
            Form form_updata_user = new updata_user(id);
            form_updata_user.Show();

        }
        /*刷新表格*/
        private void skinButton2_Click(object sender, EventArgs e)
        {
            skinDataGridView1.DataSource = db.show_usertable().Tables[0];
        }

        /*
         * 点击删除按钮
         * 同时删除sql server中对应数据
         */
        private void skinButton3_Click(object sender, EventArgs e)
        {
            int id;
            id = int.Parse(this.skinDataGridView1[0, skinDataGridView1.CurrentRow.Index].Value.ToString());
            Boolean delete_success = db.delete_user(id);
            if (delete_success)
            {
                skinDataGridView1.DataSource = db.show_usertable().Tables[0];
                MessageBox.Show("删除成功");
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }
        /*双击任一单元格显示修改新闻*/
        private void skinDataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id;
            id = int.Parse(this.skinDataGridView2[0, e.RowIndex].Value.ToString());
            Form form_update_news = new update_news(id);
            form_update_news.Show();
        }
        /*刷新新闻表格*/
        private void skinButton5_Click(object sender, EventArgs e)
        {
            skinDataGridView2.DataSource = db.show_newstable().Tables[0];
        }

        /*按下按钮删除新闻*/
        private void skinButton4_Click(object sender, EventArgs e)
        {
            int id;
            id = int.Parse(this.skinDataGridView2[0, skinDataGridView2.CurrentRow.Index].Value.ToString());
            string newsname = this.skinDataGridView2[1, skinDataGridView2.CurrentRow.Index].Value.ToString();
            Boolean delete_success = db.delete_news(id,newsname);
            if (delete_success)
            {
                skinDataGridView2.DataSource = db.show_newstable().Tables[0];
                MessageBox.Show("删除成功");
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        /*刷新审核表格*/
        private void skinButton7_Click(object sender, EventArgs e)
        {
            skinDataGridView3.DataSource = db.show_newsUnpassed().Tables[0];
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            int id;
            id = int.Parse(this.skinDataGridView3[0, skinDataGridView3.CurrentRow.Index].Value.ToString());
            Boolean passed_success = db.update_NewsPassed(id);
            if (passed_success)
            {
                skinDataGridView3.DataSource = db.show_newsUnpassed().Tables[0];
                MessageBox.Show("新闻通过审核");
            }
            else
            {
                MessageBox.Show("审核失败");
            }
        }
        private void skinRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }
    }
}
