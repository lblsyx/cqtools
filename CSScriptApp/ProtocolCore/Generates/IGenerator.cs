using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CSScriptApp.ProtocolCore.GenProtocols;

namespace CSScriptApp.ProtocolCore.Generates
{
    public interface IGenerator
    {
        string Name { get; }

        string ReplaceStr { get; }

        PathType Path1Type { get; }

        string Path1Filter { get; }

        PathType Path2Type { get; }

        string Path2Filter { get; }

        void Generate(ProjectInfo oProjectInfo, GeneratorSetting oGeneratorSetting, string sPath1, string sPath2);
    }
}
