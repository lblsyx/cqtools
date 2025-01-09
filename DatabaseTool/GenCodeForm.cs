using DatabaseTool.DatabaseCore;
using DatabaseTool.DatabaseCore.Generates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatabaseTool
{
    public partial class GenCodeForm : ToolForm
    {
        private DBInfo mDBInfo;
        private TBInfo[] mTBInfos;
        private GeneratorPath mGeneratorPath;
        private IStructGenerator mIStructGenerator;
        private GeneratorSetting mGeneratorSetting;

        public GenCodeForm()
        {
            InitializeComponent();
        }

        public void SetGenerator(DBInfo oDBInfo, TBInfo[] oTBInfos, IStructGenerator iIStructGenerator)
        {
            mDBInfo = oDBInfo;
            mTBInfos = oTBInfos;
            mIStructGenerator = iIStructGenerator;

            mGeneratorPath = Global.AppPaths.GetGeneratorPath(mDBInfo.DBName, mIStructGenerator.Type);
            mGeneratorSetting = Global.AppConfig.GetGeneratorSetting(mDBInfo.DBName, mIStructGenerator.Type);

            textBox1.Text = mGeneratorPath.OutputPath1;
            textBox2.Text = mGeneratorPath.OutputPath2;
            richTextBox1.Text = mGeneratorSetting.CodeContentFormat1;
            richTextBox2.Text = mGeneratorSetting.CodeContentFormat2;

            richTextBox2.Enabled = mIStructGenerator.OutputPathType2 != PathType.None;

            groupBox1.Text = string.Format("代码模板【需要包含 [{0}] 字符串】", mIStructGenerator.ReplaceStr);
            groupBox3.Text = string.Format("代码模板【需要包含 [{0}] 字符串】", mIStructGenerator.ReplaceStr);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string path = DatabaseUtil.GetPath(mIStructGenerator.OutputPathType1, mIStructGenerator.OutputFilter1);

            if (string.IsNullOrEmpty(path)) return;

            textBox1.Text = path;

            mGeneratorPath.OutputPath1 = path;

            Global.AppPaths.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = DatabaseUtil.GetPath(mIStructGenerator.OutputPathType2, mIStructGenerator.OutputFilter2);

            if (string.IsNullOrEmpty(path)) return;

            textBox2.Text = path;

            mGeneratorPath.OutputPath2 = path;

            Global.AppPaths.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.IndexOf(mIStructGenerator.ReplaceStr) == -1)
            {
                MessageBox.Show(string.Format("未包含 {0} 字符串!", mIStructGenerator.ReplaceStr));
                return;
            }

            if (mIStructGenerator.OutputPathType2 != PathType.None)
            {
                if (richTextBox2.Text.IndexOf(mIStructGenerator.ReplaceStr) == -1)
                {
                    MessageBox.Show(string.Format("未包含 {0} 字符串!", mIStructGenerator.ReplaceStr));
                    return;
                }
            }

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show(string.Format("未选择输出路径!"));
                return;
            }

            if (mIStructGenerator.OutputPathType2 != PathType.None)
            {
                if (string.IsNullOrEmpty(textBox2.Text))
                {
                    MessageBox.Show(string.Format("未选择输出路径!"));
                    return;
                }
            }

            mGeneratorSetting.CodeContentFormat1 = richTextBox1.Text;
            mGeneratorSetting.CodeContentFormat2 = richTextBox2.Text;
            Global.AppConfig.Save();

            mGeneratorPath.OutputPath1 = textBox1.Text;
            mGeneratorPath.OutputPath2 = textBox2.Text;
            Global.AppPaths.Save();

            try
            {
                mIStructGenerator.Generate(mDBInfo, mTBInfos, mGeneratorSetting, mGeneratorPath.OutputPath1, mGeneratorPath.OutputPath2);

                MessageBox.Show("生成成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("生成{0}失败！ErrorMsg:{1}", mIStructGenerator.Name, ex.Message));
            }

            Global.AppConfig.Save();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.IndexOf(mIStructGenerator.ReplaceStr) == -1)
            {
                MessageBox.Show(string.Format("未包含 {0} 字符串!", mIStructGenerator.ReplaceStr));
                return;
            }

            if (mIStructGenerator.OutputPathType2 != PathType.None)
            {
                if (richTextBox2.Text.IndexOf(mIStructGenerator.ReplaceStr) == -1)
                {
                    MessageBox.Show(string.Format("未包含 {0} 字符串!", mIStructGenerator.ReplaceStr));
                    return;
                }
            }

            mGeneratorSetting.CodeContentFormat1 = richTextBox1.Text;
            mGeneratorSetting.CodeContentFormat2 = richTextBox2.Text;

            Global.AppConfig.Save();
        }
    }
}
