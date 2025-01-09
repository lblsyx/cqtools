using DatabaseTool.DatabaseCore;
using DatabaseTool.DatabaseCore.Generates;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace DatabaseTool
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public void RefreshChooseDBMenuItem()
        {
            ChooseToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < Global.AppConfig.Connections.Count; i++)
            {
                ConnectionInfo oConnectionInfo = Global.AppConfig.Connections[i];

                ToolStripMenuItem oConnectionToolStripMenuItem = new ToolStripMenuItem();
                oConnectionToolStripMenuItem.Text = string.Format("{0}:{1}", oConnectionInfo.Host, oConnectionInfo.Port);
                oConnectionToolStripMenuItem.Tag = oConnectionInfo;
                oConnectionToolStripMenuItem.Click += oConnectionToolStripMenuItem_Click;
                ChooseToolStripMenuItem.DropDownItems.Add(oConnectionToolStripMenuItem);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Global.AppPaths.Load();
            Global.AppConfig.Load();

            DatabaseUtil.CompilerScripts();

            genStructToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < StructGeneratorMgr.StructGeneratorList.Count; i++)
            {
                IStructGenerator iIStructGenerator = StructGeneratorMgr.StructGeneratorList[i];
                ToolStripMenuItem oStructGeneratorToolStripMenuItem = new ToolStripMenuItem();
                oStructGeneratorToolStripMenuItem.Text = iIStructGenerator.Name;
                oStructGeneratorToolStripMenuItem.Tag = iIStructGenerator;
                oStructGeneratorToolStripMenuItem.Click += oStructGeneratorToolStripMenuItem_Click;
                genStructToolStripMenuItem.DropDownItems.Add(oStructGeneratorToolStripMenuItem);
            }

            RefreshChooseDBMenuItem();

            dataGridView1.DataSource = Global.DBInfoList;

            dataGridView2.DataSource = Global.TBInfoList;
        }

        private void oStructGeneratorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择数据库!");
                return;
            }

            DBInfo oDBInfo = dataGridView1.SelectedRows[0].DataBoundItem as DBInfo;

            List<TBInfo> selectedTBList = new List<TBInfo>();
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                TBInfo oTBInfo = dataGridView2.Rows[i].DataBoundItem as TBInfo;

                if (oTBInfo != null && oTBInfo.Selected)
                {
                    selectedTBList.Add(oTBInfo);
                }
            }

            ToolStripMenuItem oStructGeneratorToolStripMenuItem = sender as ToolStripMenuItem;
            IStructGenerator iIStructGenerator = oStructGeneratorToolStripMenuItem.Tag as IStructGenerator;

            GenCodeForm oGenCodeForm = new GenCodeForm();
            oGenCodeForm.SetGenerator(oDBInfo, selectedTBList.ToArray(), iIStructGenerator);
            oGenCodeForm.ShowDialog(this);
        }

        private void oConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem oConnectionToolStripMenuItem = sender as ToolStripMenuItem;
            ConnectionInfo oConnectionInfo = oConnectionToolStripMenuItem.Tag as ConnectionInfo;
            textBox1.Text = oConnectionInfo.Host;
            textBox2.Text = oConnectionInfo.Port.ToString();
            textBox3.Text = oConnectionInfo.User;
            textBox4.Text = oConnectionInfo.Pass;
            DBHelper.SetConnectionInfo(oConnectionInfo.Host, oConnectionInfo.Port, oConnectionInfo.User, oConnectionInfo.Pass, "mysql");
            DatabaseUtil.ReloadDatabaseList();
        }

        private void scanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScanDBForm oScanDBForm = new ScanDBForm();
            oScanDBForm.ShowDialog(this);
        }

        private void DatabaseMgrToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectionMgrForm oConnectionMgrForm = new ConnectionMgrForm();
            oConnectionMgrForm.ShowDialog(this);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {
                DBInfo oDBInfo = dataGridView1.SelectedRows[0].DataBoundItem as DBInfo;

                DatabaseUtil.ReloadTableList(oDBInfo);
            }
        }

        private void refreshDBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DatabaseUtil.ReloadDatabaseList();
        }

        private void clearSingleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("未选择数据库");
                return;
            }

            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("未选择要清除的数据表");
                return;
            }

            DBInfo oDBInfo = dataGridView1.SelectedRows[0].DataBoundItem as DBInfo;
            TBInfo oTBInfo = dataGridView2.SelectedRows[0].DataBoundItem as TBInfo;

            if (MessageBox.Show(string.Format("是否删除`{0}`.`{1}`表数据", oDBInfo.DBName, oTBInfo.TBName), "删除数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                DatabaseUtil.DeleteTable(oDBInfo, oTBInfo);
                MessageBox.Show(string.Format("`{0}`.`{1}`数据删除成功!", oDBInfo.DBName, oTBInfo.TBName));
            }
        }

        private void truncateSingleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("未选择数据库");
                return;
            }

            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("未选择要清除的数据表");
                return;
            }

            DBInfo oDBInfo = dataGridView1.SelectedRows[0].DataBoundItem as DBInfo;
            TBInfo oTBInfo = dataGridView2.SelectedRows[0].DataBoundItem as TBInfo;

            if (MessageBox.Show(string.Format("是否删减`{0}`.`{1}`数据表", oDBInfo.DBName, oTBInfo.TBName), "删减表", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                DatabaseUtil.DeleteTable(oDBInfo, oTBInfo);
                DatabaseUtil.TruncateTable(oDBInfo, oTBInfo);
                MessageBox.Show(string.Format("删减`{0}`.`{1}`成功!", oDBInfo.DBName, oTBInfo.TBName));
            }
        }

        private void clearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("未选择数据库");
                return;
            }

            DBInfo oDBInfo = dataGridView1.SelectedRows[0].DataBoundItem as DBInfo;
            
            if (MessageBox.Show(string.Format("是否删除`{0}`所有表数据", oDBInfo.DBName), "删除数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                DatabaseUtil.DeleteTable(oDBInfo, Global.TBInfoList.ToArray());
                MessageBox.Show("删除所有表数据成功!");
            }
        }

        private void truncateAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("未选择数据库");
                return;
            }

            DBInfo oDBInfo = dataGridView1.SelectedRows[0].DataBoundItem as DBInfo;

            if (MessageBox.Show(string.Format("是否删减`{0}`所有表数据", oDBInfo.DBName), "删除数据", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
            {
                DatabaseUtil.DeleteTable(oDBInfo, Global.TBInfoList.ToArray());
                DatabaseUtil.TruncateTable(oDBInfo, Global.TBInfoList.ToArray());
                MessageBox.Show("删减所有表数据成功!");
            }
        }
    }
}
