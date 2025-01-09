using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseTool.DatabaseCore.Generates
{
    public interface IStructGenerator
    {
        string Type { get; }

        string Name { get; }

        string ReplaceStr { get; }

        string OutputFilter1 { get; }

        PathType OutputPathType1 { get; }

        string OutputFilter2 { get; }

        PathType OutputPathType2 { get; }

        void Generate(DBInfo oDBInfo, TBInfo[] oTBInfos, GeneratorSetting oGeneratorSetting, string sPath1, string sPath2);
    }
}
