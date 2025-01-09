using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateTool.Datas;

namespace TemplateTool.Gens
{
    public interface IGenerator
    {
        bool UseFolder { get; }

        bool RequireSecondCode { get; }

        void Generate(IList<TableInfo> schemas, string outPath, string outPath2, object others);
    }
}
