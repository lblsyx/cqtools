namespace ProtocolClient
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.topMenuStrip = new System.Windows.Forms.MenuStrip();
            this.ProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SelectProjToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.NewProjToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditProjToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProjMgrToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RefreshProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.GenCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GenSettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SettingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bottomStatusStrip = new System.Windows.Forms.StatusStrip();
            this.UserStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProjectStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ResultStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.globalTimer = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.psTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.protocolDataGridView = new System.Windows.Forms.DataGridView();
            this.protocolContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delProtocolToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.structDataGridView = new System.Windows.Forms.DataGridView();
            this.structIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structSummaryColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.structContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.newStructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editStructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delStructToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lastViewButton = new System.Windows.Forms.Button();
            this.viewButton = new System.Windows.Forms.Button();
            this.verTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fieldsRichTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.protocolIDColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.protocolNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.protocolSummaryColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.topMenuStrip.SuspendLayout();
            this.bottomStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.psTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.protocolDataGridView)).BeginInit();
            this.protocolContextMenu.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.structDataGridView)).BeginInit();
            this.structContextMenu.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // topMenuStrip
            // 
            this.topMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProjectToolStripMenuItem,
            this.ProtocolToolStripMenuItem,
            this.SettingToolStripMenuItem});
            this.topMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.topMenuStrip.Name = "topMenuStrip";
            this.topMenuStrip.Size = new System.Drawing.Size(927, 25);
            this.topMenuStrip.TabIndex = 0;
            this.topMenuStrip.Text = "topMenuStrip";
            // 
            // ProjectToolStripMenuItem
            // 
            this.ProjectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectProjToolStripMenuItem,
            this.toolStripSeparator1,
            this.NewProjToolStripMenuItem,
            this.EditProjToolStripMenuItem,
            this.ProjMgrToolStripMenuItem});
            this.ProjectToolStripMenuItem.Name = "ProjectToolStripMenuItem";
            this.ProjectToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.ProjectToolStripMenuItem.Text = "项目(&P)";
            // 
            // SelectProjToolStripMenuItem
            // 
            this.SelectProjToolStripMenuItem.Name = "SelectProjToolStripMenuItem";
            this.SelectProjToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.SelectProjToolStripMenuItem.Text = "所有项目(&S)";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // NewProjToolStripMenuItem
            // 
            this.NewProjToolStripMenuItem.Name = "NewProjToolStripMenuItem";
            this.NewProjToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.NewProjToolStripMenuItem.Text = "新建(&N)...";
            this.NewProjToolStripMenuItem.Click += new System.EventHandler(this.NewProjToolStripMenuItem_Click);
            // 
            // EditProjToolStripMenuItem
            // 
            this.EditProjToolStripMenuItem.Name = "EditProjToolStripMenuItem";
            this.EditProjToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.EditProjToolStripMenuItem.Text = "编辑(&E)...";
            this.EditProjToolStripMenuItem.Click += new System.EventHandler(this.EditProjToolStripMenuItem_Click);
            // 
            // ProjMgrToolStripMenuItem
            // 
            this.ProjMgrToolStripMenuItem.Name = "ProjMgrToolStripMenuItem";
            this.ProjMgrToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.ProjMgrToolStripMenuItem.Text = "管理(&M)...";
            this.ProjMgrToolStripMenuItem.Click += new System.EventHandler(this.ProjMgrToolStripMenuItem_Click);
            // 
            // ProtocolToolStripMenuItem
            // 
            this.ProtocolToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RefreshProtocolToolStripMenuItem,
            this.toolStripMenuItem1,
            this.GenCodeToolStripMenuItem,
            this.GenSettingToolStripMenuItem,
            this.toolStripMenuItem2,
            this.ImportToolStripMenuItem,
            this.ExportToolStripMenuItem});
            this.ProtocolToolStripMenuItem.Name = "ProtocolToolStripMenuItem";
            this.ProtocolToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.ProtocolToolStripMenuItem.Text = "协议(&X)";
            // 
            // RefreshProtocolToolStripMenuItem
            // 
            this.RefreshProtocolToolStripMenuItem.Name = "RefreshProtocolToolStripMenuItem";
            this.RefreshProtocolToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.RefreshProtocolToolStripMenuItem.Text = "刷新(&R)";
            this.RefreshProtocolToolStripMenuItem.Click += new System.EventHandler(this.RefreshProtocolToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(145, 6);
            // 
            // GenCodeToolStripMenuItem
            // 
            this.GenCodeToolStripMenuItem.Name = "GenCodeToolStripMenuItem";
            this.GenCodeToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.GenCodeToolStripMenuItem.Text = "生成代码(&G)";
            // 
            // GenSettingToolStripMenuItem
            // 
            this.GenSettingToolStripMenuItem.Name = "GenSettingToolStripMenuItem";
            this.GenSettingToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.GenSettingToolStripMenuItem.Text = "生成模板(&S)...";
            this.GenSettingToolStripMenuItem.Click += new System.EventHandler(this.GenSettingToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 6);
            // 
            // ImportToolStripMenuItem
            // 
            this.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
            this.ImportToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ImportToolStripMenuItem.Text = "导入(&I)";
            // 
            // ExportToolStripMenuItem
            // 
            this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
            this.ExportToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ExportToolStripMenuItem.Text = "导出(&E)";
            // 
            // SettingToolStripMenuItem
            // 
            this.SettingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolDBToolStripMenuItem});
            this.SettingToolStripMenuItem.Name = "SettingToolStripMenuItem";
            this.SettingToolStripMenuItem.Size = new System.Drawing.Size(59, 21);
            this.SettingToolStripMenuItem.Text = "设置(&S)";
            // 
            // ToolDBToolStripMenuItem
            // 
            this.ToolDBToolStripMenuItem.Name = "ToolDBToolStripMenuItem";
            this.ToolDBToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ToolDBToolStripMenuItem.Text = "工具选项(&T)...";
            this.ToolDBToolStripMenuItem.Click += new System.EventHandler(this.ToolDBToolStripMenuItem_Click);
            // 
            // bottomStatusStrip
            // 
            this.bottomStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UserStatusLabel,
            this.ProjectStatusLabel,
            this.ResultStatusLabel});
            this.bottomStatusStrip.Location = new System.Drawing.Point(0, 490);
            this.bottomStatusStrip.Name = "bottomStatusStrip";
            this.bottomStatusStrip.Size = new System.Drawing.Size(927, 22);
            this.bottomStatusStrip.TabIndex = 1;
            this.bottomStatusStrip.Text = "statusStrip1";
            // 
            // UserStatusLabel
            // 
            this.UserStatusLabel.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.UserStatusLabel.Name = "UserStatusLabel";
            this.UserStatusLabel.Size = new System.Drawing.Size(73, 17);
            this.UserStatusLabel.Text = "当前用户：-";
            // 
            // ProjectStatusLabel
            // 
            this.ProjectStatusLabel.ForeColor = System.Drawing.Color.DarkOrange;
            this.ProjectStatusLabel.Name = "ProjectStatusLabel";
            this.ProjectStatusLabel.Size = new System.Drawing.Size(73, 17);
            this.ProjectStatusLabel.Text = "当前项目：-";
            // 
            // ResultStatusLabel
            // 
            this.ResultStatusLabel.ForeColor = System.Drawing.Color.DimGray;
            this.ResultStatusLabel.Name = "ResultStatusLabel";
            this.ResultStatusLabel.Size = new System.Drawing.Size(24, 17);
            this.ResultStatusLabel.Text = "    ";
            // 
            // globalTimer
            // 
            this.globalTimer.Interval = 30;
            this.globalTimer.Tick += new System.EventHandler(this.globalTimer_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.psTabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(903, 458);
            this.splitContainer1.SplitterDistance = 561;
            this.splitContainer1.TabIndex = 2;
            // 
            // psTabControl
            // 
            this.psTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.psTabControl.Controls.Add(this.tabPage1);
            this.psTabControl.Controls.Add(this.tabPage2);
            this.psTabControl.Location = new System.Drawing.Point(3, 3);
            this.psTabControl.Name = "psTabControl";
            this.psTabControl.SelectedIndex = 0;
            this.psTabControl.Size = new System.Drawing.Size(555, 452);
            this.psTabControl.TabIndex = 0;
            this.psTabControl.SelectedIndexChanged += new System.EventHandler(this.psTabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.protocolDataGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(547, 426);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "协议";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // protocolDataGridView
            // 
            this.protocolDataGridView.AllowUserToAddRows = false;
            this.protocolDataGridView.AllowUserToDeleteRows = false;
            this.protocolDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.protocolDataGridView.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.protocolDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.protocolDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.protocolDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.protocolIDColumn,
            this.protocolNameColumn,
            this.Column5,
            this.Column,
            this.protocolSummaryColumn,
            this.Column1,
            this.Column2,
            this.Column6});
            this.protocolDataGridView.ContextMenuStrip = this.protocolContextMenu;
            this.protocolDataGridView.Location = new System.Drawing.Point(0, 0);
            this.protocolDataGridView.MultiSelect = false;
            this.protocolDataGridView.Name = "protocolDataGridView";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(130)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.protocolDataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.protocolDataGridView.RowHeadersWidth = 5;
            this.protocolDataGridView.RowTemplate.Height = 23;
            this.protocolDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.protocolDataGridView.Size = new System.Drawing.Size(535, 417);
            this.protocolDataGridView.TabIndex = 0;
            this.protocolDataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.protocolDataGridView_CurrentCellDirtyStateChanged);
            this.protocolDataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.Column_CellValueChanged);
            this.protocolDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.protocolDataGridView_CellMouseDoubleClick);
            this.protocolDataGridView.SelectionChanged += new System.EventHandler(this.protocolDataGridView_SelectionChanged);
            // 
            // protocolContextMenu
            // 
            this.protocolContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProtocolToolStripMenuItem,
            this.editProtocolToolStripMenuItem,
            this.delProtocolToolStripMenuItem});
            this.protocolContextMenu.Name = "protocolContextMenu";
            this.protocolContextMenu.Size = new System.Drawing.Size(152, 70);
            // 
            // newProtocolToolStripMenuItem
            // 
            this.newProtocolToolStripMenuItem.Name = "newProtocolToolStripMenuItem";
            this.newProtocolToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.newProtocolToolStripMenuItem.Text = "新增协议(&N)...";
            this.newProtocolToolStripMenuItem.Click += new System.EventHandler(this.newProtocolToolStripMenuItem_Click);
            // 
            // editProtocolToolStripMenuItem
            // 
            this.editProtocolToolStripMenuItem.Name = "editProtocolToolStripMenuItem";
            this.editProtocolToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.editProtocolToolStripMenuItem.Text = "编辑协议(&E)...";
            this.editProtocolToolStripMenuItem.Click += new System.EventHandler(this.editProtocolToolStripMenuItem_Click);
            // 
            // delProtocolToolStripMenuItem
            // 
            this.delProtocolToolStripMenuItem.Name = "delProtocolToolStripMenuItem";
            this.delProtocolToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.delProtocolToolStripMenuItem.Text = "删除协议(&D)...";
            this.delProtocolToolStripMenuItem.Click += new System.EventHandler(this.delProtocolToolStripMenuItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.structDataGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(547, 426);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "结构";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // structDataGridView
            // 
            this.structDataGridView.AllowUserToAddRows = false;
            this.structDataGridView.AllowUserToDeleteRows = false;
            this.structDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.structDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.structDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.structIDColumn,
            this.structNameColumn,
            this.structSummaryColumn,
            this.Column3,
            this.Column4});
            this.structDataGridView.ContextMenuStrip = this.structContextMenu;
            this.structDataGridView.Location = new System.Drawing.Point(6, 6);
            this.structDataGridView.MultiSelect = false;
            this.structDataGridView.Name = "structDataGridView";
            this.structDataGridView.ReadOnly = true;
            this.structDataGridView.RowTemplate.Height = 23;
            this.structDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.structDataGridView.Size = new System.Drawing.Size(535, 414);
            this.structDataGridView.TabIndex = 0;
            this.structDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.structDataGridView_CellMouseDoubleClick);
            this.structDataGridView.SelectionChanged += new System.EventHandler(this.structDataGridView_SelectionChanged);
            // 
            // structIDColumn
            // 
            this.structIDColumn.DataPropertyName = "StructID";
            this.structIDColumn.HeaderText = "结构ID";
            this.structIDColumn.Name = "structIDColumn";
            this.structIDColumn.ReadOnly = true;
            this.structIDColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // structNameColumn
            // 
            this.structNameColumn.DataPropertyName = "StructName";
            this.structNameColumn.HeaderText = "结构名称";
            this.structNameColumn.Name = "structNameColumn";
            this.structNameColumn.ReadOnly = true;
            this.structNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // structSummaryColumn
            // 
            this.structSummaryColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.structSummaryColumn.DataPropertyName = "StructSummary";
            this.structSummaryColumn.HeaderText = "结构说明";
            this.structSummaryColumn.Name = "structSummaryColumn";
            this.structSummaryColumn.ReadOnly = true;
            this.structSummaryColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "SortIndex";
            this.Column3.HeaderText = "排序索引";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Visible = false;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ProjectID";
            this.Column4.HeaderText = "项目ID";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Visible = false;
            // 
            // structContextMenu
            // 
            this.structContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newStructToolStripMenuItem,
            this.editStructToolStripMenuItem,
            this.delStructToolStripMenuItem});
            this.structContextMenu.Name = "structContextMenu";
            this.structContextMenu.Size = new System.Drawing.Size(152, 70);
            // 
            // newStructToolStripMenuItem
            // 
            this.newStructToolStripMenuItem.Name = "newStructToolStripMenuItem";
            this.newStructToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.newStructToolStripMenuItem.Text = "新增结构(&N)...";
            this.newStructToolStripMenuItem.Click += new System.EventHandler(this.newStructToolStripMenuItem_Click);
            // 
            // editStructToolStripMenuItem
            // 
            this.editStructToolStripMenuItem.Name = "editStructToolStripMenuItem";
            this.editStructToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.editStructToolStripMenuItem.Text = "编辑结构(&E)...";
            this.editStructToolStripMenuItem.Click += new System.EventHandler(this.editStructToolStripMenuItem_Click);
            // 
            // delStructToolStripMenuItem
            // 
            this.delStructToolStripMenuItem.Name = "delStructToolStripMenuItem";
            this.delStructToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.delStructToolStripMenuItem.Text = "删除结构(&D)...";
            this.delStructToolStripMenuItem.Click += new System.EventHandler(this.delStructToolStripMenuItem_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lastViewButton);
            this.groupBox3.Controls.Add(this.viewButton);
            this.groupBox3.Controls.Add(this.verTextBox);
            this.groupBox3.Location = new System.Drawing.Point(3, 16);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(277, 51);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "版本";
            // 
            // lastViewButton
            // 
            this.lastViewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lastViewButton.Location = new System.Drawing.Point(196, 18);
            this.lastViewButton.Name = "lastViewButton";
            this.lastViewButton.Size = new System.Drawing.Size(75, 23);
            this.lastViewButton.TabIndex = 2;
            this.lastViewButton.Text = "最新(&R)";
            this.lastViewButton.UseVisualStyleBackColor = true;
            this.lastViewButton.Click += new System.EventHandler(this.lastViewButton_Click);
            // 
            // viewButton
            // 
            this.viewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.viewButton.Location = new System.Drawing.Point(115, 18);
            this.viewButton.Name = "viewButton";
            this.viewButton.Size = new System.Drawing.Size(75, 23);
            this.viewButton.TabIndex = 1;
            this.viewButton.Text = "查看(&V)";
            this.viewButton.UseVisualStyleBackColor = true;
            this.viewButton.Click += new System.EventHandler(this.viewButton_Click);
            // 
            // verTextBox
            // 
            this.verTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verTextBox.Location = new System.Drawing.Point(6, 20);
            this.verTextBox.Name = "verTextBox";
            this.verTextBox.Size = new System.Drawing.Size(103, 21);
            this.verTextBox.TabIndex = 0;
            this.verTextBox.TextChanged += new System.EventHandler(this.verTextBox_TextChanged);
            this.verTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.verTextBox_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.fieldsRichTextBox);
            this.groupBox2.Location = new System.Drawing.Point(3, 134);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 321);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "字段";
            // 
            // fieldsRichTextBox
            // 
            this.fieldsRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldsRichTextBox.Location = new System.Drawing.Point(6, 20);
            this.fieldsRichTextBox.Name = "fieldsRichTextBox";
            this.fieldsRichTextBox.ReadOnly = true;
            this.fieldsRichTextBox.Size = new System.Drawing.Size(320, 295);
            this.fieldsRichTextBox.TabIndex = 0;
            this.fieldsRichTextBox.Text = "";
            this.fieldsRichTextBox.WordWrap = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.searchButton);
            this.groupBox1.Controls.Add(this.searchTextBox);
            this.groupBox1.Location = new System.Drawing.Point(3, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "搜索";
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.Location = new System.Drawing.Point(251, 21);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "定位(&F)";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(6, 22);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(239, 21);
            this.searchTextBox.TabIndex = 0;
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // protocolIDColumn
            // 
            this.protocolIDColumn.DataPropertyName = "ProtocolID";
            this.protocolIDColumn.FillWeight = 70F;
            this.protocolIDColumn.HeaderText = "协议ID";
            this.protocolIDColumn.Name = "protocolIDColumn";
            this.protocolIDColumn.ReadOnly = true;
            this.protocolIDColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.protocolIDColumn.Width = 70;
            // 
            // protocolNameColumn
            // 
            this.protocolNameColumn.DataPropertyName = "ProtocolName";
            this.protocolNameColumn.FillWeight = 130F;
            this.protocolNameColumn.HeaderText = "协议名称";
            this.protocolNameColumn.Name = "protocolNameColumn";
            this.protocolNameColumn.ReadOnly = true;
            this.protocolNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.protocolNameColumn.Width = 130;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "ProtocolCode";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column5.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column5.FillWeight = 50F;
            this.Column5.HeaderText = "协议号";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 50;
            // 
            // Column
            // 
            this.Column.DataPropertyName = "FromClient";
            this.Column.FalseValue = "0";
            this.Column.FillWeight = 75F;
            this.Column.HeaderText = "Client请求";
            this.Column.Name = "Column";
            this.Column.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column.TrueValue = "1";
            this.Column.Width = 75;
            // 
            // protocolSummaryColumn
            // 
            this.protocolSummaryColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.protocolSummaryColumn.DataPropertyName = "ProtocolSummary";
            this.protocolSummaryColumn.HeaderText = "协议说明";
            this.protocolSummaryColumn.Name = "protocolSummaryColumn";
            this.protocolSummaryColumn.ReadOnly = true;
            this.protocolSummaryColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "SortIndex";
            this.Column1.HeaderText = "排序索引";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Visible = false;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "ProjectID";
            this.Column2.HeaderText = "项目ID";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Visible = false;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "OperateType";
            this.Column6.HeaderText = "操作";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 512);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.bottomStatusStrip);
            this.Controls.Add(this.topMenuStrip);
            this.MainMenuStrip = this.topMenuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 550);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "协议工具";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.topMenuStrip.ResumeLayout(false);
            this.topMenuStrip.PerformLayout();
            this.bottomStatusStrip.ResumeLayout(false);
            this.bottomStatusStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.psTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.protocolDataGridView)).EndInit();
            this.protocolContextMenu.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.structDataGridView)).EndInit();
            this.structContextMenu.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip topMenuStrip;
        private System.Windows.Forms.StatusStrip bottomStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel ResultStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem ProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel UserStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem NewProjToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SelectProjToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Timer globalTimer;
        private System.Windows.Forms.ToolStripMenuItem EditProjToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ProjMgrToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl psTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox fieldsRichTextBox;
        private System.Windows.Forms.ContextMenuStrip protocolContextMenu;
        private System.Windows.Forms.ToolStripMenuItem editProtocolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProtocolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem delProtocolToolStripMenuItem;
        private System.Windows.Forms.DataGridView structDataGridView;
        private System.Windows.Forms.ContextMenuStrip structContextMenu;
        private System.Windows.Forms.ToolStripMenuItem newStructToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editStructToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem delStructToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel ProjectStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem ProtocolToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RefreshProtocolToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem GenCodeToolStripMenuItem;
        private System.Windows.Forms.DataGridView protocolDataGridView;
        private System.Windows.Forms.ToolStripMenuItem GenSettingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn structIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn structNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn structSummaryColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button viewButton;
        private System.Windows.Forms.TextBox verTextBox;
        private System.Windows.Forms.Button lastViewButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn protocolIDColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn protocolNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column;
        private System.Windows.Forms.DataGridViewTextBoxColumn protocolSummaryColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}

