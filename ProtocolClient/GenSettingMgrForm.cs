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
    public partial class GenSettingMgrForm : ToolForm
    {
        private BindingList<GeneratorSetting> mGeneratorSettingList;
        public GenSettingMgrForm()
        {
            InitializeComponent();
        }

        private void GenSettingMgrForm_Load(object sender, EventArgs e)
        {
            mGeneratorSettingList = new BindingList<GeneratorSetting>();
            settingDataGridView.DataSource = mGeneratorSettingList;
            reloadGeneratorSettingList();
        }

        private void reloadGeneratorSettingList()
        {
            ProtocolUtil.LoadGeneratorSetting(mGeneratorSettingList);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            GenSettingForm oGenSettingForm = new GenSettingForm();
            oGenSettingForm.New();
            oGenSettingForm.ShowDialog(this);
            reloadGeneratorSettingList();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (settingDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = settingDataGridView.SelectedRows[0];

                GeneratorSetting oGeneratorSetting = oDataGridViewRow.DataBoundItem as GeneratorSetting;

                if (oGeneratorSetting != null)
                {
                    GenSettingForm oGenSettingForm = new GenSettingForm();
                    oGenSettingForm.Edit(oGeneratorSetting);
                    oGenSettingForm.ShowDialog(this);
                }
                else
                {
                    MessageBox.Show("获取编辑数据错误!");
                }
            }
            else
            {
                MessageBox.Show("请先选择生成模板后编辑!");
            }
        }

        private void settingDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            toolStripButton2_Click(null, null);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (settingDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = settingDataGridView.SelectedRows[0];

                GeneratorSetting oGeneratorSetting = oDataGridViewRow.DataBoundItem as GeneratorSetting;

                if (oGeneratorSetting != null)
                {
                    if (MessageBox.Show(string.Format("是否删除【{1}({0})】生成模板，此操作无法恢复!", oGeneratorSetting.GeneratorType, oGeneratorSetting.GeneratorName), "删除设置", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (ProtocolUtil.DelGeneratorSetting(oGeneratorSetting))
                        {
                            mGeneratorSettingList.Remove(oGeneratorSetting);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("获取删除数据错误!");
                }
            }
            else
            {
                MessageBox.Show("请先选择生成模板后编辑!");
            }
        }

        private void settingDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (settingDataGridView.SelectedRows.Count != 0)
            {
                DataGridViewRow oDataGridViewRow = settingDataGridView.SelectedRows[0];

                GeneratorSetting oGeneratorSetting = oDataGridViewRow.DataBoundItem as GeneratorSetting;

                if (oGeneratorSetting != null)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("【CodeFormat1】\n");
                    sb.Append(oGeneratorSetting.ContentFormat1);

                    int maxLen = 0;
                    string[] temp = oGeneratorSetting.ContentFormat1.Split('\n');
                    foreach (var item in temp)
                    {
                        maxLen = Math.Max(maxLen, item.Length);
                    }
                    temp = oGeneratorSetting.ContentFormat2.Split('\n');
                    foreach (var item in temp)
                    {
                        maxLen = Math.Max(maxLen, item.Length);
                    }
                    string empty = string.Empty.PadRight(maxLen + 3, '=');

                    sb.Append("\n\n");
                    sb.Append(empty);
                    sb.Append("\n\n【CodeFormat2】\n");
                    sb.Append(oGeneratorSetting.ContentFormat2);

                    codeRichTextBox.Text = sb.ToString();

                    return;
                }
            }

            codeRichTextBox.Text = string.Empty;
        }
    }
}
