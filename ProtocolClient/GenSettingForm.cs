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
    public partial class GenSettingForm : ToolForm
    {
        private bool mNewGeneratorSetting;
        private GeneratorSetting mGeneratorSetting;

        public GenSettingForm()
        {
            InitializeComponent();
        }

        public void New()
        {
            mNewGeneratorSetting = true;
            mGeneratorSetting = new GeneratorSetting();
            code1RichTextBox.Text = string.Empty;
            code2RichTextBox.Text = string.Empty;
        }

        public void Edit(GeneratorSetting oGeneratorSetting)
        {
            mNewGeneratorSetting = false;
            mGeneratorSetting = oGeneratorSetting;
            code1RichTextBox.Text = mGeneratorSetting.ContentFormat1;
            code2RichTextBox.Text = mGeneratorSetting.ContentFormat2;
        }

        private void GenSettingForm_Load(object sender, EventArgs e)
        {
            BindingList<GeneratorSetting> list = new BindingList<GeneratorSetting>();

            ProtocolUtil.LoadGeneratorSetting(list);

            BindingList<GeneratorInfo> glist = new BindingList<GeneratorInfo>();
            foreach (var item in GeneratorMgr.GeneratorInfoList)
            {
                bool bExists = false;

                for (int i = 0; i < list.Count; i++)
                {
                    if (item.Type == list[i].GeneratorType && list[i].GeneratorType != mGeneratorSetting.GeneratorType)
                    {
                        bExists = true;
                        break;
                    }
                }

                if (bExists == false)
                {
                    glist.Add(item);
                }
            }

            typeComboBox.DisplayMember = "Name";
            typeComboBox.ValueMember = "Type";
            typeComboBox.DataSource = glist;

            int idx = -1;
            for (int i = 0; i < glist.Count; i++)
            {
                if (glist[i].Type == mGeneratorSetting.GeneratorType)
                {
                    idx = i;
                    break;
                }
            }
            typeComboBox.SelectedIndex = idx;
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GeneratorInfo oGeneratorInfo = typeComboBox.SelectedItem as GeneratorInfo;

            if (oGeneratorInfo != null)
            {
                IGenerator iIGenerator = GeneratorMgr.GetGenerator(oGeneratorInfo.Type);

                groupBox1.Text = string.Format("代码模板【必须包含 {0} 字符串】", iIGenerator.ReplaceStr);
                enableCode2CheckBox.Checked = iIGenerator.Path2Type != PathType.None;
                code2RichTextBox.Enabled = enableCode2CheckBox.Checked;
            }
            else
            {
                groupBox1.Text = string.Format("代码模板【必须包含 {0} 字符串】", Global.CODE_REPLACE_STR);
                code2RichTextBox.Enabled = true;
                enableCode2CheckBox.Checked = true;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (typeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("未选择生成器!");
                return;
            }

            if (string.IsNullOrEmpty(code1RichTextBox.Text))
            {
                MessageBox.Show("左侧代码模板不能为空!");
                return;
            }

            GeneratorInfo oGeneratorInfo = typeComboBox.SelectedItem as GeneratorInfo;
            IGenerator iIGenerator = GeneratorMgr.GetGenerator(oGeneratorInfo.Type);

            if (code1RichTextBox.Text.IndexOf(iIGenerator.ReplaceStr) == -1)
            {
                MessageBox.Show(string.Format("左侧代码模板里未包含 {0} 字符串!", iIGenerator.ReplaceStr));
                return;
            }

            if (iIGenerator.Path2Type != PathType.None)
            {
                if (string.IsNullOrEmpty(code2RichTextBox.Text))
                {
                    MessageBox.Show("右侧代码模板不能为空!");
                    return;
                }

                if (code2RichTextBox.Text.IndexOf(iIGenerator.ReplaceStr) == -1)
                {
                    MessageBox.Show(string.Format("右侧代码模板里未包含 {0} 字符串!", iIGenerator.ReplaceStr));
                    return;
                }
            }
            else
            {
                code2RichTextBox.Text = string.Empty;
            }

            if (mNewGeneratorSetting == false && mGeneratorSetting.GeneratorType != oGeneratorInfo.Type)
            {
                if (ProtocolUtil.CheckGeneratorSetting(oGeneratorInfo.Type))
                {
                    return;
                }
            }

            mGeneratorSetting.GeneratorType = oGeneratorInfo.Type;
            mGeneratorSetting.GeneratorName = oGeneratorInfo.Name;
            mGeneratorSetting.ProjectID = Global.SelectedProject.ProjectID;
            mGeneratorSetting.ContentFormat1 = code1RichTextBox.Text;
            mGeneratorSetting.ContentFormat2 = code2RichTextBox.Text;

            if (mNewGeneratorSetting)
            {//新建
                if (ProtocolUtil.AddGeneratorSetting(mGeneratorSetting))
                {
                    Close();
                }
            }
            else
            {//编辑
                if (ProtocolUtil.UpdateGeneratorSetting(mGeneratorSetting))
                {
                    Close();
                }
            }
        }
    }
}
