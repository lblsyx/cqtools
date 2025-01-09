#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CSScriptApp.Scripts
{
    public class SVNCOUP : IScriptMethod
    {
        #region IScriptMethod 成员

        public object Do(params object[] args)
        {
            try
            {
                string svnFolder = args[0] as string;
                string svnUrl = args[1] as string;
                string svnUser = args[2] as string;
                string svnPass = args[3] as string;
                bool bRecursive = false;
                if (args.Length == 5)
                {
                    bRecursive = (bool)args[4];
                }

                string arguments = string.Empty;
                string cmd = Path.Combine(Global.CurrentDirectory, "svn/svn.exe");

                if (Directory.Exists(svnFolder))
                {
                    arguments = string.Format(" up {0} -q{3} -r HEAD --username {1} --password {2}", svnFolder, svnUser, svnPass, bRecursive ? "" : " -N");
                }
                else
                {
                    arguments = string.Format(" co {0} {1} -q{4} -r HEAD --username {2} --password {3}", svnUrl, svnFolder, svnUser, svnPass, bRecursive ? "" : " -N");
                }

                return ScriptMethod.ExecCommand(cmd, arguments);
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