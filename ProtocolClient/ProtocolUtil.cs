using ProtocolCore;
using ProtocolCore.Exports;
using ProtocolCore.Generates;
using ProtocolCore.Imports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UnityLight;

namespace ProtocolClient
{
    public class ProtocolUtil
    {
        public static void saveText(string str)
        {
            StreamWriter sw = new StreamWriter(@"C:\txtwriter.txt", true);
            sw.WriteLine(str);
            sw.Close();//写入
        }

        public static void CompilerScripts()
        {
#if USE_SCRIPT
            List<string> reflections = new List<string>();
            reflections.AddRange(Global.AppConfig.ScriptReflections);
            string name = System.IO.Path.GetFileName(Application.ExecutablePath);

            if (reflections.IndexOf(name) == -1)
            {
                reflections.Add(name);
            }

            //移动到配置文件内
            //string[] reflections = new string[] {
            //    "System.dll",
            //    "System.Core.dll",
            //    "System.Data.dll",
            //    "System.Data.DataSetExtensions.dll",
            //    "System.Deployment.dll",
            //    "System.Drawing.dll",
            //    "System.Windows.Forms.dll",
            //    "System.Xml.dll",
            //    "System.Xml.Linq.dll",
            //    "MySql.Data.dll",
            //    "UnityLight.dll",
            //    name
            //};

            string ExecutablePath = System.Windows.Forms.Application.StartupPath;

            System.Reflection.Assembly ass = ScriptCompiler.CompileCS(ExecutablePath + "\\Scripts", ExecutablePath + "\\Scripts.dll", reflections.ToArray());
#else
            System.Reflection.Assembly ass = typeof(ProtocolUtil).Assembly;
#endif
            ImportMgr.SearchAssembly(ass);
            ExportMgr.SearchAssembly(ass);
            GeneratorMgr.SearchAssembly(ass);

            Global.MainForm.SetImports(ImportMgr.ImportList);
            Global.MainForm.SetExports(ExportMgr.ExportList);
            Global.MainForm.SetGenerators(GeneratorMgr.GeneratorInfoList);
        }

        public static string GetPath(PathType ePathType, string sFilter)
        {
            string path = string.Empty;

            switch (ePathType)
            {
                case PathType.FilePath:
                    {
                        OpenFileDialog oOpenFileDialog = new OpenFileDialog();
                        oOpenFileDialog.Title = "选择文件:";
                        oOpenFileDialog.Filter = sFilter;
                        if (oOpenFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            path = oOpenFileDialog.FileName;
                        }
                    }
                    break;
                case PathType.FolderPath:
                    {
                        FolderBrowserDialog oFolderBrowserDialog = new FolderBrowserDialog();
                        oFolderBrowserDialog.ShowNewFolderButton = true;
                        //oFolderBrowserDialog.SelectedPath = path;
                        oFolderBrowserDialog.Description = "选择文件夹:";
                        if (oFolderBrowserDialog.ShowDialog() == DialogResult.OK)
                        {
                            path = oFolderBrowserDialog.SelectedPath;
                        }
                    }
                    break;
            }

            return path;
        }

        public static bool ExecCommand(string cmd, string arguments, bool showWindow = false)
        {
            try
            {
                Process oProcess = new Process();
                oProcess.StartInfo.FileName = cmd;
                oProcess.StartInfo.Arguments = arguments;
                oProcess.StartInfo.CreateNoWindow = !showWindow;
                oProcess.StartInfo.UseShellExecute = false;
                oProcess.Start();
                oProcess.WaitForExit();
                oProcess.Close();
                //Process.Start(cmd, string.Format("up -q {0}", upFolder));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}执行失败!ErrMsg:\r\n{0}", ex.Message, cmd));
                return false;
            }
        }

        public static bool CheckUserName()
        {
            if (string.IsNullOrEmpty(Global.AppConfig.Name))
            {
                MessageBox.Show("姓名未设置!请打开菜单[设置 -> 工具选项]进行设置!");
                return false;
            }

            return true;
        }

        public static bool CheckSQLConnectionString()
        {
            if (Global.AppConfig.SQLPort <= 0 || string.IsNullOrEmpty(Global.AppConfig.SQLHost) || string.IsNullOrEmpty(Global.AppConfig.SQLDB) || string.IsNullOrEmpty(Global.AppConfig.SQLUser))
            {
                MessageBox.Show("数据库未设置!请打开菜单[设置 -> 工具选项]进行设置!");
                return false;
            }

            DBHelper.SetConnectionInfo(Global.AppConfig.SQLHost, Global.AppConfig.SQLPort, Global.AppConfig.SQLUser, Global.AppConfig.SQLPass, Global.AppConfig.SQLDB);

            return true;
        }

