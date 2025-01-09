using ProtocolCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolCore.Imports
{
    public interface IImport
    {
        string Name { get; }

        string InputFilter { get; }

        PathType InputPathType { get; }

        ProjectInfo Import(string sPath);
    }
}
