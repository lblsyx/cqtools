#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSScriptApp.Scripts
{
    public class CompressMornPNG : IScriptMethod
    {
        public const bool COMPRESS = true;

        #region IScriptMethod 成员

        public object Do(params object[] args)
        {
            try
            {
                bool allSuccess = true;
                string dir = args[0] as string;
                string output = args[1] as string;
                Directory.CreateDirectory(output);

                //string tempDir = output;// Path.Combine(output, "Temp");
                //Directory.CreateDirectory(tempDir);
                string cmd = Path.Combine(Global.CurrentDirectory, "pngquant\\pngquant.exe");
                
                List<string> files = new List<string>();
                ScriptMethod.FindChildren(dir, files, "*.png");

                foreach (var item in files)
                {
                    string source = item;
                    string fileName = GetMornUIFileName(source, dir);
                    string target = Path.Combine(output, fileName) + ".png";

                    if (COMPRESS)
                    {
                        long oLen = ScriptMethod.GetFileLength(source);
                        string compressedFile = Path.GetFullPath("compressed.png");//Path.Combine(tempDir, "compressed.png");
                        Program.WriteToConsole("Compress file：{0}", source.Replace(dir, string.Empty).Substring(1));
                        if (ScriptMethod.ExecCommand(cmd, " --force --verbose -o compressed.png 256 \"" + source + "\""))
                        {
                            long nLen = ScriptMethod.GetFileLength(compressedFile);
                            if (nLen < oLen) source = compressedFile;
                        }
                        else
                        {
                            allSuccess = false;
                            Program.WriteToConsole("Compress failed!!!File：{0}", source);
                        }
                        //if (source.Replace(dir, string.Empty).Substring(1).IndexOf(" ") != -1)
                        //{
                        //    Program.WriteToConsole("Compress file：{0}", source.Replace(dir, string.Empty).Substring(1));
                        //}
                    }

                    File.Delete(target);
                    File.Copy(source, target);
                }

                //Directory.Delete(tempDir, true);
                return allSuccess;
            }
            catch (Exception ex)
            {
                Program.WriteToConsole(ex.Message);
                Program.WriteToConsole(ex.StackTrace);
                return false;
            }
        }

        #endregion

        public string GetMornUIFileName(string path, string root)
        {
            if (File.Exists(path) == false) return string.Empty;

            string ext = path.Substring(path.IndexOf(".") + 1);

            string temp = path.Replace(root, string.Empty);
            temp = temp.Replace("." + ext, string.Empty);
            temp = temp.Replace("\\", ".");
            temp = ext + temp;

            return temp;
        }
    }
}
#endif