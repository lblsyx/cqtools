using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityLight.Loggers;

namespace TemplateTool
{
    public class TextBoxLogger : ILogger
    {
        private TextBox _textBox;

        public TextBoxLogger(TextBox textBox)
        {
            _textBox = textBox;
        }

        public void Print(LogLevel oLogLevel, string msg)
        {
            switch (oLogLevel)
            {
                case LogLevel.DEBUG:
                    _textBox.AppendText(string.Format("{0}\r\n", msg));
                    break;
                case LogLevel.INFO:
                    _textBox.AppendText(string.Format("{0}\r\n", msg));
                    break;
                case LogLevel.WARN:
                    _textBox.AppendText(string.Format("{0}\r\n", msg));
                    break;
                case LogLevel.ERROR:
                    _textBox.AppendText(string.Format("{0}\r\n", msg));
                    break;
                case LogLevel.FATAL:
                    _textBox.AppendText(string.Format("{0}\r\n", msg));
                    break;
            }
        }

        public void Log(LogLevel oLogLevel, string msg)
        {
            string content;
            switch (oLogLevel)
            {
                case LogLevel.DEBUG:
                    {
                        content = string.Format("[D]{0}\r\n", msg);
                        if (_textBox.Text.IndexOf(content) == -1)
                        {
                            _textBox.AppendText(content);
                        }
                    }
                    break;
                case LogLevel.INFO:
                    {
                        content = string.Format("[I]{0}\r\n", msg);
                        if (_textBox.Text.IndexOf(content) == -1)
                        {
                            _textBox.AppendText(content);
                        }
                    }
                    break;
                case LogLevel.WARN:
                    {
                        content = string.Format("[W]{0}\r\n", msg);
                        if (_textBox.Text.IndexOf(content) == -1)
                        {
                            _textBox.AppendText(content);
                        }
                    }
                    break;
                case LogLevel.ERROR:
                    {
                        content = string.Format("[E]{0}\r\n", msg);
                        if (_textBox.Text.IndexOf(content) == -1)
                        {
                            _textBox.AppendText(content);
                        }
                    }
                    break;
                case LogLevel.FATAL:
                    {
                        content = string.Format("[F]{0}\r\n", msg);
                        if (_textBox.Text.IndexOf(content) == -1)
                        {
                            _textBox.AppendText(content);
                        }
                    }
                    break;
            }
        }


        public void Clear()
        {
            _textBox.Text = string.Empty;
        }
    }
}
