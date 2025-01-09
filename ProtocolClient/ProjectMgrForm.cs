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
    public partial class ProjectMgrForm : ToolForm
    {
        public ProjectMgrForm()
        {
            InitializeComponent();
        }

        private void ProjMgrForm_Load(object sender, EventArgs e)
        {
            projectDataGridView.DataSource = Global.ProjectList;
        }

        private void newProjToolButton_Click(object sender, EventArgs e)
        {
            ProjectInfoForm oProjInfoForm = new ProjectInfoForm();
            oProjInfoForm.New();
            oProjInfoForm.ShowDialog(this);
        }

        private void editProjToolButton_Click(object sender, EventArgs e)
        {
            if (projectDataGridView.SelectedRows.Count != 0)
            {
                int idx = projectDataGridView.Rows.IndexOf(projectDataGridView.SelectedRows[0]);

                ProjectInfoForm oProjInfoForm = new ProjectInfoForm();
                oProjInfoForm.Edit(Global.SelectedProject);
                oProjInfoForm.ShowDialog(this);
            }
            else
            {
                MessageBox.Show("请选择项目后编辑!");
            }
        }

        private void projectDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            editProjToolButton_Click(sender, null);
        }

        private void delProjToolButton_Click(object sender, EventArgs e)
        {
            if (projectDataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择项目后删除!");
                return;
            }
            
            int idx = projectDataGridView.Rows.IndexOf(projectDataGridView.SelectedRows[0]);
            ProjectInfo oProjectInfo = Global.ProjectList[idx];
            if (ProtocolUtil.DelProject(oProjectInfo))
            {
                Global.DelProject(oProjectInfo);
                MessageBox.Show(string.Format("删除{0}成功！", oProjectInfo.ProjectName));
            }
        }
    }
}
