namespace ProtocolClient
{
    partial class ProtocolInfoForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.projectStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.okButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.reqDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.resDataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.allSwapButton = new System.Windows.Forms.Button();
            this.allLeftButton = new System.Windows.Forms.Button();
            this.allRightbutton = new System.Windows.Forms.Button();
            this.leftButton = new System.Windows.Forms.Button();
            this.rightButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.protocolCodeTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.protocolSummaryTextBox = new System.Windows.Forms.TextBox();
            this.protocolNameTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.resFieldNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resFieldTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.resMapKeyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resMapValueType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resSummaryColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resSortIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reqFieldNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reqFieldTypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.reqMapKeyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reqMapValueType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reqLengthColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reqSummayrColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reqSortIndexColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reqDataGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resDataGridView)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.projectStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(984, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // projectStatusLabel
            // 
            this.projectStatusLabel.ForeColor = System.Drawing.Color.DarkOrange;
            this.projectStatusLabel.Name = "projectStatusLabel";
            this.projectStatusLabel.Size = new System.Drawing.Size(73, 17);
            this.projectStatusLabel.Text = "当前项目：-";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(859, 498);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(110, 30);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "确定(&O)";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 83);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(960, 412);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.reqDataGridView);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 406);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "请求字段";
            // 
            // reqDataGridView
            // 
            this.reqDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.reqDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reqDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.reqFieldNameColumn,
            this.reqFieldTypeColumn,
            this.reqMapKeyType,
            this.reqMapValueType,
            this.reqLengthColumn,
            this.reqSummayrColumn,
            this.reqSortIndexColumn});
            this.reqDataGridView.Location = new System.Drawing.Point(6, 20);
            this.reqDataGridView.Name = "reqDataGridView";
            this.reqDataGridView.RowTemplate.Height = 23;
            this.reqDataGridView.Size = new System.Drawing.Size(432, 380);
            this.reqDataGridView.TabIndex = 0;
            this.reqDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.reqDataGridView_CellContentClick);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.resDataGridView);
            this.groupBox3.Location = new System.Drawing.Point(513, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(444, 406);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "响应字段";
            // 
            // resDataGridView
            // 
            this.resDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.resFieldNameColumn,
            this.resFieldTypeColumn,
            this.resMapKeyType,
            this.resMapValueType,
            this.resLengthColumn,
            this.resSummaryColumn,
            this.resSortIndexColumn});
            this.resDataGridView.Location = new System.Drawing.Point(6, 20);
            this.resDataGridView.Name = "resDataGridView";
            this.resDataGridView.RowTemplate.Height = 23;
            this.resDataGridView.Size = new System.Drawing.Size(432, 380);
            this.resDataGridView.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.allSwapButton);
            this.groupBox4.Controls.Add(this.allLeftButton);
            this.groupBox4.Controls.Add(this.allRightbutton);
            this.groupBox4.Controls.Add(this.leftButton);
            this.groupBox4.Controls.Add(this.rightButton);
            this.groupBox4.Location = new System.Drawing.Point(453, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(54, 406);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // allSwapButton
            // 
            this.allSwapButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.allSwapButton.Location = new System.Drawing.Point(6, 238);
            this.allSwapButton.Name = "allSwapButton";
            this.allSwapButton.Size = new System.Drawing.Size(42, 23);
            this.allSwapButton.TabIndex = 4;
            this.allSwapButton.Text = "<<>>";
            this.allSwapButton.UseVisualStyleBackColor = true;
            this.allSwapButton.Click += new System.EventHandler(this.allSwapButton_Click);
            // 
            // allLeftButton
            // 
            this.allLeftButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.allLeftButton.Location = new System.Drawing.Point(6, 209);
            this.allLeftButton.Name = "allLeftButton";
            this.allLeftButton.Size = new System.Drawing.Size(42, 23);
            this.allLeftButton.TabIndex = 3;
            this.allLeftButton.Text = "<<";
            this.allLeftButton.UseVisualStyleBackColor = true;
            this.allLeftButton.Click += new System.EventHandler(this.allLeftButton_Click);
            // 
            // allRightbutton
            // 
            this.allRightbutton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.allRightbutton.Location = new System.Drawing.Point(6, 180);
            this.allRightbutton.Name = "allRightbutton";
            this.allRightbutton.Size = new System.Drawing.Size(42, 23);
            this.allRightbutton.TabIndex = 2;
            this.allRightbutton.Text = ">>";
            this.allRightbutton.UseVisualStyleBackColor = true;
            this.allRightbutton.Click += new System.EventHandler(this.allRightbutton_Click);
            // 
            // leftButton
            // 
            this.leftButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.leftButton.Location = new System.Drawing.Point(6, 151);
            this.leftButton.Name = "leftButton";
            this.leftButton.Size = new System.Drawing.Size(42, 23);
            this.leftButton.TabIndex = 1;
            this.leftButton.Text = "<";
            this.leftButton.UseVisualStyleBackColor = true;
            this.leftButton.Click += new System.EventHandler(this.leftButton_Click);
            // 
            // rightButton
            // 
            this.rightButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.rightButton.Location = new System.Drawing.Point(6, 122);
            this.rightButton.Name = "rightButton";
            this.rightButton.Size = new System.Drawing.Size(42, 23);
            this.rightButton.TabIndex = 0;
            this.rightButton.Text = ">";
            this.rightButton.UseVisualStyleBackColor = true;
            this.rightButton.Click += new System.EventHandler(this.rightButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.protocolCodeTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.protocolSummaryTextBox);
            this.groupBox1.Controls.Add(this.protocolNameTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(960, 65);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "协议信息";
            // 
            // protocolCodeTextBox
            // 
            this.protocolCodeTextBox.Location = new System.Drawing.Point(267, 27);
            this.protocolCodeTextBox.Name = "protocolCodeTextBox";
            this.protocolCodeTextBox.Size = new System.Drawing.Size(32, 21);
            this.protocolCodeTextBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(220, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "协议号：";
            // 
            // protocolSummaryTextBox
            // 
            this.protocolSummaryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.protocolSummaryTextBox.Location = new System.Drawing.Point(474, 27);
            this.protocolSummaryTextBox.Name = "protocolSummaryTextBox";
            this.protocolSummaryTextBox.Size = new System.Drawing.Size(459, 21);
            this.protocolSummaryTextBox.TabIndex = 3;
            // 
            // protocolNameTextBox
            // 
            this.protocolNameTextBox.Location = new System.Drawing.Point(80, 27);
            this.protocolNameTextBox.Name = "protocolNameTextBox";
            this.protocolNameTextBox.Size = new System.Drawing.Size(134, 21);
            this.protocolNameTextBox.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(413, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "协议说明：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "协议枚举：";
            // 
            // resFieldNameColumn
            // 
            this.resFieldNameColumn.DataPropertyName = "FieldName";
            this.resFieldNameColumn.Frozen = true;
            this.resFieldNameColumn.HeaderText = "字段名";
            this.resFieldNameColumn.Name = "resFieldNameColumn";
            this.resFieldNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.resFieldNameColumn.Width = 80;
            // 
            // resFieldTypeColumn
            // 
            this.resFieldTypeColumn.DataPropertyName = "FieldType";
            this.resFieldTypeColumn.Frozen = true;
            this.resFieldTypeColumn.HeaderText = "字段类型";
            this.resFieldTypeColumn.Name = "resFieldTypeColumn";
            this.resFieldTypeColumn.Width = 80;
            // 
            // resMapKeyType
            // 
            this.resMapKeyType.DataPropertyName = "MapFieldKeyType";
            this.resMapKeyType.HeaderText = "Key";
            this.resMapKeyType.Name = "resMapKeyType";
            // 
            // resMapValueType
            // 
            this.resMapValueType.DataPropertyName = "MapFieldValueType";
            this.resMapValueType.HeaderText = "Value";
            this.resMapValueType.Name = "resMapValueType";
            // 
            // resLengthColumn
            // 
            this.resLengthColumn.DataPropertyName = "FieldLength";
            this.resLengthColumn.HeaderText = "类型长度";
            this.resLengthColumn.Name = "resLengthColumn";
            this.resLengthColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.resLengthColumn.Width = 80;
            // 
            // resSummaryColumn
            // 
            this.resSummaryColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.resSummaryColumn.DataPropertyName = "FieldSummary";
            this.resSummaryColumn.HeaderText = "字段说明";
            this.resSummaryColumn.Name = "resSummaryColumn";
            this.resSummaryColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // resSortIndexColumn
            // 
            this.resSortIndexColumn.DataPropertyName = "SortIndex";
            this.resSortIndexColumn.HeaderText = "排序索引";
            this.resSortIndexColumn.Name = "resSortIndexColumn";
            this.resSortIndexColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.resSortIndexColumn.Visible = false;
            // 
            // reqFieldNameColumn
            // 
            this.reqFieldNameColumn.DataPropertyName = "FieldName";
            this.reqFieldNameColumn.Frozen = true;
            this.reqFieldNameColumn.HeaderText = "字段名";
            this.reqFieldNameColumn.Name = "reqFieldNameColumn";
            this.reqFieldNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.reqFieldNameColumn.Width = 80;
            // 
            // reqFieldTypeColumn
            // 
            this.reqFieldTypeColumn.DataPropertyName = "FieldType";
            this.reqFieldTypeColumn.Frozen = true;
            this.reqFieldTypeColumn.HeaderText = "字段类型";
            this.reqFieldTypeColumn.Name = "reqFieldTypeColumn";
            this.reqFieldTypeColumn.Width = 80;
            // 
            // reqMapKeyType
            // 
            this.reqMapKeyType.DataPropertyName = "MapFieldKeyType";
            this.reqMapKeyType.Frozen = true;
            this.reqMapKeyType.HeaderText = "Key";
            this.reqMapKeyType.Name = "reqMapKeyType";
            // 
            // reqMapValueType
            // 
            this.reqMapValueType.DataPropertyName = "MapFieldValueType";
            this.reqMapValueType.Frozen = true;
            this.reqMapValueType.HeaderText = "Value";
            this.reqMapValueType.Name = "reqMapValueType";
            // 
            // reqLengthColumn
            // 
            this.reqLengthColumn.DataPropertyName = "FieldLength";
            this.reqLengthColumn.Frozen = true;
            this.reqLengthColumn.HeaderText = "类型长度";
            this.reqLengthColumn.Name = "reqLengthColumn";
            this.reqLengthColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.reqLengthColumn.Width = 80;
            // 
            // reqSummayrColumn
            // 
            this.reqSummayrColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.reqSummayrColumn.DataPropertyName = "FieldSummary";
            this.reqSummayrColumn.HeaderText = "字段说明";
            this.reqSummayrColumn.Name = "reqSummayrColumn";
            this.reqSummayrColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // reqSortIndexColumn
            // 
            this.reqSortIndexColumn.DataPropertyName = "SortIndex";
            this.reqSortIndexColumn.HeaderText = "排序索引";
            this.reqSortIndexColumn.Name = "reqSortIndexColumn";
            this.reqSortIndexColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.reqSortIndexColumn.Visible = false;
            // 
            // ProtocolInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox1);
            this.MinimizeBox = false;
            this.Name = "ProtocolInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "协议信息";
            this.Load += new System.EventHandler(this.ProtocolInfoForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.reqDataGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.resDataGridView)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox protocolSummaryTextBox;
        private System.Windows.Forms.TextBox protocolNameTextBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView reqDataGridView;
        private System.Windows.Forms.DataGridView resDataGridView;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel projectStatusLabel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button rightButton;
        private System.Windows.Forms.Button leftButton;
        private System.Windows.Forms.Button allLeftButton;
        private System.Windows.Forms.Button allRightbutton;
        private System.Windows.Forms.Button allSwapButton;
        private System.Windows.Forms.TextBox protocolCodeTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn reqFieldNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn reqFieldTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reqMapKeyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn reqMapValueType;
        private System.Windows.Forms.DataGridViewTextBoxColumn reqLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reqSummayrColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn reqSortIndexColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resFieldNameColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn resFieldTypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resMapKeyType;
        private System.Windows.Forms.DataGridViewTextBoxColumn resMapValueType;
        private System.Windows.Forms.DataGridViewTextBoxColumn resLengthColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resSummaryColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn resSortIndexColumn;
    }
}