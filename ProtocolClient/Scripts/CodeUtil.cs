using ProtocolCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ProtocolClient.Scripts
{
    /// <summary>
    /// 解析JSON
    /// </summary>
    public static class CodeUtil
    {
        /// <summary>
        /// 计算相对路径
        /// 后者相对前者的路径。
        /// </summary>
        /// <param name="mainDir">主目录</param>
        /// <param name="fullFilePath">文件的绝对路径</param>
        /// <returns>fullFilePath相对于mainDir的路径</returns>
        /// <example>
        /// @"..\..\regedit.exe" = GetRelativePath(@"D:\Windows\Web\Wallpaper\", @"D:\Windows\regedit.exe" );
        /// </example>
        public static string GetRelativePath(string mainDir, string fullFilePath)
        {
            if (!mainDir.EndsWith("\\"))
            {
                mainDir += "\\";
            }

            int intIndex = -1, intPos = mainDir.IndexOf('\\');

            while (intPos >= 0)
            {
                intPos++;
                if (string.Compare(mainDir, 0, fullFilePath, 0, intPos, true) != 0) break;
                intIndex = intPos;
                intPos = mainDir.IndexOf('\\', intPos);
            }

            if (intIndex >= 0)
            {
                fullFilePath = fullFilePath.Substring(intIndex);
                intPos = mainDir.IndexOf("\\", intIndex);
                while (intPos >= 0)
                {
                    fullFilePath = "..\\" + fullFilePath;
                    intPos = mainDir.IndexOf("\\", intPos + 1);
                }
            }

            return fullFilePath;
        }

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
                    return string.Format("ServerLight::Stream {0};", columnName);

                case "date":
                case "datetime":
                case "timestamp":
                    return string.Format("time_t {0};", columnName);
            }

            return string.Format("unknow {0};", columnName);
        }

        /// <summary>
        /// 数据库类型转换为C++框架的SQL枚举类型
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="columnType"></param>
        /// <param name="extraType"></param>
        /// <returns></returns>
        public static string ColumnType2CPPSQLType(string dataType, string columnType, string extraType)
        {
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
                    rlt = "eFieldTypeInt";
                    break;

                case "bigint":
                    rlt = "eFieldTypeInt64";
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

            if (columnType.ToUpper().IndexOf("PRI") != -1)
            {
                rlt += " | eFieldTypePK";
            }

            if (extraType.ToLower().IndexOf("auto_increment") != -1)
            {
                rlt += " | eFieldTypeAuto";
            }

            return rlt;
        }

        public static string FieldData2CtorField(FieldInfo fieldData, int nBSCount)
        {
            string rlt = string.Empty;//.Format("unknow {0};", fieldData.FieldName);
            string sBlankSpace = string.Empty;
            for (int i = 0; i < nBSCount; i++)
            {
                sBlankSpace += " ";
            }
            switch (fieldData.FieldType)
            {
                case "string":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} = \"\";//{1};", fieldData.FieldName, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength != "*")
                        {
                            rlt = string.Format("for (int i = 0; i < {0}; i++)\r\n", fieldData.FieldLength);
                            rlt += "    {\r\n";
                            rlt += string.Format("        {0}[i] = \"\";//{1}\r\n", fieldData.FieldName, fieldData.FieldSummary);
                            rlt += "    }";
                        }
                        else
                        {
                            //rlt = string.Empty;
                            rlt = string.Format("{0}.clear();//{1}", fieldData.FieldName, fieldData.FieldSummary);
                        }
                    }
                    break;

                case "stream":
                    //rlt = string.Empty;
                    rlt = string.Format("{0}.ClearStream();//{1}", fieldData.FieldName, fieldData.FieldSummary);
                    break;
                case "map":
                    rlt = string.Format("{0}.clear();//{1}", fieldData.FieldName, fieldData.FieldSummary);
                    break;
                case "bool":
                case "char":
                case "uchar":
                case "short":
                case "ushort":
                case "int":
                case "uint":
                case "int64":
                case "uint64":
                case "float":
                case "double":
                    //default:
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} = 0;//{1};", fieldData.FieldName, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength != "*")
                        {
                            rlt = string.Format("memset({0}, 0, sizeof({1}) * {2});//{3}", fieldData.FieldName, fieldData.FieldType, fieldData.FieldLength, fieldData.FieldSummary);
                        }
                        else
                        {
                            //rlt = string.Empty;
                            rlt = string.Format("{0}.clear();//{1}", fieldData.FieldName, fieldData.FieldSummary);
                        }
                    }
                    break;
                default://结构体变量
                    {
                        if (string.IsNullOrEmpty(fieldData.FieldLength))
                        {
                            rlt = string.Format("{0}.reset();//{1};", fieldData.FieldName, fieldData.FieldSummary);
                        }
                        else
                        {
                            if (fieldData.FieldLength != "*")
                            {
                                rlt = string.Format("for (int i = 0; i < {2}; i++)\r\n", fieldData.FieldName, fieldData.FieldType, fieldData.FieldLength, fieldData.FieldSummary);
                                rlt += "    {\r\n";
                                rlt += string.Format("        {0}[i].reset();\r\n", fieldData.FieldName);
                                rlt += "    }";
                            }
                            else
                            {
                                //rlt = string.Empty;
                                rlt = string.Format("{0}.clear();//{1}", fieldData.FieldName, fieldData.FieldSummary);
                            }
                        }
                    }
                    break;
            }

            return sBlankSpace + rlt;
        }

        public static string FieldData2CloneField(FieldInfo fieldData, int nBSCount)
        {
            string rlt = string.Empty;//.Format("unknow {0};", fieldData.FieldName);
            string sBlankSpace = string.Empty;
            for (int i = 0; i < nBSCount; i++)
            {
                sBlankSpace += " ";
            }
            switch (fieldData.FieldType)
            {
                case "string":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("pkg->{0} = {0};//{1};", fieldData.FieldName, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength != "*")
                        {
                            rlt = string.Format("for (int i = 0; i < {0}; i++)\r\n", fieldData.FieldLength);
                            rlt += "    {\r\n";
                            rlt += string.Format("        pkg->{0}[i] = {0}[i];//{1}\r\n", fieldData.FieldName, fieldData.FieldSummary);
                            rlt += "    }";
                        }
                        else
                        {
                            rlt = string.Format("for (size_t i = 0; i < {0}.size(); i++)\r\n", fieldData.FieldName);
                            rlt += "    {\r\n";
                            rlt += string.Format("        pkg->{0}.push_back({0}[i]);//{1}\r\n", fieldData.FieldName, fieldData.FieldSummary);
                            rlt += "    }";
                        }
                    }
                    break;

                case "stream":
                    //rlt = string.Empty;
                    //rlt = string.Format("{0}.ClearStream();//{1}", fieldData.FieldName, fieldData.FieldSummary);
                    break;

                case "bool":
                case "char":
                case "uchar":
                case "short":
                case "ushort":
                case "int":
                case "uint":
                case "int64":
                case "uint64":
                case "float":
                case "double":
                default:
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("pkg->{0} = {0};//{1};", fieldData.FieldName, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength != "*")
                        {
                            //rlt = string.Format("memcpy_s(pkg->{0}, sizeof({1}) * {2}, {0}, sizeof({1}) * {2});//{3}", fieldData.FieldName, fieldData.FieldType, fieldData.FieldLength, fieldData.FieldSummary);
                            rlt = string.Format("for (int i = 0; i < {0}; i++)\r\n", fieldData.FieldLength);
                            rlt += "    {\r\n";
                            rlt += string.Format("        pkg->{0}[i] = {0}[i];//{1}\r\n", fieldData.FieldName, fieldData.FieldSummary);
                            rlt += "    }";
                        }
                        else
                        {
                            rlt = string.Format("for (size_t i = 0; i < {0}.size(); i++)\r\n", fieldData.FieldName);
                            rlt += "    {\r\n";
                            rlt += string.Format("        pkg->{0}.push_back({0}[i]);//{1}\r\n", fieldData.FieldName, fieldData.FieldSummary);
                            rlt += "    }";
                            //rlt = string.Format("{0}.clear();//{1}", fieldData.FieldName, fieldData.FieldSummary);
                        }
                    }
                    break;
            }

            return sBlankSpace + rlt;
        }

        /// <summary>
        /// 字段信息转换为C++字段
        /// </summary>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string FieldData2CPPField(FieldInfo fieldData, int nBSCount)
        {
            string rlt = string.Format("unknow {0};", fieldData.FieldName);
            string sBlankSpace = string.Empty;
            for (int i = 0; i < nBSCount; i++)
            {
                sBlankSpace += " ";
            }
            switch (fieldData.FieldType)
            {
                case "string":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("std::string {0};//{1};", fieldData.FieldName, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("std::vector< std::string > {0};//{1};", fieldData.FieldName, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("std::string {0}[{1}];//{2};", fieldData.FieldName, fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;
                case "map":
                    {
                        string mapFieldKeyType = fieldData.MapFieldKeyType == "string" ? "std::string" : fieldData.MapFieldKeyType;
                        string mapFieldValueType = fieldData.MapFieldValueType == "string" ? "std::string" : fieldData.MapFieldValueType;
                        rlt = string.Format("std::map<{1}, {2}> {0};//{3};", fieldData.FieldName, mapFieldKeyType, mapFieldValueType, fieldData.FieldSummary);
                    }
                    break;
                case "stream":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("ServerLight::Stream {0};//{1};", fieldData.FieldName, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("std::vector< ServerLight::Stream > {0};//{1};", fieldData.FieldName, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("ServerLight::Stream {0}[{1}];//{2};", fieldData.FieldName, fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;

                //case "bool":
                //case "char":
                //case "uchar":
                //case "short":
                //case "ushort":
                //case "int":
                //case "uint":
                //case "int64":
                //case "uint64":
                //case "float":
                //case "double":
                default:
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {//普通字段
                        rlt = string.Format("{0} {1};//{2};", fieldData.FieldType, fieldData.FieldName, fieldData.FieldSummary);
                    }
                    else
                    {//数组字段
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("std::vector< {0} > {1};//{2};", fieldData.FieldType, fieldData.FieldName, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("{0} {1}[{2}];//{3};", fieldData.FieldType, fieldData.FieldName, fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;
            }

            return sBlankSpace + rlt;
        }

        /// <summary>
        /// 字段信息转换为C++序列化流操作
        /// </summary>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string FieldData2CPPSerializeBin(FieldInfo fieldData, string scope, int nBSCount)
        {
            string rlt;
            string sBlankSpace = string.Empty;
            for (int i = 0; i < nBSCount; i++)
            {
                sBlankSpace += " ";
            }

            if (fieldData.FieldType == "map")
            {
                rlt = string.Format(@"{2}ushort us{0}Len = (ushort){1}{0}.size();
{2}stream << us{0}Len;
{2}for(auto& [key, value] : {1}{0})
{2}{{
{2}    stream << key;
{2}    stream << value;
{2}}}", fieldData.FieldName, scope, sBlankSpace);
            }
            else if (string.IsNullOrEmpty(fieldData.FieldLength))
            {//普通变量
                rlt = string.Format("{2}stream << {1}{0};", fieldData.FieldName, scope, sBlankSpace);
            }
            else
            {
                if (fieldData.FieldLength == "*")
                {
                    rlt = string.Format(@"{2}ushort us{0}Len = (ushort){1}{0}.size();
{2}stream << us{0}Len;
{2}for(int i = 0; i < us{0}Len; ++i)
{2}{{
{2}    stream << {1}{0}[i];
{2}}}", fieldData.FieldName, scope, sBlankSpace);
                }
                else
                {
                    rlt = string.Format(@"{3}for(int i = 0; i < {1}; ++i)
{3}{{
{3}    stream << {2}{0}[i];
{3}}}", fieldData.FieldName, fieldData.FieldLength, scope, sBlankSpace);
                }
            }

            return rlt;
        }

        /// <summary>
        /// 字段信息转换为C++反序列化流操作
        /// </summary>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string FieldData2CPPDeserializeBin(FieldInfo fieldData, string scope, int nBSCount)
        {
            string rlt;
            string sBlankSpace = string.Empty;
            for (int i = 0; i < nBSCount; i++)
            {
                sBlankSpace += " ";
            }

            if (fieldData.FieldType == "map")
            {
                string mapFieldKeyType = fieldData.MapFieldKeyType == "string" ? "std::string" : fieldData.MapFieldKeyType;
                string mapFieldValueType = fieldData.MapFieldValueType == "string" ? "std::string" : fieldData.MapFieldValueType;
                rlt = string.Format(@"{2}ushort us{0}Len = 0;
{2}{3} o{0}Key;
{2}{4} o{0}Value;
{2}stream >> us{0}Len;
{2}for(int i = 0; i < us{0}Len; ++i)
{2}{{
{2}    stream >> o{0}Key;
{2}    stream >> o{0}Value;
{2}    {1}{0}[o{0}Key] = o{0}Value;
{2}}}", fieldData.FieldName, scope, sBlankSpace, mapFieldKeyType, mapFieldValueType);
            }
            else if (string.IsNullOrEmpty(fieldData.FieldLength))
            {//普通变量
                rlt = string.Format(@"{2}stream >> {1}{0};", fieldData.FieldName, scope, sBlankSpace);
            }
            else
            {
                if (fieldData.FieldLength == "*")
                {
                    string fieldType = fieldData.FieldType == "string" ? "std::string" : fieldData.FieldType;
                    string sReset = string.Format(@"{1}    o{0}Item.reset();", fieldData.FieldName, sBlankSpace);
                    rlt = string.Format(@"{3}ushort us{0}Len = 0;
{3}stream >> us{0}Len;
{3}{2} o{0}Item;
{3}for(int i = 0; i < us{0}Len; ++i)
{3}{{
{4}
{3}    stream >> o{0}Item;
{3}    {1}{0}.push_back(o{0}Item);
{3}}}", fieldData.FieldName, scope, fieldType, sBlankSpace, CheckIsCustomizeStruct(fieldData) ? sReset : "");
                }
                else
                {
                    rlt = string.Format(@"{3}for(int i = 0; i < {1}; ++i)
{3}{{
{3}    stream >> {2}{0}[i];
{3}}}", fieldData.FieldName, fieldData.FieldLength, scope, sBlankSpace);
                }
            }

            return rlt;
        }

        /// <summary>
        /// 字段信息转换为C++序列化JSON操作
        /// </summary>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string FieldData2CPPSerializeJson(FieldInfo fieldData, string scope, int nBSCount)
        {
            string rlt;
            string sBlankSpace = string.Empty;
            for (int i = 0; i < nBSCount; i++)
            {
                sBlankSpace += " ";
            }

            if(fieldData.FieldType == "map")
            {
                if (fieldData.MapFieldKeyType == "string")
                {
                    rlt = string.Format(@"{2}for(auto& [key, value] : {1}{0})
{2}{{
{2}    jsonvalue[""{0}""][key] << value;
{2}}}", fieldData.FieldName, scope, sBlankSpace);
                }
                else
                {
                    rlt = string.Format(@"{2}for(auto& [key, value] : {1}{0})
{2}{{
{2}    jsonvalue[""{0}""][std::to_string(key)] << value;
{2}}}", fieldData.FieldName, scope, sBlankSpace);
                }
            }
            else if (string.IsNullOrEmpty(fieldData.FieldLength))
            {
                rlt = string.Format("{2}jsonvalue[\"{0}\"] << {1}{0};", fieldData.FieldName, scope, sBlankSpace);
            }
            else
            {
                if (fieldData.FieldLength == "*")
                {
                    rlt = string.Format(@"{2}ushort us{0}Len = (ushort){1}{0}.size();
{2}for(int i = 0; i < us{0}Len; ++i)
{2}{{
{2}    jsonvalue[""{0}""][i] << {1}{0}[i];
{2}}}", fieldData.FieldName, scope, sBlankSpace);
                }
                else
                {
                    rlt = string.Format(@"{3}for(int i = 0; i < {1}; ++i)
{3}{{
{3}    jsonvalue[""{0}""][i] << {2}{0}[i];
{3}}}", fieldData.FieldName, fieldData.FieldLength, scope, sBlankSpace);
                }
            }

            return rlt;
        }

        /// <summary>
        /// 字段信息转换为C++反序列化JSON操作
        /// </summary>
        /// <param name="fieldData"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static string FieldData2CPPDeserializeJson(FieldInfo fieldData, string scope, int nBSCount)
        {
            string rlt;
            string sBlankSpace = string.Empty;
            for (int i = 0; i < nBSCount; i++)
            {
                sBlankSpace += " ";
            }

            if (fieldData.FieldType == "map")
            {
                if (fieldData.MapFieldKeyType == "string")
                {
                    rlt = string.Format(@"{2} for (auto& name : jsonvalue[""{0}""].getMemberNames())
{2}{{
{2}    jsonvalue[""{0}""][name] >> {1}{0}[name];
{2}}}", fieldData.FieldName, scope, sBlankSpace);
                }
                else
                {
                    rlt = string.Format(@"{2}{3} o{0}Key;
{2}for (auto& name : jsonvalue[""{0}""].getMemberNames())
{2}{{
{2}    std::stringstream ss(name);
{2}    ss >> o{0}Key;
{2}    jsonvalue[""{0}""][name] >> {1}{0}[o{0}Key];
{2}}}", fieldData.FieldName, scope, sBlankSpace, fieldData.MapFieldKeyType);
                }
            }
            else if (string.IsNullOrEmpty(fieldData.FieldLength))
            {
                rlt = string.Format("{2}jsonvalue[\"{0}\"] >> {1}{0};", fieldData.FieldName, scope, sBlankSpace);
            }
            else
            {
                if (fieldData.FieldLength == "*")
                {
                    //rlt = string.Format(@"{2}ushort us{0}Len = (ushort){1}{0}.size();
                    rlt = string.Format(@"{2}ushort us{0}Len = (ushort)jsonvalue[""{0}""].size();                    
{2}{1}{0}.resize(jsonvalue[""{0}""].size());
{2}for(int i = 0; i < us{0}Len; ++i)
{2}{{
{2}    jsonvalue[""{0}""][i] >> {1}{0}[i];
{2}}}", fieldData.FieldName, scope, sBlankSpace);
                }
                else
                {
                    rlt = string.Format(@"{3}for(int i = 0; i < {1}; ++i)
{3}{{
{3}    jsonvalue[""{0}""][i] >> {2}{0}[i];
{3}}}", fieldData.FieldName, fieldData.FieldLength, scope, sBlankSpace);
                }
            }

            return rlt;
        }

        /// <summary>
        /// 字段信息转换为Pb字段
        /// </summary>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string GetProtobufField(FieldInfo fieldData, int index)
        {
            string rlt = string.Format("unknown {0} = 1;", fieldData.FieldName); // Protobuf 格式的字段定义

            switch (fieldData.FieldType)
            {
                case "string":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("string {0} = {1};//{2}", fieldData.FieldName, index, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("repeated string {0} = {1};//{2}", fieldData.FieldName, index, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("repeated string {0} = {1};//{2} //固定长数组，其len: {3}", fieldData.FieldName, index, fieldData.FieldSummary, fieldData.FieldLength);
                        }
                    }
                    break;

                case "char":
                case "short":
                case "int":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("int32 {0} = {1};//{2}", fieldData.FieldName, index, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("repeated int32 {0} = {1};//{2}", fieldData.FieldName, index, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("repeated int32 {0} = {1};//{2} //固定长数组，其len: {3}", fieldData.FieldName, index, fieldData.FieldSummary, fieldData.FieldLength);
                        }
                    }
                    break;

                case "uchar":
                case "ushort":
                case "uint":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("uint32 {0} = {1};//{2}", fieldData.FieldName, index, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("repeated uint32 {0} = {1};//{2}", fieldData.FieldName, index, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("repeated uint32 {0} = {1};//{2} //固定长数组，其len: {3}", fieldData.FieldName, index, fieldData.FieldSummary, fieldData.FieldLength);
                        }
                    }
                    break;

                //TODO(elias): 目前没有stream
                case "stream":
                case "bool":
                case "int64":
                case "uint64":
                case "float":
                case "double":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} {1} = {2};//{3}", fieldData.FieldType, fieldData.FieldName, index, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("repeated {0} {1} = {2};//{3}", fieldData.FieldType, fieldData.FieldName, index, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("repeated {0} {1} = {2};//{3} //固定长数组，其len: {4}", fieldData.FieldType, fieldData.FieldName, index, fieldData.FieldSummary, fieldData.FieldLength);
                        }
                    }
                    break;

                // 自定义类型字段
                default:
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("Pb{0} {1} = {2};//{3}", fieldData.FieldType, fieldData.FieldName, index, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("repeated Pb{0} {1} = {2};//{3}", fieldData.FieldType, fieldData.FieldName, index, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("repeated Pb{0} {1} = {2};//{3} //固定长数组，其len: {4}", fieldData.FieldType, fieldData.FieldName, index, fieldData.FieldSummary, fieldData.FieldLength);
                        }
                    }
                    break;
            }

            return "\t" + rlt;
        }

        /// <summary>
        /// 获取Golang标准类型
        /// </summary>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string GetGolangType(FieldInfo fieldData)
        {
            switch (fieldData.FieldType)
            {
                case "char":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "int8";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "[]int8";
                        }
                        else
                        {
                            return string.Format("[{0}]int8", fieldData.FieldLength);
                        }
                    }

                case "uchar":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "uint8";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "[]uint8";
                        }
                        else
                        {
                            return string.Format("[{0}]uint8", fieldData.FieldLength);
                        }
                    }

                case "short":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "int16";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "[]int16";
                        }
                        else
                        {
                            return string.Format("[{0}]int16", fieldData.FieldLength);
                        }
                    }

                case "ushort":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "uint16";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "[]uint16";
                        }
                        else
                        {
                            return string.Format("[{0}]uint16", fieldData.FieldLength);
                        }
                    }

                case "float":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "float32";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "[]float32";
                        }
                        else
                        {
                            return string.Format("[{0}]float32", fieldData.FieldLength);
                        }
                    }

                case "double":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "float64";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "[]float64";
                        }
                        else
                        {
                            return string.Format("[{0}]float64", fieldData.FieldLength);
                        }
                    }

                //case "bool":
                //case "int":
                //case "uint":
                //case "int64":
                //case "uint64":
                //case "string":
                //case "stream":
                default:
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return fieldData.FieldType;
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return string.Format("[]{0}", fieldData.FieldType);
                        }
                        else
                        {
                            return string.Format("[{0}]{1}", fieldData.FieldLength, fieldData.FieldType);
                        }
                    }
            }
        }

        /// <summary>
        /// 获取Golang默认的返回值
        /// </summary>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string GetGolangReturn(FieldInfo fieldData)
        {
            switch (fieldData.FieldType)
            {
                case "bool":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "false";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "nil";
                        }
                        else
                        {
                            return string.Format("[{0}]bool{{}}", fieldData.FieldLength);
                        }
                    }

                case "string":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "\"\"";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "nil";
                        }
                        else
                        {
                            return string.Format("[{0}]string{{}}", fieldData.FieldLength);
                        }
                    }

                case "stream":
                    return "nil";

                case "char":
                case "uchar":
                case "short":
                case "ushort":
                case "float":
                case "double":
                case "int":
                case "uint":
                case "int64":
                case "uint64":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return "0";
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "nil";
                        }
                        else
                        {
                            return string.Format("[{0}]{1}{{}}", fieldData.FieldLength, fieldData.FieldType);
                        }
                    }

                default:
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        return string.Format("{0}{{}}", fieldData.FieldType);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            return "nil";
                        }
                        else
                        {
                            return string.Format("[{0}]{1}{{}}", fieldData.FieldLength, fieldData.FieldType);
                        }
                    }
            }
        }

        /// <summary>
        /// 字段信息转换为Golang结构体字段
        /// </summary>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string GetGolangSTField(FieldInfo fieldData, int nBSCount)
        {
            int padding = 40;

            // 字段名首字母大写，因为go中大写为公有，小写为私有
            string name = fieldData.FieldName;
            name = char.ToUpper(name[0]) + name.Substring(1);

            string rlt = string.Format("{0} unknow", name);

            string sBlankSpace = string.Empty;
            for (int i = 0; i < nBSCount; i++)
            {
                sBlankSpace += " ";
            }
            switch (fieldData.FieldType)
            {
                case "char":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} int8 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("{0} []int8 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("{0} [{1}]int8 // {2}", name.PadRight(padding), fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;

                case "uchar":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} uint8 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("{0} []uint8 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("{0} [{1}]uint8 // {2}", name.PadRight(padding), fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;

                case "short":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} int16 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("{0} []int16 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("{0} [{1}]int16 // {2}", name.PadRight(padding), fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;

                case "ushort":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} uint16 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("{0} []uint16 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("{0} [{1}]uint16 // {2}", name.PadRight(padding), fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;

                case "float":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} float32 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("{0} []float32 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("{0} [{1}]float32 // {2}", name.PadRight(padding), fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;

                case "double":
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {
                        rlt = string.Format("{0} float64 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("{0} []float64 // {1}", name.PadRight(padding), fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("{0} [{1}]float64 // {2}", name.PadRight(padding), fieldData.FieldLength, fieldData.FieldSummary);
                        }
                    }
                    break;

                // TODO(elias)：先不管stream
                //case "stream":

                //case "bool":
                //case "int":
                //case "uint":
                //case "int64":
                //case "uint64":
                //case "string":
                default:
                    if (string.IsNullOrEmpty(fieldData.FieldLength))
                    {//普通字段
                        rlt = string.Format("{0} {1} // {2}", name.PadRight(padding), fieldData.FieldType, fieldData.FieldSummary);
                    }
                    else
                    {
                        if (fieldData.FieldLength == "*")
                        {
                            rlt = string.Format("{0} []{1} // {2}", name.PadRight(padding), fieldData.FieldType, fieldData.FieldSummary);
                        }
                        else
                        {
                            rlt = string.Format("{0} [{1}]{2} // {3}", name.PadRight(padding), fieldData.FieldLength, fieldData.FieldType, fieldData.FieldSummary);
                        }
                    }
                    break;
            }

            return sBlankSpace + rlt;
        }

        /// <summary>
        /// Golang接口实现函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetGolangIFFunc(string name)
        {
            StringBuilder res = new StringBuilder();
            // 每个字段的操作函数
            res.Append(string.Format("func (p *{0}) GetPacketID() uint16 {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn 0\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn p.Header.PacketID\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) GetOwnerID1() uint32 {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn 0\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn p.Header.OwnerID1\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) GetOwnerID2() uint32 {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn 0\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn p.Header.OwnerID2\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) GetSourceID1() uint8 {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn 0\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn p.Header.SourceID1\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) GetSourceID2() uint8 {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn 0\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn p.Header.SourceID2\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) GetTargetID1() uint8 {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn 0\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn p.Header.TargetID1\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) GetTargetID2() uint8 {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn 0\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn p.Header.TargetID2\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) GetPacketSize() uint32 {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn 0\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn p.Header.PacketSize\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) SetPacketSize(size uint32) {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn\r\n");
            res.Append("\t}\r\n");
            res.Append("\tp.Header.PacketSize = size\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) SetHeader(header *PacketHeader) {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn\r\n");
            res.Append("\t}\r\n");
            res.Append("\tp.Header = header\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) Serialize(buf *bytes.Buffer) error {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn errors.New(\"packet is nil\")\r\n");
            res.Append("\t}\r\n");
            res.Append("\r\n");
            res.Append("\tval := reflect.ValueOf(p).Elem()\r\n");
            res.Append("\tfor i := 0; i < val.NumField(); i++ {\r\n");
            res.Append("\t\tif i == 0 && val.Type().Field(i).Name == \"Header\" {\r\n");
            res.Append("\t\t\tcontinue\r\n");
            res.Append("\t\t}\r\n");
            res.Append("\r\n");
            res.Append("\t\tfield := val.Field(i)\r\n");
            res.Append("\t\tif err := writeToByteArray(buf, field); err != nil {\r\n");
            res.Append("\t\t\treturn err\r\n");
            res.Append("\t\t}\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn nil\r\n");
            res.Append("}\r\n");

            res.Append("\r\n");
            res.Append(string.Format("func (p *{0}) Deserialize(buf *bytes.Buffer) error {{\r\n", name));
            res.Append("\tif p == nil || p.Header == nil {\r\n");
            res.Append("\t\treturn errors.New(\"packet is nil\")\r\n");
            res.Append("\t}\r\n");
            res.Append("\r\n");
            res.Append("\tval := reflect.ValueOf(p).Elem()\r\n");
            res.Append("\tidx := 0\r\n");
            res.Append("\r\n");
            res.Append("\tfor i := 0; i < val.NumField(); i++ {\r\n");
            res.Append("\t\tif i == 0 && val.Type().Field(i).Name == \"Header\" {\r\n");
            res.Append("\t\t\tcontinue\r\n");
            res.Append("\t\t}\r\n");
            res.Append("\r\n");
            res.Append("\t\tfield := val.Field(i)\r\n");
            res.Append("\t\tif err := readFromByteArray(buf.Bytes(), &idx, field); err != nil {\r\n");
            res.Append("\t\t\treturn err\r\n");
            res.Append("\t\t}\r\n");
            res.Append("\t}\r\n");
            res.Append("\treturn nil\r\n");
            res.Append("}\r\n");

            return res.ToString();
        }

        /// <summary>
        /// Golang操作函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldData"></param>
        /// <returns></returns>
        public static string GetGolangOPFunc(string structName, FieldInfo fieldData)
        {
            // 字段名首字母大写，因为go中大写为公有，小写为私有
            string fieldName = fieldData.FieldName;
            fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

            StringBuilder res = new StringBuilder();
            // 每个字段的操作函数
            res.Append(string.Format("func (p *{0}) Get{1}() {2} {{\r\n", structName, fieldName, CodeUtil.GetGolangType(fieldData)));
            res.Append("\tif p == nil {\r\n");
            res.Append(string.Format("\t\treturn {0}\r\n", CodeUtil.GetGolangReturn(fieldData)));
            res.Append("\t}\r\n");
            res.Append(string.Format("\treturn p.{0}\r\n", fieldName));
            res.Append("}\r\n");

            return res.ToString();
        }

        /// <summary>
        /// GolangPbType函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetPbType(string type)
        {
            switch (type)
            {
                case "char":
                case "short":
                case "int":
                    return "int32";

                case "uchar":
                case "ushort":
                case "uint":
                    return "uint32";

                case "float":
                    return "float32";

                case "double":
                    return "float64";

                case "string":
                case "bool":
                case "int64":
                case "uint64":
                case "stream":
                    return type;

                default:    //自定义类型
                    return "struct";
            }
        }

        /// <summary>
        /// GolangSTConvertPb函数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetGolangSTConvertFunc(StructInfo st)
        {
            // 字段名首字母大写，因为go中大写为公有，小写为私有
            string structName = st.StructName;
            structName = char.ToUpper(structName[0]) + structName.Substring(1);

            StringBuilder res = new StringBuilder();
            StringBuilder array = new StringBuilder();

            res.Append(string.Format("func (p *{0}) ConvertPb() *Pb{0} {{\r\n", structName));
            res.Append("\tif p == nil {\r\n");
            res.Append("\t\treturn nil\t\n");
            res.Append("\t}\r\n");
            res.Append(string.Format("\tres := &Pb{0} {{\r\n", structName));
            // 遍历每一个字段，数组另外处理
            foreach (var fielditem in st.Fields)
            {
                // Pb字段的首字母总是大写的
                string uName = fielditem.FieldName;
                uName = char.ToUpper(uName[0]) + uName.Substring(1);

                // 普通字段
                if (string.IsNullOrEmpty(fielditem.FieldLength))
                {
                    string type = GetPbType(fielditem.FieldType);
                    if (type == "struct")
                    {
                        res.Append(string.Format("\t\t{0}: p.{0}.ConvertPb(),\r\n", uName));
                    }
                    else
                    {
                        res.Append(string.Format("\t\t{0}: {1}(p.{0}),\r\n", uName, type));
                    }
                }
                else    // 切片或数组
                {
                    string type = GetPbType(fielditem.FieldType);
                    if (type == "struct")
                    {
                        array.Append(string.Format("\to{0} := make([]*Pb{1}, len(p.{0}))\r\n", uName, fielditem.FieldType));
                        array.Append(string.Format("\tfor i, val := range p.{0} {{\r\n", uName));
                        array.Append(string.Format("\t\to{0}[i] = val.ConvertPb()\r\n", uName));
                        array.Append("\t}\r\n");
                        array.Append(string.Format("\tres.{0} = o{0}\r\n\r\n", uName));
                    }
                    else
                    {
                        array.Append(string.Format("\to{0} := make([]{1}, len(p.{0}))\r\n", uName, type));
                        array.Append(string.Format("\tfor i, val := range p.{0} {{\r\n", uName));
                        array.Append(string.Format("\t\to{0}[i] = {1}(val)\r\n", uName, type));
                        array.Append("\t}\r\n");
                        array.Append(string.Format("\tres.{0} = o{0}\r\n\r\n", uName));
                    }
                }
            }
            res.Append("\t}\r\n\r\n");
            res.Append(array.ToString());
            res.Append("\treturn res;\r\n");
            res.Append("}\r\n");

            return res.ToString();
        }

        /// <summary>
        /// GolangPTConvertPb函数
        /// </summary>
        /// <param name="st"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGolangPTConvertFunc(ProtocolInfo pt, ProtocolType pType)
        {
            // 字段名首字母大写，因为go中大写为公有，小写为私有
            string protocolName = pt.ProtocolName;
            protocolName = char.ToUpper(protocolName[0]) + protocolName.Substring(1);

            string name = string.Format("{0}unknow", protocolName);
            BindingList<FieldInfo> fields = null;
            if (pType == ProtocolType.C2S)
            {
                name = string.Format("{0}C2S", protocolName);
                fields = pt.ReqFields;
            }
            else if (pType == ProtocolType.S2C)
            {
                name = string.Format("{0}S2C", protocolName);
                fields = pt.ResFields;
            }

            StringBuilder res = new StringBuilder();
            StringBuilder array = new StringBuilder();

            res.Append(string.Format("func (p *{0}) ConvertPb() *Pb{0} {{\r\n", name));
            res.Append("\tif p == nil {\r\n");
            res.Append("\t\treturn nil\t\n");
            res.Append("\t}\r\n");
            res.Append(string.Format("\tres := &Pb{0} {{\r\n", name));
            // 遍历每一个字段，数组另外处理
            foreach (var fielditem in fields)
            {
                // Pb类型的首字母总是大写的
                string uName = fielditem.FieldName;
                uName = char.ToUpper(uName[0]) + uName.Substring(1);

                string type = GetPbType(fielditem.FieldType);
                // 普通字段
                if (string.IsNullOrEmpty(fielditem.FieldLength))
                {
                    if (type == "struct")
                    {
                        res.Append(string.Format("\t\t{0}: p.{0}.ConvertPb(),\r\n", uName));
                    }
                    else
                    {
                        res.Append(string.Format("\t\t{0}: {1}(p.{0}),\r\n", uName, type));
                    }
                }
                else    // 切片或数组
                {
                    if (type == "struct")
                    {
                        array.Append(string.Format("\to{0} := make([]*Pb{1}, len(p.{0}))\r\n", uName, fielditem.FieldType));
                        array.Append(string.Format("\tfor i, val := range p.{0} {{\r\n", uName));
                        array.Append(string.Format("\t\to{0}[i] = val.ConvertPb()\r\n", uName));
                        array.Append("\t}\r\n");
                        array.Append(string.Format("\tres.{0} = o{0}\r\n\r\n", uName));
                    }
                    else
                    {
                        array.Append(string.Format("\to{0} := make([]{1}, len(p.{0}))\r\n", uName, type));
                        array.Append(string.Format("\tfor i, val := range p.{0} {{\r\n", uName));
                        array.Append(string.Format("\t\to{0}[i] = {1}(val)\r\n", uName, type));
                        array.Append("\t}\r\n");
                        array.Append(string.Format("\tres.{0} = o{0}\r\n\r\n", uName));
                    }
                }
            }
            res.Append("\t}\r\n\r\n");
            res.Append(array.ToString());
            res.Append("\treturn res;\r\n");
            res.Append("}\r\n");

            return res.ToString();
        }

        public static bool CheckIsCustomizeStruct(FieldInfo fieldData)
        {
            bool bCustomize = true;
            switch (fieldData.FieldType)
            {
                case "string":
                case "stream":
                case "bool":
                case "char":
                case "uchar":
                case "short":
                case "ushort":
                case "int":
                case "uint":
                case "int64":
                case "uint64":
                case "float":
                case "double":
                    bCustomize = false;
                    break;
                default:
                    break;
            }
            return bCustomize;
        }

        public static string FieldData2AS3Field(FieldInfo fieldData)
        {
            string rlt = string.Format("    public var {0}:Object;", fieldData.FieldName);

            if (string.IsNullOrEmpty(fieldData.FieldLength))
            {
                switch (fieldData.FieldType)
                {
                    case "bool":
                        rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:Boolean;", fieldData.FieldName, fieldData.FieldSummary);
                        break;

                    case "char":
                    case "short":
                    case "int":
                        rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:int;", fieldData.FieldName, fieldData.FieldSummary);
                        break;

                    case "uchar":
                    case "ushort":
                    case "uint":
                        rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:uint;", fieldData.FieldName, fieldData.FieldSummary);
                        break;

                    case "int64":
                    case "uint64":
                        rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:Number = 0;", fieldData.FieldName, fieldData.FieldSummary);
                        break;

                    case "float":
                    case "double":
                        rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:Number = 0;", fieldData.FieldName, fieldData.FieldSummary);
                        break;

                    case "string":
                        rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:String = \"\";", fieldData.FieldName, fieldData.FieldSummary);
                        break;

                    case "stream":
                        rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:ByteStream = new ByteStream();", fieldData.FieldName, fieldData.FieldSummary);
                        break;

                    default:
                        rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:{2} = new {2}();", fieldData.FieldName, fieldData.FieldSummary, fieldData.FieldType);
                        break;
                }
            }
            else
            {
                rlt = string.Format("    /**\r\n     * {1}\r\n     */\r\n    public var {0}:Array = [];", fieldData.FieldName, fieldData.FieldSummary);
            }

            return rlt;
        }

        public static string FieldData2AS3Write(FieldInfo fieldData, string scope, string tempVar)
        {
            string rlt = "";
            string pthis = string.IsNullOrEmpty(scope) ? "" : scope + ".";

            if (string.IsNullOrEmpty(fieldData.FieldLength))
            {//普通变量
                rlt = getAS3WriteCode(fieldData, pthis, false, tempVar);
            }
            else
            {
                if (fieldData.FieldLength == "*")
                {
                    rlt = string.Format("        bytes.writeShort({1}{0}.length);\r\n", fieldData.FieldName, pthis);
                    rlt += string.Format("        for (var {2}:int = 0; {2} < {1}{0}.length; ++{2}) ", fieldData.FieldName, pthis, tempVar);
                }
                else
                {
                    rlt = string.Format("        for (var {1}:int = 0; {1} < {0}; ++{1}) ", fieldData.FieldLength, tempVar);
                }
                rlt += "{\r\n";
                rlt += "    " + getAS3WriteCode(fieldData, pthis, true, tempVar);
                rlt += "\r\n        }";
            }

            return rlt;
        }

        public static string FieldData2AS3Read(FieldInfo fieldData, string scope, string tempVar, ref bool tag_isFirst)
        {
            string rlt = "";
            string pthis = string.IsNullOrEmpty(scope) ? "" : scope + ".";

            if (string.IsNullOrEmpty(fieldData.FieldLength))
            {   //普通变量
                rlt = getAS3ReadCode(fieldData, pthis, false, ref tag_isFirst);
            }
            else
            {
                if (fieldData.FieldLength == "*")
                {
                    rlt = string.Format("        var {0}Len:uint = bytes.readUnsignedShort();\r\n", tempVar);
                    rlt += string.Format("        for (var {0}:int = 0; {0} < {0}Len; ++{0}) ", tempVar);
                }
                else
                {
                    rlt = string.Format("        for (var {1}:int = 0; {1} < {0}; ++{1}) ", fieldData.FieldLength, tempVar);
                }
                rlt += "{\r\n";
                rlt += getAS3ReadCode(fieldData, pthis, true, ref tag_isFirst, tempVar, "\t");
                rlt += "\r\n        }";
            }

            return rlt;
        }

        private static string getAS3ReadCode(FieldInfo fieldData, string scope, bool isArray, ref bool tag_IsFirst, string tempVar = "0", string BlankSpace = "")
        {
            string rlt = "";

            switch (fieldData.FieldType)
            {
                case "bool":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readBoolean());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readBoolean();", fieldData.FieldName, scope);
                    }
                    break;

                case "char":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readByte());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readByte();", fieldData.FieldName, scope);
                    }
                    break;

                case "uchar":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readUnsignedByte());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readUnsignedByte();", fieldData.FieldName, scope);
                    }
                    break;

                case "short":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readShort());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readShort();", fieldData.FieldName, scope);
                    }
                    break;

                case "ushort":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readUnsignedShort());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readUnsignedShort();", fieldData.FieldName, scope);
                    }
                    break;

                case "int":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readInt());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readInt();", fieldData.FieldName, scope);
                    }
                    break;

                case "uint":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readUnsignedInt());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readUnsignedInt();", fieldData.FieldName, scope);
                    }
                    break;

                case "int64":
                    StringBuilder sbTemp = new StringBuilder();
                    if (isArray)
                    {
                        if (tag_IsFirst)
                        {
                            sbTemp.Append(BlankSpace + "\t\tvar iTemp: int = bytes.readInt();\r\n");
                            tag_IsFirst = false;
                        }
                        else
                        {
                            sbTemp.Append(BlankSpace + "\t\tiTemp = bytes.readInt();\r\n");
                        }
                        sbTemp.Append(BlankSpace + "\t\tif(iTemp < 0)\r\n");
                        sbTemp.Append(BlankSpace + "\t\t{\r\n");
                        sbTemp.Append(BlankSpace + string.Format("\t\t\t{1}{0}[{2}] = (bytes.readInt());\r\n", fieldData.FieldName, scope, tempVar));
                        sbTemp.Append(BlankSpace + "\t\t}\r\n");
                        sbTemp.Append(BlankSpace + "\t\telse\r\n");
                        sbTemp.Append(BlankSpace + "\t\t{\r\n");
                        sbTemp.Append(BlankSpace + "\t\t\tbytes.position -= 4;\r\n");
                        sbTemp.Append(BlankSpace + string.Format("\t\t\t{1}{0}[{2}] = bytes.readInt64();\r\n", fieldData.FieldName, scope, tempVar));
                        sbTemp.Append(BlankSpace + "\t\t}\r\n");
                        //rlt = string.Format("        {1}{0}[{2}] = (bytes.readInt64());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        if (tag_IsFirst)
                        {
                            sbTemp.Append(BlankSpace + "\t\tvar iTemp: int = bytes.readInt();\r\n");
                            tag_IsFirst = false;
                        }
                        else
                        {
                            sbTemp.Append(BlankSpace + "\t\tiTemp = bytes.readInt();\r\n");
                        }
                        sbTemp.Append(BlankSpace + "\t\tif(iTemp < 0)\r\n");
                        sbTemp.Append(BlankSpace + "\t\t{\r\n");
                        sbTemp.Append(BlankSpace + string.Format("\t\t\t{1}{0} = bytes.readInt();\r\n", fieldData.FieldName, scope));
                        sbTemp.Append(BlankSpace + "\t\t}\r\n");
                        sbTemp.Append(BlankSpace + "\t\telse\r\n");
                        sbTemp.Append(BlankSpace + "\t\t{\r\n");
                        sbTemp.Append(BlankSpace + "\t\t\tbytes.position -= 4;\r\n");
                        sbTemp.Append(BlankSpace + string.Format("\t\t\t{1}{0} = bytes.readInt64();\r\n", fieldData.FieldName, scope));
                        sbTemp.Append(BlankSpace + "\t\t}\r\n");
                        //rlt = string.Format("        {1}{0} = bytes.readInt64();", fieldData.FieldName, scope);
                    }
                    return sbTemp.ToString();
                //break;

                case "uint64":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readUnsignedInt64());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readUnsignedInt64();", fieldData.FieldName, scope);
                    }
                    break;

                case "float":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readFloat());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readFloat();", fieldData.FieldName, scope);
                    }
                    break;

                case "double":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readDouble());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readDouble();", fieldData.FieldName, scope);
                    }
                    break;

                case "string":
                    if (isArray)
                    {
                        rlt = string.Format("        {1}{0}[{2}] = (bytes.readString());", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {1}{0} = bytes.readString();", fieldData.FieldName, scope);
                    }
                    break;

                case "stream":
                    if (isArray)
                    {
                        rlt = "        var len:uint = bytes.readUnsignedShort();\r\n";
                        rlt += "        var stream:ByteStream = new ByteStream();\r\n";
                        rlt += "        bytes.readBytes(stream, 0, len);\r\n";
                        rlt += string.Format("        {1}{0}[{2}] = (stream)", fieldData.FieldName, scope, tempVar);
                    }
                    else
                    {
                        rlt = "        var len:uint = bytes.readUnsignedShort();\r\n";
                        rlt += string.Format("        bytes.readBytes({1}{0}, 0, len);", fieldData.FieldName, scope);
                    }
                    break;

                default:
                    if (isArray)
                    {
                        rlt = string.Format("        {2}{0}[{3}] = (PacketUtil.read{1}(bytes));", fieldData.FieldName, fieldData.FieldType, scope, tempVar);
                    }
                    else
                    {
                        rlt = string.Format("        {2}{0} = PacketUtil.read{1}(bytes);", fieldData.FieldName, fieldData.FieldType, scope);
                    }
                    break;
            }

            return BlankSpace + rlt;
        }

        private static string getAS3WriteCode(FieldInfo fieldData, string scope, bool isArray, string forVar)
        {
            string rlt = "";

            switch (fieldData.FieldType)
            {
                case "bool":
                    rlt = string.Format("        bytes.writeBoolean({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "char":
                case "uchar":
                    rlt = string.Format("        bytes.writeByte({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "short":
                case "ushort":
                    rlt = string.Format("        bytes.writeShort({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "int":
                    rlt = string.Format("        bytes.writeInt({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "uint":
                    rlt = string.Format("        bytes.writeUnsignedInt({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "int64":
                    rlt = string.Format("        bytes.writeInt64({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "uint64":
                    rlt = string.Format("        bytes.writeUnsignedInt64({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "float":
                    rlt = string.Format("        bytes.writeFloat({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "double":
                    rlt = string.Format("        bytes.writeDouble({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "string":
                    rlt = string.Format("        bytes.writeString({1}{0}{2});", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                case "stream":
                    rlt = string.Format("        bytes.writeShort({1}{0}{2}.length);", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    rlt += string.Format("        bytes.writeBytes({1}{0}{2}, 0, {1}{0}{2}.length);", fieldData.FieldName, scope, isArray ? "[" + forVar + "]" : "");
                    break;

                default:
                    rlt = string.Format("        PacketUtil.write{1}(bytes, {2}{0}{3} as {1});", fieldData.FieldName, fieldData.FieldType, scope, isArray ? "[" + forVar + "]" : "");
                    break;
            }

            return rlt;
        }

        /// <summary>
        /// 转换对应字段的 数据类型
        /// </summary>
        /// <param name="fieldData">字段数据</param>
        /// <returns>c#的数据类型</returns>
        public static string GetU3DTypeStr(FieldInfo fieldData, bool OnlyStr)
        {
            string rlt = "";

            if ((string.IsNullOrEmpty(fieldData.FieldLength)) || (OnlyStr))
            {
                //{ "bool", "char", "uchar", "short", "ushort", "int", "uint", "int64", "uint64", "float", "double", "string", "stream" };
                switch (fieldData.FieldType)
                {
                    case "bool":
                        rlt = "bool";
                        break;

                    case "char":
                        rlt = "sbyte";
                        break;

                    case "uchar":
                        rlt = "byte";
                        break;

                    case "short":
                        rlt = "short";
                        break;

                    case "ushort":
                        rlt = "ushort";
                        break;

                    case "int":
                        rlt = "int";
                        break;

                    case "uint":
                        rlt = "uint";
                        break;

                    case "int64":
                        rlt = "long";
                        break;

                    case "uint64":
                        rlt = "ulong";
                        break;

                    case "float":
                        rlt = "float";
                        break;

                    case "double":
                        rlt = "double";
                        break;

                    case "string":
                        rlt = "string";
                        break;

                    case "stream":
                        rlt = "stream";
                        break;

                    default:
                        {
                            rlt = "Pak";
                            break;
                        }
                }
            }
            else
            {
                rlt = "List";
            }

            return rlt;
        }

        public static string GetU3DTypeStr(FieldInfo fieldData)
        {
            string rlt = "";

            switch (fieldData.FieldType)
            {
                case "bool":
                    rlt = "bool";
                    break;

                case "char":
                    rlt = "sbyte";
                    break;

                case "uchar":
                    rlt = "byte";
                    break;

                case "short":
                    rlt = "short";
                    break;

                case "ushort":
                    rlt = "ushort";
                    break;

                case "int":
                    rlt = "int";
                    break;

                case "uint":
                    rlt = "uint";
                    break;

                case "int64":
                    rlt = "long";
                    break;

                case "uint64":
                    rlt = "ulong";
                    break;

                case "float":
                    rlt = "float";
                    break;

                case "double":
                    rlt = "double";
                    break;

                case "string":
                    rlt = "string";
                    break;

                case "stream":
                    rlt = "stream";
                    break;

                default:
                    {
                        rlt = fieldData.FieldType;
                        break;
                    }
            }

            return rlt;
        }

        public static string GetU3DWriteStr(FieldInfo fieldData, bool OnlyStr)
        {
            string rlt = "";

            if ((string.IsNullOrEmpty(fieldData.FieldLength)) || (OnlyStr))
            {
                //{ "bool", "char", "uchar", "short", "ushort", "int", "uint", "int64", "uint64", "float", "double", "string", "stream" };
                switch (fieldData.FieldType)
                {
                    case "bool":
                        rlt = "WriteBoolean";
                        break;

                    case "char":
                        rlt = "WriteSByte";
                        break;

                    case "uchar":
                        rlt = "WriteByte";
                        break;

                    case "short":
                        rlt = "WriteShort";
                        break;

                    case "ushort":
                        rlt = "WriteUShort";
                        break;

                    case "int":
                        rlt = "WriteInt";
                        break;

                    case "uint":
                        rlt = "WriteUInt";
                        break;

                    case "int64":
                        rlt = "WriteInt64";
                        break;

                    case "uint64":
                        rlt = "WriteUInt64";
                        break;

                    case "float":
                        rlt = "WriteFloat";
                        break;

                    case "double":
                        rlt = "WriteDouble";
                        break;

                    case "string":
                        rlt = "WriteUTF";
                        break;

                    case "stream":
                        rlt = "stream";
                        break;

                    default:
                        rlt = "Pak";
                        break;
                }
            }
            else
            {
                rlt = "List";
            }

            return rlt;
        }

        public static string GetU3DReadStr(FieldInfo fieldData, bool OnlyStr)
        {
            string rlt = "";

            if ((string.IsNullOrEmpty(fieldData.FieldLength)) || (OnlyStr))
            {
                //{ "bool", "char", "uchar", "short", "ushort", "int", "uint", "int64", "uint64", "float", "double", "string", "stream" };
                switch (fieldData.FieldType)
                {
                    case "bool":
                        rlt = "ReadBoolean";
                        break;

                    case "char":
                        rlt = "ReadSByte";
                        break;

                    case "uchar":
                        rlt = "ReadByte";
                        break;

                    case "short":
                        rlt = "ReadShort";
                        break;

                    case "ushort":
                        rlt = "ReadUShort";
                        break;

                    case "int":
                        rlt = "ReadInt";
                        break;

                    case "uint":
                        rlt = "ReadUInt";
                        break;

                    case "int64":
                        rlt = "ReadInt64";
                        break;

                    case "uint64":
                        rlt = "ReadUInt64";
                        break;

                    case "float":
                        rlt = "ReadFloat";
                        break;

                    case "double":
                        rlt = "ReadDouble";
                        break;

                    case "string":
                        rlt = "ReadUTF";
                        break;

                    case "stream":
                        rlt = "stream";
                        break;

                    default:
                        rlt = "Pak";
                        break;
                }
            }
            else
            {
                rlt = "List";
            }

            return rlt;
        }

        public static string GetU3DProtoVarStr(FieldInfo fieldData)
        {
            string rlt = "";

            if (string.IsNullOrEmpty(fieldData.FieldLength))
            {
                //{ "bool", "char", "uchar", "short", "ushort", "int", "uint", "int64", "uint64", "float", "double", "string", "stream" };
                switch (fieldData.FieldType)
                {
                    case "bool":
                        rlt = "bool " + fieldData.FieldName;
                        break;

                    case "char":
                        rlt = "sbyte " + fieldData.FieldName;
                        break;

                    case "uchar":
                        rlt = "byte " + fieldData.FieldName;
                        break;

                    case "short":
                        rlt = "short " + fieldData.FieldName;
                        break;

                    case "ushort":
                        rlt = "ushort " + fieldData.FieldName;
                        break;

                    case "int":
                        rlt = "int " + fieldData.FieldName;
                        break;

                    case "uint":
                        rlt = "uint " + fieldData.FieldName;
                        break;

                    case "int64":
                        rlt = "long " + fieldData.FieldName;
                        break;

                    case "uint64":
                        rlt = "ulong " + fieldData.FieldName;
                        break;

                    case "float":
                        rlt = "float " + fieldData.FieldName;
                        break;

                    case "double":
                        rlt = "double " + fieldData.FieldName;
                        break;

                    case "string":
                        rlt = "string " + fieldData.FieldName;
                        break;

                    case "stream":
                        rlt = "stream " + fieldData.FieldName;
                        break;

                    default:
                        rlt = string.Format("{0} {1} = new {0}()", fieldData.FieldType, fieldData.FieldName);
                        break;
                }
            }
            else
            {
                rlt = string.Format("List<{0}> {1} = new List<{0}>()", GetU3DTypeStr(fieldData, false), fieldData.FieldName);
            }

            return rlt;
        }

        public static string GetU3DTypeDefaultValueCode(FieldInfo fieldData)
        {
            string rlt = string.Empty;
            if (string.IsNullOrEmpty(fieldData.FieldLength))
            {
                switch (fieldData.FieldType)
                {
                    case "bool":
                        rlt = fieldData.FieldName + " = false;";
                        break;

                    case "char":
                    case "uchar":
                    case "short":
                    case "ushort":
                    case "int":
                    case "uint":
                    case "int64":
                    case "uint64":
                    case "float":
                    case "double":
                        rlt = fieldData.FieldName + " = 0;";
                        break;

                    case "string":
                        rlt = fieldData.FieldName + " = string.Empty;";
                        break;

                    //case "stream":
                    //    rlt = "stream " + fieldData.FieldName;
                    //    break;

                    default:
                        rlt = fieldData.FieldName + " = default(" + fieldData.FieldType + ");";
                        break;
                }
            }
            else if (fieldData.FieldLength == "*")
            {
                rlt = fieldData.FieldName + ".Clear();";
            }
            else
            {
                rlt = "Array.Clear(" + fieldData.FieldName + ", 0, " + fieldData.FieldLength + ");";
            }

            return rlt;
        }
    }
}