        public static bool CheckSelectProject()
        {
            if (Global.SelectedProject == null)
            {
                MessageBox.Show("未选择项目!请打开菜单[项目 -> 所有项目]进行选择!");
                return false;
            }

            return true;
        }

        public static bool CheckRight()
        {
            if (string.IsNullOrEmpty(Global.AppConfig.Name))
            {
                MessageBox.Show("【姓名】未设置!请打开菜单[设置 -> 工具选项]进行设置!");
                return false;
            }

            if (Global.SelectedProject == null)
            {
                MessageBox.Show("未选择【项目】!请打开菜单[项目 -> 所有项目]进行选择!");
                return false;
            }

            return true;
        }

        public static bool CheckFieldNameAndType(BindingList<FieldInfo> list, string pre = "")
        {
            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (string.IsNullOrEmpty(item.FieldName))
                {
                    MessageBox.Show(string.Format("【{1}】的第【{0}】行的【名称】不能为空!", i + 1, pre));
                    return false;
                }
                if (string.IsNullOrEmpty(item.FieldType))
                {
                    MessageBox.Show(string.Format("【{1}】的第【{0}】行的【类型】不能为空!", i + 1, pre));
                    return false;
                }
            }
            return true;
        }

        public static int ReloadCurStructOperateVer()
        {
            Global.StructOperateVer = MySQLUtil.ReloadCurStructOperateVer();

            return Global.StructOperateVer;
        }

        public static int ReloadCurProtocolOperateVer()
        {
            Global.ProtocolOperateVer = MySQLUtil.ReloadCurProtocolOperateVer();

            return Global.ProtocolOperateVer;
        }

        public static bool ProjectExists(string sProjectName)
        {
            bool bResult = MySQLUtil.ProjectExists(sProjectName);

            if (bResult)
            {
                MessageBox.Show(string.Format("项目【{0}】已存在!", sProjectName));
            }

            return bResult;
        }

        public static bool AddProject(ProjectInfo oProjectInfo)
        {
            long id = MySQLUtil.AddProject(oProjectInfo);

            if (id == 0)
            {
                MessageBox.Show(string.Format("项目[{0}]新建失败!", oProjectInfo.ProjectName));
                return false;
            }

            oProjectInfo.ProjectID = (int)id;
            Global.AddProject(oProjectInfo);

            return true;
        }

        public static bool UpdateProject(ProjectInfo oProjectInfo)
        {
            bool bResult = MySQLUtil.UpdateProject(oProjectInfo);

            if (bResult == false)
            {
                MessageBox.Show(string.Format("项目[{0}]更新失败!"), oProjectInfo.ProjectName);
            }

            Global.UpdateProject(oProjectInfo);

            return bResult;
        }

        public static bool DelProject(ProjectInfo oProjectInfo)
        {
            bool bResult = MySQLUtil.DelProject(oProjectInfo);

            if (bResult == false)
            {
                MessageBox.Show(string.Format("项目[{0}]删除失败!", oProjectInfo.ProjectName));
            }

            return bResult;
        }

        public static void ReloadProjectList()
        {
            ProjectInfo oProjectInfo = Global.SelectedProject;

            Global.ClearProjects();

            List<ProjectInfo> list = new List<ProjectInfo>();
            MySQLUtil.ReloadProjectList(list);

            bool bSetSelected = false;
            for (int i = 0; i < list.Count; i++)
            {
                Global.AddProject(list[i]);

                if (oProjectInfo != null && list[i].ProjectID == oProjectInfo.ProjectID)
                {
                    bSetSelected = true;
                    Global.SelectedProject = list[i];
                }
            }

            if (oProjectInfo != null && bSetSelected == false)
            {
                Global.SelectedProject = null;
            }
        }

        public static bool StructExists(string sStructName)
        {
            if (Global.SelectedProject == null) throw new Exception("未选择项目!");

            bool bResult = MySQLUtil.StructExists(Global.SelectedProject.ProjectID, sStructName);

            if (bResult)
            {
                MessageBox.Show(string.Format("结构【{0}】已存在!", sStructName));
            }

            return bResult;
        }

