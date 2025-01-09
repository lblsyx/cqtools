using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using UnityLight.Loggers;
using TemplateTool.Datas;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using MySql.Data.MySqlClient;
using System.Data;
using System.Xml;
using ExcelDataReader;
using Newtonsoft.Json;
using System.Security.AccessControl;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel.Charts;
using NPOI.OpenXmlFormats.Spreadsheet;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace TemplateTool.Utils
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
						//FileInfo[] fileArray = dir.GetFiles(filter, SearchOption.AllDirectories);
						FileInfo[] fileArray = Array.Empty<FileInfo>();
                        if (filter == "*.xls")
						{
                            var files = dir.GetFiles("*.xls", SearchOption.AllDirectories);
                            fileArray = files.Where(file => !file.Name.EndsWith(".xlsx")).ToArray();
                        }
						else
						{
							fileArray = dir.GetFiles(filter, SearchOption.AllDirectories);
						}
						
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
				if(path.EndsWith("xls"))
				{
					workbook = new HSSFWorkbook(fs);
				}
				else if(path.EndsWith("xlsx"))
				{
					workbook = new XSSFWorkbook(fs);
				}
				fs.Close();
			}
			catch (Exception ex)
			{
				XLogger.Error(path + "\r\n" + ex.Message);
			}

			return workbook;
		}
		public delegate void AsyncDelegate(string filePath, List<string> ignoreNames, bool bReadValues);
		public static IList<TableInfo> _parsedTableList = new List<TableInfo>();
		public static int _leftFileCount = 0;
		public static int _totalFileCount = 0;
		public static Mutex _mut = new Mutex();

		public static MainForm _mainForm = null;

		/// <summary>
		/// 异步加载完成，子线程调用
		/// </summary>
		/// <param name="iar"></param>
		private static void LoadFinished(IAsyncResult iar)
		{
			//var id = Thread.CurrentThread.ManagedThreadId.ToString();
			//XLogger.InfoFormat("LoadFinished ThreadId:{0}", id);
			//if (iar == null)
			//{
			//    Console.WriteLine("IAsyncResult is null");
			//    return;
			//}
			// XLogger.InfoFormat("_parsedTableList：{0} ", _parsedTableList.Count);

			AsyncResult ar = (AsyncResult)iar;
			//var del = ar.AsyncDelegate as AsyncDelegate;
			//_mut.WaitOne();
			--_leftFileCount;
			//XLogger.InfoFormat("加载完成,进度：{0}/{1},剩余：{2}", data, _totalFileCount, _leftFileCount);
			//_mut.ReleaseMutex();
			if (_leftFileCount == 0)
			{
				XLogger.InfoFormat("所有文件加载完成");
			}
		}
		public static void StopUpload()
		{
			if (_mainForm != null)
			{
				_mainForm.stopUpload();
			}
		}

		public static void ParseTableListAsync(string path, string ignoreNames, bool bReadValues, object others)
		{
			//var id = Thread.CurrentThread.ManagedThreadId.ToString();
			//XLogger.InfoFormat("ParseTableListAsync ThreadId:{0}", id);
			_mainForm = others as MainForm;
			//var list = new List<TableInfo>();
			string[] ignoreNameArray = ignoreNames.Split(new char[] { '|', ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);
			List<string> ignoreNameList = new List<string>();
			ignoreNameList.AddRange(ignoreNameArray);

			if (File.Exists(path))
			{
				_leftFileCount = 1;
				_totalFileCount = 1;
				AsyncDelegate funcNewTask = new AsyncDelegate(ParseTableAsync);
				//参数2：回调函数，参数3：@Object异步信息
				IAsyncResult ar2 = funcNewTask.BeginInvoke(path, ignoreNameList, bReadValues, new AsyncCallback(LoadFinished), null);
				//在这里如果要等待委托异步完成后再进行，使用WaitOne();
				//ar2.AsyncWaitHandle.WaitOne();
			}
			else if (Directory.Exists(path))
			{
				IList<string> files = new List<string>();
				GetDirectoryChildren(path, files, "*.xls|*.xlsx");
				_leftFileCount = files.Count;
				_totalFileCount = _leftFileCount;
				XLogger.InfoFormat("要解析的文件数量：{0}", _totalFileCount);
				foreach (string file in files)
				{
					
					AsyncDelegate funcNewTask = new AsyncDelegate(ParseTableAsync);
					//参数2：回调函数，参数3：@Object异步信息
					funcNewTask.BeginInvoke(file, ignoreNameList, bReadValues, new AsyncCallback(LoadFinished), null);
				}
			}
			else
			{
				XLogger.ErrorFormat("无法识别的路径：{0}", path);
			}
		}
		public static IList<TableInfo> ParseTableList(string path, string ignoreNames, bool bReadValues)
		{
			var list = new List<TableInfo>();
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

		private static bool IsVailedFirstCell(ICell firstCell)
		{
			if (firstCell.CellType == CellType.Numeric)
			{
				return true;
			}
			else if (firstCell.CellType == CellType.Formula)
			{
				if (firstCell.CachedFormulaResultType == CellType.Numeric)
				{
					return true;
				}
				else if (firstCell.CachedFormulaResultType == CellType.String)
				{
					return (string.IsNullOrEmpty(firstCell.StringCellValue) == false);
				}
				else
				{
					return false;
				}
			}
			else if (firstCell.CellType == CellType.String)
			{
				return (string.IsNullOrEmpty(firstCell.StringCellValue) == false);
			}

			return false;
		}

		/// <summary>
		/// 异步解析单个Excel,子线程调用
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="list">数据add到这个list中</param>
		/// <param name="ignoreNames"></param>
		/// <param name="bReadValues"></param>
		public static void ParseTableAsync(string filePath, List<string> ignoreNames, bool bReadValues)
		{
			//var id = Thread.CurrentThread.ManagedThreadId.ToString();
			//XLogger.InfoFormat("ParseTableAsync ThreadId:{0}",id);
			//IList<TableInfo> list = new List<TableInfo>();
			if (File.Exists(filePath) == false)
			{
				XLogger.InfoFormat("File not Exists");
				return;
			}
			IWorkbook workbook = OpenExcel(filePath);
			if (workbook == null)
			{
				XLogger.InfoFormat("workbook == null");
				return;
			}
			for (int i = 0; i < workbook.NumberOfSheets; i++)
			{
				ISheet sheet = workbook.GetSheetAt(i);
				if (sheet == null)
				{
					XLogger.ErrorFormat("获取工作表失败！Index:{0}", i);
					continue;
				}

				if (sheet.SheetName.IndexOf("__") == 0)
				{
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
					table.RowCount = 0;

					if (bReadValues)
					{
						for (int m = 3; m <= sheet.LastRowNum; m++)
						{
							IRow row = sheet.GetRow(m);
							if (row == null)
								continue;

							ICell firstCell = row.GetCell(0);
							if (firstCell == null)
								continue;

							if (false == IsVailedFirstCell(firstCell))
								continue;

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

					_mut.WaitOne();
					_parsedTableList.Add(table);
					_mut.ReleaseMutex();
				}
			}

			workbook.Close();
		}

		/// <summary>
		/// 解析单个Excel
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="list">数据add到这个list中</param>
		/// <param name="ignoreNames"></param>
		/// <param name="bReadValues"></param>
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

				if (sheet.SheetName.IndexOf("__") == 0)
				{
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

							try
							{
								if (string.IsNullOrEmpty(firstCell.StringCellValue))
								{
									table.ExcelFile = filePath;
									table.SheetName = sheet.SheetName;
									//XLogger.ErrorFormat($"模板表发现空数据 表名：{table.TableName} 文件路径：{table.ExcelFile}");
									break;
								}
							}
							catch
							{

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

			workbook.Close();
		}

		private static object GetFieldValue(FieldInfo field, ICell cell, string sheetName, int rowIndex)
		{
			if (cell != null)
			{
				try
				{
					switch (field.FieldType)
					{
						case "int":
							{
								if ((cell.CellType == CellType.Numeric) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric))
								{
									return Convert.ToInt32(cell.NumericCellValue);
								}
								else if ((cell.CellType == CellType.String) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.String))
								{
									return Convert.ToInt32(cell.StringCellValue);
								}
								else if(cell.CellType == CellType.Blank)
								{
									return 0;
								}
								break;
							}
						case "uint":
							{
								if ((cell.CellType == CellType.Numeric) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric))
								{
									return Convert.ToUInt32(cell.NumericCellValue);
								}
								else if ((cell.CellType == CellType.String) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.String))
								{
									return Convert.ToUInt32(cell.StringCellValue);
								}
								else if (cell.CellType == CellType.Blank)
								{
									return 0;
								}
								break;
							}
						case "int64":
							{
								if ((cell.CellType == CellType.Numeric) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric))
								{
									return Convert.ToInt64(cell.NumericCellValue);
								}
								else if ((cell.CellType == CellType.String) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.String))
								{
									return Convert.ToInt64(cell.StringCellValue);
								}
								else if (cell.CellType == CellType.Blank)
								{
									return 0;
								}
								break;
							}
						case "uint64":
							{
								if ((cell.CellType == CellType.Numeric) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric))
								{
									return Convert.ToUInt64(cell.NumericCellValue);
								}
								else if ((cell.CellType == CellType.String) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.String))
								{
									return Convert.ToUInt64(cell.StringCellValue);
								}
								else if (cell.CellType == CellType.Blank)
								{
									return 0;
								}
								break;
							}
						case "float":
							{
								if ((cell.CellType == CellType.Numeric) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric))
								{
									return Convert.ToSingle(cell.NumericCellValue);
								}
								else if ((cell.CellType == CellType.String) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.String))
								{
									return Convert.ToSingle(cell.StringCellValue);
								}
								else if (cell.CellType == CellType.Blank)
								{
									return 0;
								}
								break;
							}
						case "double":
							{
								if ((cell.CellType == CellType.Numeric) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric))
								{
									return Convert.ToDouble(cell.NumericCellValue);
								}
								else if ((cell.CellType == CellType.String) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.String))
								{
									return Convert.ToDouble(cell.StringCellValue);
								}
								else if (cell.CellType == CellType.Blank)
								{
									return 0;
								}
								break;
							}
						case "string":
							{
								if ((cell.CellType == CellType.Numeric) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.Numeric))
								{
									return Convert.ToString(cell.NumericCellValue);
								}
								else if ((cell.CellType == CellType.String) || (cell.CellType == CellType.Formula && cell.CachedFormulaResultType == CellType.String))
								{
									if (string.IsNullOrEmpty(cell.StringCellValue))
									{
										return string.Empty;
									}
									return cell.StringCellValue;
								}
								else if (cell.CellType == CellType.Blank)
								{
									return string.Empty;
								}
								break;
							}
					}
				}
				catch
				{
					XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType);
				}

				XLogger.ErrorFormat("数据转换出错，可能类型不正确!当前转换的单元格为：\r\nSheetName：{0}, Row：{1}, FieldName：{2}, FieldType：{3}", sheetName, rowIndex + 1, field.FieldName, field.FieldType);

				return 0;
			}
			else
			{
				switch (field.FieldType)
				{
					case "int":
					case "uint":
					case "int64":
					case "uint64":
					case "float":
					case "double":
						{
							XLogger.WarnFormat("单元格为 null，使用 {0} 类型的默认值：0\r\nSheetName：{1}, Row：{2}, FieldName：{3}, FieldType：{4}", field.FieldType, sheetName, rowIndex + 1, field.FieldName, field.FieldType);
							return 0;
						}
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

		public static void CreateExcelFile(string output, TableInfo oTableInfo)
		{
			if (string.IsNullOrEmpty(output)) return;

			Directory.CreateDirectory(output);

			IWorkbook workbook = new XSSFWorkbook();
			ISheet sheet = workbook.CreateSheet(oTableInfo.SheetName);
			IRow row1 = sheet.CreateRow(0);//第一行
			IRow row2 = sheet.CreateRow(1);//第二行
			IRow row3 = sheet.CreateRow(2);//第三行

			for (int i = 0; i < oTableInfo.TableFields.Count; i++)
			{
				ICell cell1 = row1.CreateCell(i);
				ICell cell2 = row2.CreateCell(i);
				ICell cell3 = row3.CreateCell(i);
				FieldInfo oFieldInfo = oTableInfo.TableFields[i];

				cell1.SetCellValue(oFieldInfo.FieldType);
				cell2.SetCellValue(oFieldInfo.FieldName);
				cell3.SetCellValue(oFieldInfo.FieldSummary);
			}

			if (oTableInfo.TableFields.Count == 0) return;
			int nRowCount = oTableInfo.TableFields[0].FieldValues.Count;
			for (int i = 0; i < nRowCount; i++)
			{
				IRow row = sheet.CreateRow(i + 3);

				for (int j = 0; j < oTableInfo.TableFields.Count; j++)
				{
					FieldInfo oFieldInfo = oTableInfo.TableFields[j];
					object data = oFieldInfo.FieldValues[i];
					ICell cell = row.CreateCell(j);
					if (data != null)
					{
						try
						{
							cell.SetCellValue(data.ToString());
							//cell.SetCellType(GetExcelType(oFieldInfo));
						}
						catch (Exception ex)
						{
							XLogger.Error(string.Format("TabelName:{0}, FieldName:{1}", oTableInfo.TableName, oFieldInfo.FieldName));
						}
					}
					else
					{
						cell.SetCellValue("");
					}
				}
			}

			string filePath = Path.Combine(output, oTableInfo.TableName + ".xls");
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				workbook.Write(ms);
				using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
				{
					byte[] data = ms.ToArray();
					fs.Write(data, 0, data.Length);
					fs.Flush();
				}
			}
			workbook.Close();
		}

		private static CellType GetExcelType(FieldInfo oFieldInfo)
		{
			switch (oFieldInfo.FieldType.ToLower())
			{
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
				case "long":
				case "ulong":
					return CellType.Numeric;
			}

			return CellType.Unknown;
		}

		// ----------------------Excel转Json----------------------------
        public static string GetFullNameType(string type)
        {
            switch (type)      // 依据是TypeCode
            {
                case "int":
                case "int32":
                    return "Int32";

                case "uint":
                case "uint32":
                    return "UInt32";

                case "int64":
                    return "Int64";

                case "uint64":
                    return "UInt64";

                case "string":
                    return "String";

                case "float":
                    return "Single";

                case "double":
                    return "Double";

                default:
                    Console.WriteLine("Wrong Type | {0}", type);
                    break;
            }
            return "";
        }

        // 支持单个excel多张sheet生成
        public static void ExcelToJson(string excelFilePath, string jsonOutputPath, string errorlogPath)
        {
			string errorMsg;
            try
            {
                using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    // Use ExcelDataReader to read the Excel file
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();

						// 正则表达式，用于检测只有字母,数字,下划线
                        Regex regex = new Regex("^[a-zA-Z0-9_]+$");

                        // 遍历每一个sheet
                        for (var i = 0; i != result.Tables.Count; ++i)
                        {
                            var sheet = result.Tables[i];

                            // 表名只能有字母和数字
                            if (!regex.IsMatch(sheet.TableName.ToString()))
                            {
								errorMsg = "表名只能包含字母和数字 | sheet: " + sheet.TableName + " | file: " + excelFilePath;
                                XLogger.Error(errorMsg);

								if (errorlogPath != null)
								{
                                    File.AppendAllText(errorlogPath, errorMsg + Environment.NewLine);
                                }
                                if (Global.args.Length > 0)
								{
                                    Environment.Exit((int)EErrorCode.HaveInvalidChar);
                                }
                            }

                            if (sheet.Rows.Count < 2)
                            {
                                continue;
                            }

                            // Get the header rows
                            var typesRow = sheet.Rows[0];
                            var namesRow = sheet.Rows[1];
                            var commentsRow = sheet.Rows[2];

							if (typesRow[0] == null || typesRow[0] == DBNull.Value ||
								namesRow[0] == null || namesRow[0] == DBNull.Value ||
								commentsRow[0] == null || commentsRow[0] == DBNull.Value)
							{
								continue;
							}

                            // dataRows存放所有数据行
                            var dataRows = new DataTable();
                            for (var j = 0; j < sheet.Columns.Count; j++)
                            {
								if (namesRow[j] == null)
									continue;

                                // 检查 namesRow[j] 是否仅包含字母和数字
                                if (!regex.IsMatch(namesRow[j].ToString()))
                                {
									if (namesRow[j] != DBNull.Value &&
										!string.IsNullOrEmpty(namesRow[j].ToString()))
									{
										errorMsg = "字段名只能包含字母和数字 | column: " + (j + 1).ToString() + " | name: " + namesRow[j].ToString() + " | sheet: " + sheet.TableName + " | file: " + excelFilePath;
                                        XLogger.Error(errorMsg);
										
										if (errorlogPath != null)
										{
											File.AppendAllText(errorlogPath, errorMsg + Environment.NewLine);
										}
                                        if (Global.args.Length > 0)
                                        {
                                            Environment.Exit((int)EErrorCode.HaveInvalidChar);
                                        }
                                    }
                                }
                                dataRows.Columns.Add(namesRow[j].ToString(), typeof(string));
                            }

                            for (var j = 3; j < sheet.Rows.Count; j++)
                            {
                                var row = sheet.Rows[j];
                                if (row == null) continue;
                                var newRow = dataRows.Rows.Add();
                                for (var k = 0; k < sheet.Columns.Count; k++)
                                {
                                    var cell = row[k];
                                    if (cell == null) continue;
                                    newRow[k] = cell.ToString();
                                }
                            }

                            // 转json
                            // 遍历dataRows每一个单元格
                            var data = new List<object>();
                            for (var row = 0; row != dataRows.Rows.Count; row++)
                            {
								//如果该行的第一列id列为空，就无视
							   var idCellValue = dataRows.Rows[row][0];
								if (
									dataRows.Rows[row] == null ||
									idCellValue == null ||
									idCellValue == DBNull.Value ||
									string.IsNullOrEmpty(idCellValue.ToString())
									) continue;

								var obj = new System.Dynamic.ExpandoObject() as IDictionary<string, object>;

                                for (var column = 0; column != dataRows.Columns.Count; column++)
                                {
                                    // 如果该列的第一行类型行为空，就无视
                                    var typeCellValue = sheet.Rows[0][column];
                                    if (
                                        typeCellValue == null ||
                                        typeCellValue == DBNull.Value ||
                                        string.IsNullOrEmpty(typeCellValue.ToString())
                                        ) continue;

                                    var columnName = namesRow[column].ToString();

                                    try
                                    {
                                        // 需要把类型转成c#的完整类型
                                        var t = typesRow[column].ToString()?.ToLower();
                                        string tType = GetFullNameType(t);

                                        // 对Int32做一下特殊检查，针对形如60.00000001的数值，策划那边表格没找出格式问题
                                        var targetValue = dataRows.Rows[row][column];
                                        if (tType == "Int32" || tType == "UInt32")
                                        {
                                            double doubleValue;
                                            if (Double.TryParse(targetValue.ToString(), out doubleValue))
                                            {
                                                if (Math.Round(doubleValue, 3) == Math.Round(doubleValue, 0) || Math.Round(doubleValue, 3) == Math.Round(doubleValue, 9))
                                                {
                                                    targetValue = (int)Math.Round(doubleValue, 0);
                                                    //Console.WriteLine("============== sheetName: {0}, oldValue: {1}, newValue: {2}", sheet.TableName, dataRows.Rows[row][column], targetValue);
                                                }
                                            }
                                        }

                                        var columnType = Type.GetType("System." + tType);
                                        var columnValue = Convert.ChangeType(targetValue, columnType);
                                        obj[columnName] = columnValue;
                                    }
                                    catch
                                    {
										errorMsg = "Type.GetType 错误 | path: " + excelFilePath + "\n sheetName: " + sheet.TableName + ", columnName: " + columnName + " , row: " + (row + 4).ToString() + " , column: " + (column + 1).ToString() + ", value: " + dataRows.Rows[row][column].ToString();
                                        XLogger.Error(errorMsg);

										if (errorlogPath != null)
										{
											File.AppendAllText(errorlogPath, errorMsg + Environment.NewLine);
										}
                                        if (Global.args.Length != 0)
                                        {
                                            Environment.Exit((int)EErrorCode.Excel2JsonGetTypeFail);
                                        }
                                    }
                                }
								data.Add(obj);
                            }
                            var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

							var encoding = Encoding.GetEncoding("GBK");
							//var encoding = Encoding.UTF8;
                            using (var writer = new StreamWriter(jsonOutputPath + "\\" + sheet.TableName + ".json", false, encoding))
                            {
                                writer.Write(json);
                            }
                        }
                    }
                }
            }
            catch
            {
				errorMsg = "生成失败，表格崩了！| path: " + excelFilePath;
                XLogger.Error(errorMsg);

				if (errorlogPath != null)
				{
					File.AppendAllText(errorlogPath, errorMsg + Environment.NewLine);
				}
				if (Global.args.Length != 0)
				{
                    Environment.Exit((int)EErrorCode.Excel2JsonFail);
                }
            }
        }

        // 支持指定目录下所有表格
        public static void TraverseDirectory(string directoryPath, string jsonOutputPath, string errorlogPath)
        {
            // 对当前目录下所有文件做操作
            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                if (Path.GetExtension(file).Equals(".xls", StringComparison.OrdinalIgnoreCase) ||
                    Path.GetExtension(file).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    ExcelToJson(file, jsonOutputPath, errorlogPath);
                }
            }

            // 递归遍历所有子目录
            string[] directories = Directory.GetDirectories(directoryPath);
            foreach (string directory in directories)
            {
                TraverseDirectory(directory, jsonOutputPath, errorlogPath);
            }
        }
    }
}
