using DatabaseTool.DatabaseCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatabaseTool
{
    public partial class ConnectionMgrForm : ToolForm
    {
        private bool mEditted = false;
        private ConnectionInfo mConnectionInfo;

        public ConnectionMgrForm()
        {
            InitializeComponent();
        }

        private void ConnectionMgrForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = Global.AppConfig.Connections;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (mEditted)
            {
                mConnectionInfo.Host = hostTextBox.Text;
                mConnectionInfo.Port = Convert.ToInt32(portTextBox.Text);
                mConnectionInfo.User = userTextBox.Text;
                mConnectionInfo.Pass = passTextBox.Text;
            }
            else
            {
                if (DatabaseUtil.HasListenPort(hostTextBox.Text, Convert.ToInt32(portTextBox.Text)) == false)
                {
                    MessageBox.Show("连接测试失败!");
                    return;
                }

                for (int i = 0; i < Global.AppConfig.Connections.Count; i++)
                {
                    var item = Global.AppConfig.Connections[i];
                    if (item.Host == hostTextBox.Text && item.Port == Convert.ToInt32(portTextBox.Text) && item.User == userTextBox.Text)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[0];
                        MessageBox.Show("已存在相同的连接信息!");
                        return;
                    }
                }

                ConnectionInfo oConnectionInfo = new ConnectionInfo();
                oConnectionInfo.Host = hostTextBox.Text;
                oConnectionInfo.Port = Convert.ToInt32(portTextBox.Text);
                oConnectionInfo.User = userTextBox.Text;
                oConnectionInfo.Pass = passTextBox.Text;
                Global.AppConfig.Connections.Add(oConnectionInfo);
            }
            Global.AppConfig.Save();

            reset();

            Global.MainForm.RefreshChooseDBMenuItem();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (mEditted) reset();
        }

        private void reset()
        {
            mEditted = false;
            mConnectionInfo = null;
            hostTextBox.Text = "127.0.0.1";
            portTextBox.Text = "3306";
            userTextBox.Text = "root";
            passTextBox.Text = string.Empty;
            cancelButton.Visible = false;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                mEditted = true;
                mConnectionInfo = Global.AppConfig.Connections[e.RowIndex];

                hostTextBox.Text = mConnectionInfo.Host;
                portTextBox.Text = mConnectionInfo.Port.ToString();
                userTextBox.Text = mConnectionInfo.User;
                passTextBox.Text = mConnectionInfo.Pass;
                cancelButton.Visible = true;
            }
        }
    }
}