        public static bool AddStruct(StructInfo oStructInfo)
        {
            if (Global.SelectedProject == null || oStructInfo == null) return false;

            oStructInfo.OperateType = 1;
            oStructInfo.SortIndex = MySQLUtil.GetMaxStructSortIndex(Global.SelectedProject.ProjectID) + 1;
            long id = MySQLUtil.AddStruct(Global.SelectedProject, oStructInfo);

            if (id == 0)
            {
                MessageBox.Show(string.Format("新建结构【{0}】失败!", oStructInfo.StructName));
                return false;
            }
            else
            {
                Global.StructOperateVer = oStructInfo.StructID;
            }

            int index = Global.SelectedProject.Structs.Count;
            if (index > 0)
            {
                Global.SelectedProject.Structs.Insert(index - 1, oStructInfo);
            }
            
            index = Global.FieldTypeList.Count;
            if (index > 0)
            {
                Global.FieldTypeList.Insert(index - 1, oStructInfo.StructName);
            }
            
            //Global.SelectedProject.Structs.Add(oStructInfo);
            //Global.FieldTypeList.Add(oStructInfo.StructName);
            return true;
        }

        public static bool UpdateStruct(StructInfo oStructInfo, string oldName)
        {
            if (Global.SelectedProject == null || oStructInfo == null) return false;

            oStructInfo.OperateType = 2;
            bool bResult = MySQLUtil.AddStruct(Global.SelectedProject, oStructInfo) != 0;

            if (bResult == false)
            {
                MessageBox.Show(string.Format("更新结构【{0}】失败!", oStructInfo.StructName));
            }
            else
            {
                if (oStructInfo.StructName != oldName)
                {
                    int idx = Global.FieldTypeList.IndexOf(oldName);

                    if (idx != -1) Global.FieldTypeList[idx] = oStructInfo.StructName;

                    for (int i = 0; i < Global.SelectedProject.Protocols.Count; i++)
                    {
                        ProtocolInfo oProtocolInfo = Global.SelectedProject.Protocols[i];
                        bool bHasStructField = false;
                        for (int j = 0; j < oProtocolInfo.ReqFields.Count; j++)
                        {
                            FieldInfo oFieldInfo = oProtocolInfo.ReqFields[j];
                            if (oFieldInfo.FieldType == oldName)
                            {
                                bHasStructField = true;
                                oFieldInfo.FieldType = oStructInfo.StructName;
                            }
                        }

                        for (int j = 0; j < oProtocolInfo.ResFields.Count; j++)
                        {
                            FieldInfo oFieldInfo = oProtocolInfo.ReqFields[j];
                            if (oFieldInfo.FieldType == oldName)
                            {
                                bHasStructField = true;
                                oFieldInfo.FieldType = oStructInfo.StructName;
                            }
                        }

                        if (bHasStructField)
                        {
                            ProtocolUtil.UpdateProtocol(oProtocolInfo);
                        }
                    }
                }

                Global.StructOperateVer = oStructInfo.StructID;
            }

            return bResult;
        }

        public static bool DelStruct(StructInfo oStructInfo)
        {
            if (Global.SelectedProject == null || oStructInfo == null) return false;

            oStructInfo.OperateType = 3;
            bool bResult = MySQLUtil.AddStruct(Global.SelectedProject, oStructInfo) != 0;
            if (bResult)
            {
                Global.SelectedProject.Structs.Remove(oStructInfo);
                Global.FieldTypeList.Remove(oStructInfo.StructName);
                Global.StructOperateVer = oStructInfo.StructID;
            }
            else
            {
                MessageBox.Show(string.Format("删除结构【{0}】失败!", oStructInfo.StructName));
            }

            return bResult;
        }

        public static bool DelStructImp(StructInfo oStructInfo)
        {
            if (Global.SelectedProject == null || oStructInfo == null) return false;

            bool bResult = MySQLUtil.DelStruct(oStructInfo);
            if (bResult)
            {
                Global.SelectedProject.Structs.Remove(oStructInfo);
                Global.FieldTypeList.Remove(oStructInfo.StructName);
            }
            else
            {
                MessageBox.Show(string.Format("删除结构【{0}】失败!", oStructInfo.StructName));
            }

            return bResult;
        }

        public static void LoadStructFields(StructInfo oStructInfo)
        {
            if (oStructInfo == null) return;

            oStructInfo.Fields.Clear();

            MySQLUtil.LoadStructFields(oStructInfo);
        }

