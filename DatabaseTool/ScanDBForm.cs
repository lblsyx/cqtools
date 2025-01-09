using DatabaseTool.DatabaseCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace DatabaseTool
{
    public partial class ScanDBForm : ToolForm
    {
        private BindingList<ConnectionInfo> mConnections = new BindingList<ConnectionInfo>();

        private byte start1;
        private byte start2;
        private byte start3;
        private byte start4;

        private byte end1;
        private byte end2;
        private byte end3;
        private byte end4;

        private int port;

        private int i1;
        private int i2;
        private int i3;
        private int i4;

        public ScanDBForm()
        {
            InitializeComponent();
        }

        private void ScanDBForm_Load(object sender, EventArgs e)
        {
            scanResultDataGridView.DataSource = mConnections;
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            string[] startIP = textBox1.Text.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            string[] endIP = textBox2.Text.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);

            if (startIP.Length != 4 || endIP.Length != 4)
            {
                MessageBox.Show("请输入正确的IP地址!");
                return;
            }

            port = Convert.ToInt32(textBox3.Text.Trim());
            if (port <= 0) return;

            start1 = Convert.ToByte(startIP[0]);
            start2 = Convert.ToByte(startIP[1]);
            start3 = Convert.ToByte(startIP[2]);
            start4 = Convert.ToByte(startIP[3]);

            end1 = Convert.ToByte(endIP[0]);
            end2 = Convert.ToByte(endIP[1]);
            end3 = Convert.ToByte(endIP[2]);
            end4 = Convert.ToByte(endIP[3]);

            i1 = start1;
            i2 = start2;
            i3 = start3;
            i4 = start4;

            int diffCount = 0;
            if (start4 != end4) diffCount += 1;
            if (start3 != end3) diffCount += 1;
            if (start2 != end2) diffCount += 1;
            if (start1 != end1) diffCount += 1;
            if (diffCount != 1)
            {
                MessageBox.Show("请输入正确的IP地址段,仅允许一个数字变化!");
                return;
            }

            mConnections.Clear();

            scanTimer.Start();

            scanButton.Enabled = false;
        }

        private void scanTimer_Tick(object sender, EventArgs e)
        {
            string str = string.Format("{0}.{1}.{2}.{3}", i1, i2, i3, i4);

            if (DatabaseUtil.HasListenPort(str, port))
            {
                ConnectionInfo oConnectionInfo = new ConnectionInfo();
                oConnectionInfo.Host = str;
                oConnectionInfo.Port = port;
                mConnections.Add(oConnectionInfo);
            }

            if (start4 != end4)
            {
                i4 += 1;
            }
            else if (start3 != end3)
            {
                i3 += 1;
            }
            else if (start2 != end2)
            {
                i2 += 1;
            }
            else if (start1 != end1)
            {
                i1 += 1;
            }

            if (i1 > end1 || i2 > end2 || i3 > end3 || i4 > end4)
            {
                scanTimer.Stop();
                scanButton.Enabled = true;
            }
        }
    }
}
