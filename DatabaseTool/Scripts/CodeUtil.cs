using DatabaseTool.DatabaseCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DatabaseTool.Scripts
{
    /// <summary>
    /// 解析JSON
    /// </summary>
    public static class CodeUtil
    {
        /// <summary>
        /// 数据表名称转换为类名;
        /// </summary>
        /// <param name="tbname"></param>
        /// <returns></returns>
        public static string TBName2CLSName(string tbname)
        {
            if (string.IsNullOrEmpty(tbname))
            {
                return tbname;
            }

            return /*"TB" + */tbname.Substring(0, 1).ToUpper() + tbname.Substring(1);
        }

        public static string ColumnTypeDefaultValue(string columnName, string columnType, int len)
        {
            switch (columnType.ToLower())
            {
                case "bit":
                    return string.Format("{0} = false;", columnName);
                case "int":
                case "bigint":
                case "integer":
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "float":
                case "double":
                case "numeric":
                case "decimal":
                case "date":
                case "datetime":
                case "timestamp":
                    return string.Format("{0} = 0;", columnName);
                case "char":
                case "varchar":
                    return string.Format("memset({0}, 0, {1});", columnName, len);
                case "text":
                case "tinytext":
                case "longtext":
                case "mediumtext":
                    return string.Format("{0} = \"\";", columnName);

                case "blob":
                case "tinyblob":
                case "longblob":
                case "mediumblob":
                case "binary":
                case "varbinary":
                    return string.Format("{0}.clear();", columnName);
            }

            return string.Format("{0} = unknow;", columnName);
        }

        public static bool IsBinColumn(string columnType)
        {
            switch (columnType.ToLower())
            {
                case "blob":
                case "tinyblob":
                case "longblob":
                case "mediumblob":
                case "binary":
                case "varbinary":
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 数据库类型转换为C++类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static string ColumnField2CPPField(string columnName, string dataType, bool unsign, int len)
        {
            switch (dataType.ToLower())
            {
                case "bit":
                    return string.Format("bool {0};", columnName);

                case "int":
                case "integer":
                case "tinyint":
                case "smallint":
                case "mediumint":
                    if (unsign)
                    {
                        return string.Format("uint {0};", columnName);
                    }
                    else
                    {
                        return string.Format("int {0};", columnName);
                    }

                case "bigint":
                    if (unsign)
                    {
                        return string.Format("uint64 {0};", columnName);
                    }
                    else
                    {
                        return string.Format("int64 {0};", columnName);
                    }

                case "float":
                    return string.Format("float {0};", columnName);

                case "double":
                case "numeric":
                case "decimal":
                    return string.Format("double {0};", columnName);

                case "char":
                case "varchar":
                    return string.Format("char {0}[{1}];", columnName, len);

                case "text":
                case "tinytext":
                case "longtext":
                case "mediumtext":
                    return string.Format("std::string {0};", columnName);

                case "blob":
                case "tinyblob":
                case "longblob":
                case "mediumblob":
                case "binary":
                case "varbinary":
                    return string.Format("std::vector<uchar> {0};", columnName);

                case "date":
                case "datetime":
                case "timestamp":
                    return string.Format("time_t {0};", columnName);
            }

            return string.Format("unknow {0};", columnName);
        }

        /// <summary>
        /// 数据库类型转换为占位符
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public static string ColumnField2PlaceHolder(string dataType, bool unsign)
        {
            switch (dataType.ToLower())
            {
                case "bit":
                    return "%d";

                case "int":
                case "integer":
                case "tinyint":
                case "smallint":
                case "mediumint":
                    if (unsign)
                    {
                        return "%u";
                    }
                    else
                    {
                        return "%d";
                    }

                case "bigint":
                    if (unsign)
                    {
                        return "%llu";
                    }
                    else
                    {
                        return "%lld";
                    }

                case "float":
                case "double":
                case "numeric":
                case "decimal":
                    return "%f";

                case "char":
                case "varchar":
                    return "%s";

                case "text":
                case "tinytext":
                case "longtext":
                case "mediumtext":
                    return "'%s'";

                case "blob":
                case "tinyblob":
                case "longblob":
                case "mediumblob":
                case "binary":
                case "varbinary":
                    return "?";

                case "date":
                case "datetime":
                case "timestamp":
                    return "%lld";
            }

            return "?";
        }

        /// <summary>
        /// 数据库类型转换为C++框架的SQL枚举类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="columnType"></param>
        /// <param name="extraType"></param>
        /// <returns></returns>
        public static string ColumnType2CPPSQLType(string dataType, string columnType, string extraType, string columnKey)
        {
            bool unsigned = false;
            string rlt = "eFieldTypeUnknow";

            switch (dataType.ToLower())
            {
                case "bit":
                    rlt = "eFieldTypeBool";
                    break;

                case "int":
                case "integer":
                case "tinyint":
                case "smallint":
                case "mediumint":
                    if (columnType.IndexOf("unsigned") != -1)
                    {
                        rlt = "eFieldTypeUInt";
                    }
                    else
                    {
                        rlt = "eFieldTypeInt";
                    }
                    break;

                case "bigint":
                    if (columnType.IndexOf("unsigned") != -1)
                    {
                        rlt = "eFieldTypeUInt64";
                    }
                    else
                    {
                        rlt = "eFieldTypeInt64";
                    }
                    break;

                case "float":
                    rlt = "eFieldTypeFloat";
                    break;

                case "double":
                case "numeric":
                case "decimal":
                    rlt = "eFieldTypeDouble";
                    break;

                case "char":
                case "varchar":
                    rlt = "eFieldTypeString";
                    break;

                case "text":
                case "tinytext":
                case "longtext":
                case "mediumtext":
                    rlt = "eFieldTypeText";
                    break;

                case "blob":
                case "tinyblob":
                case "longblob":
                case "mediumblob":
                case "binary":
                case "varbinary":
                    rlt = "eFieldTypeBin";
                    break;

                case "datetime":
                case "timestamp":
                    rlt = "eFieldTypeDateTime";
                    break;
            }

            if (columnKey.ToUpper().IndexOf("PRI") != -1)
            {
                rlt += " | eFieldTypePK";
            }

            if (extraType.ToLower().IndexOf("auto_increment") != -1)
            {
                rlt += " | eFieldTypeAuto";
            }

            return rlt;
        }
    }
}