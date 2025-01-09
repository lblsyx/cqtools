using ProtocolCore;
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
    public partial class StructInfoForm : ToolForm
    {
        private bool mNewStructInfo;
        private StructInfo mStructInfo;
        private StructInfo mOldStructInfo;
        private RowDragDrop<FieldInfo> mStructRowDragDrop;

        public StructInfoForm()
        {
            InitializeComponent();
        }

        public void New()
        {
            Text = "新建结构";
            //structNameTextBox.Enabled = true;
            mNewStructInfo = true;
            mOldStructInfo = null;
            mStructInfo = new StructInfo();
            structNameTextBox.Text = string.Empty;
            structSummaryTextBox.Text = string.Empty;
            fieldsDataGridView.DataSource = mStructInfo.Fields;
            mStructInfo.ProjectID = Global.SelectedProject.ProjectID;
        }

        public void Edit(StructInfo oStructInfo)
        {
            if (oStructInfo == null) return;

            mStructInfo = oStructInfo;
            mOldStructInfo = oStructInfo.Clone();

            ProtocolUtil.LoadStructFields(mStructInfo);

            //structNameTextBox.Enabled = false;
            structNameTextBox.Text = mStructInfo.StructName;
            structSummaryTextBox.Text = mStructInfo.StructSummary;
            enableMsgPack.Checked = (mStructInfo.EnableMsgpack == 1);
            fieldsDataGridView.DataSource = mStructInfo.Fields;
            Text = string.Format("编辑【{0}】结构", mStructInfo.StructName);
        }

        private void StructInfoForm_Load(object sender, EventArgs e)
        {
            DataGridViewComboBoxColumn oDataGridViewComboBoxColumn = null;

            oDataGridViewComboBoxColumn = fieldsDataGridView.Columns["fieldTypeColumn"] as DataGridViewComboBoxColumn;
            oDataGridViewComboBoxColumn.DataSource = Global.FieldTypeList;

            mStructRowDragDrop = new RowDragDrop<FieldInfo>(fieldsDataGridView);
            if (enableMsgPack.Checked)
            {
                mStructRowDragDrop.UnregisterDragEvent();
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // 用结构的最新版本号和当前版本号对比
            if (Global.StructOperateVer != MySQLUtil.ReloadCurStructOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }
            for (int i = 0; i < mStructInfo.Fields.Count; i++)
            {
                mStructInfo.Fields[i].SortIndex = i;
            }

            if (string.IsNullOrEmpty(structNameTextBox.Text.Trim()))
            {
                MessageBox.Show("结构名称不能为空!");
                return;
            }

            if (mStructInfo.StructName != structNameTextBox.Text.Trim())
            {
                if (ProtocolUtil.StructExists(structNameTextBox.Text.Trim()))
                {
                    return;
                }
            }

            if (ProtocolUtil.CheckFieldNameAndType(mStructInfo.Fields, "字段列表") == false)
            {
                return;
            }

            string oldName = mStructInfo.StructName;
            //string oldSummary = mStructInfo.StructSummary;
            mStructInfo.StructName = structNameTextBox.Text.Trim();
            mStructInfo.StructSummary = structSummaryTextBox.Text.Trim();

            if (mNewStructInfo)
            {//新建
                if (ProtocolUtil.AddStruct(mStructInfo))
                {
                    Close();
                }
            }
            else
            {//修改
                if (mOldStructInfo.StructName != mStructInfo.StructName)
                {//修改了结构名称
                    if (ProtocolUtil.DelStruct(mOldStructInfo) == false)
                    {
                        MessageBox.Show("修改失败!");
                        return;
                    }
                }
                if (ProtocolUtil.UpdateStruct(mStructInfo, oldName))
                {
                    Close();
                }
                else
                {
                    ProtocolUtil.DelStructImp(mOldStructInfo);
                    MessageBox.Show("修改失败!");
                }
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (enableMsgPack.Checked)
            {
                mStructInfo.EnableMsgpack = 1;
                //禁止删除和拖动
                this.fieldsDataGridView.AllowUserToDeleteRows = false;
                if (mStructRowDragDrop != null)
                    mStructRowDragDrop.UnregisterDragEvent();
            }
            else
            {
                mStructInfo.EnableMsgpack = 0;
                this.fieldsDataGridView.AllowUserToDeleteRows = true;
                if (mStructRowDragDrop != null)
                    mStructRowDragDrop.RegisterDragEvent();
            }
        }
    }
}
