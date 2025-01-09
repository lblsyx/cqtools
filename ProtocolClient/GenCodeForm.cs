using ProtocolCore;
using ProtocolCore.Generates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProtocolClient
{
    public partial class GenCodeForm : ToolForm
    {
        private IGenerator mIGenerator;
        private GeneratorInfo mGeneratorInfo;
        private GeneratorSetting mGeneratorSetting;

        public GenCodeForm()
        {
            InitializeComponent();
        }

        public void SetGeneratorInfo(GeneratorInfo oGeneratorInfo, GeneratorSetting oGeneratorSetting)
        {
            mGeneratorInfo = oGeneratorInfo;
            mGeneratorSetting = oGeneratorSetting;
        }

        private void GenCodeForm_Load(object sender, EventArgs e)
        {
            Text = string.Format("生成{0}({1})", mGeneratorInfo.Name, mGeneratorInfo.Type);

            mIGenerator = GeneratorMgr.GetGenerator(mGeneratorInfo.Type);
            code1RichTextBox.Text = mGeneratorSetting.ContentFormat1;
            code2RichTextBox.Text = mGeneratorSetting.ContentFormat2;
            path2Button.Enabled = mIGenerator.Path2Type != PathType.None;
            path2TextBox.Enabled = mIGenerator.Path2Type != PathType.None;
            path1TextBox.Text = Global.AppConfig.GetPath1(Global.SelectedProject.ProjectID, mGeneratorInfo.Type);
            path2TextBox.Text = Global.AppConfig.GetPath2(Global.SelectedProject.ProjectID, mGeneratorInfo.Type);

            if (Global.args.Length != 0)
            {
                genCodeButton_Click(null, null);
            }
        }

        private void path1Button_Click(object sender, EventArgs e)
        {
            string path = ProtocolUtil.GetPath(mIGenerator.Path1Type, mIGenerator.Path1Filter);

            if (string.IsNullOrEmpty(path) == false)
            {
                path1TextBox.Text = path;
                Global.AppConfig.SetPath1(Global.SelectedProject.ProjectID, mGeneratorInfo.Type, path1TextBox.Text);
            }
        }

        private void path2Button_Click(object sender, EventArgs e)
        {
            if (mIGenerator.Path2Type == PathType.None) return;

            string path = ProtocolUtil.GetPath(mIGenerator.Path2Type, mIGenerator.Path2Filter);

            if (string.IsNullOrEmpty(path) == false)
            {
                path2TextBox.Text = path;
                Global.AppConfig.SetPath2(Global.SelectedProject.ProjectID, mGeneratorInfo.Type, path2TextBox.Text);
            }
        }

        private void genCodeButton_Click(object sender, EventArgs e)
        {
            IGenerator iIGenerator = GeneratorMgr.GetGenerator(mGeneratorInfo.Type);

            if (iIGenerator != null)
            {
                try
                {
                    iIGenerator.Generate(Global.SelectedProject, mGeneratorSetting, path1TextBox.Text.Trim(), path2TextBox.Text.Trim());
                    
                    // 如果是带参数的，直接退出程序
                    if (Global.args.Length != 0)
                        System.Environment.Exit(0);
                    else
                        MessageBox.Show(string.Format("{0}代码生成成功!", mGeneratorInfo.Name));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("代码生成失败!ErrMsg:{0}", ex.Message));
                    return;
                }
            }
        }
    }
}
