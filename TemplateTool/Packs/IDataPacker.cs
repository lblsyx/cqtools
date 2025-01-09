using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TemplateTool.Datas;

namespace TemplateTool.Packs
{
    public interface IDataPacker
    {
        void PackData(IList<TableInfo> schemas, string outPath, object others, string errorlogPath);
        bool initData(string outPath, object others, bool needDelete);
    }
}
