using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSScriptApp.ProtocolCore.GenProtocols;

namespace CSScriptApp.ProtocolCore.Exports
{
    public interface IExport
    {
        string Name { get; }

        string OutputFilter { get; }

        PathType OutputPathType { get; }

        void Export(ProjectInfo oProjectInfo, string sPath);
    }
}
