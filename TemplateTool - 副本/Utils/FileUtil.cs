using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TemplateTool.Utils
{
    public class FileUtil
    {
        public static string BrowseFile(string filter = null, string initDir = null)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.RestoreDirectory = true;

            if (string.IsNullOrEmpty(initDir) == false)
            {
                ofd.InitialDirectory = initDir;
            }

            if (string.IsNullOrEmpty(filter) == false)
            {
                ofd.Filter = filter;
            }

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                return ofd.FileName;
            }

            return string.Empty;
        }
        public static string SelectOneFile(string title, string openDisk,string filter)
        {
            string file = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;      //该值确定是否可以选择多个文件
            dialog.Title = "选择文件";     //弹窗的标题
            if (string.IsNullOrEmpty(title) == false)
            {
                dialog.Title = title;
            }
            if (string.IsNullOrEmpty(openDisk) == false)
            {
                dialog.InitialDirectory = openDisk;
            }
            if (string.IsNullOrEmpty(filter) == false)
            {
                dialog.Filter = filter;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                file = dialog.FileName;
            }
            return file;
        }

        public static string BrowseFolder(string description = null)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (string.IsNullOrEmpty(description) == false)
            {
                dialog.Description = description;
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                return dialog.SelectedPath;
            }

            return string.Empty;
        }

        public static byte[] ReadFileBytes(string path)
        {
            try
            {
                byte[] buffer = new byte[1024];

                using (MemoryStream ms = new MemoryStream())
                {
                    using (FileStream fs = File.OpenRead(path))
                    {
                        while (true)
                        {
                            int len = fs.Read(buffer, 0, buffer.Length);
                            if (len > 0) ms.Write(buffer, 0, len);
                            else break;
                        }

                        return ms.ToArray();
                    }
                }
            }
            catch// (Exception ex)
            {
                return new byte[0];
            }
        }

        public static bool WriteFileBytes(string path, byte[] bytes, int offset = 0)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Write(bytes, offset, bytes.Length - offset);
                    fs.Flush();
                }

                return true;
            }
            catch// (Exception ex)
            {
                return false;
            }
        }
    }
}
