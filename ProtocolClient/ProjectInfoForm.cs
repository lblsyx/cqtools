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
    public partial class ProjectInfoForm : ToolForm
    {
        private bool mNewProject;
        private ProjectInfo mProjectInfo;

        public ProjectInfoForm()
        {
            InitializeComponent();
        }

        public void New()
        {
            mProjectInfo = new ProjectInfo();
            mNewProject = true;
            okButton.Enabled = true;
            groupBox1.Text = "新建";
            projNameTextBox.Text = string.Empty;
            projSummaryTextBox.Text = string.Empty;
        }

        public void Edit(ProjectInfo oProjectInfo)
        {
            mProjectInfo = oProjectInfo;

            mNewProject = false;
            okButton.Enabled = true;
            groupBox1.Text = "编辑";
            projNameTextBox.Text = mProjectInfo.ProjectName;
            packetVerTextBox.Text = mProjectInfo.PacketVer.ToString();
            projSummaryTextBox.Text = mProjectInfo.ProjectSummary;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(projNameTextBox.Text.Trim()))
            {
                MessageBox.Show("项目名称不能为空!");
                return;
            }

            if (mProjectInfo.ProjectName != projNameTextBox.Text.Trim())
            {
                if (ProtocolUtil.ProjectExists(projNameTextBox.Text.Trim()))
                {
                    return;
                }
            }

            //string oldName = mProjectInfo.ProjectName;
            //string oldSummary = mProjectInfo.ProjectSummary;
            mProjectInfo.ProjectName = projNameTextBox.Text.Trim();
            mProjectInfo.PacketVer = Convert.ToUInt16(packetVerTextBox.Text.Trim());
            mProjectInfo.ProjectSummary = projSummaryTextBox.Text.Trim();

            if (mNewProject)
            {
                if (ProtocolUtil.AddProject(mProjectInfo))
                {
                    Close();
                    return;
                }
            }
            else
            {
                if (ProtocolUtil.UpdateProject(mProjectInfo))
                {
                    Close();
                    return;
                }
                //else
                //{
                //    mProjectInfo.ProjectName = oldName;
                //    mProjectInfo.ProjectSummary = oldSummary;
                //}
            }
        }
    }
}
