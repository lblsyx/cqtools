using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityLight.Loggers;
using System.Windows.Forms;

namespace ProtocolClient
{
    public class MsgBoxLogger : ILogger
    {
        #region ILogger 成员

        public void Log(LogLevel oLogLevel, string msg)
        {
            MessageBox.Show(msg);
        }

        public void Clear()
        {
        }

        #endregion
    }
}
