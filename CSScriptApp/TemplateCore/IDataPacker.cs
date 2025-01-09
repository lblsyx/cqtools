using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSScriptApp.TemplateCore
{
    public interface IDataPacker
    {
        void PackData(IList<TableInfo> schemas, string outPath, object others);
    }
}
