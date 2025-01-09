using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSScriptApp.ProtocolCore.GenProtocols;

namespace CSScriptApp.ProtocolCore.Imports
{
    public interface IImport
    {
        string Name { get; }

        string InputFilter { get; }

        PathType InputPathType { get; }

        ProjectInfo Import(string sPath);
    }
}
