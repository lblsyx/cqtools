using ProtocolCore;
using ProtocolCore.Generates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtocolCore.Exports
{
    public interface IExport
    {
        string Name { get; }

        string OutputFilter { get; }

        PathType OutputPathType { get; }

        void Export(ProjectInfo oProjectInfo, string sPath);
    }
}
