using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TemplateTool.Datas
{
    public class FieldInfo
    {
        public string FieldType;
        public string FieldName;
        public int    FieldIndex;
        public string FieldSummary;
        public string FieldLength;

        public List<object> FieldValues = new List<object>();
    }

    public class TableInfo
    {
        public string ExcelFile = string.Empty;
        public string TableName = string.Empty;
        public string SheetName = string.Empty;
        public string TableSummary = string.Empty;
        public string FieldLength = string.Empty;
        public uint RowCount = 0;
        public IList<FieldInfo> TableFields = new List<FieldInfo>();
    }
}