        public static bool CheckStructLocker(StructInfo oStructInfo)
        {
            if (oStructInfo == null) return false;

            string name = MySQLUtil.GetStructLocker(oStructInfo);

            if (string.IsNullOrEmpty(name) == false)
            {
                if (MessageBox.Show(string.Format("【{0}】结构被 {1} 锁定，是否强制锁定？", oStructInfo.StructName, name), "锁定", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {//强制锁定时返回未被锁定状态
                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool UpdateStructLocker(StructInfo oStructInfo, string sName)
        {
            if (oStructInfo == null) return false;

            return MySQLUtil.UpdateStructLocker(oStructInfo, sName);
        }

        public static void ClearStructLocker(StructInfo oStructInfo, string sName)
        {
            if (oStructInfo == null) return;

            MySQLUtil.ClearStructLocker(oStructInfo, sName);
        }

        public static void ReloadStructList(ProjectInfo oProjectInfo, int nOperateVer)
        {
            if (oProjectInfo == null) return;
            oProjectInfo.Structs.Clear();

            List<StructInfo> list = new List<StructInfo>();
            MySQLUtil.ReloadStructList(oProjectInfo, list, nOperateVer);

            Global.FieldTypeList.Clear();
            Global.FieldTypeList.AddRange(Global.BasicTypeArray);

            for (int i = 0; i < list.Count; i++)
            {
                oProjectInfo.Structs.Add(list[i]);
                Global.FieldTypeList.Add(list[i].StructName);
            }
        }

        public static bool ProtocolExists(string sProtocolName)
        {
            if (Global.SelectedProject == null) throw new Exception("未选择项目!");

            bool bResult = MySQLUtil.ProtocolExists(Global.SelectedProject.ProjectID, sProtocolName);

            if (bResult)
            {
                MessageBox.Show(string.Format("协议【{0}】已存在!", sProtocolName));
            }

            return bResult;
        }

        public static bool ProtocolCodeExists(int nProtocolCode)
        {
            if (Global.SelectedProject == null) throw new Exception("未选择项目!");

            bool bResult = MySQLUtil.ProtocolCodeExists(Global.SelectedProject.ProjectID, nProtocolCode);

            if (bResult)
            {
                MessageBox.Show(string.Format("协议号【{0}】已存在!", nProtocolCode));
            }

            return bResult;
        }

        public static bool AddProtocol(ProtocolInfo oProtocolInfo)
        {
            if (Global.SelectedProject == null || oProtocolInfo == null) return false;

            oProtocolInfo.OperateType = 1;
            oProtocolInfo.SortIndex = MySQLUtil.GetMaxProtocolSortIndex(Global.SelectedProject.ProjectID) + 1;
            long id = MySQLUtil.AddProtocol(Global.SelectedProject, oProtocolInfo);

            if (id == 0)
            {
                MessageBox.Show("新建协议【{0}】失败!", oProtocolInfo.ProtocolName);
                return false;
            }
            else
            {
                Global.ProtocolOperateVer = oProtocolInfo.ProtocolID;
            }

            Global.SelectedProject.Protocols.Add(oProtocolInfo);
            return true;
        }

        public static bool UpdateProtocol(ProtocolInfo oProtocolInfo)
        {
            if (Global.SelectedProject == null || oProtocolInfo == null) return false;

            oProtocolInfo.OperateType = 2;
            bool bResult = MySQLUtil.AddProtocol(Global.SelectedProject, oProtocolInfo) != 0;

            if (bResult == false)
            {
                MessageBox.Show(string.Format("更新协议【{0}】失败!", oProtocolInfo.ProtocolName));
            }
            else
            {
                Global.ProtocolOperateVer = oProtocolInfo.ProtocolID;
            }
        
            return bResult;
        }
        
        public static bool DelProtocol(ProtocolInfo oProtocolInfo)
        {
            if (Global.SelectedProject == null || oProtocolInfo == null) return false;

            oProtocolInfo.OperateType = 3;
            bool bResult = MySQLUtil.AddProtocol(Global.SelectedProject, oProtocolInfo) != 0;
        
            if (bResult)
            {
                Global.SelectedProject.Protocols.Remove(oProtocolInfo);
                Global.ProtocolOperateVer = oProtocolInfo.ProtocolID;
            }
            else
            {
                MessageBox.Show(string.Format("删除协议【{0}】失败!", oProtocolInfo.ProtocolName));
            }
            return bResult;
        }

        public static bool DelProtocolImp(ProtocolInfo oProtocolInfo)
        {
            if (Global.SelectedProject == null || oProtocolInfo == null) return false;

            bool bResult = MySQLUtil.DelProtocol(oProtocolInfo);

            if (bResult)
            {
                Global.SelectedProject.Protocols.Remove(oProtocolInfo);
            }
            else
            {
                MessageBox.Show(string.Format("删除协议【{0}】失败!", oProtocolInfo.ProtocolName));
            }
            return bResult;
        }
        
        public static void LoadProtocolFields(ProtocolInfo oProtocolInfo)
        {
            if (oProtocolInfo == null) return;

            oProtocolInfo.ReqFields.Clear();
            oProtocolInfo.ResFields.Clear();

            MySQLUtil.LoadProtocolFields(oProtocolInfo);
        }

        public static bool CheckProtocolLocker(ProtocolInfo oProtocolInfo)
        {
            if (oProtocolInfo == null) return false;

            string name = MySQLUtil.GetProtocolLocker(oProtocolInfo);

            if (string.IsNullOrEmpty(name) == false)
            {
                if (MessageBox.Show(string.Format("【{0}】协议被 {1} 锁定，是否强制锁定？", oProtocolInfo.ProtocolName, name), "锁定", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {//强制锁定时返回未被锁定状态
                    return false;
                }

                return true;
            }

            return false;
        }

        public static bool UpdateProtocolLocker(ProtocolInfo oProtocolInfo, string sName)
        {
            if (oProtocolInfo == null) return false;

            return MySQLUtil.UpdateProtocolLocker(oProtocolInfo, sName);
        }

        public static void ClearProtocolLocker(ProtocolInfo oProtocolInfo, string sName)
        {
            if (oProtocolInfo == null) return;

            MySQLUtil.ClearProtocolLocker(oProtocolInfo, sName);
        }

        public static void ReloadProtocolList(ProjectInfo oProjectInfo, int nOperateVer)
        {
            if (oProjectInfo == null) return;

            oProjectInfo.Protocols.Clear();

            List<ProtocolInfo> list = new List<ProtocolInfo>();
            MySQLUtil.ReloadProtocolList(oProjectInfo, list, nOperateVer);

            for (int i = 0; i < list.Count; i++)
            {
                oProjectInfo.Protocols.Add(list[i]);
            }
        }

        public static bool CheckGeneratorSetting(string type)
        {
            bool bResult = MySQLUtil.CheckGeneratorSetting(Global.SelectedProject, type);

            if (bResult)
            {
                MessageBox.Show(string.Format("{0}的生成模板已存在!", type));
            }

            return bResult;
        }

        public static bool AddGeneratorSetting(GeneratorSetting oGeneratorSetting)
        {
            if (oGeneratorSetting == null) return false;

            long id = MySQLUtil.AddGeneratorSetting(oGeneratorSetting);

            if (id == 0)
            {
                MessageBox.Show("新建生成模板失败!");
            }

            return id != 0;
        }

        public static bool UpdateGeneratorSetting(GeneratorSetting oGeneratorSetting)
        {
            if (oGeneratorSetting == null) return false;

            bool bResult = MySQLUtil.UpdateGeneratorSetting(oGeneratorSetting);

            if (bResult == false)
            {
                MessageBox.Show("更新生成模板失败!");
            }

            return bResult;
        }

        public static bool DelGeneratorSetting(GeneratorSetting oGeneratorSetting)
        {
            if (oGeneratorSetting == null) return false;

            bool bResult = MySQLUtil.DelGeneratorSetting(oGeneratorSetting);

            if (bResult == false)
            {
                MessageBox.Show("删除生成模板失败!");
            }

            return bResult;
        }

        public static GeneratorSetting LoadGeneratorSetting(string sGeneratorType)
        {
            return MySQLUtil.LoadGeneratorSetting(Global.SelectedProject, sGeneratorType);
        }

        public static void LoadGeneratorSetting(BindingList<GeneratorSetting> list)
        {
            List<GeneratorSetting> temp = MySQLUtil.LoadGeneratorSetting(Global.SelectedProject);
            list.Clear();
            foreach (var item in temp)
            {
                IGenerator iIGenerator = GeneratorMgr.GetGenerator(item.GeneratorType);
                if (iIGenerator != null)
                {
                    item.GeneratorName = iIGenerator.Name;
                    list.Add(item);
                }
            }
        }
    }
}
