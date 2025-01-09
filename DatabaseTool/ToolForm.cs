using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DatabaseTool
{
    public class ToolForm : Form
    {
        public const int WM_KEYDOWN = 256;

        public const int WM_SYSKEYDOWN = 260;

        protected bool mProcessPressESC = true;
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (mProcessPressESC)
            {
                if (msg.Msg == WM_KEYDOWN || msg.Msg == WM_SYSKEYDOWN)
                {
                    switch (keyData)
                    {
                        case Keys.Escape:
                            this.Close();
                            return false;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
