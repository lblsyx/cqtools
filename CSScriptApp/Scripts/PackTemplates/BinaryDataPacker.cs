#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityLight.Internets;
using UnityLight.Loggers;
using CSScriptApp.TemplateCore;
using zlib;

namespace CSScriptApp.Scripts.PackTemplates
{
    [DataPacker((int)PackerType.BinaryPacker, "二进制(zlib)", true)]
    public class BinaryDataPacker : IDataPacker
    {
        public void PackData(IList<TableInfo> schemas, string outPath, object others)
        {
            //MessageBox.Show("二进制打包器未实现!");

            ByteArray fileBytes = new ByteArray();
            fileBytes.AutoResize = true;

            for (int i = 0; i < schemas.Count; i++)
            {
                TableInfo table = schemas[i];
                fileBytes.WriteUTF(table.TableName);
                fileBytes.WriteUTF("字段类型");//\t隔开的文本
                fileBytes.WriteUTF("字段名称");//\t隔开的文本
                fileBytes.WriteUTF("字段说明");//\t隔开的文本

                ByteArray temp = new ByteArray();
                temp.AutoResize = true;

                temp.WriteUInt(table.RowCount);
                for (int m = 0; m < table.RowCount; m++)
                {//循环写入行数据
                    for (int n = 0; n < table.TableFields.Count; n++)
                    {//循环写入列数据
                        WriteFieldValue(temp, table.TableFields[n], m);
                    }
                }
                fileBytes.WriteUInt((uint)temp.Length);
                fileBytes.WriteBytes(temp.Buffer, 0, temp.Length);
            }

            ByteArray writeBytes = new ByteArray();
            writeBytes.AutoResize = true;
            writeBytes.WriteUInt((uint)fileBytes.Length);
            ByteArray temp2 = fileBytes.Compress();
            writeBytes.WriteUInt((uint)temp2.Length);
            writeBytes.WriteBytes(temp2.Buffer, 0, temp2.Length);
            try
            {
                if (File.Exists(outPath)) File.Delete(outPath);

                using (Stream stream = new FileStream(outPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    //stream.Write(fileBytes.Buffer, 0, fileBytes.Length);
                    using (ZOutputStream zOut = new ZOutputStream(stream, zlibConst.Z_BEST_COMPRESSION))
                    {
                        zOut.Write(writeBytes.Buffer, 0, writeBytes.Length);
                        zOut.Flush();
                    }
                }

                Program.WriteToConsole("打包模板数据成功!");
            }
            catch (Exception ex)
            {
                Program.WriteToConsole(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }
        }

        private void WriteFieldValue(ByteArray bytes, FieldInfo field, int valIndex)
        {
            switch (field.FieldType)
            {
                case "sbyte":
                    bytes.WriteSByte((sbyte)field.FieldValues[valIndex]);
                    break;
                case "byte":
                    bytes.WriteByte((byte)field.FieldValues[valIndex]);
                    break;
                case "bool":
                case "boolean":
                    bytes.WriteBoolean((bool)field.FieldValues[valIndex]);
                    break;
                case "short":
                    bytes.WriteShort((short)field.FieldValues[valIndex]);
                    break;
                case "ushort":
                    bytes.WriteUShort((ushort)field.FieldValues[valIndex]);
                    break;
                case "int":
                    bytes.WriteInt((int)field.FieldValues[valIndex]);
                    break;
                case "uint":
                    bytes.WriteUInt((uint)field.FieldValues[valIndex]);
                    break;
                case "int64":
                case "long":
                    bytes.WriteInt64((long)field.FieldValues[valIndex]);
                    break;
                case "uint64":
                case "ulong":
                    bytes.WriteUInt64((ulong)field.FieldValues[valIndex]);
                    break;
                case "float":
                    bytes.WriteFloat((float)field.FieldValues[valIndex]);
                    break;
                case "double":
                    bytes.WriteDouble((double)field.FieldValues[valIndex]);
                    break;
                case "varchar":
                case "string":
                    bytes.WriteUTF((string)field.FieldValues[valIndex]);
                    break;
                default:
                    bytes.WriteUTF(string.Empty);
                    break;
            }
        }
    }
}
#endif