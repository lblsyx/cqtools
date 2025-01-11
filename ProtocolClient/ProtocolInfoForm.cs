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
    public partial class ProtocolInfoForm : ToolForm
    {
        private bool mNewProtocolInfo;
        private ProtocolInfo mProtocolInfo;
        private ProtocolInfo mOldProtocolInfo;
        private RowDragDrop<FieldInfo> mReqRowDragDrop;
        private RowDragDrop<FieldInfo> mResRowDragDrop;

        public ProtocolInfoForm()
        {
            InitializeComponent();
        }

        public void New()
        {
            Text = "新建协议";
            mNewProtocolInfo = true;
            mOldProtocolInfo = null;
            mProtocolInfo = new ProtocolInfo();
            protocolNameTextBox.Text = string.Empty;
            protocolCodeTextBox.Text = "1";
            protocolSummaryTextBox.Text = string.Empty;
            reqDataGridView.DataSource = mProtocolInfo.ReqFields;
            resDataGridView.DataSource = mProtocolInfo.ResFields;
        }

        public void Edit(ProtocolInfo oProtocolInfo)
        {
            mNewProtocolInfo = false;
            mProtocolInfo = oProtocolInfo;
            mOldProtocolInfo = oProtocolInfo.Clone();
            if (mProtocolInfo != null)
            {
                Text = string.Format("编辑【{0}】协议", mProtocolInfo.ProtocolName);

                ProtocolUtil.LoadProtocolFields(mProtocolInfo);

                protocolNameTextBox.Text = mProtocolInfo.ProtocolName;
                protocolCodeTextBox.Text = mProtocolInfo.ProtocolCode.ToString();
                protocolSummaryTextBox.Text = mProtocolInfo.ProtocolSummary;
                reqDataGridView.DataSource = mProtocolInfo.ReqFields;
                resDataGridView.DataSource = mProtocolInfo.ResFields;
            }
        }

        private void ProtocolInfoForm_Load(object sender, EventArgs e)
        {
            if (mNewProtocolInfo == false && mProtocolInfo == null)
            {
                MessageBox.Show("无法编辑空对象");
                Close();
                return;
            }

            projectStatusLabel.Text = string.Format("当前项目：{0}", Global.SelectedProject.ProjectName);

            DataGridViewComboBoxColumn oDataGridViewComboBoxColumn = null;

            oDataGridViewComboBoxColumn = reqDataGridView.Columns["reqFieldTypeColumn"] as DataGridViewComboBoxColumn;
            oDataGridViewComboBoxColumn.DataSource = Global.FieldTypeList;
            reqDataGridView.EditingControlShowing += DataGridView_EditingControlShowing;

            oDataGridViewComboBoxColumn = resDataGridView.Columns["resFieldTypeColumn"] as DataGridViewComboBoxColumn;
            oDataGridViewComboBoxColumn.DataSource = Global.FieldTypeList;
            resDataGridView.EditingControlShowing += DataGridView_EditingControlShowing;

            mReqRowDragDrop = new RowDragDrop<FieldInfo>(reqDataGridView);
            mResRowDragDrop = new RowDragDrop<FieldInfo>(resDataGridView);
        }

        private void DataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control is ComboBox comboBox)
            {
                comboBox.SelectedIndexChanged -= ComboBox_SelectedIndexChanged;
                comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            }
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                DataGridView grid = reqDataGridView.CurrentCell != null ? reqDataGridView : resDataGridView;
                if (grid == null) return;
                var cell = grid.CurrentCell;
                if (cell == null) return;
                var row = cell.OwningRow;

                string prefix = grid == reqDataGridView ? "req" : "res";
                var keyTypeCell = row.Cells[$"{prefix}MapKeyType"];
                var valueTypeCell = row.Cells[$"{prefix}MapValueType"];
                var LengthColumnCell = row.Cells[$"{prefix}LengthColumn"];
                if (comboBox.SelectedItem==null)
                {
                    return;
                }
                else if (comboBox.SelectedItem.ToString() == "map")
                {
                    //// 如果是新选择的 map 类型且 key/value 类型为空，则弹出设置窗口
                    if (string.IsNullOrEmpty(keyTypeCell.Value?.ToString()) &&
                        string.IsNullOrEmpty(valueTypeCell.Value?.ToString()))
                    {
                        using (var form = new MapTypeSettingForm(keyTypeCell.Value?.ToString(), valueTypeCell.Value?.ToString()))
                        {
                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                keyTypeCell.Value = form.KeyType;
                                valueTypeCell.Value = form.ValueType;
                            }
                            else
                            {
                                if(keyTypeCell.Value == null)
                                    keyTypeCell.Value = "int";
                                if (valueTypeCell.Value == null)
                                    valueTypeCell.Value = "int";
                            }
                        }
                    }
                    LengthColumnCell.Value = null;
                }
                else
                {
                    // 如果选择了非 map 类型，清空 key/value 类型
                    keyTypeCell.Value = null;
                    valueTypeCell.Value = null;
                }
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            // 用协议最新版本号和当前版本号对比
            if (Global.ProtocolOperateVer != MySQLUtil.ReloadCurProtocolOperateVer())
            {
                MessageBox.Show("当前版本非最新版本请更新");
                return;
            }
            for (int i = 0; i < mProtocolInfo.ReqFields.Count; i++)
            {
                mProtocolInfo.ReqFields[i].SortIndex = i;
            }

            for (int i = 0; i < mProtocolInfo.ResFields.Count; i++)
            {
                mProtocolInfo.ResFields[i].SortIndex = i;
            }

            if (string.IsNullOrEmpty(protocolNameTextBox.Text.Trim()))
            {
                MessageBox.Show("协议名称不能为空!");
                return;
            }

            if (ProtocolUtil.CheckFieldNameAndType(mProtocolInfo.ReqFields, "请求字段") == false)
            {
                return;
            }

            if (ProtocolUtil.CheckFieldNameAndType(mProtocolInfo.ResFields, "响应字段") == false)
            {
                return;
            }

            if (mProtocolInfo.ProtocolName != protocolNameTextBox.Text.Trim())
            {
                if (ProtocolUtil.ProtocolExists(protocolNameTextBox.Text.Trim()))
                {
                    return;
                }
            }

            int pcode = Convert.ToInt32(protocolCodeTextBox.Text);
            if ((pcode % 2) == 0)
            {
                MessageBox.Show("协议号必需为奇数");
                return;
            }


            //string oldName = mProtocolInfo.ProtocolName;
            //string oldSummary = mProtocolInfo.ProtocolSummary;
            mProtocolInfo.ProtocolName = protocolNameTextBox.Text.Trim();
            mProtocolInfo.ProtocolCode = pcode;

            mProtocolInfo.ProtocolSummary = protocolSummaryTextBox.Text.Trim();
            //mProtocolInfo.SortIndex = Global.SelectedProject.Protocols.Count;

            if (mNewProtocolInfo)
            {//新建
                if (ProtocolUtil.AddProtocol(mProtocolInfo))
                {
                    Close();
                }
            }
            else
            {//修改
                if (mOldProtocolInfo.ProtocolName != mProtocolInfo.ProtocolName)
                {//修改了协议名称
                    if (ProtocolUtil.DelProtocol(mOldProtocolInfo) == false)
                    {
                        MessageBox.Show("修改失败!");
                        return;
                    }
                }
                
                if (ProtocolUtil.UpdateProtocol(mProtocolInfo))
                {
                    Close();
                }
                else
                {
                    ProtocolUtil.DelProtocolImp(mOldProtocolInfo);
                    MessageBox.Show("修改失败!");
                }
            }
        }

        private void rightButton_Click(object sender, EventArgs e)
        {
            if (mProtocolInfo == null) return;

            if (reqDataGridView.SelectedRows.Count != 0)
            {
                List<FieldInfo> list = new List<FieldInfo>();
                for (int i = 0; i < reqDataGridView.SelectedRows.Count; i++)
                {
                    int idx = reqDataGridView.Rows.IndexOf(reqDataGridView.SelectedRows[i]);
                    list.Add(mProtocolInfo.ReqFields[idx]);
                }

                foreach (var item in list)
                {
                    mProtocolInfo.ReqFields.Remove(item);
                    mProtocolInfo.ResFields.Add(item);
                }
            }
            else
            {
                MessageBox.Show("请选择字段后移动!");
            }
        }

        private void leftButton_Click(object sender, EventArgs e)
        {
            if (mProtocolInfo == null) return;

            if (resDataGridView.SelectedRows.Count != 0)
            {
                List<FieldInfo> list = new List<FieldInfo>();
                for (int i = 0; i < resDataGridView.SelectedRows.Count; i++)
                {
                    int idx = resDataGridView.Rows.IndexOf(resDataGridView.SelectedRows[i]);
                    list.Add(mProtocolInfo.ResFields[idx]);
                }

                foreach (var item in list)
                {
                    mProtocolInfo.ResFields.Remove(item);
                    mProtocolInfo.ReqFields.Add(item);
                }
            }
            else
            {
                MessageBox.Show("请选择字段后移动!");
            }
        }

        private void allRightbutton_Click(object sender, EventArgs e)
        {
            if (mProtocolInfo == null) return;

            List<FieldInfo> reqList = new List<FieldInfo>();
            reqList.AddRange(mProtocolInfo.ReqFields);
            mProtocolInfo.ReqFields.Clear();

            foreach (var item in reqList)
            {
                mProtocolInfo.ResFields.Add(item);
            }
        }

        private void allLeftButton_Click(object sender, EventArgs e)
        {
            if (mProtocolInfo == null) return;

            List<FieldInfo> resList = new List<FieldInfo>();
            resList.AddRange(mProtocolInfo.ResFields);
            mProtocolInfo.ResFields.Clear();

            foreach (var item in resList)
            {
                mProtocolInfo.ReqFields.Add(item);
            }
        }

        private void allSwapButton_Click(object sender, EventArgs e)
        {
            if (mProtocolInfo == null) return;

            List<FieldInfo> reqList = new List<FieldInfo>();
            reqList.AddRange(mProtocolInfo.ReqFields);
            mProtocolInfo.ReqFields.Clear();

            List<FieldInfo> resList = new List<FieldInfo>();
            resList.AddRange(mProtocolInfo.ResFields);
            mProtocolInfo.ResFields.Clear();

            foreach (var item in resList)
            {
                mProtocolInfo.ReqFields.Add(item);
            }

            foreach (var item in reqList)
            {
                mProtocolInfo.ResFields.Add(item);
            }
        }

        private void reqDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
