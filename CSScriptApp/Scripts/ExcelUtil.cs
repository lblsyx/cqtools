#if !USE_SCRIPT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using UnityLight.Loggers;
using CSScriptApp.TemplateCore;

namespace CSScriptApp.Scripts
{
    public class ExcelUtil
    {
        public static void GetDirectoryChildren(string path, IList<string> list, string extFilters)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                string[] filters = null;
                List<FileInfo> fileList = new List<FileInfo>();

                if (string.IsNullOrEmpty(extFilters) == false)
                {
                    filters = extFilters.Split(new char[] { '|', ';', '；', ',', '，' }, StringSplitOptions.RemoveEmptyEntries);
                }

                if (filters != null)
                {
                    foreach (string filter in filters)
                    {
                        FileInfo[] fileArray = dir.GetFiles(filter, SearchOption.AllDirectories);
                        fileList.AddRange(fileArray);
                    }
                }
                else
                {
                    FileInfo[] fileArray = dir.GetFiles("*", SearchOption.AllDirectories);
                    fileList.AddRange(fileArray);
                }

                foreach (FileInfo item in fileList)
                {
                    list.Add(item.FullName);
                }
            }
            else
            {
                XLogger.ErrorFormat("文件夹路径错误：{0}", path);
            }
        }

        public static IWorkbook OpenExcel(string path)
        {
            IWorkbook workbook = null;

            try
            {
                FileStream fs = File.OpenRead(path);
                workbook = new HSSFWorkbook(fs);
                fs.Close();
            }
            catch (Exception ex)
            {
                XLogger.Error(ex.Message);
            }

            return workbook;
        }

        public static IList<TableInfo> ParseTableList(string path, string ignoreNames, bool bReadValues)
        {
            IList<TableInfo> list = new List<TableInfo>();
            string[] ignoreNameArray = ignoreNames.Split(new char[] { '|', ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> ignoreNameList = new List<string>();
            ignoreNameList.AddRange(ignoreNameArray);

            if (File.Exists(path))
            {
                ParseTableSchemaListImp(path, list, ignoreNameList, bReadValues);
            }
            else if (Directory.Exists(path))
            {
                IList<string> files = new List<string>();
                GetDirectoryChildren(path, files, "*.xls|*.xlsx");
                foreach (string file in files)
                {
                    ParseTableSchemaListImp(file, list, ignoreNameList, bReadValues);
                }
            }
            else
            {
                XLogger.ErrorFormat("无法识别的路径：{0}", path);
            }

            return list;
        }

        public static void ParseTableSchemaListImp(string filePath, IList<TableInfo> list, List<string> ignoreNames, bool bReadValues)
        {
            if (File.Exists(filePath) == false) return;
            IWorkbook workbook = OpenExcel(filePath);
            if (workbook == null) return;

            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                ISheet sheet = workbook.GetSheetAt(i);
                if (sheet == null)
                {
                    XLogger.ErrorFormat("获取工作表失败！Index:{0}", i);
                    continue;
                }

                TableInfo table = new TableInfo();
                string[] temp = sheet.SheetName.Split(new char[] { '|', '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (temp.Length >= 2)
                {
                    table.TableName = temp[1];
                    table.TableSummary = temp[0];
                }
                else
                {
                    table.TableName = sheet.SheetName;
                }

                if (ignoreNames.IndexOf(table.TableName) != -1) continue;

                //try
                //{
                table.ExcelFile = filePath;
                table.SheetName = sheet.SheetName;

                if (sheet.LastRowNum < 2) continue;

                IRow row1 = sheet.GetRow(0);
                IRow row2 = sheet.GetRow(1);
                IRow row3 = sheet.GetRow(2);

                if (row1 == null || row2 == null || row3 == null) continue;

                for (int j = 0; j < row1.LastCellNum; j++)
                {
                    ICell cell1 = row1.GetCell(j);//类型
                    ICell cell2 = row2.GetCell(j);//名称
                    ICell cell3 = row3.GetCell(j);//说明

                    if (cell1 == null || cell2 == null) break;

                    if (string.IsNullOrEmpty(cell1.StringCellValue)) break;
                    if (string.IsNullOrEmpty(cell2.StringCellValue)) break;
                    //if (string.IsNullOrEmpty(cell3.StringCellValue)) break;

                    FieldInfo field = new FieldInfo();
                    field.FieldIndex = j;
                    field.FieldType = cell1.StringCellValue.Trim().ToLower();
                    field.FieldName = cell2.StringCellValue.Trim();
                    if (cell3 != null) field.FieldSummary = cell3.StringCellValue;
                    else field.FieldSummary = string.Empty;
                    table.TableFields.Add(field);
                }

                if (table.TableFields.Count != 0)
                {
                    list.Add(table);
                    table.RowCount = 0;

                    if (bReadValues)
                    {
                        for (int m = 3; m <= sheet.LastRowNum; m++)
                        {
                            IRow row = sheet.GetRow(m);
                            if (row == null)
                            {
                                XLogger.ErrorFormat("当前行为空，数据结束，行信息：SheetName：{0}, Row：{1}", table.SheetName, m + 1);
                                break;
                            }
                            ICell firstCell = row.GetCell(0);
                            if (firstCell == null)
                            {
                                XLogger.ErrorFormat("当前行的首个单元格为空，数据结束，行信息：\r\nSheetName：{0}, Row：{1}", table.SheetName, m + 1);
                                break;
                            }

                            for (int n = 0; n < table.TableFields.Count; n++)
                            {
                                FieldInfo field = table.TableFields[n];

                                ICell cell = row.GetCell(field.FieldIndex);

                                if (cell == null && field.FieldType.ToLower() != "string")
                                {
                                    throw new Exception(string.Format("获取行列数据失败!SheetName:{0}, Row:{1}, Column:{2}[{3}]", sheet.SheetName, m + 1, n + 1, field.FieldName));
                                }

                                field.FieldValues.Add(GetFieldValue(field, cell, table.SheetName, m));
                            }
                            table.RowCount += 1;
                        }
                    }
                }
                //}
                //catch (Exception ex)
                //{
                //    XLogger.ErrorFormat("解析表结构失败!SheetName:{0}, Error:{1}", sheet.SheetName, ex.Message);
                //}
            }
        }

        private static object GetFieldValue(FieldInfo field, ICell cell, string sheetName, int rowIndex)
        {
            if (cell != null)
            {
                switch (field.FieldType)
                {
                    case "sbyte":
                        {
                            try
                            {
                                return Convert.ToSByte(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }
                            try
                            {
                                return Convert.ToSByte(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "byte":
                        {
                            try
                            {
                                return Convert.ToByte(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }
                            try
                            {
                                return Convert.ToByte(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "bool":
                    case "boolean":
                        {
                            try
                            {
                                string val = cell.StringCellValue;
                                if (string.IsNullOrEmpty(val) == false && (val.Trim() == "true" || val.Trim() != "0"))
                                {
                                    return true;
                                }

                                return false;
                            }
                            catch// (Exception ex)
                            {
                            }

                            try
                            {
                                byte val = Convert.ToByte(cell.NumericCellValue);
                                if (val != 0) return true;
                                return false;
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return false;
                            }
                        }
                    case "short":
                        {
                            try
                            {
                                return Convert.ToInt16(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }

                            try
                            {
                                return Convert.ToInt16(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "ushort":
                        {
                            try
                            {
                                return Convert.ToUInt16(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }

                            try
                            {
                                return Convert.ToUInt16(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "int":
                        {
                            try
                            {
                                return Convert.ToInt32(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }

                            try
                            {
                                return Convert.ToInt32(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "uint":
                        {
                            try
                            {
                                return Convert.ToUInt32(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }

                            try
                            {
                                return Convert.ToUInt32(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "int64":
                    case "long":
                        {
                            try
                            {
                                return Convert.ToInt64(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }

                            try
                            {
                                return Convert.ToInt64(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "uint64":
                    case "ulong":
                        {
                            try
                            {
                                return Convert.ToUInt64(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }
                            try
                            {
                                return Convert.ToUInt64(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "float":
                        {
                            try
                            {
                                return Convert.ToSingle(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }
                            try
                            {
                                return Convert.ToSingle(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "double":
                        {
                            try
                            {
                                return Convert.ToDouble(cell.StringCellValue);
                            }
                            catch// (Exception ex)
                            {
                            }
                            try
                            {
                                return Convert.ToDouble(cell.NumericCellValue);
                            }
                            catch (Exception e)
                            {
                                XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                                return 0;
                            }
                        }
                    case "varchar":
                    case "string":
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(cell.StringCellValue))
                                {
                                    return string.Empty;
                                }

                                return cell.StringCellValue;
                            }
                            catch// (Exception e)
                            {
                                //XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nErrorMsg：{4}\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType, e.Message);
                            }

                            try
                            {
                                string str = Convert.ToString(cell.NumericCellValue);
                                if (str == null) str = string.Empty;
                                return str;
                            }
                            catch// (Exception ex)
                            {
                            }

                            return string.Empty;
                        }
                }

                XLogger.WarnFormat("无法识别的数据类型，默认使用字符串类型\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType);

                if (string.IsNullOrEmpty(cell.StringCellValue)) return string.Empty;

                return cell.StringCellValue;
            }
            else
            {
                switch (field.FieldType)
                {
                    case "bool":
                    case "boolean":
                        {
                            XLogger.WarnFormat("单元格为 null，使用 {0} 类型的默认值：false\r\nSheetName：{1}, Row：{2}, FieldName：{3}, FieldType：{4}", field.FieldType, sheetName, rowIndex + 1, field.FieldName, field.FieldType);
                            return false;
                        }
                    case "sbyte":
                    case "byte":
                    case "short":
                    case "ushort":
                    case "int":
                    case "uint":
                    case "int64":
                    case "long":
                    case "uint64":
                    case "ulong":
                    case "float":
                    case "double":
                        {
                            XLogger.WarnFormat("单元格为 null，使用 {0} 类型的默认值：0\r\nSheetName：{1}, Row：{2}, FieldName：{3}, FieldType：{4}", field.FieldType, sheetName, rowIndex + 1, field.FieldName, field.FieldType);
                            return 0;
                        }
                    case "varchar":
                    case "string":
                        {
                            //XLogger.WarnFormat("单元格为 null，使用 {0} 类型的默认值：string.Empty\r\nSheetName：{1}, Row：{2}, FieldName：{3}, FieldType：{4}", field.FieldType, sheetName, rowIndex + 1, field.FieldName, field.FieldType);
                            return string.Empty;
                        }
                }

                XLogger.WarnFormat("单元格为 null，类型 {0} 未知，使用统一默认值：string.Empty\r\nSheetName：{1}, Row：{2}, FieldName：{3}, FieldType：{4}", field.FieldType, sheetName, rowIndex + 1, field.FieldName, field.FieldType);
                return string.Empty;
            }
        }
    }
}
#endif