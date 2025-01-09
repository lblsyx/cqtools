#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSScriptApp.Scripts
{
    public class UnCompressSWF : IScriptMethod
    {
        #region IScriptMethod 成员

        public object Do(params object[] args)
        {
            try
            {
                string file = args[0] as string;
                string output = args[1] as string;

                if (File.Exists(file) == false)
                {
                    Program.WriteToConsole("找不到文件!!!File：{0}", file);
                    return false;
                }

                string tempDir = Path.Combine(Global.CurrentDirectory, "SWC");
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
                Directory.CreateDirectory(tempDir);
                string cmd = Path.Combine(Global.CurrentDirectory, "7z\\7z.exe");
                string arguments = " e " + file + " -o" + tempDir + " library.swf -y";
                bool rlt = ScriptMethod.ExecCommand(cmd, arguments);

                if (rlt)
                {
                    File.Copy(Path.Combine(tempDir, "library.swf"), output, true);
                }

                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }

                return rlt;
            }
            catch (Exception ex)
            {
                Program.WriteToConsole(ex.Message);
                Program.WriteToConsole(ex.StackTrace);
                return false;
            }
        }

        #endregion
    }
}
#endif