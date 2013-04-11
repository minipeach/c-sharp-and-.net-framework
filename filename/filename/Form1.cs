using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace filename
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        FileInfo[] filelist;
        string strFileFolder;

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("请选择文件夹", "错误");
                return;
            }
            string strOldFileName;
            string strNewFileName;
            string prefix = this.textBox1.Text.Trim();
            string strNewFilePath;
            string name = "";    //文件名
            string ext = "";     //扩展名
            int TotalFiles = 0;  //重命名的文件数
            int i = 0;

            DateTime StartTime = DateTime.Now;//获取开始时间   
            if (radioButton1.Checked)
            {
                foreach (FileInfo fi in filelist)
                {
                    if (i.ToString().Length == 1)
                        name = prefix + "00" + i.ToString();
                    else if (i.ToString().Length == 2)
                        name = prefix + "0" + i.ToString();
                    else
                        name = prefix + i.ToString();
                    strOldFileName = fi.Name;
                    //strNewFileName = fi.Name.Replace(strOldPart, strNewPart); 
                    ext = fi.Extension;
                    if (ext == ".jpg" || ext == ".gif" || ext == ".jpeg" || ext == ".JPG" || ext == ".GIF" || ext == ".JPEG" || ext == ".bmp" || ext == ".BMP")
                    {
                        strNewFileName = name + ext; ;
                        strNewFilePath = strFileFolder + "\\" + strNewFileName;
                        fi.MoveTo(strNewFilePath);
                        TotalFiles += 1;
                        i++;
                        this.listBox1.Items.Add(strOldFileName + "  已重命名为  " + strNewFileName);
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                }
            }
            else
            {
                foreach (FileInfo fi in filelist)
                {
                    strOldFileName = fi.Name;
                    //strNewFileName = fi.Name.Replace(strOldPart, strNewPart); 
                    ext = fi.Extension;
                    name = fi.Name.Replace(ext, "");
                    if (ext == ".jpg" || ext == ".gif" || ext == ".jpeg" || ext == ".JPG" || ext == ".GIF" || ext == ".JPEG" || ext == ".bmp" || ext == ".BMP")
                    {
                        strNewFileName = name + prefix;
                        strNewFilePath = strFileFolder + "\\" + strNewFileName;
                        fi.MoveTo(strNewFilePath);
                        TotalFiles += 1;
                        i++;
                        this.listBox1.Items.Add(strOldFileName + "  已重命名为  " + strNewFileName);
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                }
            }
            DateTime EndTime = DateTime.Now;//获取结束时间
            TimeSpan ts = EndTime - StartTime;
            this.listBox1.Items.Add("操作结束," + TotalFiles + "个文件被修改");
            this.listBox1.Items.Add("总耗时：" + ts.Hours.ToString() + "时" + ts.Minutes.ToString() + "分" + ts.Seconds.ToString() + "秒");
            listBox1.SelectedIndex = listBox1.Items.Count - 1;

        }



        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            FolderBrowserDialog f1 = new FolderBrowserDialog();
            if (f1.ShowDialog() == DialogResult.OK)
            {
                strFileFolder = f1.SelectedPath;
                DirectoryInfo di = new DirectoryInfo(strFileFolder);
                filelist = di.GetFiles("*.*");
                int count = 0;
                foreach (FileInfo fi in filelist)
                {
                    if (fi.Extension == ".jpg" || fi.Extension == ".gif" || fi.Extension == ".jpeg" || fi.Extension == ".JPG" || fi.Extension == ".GIF" || fi.Extension == ".JPEG"
                        || fi.Extension == ".bmp" || fi.Extension == ".BMP")//判断扩展名
                    {
                        count++;
                        this.listBox1.Items.Add(fi.Name + "已添加");
                        listBox1.SelectedIndex = listBox1.Items.Count - 1;
                    }
                }
                this.listBox1.Items.Add("共添加" + count + "个文件");
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!textBox1.Text.Trim().Equals(""))
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label1.Text = "请输入前缀";
                textBox1.Text = "";
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label1.Text = "请输入扩展名";
                textBox1.Text = "";
            }
        }

   

    }
}
