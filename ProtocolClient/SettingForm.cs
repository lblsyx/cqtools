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
    public partial class SettingForm : ToolForm
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private void DBSettingForm_Load(object sender, EventArgs e)
        {
            nameTextBox.Text = Global.AppConfig.Name;
            hostTextBox.Text = Global.AppConfig.SQLHost;
            portTextBox.Text = Global.AppConfig.SQLPort.ToString();
            userTextBox.Text = Global.AppConfig.SQLUser;
            passwdTextBox.Text = Global.AppConfig.SQLPass;
            dbTextBox.Text = Global.AppConfig.SQLDB;
        }

        private void portTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) == false && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Global.AppConfig.Name = nameTextBox.Text.Trim();
            Global.AppConfig.SQLHost = hostTextBox.Text.Trim();
            Global.AppConfig.SQLPort = Convert.ToInt32(portTextBox.Text.Trim());
            Global.AppConfig.SQLUser = userTextBox.Text.Trim();
            Global.AppConfig.SQLPass = passwdTextBox.Text.Trim();
            Global.AppConfig.SQLDB = dbTextBox.Text.Trim();
            Global.AppConfig.Save();
            Close();
        }
    }
}
