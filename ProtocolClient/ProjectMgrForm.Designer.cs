namespace ProtocolClient
{
    partial class ProjectMgrForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectMgrForm));
            this.projMgrToolStrip = new System.Windows.Forms.ToolStrip();
            this.newProjToolButton = new System.Windows.Forms.ToolStripButton();
            this.editProjToolButton = new System.Windows.Forms.ToolStripButton();
            this.delProjToolButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.searchToolButton = new System.Windows.Forms.ToolStripButton();
            this.projectDataGridView = new System.Windows.Forms.DataGridView();
            this.ProjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.packetVer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectSummary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.projMgrToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // projMgrToolStrip
            // 
            this.projMgrToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjToolButton,
            this.editProjToolButton,
            this.delProjToolButton,
            this.toolStripSeparator1,
            this.toolStripTextBox1,
            this.searchToolButton});
            this.projMgrToolStrip.Location = new System.Drawing.Point(0, 0);
            this.projMgrToolStrip.Name = "projMgrToolStrip";
            this.projMgrToolStrip.Size = new System.Drawing.Size(584, 25);
            this.projMgrToolStrip.TabIndex = 0;
            this.projMgrToolStrip.Text = "toolStrip1";
            // 
            // newProjToolButton
            // 
            this.newProjToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newProjToolButton.Image = global::ProtocolClient.Properties.Resources._new;
            this.newProjToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newProjToolButton.Name = "newProjToolButton";
            this.newProjToolButton.Size = new System.Drawing.Size(23, 22);
            this.newProjToolButton.Text = "新建...";
            this.newProjToolButton.Click += new System.EventHandler(this.newProjToolButton_Click);
            // 
            // editProjToolButton
            // 
            this.editProjToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.editProjToolButton.Image = global::ProtocolClient.Properties.Resources.edit;
            this.editProjToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.editProjToolButton.Name = "editProjToolButton";
            this.editProjToolButton.Size = new System.Drawing.Size(23, 22);
            this.editProjToolButton.Text = "编辑...";
            this.editProjToolButton.Click += new System.EventHandler(this.editProjToolButton_Click);
            // 
            // delProjToolButton
            // 
            this.delProjToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.delProjToolButton.Image = global::ProtocolClient.Properties.Resources.del;
            this.delProjToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.delProjToolButton.Name = "delProjToolButton";
            this.delProjToolButton.Size = new System.Drawing.Size(23, 22);
            this.delProjToolButton.Text = "删除...";
            this.delProjToolButton.Click += new System.EventHandler(this.delProjToolButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(200, 25);
            // 
            // searchToolButton
            // 
            this.searchToolButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchToolButton.Image = ((System.Drawing.Image)(resources.GetObject("searchToolButton.Image")));
            this.searchToolButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.searchToolButton.Name = "searchToolButton";
            this.searchToolButton.Size = new System.Drawing.Size(23, 22);
            this.searchToolButton.Text = "搜索";
            // 
            // projectDataGridView
            // 
            this.projectDataGridView.AllowUserToAddRows = false;
            this.projectDataGridView.AllowUserToDeleteRows = false;
            this.projectDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.projectDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProjectID,
            this.ProjectName,
            this.packetVer,
            this.ProjectSummary});
            this.projectDataGridView.Location = new System.Drawing.Point(12, 28);
            this.projectDataGridView.Name = "projectDataGridView";
            this.projectDataGridView.RowTemplate.Height = 23;
            this.projectDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.projectDataGridView.Size = new System.Drawing.Size(560, 321);
            this.projectDataGridView.TabIndex = 1;
            this.projectDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.projectDataGridView_CellMouseDoubleClick);
            // 
            // ProjectID
            // 
            this.ProjectID.DataPropertyName = "ProjectID";
            this.ProjectID.Frozen = true;
            this.ProjectID.HeaderText = "项目ID";
            this.ProjectID.Name = "ProjectID";
            this.ProjectID.ReadOnly = true;
            this.ProjectID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProjectName
            // 
            this.ProjectName.DataPropertyName = "ProjectName";
            this.ProjectName.Frozen = true;
            this.ProjectName.HeaderText = "项目名称";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // packetVer
            // 
            this.packetVer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.packetVer.DataPropertyName = "PacketVer";
            this.packetVer.HeaderText = "协议版本号";
            this.packetVer.Name = "packetVer";
            this.packetVer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProjectSummary
            // 
            this.ProjectSummary.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ProjectSummary.DataPropertyName = "ProjectSummary";
            this.ProjectSummary.HeaderText = "项目说明";
            this.ProjectSummary.Name = "ProjectSummary";
            this.ProjectSummary.ReadOnly = true;
            this.ProjectSummary.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProjectMgrForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.projectDataGridView);
            this.Controls.Add(this.projMgrToolStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectMgrForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "项目列表";
            this.Load += new System.EventHandler(this.ProjMgrForm_Load);
            this.projMgrToolStrip.ResumeLayout(false);
            this.projMgrToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.projectDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip projMgrToolStrip;
        private System.Windows.Forms.ToolStripButton newProjToolButton;
        private System.Windows.Forms.ToolStripButton editProjToolButton;
        private System.Windows.Forms.ToolStripButton delProjToolButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripButton searchToolButton;
        private System.Windows.Forms.DataGridView projectDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn packetVer;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectSummary;
    }
}