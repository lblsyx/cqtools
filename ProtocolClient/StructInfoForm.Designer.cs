using System.Windows.Forms;
using System;

namespace ProtocolClient
{
    partial class StructInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.enableMsgPack = new System.Windows.Forms.CheckBox();
            this.structSummaryTextBox = new System.Windows.Forms.TextBox();
            this.structNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fieldsDataGridView = new System.Windows.Forms.DataGridView();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fieldTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MapKeyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MapValueType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.feildLengthCloumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldsDataGridView)).BeginInit();
            this.SuspendLayout();
            this.fieldsDataGridView.EditingControlShowing += DataGridView_EditingControlShowing;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.enableMsgPack);
            this.groupBox1.Controls.Add(this.structSummaryTextBox);
            this.groupBox1.Controls.Add(this.structNameTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(522, 98);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "结构信息";
            // 
            // enableMsgPack
            // 
            this.enableMsgPack.AutoSize = true;
            this.enableMsgPack.Location = new System.Drawing.Point(421, 65);
            this.enableMsgPack.Name = "enableMsgPack";
            this.enableMsgPack.Size = new System.Drawing.Size(90, 16);
            this.enableMsgPack.TabIndex = 4;
            this.enableMsgPack.Text = "支持MsgPack";
            this.enableMsgPack.UseVisualStyleBackColor = true;
            this.enableMsgPack.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // structSummaryTextBox
            // 
            this.structSummaryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.structSummaryTextBox.Location = new System.Drawing.Point(266, 27);
            this.structSummaryTextBox.Name = "structSummaryTextBox";
            this.structSummaryTextBox.Size = new System.Drawing.Size(233, 21);
            this.structSummaryTextBox.TabIndex = 3;
            // 
            // structNameTextBox
            // 
            this.structNameTextBox.Location = new System.Drawing.Point(80, 27);
            this.structNameTextBox.Name = "structNameTextBox";
            this.structNameTextBox.Size = new System.Drawing.Size(101, 21);
            this.structNameTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(205, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "结构说明：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "结构名称：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.fieldsDataGridView);
            this.groupBox2.Location = new System.Drawing.Point(12, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(522, 347);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "字段列表";
            // 
            // fieldsDataGridView
            // 
            this.fieldsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.fieldsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.fieldTypeColumn,
            this.MapKeyType,
            this.MapValueType,
            this.feildLengthCloumn,
            this.Column4,
            this.Column5,
            this.Column6});
            this.fieldsDataGridView.Location = new System.Drawing.Point(6, 20);
            this.fieldsDataGridView.Name = "fieldsDataGridView";
            this.fieldsDataGridView.RowTemplate.Height = 23;
            this.fieldsDataGridView.Size = new System.Drawing.Size(510, 321);
            this.fieldsDataGridView.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.okButton.Location = new System.Drawing.Point(170, 469);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(100, 35);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "确定(&O)";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cancelButton.Location = new System.Drawing.Point(276, 469);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(100, 35);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "取消(&C)";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "FieldName";
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "字段名称";
            this.Column1.Name = "Column1";
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // fieldTypeColumn
            // 
            this.fieldTypeColumn.DataPropertyName = "FieldType";
            this.fieldTypeColumn.HeaderText = "字段类型";
            this.fieldTypeColumn.Name = "fieldTypeColumn";
            // 
            // MapKeyType
            // 
            this.MapKeyType.DataPropertyName = "MapFieldKeyType";
            this.MapKeyType.HeaderText = "Key";
            this.MapKeyType.Name = "MapKeyType";
            // 
            // MapValueType
            // 
            this.MapValueType.DataPropertyName = "MapFieldValueType";
            this.MapValueType.HeaderText = "Value";
            this.MapValueType.Name = "MapValueType";
            // 
            // feildLengthCloumn
            // 
            this.feildLengthCloumn.DataPropertyName = "FieldLength";
            this.feildLengthCloumn.HeaderText = "类型长度";
            this.feildLengthCloumn.Name = "feildLengthCloumn";
            this.feildLengthCloumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column4.DataPropertyName = "FieldSummary";
            this.Column4.HeaderText = "字段说明";
            this.Column4.Name = "Column4";
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "SortIndex";
            this.Column5.HeaderText = "排序索引";
            this.Column5.Name = "Column5";
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Visible = false;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "ProjectID";
            this.Column6.HeaderText = "项目ID";
            this.Column6.Name = "Column6";
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Visible = false;
            // 
            // StructInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 516);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StructInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "结构信息";
            this.Load += new System.EventHandler(this.StructInfoForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldsDataGridView)).EndInit();
            this.ResumeLayout(false);

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
                var cell = fieldsDataGridView.CurrentCell;
                if (cell == null) return;
                var row = cell.OwningRow;

                var keyTypeCell = row.Cells[$"MapKeyType"];
                var valueTypeCell = row.Cells[$"MapValueType"];
                var LengthColumnCell = row.Cells[$"feildLengthCloumn"];
                if (comboBox.SelectedItem == null)
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
                                if (keyTypeCell.Value == null)
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

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox structSummaryTextBox;
        private System.Windows.Forms.TextBox structNameTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView fieldsDataGridView;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox enableMsgPack;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewComboBoxColumn fieldTypeColumn;
        private DataGridViewTextBoxColumn MapKeyType;
        private DataGridViewTextBoxColumn MapValueType;
        private DataGridViewTextBoxColumn feildLengthCloumn;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewTextBoxColumn Column5;
        private DataGridViewTextBoxColumn Column6;
    }
}