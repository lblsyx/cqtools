using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TemplateTool.Datas;
using TemplateTool.Utils;
using UnityLight.Internets;
using UnityLight.Loggers;
using zlib;

namespace TemplateTool.Packs.Packers
{
    [DataPacker((int)PackerType.BinaryPacker, "二进制(zlib)", true)]
    public class BinaryDataPacker : IDataPacker
    {
        public bool initData(string outPath, object others, bool needDelete)
        {
            return true;
        }

        public void PackData(IList<TableInfo> schemas, string outPath, object others, string errorlogPath)
        {
            //MessageBox.Show("二进制打包器未实现!");

            ByteArray fileBytes = new ByteArray();
            fileBytes.AutoResize = true;

            for (int i = 0; i < schemas.Count; i++)
            {
                ByteArray temp = new ByteArray();
                temp.AutoResize = true;

                TableInfo table = schemas[i];
                fileBytes.WriteUTF(table.TableName);
                fileBytes.WriteUTF("字段类型");//\t隔开的文本
                fileBytes.WriteUTF("字段名称");//\t隔开的文本
                fileBytes.WriteUTF("字段说明");//\t隔开的文本

                List<int> tList = new List<int>();

                temp.WriteUInt(table.RowCount);
                for (int m = 0; m < table.RowCount; m++)
                {//循环写入行数据
                    for (int n = 0; n < table.TableFields.Count; n++)
                    {//循环写入列数据
                        if(n == 0)
                        {
                            FieldInfo field = table.TableFields[n];
                            int valIndex = m;
                            int TID = (int)field.FieldValues[valIndex];

                            //if(TID == 0)
                            //{
                            //    XLogger.ErrorFormat($"模板表 TID为0，请检查是否有空数据 表名：{table.TableName} 文件路径：{table.ExcelFile}");
                            //}
                            //else 
                            if (tList.IndexOf(TID) > -1)
                            {
                                string errorMsg = $"模板表发生重复 TID: {TID} 表名：{table.TableName} 文件路径：{table.ExcelFile}";
                                XLogger.ErrorFormat(errorMsg);
                                if (errorlogPath != null)
                                {
                                    File.AppendAllText(errorlogPath, errorMsg);
                                }
                            }
                            else
                            {
                                tList.Add(TID);
                            }
                        }
                       
                        WriteFieldValue(temp, table.TableFields[n], m);
                    }
                }
                fileBytes.WriteUInt((uint)temp.Length);
                fileBytes.WriteBytes(temp.Buffer, 0, temp.Length);
            }

            ByteArray contentBytes = new ByteArray();
            contentBytes.AutoResize = true;
            contentBytes.WriteUInt((uint)fileBytes.Length);

            byte[] bytes = null;

            try
            {
                if (File.Exists(outPath)) File.Delete(outPath);

                using (MemoryStream stream = new MemoryStream())
                {
                    using (ZOutputStream zOut = new ZOutputStream(stream, zlibConst.Z_BEST_COMPRESSION))
                    {
                        zOut.Write(fileBytes.Buffer, 0, fileBytes.Length);
                        zOut.finish();
                        bytes = stream.GetBuffer();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace));
            }

            if (bytes == null)
            {
                MessageBox.Show("打包失败!");
                return;
            }

            contentBytes.WriteUInt((uint)bytes.Length);
            contentBytes.WriteBytes(bytes);
            byte[] contents = new byte[contentBytes.Length];
            
            Buffer.BlockCopy(contentBytes.Buffer, 0, contents, 0, contentBytes.Length);
            File.WriteAllBytes(outPath, contents);

            if (Global.args.Length != 0)
            {
                System.Environment.Exit(0);   
            }
            else
            {
                if (SeqRun.Instance.isRunning())
                {
                    SeqRun.Instance.onCurTaskFinished();
                }
                else
                {
                    var packParam = ToolConfig.Instance.GetPackParameter((int)PackerType.BinaryPacker);
                    if (packParam != null && packParam.PackDataAutoCommit)
                    {
                        SvnUtil.CoverCommit(packParam.PackDataFileOutputPath,false);
                    }
                    MessageBox.Show("打包成功!");
                }
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
                //case "int64":
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


                case "int64":
                    double vOut = Convert.ToDouble(field.FieldValues[valIndex]);
                    bytes.WriteDouble(vOut);
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
