using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseTool.DatabaseCore
{
    public class ColumnInfo
    {
        public string ColumnName { get; set; }

        public string ColumnType { get; set; }

        public string DataType { get; set; }

        public int TypeMaxLength { get; set; }

        public string Comment { get; set; }

        public string Extra { get; set; }
    }
}
