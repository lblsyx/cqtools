using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TemplateTool.Utils;
using System.IO;

namespace SVNUrl
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = FileUtil.BrowseFolder();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Text = FileUtil.BrowseFile();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("先选择搜索目录!");
                return;
            }

            if (string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("先选择输出文件!");
                return;
            }

            List<string> list = new List<string>();
            List<string> extList = new List<string>();
            List<string> excepts = new List<string>();
            excepts.AddRange(textBox2.Text.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            string replaceDir = textBox1.Text.LastIndexOf("\\") != textBox1.Text.Length - 1 ? textBox1.Text + "\\" : textBox1.Text;
            GetUrlRecursively(textBox3.Text, textBox1.Text, replaceDir, list, excepts, extList);

            string str = string.Join("\r\n", list.ToArray());
            byte[] fileBytes = Encoding.Default.GetBytes(str);
            FileUtil.WriteFileBytes(textBox4.Text, fileBytes);

            textBox5.Text = string.Join("\r\n", extList.ToArray());

            MessageBox.Show("提取完成!");
        }

        private void GetUrlRecursively(string pre, string dir, string replaceDir, List<string> list, List<string> exceptDirs, List<string> extList)
        {
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(dir);

            DirectoryInfo[] oDirectoryInfos = oDirectoryInfo.GetDirectories();

            foreach (var item in oDirectoryInfos)
            {
                if (exceptDirs.IndexOf(item.Name.Trim()) != -1)
                {
                    continue;
                }

                GetUrlRecursively(pre, item.FullName, replaceDir, list, exceptDirs, extList);
            }

            FileInfo[] fileArray = oDirectoryInfo.GetFiles();

            foreach (var item in fileArray)
            {
                string file = item.FullName.Replace(replaceDir, string.Empty).Replace("\\", "/");
                file = Path.Combine(pre, file);
                list.Add(file);

                if (extList.IndexOf(item.Extension) == -1)
                {
                    extList.Add(item.Extension);
                }
            }
        }
    }
}
